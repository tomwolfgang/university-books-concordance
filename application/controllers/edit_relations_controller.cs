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
  class EditRelationsController {
    private EditRelationsDialog _parentForm = null;
    private BusinessLogic _businessLogic = null;
    private LoadingScreenDialog _currentLoadingScreen = null;
    private Relation _currentSelectedRelation = null;

    //--------------------------------------------------------------------------
    public EditRelationsController(
      EditRelationsDialog parentForm,
      BusinessLogic businessLogic) {
      _parentForm = parentForm;
      _businessLogic = businessLogic;

      UpdateRelationsCount();
      UpdateWordPairsCount(null);
      UpdateWordsInDatabaseCount();
    }

    //--------------------------------------------------------------------------
    internal void Initialize() {
      _currentSelectedRelation = null;

      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Retreiving Relations ...", false);

      UpdateRelationsCount();

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Relations.GetAll((relations, status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillRelationsListOnUIThread(relations, false);
          }

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
    public void LoadWords() {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Retreiving Words ...", false);

      UpdateWordsInDatabaseCount();

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        // get all words
        _businessLogic.Queries.GetWords(new List<Document>(), 
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

      _parentForm.btnLoadWords.Enabled = false;
    }

    //--------------------------------------------------------------------------
    public void RelationDoubleClicked(Relation relation) {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Retreiving Relation Words ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Relations.GetWordPairs(
          relation, (wordPairs, status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillRelationWordPairsListOnUIThread(relation, wordPairs, false);
          }

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

      _currentSelectedRelation = relation;
      _parentForm.btnAddWordPair.Enabled = true;
    }

    //--------------------------------------------------------------------------
    public void AddRelation(string newRelation) {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Adding Relation ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Relations.AddRelation(
          newRelation, (relation, status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillRelationsListOnUIThread(new List<Relation>(){ relation }, true);
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
    public void AddRelationWordPair(string firstWord, string secondWord) {
      if (_currentSelectedRelation == null) {
        return;
      }

      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Adding Relation Word Pair ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Relations.AddRelationWords(
          _currentSelectedRelation, 
          firstWord, 
          secondWord,
          (wordPair, status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillRelationWordPairsListOnUIThread(
              _currentSelectedRelation,
              new List<Tuple<Word, Word>> { wordPair },
              true);
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
    public void RemoveRelation(ListViewItem relationItem) {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Removing Relation ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Relations.RemoveRelation(
          (Relation)relationItem.Tag, (status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            _parentForm.Invoke((MethodInvoker)delegate {
              relationItem.Remove();
              UpdateRelationsCount();
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

      if (_currentSelectedRelation == null) {
        return;
      }

      Relation removedItem = (Relation)relationItem.Tag;
      if (removedItem.Name.ToLower() != 
          _currentSelectedRelation.Name.ToLower()) {
        return;
      }

      _currentSelectedRelation = null;
      _parentForm.listViewWordPairsInRelation.Items.Clear();
      _parentForm.btnAddWordPair.Enabled = false;
      UpdateWordPairsCount(null);
    }

    //--------------------------------------------------------------------------
    public void RemoveRelationWordPair(ListViewItem item) {
      if (_currentSelectedRelation == null) {
        return;
      }

      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Removing Relation Word Pair ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Relations.RemoveRelationWordPair(
          _currentSelectedRelation, 
          (Tuple<Word, Word>)item.Tag, 
          (status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            _parentForm.Invoke((MethodInvoker)delegate {
              item.Remove();
              UpdateWordPairsCount(_currentSelectedRelation);
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
    private void FillRelationsListOnUIThread(List<Relation> relations, 
                                             bool append) {
      _parentForm.Invoke((MethodInvoker)delegate {
        if (!append) {
          _parentForm.listViewRelations.Items.Clear();
        }

        foreach (Relation item in relations) {
          ListViewItem listViewItem = new ListViewItem();
          listViewItem.Text = item.Name;
          listViewItem.Tag = item;
          _parentForm.listViewRelations.Items.Add(listViewItem);
        }

        _parentForm.listViewRelations.Sorting = SortOrder.Ascending;
        _parentForm.listViewRelations.Sort();
        UpdateRelationsCount();
      });
    }

    //--------------------------------------------------------------------------
    private void FillWordsListOnUIThread(List<Word> words) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.listViewWordsInDatabase.Items.Clear();
        foreach (Word item in words) {
          ListViewItem listViewItem = new ListViewItem();
          listViewItem.Text = item.Value;
          listViewItem.Tag = item;
          _parentForm.listViewWordsInDatabase.Items.Add(listViewItem);
        }

        _parentForm.listViewWordsInDatabase.Sorting = SortOrder.Ascending;
        _parentForm.listViewWordsInDatabase.Sort();
        UpdateWordsInDatabaseCount();
      });
    }

    //--------------------------------------------------------------------------
    private void FillRelationWordPairsListOnUIThread(
      Relation relation, 
      List<Tuple<Word,Word>> wordPairs, 
      bool append) {

      _parentForm.Invoke((MethodInvoker)delegate {
        if (!append) {
          _parentForm.listViewWordPairsInRelation.Items.Clear();
        }

        foreach (Tuple<Word, Word> item in wordPairs) {
          ListViewItem listViewItem = new ListViewItem();
          listViewItem.Text = item.Item1.Value;
          listViewItem.SubItems.Add(item.Item2.Value);
          listViewItem.Tag = item;
          _parentForm.listViewWordPairsInRelation.Items.Add(listViewItem);
        }

        _parentForm.listViewWordPairsInRelation.Sorting = SortOrder.Ascending;
        _parentForm.listViewWordPairsInRelation.Sort();
        UpdateWordPairsCount(relation);
      });
    }

    //--------------------------------------------------------------------------
    private void UpdateRelationsCount() {
      _parentForm.groupBoxRelations.Text = String.Format(
        "Relations ({0}):",
        _parentForm.listViewRelations.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }

    //--------------------------------------------------------------------------
    private void UpdateWordPairsCount(Relation relation) {
      _parentForm.groupBoxWordPairs.Text = String.Format(
        "Words in {0} ({1}):",
        relation == null ? "Relation" : relation.Name,
        _parentForm.listViewWordPairsInRelation.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }

    //--------------------------------------------------------------------------
    private void UpdateWordsInDatabaseCount() {
      _parentForm.groupBoxWordsInDatabase.Text = String.Format(
        "Words in DB ({0}):",
        _parentForm.listViewWordsInDatabase.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }


  }
}
