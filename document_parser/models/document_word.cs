using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace document_parser.models {

  //----------------------------------------------------------------------------
  public class DocumentWord {
    internal DocumentWord() {
    }

    public override string ToString() {
      return Text;
    }

    public string Text {
      get;
      internal set;
    }

    public uint OffsetInFile {
      get;
      internal set;
    }

    public uint Page {
      get;
      internal set;
    }

    public uint Paragraph {
      get;
      internal set;
    }

    public uint Sentence {
      get;
      internal set;
    }

    public uint IndexInSentence {
      get;
      internal set;
    }

    public uint Line {
      get;
      internal set;
    }


  }

} //namespace business_logic
