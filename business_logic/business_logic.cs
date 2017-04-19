using books.business_logic.common;
using books.business_logic.models;
using books.business_logic.services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic {
  //----------------------------------------------------------------------------
  /// <summary>
  /// This is the entry point into the business logic and also the only 
  /// interface to use.
  /// 
  /// It is composed of static functions that receive a callback as they
  /// all run on a separate thread from the callers.
  /// 
  /// The callbacks will be triggered on the separate thread.
  /// </summary>
  public class BusinessLogic : IStatsUpdates {
    private ThreadWorkerQueue _workerThread = 
      new ThreadWorkerQueue("Business Logic Worker");
    private Stats _stats = null;

    public ExportImport ExportImport {
      get; internal set;
    }

    public Queries Queries {
      get; internal set;
    }

    public Groups Groups {
      get; internal set;
    }

    public Relations Relations {
      get; internal set;
    }

    public Phrases Phrases {
      get; internal set;
    }

    public Statistics Stats {
      get; internal set;
    }

    //--------------------------------------------------------------------------
    public BusinessLogic(
      Configuration config, 
      BusinessLogicDelegate delegateImpl) {
      
      if ((config == null) || (delegateImpl == null)) {
        throw new ArgumentNullException();
      }

      // set global parameters shared by all services
      GlobalParamatersService.Configuration = config;
      GlobalParamatersService.Delegate = delegateImpl;

      this.ExportImport = new ExportImport(_workerThread, this);
      this.Queries = new Queries(_workerThread);
      this.Groups = new Groups(_workerThread, this);
      this.Relations = new Relations(_workerThread, this);
      this.Phrases = new Phrases(_workerThread, this);
      this.Stats = new Statistics(_workerThread);
    }

    //--------------------------------------------------------------------------
    public bool Initialize() {
      if (!_workerThread.Start()) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          DatabaseConnectionService.Instance.Initialize();
          try {
            WordsService.Instance.Initialize();
            CollectStats();
          } catch(Exception) {
            // ignore it because we might have an empty DB
          }

          GlobalParamatersService.Delegate.OnInitializationComplete(
            true, null);
        } catch (Exception e) {
          GlobalParamatersService.Delegate.OnInitializationComplete(
            false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    // Tries to load a new document into the DB
    // Triggers OnLoadDocumentXXX callbacks
    public bool LoadDocument(FileInfo localDocumentFile) {
      return _workerThread.PostTask(_ => {
        try {
          DocumentsService.Instance.Load(localDocumentFile);
          CollectStats();
        } catch (Exception e) {
          GlobalParamatersService.Delegate.OnLoadDocumentComplete(
            localDocumentFile, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public void CollectStats() { 
      _stats = new Stats() {
        Documents = DocumentsService.Instance.GetCount(),
        UniqueWords = ContainsService.Instance.GetUniqueWordsCount(),
        IndexedWords = ContainsService.Instance.GetCount(),
        Groups = GroupsService.Instance.GetCount(),
        Relations = RelationsService.Instance.GetCount()
      };

      GlobalParamatersService.Delegate.OnStatsUpdate(_stats);
    }

    //--------------------------------------------------------------------------
    public void UpdateWords() {
      if (_stats == null) {
        CollectStats();
        return;
      }

      _stats.UniqueWords = WordsService.Instance.GetCount();
      GlobalParamatersService.Delegate.OnStatsUpdate(_stats);
    }

    //--------------------------------------------------------------------------
    public void UpdateGroups() {
      if (_stats == null) {
        CollectStats();
        return;
      }

      _stats.Groups = GroupsService.Instance.GetCount();
      GlobalParamatersService.Delegate.OnStatsUpdate(_stats);
    }

    //--------------------------------------------------------------------------
    public void UpdateRelations() {
      if (_stats == null) {
        CollectStats();
        return;
      }

      _stats.Relations = RelationsService.Instance.GetCount();
      GlobalParamatersService.Delegate.OnStatsUpdate(_stats);
    }

  }
}
