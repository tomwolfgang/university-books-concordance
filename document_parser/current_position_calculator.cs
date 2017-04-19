using document_parser.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace document_parser {
  /// <summary>
  /// This class calculates the current line and paragraph we are on during the
  /// parsing of the document.  It does it by calculating the newlines between 
  /// two consecutive words.  
  /// 
  /// A single new line = line
  /// 2 or more new lines = 2 or more lines + 1 paragraph
  /// </summary>
  class CurrentPositionCalculator {
    #region Member variables

    private uint _logicalWordInSentence = 0;
    // internal means the index of the word in the InternalSentence, where as
    // logical means the calculated index of the word in the sentence.  For 
    // example, a sentenc like: Tom's apple would be broken down by Stanford
    // into three words: [Tom] ['s] [apple] - we need to keep track of the
    // internal index we are on (apple = 3) but return apple = 2 to the caller
    private uint _internalWordInSentence = 0;
    private uint _line = 0;
    private uint _sentence = 0;
    private uint _paragraph = 0;
    private uint _page = 0;

    // we use this to calculate the fileBytePosition of the words.  It allows us
    // to support utf8 encoded files
    private uint _currentByteOffset = 0;

    private string _rawData;
    private uint _linesPerPage;
    private InternalWord _lastWord;

    #endregion

    #region Properties

    public uint LogicalWord {
      get {
        return _logicalWordInSentence;
      }
    }

    public uint InternalWord {
      get {
        return _internalWordInSentence;
      }
    }

    public uint Line {
      get {
        return _line;
      }
    }

    public uint Sentence {
      get {
        return _sentence;
      }
    }

    public uint Paragraph {
      get {
        return _paragraph;
      }
    }

    public uint Page {
      get {
        return _page;
      }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Constructor requires the entire document string so that it can 
    /// calculate exact offsets
    /// </summary>
    /// <param name="rawData"></param>
    /// <param name="linesPerPage"></param>
    public CurrentPositionCalculator(string rawData, uint linesPerPage) {
      _rawData = rawData;
      _linesPerPage = linesPerPage;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="qualifiedWord"></param>
    public void IncreaseWord(bool qualifiedWord) {
      _internalWordInSentence++;
      
      if (qualifiedWord) {
        _logicalWordInSentence++;
      }
    }
    /// <summary>
    /// We don't have this information internally
    /// </summary>
    public void ResetWordInSentence() {
      _logicalWordInSentence = 0;
      _internalWordInSentence = 0;
    }

    /// <summary>
    /// We don't have this information internally
    /// </summary>
    public void IncreaseSentence() {
      _sentence++;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentWord"></param>
    /// <param name="qualifiedWord"></param>
    public void Calculate(InternalWord currentWord) {
      // start with the begining of the file/data
      int lastWordEndOffset = 0;

      if (_lastWord != null) {
        // if we had a word before, start with the end of
        // that word
        lastWordEndOffset = (int)_lastWord.characterOffsetEnd;
      }

      string delta = _rawData.Substring(
        lastWordEndOffset,
        (int)currentWord.characterOffsetBegin - lastWordEndOffset);

      _currentByteOffset += GetMultibyteOffset(delta);
      uint newLines = CountNewLines(delta);
      _line += newLines;

      // 2 or more newlines are treated as a new paragraph
      if (newLines > 1) {
        _paragraph++;
      }

      _currentByteOffset += GetMultibyteOffset(currentWord.originalText);

      _page = _line / _linesPerPage;
      _lastWord = currentWord;
    }

    /// <summary>
    // This is where we detect special chars in the word that are utf8 encoded
    // into a unicode word of a lesser length, and thus, create an offset in
    // the file location of each following word
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private uint GetMultibyteOffset(string text) {
      uint count = (uint)UTF8Encoding.UTF8.GetByteCount(text);
      if (count == text.Length) {
        return 0;
      }

      // the multibyte representation of the word is longer than the actual
      // Unicode word - so we have to remember this offset
      return count - (uint)text.Length;
    }

    internal void FillWord(ref DocumentWord word, InternalWord currentWord) {
      word.Text = currentWord.originalText;
      word.Page = _page;
      word.Paragraph = _paragraph;
      word.Sentence = _sentence;
      word.IndexInSentence = _logicalWordInSentence;
      word.Line = _line;

      // calculate file offset
      word.OffsetInFile = currentWord.characterOffsetBegin + _currentByteOffset;

      // if the offset is due to the current word - remove the offset part of
      // the current word.  We do this because we want to keep |FillWord| as 
      // something that doesn't changes the internal state of 
      // CurrentPositionCalculator - unlike the: |Calculate| method
      uint count = (uint)UTF8Encoding.UTF8.GetByteCount(
        currentWord.originalText);
      if (count > currentWord.originalText.Length) {
        word.OffsetInFile -= (uint)(count - currentWord.originalText.Length);
      }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Searches for newlines in a given string and returns
    /// the number of their appearances
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private uint CountNewLines(string data) {
      uint count = 0;
      foreach (var item in data) {
        if (item == '\n') {
          count++;
        }
      }

      return count;
    }

    #endregion

  }
}
