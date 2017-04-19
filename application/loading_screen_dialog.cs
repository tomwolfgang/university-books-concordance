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
  //-------------------------------------------------------------------------
  public partial class LoadingScreenDialog : Form {

    public bool CancelClicked {
      get;
      private set;
    }

    // block F4 closing
    private bool _allowClose = false;

    //-------------------------------------------------------------------------
    public LoadingScreenDialog(string initializationMessage, 
                               bool showCancelButton = false) {
      CancelClicked = false;
      InitializeComponent();

      btnCancel.Visible = showCancelButton;
      lblLoadingMessage.Text = initializationMessage;
    }

    //-------------------------------------------------------------------------
    public void CloseScreen() {
      _allowClose = true;
      this.Close();
    }

    //-------------------------------------------------------------------------
    public void SetProgress(int percent) {
      progressBar.Visible = true;
      progressBar.Value = percent;
    }

    public void SetLabel(string label) {
      lblLoadingMessage.Text = label;
    }


    private void LoadingScreen_FormClosing(
      object sender, FormClosingEventArgs e) {
      e.Cancel = !_allowClose;
    }

    private void btnCancel_Click(object sender, EventArgs e) {
      CancelClicked = true;
      btnCancel.Enabled = false;
    }
  }
}
