using document_parser.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace document_parser {
  class QualifiedWords {

    #region Member variables

    /// <summary>
    /// These suffixes are ignorable
    /// </summary>
    private static HashSet<string> _ignorableSuffixes =
      new HashSet<string>() {
      "'s",
      "n't",
      "'d", // he'd
      "'ve", // I've
      "'re", // You're
      "'ll", // I'll
      "'m" // I'm
    };

    /// <summary>
    /// black list words that are comprised only of these chars
    /// </summary>
    private static string _blacklistRepeatedCharsRegExp =
      "^['*._:;/ \"\\-\\’\\“\\?\\!\\]\\[\\(\\)\\{\\}]+$";

    /// <summary>
    /// Here we collect unknown suffixes so that we can later analyze them
    /// </summary>
    private HashSet<string> _unknownSuffixes = new HashSet<string>();
    #endregion

    #region Properties
    public HashSet<string> UnknownSuffixes {
      get {
        return _unknownSuffixes;
      }
    }

    #endregion

    #region Public functions

    public bool Check(
      InternalSentence sentence, InternalWord word, int wordInSentence) {
      if (!PassesOneCharAlphaNumericCheck(word)) {
        return false;
      }

      if (!PassesSuffixCheck(sentence, word, wordInSentence)) {
        return false;
      }

      if (!PassesBlacklistRepeatedCharsCheck(word)) {
        return false;
      }

      return true;
    }

    #endregion

    #region Private functions

    /// <summary>
    /// check non-alpha-numerics
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    private bool PassesOneCharAlphaNumericCheck(InternalWord word) {
      // is it one character long?
      // we use file offsets to ignore annotations from Stanford
      if ((word.characterOffsetEnd - word.characterOffsetBegin) != 1) {
        return true;
      }

      // if it is a single character - check if it is an alphanumeric char
      Regex rgx = new Regex(@"^[a-zA-Z0-9]$", RegexOptions.IgnoreCase);
      bool ret = rgx.IsMatch(word.originalText);
      return ret;
    }

    /// <summary>
    /// Check if this word is only a suffix for a qualified word.  
    /// We and ignore them - for example:
    /// Tom's house...
    /// Tom couldn't (n't)
    /// You're
    /// 
    /// NOTE: we might want to make a specific suffix part of the last
    /// word (Could + n't = Couldn't) - in that case, we need to change
    /// the |Check| function altogether
    /// </summary>
    /// <param name="sentence"></param>
    /// <param name="word"></param>
    /// <param name="wordInSentence"></param>
    /// <returns></returns>
    private bool PassesSuffixCheck(
      InternalSentence sentence, InternalWord word, int wordInSentence) {

      // first, get the last word
      int lastWordIndex = wordInSentence - 1;
      if (lastWordIndex < 0) {
        // first word of the sentence
        return true;
      }

      InternalWord lastWord = sentence.words[lastWordIndex];
      // check there are no spaces/characters between the last word and the 
      // suffix
      if (lastWord.characterOffsetEnd != word.characterOffsetBegin) {
        return true;
      }

      // is this an ignorable suffix?
      string lowerCaseWord = word.annotatedText.ToLower();
      if (_ignorableSuffixes.Contains(lowerCaseWord)) {
        return false; // we want to ignore it
      }

      // don't ignore it - add to unkonwn list only if it contains
      // a non-alphanumeric character
      Regex rgx = new Regex(@"[a-z0-9]+", RegexOptions.IgnoreCase);
      if (!rgx.IsMatch(lowerCaseWord)) {
        _unknownSuffixes.Add(lowerCaseWord);
      }

      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sentence"></param>
    /// <param name="word"></param>
    /// <param name="wordInSentence"></param>
    /// <returns></returns>
    private bool PassesBlacklistRepeatedCharsCheck(InternalWord word) {
      string lowerCaseWord = word.annotatedText.ToLower();
      Regex rgx = new Regex(_blacklistRepeatedCharsRegExp, 
                            RegexOptions.IgnoreCase);
      if (rgx.IsMatch(lowerCaseWord)) {
        return false;
      }

      return true;
    }

    #endregion
  }
}
