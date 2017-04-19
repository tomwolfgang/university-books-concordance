using application.controllers;
using books.business_logic;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace application {
  public partial class DocumentsQueryDialog : Form {
    //--------------------------------------------------------------------------
    private DocumentsQueryController _controller = null;

    // used for words query
    public List<Document> SelectedDocuments {
      get;
      set;
    }

    //--------------------------------------------------------------------------
    public DocumentsQueryDialog(BusinessLogic businessLogic, 
                                bool withNextButton) {
      InitializeComponent();
      SelectedDocuments = new List<Document>();
      
      _controller = new DocumentsQueryController(this, 
                                                 businessLogic, 
                                                 withNextButton);
    }

    //--------------------------------------------------------------------------
    private void checkBoxGutenbergId_CheckedChanged(object sender, 
                                                    EventArgs e) {
      textBoxGutenbergId.Enabled = checkBoxGutenbergId.Checked;
    }

    //--------------------------------------------------------------------------
    private void checkBoxTitle_CheckedChanged(object sender, EventArgs e) {
      textBoxTitle.Enabled = checkBoxTitle.Checked;
    }

    //--------------------------------------------------------------------------
    private void checkBoxAuthor_CheckedChanged(object sender, EventArgs e) {
      textBoxAuthor.Enabled = checkBoxAuthor.Checked;
    }

    //--------------------------------------------------------------------------
    private void checkBoxReleaseDate_CheckedChanged(object sender, 
                                                    EventArgs e) {
      dateTimePicker.Enabled = checkBoxReleaseDate.Checked;
    }

    //--------------------------------------------------------------------------
    private void btnAddWord_Click(object sender, EventArgs e) {
      _controller.AddWord();
    }

    //-------------------------------------------------------------------------
    private void btnRemoveWord_Click(object sender, EventArgs e) {
      _controller.RemoveSelectedWords();
    }

    //-------------------------------------------------------------------------
    private void textBoxWord_KeyPress(object sender, KeyPressEventArgs e) {
      if (e.KeyChar == 13) {
        _controller.AddWord();
      }
    }

    //-------------------------------------------------------------------------
    private void listBoxWords_KeyUp(object sender, KeyEventArgs e) {
      if (e.KeyCode == Keys.Delete) {
        _controller.RemoveSelectedWords();
      }
    }

    //-------------------------------------------------------------------------
    private void btnQuery_Click(object sender, EventArgs e) {
      _controller.PerformQuery();
    }

    //-------------------------------------------------------------------------
    private void listViewDocuments_MouseDoubleClick(
      object sender, MouseEventArgs e) {
      _controller.OpenSelectedDocument();
    }

    //-------------------------------------------------------------------------
    private void btnNext_Click(object sender, EventArgs e) {
      _controller.SetSelectedDocumentsAndClose();
    }
  }
}
