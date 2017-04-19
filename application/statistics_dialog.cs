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
  public partial class StatisticsDialog : Form {
    private StatisticsController _controller = null;

    //--------------------------------------------------------------------------
    public StatisticsDialog(BusinessLogic businessLogic) {
      InitializeComponent();
      _controller = new StatisticsController(this, businessLogic);
    }

    //--------------------------------------------------------------------------
    private void StatisticsDialog_Shown(object sender, EventArgs e) {
      _controller.Initialize();
    }

    //--------------------------------------------------------------------------
    private void listViewWordFrequencies_DoubleClick(object sender, 
                                                     EventArgs e) {
      if (listViewWordFrequencies.SelectedItems.Count <= 0) {
        return;
      }

      Word word = (Word)listViewWordFrequencies.SelectedItems[0].Tag;
      _controller.InspectWord(word);
    }

    private void StatisticsDialog_FormClosing(object sender, 
                                              FormClosingEventArgs e) {
      e.Cancel = _controller.FormClosing();
    }
  }
}
