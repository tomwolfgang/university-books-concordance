using books.business_logic;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace application.controllers {
  //----------------------------------------------------------------------------
  class DocumentsQueryController {
    private DocumentsQueryDialog _parentForm = null;
    private BusinessLogic _businessLogic = null;
    private LoadingScreenDialog _currentLoadingScreen = null;
    private bool _withNextButton = false;

    //--------------------------------------------------------------------------
    public DocumentsQueryController(
      DocumentsQueryDialog parentForm, 
      BusinessLogic businessLogic, 
      bool withNextButton) {
      _parentForm = parentForm;
      _businessLogic = businessLogic;
      _withNextButton = withNextButton;

      InitializeForWordsQuery();

      UpdateDocumentResultsCount();
      UpdateWordsFilterCount();
    }

    //--------------------------------------------------------------------------
    public void AddWord() {
      string newWord = _parentForm.textBoxWord.Text.ToLower();
      if (newWord.Length <= 0) {
        return;
      }

      _parentForm.textBoxWord.Clear();

      foreach (string word in _parentForm.listBoxWords.Items) {
        if (word.ToLower().Equals(newWord)) {
          // already exists in the list
          return;
        }
      }

      _parentForm.listBoxWords.Items.Add(newWord);
      UpdateWordsFilterCount();
    }

    //--------------------------------------------------------------------------
    public void RemoveSelectedWords() {
      ListBox.SelectedObjectCollection selectedItems =
        new ListBox.SelectedObjectCollection(_parentForm.listBoxWords);
      selectedItems = _parentForm.listBoxWords.SelectedItems;

      for (int i = selectedItems.Count - 1; i >= 0; --i) {
        _parentForm.listBoxWords.Items.Remove(selectedItems[i]);
      }

      UpdateWordsFilterCount();
    }

    //--------------------------------------------------------------------------
    public void PerformQuery() {
      List<string> words = BuildWordsForQuery();
      List<DocumentProperty> documentProperties =
        BuildDocumentPropertiesForQuery();

      string queryMessageResult = "";
      List<Document> resultDocuments = null;

      _currentLoadingScreen = new LoadingScreenDialog(
        "Running Documents Query ...", false);

      UpdateDocumentResultsCount();

      // in order to show the loading screen as a modal dialog - we have to 
      // run the query on a new thread
      Task.Run(() => {
        // the sleep will promise _currentLoadingScreen.ShowDialog is called
        // before the query result comes in, and we can then safely call 
        // Invoke on _currentLoadingScreen
        System.Threading.Thread.Sleep(200);

        _businessLogic.Queries.GetDocuments(
          words, documentProperties, (documents, status, message) => {

            queryMessageResult = message;
            resultDocuments = documents;

            _currentLoadingScreen.Invoke((MethodInvoker)delegate {
              _currentLoadingScreen.CloseScreen();
            });
          }); // GetDocuments
      });

      // this line waits until _currentLoadingScreen.CloseScreen() is called
      _currentLoadingScreen.ShowDialog();

      // if we are here it means we got a result from the query
      _currentLoadingScreen = null;
      _parentForm.listViewDocuments.Items.Clear();

      // check results
      if (!String.IsNullOrEmpty(queryMessageResult)) {
        System.Windows.Forms.MessageBox.Show(queryMessageResult, "Error");
        return;
      }

      foreach (Document item in resultDocuments) {
        ListViewItem listViewItem = new ListViewItem();
        listViewItem.Text = item.GutenbergId;
        listViewItem.SubItems.Add(item.Author);
        listViewItem.SubItems.Add(item.Title);
        listViewItem.SubItems.Add(item.ReleaseDate.Value.ToShortDateString());
        listViewItem.Tag = item;
        _parentForm.listViewDocuments.Items.Add(listViewItem);
      }

      UpdateDocumentResultsCount();
    }

    //--------------------------------------------------------------------------
    public void OpenSelectedDocument() {
      if (_parentForm.listViewDocuments.SelectedItems.Count == 0) {
        return;
      }

      Document document = 
        (Document)_parentForm.listViewDocuments.SelectedItems[0].Tag;
      Process.Start(document.LocalFile.FullName);
    }

    //--------------------------------------------------------------------------
    public void SetSelectedDocumentsAndClose() {
      _parentForm.SelectedDocuments.Clear();
      
      var selectedItems = _parentForm.listViewDocuments.SelectedItems;
      for (int i = 0; i < selectedItems.Count; ++i) {
        _parentForm.SelectedDocuments.Add((Document)selectedItems[i].Tag);
      }

      _parentForm.DialogResult = DialogResult.OK;
      _parentForm.Close();
    }

    //--------------------------------------------------------------------------
    private void InitializeForWordsQuery() {
      if (!_withNextButton) {
        _parentForm.Height = _parentForm.groupBoxResults.Bottom + 45;
        _parentForm.btnNext.Enabled = false;
        return;
      }

      _parentForm.Text = 
        "Book Concordance - Inspector - Select Documents";
      
      // reveal the next button
      _parentForm.Height = _parentForm.lblWordsQueryInformation2.Bottom + 45;
      _parentForm.btnNext.Enabled = true;
    }

    //--------------------------------------------------------------------------
    private List<string> BuildWordsForQuery() {
      List<string> words = new List<string>();

      foreach (string word in _parentForm.listBoxWords.Items) {
        words.Add(word);
      }

      return words;
    }

    //--------------------------------------------------------------------------
    private List<DocumentProperty> BuildDocumentPropertiesForQuery() {
      List<DocumentProperty> result = new List<DocumentProperty>();

      // gutenberg id
      if (_parentForm.checkBoxGutenbergId.Checked) {
        result.Add(new DocumentProperty() {
          Field = DocumentProperty.Property.GutenbergId,
          StrValue = _parentForm.textBoxGutenbergId.Text
        });
      }

      // title
      if (_parentForm.checkBoxTitle.Checked) {
        result.Add(new DocumentProperty() {
          Field = DocumentProperty.Property.Title,
          StrValue = _parentForm.textBoxTitle.Text
        });
      }

      // author
      if (_parentForm.checkBoxAuthor.Checked) {
        result.Add(new DocumentProperty() {
          Field = DocumentProperty.Property.Author,
          StrValue = _parentForm.textBoxAuthor.Text
        });
      }

      // release date
      if (_parentForm.checkBoxReleaseDate.Checked) {
        result.Add(new DocumentProperty() {
          Field = DocumentProperty.Property.ReleaseDate,
          DateTimeValue = _parentForm.dateTimePicker.Value
        });
      }

      return result;
    }

    //--------------------------------------------------------------------------
    private void UpdateWordsFilterCount() {
      _parentForm.groupBoxWords.Text = String.Format(
        "Words Filter ({0}):",
        _parentForm.listBoxWords.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }

    //--------------------------------------------------------------------------
    private void UpdateDocumentResultsCount() {
      _parentForm.groupBoxResults.Text = String.Format(
        "Results ({0}):",
        _parentForm.listViewDocuments.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }

  }
}
