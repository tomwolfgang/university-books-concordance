using application.common;
using books.business_logic;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace application.controllers {
  //----------------------------------------------------------------------------
  class WordsInspectorController {
    private WordsInspectorDialog _parentForm = null;
    private BusinessLogic _businessLogic = null;
    private LoadingScreenDialog _currentLoadingScreen = null;

    private List<Document> _documentsFilter = null;
    private bool _showGroups = false;
    private Word _wordToInspect = null;

    public bool ShowGroups {
      get {
        return _showGroups;
      }
    }

    //--------------------------------------------------------------------------
    public WordsInspectorController(
      WordsInspectorDialog parentForm,
      BusinessLogic businessLogic,
      List<Document> documentsFilter,
      bool showGroups,
      Word wordToInspect) {
      _parentForm = parentForm;
      _businessLogic = businessLogic;
      _documentsFilter = documentsFilter;
      _showGroups = showGroups;
      _wordToInspect = wordToInspect;
    }

    //--------------------------------------------------------------------------
    public void Initialize() {
      if (!_showGroups) {
        InitializeWithDocumentsFilter();
      } else {
        InitializeForGroups();
      }
    }

    //--------------------------------------------------------------------------
    public void InitializeForGroups() {
      _parentForm.Text = "Book Concordance - Groups Inspector";
      _parentForm.listViewWords.Columns[0].Text = "Group";

      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Running Groups Query ...", false);

      UpdateGroupsCount();
      UpdateLocationsCount();

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Groups.GetAll((groups, status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillGroupsListOnUIThread(groups);
          }

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
    public void InitializeWithDocumentsFilter() {
      string queryMessageResult = "";

      UpdateWordsCount();
      UpdateLocationsCount();
     
      if (_wordToInspect != null) {
        // we are inspecting a single word
        FillWordsListOnUIThread(new List<Word> { _wordToInspect });
        _parentForm.listViewWords.Items[0].Selected = true;
        WordDoubleClicked(_wordToInspect);
        return;
      }

      _currentLoadingScreen = new LoadingScreenDialog(
        "Running Words Query ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Queries.GetWords(_documentsFilter, 
                                        new List<WordLocationProperty>(),   
                                        (words, status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillWordsListOnUIThread(words);
          }

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
    public void WordDoubleClicked(Word word) {
      string queryMessageResult = "";
      _parentForm.richTextBoxContents.Text = "";      

      _currentLoadingScreen = new LoadingScreenDialog(
        "Running Word in Document Query ...", false);

      UpdateLocationsCount();

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Queries.GetWordLocationDetails(
          word, 
          _documentsFilter, 
          (locations, status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
              FillLocationsListOnUIThread(locations, word);
            }

            _currentLoadingScreen.Invoke((MethodInvoker)delegate {
            _currentLoadingScreen.CloseScreen();
          });
        }); // GetDocuments
      });

      // this line waits until _currentLoadingScreen.CloseScreen() is called
      _currentLoadingScreen.ShowDialog();
      _currentLoadingScreen = null;

      // check results
      if (!String.IsNullOrEmpty(queryMessageResult)) {
        System.Windows.Forms.MessageBox.Show(queryMessageResult, "Error");
        return;
      }
    }

    //--------------------------------------------------------------------------
    public void GroupDoubleClicked(Group group) {
      string queryMessageResult = "";
      _parentForm.richTextBoxContents.Text = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Running Groups Locations Query ...", false);

      UpdateLocationsCount();

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Queries.GetWordsLocationDetails(
          group,
          (wordsLocations, status, message) => {
          
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillWordsLocationsListOnUIThread(wordsLocations);
          }

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
    public void LocationDoubleClicked(LocationDetail location) {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Retreiving Contents from Document ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Queries.GetContents(
          location,
          (contents, wordOffsetBegin, wordOffsetEnd, status, message) => {
            queryMessageResult = message;

            if (String.IsNullOrEmpty(queryMessageResult)) {
              _parentForm.Invoke((MethodInvoker)delegate {
                UIUtils.SetRichTextContents(_parentForm.richTextBoxContents,
                                            contents, 
                                            wordOffsetBegin, 
                                            wordOffsetEnd);
              });
            }

            _currentLoadingScreen.Invoke((MethodInvoker)delegate {
              _currentLoadingScreen.CloseScreen();
            });
          }); // GetContents
      });

      // this line waits until _currentLoadingScreen.CloseScreen() is called
      _currentLoadingScreen.ShowDialog();
      _currentLoadingScreen = null;

      // check results
      if (!String.IsNullOrEmpty(queryMessageResult)) {
        System.Windows.Forms.MessageBox.Show(queryMessageResult, "Error");
        return;
      }
    }


    //--------------------------------------------------------------------------
    private void FillWordsListOnUIThread(List<Word> words) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.listViewWords.Items.Clear();
        foreach (Word item in words) {
          ListViewItem listViewItem = new ListViewItem();
          listViewItem.Text = item.Value;
          listViewItem.Tag = item;
          _parentForm.listViewWords.Items.Add(listViewItem);
        }

        _parentForm.listViewWords.Sorting = SortOrder.Ascending;
        _parentForm.listViewWords.Sort();
        UpdateWordsCount();
      });
    }

    //--------------------------------------------------------------------------
    private void FillGroupsListOnUIThread(List<Group> groups) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.listViewWords.Items.Clear();
        foreach (Group item in groups) {
          ListViewItem listViewItem = new ListViewItem();
          listViewItem.Text = item.Name;
          listViewItem.Tag = item;
          _parentForm.listViewWords.Items.Add(listViewItem);
        }

        _parentForm.listViewWords.Sorting = SortOrder.Ascending;
        _parentForm.listViewWords.Sort();
        UpdateGroupsCount();
      });
    }

    //--------------------------------------------------------------------------
    private void FillWordsLocationsListOnUIThread(
      List<WordLocationDetails> wordsLocations) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.listViewLocations.Items.Clear();
      });

      foreach (WordLocationDetails item in wordsLocations) {
        FillLocationsListOnUIThread(item, item.Word, true);
      }
    }

    //--------------------------------------------------------------------------
    private void FillLocationsListOnUIThread(WordLocationDetails locations, 
                                             Word word,
                                             bool append = false) {
      // do we have a documents filter?
      _parentForm.Invoke((MethodInvoker)delegate {
        if (!append) {
          _parentForm.listViewLocations.Items.Clear();
        }

        foreach (LocationDetail item in locations.LocationDetails) {
          ListViewItem listViewItem = new ListViewItem();
          listViewItem.Text = item.Document.GutenbergId;
          listViewItem.SubItems.Add(item.Document.Title);
          listViewItem.SubItems.Add(word.Value);
          listViewItem.SubItems.Add((item.Location.Line+1).ToString(
            "N0",
            CultureInfo.InvariantCulture));
          listViewItem.SubItems.Add((item.Location.Page+1).ToString(
            "N0",
            CultureInfo.InvariantCulture));
          listViewItem.SubItems.Add((item.Location.Sentence+1).ToString(
            "N0",
            CultureInfo.InvariantCulture));
          listViewItem.SubItems.Add((item.Location.Paragraph+1).ToString(
            "N0",
            CultureInfo.InvariantCulture));
          listViewItem.Tag = item;
          _parentForm.listViewLocations.Items.Add(listViewItem);
        }

        UpdateLocationsCount();

        // update the title of the form to reflect that currently selected word
        _parentForm.Text = String.Format(
          "Book Concordance - {0} Inspector", 
          _showGroups ? "Groups" : "Words");
      });
    }

    //--------------------------------------------------------------------------
    private void UpdateWordsCount() {
      _parentForm.groupBoxWords.Text = String.Format(
        "Words ({0}):",
        _parentForm.listViewWords.Items.Count.ToString(
          "N0", 
          CultureInfo.InvariantCulture));
    }

    //--------------------------------------------------------------------------
    private void UpdateGroupsCount() {
      _parentForm.groupBoxWords.Text = String.Format(
        "Groups ({0}):",
        _parentForm.listViewWords.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }
    
    //--------------------------------------------------------------------------
    private void UpdateLocationsCount() {
      _parentForm.groupBoxLocations.Text = String.Format(
        "Results ({0}):",
        _parentForm.listViewLocations.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }

  }
}
