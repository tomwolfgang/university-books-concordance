using books.business_logic;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace application.controllers {
  //----------------------------------------------------------------------------
  class EditPhrasesController {
    private EditPhrasesDialog _parentForm = null;
    private BusinessLogic _businessLogic = null;
    private LoadingScreenDialog _currentLoadingScreen = null;

    //--------------------------------------------------------------------------
    public EditPhrasesController(
      EditPhrasesDialog parentForm,
      BusinessLogic businessLogic) {
      _parentForm = parentForm;
      _businessLogic = businessLogic;

      UpdatePhrasesCount();
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
    public void RemovePhrase(ListViewItem phraseItem) {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Removing Phrase ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Phrases.Remove((Phrase)phraseItem.Tag, 
                                      (status, message) => {
          queryMessageResult = message;

          if (status) {
            _parentForm.Invoke((MethodInvoker)delegate {
              phraseItem.Remove();
              UpdatePhrasesCount();
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

      _parentForm.richTextBoxContents.ResetText();
    }

    //--------------------------------------------------------------------------
    public void AddPhrase(string text) {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Adding Phrase ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Phrases.Add(text, (phrase, status, message) => {
          queryMessageResult = message;

          if (status) {
            _parentForm.Invoke((MethodInvoker)delegate {
              FillPhrasesListOnUIThread(new List<Phrase>() { phrase }, true);
              _parentForm.richTextBoxAdd.ResetText();
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
    private void UpdatePhrasesCount() {
      _parentForm.groupBoxPhrases.Text = String.Format(
        "Phrases ({0}):",
        _parentForm.listViewPhrases.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }

  }
}
