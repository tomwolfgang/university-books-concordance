using System;
using System.Collections.Generic;
using document_parser.models;
using System.Text.RegularExpressions;

namespace document_parser {
  //-------------------------------------------------------------------------
  /// <summary>
  /// This class parses meta data from sentences and stores them for later
  /// access.  This means that the class has a state!
  /// </summary>
  class MetaDataParser {

    #region Constants
    //-------------------------------------------------------------------------
    public const string kGutendbergId = "gutenbergid";
    #endregion

    #region Member variables

    //-------------------------------------------------------------------------
    /// <summary>
    /// the "state" - the fields that were extracted thus far and are also
    /// accessible to the caller (via the MetaData property)
    /// </summary>
    private Dictionary<String, String> _metaDataFields =
      new Dictionary<String, String>();

    //-------------------------------------------------------------------------
    /// <summary>
    /// a white-listed key is a key that we don't "cleanup" it's values.
    /// a non-whitelisted key will have different cleanups that we will perform
    /// according to a (small) emprical review of Gutenberg books
    /// </summary>
    private Dictionary<String, bool> _whitelistedKeys =
      new Dictionary<String, bool>() {
        { "author", true },
        { "title", true }
    };

    #endregion

    #region Properties

    //-------------------------------------------------------------------------
    /// <summary>
    /// This is how you get the actual meta data field-value pairs
    /// </summary>
    public Dictionary<String, String> Fields {
      get {
        return _metaDataFields;
      }
    }

    #endregion

    #region Public methods

    //-------------------------------------------------------------------------
    /// <summary>
    /// constructor
    /// </summary>
    public MetaDataParser() {
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// 
    /// Examples:
    /// Title: Pride and Prejudice
    ///
    /// Title: The Defence of Lucknow
    ///    A Diary Recording the Daily Events during the Siege of the...
    /// 
    /// Author: Jane Austen
    /// Posting Date: August 26, 2008 [EBook #1342]
    /// Release Date: June, 1998
    /// Last updated: February 15, 2015]
    /// [Last updated: December 20, 2011]
    /// Language: English
    /// 
    /// 
    /// Sometimes Stanford will return a sentence with multiple (Key: Value)
    /// pairs - so we will not use Stanford word parsing here
    /// </summary>
    /// <param name="sentence"></param>
    public void Parse(InternalSentence sentence) {
      // first, we split the sentence into lines - this will help tackling both
      // multi-line values (like the Title: The Defence of...)
      // and multi-field sentences - like the dates example above
      //
      // we handle both Windows and Unix style newline
      var splitData = sentence.text.Split(
        new string[] { Environment.NewLine }, StringSplitOptions.None);

      int index = 0;
      while (index < splitData.Length) {
        string currentItem = splitData[index];
        // try to get Gutenberg id: [EBook #id]
        TryExtractingGutenbergId(currentItem);

        TrimBrackets(ref currentItem);

        // extract (key, value) pair
        string key = "", value = "";
        if (!ExtractKeyValue(currentItem, ref key, ref value)) {
          index++;
          continue;
        }

        if (_whitelistedKeys.ContainsKey(key.ToLower())) {
          int linesAdded;
          AppendNextLinesToValue(splitData, index, ref value, out linesAdded);
          index += linesAdded;
        } else {
          // if it isn't a whitelisted key - we need ot clean up values
          // according to what we know we don't need (by reviewing documents)
          CleanUpValue(ref value);
        }

        index++;
        // add to member variable
        _metaDataFields.Add(key.ToLower(), value);
      }
    }

    #endregion

    #region Private methods

    //-------------------------------------------------------------------------
    /// <summary>
    /// remove prefix [ and suffix ]
    /// </summary>
    /// <param name="data"></param>
    private void TrimBrackets(ref string data) {
      if (data.StartsWith("[")) {
        data = data.Substring(1);
      }

      if (data.EndsWith("]")) {
        data = data.Substring(0, data.Length - 1);
      }
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// returns true if we managed to extract a (key, value) pair, otherwise 
    /// false is returned.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private bool ExtractKeyValue(
      string data, ref string key, ref string value) {
      key = value = "";

      Regex rgx = new Regex(@"(.*)\:(.*)", RegexOptions.IgnoreCase);
      MatchCollection matches = rgx.Matches(data);

      // should only have one match
      if (matches.Count != 1) {
        return false;
      }

      // we expect 3 matches: the full sentence, the key and the value
      if (matches[0].Groups.Count != 3) {
        return false;
      }

      key = matches[0].Groups[1].Value.Trim();
      value = matches[0].Groups[2].Value.Trim();
      return true;
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// cleanup values from parts we don't care about - currently we cleanup:
    /// 1. Posting Date: August 26, 2008 [EBook #1342] - the [EBook... part
    /// </summary>
    /// <param name="value"></param>
    private void CleanUpValue(ref string value) {
      int find = value.IndexOf('[');
      if (find > 0) {
        value = value.Substring(0, find);
        value = value.Trim();
      }
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// for whitelisted keys - the value may span multiple lines, so we iterate 
    /// the following lines until reaching another pair or an empty line 
    /// (which means we've reached double new lines)
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="currentIndex"></param>
    /// <param name="value"></param>
    /// <param name="linesAdded"></param>
    private void AppendNextLinesToValue(
      string[] lines, int currentIndex, ref string value, out int linesAdded) {
      linesAdded = 0;

      string tempKey = "", tempVal = "";
      int tempIndex = currentIndex + 1;

      while ((tempIndex < lines.Length) &&
             (lines[tempIndex].Trim().Length > 0) &&
             (!ExtractKeyValue(lines[tempIndex], ref tempKey, ref tempVal))) {
        value += Environment.NewLine + lines[tempIndex];
        linesAdded++;
        tempIndex++;
      }
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// Here we try to extract the Gutenberg Id from the document.
    /// Some examples:
    /// Posting Date: June 25, 2008 [EBook #11] --> 11
    /// Release Date: September 19, 2016  [eBook #53093] --> 53093
    /// Release Date: August 20, 2006 [EBook #74] --> 74
    /// </summary>
    private void TryExtractingGutenbergId(string data) {
      // try to get Gutenberg id: [EBook #id]
      Regex rgx = new Regex(@"\[ebook #(\d+)\]", RegexOptions.IgnoreCase);
      MatchCollection matches = rgx.Matches(data);

      // should only have one match
      if (matches.Count != 1) {
        return;
      }

      // we expect 2 matches: the full sentence and the id
      if (matches[0].Groups.Count != 2) {
        return;
      }

      string gutendbergId = matches[0].Groups[1].Value.Trim();

      // add to the meta-data
      _metaDataFields.Add(kGutendbergId, gutendbergId);
    }

    #endregion
  }
}
