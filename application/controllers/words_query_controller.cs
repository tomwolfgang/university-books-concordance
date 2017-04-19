using application.common;
using books.business_logic;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace application.controllers {
  class WordsQueryController {
    //-------------------------------------------------------------------------
    private WordsQueryDialog _parentForm = null;
    private BusinessLogic _businessLogic = null;
    private LoadingScreenDialog _currentLoadingScreen = null;

    //-------------------------------------------------------------------------
    public WordsQueryController(
      WordsQueryDialog parentForm,
      BusinessLogic businessLogic) {
      _parentForm = parentForm;
      _businessLogic = businessLogic;
    }

    //--------------------------------------------------------------------------
    public void PerformQuery() {
      List<DocumentProperty> documentProperties = 
        BuildDocumentPropertiesForQuery();

      List<WordLocationProperty> wordLocationProperties =
        BuildWordLocationPropertiesForQuery();

      string queryMessageResult = "";
      _parentForm.richTextBoxContents.Text = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Running Words Query ...", false);

      UpdateLocationsCount();

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Queries.GetWordsLocationDetails(
          documentProperties,
          wordLocationProperties,
          (wordLocationsDetails, status, message) => {
            queryMessageResult = message;

            if (String.IsNullOrEmpty(queryMessageResult)) {
              FillLocationsListOnUIThread(wordLocationsDetails);
            }

            _currentLoadingScreen.Invoke((MethodInvoker)delegate {
              _currentLoadingScreen.CloseScreen();
            });
          }); // GetWordsLocationDetails
      });

      _currentLoadingScreen.ShowDialog();
      _currentLoadingScreen = null;

      // check results
      if (!String.IsNullOrEmpty(queryMessageResult)) {
        System.Windows.Forms.MessageBox.Show(queryMessageResult, "Error");
        return;
      }
    }

    //--------------------------------------------------------------------------
    private List<DocumentProperty> BuildDocumentPropertiesForQuery() {
      List<DocumentProperty> result = new List<DocumentProperty>();

      // gutenberg id
      if (_parentForm.checkBoxGutenbergId.Checked) {
        result.Add(new DocumentProperty() {
          Field = DocumentProperty.Property.GutenbergId,
          StrValue = _parentForm.textBoxGutenbergId.Text
        });
      }

      // title
      if (_parentForm.checkBoxTitle.Checked) {
        result.Add(new DocumentProperty() {
          Field = DocumentProperty.Property.Title,
          StrValue = _parentForm.textBoxTitle.Text
        });
      }

      // author
      if (_parentForm.checkBoxAuthor.Checked) {
        result.Add(new DocumentProperty() {
          Field = DocumentProperty.Property.Author,
          StrValue = _parentForm.textBoxAuthor.Text
        });
      }

      // release date
      if (_parentForm.checkBoxReleaseDate.Checked) {
        result.Add(new DocumentProperty() {
          Field = DocumentProperty.Property.ReleaseDate,
          DateTimeValue = _parentForm.dateTimePicker.Value
        });
      }

      return result;
    }

    //--------------------------------------------------------------------------
    public void LocationDoubleClicked(LocationDetail location) {
      string queryMessageResult = "";

      _currentLoadingScreen = new LoadingScreenDialog(
        "Retreiving Contents from Document ...", false);

      Task.Run(() => {
        System.Threading.Thread.Sleep(200);

        _businessLogic.Queries.GetContents(
          location,
          (contents, wordOffsetBegin, wordOffsetEnd, status, message) => {
            queryMessageResult = message;

            if (String.IsNullOrEmpty(queryMessageResult)) {
              _parentForm.Invoke((MethodInvoker)delegate {
                UIUtils.SetRichTextContents(_parentForm.richTextBoxContents,
                                            contents,
                                            wordOffsetBegin,
                                            wordOffsetEnd);
              });
            }

            _currentLoadingScreen.Invoke((MethodInvoker)delegate {
              _currentLoadingScreen.CloseScreen();
            });
          }); // GetContents
      });

      // this line waits until _currentLoadingScreen.CloseScreen() is called
      _currentLoadingScreen.ShowDialog();
      _currentLoadingScreen = null;

      // check results
      if (!String.IsNullOrEmpty(queryMessageResult)) {
        System.Windows.Forms.MessageBox.Show(queryMessageResult, "Error");
        return;
      }
    }

    //--------------------------------------------------------------------------
    private List<WordLocationProperty> BuildWordLocationPropertiesForQuery() {
      List<WordLocationProperty> result = new List<WordLocationProperty>();

      // line
      if (_parentForm.checkBoxLine.Checked) {
        int val = 0;
        if (!Int32.TryParse(_parentForm.textBoxLine.Text, out val)) {
          _parentForm.textBoxLine.Select();
          throw new Exception("The line filter must be a valid numeric value");
        }

        result.Add(new WordLocationProperty() {
          Field = WordLocationProperty.Property.Line,
          Value = val - 1 // we are 0-based
        });
      }

      // line
      if (_parentForm.checkBoxSentenceIndex.Checked) {
        int val = 0;
        if (!Int32.TryParse(_parentForm.textBoxSentenceIndex.Text, out val)) {
          _parentForm.textBoxSentenceIndex.Select();
          throw new Exception("The line filter must be a valid numeric value");
        }

        result.Add(new WordLocationProperty() {
          Field = WordLocationProperty.Property.SentenceIndex,
          Value = val - 1 // we are 0-based
        });
      }

      // page
      if (_parentForm.checkBoxPage.Checked) {
        int val = 0;
        if (!Int32.TryParse(_parentForm.textBoxPage.Text, out val)) {
          _parentForm.textBoxPage.Select();
          throw new Exception("The page filter must be a valid numeric value");
        }

        result.Add(new WordLocationProperty() {
          Field = WordLocationProperty.Property.Page,
          Value = val - 1 // we are 0-based
        });
      }

      // sentence
      if (_parentForm.checkBoxSentence.Checked) {
        int val = 0;
        if (!Int32.TryParse(_parentForm.textBoxSentence.Text, out val)) {
          _parentForm.textBoxSentence.Select();
          throw new Exception(
            "The sentence filter must be a valid numeric value");
        }

        result.Add(new WordLocationProperty() {
          Field = WordLocationProperty.Property.Sentence,
          Value = val - 1 // we are 0-based
        });
      }

      // paragraph
      if (_parentForm.checkBoxParagraph.Checked) {
        int val = 0;
        if (!Int32.TryParse(_parentForm.textBoxParagraph.Text, out val)) {
          _parentForm.textBoxParagraph.Select();
          throw new Exception(
            "The paragraph filter must be a valid numeric value");
        }

        result.Add(new WordLocationProperty() {
          Field = WordLocationProperty.Property.Paragraph,
          Value = val - 1 // we are 0-based
        });
      }

      return result;
    }

    //--------------------------------------------------------------------------
    private void FillLocationsListOnUIThread(
      List<WordLocationDetails> locations) {
      // do we have a documents filter?
      _parentForm.Invoke((MethodInvoker)delegate {
        _parentForm.listViewLocations.Items.Clear();

        foreach (WordLocationDetails item in locations) {
          foreach (LocationDetail location in item.LocationDetails) {
            //Document Id,Document Title,Word,Line,Index,Page,Sentence,Paragraph
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = location.Document.GutenbergId;
            listViewItem.SubItems.Add(location.Document.Title);
            
            listViewItem.SubItems.Add(item.Word.Value);

            listViewItem.SubItems.Add((location.Location.Line + 1).ToString(
              "N0",
              CultureInfo.InvariantCulture));
            listViewItem.SubItems.Add((location.Location.IndexInSentence + 1)
              .ToString("N0",CultureInfo.InvariantCulture));
            listViewItem.SubItems.Add((location.Location.Page + 1).ToString("N0",
              CultureInfo.InvariantCulture));

            listViewItem.SubItems.Add((location.Location.Sentence + 1).ToString(
              "N0",
              CultureInfo.InvariantCulture));
            listViewItem.SubItems.Add((location.Location.Paragraph + 1).ToString(
              "N0",
              CultureInfo.InvariantCulture));
            listViewItem.Tag = location;
            _parentForm.listViewLocations.Items.Add(listViewItem);
          }
        }

        UpdateLocationsCount();
      });
    }

    //--------------------------------------------------------------------------
    private void UpdateLocationsCount() {
      _parentForm.groupBoxLocations.Text = String.Format(
        "Documents ({0}):",
        _parentForm.listViewLocations.Items.Count.ToString(
          "N0",
          CultureInfo.InvariantCulture));
    }

  }
}
