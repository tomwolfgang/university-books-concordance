using System;
using System.IO;
using document_parser.stanford;
using System.Collections.Generic;
using document_parser.models;
using System.Text;

namespace document_parser {
  public class DocumentParser {

    #region Member variables
    // this is how we calculate the current page
    private const uint kLinesPerPage = 100;

    // we need the original data for calculating lines
    private StanfordDocumentFacade _stanfordDocument = null;
    private MetaDataParser _metaDataParser = new MetaDataParser();
    private CurrentPositionCalculator _currentPosition = null;
    private QualifiedWords _qualifiedWords = new QualifiedWords();

    // used to implement GetNextWord
    private InternalSentence _currentSentence = new InternalSentence();

    // indicates if the document was created from a file (or phrase)
    private bool _fromFile = true;

    #endregion

    #region Properties
    
    
    public string GutenbergId {
      get; internal set;
    }

    public Dictionary<String, String> MetaData {
      get {
        return _metaDataParser.Fields;
      }
    }

    /// <summary>
    /// For debugging purposes - we can retrieve all the uknown suffixes
    /// of the document and improve our algorithm
    /// </summary>
    public HashSet<string> UnknownSuffixes {
      get {
        return _qualifiedWords.UnknownSuffixes;
      }
    }

    /// <summary>
    /// How many sentences are in the document
    /// </summary>
    public int SentencesCount {
      get {
        if (_stanfordDocument == null) {
          return 0;
        }

        return _stanfordDocument.SentencesCount;
      }
    }
    #endregion

    #region Public methods

