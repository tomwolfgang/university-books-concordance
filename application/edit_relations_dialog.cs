using application.controllers;
using books.business_logic;
using books.business_logic.models;
using System;
using System.Windows.Forms;

namespace application {
  public partial class EditRelationsDialog : Form {
    //--------------------------------------------------------------------------
    private EditRelationsController _controller = null;

    //--------------------------------------------------------------------------
    public EditRelationsDialog(BusinessLogic businessLogic) {
      InitializeComponent();

      _controller = new EditRelationsController(this, businessLogic);
    }

    //--------------------------------------------------------------------------
    private void EditRelationsDialog_Shown(object sender, System.EventArgs e) {
      _controller.Initialize();
    }

    //--------------------------------------------------------------------------
    private void btnLoadWords_Click(object sender, System.EventArgs e) {
      _controller.LoadWords();
    }

    //--------------------------------------------------------------------------
    private void listViewRelations_DoubleClick(object sender, 
                                               System.EventArgs e) {
      if (listViewRelations.SelectedItems.Count <= 0) {
        return;
      }

      ListViewItem item = listViewRelations.SelectedItems[0];
      _controller.RelationDoubleClicked((Relation)item.Tag);
    }

    //--------------------------------------------------------------------------
    private void listViewRelations_SelectedIndexChanged(object sender, 
                                                        System.EventArgs e) {
      btnRemoveRelation.Enabled = (listViewRelations.SelectedItems.Count > 0);
    }

    //--------------------------------------------------------------------------
    private void listViewWordPairsInRelation_SelectedIndexChanged(
      object sender, System.EventArgs e) {
      btnRemoveWordPair.Enabled = 
        (listViewWordPairsInRelation.SelectedItems.Count > 0);
    }

    //--------------------------------------------------------------------------
    private void btnRemoveRelation_Click(object sender, System.EventArgs e) {
      if (listViewRelations.SelectedItems.Count <= 0) {
        return;
      }

      ListViewItem item = listViewRelations.SelectedItems[0];
      _controller.RemoveRelation(item);
    }

    //--------------------------------------------------------------------------
    private void btnRemoveWordPair_Click(object sender, System.EventArgs e) {
      if (listViewWordPairsInRelation.SelectedItems.Count <= 0) {
        return;
      }

      ListViewItem item = listViewWordPairsInRelation.SelectedItems[0];
      _controller.RemoveRelationWordPair(item);
    }

    //--------------------------------------------------------------------------
    private void btnAddRelation_Click(object sender, System.EventArgs e) {
      string newRelation = textBoxRelation.Text;
      if (newRelation.Length <= 0) {
        return;
      }

      foreach (ListViewItem item in listViewRelations.Items) {
        string relation = item.Text;
        if (relation.ToLower().Equals(newRelation.ToLower())) {
          // already exists in the list
          item.Selected = true;
          return;
        }
      }

      textBoxRelation.Clear();
      _controller.AddRelation(newRelation);
    }

    //--------------------------------------------------------------------------
    private void btnAddWordPair_Click(object sender, System.EventArgs e) {
      if (textBoxFirstWord.Text.Length <= 0) {
        textBoxFirstWord.Select();
        return;
      }

      if (textBoxSecondWord.Text.Length <= 0) {
        textBoxSecondWord.Select();
        return;
      }

      foreach (ListViewItem item in listViewWordPairsInRelation.Items) {
        Tuple<Word, Word> wordPair = (Tuple<Word, Word>)item.Tag;
        if ((wordPair.Item1.Value.ToLower().Equals(textBoxFirstWord.Text.ToLower())) && 
            (wordPair.Item2.Value.ToLower().Equals(textBoxSecondWord.Text.ToLower()))) {
          // already exists in the list
          item.Selected = true;
          return;
        }
      }

      _controller.AddRelationWordPair(textBoxFirstWord.Text,
                                      textBoxSecondWord.Text);

      textBoxFirstWord.Clear();
      textBoxSecondWord.Clear();
    }

    //--------------------------------------------------------------------------
    private void textBoxFirstWord_DragDrop(object sender, DragEventArgs e) {
      try {
        var dragItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
        textBoxFirstWord.Text = dragItem.Text;
      } catch (Exception) {
      }
    }

    //--------------------------------------------------------------------------
    private void textBoxFirstWord_DragEnter(object sender, DragEventArgs e) {
      e.Effect = DragDropEffects.Copy;
    }

    //--------------------------------------------------------------------------
    private void textBoxSecondWord_DragDrop(object sender, DragEventArgs e) {
      try {
        var dragItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
        textBoxSecondWord.Text = dragItem.Text;
      } catch(Exception) {
      }
    }

    //--------------------------------------------------------------------------
    private void textBoxSecondWord_DragEnter(object sender, DragEventArgs e) {
      e.Effect = DragDropEffects.Copy;
    }

    //--------------------------------------------------------------------------
    private void listViewWordsInDatabase_ItemDrag(object sender, 
                                                  ItemDragEventArgs e) {
      DoDragDrop(e.Item, DragDropEffects.Copy);
    }
  }
}
