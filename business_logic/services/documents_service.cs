using books.business_logic.data_access_layer;
using books.business_logic.models;
using document_parser;
using document_parser.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace books.business_logic.services {
  //----------------------------------------------------------------------------
  class DocumentsService {
    private static DocumentsService _instance = new DocumentsService();
    private Dictionary<long, Document> _documentCache = 
      new Dictionary<long, Document>();

    //--------------------------------------------------------------------------
    /// <summary>
    /// </summary>
    private DocumentsService() {
    }

    //--------------------------------------------------------------------------
    static DocumentsService() {
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// Get singelton instance
    /// </summary>
    public static DocumentsService Instance {
      get {
        return _instance;
      }
    }

    //--------------------------------------------------------------------------
    public bool Initialize() {
      throw new NotImplementedException();
    }

    //--------------------------------------------------------------------------
    public Int64 GetCount() {
      return DocumentsDao.GetCount();
    }

    //--------------------------------------------------------------------------
    public bool Load(FileInfo file) {
      DocumentParser documentParser = DocumentParser.FromFile(file);

      GlobalParamatersService.Delegate.OnLoadDocumentParsed(file);

      // Insert the document into the DB
      Document document = InsertDocument(documentParser);

      // load document's contents into DB - this can be a very long task
      // due to the amounts of INSERT statements performed - therefore,
      // we wrap the call to |InsertWords| inside a transaction and basically
      // perform one big bulk.  This tremendously improves performance
      DatabaseConnectionService.Instance.SafeTransaction(_ => {
        InsertWords(file, documentParser, document);
        document.TableLoadState = Document.LoadState.Complete;
        DocumentsDao.Update(document);
      });

      GlobalParamatersService.Delegate.OnLoadDocumentComplete(
        file, true, null);
      return true;
    }

    //--------------------------------------------------------------------------
    public void CreateTable() {
      DocumentsDao.CreateTable();
    }

    //--------------------------------------------------------------------------
    public void DropTable() {
      DocumentsDao.DropTable();
      _documentCache.Clear();
    }

    //--------------------------------------------------------------------------
    public List<Document> QueryByWords(List<string> words) {
      return DocumentsDao.QueryByWords(words);
    }

    //--------------------------------------------------------------------------
    public List<Document>  Query(
      List<string> words,
      List<DocumentProperty> properties) {
      List<Document> documents = DocumentsDao.Query(words, properties);

      // NOTE: it seems to me to be more efficient if we query the entire
      // Document object from the Dao in this scenario - instaed of getting only
      // a list of IDs and then calling |GetById| on each
      foreach (Document document in documents) {
        _documentCache[document.Id] = document;
      }

      return documents;
    }

    //--------------------------------------------------------------------------
    public Document GetById(long documentId) {
      if (_documentCache.ContainsKey(documentId)) {
        return _documentCache[documentId];
      }

      Document document = DocumentsDao.GetDocumentById(documentId);
      _documentCache[document.Id] = document;
      return document;
    }

    //--------------------------------------------------------------------------
    public string GetContents(LocationDetail locationDetail, 
                              long fileOffsetFrom, 
                              long fileOffsetTo,
                              out long contentsWordOffsetBegin,
                              out long contentsWordOffsetEnd) {
      contentsWordOffsetBegin = 0;
      contentsWordOffsetEnd = 0;

      Document document = locationDetail.Document;

      string contents = DocumentParser.GetContents(document.LocalFile, 
                                                   fileOffsetFrom, 
                                                   fileOffsetTo);

      long binaryOffset = locationDetail.Location.FileOffset - fileOffsetFrom;
      contentsWordOffsetBegin = 
        DocumentParser.IndexFromBinaryOffsetInContent(contents, binaryOffset);

      Word word = WordsService.Instance.GetWordById(
        locationDetail.Location.WordId);
      if (word == null) {
        throw new Exception("Word not found in DB?!");
      }
      contentsWordOffsetEnd = contentsWordOffsetBegin + word.Length;

      return contents;
    }

    //--------------------------------------------------------------------------
    public string GetContents(Tuple<LocationDetail, LocationDetail> phrase,
                              long fileOffsetFrom,
                              long fileOffsetTo,
                              out long contentsPhraseOffsetBegin,
                              out long contentsPhraseOffsetEnd) {
      contentsPhraseOffsetBegin = 0;
      contentsPhraseOffsetEnd = 0;

      Document document = phrase.Item1.Document;

      string contents = DocumentParser.GetContents(document.LocalFile,
                                                   fileOffsetFrom,
                                                   fileOffsetTo);

      long binaryOffset = phrase.Item1.Location.FileOffset - fileOffsetFrom;
      contentsPhraseOffsetBegin =
        DocumentParser.IndexFromBinaryOffsetInContent(contents, binaryOffset);

      // calc the begining of the last word of the phrase
      binaryOffset = phrase.Item2.Location.FileOffset - fileOffsetFrom;
      contentsPhraseOffsetEnd =
        DocumentParser.IndexFromBinaryOffsetInContent(contents, binaryOffset);

      Word word = WordsService.Instance.GetWordById(
        phrase.Item2.Location.WordId);
      if (word == null) {
        throw new Exception("Word not found in DB?!");
      }

      contentsPhraseOffsetEnd = contentsPhraseOffsetEnd + word.Length;

      return contents;
    }

    //-------------------------------------------------------------------------
    private void ImportStorage(FileInfo importFilename) {
      DirectoryInfo importStorageFolder = new DirectoryInfo(
        Path.Combine(new string[] {
              importFilename.Directory.FullName,
              // remove extension
              importFilename.Name.Substring(0, importFilename.Name.Length - 4) +
              "_storage" }));

      if (!importStorageFolder.Exists) {
        throw new Exception(String.Format(
          "Storage folder {0) is missing!", importStorageFolder.FullName));
      }

      DirectoryInfo storageFolder = 
        GlobalParamatersService.Configuration.Storage;
      // delete existing
      if (storageFolder.Exists) {
        storageFolder.Delete(true);
      }

      // create the folder
      storageFolder.Create();

      // copy storage files
      FileInfo[] files = importStorageFolder.GetFiles();
      foreach (FileInfo file in files) {
        string temppath = Path.Combine(storageFolder.FullName, file.Name);
        file.CopyTo(temppath, false);
      }
    }

    //-------------------------------------------------------------------------
    public void Import(XmlDocument document, FileInfo importFilename) {
      ImportStorage(importFilename);

      _documentCache.Clear();

      GlobalParamatersService.Delegate.OnDatabaseImportProgress(0);

      DatabaseConnectionService.Instance.SafeTransaction(_ => {
        XmlNodeList xmlDocuments = 
          document.DocumentElement.SelectNodes(".//document");

        int total = xmlDocuments.Count;
        int processed = 0;

        foreach (XmlNode xmlDocument in xmlDocuments) {
          Document doc = DocumentsDao.Import(xmlDocument);
          _documentCache[doc.Id] = doc;

          processed++;
          float percent = (float)processed / (float)total;
          percent *= 100;
          GlobalParamatersService.Delegate.OnDatabaseImportProgress(
            (int)percent);
        }
      });
    }

    //--------------------------------------------------------------------------
    private Document InsertDocument(DocumentParser document) {
      // first, check if the document already exists
      Document doc = DocumentsDao.GetDocumentById(document.GutenbergId);
      if ((doc != null) && 
          (doc.TableLoadState == Document.LoadState.Complete)) {
        throw new Exception("document already exists!");
      }

      if (doc != null) {
        // if the document exists but the load state wasn't complete, it means
        // we probably failed to load it previously and so we will run a clean
        // up before trying to reload it
        CleanupDocument(doc);
        doc = null;
      }

      // check we have the minimal set of meta data
      doc = GenerateDocumentModel(document);
      DocumentsDao.Insert(doc);
      _documentCache[doc.Id] = doc;
      return doc;
    }

    //--------------------------------------------------------------------------
    private void CleanupDocument(Document doc) {
      if (doc == null) {
        return;
      }

      ContainsDao.DeleteByDocument(doc);

      _documentCache.Remove(doc.Id);
      DocumentsDao.Delete(doc);
    }

    //--------------------------------------------------------------------------
    // returns the filename
    private FileInfo SaveToStorage(DocumentParser document) {
      string filename = document.GutenbergId + ".txt";
      filename = Path.Combine(
        GlobalParamatersService.Configuration.Storage.FullName, filename);
      document.Save(new FileInfo(filename), true);
      return new FileInfo(filename);
    }

    //--------------------------------------------------------------------------
    private Document GenerateDocumentModel(DocumentParser document) {
      Document doc = new Document() {
        GutenbergId = document.GutenbergId
      };
    
      string value;

      if (!document.MetaData.TryGetValue("title", out value)) {
        throw new Exception("Document is missing required field: " + "title");
      }
      doc.Title = value;

      if (!document.MetaData.TryGetValue("author", out value)) {
        throw new Exception("Document is missing required field: " + "author");
      }
      doc.Author = value;
     
      if (document.MetaData.TryGetValue("release date", out value)) {
        DateTime output;
        if (DateTime.TryParse(value, out output)) {
          doc.ReleaseDate = output;
        }        
      }

      // save the normalized document to our storage folder
      doc.LocalFile = SaveToStorage(document);
      doc.TableLoadState = Document.LoadState.NotComplete;
      return doc;
    }

    //--------------------------------------------------------------------------
    // Inserts the document's words into the Words and Contains services
    private void InsertWords(
      FileInfo file, DocumentParser documentParser, Document doc) {
      bool cancelRequest = false;
      DocumentWord documentWord = null;

      // we will validate the integrity of our results by seeking each
      // word's position to the source file and asserting the word in the offset
      // of the file matches
      FileStream fsValidation = 
        new FileStream(doc.LocalFile.FullName, FileMode.Open);

      while (documentParser.GetNextWord(out documentWord) && !cancelRequest) {
        //performing this validation will make loading documents really slow.
        if (GlobalParamatersService.Configuration.PerformIntegrityValidations) {
          ValidateIntegrity(documentWord, fsValidation);
        }

        Word word = WordsService.Instance.GetWord(documentWord.Text);
        ContainsService.Instance.Insert(doc, word, documentWord);
        
        float percent = 
          (float)documentWord.Sentence / (float)documentParser.SentencesCount;
        percent *= 100;
        GlobalParamatersService.Delegate.OnLoadDocumentProcessing(
          file, (int)percent, ref cancelRequest);
      }

      if (cancelRequest) {
        throw new Exception("cancel was requested during document loading");
      }

      fsValidation.Close();

      // just making sure we get to 100% as we may have to skip invalid words
      // and not reach 100%
      GlobalParamatersService.Delegate.OnLoadDocumentProcessing(
        file, 100, ref cancelRequest);
    }

    //--------------------------------------------------------------------------
    private bool ValidateIntegrity(DocumentWord word, FileStream fsValidation) {
      fsValidation.Seek(word.OffsetInFile, SeekOrigin.Begin);

      int byteCount = UTF8Encoding.UTF8.GetByteCount(word.Text);
      byte[] buffer = new byte[byteCount];
      fsValidation.Read(buffer, 0, byteCount);
      string validationWord = UTF8Encoding.UTF8.GetString(buffer);

      if (validationWord.CompareTo(word.Text) != 0) {
        string description = String.Format(
          "Word: {0}, Line: {1}, Offset: {2}",
          word.Text,
          word.Line,
          word.OffsetInFile);

        fsValidation.Close();
        throw new Exception(
          "file parsing integrity validation failed:\n" + description);
      }

      return true;
    }

    //--------------------------------------------------------------------------
    public void ExportStorage(FileInfo filename) {
      DirectoryInfo storageFolder = new DirectoryInfo(
        Path.Combine(new string[] {
              filename.Directory.FullName,
              // remove extension
              filename.Name.Substring(0, filename.Name.Length - 4) +
              "_storage" }));

      // delete existing
      if (storageFolder.Exists) {
        storageFolder.Delete(true);
      }

      // create the folder
      storageFolder.Create();

      // copy storage files
      FileInfo[] files =
        GlobalParamatersService.Configuration.Storage.GetFiles();
      foreach (FileInfo file in files) {
        string temppath = Path.Combine(storageFolder.FullName, file.Name);
        file.CopyTo(temppath, false);
      }
    }

  }
}