    //--------------------------------------------------------------------------
    /// <summary>
    /// This is how you construct a new DocumentParser
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static DocumentParser FromFile(FileInfo file) {
      if (!file.Exists) {
        throw new Exception("The document file doesn't exist!");
      }

      // make sure CoreNLP is initialized
      if (!VerifyStanfordServiceIsInitialized()) {
        throw new Exception("Failed to initialize CoreNLP!");
      }

      StanfordDocumentFacade doc =
        StanfordPipelineService.Instance.Annotate(file);

      if (doc == null) {
        return null;
      }

      DocumentParser docParser = new DocumentParser(doc);

      if (!docParser.ParseMetaData()) {
        throw new Exception("Failed to get document's start mark!");
      }

      if (!docParser.CheckDocumentValidityAfterMetaData()) {
        throw new Exception(
          "The document is invalid - either no content or no end mark!");
      }

      if (!docParser.SetGutenbergId(file)) {
        throw new Exception("Failed setting Gutenberg Id!");
      }

      docParser._fromFile = true;

      return docParser;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phrase"></param>
    /// <returns></returns>
    public static DocumentParser FromPhrase(string phrase) {
      if (phrase.Length <= 0) {
        throw new Exception("The phrase is empty!");
      }

      // make sure CoreNLP is initialized
      if (!VerifyStanfordServiceIsInitialized()) {
        throw new Exception("Failed to initialize CoreNLP!");
      }

      StanfordDocumentFacade doc =
        StanfordPipelineService.Instance.Annotate(phrase);

      if (doc == null) {
        return null;
      }

      DocumentParser docParser = new DocumentParser(doc);
      if (!docParser.InitForParsing()) {
        throw new Exception("Empty phrase?!");
      }

      // #fromphrase
      docParser._fromFile = false;

      return docParser;
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// Extracts contents from a file on the local filesystem, efficiently, 
    /// by seeking to the exact location of the file
    /// </summary>
    /// <param name="localFile"></param>
    /// <param name="fileOffsetFrom"></param>
    /// <param name="fileOffsetTo"></param>
    /// <returns></returns>
    public static string GetContents(FileInfo localFile, 
                                     long fileOffsetFrom, 
                                     long fileOffsetTo) {
      if (!localFile.Exists) {
        throw new Exception(
          "the document file doesn't exist on the file storage");
      }

      string content = null;

      using (FileStream fs = File.Open(localFile.FullName, FileMode.Open)) {
        byte[] buffer = new byte[fileOffsetTo - fileOffsetFrom];
        fs.Seek(fileOffsetFrom, SeekOrigin.Begin);
        fs.Read(buffer, 0, buffer.Length);
        content = System.Text.UTF8Encoding.UTF8.GetString(buffer);
      }

      return content;
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// Convert a binary offset into a textual/contextual index.
    /// This is important in order to support utf8 encoded content where we only
    /// have the binary offset of words
    /// </summary>
    /// <param name="content"></param>
    /// <param name="binaryOffset"></param>
    /// <returns></returns>
    public static long IndexFromBinaryOffsetInContent(string content, 
                                                      long binaryOffset) {
      byte[] buffer = System.Text.UTF8Encoding.UTF8.GetBytes(content);
      string contentBefore = System.Text.UTF8Encoding.UTF8.GetString(
        buffer,
        0, (int)binaryOffset);

      // the idea is to take the text that appears before our word, in bytes,
      // and see how many utf8 encoded characters we have in this text, these 
      // characters are "absorbed" into unicode characters of the decoded text
      // and so, we can use the delta (of the utf8 text to decoded text) to 
      // convert the binaryOffset to the unicode offset
      long delta = binaryOffset - contentBefore.Length;
      long index = binaryOffset - delta;

      return index;
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool GetNextWord(out DocumentWord word) {
      word = new DocumentWord();

      if (_stanfordDocument == null) {
        throw new Exception("no document parsed");
      }

       // iterate sentences until we reach a qualified word, or end of the
      // document (valid end is when we find an end mark)
      while (NextSentenceCheck()) {
        // did we reach the end of the document?
        if (CheckIfEndMark(_currentSentence)) {
          return false;
        }

        // get next qualified word
        int i = (int)_currentPosition.InternalWord;
        for (; i < _currentSentence.words.Count; ++i) {
          InternalWord currentWord = _currentSentence.words[i];
          _currentPosition.Calculate(currentWord);

          if (_qualifiedWords.Check(_currentSentence, currentWord, i)) {
            _currentPosition.FillWord(ref word, currentWord);
            _currentPosition.IncreaseWord(true); // next word index
            return true;
          }

          _currentPosition.IncreaseWord(false);
        }
      }

      // if we are in a phrase, we just stop at the end of the phrase
      if (!_fromFile) {
        return false;
      }

      // if we've reached here it means we've reached the end of the document
      // without a valid end mark... so we throw an exception
      throw new Exception("Unexpected end of document reached!");
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// save file to disk
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="overrideIfExists"></param>
    public void Save(FileInfo fileInfo, bool overrideIfExists) {
      if (fileInfo.Exists) {
        fileInfo.Delete();
      }

      if (!fileInfo.Directory.Exists) {
        fileInfo.Directory.Create();
      }

      System.IO.File.WriteAllText(
        fileInfo.FullName, _stanfordDocument.RawData);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// protect constructor - create an object with the static functions
    /// </summary>
    /// <param name="document"></param>
    private DocumentParser(StanfordDocumentFacade document) {
      _currentPosition = new CurrentPositionCalculator(
        document.RawData, kLinesPerPage);
      _stanfordDocument = document;
    }

    /// <summary>
    /// lazy initialization of CoreNLP (stanford)
    /// </summary>
    /// <returns></returns>
    private static bool VerifyStanfordServiceIsInitialized() {
      if (StanfordPipelineService.Instance.Initialized) {
        return true;
      }

      return StanfordPipelineService.Instance.Initialize();
    }

    /// <summary>
    /// Called when parsing a phrase and not a gutenberg file
    /// </summary>
    /// <returns></returns>
    private bool InitForParsing() {
      _currentSentence = new InternalSentence();

      if (!_stanfordDocument.GetSentence(
        _currentPosition.Sentence, ref _currentSentence)) {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Parses document meta-data such as: Author, Title, Language...
    /// 
    /// The function iterates all lines until reaching the: *** START OF ... 
    /// - which is the "start mark"
    /// </summary>
    /// <returns></returns>
    private bool ParseMetaData() {
      _currentSentence = new InternalSentence();

      while (_currentPosition.Sentence < _stanfordDocument.SentencesCount) {
        if (!_stanfordDocument.GetSentence(
          _currentPosition.Sentence, ref _currentSentence)) {
          return false;
        }

        // even if we have reached the content start mark - update positions
        // so that when we parse words we keep track of the current positions
        foreach (var internalWord in _currentSentence.words) {
          _currentPosition.Calculate(internalWord);
        }

        // make sure we move to the next sentence in our counter so that we
        // read the next sentence on the next GetNextWord call
        _currentPosition.IncreaseSentence();

        // if we've reached the content start mark - stop parsing meta data
        if (CheckIfStartMark(_currentSentence)) {
          _currentPosition.ResetWordInSentence(); // to be sure this happens
          // found the start mark - read next sentence to _currentSentence
          return _stanfordDocument.GetSentence(
            _currentPosition.Sentence, ref _currentSentence);
        }

        // try extracting meta data from the sentence
        _metaDataParser.Parse(_currentSentence);
      }

      return false;
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// We check that the document has an End Mark after we already found the
    /// start mark (when parsing the Meta Data).
    /// we also check it has at least 1 sentence that isn't the end mark right
    /// after the start mark
    /// </summary>
    /// <returns></returns>
    private bool CheckDocumentValidityAfterMetaData() {
      uint sentenceIndex = _currentPosition.Sentence;
      bool foundContentSentence = false;

      InternalSentence sentence = new InternalSentence();

      while (sentenceIndex < _stanfordDocument.SentencesCount) {
        if (!_stanfordDocument.GetSentence(sentenceIndex++, ref sentence)) {
          return false;
        }

        // if we've reached the content start mark - stop parsing meta data
        if (CheckIfEndMark(sentence)) {
          return foundContentSentence;
        } else {
          // this means we have at least 1 sentence that isn't the end mark
          foundContentSentence = true;
        }
      }

      return false;
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// We try to get it from the documents Meta-Data as it is supposed to be
    /// parsed and set there by MetaDataParser.  If it doesn't exist in the
    /// meta-data - we'll set it to the filename of the document
    /// </summary>
    private bool SetGutenbergId(FileInfo filename) {
      if (this.MetaData.ContainsKey(MetaDataParser.kGutendbergId)) {
        this.GutenbergId = this.MetaData[MetaDataParser.kGutendbergId];
        return true;
      }

      // fallback to filename
      this.GutenbergId = filename.Name;
      return true;
    }

    /// <summary>
    /// Examples: 
    /// 1. *** START OF THIS PROJECT GUTENBERG EBOOK ALICE'S ADVENTURES IN WONDERLAND ***
    /// 2. ***START OF THE PROJECT GUTENBERG EBOOK THE DEFENCE OF LUCKNOW***
    /// 
    /// Most seem to be example 1 - so maybe "The Defence of Lucknow" was a typo...
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    private bool CheckIfStartMark(InternalSentence sentence) {
      // at least "***" + "START" + "OF" and ends with "***"
      if (sentence.words.Count < 4) {
        return false;
      }

      // first word should be ***
      if (!sentence.words[0].annotatedText.Equals("***")) {
        return false;
      }

      // the next two words should be: START OF
      if (!sentence.words[1].annotatedText.ToLower().Equals("start")) {
        return false;
      }

      if (!sentence.words[2].annotatedText.ToLower().Equals("of")) {
        return false;
      }

      // finally, the sentence should 
      if (!sentence.words[sentence.words.Count-1].annotatedText.Equals(
        "***")) {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Examples:
    /// 1. *** END OF THIS PROJECT GUTENBERG EBOOK PRIDE AND PREJUDICE ***
    /// 2. ***END OF THE PROJECT GUTENBERG EBOOK THE DEFENCE OF LUCKNOW***
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    private bool CheckIfEndMark(InternalSentence sentence) {
      // at least "***" + "END" + "OF" and ends with "***"
      if (sentence.words.Count < 4) {
        return false;
      }

      // first word should be ***
      if (!sentence.words[0].annotatedText.Equals("***")) {
        return false;
      }

      // the next two words should be: START OF
      if (!sentence.words[1].annotatedText.ToLower().Equals("end")) {
        return false;
      }

      if (!sentence.words[2].annotatedText.ToLower().Equals("of")) {
        return false;
      }

      // finally, the sentence should 
      if (!sentence.words[sentence.words.Count - 1].annotatedText.Equals(
        "***")) {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Check if we've reached the end of the current sentence and if so
    /// move to the next sentence (and first word of that sentence)
    /// 
    /// returns false if we can't continue - i.e. reached the end of the
    /// document (which shouldn't really happen because we check for the
    /// end of document mark...)
    /// 
    /// throws an exception if something goes wrong
    /// </summary>
    /// <returns>false - no more sentences to parse, true otherwise</returns>
    private bool NextSentenceCheck() {
      if (_currentPosition.Sentence >= _stanfordDocument.SentencesCount) {
        return false;
      }
      
      // did we reach the end of the sentence?
      if (_currentPosition.InternalWord < _currentSentence.words.Count) {
        return true;
      }

      // else... we've reached the next sentence
      _currentPosition.IncreaseSentence();
      _currentPosition.ResetWordInSentence();

      // check if we've reached the end of the document
      if (_currentPosition.Sentence >= _stanfordDocument.SentencesCount) {
        return false;
      }

      if (!_stanfordDocument.GetSentence(
        _currentPosition.Sentence, ref _currentSentence)) {
        throw new Exception("error when trying to retrieve next sentence");
      }

      return true;
    }

    #endregion
  }
}
