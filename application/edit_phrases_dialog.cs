using application.controllers;
using books.business_logic;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace application {
  public partial class EditPhrasesDialog : Form {
    //--------------------------------------------------------------------------
    private EditPhrasesController _controller = null;

    //--------------------------------------------------------------------------
    public EditPhrasesDialog(BusinessLogic businessLogic) {
      InitializeComponent();
      
      _controller = new EditPhrasesController(this, businessLogic);
    }

    //--------------------------------------------------------------------------
    private void EditPhrasesDialog_Shown(object sender, EventArgs e) {
      _controller.Initialize();
    }

    //--------------------------------------------------------------------------
    private void listViewGroups_SelectedIndexChanged(object sender,
                                                     EventArgs e) {
      richTextBoxContents.ResetText();
      btnRemove.Enabled = false;

      if (listViewPhrases.SelectedItems.Count <= 0) {
        return;
      }

      richTextBoxContents.Text = listViewPhrases.SelectedItems[0].Text;
      btnRemove.Enabled = true;
    }

    //--------------------------------------------------------------------------
    private void btnRemove_Click(object sender, EventArgs e) {
      if (listViewPhrases.SelectedItems.Count <= 0) {
        return;
      }

      _controller.RemovePhrase(listViewPhrases.SelectedItems[0]);
    }

    //--------------------------------------------------------------------------
    private void richTextBoxAdd_TextChanged(object sender, EventArgs e) {
      btnAdd.Enabled = false;

      if (richTextBoxAdd.Text.Length <= 0) {
        return;
      }

      btnAdd.Enabled = true;
    }

    //--------------------------------------------------------------------------
    private void btnAdd_Click(object sender, EventArgs e) {
      if (richTextBoxAdd.Text.Length <= 0) {
        return;
      }

      _controller.AddPhrase(richTextBoxAdd.Text);
    }

  }
}
