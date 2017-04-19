using books.business_logic.common;
using books.business_logic.models;
using books.business_logic.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic {
  //----------------------------------------------------------------------------
  public class Relations {
    public delegate void GetRelationWordPairsCallback(
      List<Tuple<Word, Word>> words,
      bool status,
      string message);

    public delegate void GetRelationsCallback(List<Relation> relations,
                                              bool status,
                                              string message);

    public delegate void AddRelationCallback(
      Relation relation, bool status, string message);

    public delegate void AddRelationWordsCallback(
      Tuple<Word, Word> wordPair, bool status, string message);

    public delegate void RemoveRelationCallback(bool status, string message);

    public delegate void RemoveRelationWordPairCallback(bool status, string message);


    //--------------------------------------------------------------------------
    private ThreadWorkerQueue _workerThread = null;
    IStatsUpdates _statsUpdate = null;

    //--------------------------------------------------------------------------
    public Relations(ThreadWorkerQueue workerThread, IStatsUpdates statsUpdate) {
      this._workerThread = workerThread;
      this._statsUpdate = statsUpdate;
    }

    //--------------------------------------------------------------------------
    public bool GetAll(GetRelationsCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          List<Relation> result = RelationsService.Instance.GetAll();
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool GetWordPairs(Relation relation, 
                             GetRelationWordPairsCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          List<Tuple<Word, Word>> result =
            RelationsService.Instance.GetWords(relation);
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool AddRelation(string relationName, AddRelationCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = false;
        Relation relationResult = null;

        try {
          relationResult = RelationsService.Instance.AddRelation(relationName);
          status = true;
          _statsUpdate.UpdateRelations();
        } catch (Exception e) {
          message = e.Message;
        }

        if (callback != null) {
          callback(relationResult, status, message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool RemoveRelation(Relation relation, 
                               RemoveRelationCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = false;

        try {
          status = RelationsService.Instance.RemoveRelation(relation);
          //_statsUpdate.UpdateGroups();
        } catch (Exception e) {
          message = e.Message;
        }

        if (callback != null) {
          callback(status, message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool AddRelationWords(Relation relation,
                                 string firstWord,
                                 string secondWord,
                                 AddRelationWordsCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = false;
        Tuple<Word, Word> result = new Tuple<Word, Word>(null, null);

        try {
          result = RelationsService.Instance.AddRelationWordPair(relation, 
                                                                 firstWord, 
                                                                 secondWord);
          _statsUpdate.UpdateWords();
          status = true;
        } catch (Exception e) {
          message = e.Message;
        }

        if (callback != null) {
          callback(result, status, message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool RemoveRelationWordPair(Relation relation, 
                                       Tuple<Word, Word> wordPair,
                                       RemoveRelationWordPairCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = false;

        try {
          RelationsService.Instance.RemoveRelationWordPair(relation, wordPair);
          status = true;
        } catch (Exception e) {
          message = e.Message;
        }

        if (callback != null) {
          callback(status, message);
        }
      });
    }

    //--------------------------------------------------------------------------
    /*public bool RemoveGroupWord(Group group,
                                Word word,
                                RemoveGroupWordCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = false;

        try {
          GroupsService.Instance.RemoveGroupWord(group, word);
          status = true;
        } catch (Exception e) {
          message = e.Message;
        }

        if (callback != null) {
          callback(status, message);
        }
      });
    }*/

  }
}
