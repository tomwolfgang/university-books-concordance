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
  class RelationsService {
    private static RelationsService _instance = new RelationsService();
    private Dictionary<long, Relation> _relationsCache = 
      new Dictionary<long, Relation>();

    //--------------------------------------------------------------------------
    /// <summary>
    /// </summary>
    private RelationsService() {
    }

    //--------------------------------------------------------------------------
    static RelationsService() {
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// Get singelton instance
    /// </summary>
    public static RelationsService Instance {
      get {
        return _instance;
      }
    }

    //--------------------------------------------------------------------------
    public Int64 GetCount() {
      return RelationsDao.GetCount();
    }

    //--------------------------------------------------------------------------
    public Relation AddRelation(string relationName) {
      Relation resultRelation = new Relation() {
        Name = relationName
      };

      RelationsDao.Insert(ref resultRelation);
      return resultRelation;
    }

    //--------------------------------------------------------------------------
    public bool RemoveRelation(Relation relation) {
      RelationsDao.DeleteAllRelationsWords(relation);
      return RelationsDao.Delete(relation);
    }

    //--------------------------------------------------------------------------
    public List<Relation> GetAll() {
      return RelationsDao.GetAll();
    }

    //--------------------------------------------------------------------------
    public void CreateTable() {
      RelationsDao.CreateTable();
    }

    //--------------------------------------------------------------------------
    public List<Relation> Query(string name) {
      //return GroupsDao.Query(name);
      return null;
    }

    //--------------------------------------------------------------------------
    public void DropTable() {
      RelationsDao.DropTable();
      _relationsCache.Clear();
    }

    //--------------------------------------------------------------------------
    public Relation GetById(long relationId) {
      if (_relationsCache.ContainsKey(relationId)) {
        return _relationsCache[relationId];
      }

      Relation relation = RelationsDao.GetRelationById(relationId);
      _relationsCache[relation.Id] = relation;
      return relation;
    }

    //--------------------------------------------------------------------------
    public List<Tuple<Word, Word>> GetWords(Relation relation) {
      // TODO: we can change to GetWords and have a join query
      List<Tuple<long, long>> wordIdPairList = 
        RelationsDao.GetWordIdPairs(relation);

      List<Tuple<Word, Word>> wordsPairList = new List<Tuple<Word,Word>>();
      foreach (Tuple<long, long> wordIdPair in wordIdPairList) {
        Tuple<Word, Word> wordsPair = new Tuple<Word, Word>(
          WordsService.Instance.GetWordById(wordIdPair.Item1),
          WordsService.Instance.GetWordById(wordIdPair.Item2)
        );
        
        wordsPairList.Add(wordsPair);
      }

      return wordsPairList;
    }

    //--------------------------------------------------------------------------
    public Tuple<Word, Word> AddRelationWordPair(Relation relation, 
                                                 string firstWord, 
                                                 string secondWord) {
      Word word1 = WordsService.Instance.GetWord(firstWord);
      Word word2 = WordsService.Instance.GetWord(secondWord);

      RelationsDao.AddRelationWords(relation, word1, word2);

      return new Tuple<Word, Word>(word1, word2);
    }

    //--------------------------------------------------------------------------
    public void RemoveRelationWordPair(Relation relation, 
                                       Tuple<Word, Word> wordPair) {
      RelationsDao.DeleteRelationWords(relation, 
                                       wordPair.Item1, 
                                       wordPair.Item2);
    }

    //--------------------------------------------------------------------------
    public void Import(XmlDocument document) {
      GlobalParamatersService.Delegate.OnDatabaseImportProgress(0);

      DatabaseConnectionService.Instance.SafeTransaction(_ => {
        XmlNodeList xmlRelationList =
          document.DocumentElement.SelectNodes(".//relation");
        XmlNodeList xmlRelationWordList =
          document.DocumentElement.SelectNodes(".//relations_words");

        int total = xmlRelationList.Count + xmlRelationWordList.Count;
        int processed = 0;

        foreach (XmlNode xmlRelation in xmlRelationList) {
          Relation relation = RelationsDao.ImportRelation(xmlRelation);
          _relationsCache[relation.Id] = relation;

          processed++;
          float percent = (float)processed / (float)total;
          percent *= 100;
          GlobalParamatersService.Delegate.OnDatabaseImportProgress(
            (int)percent);
        }

        foreach (XmlNode xmlRelationWord in xmlRelationWordList) {
          RelationsDao.ImportRelationWord(xmlRelationWord);

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
