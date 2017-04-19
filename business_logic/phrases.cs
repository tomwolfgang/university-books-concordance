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
  public class Phrases {
    public delegate void GetPhrasesCallback(List<Phrase> phrases, 
                                            bool status, 
                                            string message);

    public delegate void AddCallback(Phrase phrase, 
                                     bool status, 
                                     string message);

    public delegate void RemoveCallback(bool status, string message);

    public delegate void QueryCallback(
      List<Tuple<LocationDetail, LocationDetail>> phrases, 
      bool status, 
      string message);

    //--------------------------------------------------------------------------
    private ThreadWorkerQueue _workerThread = null;
    IStatsUpdates _statsUpdate = null;

    //--------------------------------------------------------------------------
    public Phrases(ThreadWorkerQueue workerThread, IStatsUpdates statsUpdate) {
      this._workerThread = workerThread;
      this._statsUpdate = statsUpdate;
    }

    //--------------------------------------------------------------------------
    public bool GetAll(GetPhrasesCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          List<Phrase> result = PhrasesService.Instance.GetAll();
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool Add(string phrase, AddCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = false;
        Phrase result = null;

        try {
          result = PhrasesService.Instance.AddPhrase(phrase);
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
    public bool Remove(Phrase phrase, RemoveCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = true;

        try {
          PhrasesService.Instance.RemovePhrase(phrase);
        } catch (Exception e) {
          status = false;
          message = e.Message;
        }

        if (callback != null) {
          callback(status, message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool Query(Phrase phrase, QueryCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = true;
        List<Tuple<LocationDetail, LocationDetail>> result = null;

        try {
          result = PhrasesService.Instance.Query(phrase);
        } catch (Exception e) {
          status = false;
          message = e.Message;
        }

        if (callback != null) {
          callback(result, status, message);
        }
      });
    }
  }
}
