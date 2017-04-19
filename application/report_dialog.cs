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
  public partial class ReportDialog : Form {
    //--------------------------------------------------------------------------
    public ReportDialog(Dictionary<string,string> documentsStatus) {
      InitializeComponent();

      foreach (string key in documentsStatus.Keys) {
        string status = documentsStatus[key];
        if (String.IsNullOrEmpty(status)) {
          status = "OK";
        }

        listView.Items.Add(key).SubItems.Add(status);
      }
    }
  }
}
