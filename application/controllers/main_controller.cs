using books.business_logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using document_parser;
using System.IO;
using System.Configuration;
using books.business_logic.common;
using System.Windows.Forms;
using application.services;
using books.business_logic.models;

namespace application.controllers {
  class MainController : BusinessLogicDelegate {
    private MainDialog _parentForm = null;

    private BusinessLogic _businessLogic = null;
    private DocumentsLoader _documentsLoader = null;
    private LoadingScreenDialog _currentLoadingScreen = null;

    private readonly int kDefaultWordFrequenciesLimit = 20;
    private readonly int kDefaultLongQueriesTimeoutInSeconds = 300; // 5 min

    //--------------------------------------------------------------------------
    public MainController(MainDialog parentForm) {
      _parentForm = parentForm;
    }

    //--------------------------------------------------------------------------
    public void Initialize() {
      if (_businessLogic != null) {
        return;
      }


      Configuration config = new Configuration() {
#pragma warning disable CS0618
        ConnectionString = 
          ConfigurationSettings.AppSettings["connection_string"],
        Storage = new DirectoryInfo(
          ConfigurationSettings.AppSettings["storage_folder"]),
        ContentRetreivalResolution = InitContentRetreivalSettings(),
        PerformIntegrityValidations = ShouldPerformIntegirtyValidations(),
        Statistics = GetStatisticsConfigurations()
#pragma warning restore CS0618
      };

      _businessLogic = new BusinessLogic(config, this);
      if (!_businessLogic.Initialize()) {
        System.Windows.Forms.MessageBox.Show(
          "Failed to initialize database!", "Error");
        Application.Exit();
        return;
      }

      _currentLoadingScreen = new LoadingScreenDialog(
        "Initializing Database ...", false);
      _currentLoadingScreen.ShowDialog();
    }

    //--------------------------------------------------------------------------
    public void InsertDocuments() {
      OpenFileDialog documentFilesPicker = new OpenFileDialog();
      documentFilesPicker.Title = "Select document files";
      documentFilesPicker.Multiselect = true;     
      documentFilesPicker.Filter = "txt files (*.txt)|*.txt";
      documentFilesPicker.FilterIndex = 1;
      documentFilesPicker.RestoreDirectory = true;

      if (documentFilesPicker.ShowDialog() != DialogResult.OK) {
        return;
      }

      // start loading
      _documentsLoader = 
        new DocumentsLoader(documentFilesPicker.FileNames, _businessLogic);
      if (!_documentsLoader.HasDocumentToProcess) {
        return;
      }
      
      if (!_documentsLoader.Load(_businessLogic)) {
        System.Windows.Forms.MessageBox.Show(
          "Failed loading ALL documents!", "Error");
        return;
      }

      _currentLoadingScreen = new LoadingScreenDialog(
        "Loading documents ...", true);
      _currentLoadingScreen.ShowDialog();

      // show report
      ReportDialog report = new ReportDialog(_documentsLoader.DocumentsStatus);
      report.ShowDialog();
    }

    //--------------------------------------------------------------------------
    public void EditGroups() {
      EditGroupsDialog editGroups = new EditGroupsDialog(_businessLogic);
      editGroups.ShowDialog();
    }

    //--------------------------------------------------------------------------
    public void EditRelations() {
      EditRelationsDialog editRelations = 
        new EditRelationsDialog(_businessLogic);
      editRelations.ShowDialog();
    }

    //--------------------------------------------------------------------------
    public void EditPhrases() {
      EditPhrasesDialog editPhases = new EditPhrasesDialog(_businessLogic);
      editPhases.ShowDialog();
    }

    //--------------------------------------------------------------------------
    public void MoreStats() {
      StatisticsDialog stats = new StatisticsDialog(_businessLogic);
      stats.ShowDialog();
    }

    //--------------------------------------------------------------------------
    public void ResetDB() {
      DialogResult result = System.Windows.Forms.MessageBox.Show(
        "Are you sure you want to proceed and lose all data?", 
        "Warning", 
        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
      if (result != DialogResult.Yes) {
        return;
      }

      string queryMessageResult = "";
      _currentLoadingScreen = new LoadingScreenDialog(
        "Reseting Database ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.ExportImport.ResetDB((status, message) => {
            queryMessageResult = message;

            _currentLoadingScreen.Invoke((MethodInvoker)delegate {
              _currentLoadingScreen.CloseScreen();
            });
          });
      });

      _currentLoadingScreen.ShowDialog();
      _currentLoadingScreen = null;

