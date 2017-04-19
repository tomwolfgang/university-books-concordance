using application.common;
using books.business_logic;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace application.controllers {
  class PhrasesInspectorController {
    private PhrasesInspectorDialog _parentForm = null;
    private BusinessLogic _businessLogic = null;
    private LoadingScreenDialog _currentLoadingScreen = null;

    //--------------------------------------------------------------------------
    public PhrasesInspectorController(
      PhrasesInspectorDialog parentForm,
      BusinessLogic businessLogic) {
      _parentForm = parentForm;
      _businessLogic = businessLogic;

      UpdatePhrasesCount();
      UpdateLocationsCount();
    }

    //--------------------------------------------------------------------------
    internal void Initialize() {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog("Retreiving Phrases ...",
                                                      false);

      UpdatePhrasesCount();

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Phrases.GetAll((phrases, status, message) => {
          queryMessageResult = message;

          if (status) {
            FillPhrasesListOnUIThread(phrases, false);
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
    public void QueryPhrase(Phrase phrase) {
      string queryMessageResult = "";
      _parentForm.richTextBoxContents.Text = "";      

      _currentLoadingScreen = new LoadingScreenDialog(
        "Running Phrase Query ...", false);

      UpdateLocationsCount();

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);
        _businessLogic.Phrases.Query(phrase, 
                                     (phrasesLocations, status, message) => {
          queryMessageResult = message;

          if (status) {
            FillLocationsListOnUIThread(phrasesLocations);
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
    public void GetLocationItemContents(ListViewItem listViewItem) {
      Tuple<LocationDetail, LocationDetail> phraseLocations = 
        (Tuple<LocationDetail, LocationDetail>)listViewItem.Tag;

      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Retreiving Contents from Document ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Queries.GetContents(
          phraseLocations,
          (contents, wordOffsetBegin, wordOffsetEnd, status, message) => {
            queryMessageResult = message;

            if (status) {
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
    private void FillPhrasesListOnUIThread(List<Phrase> phrases, bool append) {
      _parentForm.Invoke((MethodInvoker)delegate {
        if (!append) {
          _parentForm.listViewPhrases.Items.Clear();
        }

        foreach (Phrase phrase in phrases) {
          ListViewItem listViewItem = new ListViewItem();

          StringBuilder text = new StringBuilder();
          foreach (Word word in phrase.Words) {
            text.Append(" " + word.Value);
          }

          listViewItem.Text = text.ToString().TrimStart();
          listViewItem.Tag = phrase;
          _parentForm.listViewPhrases.Items.Add(listViewItem);
        }

        _parentForm.listViewPhrases.Sorting = SortOrder.Ascending;
        _parentForm.listViewPhrases.Sort();
        UpdatePhrasesCount();
      });
    }

    //--------------------------------------------------------------------------
    private void FillLocationsListOnUIThread(
      List<Tuple<LocationDetail, LocationDetail>> locations) {
      
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.listViewLocations.Items.Clear();

        foreach (var item in locations) {
          //doc id, title, page, sentence, paragraph
          ListViewItem listViewItem = new ListViewItem();
          listViewItem.Text = item.Item1.Document.GutenbergId;
          listViewItem.SubItems.Add(item.Item1.Document.Title);
          listViewItem.SubItems.Add((item.Item1.Location.Page + 1).ToString(
            "N0",
            CultureInfo.InvariantCulture));
          listViewItem.SubItems.Add((item.Item1.Location.Sentence + 1).ToString(
            "N0",
            CultureInfo.InvariantCulture));
          listViewItem.SubItems.Add((item.Item1.Location.Paragraph + 1).ToString(
            "N0",
            CultureInfo.InvariantCulture));
          listViewItem.Tag = item;
          _parentForm.listViewLocations.Items.Add(listViewItem);
        }

        UpdateLocationsCount();
      });
    }

    //--------------------------------------------------------------------------
    private void UpdatePhrasesCount() {
      _parentForm.groupBoxPhrases.Text = String.Format(
        "Phrases ({0}):",
        _parentForm.listViewPhrases.Items.Count.ToString(
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
