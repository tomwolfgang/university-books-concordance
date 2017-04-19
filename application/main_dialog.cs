using application.controllers;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace application {
  //----------------------------------------------------------------------------
  public partial class MainDialog : Form {
    private MainController _controller = null;

    //--------------------------------------------------------------------------
    public MainDialog() {
      InitializeComponent();
      _controller = new MainController(this);
    }

    //--------------------------------------------------------------------------
    public void SetStats(Stats stats) {
      lblStatsDocuments.Text = String.Format(
        "Documents: {0}",
        stats.Documents.ToString("N0", CultureInfo.InvariantCulture));
      lblStatsUniqueWords.Text = String.Format(
        "Unique Words: {0}",
        stats.UniqueWords.ToString("N0", CultureInfo.InvariantCulture));
      lblStatsIndexedWords.Text = String.Format(
        "Indexed Words: {0}",
        stats.IndexedWords.ToString("N0", CultureInfo.InvariantCulture));
      lblStatsGroups.Text = String.Format(
        "Groups: {0}",
        stats.Groups.ToString("N0", CultureInfo.InvariantCulture));
      lblStatsRelations.Text = String.Format(
        "Relations: {0}",
        stats.Relations.ToString("N0", CultureInfo.InvariantCulture));      
    }

    //--------------------------------------------------------------------------
    private void Main_Shown(object sender, EventArgs e) {
      _controller.Initialize();
    }

    //--------------------------------------------------------------------------
    private void btnInsertDocuments_Click(object sender, EventArgs e) {
      _controller.InsertDocuments();
    }

    //--------------------------------------------------------------------------
    private void btnEditGroups_Click(object sender, EventArgs e) {
      _controller.EditGroups();
    }

    //--------------------------------------------------------------------------
    private void btnEditRelations_Click(object sender, EventArgs e) {
      _controller.EditRelations();
    }

    //--------------------------------------------------------------------------
    private void btnQueryDocuments_Click(object sender, EventArgs e) {
      _controller.QueryDocuments();
    }

    //--------------------------------------------------------------------------
    private void btnWordsInspector_Click(object sender, EventArgs e) {
      _controller.InspectWords();
    }

    //--------------------------------------------------------------------------
    private void btnGroupsInspector_Click(object sender, EventArgs e) {
      _controller.InspectGroups();
    }

    //--------------------------------------------------------------------------
    private void btnWordsQuery_Click(object sender, EventArgs e) {
      _controller.WordsQuery();
    }

    //--------------------------------------------------------------------------
    private void btnEditPhrases_Click(object sender, EventArgs e) {
      _controller.EditPhrases();
    }

    //--------------------------------------------------------------------------
    private void btnPhrasesInspector_Click(object sender, EventArgs e) {
      _controller.InspectPhrases();
    }

    //--------------------------------------------------------------------------
    private void btnExport_Click(object sender, EventArgs e) {
      _controller.ExportDB();
    }

    //--------------------------------------------------------------------------
    private void btnImport_Click(object sender, EventArgs e) {
      _controller.ImportDB();
    }

    //--------------------------------------------------------------------------
    private void btnDropDB_Click(object sender, EventArgs e) {
      _controller.ResetDB();
    }

    private void linkLabel1_LinkClicked(object sender, 
                                        LinkLabelLinkClickedEventArgs e) {
      _controller.MoreStats();
    }
  }
}
