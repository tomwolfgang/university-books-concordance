using edu.stanford.nlp.io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace document_parser.stanford {
  /// <summary>
  /// I've noticed some documents we downloaded from Gutenberg (like 
  /// 74-0.txt - The Adventures of Tom Sawyer) are not UTF8 encoded, however,
  /// they use high-ascii chars for apostrophe and quotation marks.  
  /// This class will normalize all such chars to "low-ascii" chars
  /// and give a report on other unknown chars
  /// 
  /// NOTE: we do not change the length of the given document - only change
  /// single chars - otherwise, we might cause problems with calculating
  /// positions of words
  /// </summary>
  class HighAsciiNormalization {
    #region Member variables
    private HashSet<byte> _unknownChars = new HashSet<byte>();
    static private Dictionary<byte, byte> _normalizationDictionary
      = new Dictionary<byte, byte>() {
      { 0x91, 0x27 }, // apostrophe
      { 0x92, 0x27 }, // apostrophe
      { 0x93, 0x22 }, // quotation
      { 0x94, 0x22 }, // quotation
      { 0xa0, 0x20 } // no break space
      };
    #endregion

    #region Properties
    public HashSet<byte> UnknownChars {
      get {
        return _unknownChars;
      }
    }
    #endregion

    #region Public methods
    public HighAsciiNormalization() {
    }

    static public bool NormalizedCharacter(byte character) {
      return _normalizationDictionary.ContainsKey(character);
    }

    static public bool RequiresNormalization(byte[] document) {
      // Check if UTF8 encoded (by BOM)
      if ((document[0] == 0xef) &&
          (document[1] == 0xbb) &&
          (document[2] == 0xbf)) {
        return false;
      }

      return true;
    }

    public string Normalize(ref byte[] document) {
      _unknownChars.Clear();

      for (int i = 0; i < document.Length; i++) {
        if (_normalizationDictionary.ContainsKey(document[i])) {
          document[i] = _normalizationDictionary[document[i]];
        } else if ((uint)document[i] > 128) {
          // unknown high-ascii char
          _unknownChars.Add(document[i]);
        }
      }

      UTF8Encoding temp = new UTF8Encoding(true);
      return temp.GetString(document);
    }
    #endregion
  }
}