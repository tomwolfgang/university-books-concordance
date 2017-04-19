using books.business_logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.services {
  //---------------------------------------------------------------------------
  class DocumentsLoader {
    private string[] _documents = null;
    private Dictionary<string, string> _documentLoadStatus = 
      new Dictionary<string, string>();
    private BusinessLogic _businessLogic = null;

    private int _currentDocument = 0;

    //-------------------------------------------------------------------------
    public bool HasDocumentToProcess {
      get {
        return _currentDocument < _documents.Length;
      }
    }

    //-------------------------------------------------------------------------
    // used at the end to get a report of what happened
    public Dictionary<string, string> DocumentsStatus {
      get {
        return _documentLoadStatus;
      }
    }

    //-------------------------------------------------------------------------
    public DocumentsLoader(string[] documents, BusinessLogic businessLogic) {
      _documents = documents;
      _businessLogic = businessLogic;
    }

    //-------------------------------------------------------------------------
    public bool Load(BusinessLogic _businessLogic) {
      _currentDocument = 0;

      for (int i = 0; i < _documents.Length; ++i) {
        if (!_businessLogic.LoadDocument(new FileInfo(_documents[i]))) {
          _documentLoadStatus[_documents[i]] = "Failed calling LoadDocument?!";
        }        
      }

      return true;
    }

    //-------------------------------------------------------------------------
    public string GetDescription() {
      return String.Format(
        "Loading document {0}/{1} ...",
        _currentDocument + 1,
        _documents.Length);
    }

    //-------------------------------------------------------------------------
    public void OnLoadDocumentComplete(
      FileInfo documentFile, bool status, string message) {
      _documentLoadStatus[documentFile.FullName] = message;
      _currentDocument++;
    }
  }
}
