using books.business_logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using books.business_logic.models;
using System.Globalization;

namespace application.controllers {
  class StatisticsController : StatisticsDelegate {
    //-------------------------------------------------------------------------
    private StatisticsDialog _parentForm = null;
    private BusinessLogic _businessLogic = null;

    private bool _retreivingStats = false;

    //-------------------------------------------------------------------------
    public StatisticsController(
      StatisticsDialog parentForm,
      BusinessLogic businessLogic) {
      _parentForm = parentForm;
      _businessLogic = businessLogic;
    }

    //-------------------------------------------------------------------------
    public void Initialize() {
      _parentForm.listViewWordFrequencies.Items.Clear();
      UpdateWordFrequenciesCount();
      _parentForm.listViewWordFrequencies.Items.Add("Retreiving ...");

      _retreivingStats = true;

      _businessLogic.Stats.GetAll(this, (status, message) => {
        if (String.IsNullOrEmpty(message)) {
          _retreivingStats = false;
          return;
        }

        _parentForm.Invoke((MethodInvoker)delegate {
          System.Windows.Forms.MessageBox.Show(message, "Error");
          _retreivingStats = false;
        });
      });
    }

    //-------------------------------------------------------------------------
    public void InspectWord(Word word) {
      WordsInspectorDialog wordInspector = new WordsInspectorDialog(
        _businessLogic, null, false, word);
      wordInspector.ShowDialog();
    }

    //-------------------------------------------------------------------------
    public bool FormClosing() {
      // true to cancel closing, false to allow it
      return _retreivingStats;
    }

    //--------------------------------------------------------------------------
    public void OnAvgWordsPerLine(double value) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.lblWordsPerLine.Text = "Per Line: " +
                      value.ToString("N2", CultureInfo.InvariantCulture);
      });
    }

    //--------------------------------------------------------------------------
    public void OnAvgWordsPerSentence(double value) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.lblWordsPerSentence.Text = "Per Sentence: " +
                      value.ToString("N2", CultureInfo.InvariantCulture);
      });
    }

    //--------------------------------------------------------------------------
    public void OnAvgWordsPerParagraph(double value) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.lblWordsPerParagraph.Text = "Per Paragraph: " +
                      value.ToString("N2", CultureInfo.InvariantCulture);
      });
    }

    //--------------------------------------------------------------------------
    public void OnAvgWordsPerPage(double value) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.lblWordsPerPage.Text = "Per Page: " +
                      value.ToString("N2", CultureInfo.InvariantCulture);
      });
    }

    //--------------------------------------------------------------------------
    public void OnAvgWordsPerDocument(double value) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.lblWordsPerDocument.Text = "Per Document: " +
                      value.ToString("N2", CultureInfo.InvariantCulture);
      });
    }

    //--------------------------------------------------------------------------
    public void OnAvgCharsPerLine(double value) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.lblCharsPerLine.Text = "Per Line: " +
                      value.ToString("N2", CultureInfo.InvariantCulture);
      });
    }

    //--------------------------------------------------------------------------
    public void OnAvgCharsPerSentence(double value) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.lblCharsPerSentence.Text = "Per Sentence: " +
                      value.ToString("N2", CultureInfo.InvariantCulture);
      });
    }

    //--------------------------------------------------------------------------
    public void OnAvgCharsPerParagraph(double value) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.lblCharsPerParagraph.Text = "Per Paragraph: " +
                      value.ToString("N2", CultureInfo.InvariantCulture);
      });
    }

    //--------------------------------------------------------------------------
    public void OnAvgCharsPerPage(double value) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.lblCharsPerPage.Text = "Per Page: " +
                      value.ToString("N2", CultureInfo.InvariantCulture);
      });
    }

    //--------------------------------------------------------------------------
    public void OnAvgCharsPerDocument(double value) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.lblCharsPerDocument.Text = "Per Document: " +
                      value.ToString("N2", CultureInfo.InvariantCulture);
      });
    }

    //-------------------------------------------------------------------------
    public void OnWordFrequenciesReady(List<Tuple<Word, uint>> frequencies) {
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.listViewWordFrequencies.Items.Clear();

        int rank = 1;
        foreach (var item in frequencies) {
          ListViewItem listViewItem = new ListViewItem();
          listViewItem.Text = item.Item1.Value;
          listViewItem.SubItems.Add(rank.ToString(
            "N0",
            CultureInfo.InvariantCulture));
          listViewItem.SubItems.Add(item.Item2.ToString(
            "N0",
            CultureInfo.InvariantCulture));
          listViewItem.Tag = item.Item1;
          _parentForm.listViewWordFrequencies.Items.Add(listViewItem);

          rank++;
        }

        UpdateWordFrequenciesCount();
      });
    }

    //--------------------------------------------------------------------------
    private void UpdateWordFrequenciesCount() {
      _parentForm.groupBoxTopWords.Text = String.Format(
        "Top Words ({0}):",
        _parentForm.listViewWordFrequencies.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }
  }
}
