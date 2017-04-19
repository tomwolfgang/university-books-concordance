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
  public partial class WordsInspectorDialog : Form {
    //--------------------------------------------------------------------------
    private WordsInspectorController _controller = null;

    //--------------------------------------------------------------------------
    public WordsInspectorDialog(BusinessLogic businessLogic, 
                                List<Document> documentsFilter,
                                bool showGroups = false,
                                Word word = null) {
      InitializeComponent();

      _controller = new WordsInspectorController(this, 
                                                 businessLogic, 
                                                 documentsFilter,
                                                 showGroups,
                                                 word);
    }

    //--------------------------------------------------------------------------
    private void WordsQuery_Shown(object sender, EventArgs e) {
      _controller.Initialize();
    }

    //--------------------------------------------------------------------------
    private void listViewWords_DoubleClick(object sender, EventArgs e) {
      if (listViewWords.SelectedItems.Count <= 0) {
        return;
      }

      ListViewItem item = listViewWords.SelectedItems[0];
      if (_controller.ShowGroups) {
        _controller.GroupDoubleClicked((Group)item.Tag);
      } else {
        _controller.WordDoubleClicked((Word)item.Tag);
      }
    }

    //--------------------------------------------------------------------------
    private void listViewWords_KeyPress(object sender, KeyPressEventArgs e) {
      if (e.KeyChar != (char)13) {
        return;
      }

      if (listViewWords.SelectedItems.Count <= 0) {
        return;
      }

      ListViewItem item = listViewWords.SelectedItems[0];
      _controller.WordDoubleClicked((Word)item.Tag);

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
