using application.controllers;
using books.business_logic;
using books.business_logic.models;
using System.Windows.Forms;

namespace application {
  public partial class EditGroupsDialog : Form {
    //--------------------------------------------------------------------------
    private EditGroupsController _controller = null;

    //--------------------------------------------------------------------------
    public EditGroupsDialog(BusinessLogic businessLogic) {
      InitializeComponent();

      _controller = new EditGroupsController(this, businessLogic);
    }

    //--------------------------------------------------------------------------
    private void EditGroupsDialog_Shown(object sender, System.EventArgs e) {
      _controller.Initialize();
    }

    //--------------------------------------------------------------------------
    private void btnLoadWords_Click(object sender, System.EventArgs e) {
      _controller.LoadWords();
    }

    //--------------------------------------------------------------------------
    private void btnAddGroup_Click(object sender, System.EventArgs e) {
      _controller.AddGroup();
    }

    //--------------------------------------------------------------------------
    private void btnRemoveGroup_Click(object sender, System.EventArgs e) {
      _controller.RemoveGroup();
    }

    //--------------------------------------------------------------------------
    private void listViewGroups_SelectedIndexChanged(
      object sender, System.EventArgs e) {
      btnRemoveGroup.Enabled = (listViewGroups.SelectedItems.Count > 0);
    }

    //--------------------------------------------------------------------------
    private void listViewWordsInGroup_SelectedIndexChanged(object sender, 
                                                           System.EventArgs e) {
      btnRemoveWord.Enabled = (listViewWordsInGroup.SelectedItems.Count > 0);
    }


    //--------------------------------------------------------------------------
    private void listViewGroups_DoubleClick(object sender, System.EventArgs e) {
      if (listViewGroups.SelectedItems.Count <= 0) {
        return;
      }

      ListViewItem item = listViewGroups.SelectedItems[0];
      _controller.GroupDoubleClicked((Group)item.Tag);
    }

    //--------------------------------------------------------------------------
    private void btnAddWord_Click(object sender, System.EventArgs e) {
      if (textBoxWord.Text.Length <= 0) {
        return;
      }

      _controller.AddGroupWord(textBoxWord.Text);
      
      textBoxWord.Clear();
    }

    //--------------------------------------------------------------------------
    private void btnRemoveWord_Click(object sender, System.EventArgs e) {
      _controller.RemoveGroupWord();
    }

    //--------------------------------------------------------------------------
    private void listViewWordsInDatabase_DoubleClick(object sender, 
                                                     System.EventArgs e) {
      if (listViewWordsInDatabase.SelectedItems.Count <= 0) {
        return;
      }

      ListViewItem item = listViewWordsInDatabase.SelectedItems[0];
      _controller.AddGroupWord(item.Text);
    }

  }
}
