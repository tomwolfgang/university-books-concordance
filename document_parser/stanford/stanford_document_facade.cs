using System;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.util;
using document_parser.models;

namespace document_parser.stanford {
  /// <summary>
  /// Wrapper around sentences split with Stanford CoreNLP
  /// The idea is to translate sentences/words into native
  /// .NET objects so that it is easier to work with results
  /// from CoreNLP
  /// </summary>
  class StanfordDocumentFacade {
    private java.util.ArrayList _sentences = null;
    private string _rawData;

    // helper Java classes used with Stanford.  So we don't have to 
    // continually create new objects
    private java.lang.Class _textAnnotationClass =
      new CoreAnnotations.TextAnnotation().getClass();
    private java.lang.Class _originalTextAnnotationClass =
      new CoreAnnotations.OriginalTextAnnotation().getClass();
    private java.lang.Class _charOffsetBeginAnnotationClass =
      new CoreAnnotations.CharacterOffsetBeginAnnotation().getClass();
    private java.lang.Class _charOffsetEndAnnotationClass =
      new CoreAnnotations.CharacterOffsetEndAnnotation().getClass();
    private java.lang.Class _tokensAnnotationClass =
      new CoreAnnotations.TokensAnnotation().getClass();

    /// <summary>
    /// Constructor - receives an Annotation object
    /// </summary>
    /// <param name="annotation"></param>
    public StanfordDocumentFacade(Annotation annotation, string rawData) {
      _sentences = (java.util.ArrayList)annotation.get(
          new CoreAnnotations.SentencesAnnotation().getClass());
      _rawData = rawData;
    }

    /// <summary>
    /// This is a property that returns the number of setnences in the 
    /// document - use this to iterate each of the sentences using the
    /// |GetSentence| function
    /// </summary>
    public int SentencesCount {
      get {
        if (_sentences == null) {
          return 0;
        }

        return _sentences.size();
      }
    }

    /// <summary>
    /// </summary>
    public string RawData {
      get {
        return _rawData;
       }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetSentence(uint index, ref InternalSentence sentence) {
      if ((index < 0) ||
          (index > (this.SentencesCount - 1))) {
        throw new IndexOutOfRangeException();
      }

      // extract the basic sentence fields
      CoreMap stanfordSentence = (CoreMap)_sentences.get((int)index);
      if (!ExtractSentence(stanfordSentence, ref sentence)) {
        return false;
      }

      // extract words
      java.util.ArrayList stanfordWords =
        (java.util.ArrayList)stanfordSentence.get(_tokensAnnotationClass);

      if (stanfordWords.size() <= 0) {
        return true;
      }

      // a "minor" optimisation (capacity)
      sentence.words = new System.Collections.Generic.List<InternalWord>(
        stanfordWords.size());

      foreach (CoreMap stanfordWord in stanfordWords) {
        if (!ExtractWord(stanfordWord, ref sentence)) {
          // NOTE: currently, we fail the entire sentence even if a single
          // word fails
          return false;
        }
      };

      return true;
    }

    /// <summary>
    /// Extracts the sentence members of a Standford Sentence to a models
    /// sentence
    /// </summary>
    /// <param name="stanfordSentence"></param>
    /// <param name="sentence"></param>
    /// <returns></returns>
    private bool ExtractSentence(
      CoreMap stanfordSentence, ref InternalSentence sentence) {
      sentence.text = stanfordSentence.get(_textAnnotationClass).ToString();

      if (!UInt32.TryParse(
        stanfordSentence.get(_charOffsetBeginAnnotationClass).ToString(),
        out sentence.fileOffsetBegin)) {
        return false;
      }

      if (!UInt32.TryParse(
        stanfordSentence.get(_charOffsetEndAnnotationClass).ToString(),
        out sentence.fileOffsetEnd)) {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Extracts a Stanford-Word to a models.Word and adds to sentence
    /// </summary>
    /// <param name="stanfordWord"></param>
    /// <returns></returns>
    private bool ExtractWord(
      CoreMap stanfordWord, ref InternalSentence sentence) {
      uint offsetBegin, offsetEnd;
      if (!UInt32.TryParse(
        stanfordWord.get(_charOffsetBeginAnnotationClass).ToString(),
        out offsetBegin)) {
        return false;
      }

      if (!UInt32.TryParse(
        stanfordWord.get(_charOffsetEndAnnotationClass).ToString(),
        out offsetEnd)) {
        return false;
      }

      sentence.words.Add(new InternalWord() {
        annotatedText = stanfordWord.get(_textAnnotationClass).ToString(),
        //originalText = stanfordWord.get(_originalTextAnnotationClass).ToString(),
        originalText = _rawData.Substring((int)offsetBegin, (int)offsetEnd-(int)offsetBegin),
        characterOffsetBegin = offsetBegin,
        characterOffsetEnd = offsetEnd
      });

      return true;
    }

  } // class StanfordDocumentFacade
} // namespace business_logic
