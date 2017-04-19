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
  public class Groups {
    public delegate void GetGroupsCallback(List<Group> groups,
                                           bool status,
                                           string message);

    public delegate void GetGroupWordsCallback(List<Word> words,
                                               bool status,
                                               string message);

    public delegate void AddGroupCallback(
      Group group, bool status, string message);

    public delegate void AddGroupWordCallback(
      Word word, bool status, string message);

    public delegate void RemoveGroupCallback(bool status, string message);

    public delegate void RemoveGroupWordCallback(bool status, string message);


    //--------------------------------------------------------------------------
    private ThreadWorkerQueue _workerThread = null;
    IStatsUpdates _statsUpdate = null;

    //--------------------------------------------------------------------------
    public Groups(ThreadWorkerQueue workerThread, IStatsUpdates statsUpdate) {
      this._workerThread = workerThread;
      this._statsUpdate = statsUpdate;
    }

    //--------------------------------------------------------------------------
    public bool GetAll(GetGroupsCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          List<Group> result = GroupsService.Instance.GetAll();
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool GetWords(Group group, GetGroupWordsCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          List<Word> result =
            GroupsService.Instance.GetWords(new List<Group>() { group });
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool AddGroup(string group, AddGroupCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = false;
        Group groupResult = null;

        try {
          groupResult = GroupsService.Instance.AddGroup(group);
          status = true;
          _statsUpdate.UpdateGroups();
        } catch (Exception e) {
          message = e.Message;
        }

        if (callback != null) {
          callback(groupResult, status, message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool RemoveGroup(Group group, RemoveGroupCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = false;

        try {
          status = GroupsService.Instance.RemoveGroup(group);
          _statsUpdate.UpdateGroups();
        } catch (Exception e) {
          message = e.Message;
        }

        if (callback != null) {
          callback(status, message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool AddGroupWord(Group group, 
                             string word, 
                             AddGroupWordCallback callback) {
      return _workerThread.PostTask(_ => {
        string message = null;
        bool status = false;
        Word result = null;

        try {
          result = GroupsService.Instance.AddGroupWord(group, word);
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
    public bool RemoveGroupWord(Group group,
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
    }

  }
}
