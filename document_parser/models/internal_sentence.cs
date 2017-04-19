using System;
using System.Collections.Generic;
using System.Text;

namespace document_parser.models {
  //----------------------------------------------------------------------------
  class InternalSentence {
    public string text;
    public uint fileOffsetBegin;
    public uint fileOffsetEnd;
    public List<InternalWord> words;

    public override string ToString() {
      return text;
    }
  }

} //namespace business_logic