      if (!String.IsNullOrEmpty(queryMessageResult)) {
        System.Windows.Forms.MessageBox.Show(queryMessageResult, "Error");
        return;
      }
    }

    //--------------------------------------------------------------------------
    public void QueryDocuments() {
      DocumentsQueryDialog query = new DocumentsQueryDialog(
        _businessLogic, false);
      query.ShowDialog();
    }

    //--------------------------------------------------------------------------
    public void InspectWords() {
      DocumentsQueryDialog documentsQuery = 
        new DocumentsQueryDialog(_businessLogic, true);
      DialogResult result = documentsQuery.ShowDialog();
      if (result != DialogResult.OK) {
        return;
      }

      WordsInspectorDialog wordsQuery = new WordsInspectorDialog(
        _businessLogic, 
        documentsQuery.SelectedDocuments);
      wordsQuery.ShowDialog();
    }

    //--------------------------------------------------------------------------
    public void InspectGroups() {
      WordsInspectorDialog wordsQuery = new WordsInspectorDialog(
        _businessLogic,
        new List<Document>(),
        true);
      wordsQuery.ShowDialog();
    }

    //--------------------------------------------------------------------------
    public void InspectPhrases() {
      PhrasesInspectorDialog phrasesInspector = new PhrasesInspectorDialog(
        _businessLogic);
      phrasesInspector.ShowDialog();
    }

    //--------------------------------------------------------------------------
    public void WordsQuery() {
      WordsQueryDialog documentsQuery = new WordsQueryDialog(_businessLogic);
      DialogResult result = documentsQuery.ShowDialog();
      if (result != DialogResult.OK) {
        return;
      }
    }

    //--------------------------------------------------------------------------
    public void ExportDB() {
      SaveFileDialog saveFilePicker = new SaveFileDialog();
      saveFilePicker.Title = "Select export output file";
      saveFilePicker.Filter = "xml files (*.xml)|*.xml";
      saveFilePicker.FilterIndex = 1;
      saveFilePicker.RestoreDirectory = true;

      if (saveFilePicker.ShowDialog() != DialogResult.OK) {
        return;
      }

      if (saveFilePicker.FileName.Length <= 0) {
        return;
      }

      //saveFilePicker.FileName;
      string queryMessageResult = "";
      _currentLoadingScreen = new LoadingScreenDialog("Exporting ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.ExportImport.Export(
          new FileInfo(saveFilePicker.FileName), (status, message) => {
          queryMessageResult = message;

          _currentLoadingScreen.Invoke((MethodInvoker)delegate {
            _currentLoadingScreen.CloseScreen();
          });
        });
      });

      _currentLoadingScreen.ShowDialog();
      _currentLoadingScreen = null;

      // check results
      if (!String.IsNullOrEmpty(queryMessageResult)) {
        System.Windows.Forms.MessageBox.Show(queryMessageResult, "Error");
        return;
      }
    }

    //--------------------------------------------------------------------------
    public void ImportDB() {
      OpenFileDialog filePicker = new OpenFileDialog();
      filePicker.Title = "Select xml file to import";
      filePicker.Multiselect = false;
      filePicker.Filter = "xml files (*.xml)|*.xml";
      filePicker.FilterIndex = 1;
      filePicker.RestoreDirectory = true;

      if (filePicker.ShowDialog() != DialogResult.OK) {
        return;
      }

      DialogResult result = System.Windows.Forms.MessageBox.Show(
        "Warning: in order to import this file, the entire database will be " +
        "erased and then the file will be imported.  Any data that isn't " + 
        "part of the import will be gone.  Are you sure you want to proceed?", 
        "Warning", 
        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
      if (result != DialogResult.Yes) {
        return;
      }


      string queryMessageResult = "";
      _currentLoadingScreen = new LoadingScreenDialog(
        "Importing Database ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.ExportImport.Import(
          new FileInfo(filePicker.FileName), (status, message) => {
            queryMessageResult = message;

            _currentLoadingScreen.Invoke((MethodInvoker)delegate {
              _currentLoadingScreen.CloseScreen();
            });
          });
      });

      _currentLoadingScreen.ShowDialog();
      _currentLoadingScreen = null;

      // check results
      if (!String.IsNullOrEmpty(queryMessageResult)) {
        System.Windows.Forms.MessageBox.Show(queryMessageResult, "Error");
        return;
      }
    }

    //--------------------------------------------------------------------------
    // BusinessLogicDelegate
    public void OnInitializationComplete(bool status, string error) {
      _currentLoadingScreen.Invoke((MethodInvoker)delegate {
        _currentLoadingScreen.CloseScreen();
        _currentLoadingScreen = null;

        if (!status) {
          System.Windows.Forms.MessageBox.Show(
            "Failed to initialize database: " + error, "Error");
          Application.Exit();
          return;
        }
      });
    }

    //--------------------------------------------------------------------------
    public void OnStatsUpdate(Stats stats) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.SetStats(stats);
      });
    }

    //--------------------------------------------------------------------------
    public void OnLoadDocumentComplete(
      FileInfo documentFile, bool status, string message) {
      _documentsLoader.OnLoadDocumentComplete(documentFile, status, message);

      if ((_documentsLoader.HasDocumentToProcess) || 
          (_currentLoadingScreen == null)) {
        return;
      }

      // done - close loading screen
      _currentLoadingScreen.Invoke((MethodInvoker)delegate {
        _currentLoadingScreen.CloseScreen();
        _currentLoadingScreen = null;
      });
    }

    //--------------------------------------------------------------------------
    public void OnLoadDocumentParsed(FileInfo documentFile) {
    }

    //--------------------------------------------------------------------------
    public void OnLoadDocumentProcessing(
      FileInfo documentFile, int percent, ref bool cancel) {
      cancel = _currentLoadingScreen.CancelClicked;
      if (cancel) {
        return;
      }

      _currentLoadingScreen.Invoke((MethodInvoker)delegate {
        _currentLoadingScreen.SetProgress(percent);
        _currentLoadingScreen.SetLabel(_documentsLoader.GetDescription());
      });
    }

    //--------------------------------------------------------------------------
    public void OnDatabaseImportStatusChanged(string status) {
      _currentLoadingScreen.Invoke((MethodInvoker)delegate {
        _currentLoadingScreen.SetLabel(status);
      });
    }

    //--------------------------------------------------------------------------
    public void OnDatabaseImportProgress(int percent) {
      _currentLoadingScreen.Invoke((MethodInvoker)delegate {
        _currentLoadingScreen.SetProgress(percent);
      });
    }

    //--------------------------------------------------------------------------
    private ContentResolution InitContentRetreivalSettings() {
      // defaults for Content retrieval
      ContentResolution contentRetreivalSettings = new ContentResolution() {
        UseLines = true,
        Delta = 1
      };

      bool userLines = contentRetreivalSettings.UseLines;
      uint delta = contentRetreivalSettings.Delta;

#pragma warning disable CS0618
      Boolean.TryParse(
        ConfigurationSettings.AppSettings["content_retreival_res.lines"],
        out userLines);

      UInt32.TryParse(
        ConfigurationSettings.AppSettings["content_retreival_res.delta"],
        out delta);
#pragma warning restore CS0618

      contentRetreivalSettings.UseLines = userLines;
      contentRetreivalSettings.Delta = delta;

      return contentRetreivalSettings;
    }

    //--------------------------------------------------------------------------
    private bool ShouldPerformIntegirtyValidations() {
      bool performValidation = false;
#pragma warning disable CS0618
      Boolean.TryParse(
        ConfigurationSettings.AppSettings["perform_integrity_validations"],
        out performValidation);
#pragma warning restore CS0618

      return performValidation;
    }

    //--------------------------------------------------------------------------
    private int GetStatisticsWordFrequenciesLimit() {
      int result = kDefaultWordFrequenciesLimit;
      
      #pragma warning disable CS0618
      Int32.TryParse(
        ConfigurationSettings.AppSettings["statistics.word_frequencies.limit"],
        out result);
      #pragma warning restore CS0618

      return result;
    }

    //--------------------------------------------------------------------------
    private int GetStatisticsLongQueriesTimeout() {
      int result = kDefaultLongQueriesTimeoutInSeconds;
     
      #pragma warning disable CS0618
      Int32.TryParse(
        ConfigurationSettings.AppSettings[
          "statistics.long_queries_timeout_in_seconds"],
        out result);
      #pragma warning restore CS0618

      return result;
    }

    //--------------------------------------------------------------------------
    private books.business_logic.common.Statistics GetStatisticsConfigurations() {
      books.business_logic.common.Statistics result = 
        new books.business_logic.common.Statistics();
      
      result.WordFrequenciesLimit = GetStatisticsWordFrequenciesLimit();
      result.LongQueriesTimeoutInSeconds = GetStatisticsLongQueriesTimeout();

      return result;
    }


  }
}
