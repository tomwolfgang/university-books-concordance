using document_parser.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.models {
  //---------------------------------------------------------------------------
  /// <summary>
  /// Contains represents a single instance of a word in a document
  /// </summary>
  public class Contains {
    // only allow data_access classes to create a Contains
    //-------------------------------------------------------------------------
    internal Contains() {
    }

    internal long DocumentId {
      get;
      set;
    }

    internal long WordId {
      get;
      set;
    }

    public long Line {
      get;
      internal set;
    }

    public long FileOffset {
      get;
      internal set;
    }

    public long IndexInSentence {
      get;
      internal set;
    }

    public long Sentence {
      get;
      internal set;
    }

    public long Paragraph {
      get;
      internal set;
    }

    public long Page {
      get;
      internal set;
    }

  }
}