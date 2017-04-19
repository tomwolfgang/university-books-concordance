using System;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using edu.stanford.nlp.process;
using System.IO;
using System.Text;
using edu.stanford.nlp.io;

namespace document_parser.stanford {
  /// <summary>
  /// Wraps initialization of stanford.CoreNLP
  /// First thing to do is call: |Initialize| so that we startup the library 
  /// and we only want to startup once per program 
  /// (process)
  /// </summary>
  sealed class StanfordPipelineService {
    // member variables
    private static StanfordPipelineService _instance = 
      new StanfordPipelineService();

    private StanfordCoreNLP _pipeLine = null;

    /// <summary>
    /// singleton - so we block the constructor
    /// </summary>
    private StanfordPipelineService() {
    }

    static StanfordPipelineService() { 
    }

    /// <summary>
    /// Get singelton instance
    /// </summary>
    public static StanfordPipelineService Instance {
      get {
        return _instance;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool Initialized {
      get {
        return (_pipeLine != null);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool Initialize() {
      if (this.Initialized) {
        return true;
      }

      var props = new java.util.Properties();
      // we tokenize and sentece split
      props.setProperty("annotators", "tokenize, ssplit");

      // don't separate words only when whitespace is encountered
      // e.g. ***THIS IS... = *** + THIS + IS...
      props.setProperty("tokenize.options", "whitespace=false");

      // totally ignores parentheses and brackets - we don't care for them
      // NOTE (twolf): this doesn't really work - so we take care of it in
      // |QualifiedWords|
      props.setProperty("tokenize.options", "normalizeOtherBrackets=false");
      props.setProperty("tokenize.options", "normalizeParentheses=false");

      // version 3.7.0 of CoreNLP supports splitting hypentated words
      // yet has a whitelist of hyphenated words - so we let it do the work
      // for us and refrain from really long words like:
      // pg345.txt: two-pages-to-the-week-with-Sunday-squeezed-in-a-corner
      props.setProperty("tokenize.options", "splitHyphenated=true");
   

      // two or more newlines should be treated as a sentece break
      // this is especially important for tables of contents
      props.setProperty(
        StanfordCoreNLP.NEWLINE_IS_SENTENCE_BREAK_PROPERTY, "two");

      // ignore
      props.setProperty("ssplit.tokenPatternsToDiscard", "\\p{Punct}");

      try {
        _pipeLine = new StanfordCoreNLP(props);
      } catch (Exception) {
        _pipeLine = null;
      }

      return this.Initialized;
    }

    /// <summary>
    /// Accepts a document (text) file, lets CoreNLP process (annotate) it
    /// and returns an adapter that allows iterating the output
    /// 
    /// also, the text file (normalized) is returned
    /// </summary>
    /// <param name="file"></param>
    /// <returns>returns null if fails</returns>
    public StanfordDocumentFacade Annotate(FileInfo file) {
      if (!this.Initialized) {
        return null;
      }

      try {
        string data;
        NormalizeFile(file, out data);
        var annotation = new edu.stanford.nlp.pipeline.Annotation(data);
        _pipeLine.annotate(annotation);
        return new StanfordDocumentFacade(annotation, data);
      } catch (Exception ex) {
        throw ex;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phrase"></param>
    /// <returns></returns>
    public StanfordDocumentFacade Annotate(string phrase) {
      if (!this.Initialized) {
        return null;
      }

      try {
        var annotation = new edu.stanford.nlp.pipeline.Annotation(phrase);
        _pipeLine.annotate(annotation);
        return new StanfordDocumentFacade(annotation, phrase);
      } catch (Exception ex) {
        throw ex;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private bool NormalizeFile(FileInfo file, out string data) {
      using (FileStream fs = File.OpenRead(file.FullName)) {
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);

        if (HighAsciiNormalization.RequiresNormalization(buffer)) {
          HighAsciiNormalization normalizer = new HighAsciiNormalization();
          data = normalizer.Normalize(ref buffer);
          return true;
        } else {
          data = File.ReadAllText(file.FullName);
          return false;
        }
      }
    }

  }
} // namespace business_logic
