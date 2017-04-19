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
  class EditGroupsController {
    private EditGroupsDialog _parentForm = null;
    private BusinessLogic _businessLogic = null;
    private LoadingScreenDialog _currentLoadingScreen = null;
    private Group _currentSelectedGroup = null;

    //--------------------------------------------------------------------------
    public EditGroupsController(
      EditGroupsDialog parentForm, 
      BusinessLogic businessLogic) {
      _parentForm = parentForm;
      _businessLogic = businessLogic;

      UpdateGroupsCount();
      UpdateWordsInGroupCount(null);
      UpdateWordsInDatabaseCount();
    }

    //--------------------------------------------------------------------------
    internal void Initialize() {
      _currentSelectedGroup = null; 

      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Retreiving Groups ...", false);

      UpdateGroupsCount();

      // in order to show the loading screen as a modal dialog - we have to 
      // run the query on a new thread
      Task.Run(() => {
        // the sleep will promise _currentLoadingScreen.ShowDialog is called
        // before the query result comes in, and we can then safely call 
        // Invoke on _currentLoadingScreen
        System.Threading.Thread.Sleep(200);

        // get all words
        _businessLogic.Groups.GetAll((groups, status, message) => {
          queryMessageResult = message;

          // because we might end up adding A LOT of words to the list, we
          // want to keep the loading screen visible while adding the items
          // to the list
          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillGroupsListOnUIThread(groups, false);
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
    public void AddGroup() {
      string newGroup = _parentForm.textBoxGroup.Text;
      if (newGroup.Length <= 0) {
        return;
      }

      _parentForm.textBoxGroup.Clear();

      foreach (ListViewItem groupItem in _parentForm.listViewGroups.Items) {
        string group = groupItem.Text;
        if (group.ToLower().Equals(newGroup.ToLower())) {
          // already exists in the list
          groupItem.Selected = true;
          return;
        }
      }

      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Adding Group ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        // get all words
        _businessLogic.Groups.AddGroup(newGroup, (group, status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillGroupsListOnUIThread(new List<Group>() { group }, true);
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
    public void AddGroupWord(string newWord) {
      if (_currentSelectedGroup == null) {
        return;
      }

      if (newWord.Length <= 0) {
        return;
      }

      foreach (ListViewItem item in _parentForm.listViewWordsInGroup.Items) {
        string word = item.Text;
        if (word.ToLower().Equals(newWord.ToLower())) {
          // already exists in the list
          item.Selected = true;
          return;
        }
      }

      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Adding Group Word ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Groups.AddGroupWord(_currentSelectedGroup, 
                                           newWord, 
                                           (word, status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillGroupWordsListOnUIThread(_currentSelectedGroup,
                                         new List<Word>() { word },
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
    public void RemoveGroupWord() {
      if (_currentSelectedGroup == null) {
        return;
      }

      if (_parentForm.listViewWordsInGroup.SelectedItems.Count <=0) {
        return;
      }

      var item = _parentForm.listViewWordsInGroup.SelectedItems[0];

      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Removing Group Word ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Groups.RemoveGroupWord(_currentSelectedGroup,
                                              (Word)item.Tag,
                                              (status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            _parentForm.Invoke((MethodInvoker)delegate {
              item.Remove();
              UpdateWordsInGroupCount(_currentSelectedGroup);
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
    public void RemoveGroup() {
      if (_parentForm.listViewGroups.SelectedItems.Count <= 0) {
        return;
      }

      ListViewItem item = _parentForm.listViewGroups.SelectedItems[0];

      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Removing Group ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        // get all words
        _businessLogic.Groups.RemoveGroup(
          (Group)item.Tag, (status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            _parentForm.Invoke((MethodInvoker)delegate {
              item.Remove();
              UpdateGroupsCount();
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

      if (_currentSelectedGroup == null) {
        return;
      }

      Group removedItem = (Group)item.Tag;
      if (removedItem.Name.ToLower() != _currentSelectedGroup.Name.ToLower()) {
        return;
      }

      _currentSelectedGroup = null;
      _parentForm.listViewWordsInGroup.Items.Clear();
      _parentForm.btnAddWord.Enabled = false;
      UpdateWordsInGroupCount(null);
    }

    //--------------------------------------------------------------------------
    public void LoadWords() {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Retreiving Words ...", false);

      UpdateWordsInDatabaseCount();

      // in order to show the loading screen as a modal dialog - we have to 
      // run the query on a new thread
      Task.Run(() => {
        // the sleep will promise _currentLoadingScreen.ShowDialog is called
        // before the query result comes in, and we can then safely call 
        // Invoke on _currentLoadingScreen
        System.Threading.Thread.Sleep(200);

        // get all words
        _businessLogic.Queries.GetWords(new List<Document>(), 
                                        new List<WordLocationProperty>(),   
                                        (words, status, message) => {
          queryMessageResult = message;

          // because we might end up adding A LOT of words to the list, we
          // want to keep the loading screen visible while adding the items
          // to the list
          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillWordsListOnUIThread(words);
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

      _parentForm.btnLoadWords.Enabled = false;
    }

    //--------------------------------------------------------------------------
    public void GroupDoubleClicked(Group group) {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Retreiving Group Words ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Groups.GetWords(group, (words, status, message) => {
          queryMessageResult = message;

          if (String.IsNullOrEmpty(queryMessageResult)) {
            FillGroupWordsListOnUIThread(group, words, false);
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

      _currentSelectedGroup = group;
      _parentForm.btnAddWord.Enabled = true;
    }

    //--------------------------------------------------------------------------
    private void FillGroupsListOnUIThread(List<Group> groups, bool append) {
      _parentForm.Invoke((MethodInvoker)delegate {
        if (!append) {
          _parentForm.listViewGroups.Items.Clear();
        }

        foreach (Group item in groups) {
          ListViewItem listViewItem = new ListViewItem();
          listViewItem.Text = item.Name;
          listViewItem.Tag = item;
          _parentForm.listViewGroups.Items.Add(listViewItem);
        }

        _parentForm.listViewGroups.Sorting = SortOrder.Ascending;
        _parentForm.listViewGroups.Sort();
        UpdateGroupsCount();
      });
    }

    //--------------------------------------------------------------------------
    private void FillGroupWordsListOnUIThread(Group group, 
                                              List<Word> words, 
                                              bool append) {
      _parentForm.Invoke((MethodInvoker)delegate {
        if (!append) {
          _parentForm.listViewWordsInGroup.Items.Clear();
        }

        foreach (Word item in words) {
          ListViewItem listViewItem = new ListViewItem();
          listViewItem.Text = item.Value;
          listViewItem.Tag = item;
          _parentForm.listViewWordsInGroup.Items.Add(listViewItem);
        }

        _parentForm.listViewWordsInGroup.Sorting = SortOrder.Ascending;
        _parentForm.listViewWordsInGroup.Sort();
        UpdateWordsInGroupCount(group);
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
    private void UpdateGroupsCount() {
      _parentForm.groupBoxGroups.Text = String.Format(
        "Words in DB ({0}):",
        _parentForm.listViewGroups.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }

    //--------------------------------------------------------------------------
    private void UpdateWordsInGroupCount(Group group) {
      _parentForm.groupBoxWordsInGroup.Text = String.Format(
        "Words in {0} ({1}):",
        group == null ? "Group" : group.Name,
        _parentForm.listViewWordsInGroup.Items.Count.ToString(
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
