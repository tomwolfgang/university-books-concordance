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
  public partial class WordsQueryDialog : Form {
    //--------------------------------------------------------------------------
    private WordsQueryController _controller = null;

    //--------------------------------------------------------------------------
    public WordsQueryDialog(BusinessLogic businessLogic) {
      InitializeComponent();

      _controller = new WordsQueryController(this, businessLogic);
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
    private void checkBoxLine_CheckedChanged(object sender, EventArgs e) {
      textBoxLine.Enabled = checkBoxLine.Checked;
    }

    //--------------------------------------------------------------------------
    private void checkBoxLineIndex_CheckedChanged(object sender, EventArgs e) {
      textBoxSentenceIndex.Enabled = checkBoxSentenceIndex.Checked;
    }

    //--------------------------------------------------------------------------
    private void checkBoxPage_CheckedChanged(object sender, EventArgs e) {
      textBoxPage.Enabled = checkBoxPage.Checked;
    }

    //--------------------------------------------------------------------------
    private void checkBoxSentence_CheckedChanged(object sender, EventArgs e) {
      textBoxSentence.Enabled = checkBoxSentence.Checked;
    }

    //--------------------------------------------------------------------------
    private void checkBoxParagraph_CheckedChanged(object sender, EventArgs e) {
      textBoxParagraph.Enabled = checkBoxParagraph.Checked;
    }

    //--------------------------------------------------------------------------
    private void btnQuery_Click(object sender, EventArgs e) {
      try {
        _controller.PerformQuery();
      } catch(Exception ex) {
        System.Windows.Forms.MessageBox.Show(ex.Message, "Error");
      }
    }

    //--------------------------------------------------------------------------
    private void listViewLocations_DoubleClick(object sender, EventArgs e) {
      if (listViewLocations.SelectedItems.Count <= 0) {
        return;
      }

      ListViewItem item = listViewLocations.SelectedItems[0];
      _controller.LocationDoubleClicked((LocationDetail)item.Tag);
    }
  }
}
