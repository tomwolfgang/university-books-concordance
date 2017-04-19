using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace document_parser.models {
  //----------------------------------------------------------------------------
  class InternalWord {
    public string annotatedText;
    public string originalText;
    // NOTE - this will not hold the file offset, but rather the in-memory
    // offset of the word
    public uint characterOffsetBegin;
    public uint characterOffsetEnd;

    public override string ToString() {
      return String.Format("{0})", originalText);
    }
  }

} //namespace business_logic
