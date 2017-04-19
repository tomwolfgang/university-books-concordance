using books.business_logic.data_access_layer;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace books.business_logic.services {
  class WordsService {
    #region Member variables

    private static WordsService _instance = new WordsService();

    // holds a snapshot of all the words in the DB
    private List<Word> _words = null;

    // an index that maps word's value to the word object
    private Dictionary<string, Word> _wordIndex = 
      new Dictionary<string, Word>();
    
    // an index that maps the word's id (id field in the DB) to the word object
    private Dictionary<long, Word> _wordIdIndex = new Dictionary<long, Word>();

    #endregion

    /// <summary>
    /// </summary>
    private WordsService() {
    }

    static WordsService() {
    }

     /// <summary>
    /// Get singelton instance
    /// </summary>
    public static WordsService Instance {
      get {
        return _instance;
      }
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// Should only be called once.
    /// 
    /// Loads all the existing words from the DB and puts them into in-memory
    /// indices for optimized performance
    /// </summary>
    public bool Initialize() {
      // clear data structures
      if (_words != null) {
        _words.Clear();
      }

      _wordIndex.Clear();
      _wordIdIndex.Clear();

      try {
        WordsDao.GetAll(out _words);

        foreach (Word word in _words) {
          IndexWord(word);
        }
      } catch (Exception e) {
        throw e;
      }

      return true;
    }

    //--------------------------------------------------------------------------
    public void CreateTable() {
      WordsDao.CreateTable();
    }

    //--------------------------------------------------------------------------
    public Int64 GetCount() {
      //return WordsDao.GetCount();
      return _words.Count;
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// get a DB Word element for a given string
    /// If the word doesn't exist in-memory, we assume it doesn't exist in the 
    /// DB and so we try to add it - if that fails, we try to query it - 
    /// otherwise we throw an exception
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Word GetWord(string value) {
      // we only want to store lower case words so that our DB dosn't explode
      // of word permutations
      value = value.ToLower();

      if (_wordIndex.ContainsKey(value)) {
        return _wordIndex[value];
      }

      // doesn't exist - so we need to add it
      Word word = WordsDao.Insert(value);
      _words.Add(word);
      IndexWord(word);

      return word;
    }

    //--------------------------------------------------------------------------
    public void DropTable() {
      WordsDao.DropTable();
      _wordIdIndex.Clear();
      _wordIndex.Clear();
    }

    //--------------------------------------------------------------------------
    public Word GetWordById(long wordId) {
      if (!_wordIdIndex.ContainsKey(wordId)) {
        return null;
      }

      return _wordIdIndex[wordId];
    }

    //-------------------------------------------------------------------------
    public List<Word> Query(
      List<Document> documents, List<WordLocationProperty> properties) {

      // if we want all words - just return it from memory
      if ((documents.Count == 0) && (properties.Count == 0)) {
        return _words;
      }

      // since we already cache all words in memory - we can leverage this
      // so that the queries run faster
      // TODO: we may want to change to QueryWords so that the query is a join
      List<long> wordIds = 
        ContainsService.Instance.QueryWordIds(documents, properties);

      List<Word> result = new List<Word>();
      foreach (long wordId in wordIds) {
        result.Add(_wordIdIndex[wordId]);
      }

      return result;
    }

    //-------------------------------------------------------------------------
    public void Import(XmlDocument document) {
      _wordIndex.Clear();
      _wordIdIndex.Clear();

      GlobalParamatersService.Delegate.OnDatabaseImportProgress(0);

      DatabaseConnectionService.Instance.SafeTransaction(_ => {
        XmlNodeList xmlWords = document.DocumentElement.SelectNodes(".//word");

        int total = xmlWords.Count;
        int processed = 0;

        foreach (XmlNode xmlWord in xmlWords) {
          Word word = WordsDao.Import(xmlWord);
          IndexWord(word);

          processed++;
          float percent = (float)processed / (float)total;
          percent *= 100;
          GlobalParamatersService.Delegate.OnDatabaseImportProgress(
            (int)percent);
        }
      });
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <param name="word"></param>
    private void IndexWord(Word word) {
      _wordIndex[word.Value.ToLower()] = word;
      _wordIdIndex[word.Id] = word;
    }
  }
}
