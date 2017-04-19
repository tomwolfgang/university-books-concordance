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
  public partial class PhrasesInspectorDialog : Form {
    //--------------------------------------------------------------------------
    private PhrasesInspectorController _controller = null;

    //--------------------------------------------------------------------------
    public PhrasesInspectorDialog(BusinessLogic businessLogic) {
      InitializeComponent();

      _controller = new PhrasesInspectorController(this, businessLogic);
    }

    //--------------------------------------------------------------------------
    private void PhrasesInspectorDialog_Shown(object sender, EventArgs e) {
      _controller.Initialize();
    }

    //--------------------------------------------------------------------------
    private void listViewPhrases_DoubleClick(object sender, EventArgs e) {
      if (listViewPhrases.SelectedItems.Count <= 0) {
        return;
      }

      Phrase phrase = (Phrase)listViewPhrases.SelectedItems[0].Tag;
      _controller.QueryPhrase(phrase);
    }

    //--------------------------------------------------------------------------
    private void listViewLocations_DoubleClick(object sender, EventArgs e) {
      if (listViewLocations.SelectedItems.Count <= 0) {
        return;
      }

      _controller.GetLocationItemContents(listViewLocations.SelectedItems[0]);
    }
  }
}
