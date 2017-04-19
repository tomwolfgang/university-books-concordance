using books.business_logic.models;
using document_parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic {
  //-------------------------------------------------------------------------
  /// <summary>
  /// A delegate (callback) interface to be implemented by consumers of the
  /// BusinessLogic class.
  /// </summary>
  public interface BusinessLogicDelegate {
    void OnInitializationComplete(bool status, string error);

    void OnStatsUpdate(Stats stats);

    void OnLoadDocumentComplete(
      FileInfo documentFile, bool status, string message);
    void OnLoadDocumentParsed(FileInfo documentFile);
    void OnLoadDocumentProcessing(
      FileInfo documentFile, int percent, ref bool cancel);

    void OnDatabaseImportStatusChanged(string status);
    void OnDatabaseImportProgress(int percent);
  }
}
