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
  public class ExportImport {
    public delegate void ExportImportCallback(bool status, string message);

    //--------------------------------------------------------------------------
    private ThreadWorkerQueue _workerThread = null;
    IStatsUpdates _statsUpdate = null;

    //--------------------------------------------------------------------------
    public ExportImport(ThreadWorkerQueue workerThread, 
                        IStatsUpdates statsUpdate) {
      this._workerThread = workerThread;
      this._statsUpdate = statsUpdate;
    }

    //--------------------------------------------------------------------------
    public bool Export(FileInfo filename, ExportImportCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          DatabaseConnectionService.Instance.ExportDatabase(filename);

          callback(true, null);
        } catch (Exception e) {
          callback(false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool Import(FileInfo filename, ExportImportCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
            "Dropping tables ...");
          DatabaseConnectionService.Instance.DropTables();

          GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
            "Creating new tables ...");
          DatabaseConnectionService.Instance.CreateTables();

          // zero them all
          _statsUpdate.CollectStats();

          DatabaseConnectionService.Instance.ImportDatabase(filename, 
                                                            _statsUpdate);

          callback(true, null);
        } catch (Exception e) {
          callback(false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool ResetDB(ExportImportCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
            "Dropping tables ...");
          try {
            DatabaseConnectionService.Instance.DropTables();
          } catch(Exception) {
          }

          GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
            "Creating tables ...");
          DatabaseConnectionService.Instance.CreateTables();

          _statsUpdate.CollectStats();
          callback(true, null);
        } catch (Exception e) {
          callback(false, e.Message);
        }
      });
    }

  }
}
