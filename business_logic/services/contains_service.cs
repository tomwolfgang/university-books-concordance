using books.business_logic.data_access_layer;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using document_parser.models;
using System.Xml;

namespace books.business_logic.services {
  class ContainsService  {
    #region Member variables

    private static ContainsService _instance = new ContainsService();

    #endregion

    /// <summary>
    /// </summary>
    private ContainsService() {
    }

    static ContainsService() {
    }

     /// <summary>
    /// Get singelton instance
    /// </summary>
    public static ContainsService Instance {
      get {
        return _instance;
      }
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// Should only be called once.
    /// </summary>
    public bool Initialize() {
      return true;
    }

    //--------------------------------------------------------------------------
    public Int64 GetCount() {
      return ContainsDao.GetCount();
    }

    //--------------------------------------------------------------------------
    public Int64 GetUniqueWordsCount() {
      return ContainsDao.GetUniqueWordsCount();
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="word"></param>
    /// <param name="documentWord"></param>
    public void Insert(Document doc, Word word, DocumentWord documentWord) {
      Contains contains = new Contains() {
        DocumentId = doc.Id,
        WordId = word.Id,
        Line = documentWord.Line,
        FileOffset = documentWord.OffsetInFile,
        IndexInSentence = documentWord.IndexInSentence,
        Sentence = documentWord.Sentence,
        Paragraph = documentWord.Paragraph,
        Page = documentWord.Page
      };

      ContainsDao.Insert(contains);
    }

    //--------------------------------------------------------------------------
    public void CreateTable() {
      ContainsDao.CreateTable();
    }

    //-------------------------------------------------------------------------
    public List<long> QueryWordIds(
      List<Document> documents, List<WordLocationProperty> properties) {
      return ContainsDao.QueryWordIds(documents, properties);
    }

    //--------------------------------------------------------------------------
    public void DropTable() {
      ContainsDao.DropTable();
    }

    //-------------------------------------------------------------------------
    public WordLocationDetails Query(List<Document> documents, Word word) {
      // query the DB
      List<Contains> contains = ContainsDao.Query(documents, word);

      WordLocationDetails locationDetails = new WordLocationDetails();
      locationDetails.Word = word;
      locationDetails.LocationDetails = new List<LocationDetail>();

      foreach (Contains item in contains) {
        locationDetails.LocationDetails.Add(new LocationDetail() {
          Document = DocumentsService.Instance.GetById(item.DocumentId),
          Location = item
        });
      }

      return locationDetails;
    }

    //-------------------------------------------------------------------------
    public List<WordLocationDetails> Query(
      List<Document> documents, 
      List<WordLocationProperty> wordLocationProperties) {

      if (wordLocationProperties.Count <= 0) {
        throw new Exception("You must use, at least, ONE word location filter");
      }

      // query the DB
      List<Contains> contains = ContainsDao.Query(documents, 
                                                  wordLocationProperties);

      // we want to return a grouped result (as-in grouped by words)
      // so we use a dictionary to collect all location details per word and 
      // then we can build a grouped result of List<WordLocationDetails>
      Dictionary<long, List<LocationDetail>> wordLocationDetailsMap = 
        new Dictionary<long, List<LocationDetail>>();

      foreach (Contains containsItem in contains) {
        if (!wordLocationDetailsMap.ContainsKey(containsItem.WordId)) {
          wordLocationDetailsMap[containsItem.WordId] = 
            new List<LocationDetail>();
        }

        wordLocationDetailsMap[containsItem.WordId].Add(new LocationDetail() {
          Document = DocumentsService.Instance.GetById(containsItem.DocumentId),
          Location = containsItem
        });
      }

      // now we can convert to List<WordLocationDetails>
      List<WordLocationDetails> result = new List<WordLocationDetails>();
      foreach (var item in wordLocationDetailsMap.Keys) {
        result.Add(new WordLocationDetails() {
          Word = WordsService.Instance.GetWordById(item),
          LocationDetails = wordLocationDetailsMap[item]
        });
      }

      return result;
    }

    //-------------------------------------------------------------------------
    public List<WordLocationDetails> Query(Group group) {
      // query the DB
      List<Contains> contains = ContainsDao.Query(group);

      // we want to return a grouped result (as-in grouped by words)
      // so we use a dictionary to collect all location details per word and 
      // then we can build a grouped result of List<WordLocationDetails>
      Dictionary<long, List<LocationDetail>> wordLocationDetailsMap =
        new Dictionary<long, List<LocationDetail>>();

      foreach (Contains containsItem in contains) {
        if (!wordLocationDetailsMap.ContainsKey(containsItem.WordId)) {
          wordLocationDetailsMap[containsItem.WordId] =
            new List<LocationDetail>();
        }

        wordLocationDetailsMap[containsItem.WordId].Add(new LocationDetail() {
          Document = DocumentsService.Instance.GetById(containsItem.DocumentId),
          Location = containsItem
        });
      }

      // now we can convert to List<WordLocationDetails>
      List<WordLocationDetails> result = new List<WordLocationDetails>();
      foreach (var item in wordLocationDetailsMap.Keys) {
        result.Add(new WordLocationDetails() {
          Word = WordsService.Instance.GetWordById(item),
          LocationDetails = wordLocationDetailsMap[item]
        });
      }

      return result;
    }

    //-------------------------------------------------------------------------
    public Tuple<long, long> GetSurroundingOffsets(Contains location, 
                                                   bool lines,
                                                   long delta) {
      if (lines) {
        return ContainsDao.GetSurroundingLinesOffsets(
          location, delta, delta);
      } else {
        return ContainsDao.GetSurroundingSentencesOffsets(
          location, delta, delta);
      }
    }

    //-------------------------------------------------------------------------
    public void Import(XmlDocument document) {
      GlobalParamatersService.Delegate.OnDatabaseImportProgress(0);

      DatabaseConnectionService.Instance.SafeTransaction(_ => {
        XmlNodeList xmlContainsList =
          document.DocumentElement.SelectNodes(".//contains");

        int total = xmlContainsList.Count;
        int processed = 0;

        foreach (XmlNode xmlContains in xmlContainsList) {
          Contains contains = ContainsDao.Import(xmlContains);

          processed++;
          float percent = (float)processed / (float)total;
          percent *= 100;
          GlobalParamatersService.Delegate.OnDatabaseImportProgress(
            (int)percent);
        }
      });
    }
  }
}
