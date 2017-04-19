using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.common {
  class UIUtils {
    //--------------------------------------------------------------------------
    public static void SetRichTextContents(
      System.Windows.Forms.RichTextBox richTextBox,
      string contents,
      long wordOffsetBegin,
      long wordOffsetEnd) {

      richTextBox.SelectionColor = richTextBox.ForeColor;
      richTextBox.SelectionBackColor = richTextBox.BackColor;

      richTextBox.Text = contents;

      // highlight the word

      // first, we need to solve a problem with the richtext control that 
      // removes linefeeds (if we have CRLF it turns it into CR)
      string before = contents.Substring(0, (int)wordOffsetBegin);
      int lineFeedsBeforeBegin = 0;
      for (int i = 0; i < before.Length; ++i) {
        if (before[i] == (char)13) {
          lineFeedsBeforeBegin++;
        }
      }

      // next, we might have a CRLF that turns into a CR between 
      // wordOffsetBegin and wordOffsetEnd (if we are handling a phrase)
      string phrase = contents.Substring(
        (int)wordOffsetBegin, (int)wordOffsetEnd - (int)wordOffsetBegin);
      int lineFeedsBetweenOffsets = 0;
      for (int i = 0; i < phrase.Length; ++i) {
        if (phrase[i] == (char)13) {
          lineFeedsBetweenOffsets++;
        }
      }

      richTextBox.Select(
        (int)wordOffsetBegin - lineFeedsBeforeBegin,
        (int)wordOffsetEnd - ((int)wordOffsetBegin + lineFeedsBetweenOffsets));

      richTextBox.SelectionColor = System.Drawing.Color.White;
      richTextBox.SelectionBackColor = System.Drawing.Color.Blue;

      richTextBox.ScrollToCaret();
    }
  }
}
