using System;
using System.Collections.Generic;

namespace books.business_logic.models {
  public class Phrase {
    internal Phrase() {
    }

    public override string ToString() {
      return String.Format("({0}, words: {1})", 
                           Id, 
                           (Words != null) ? Words.Count : 0);
    }

    internal long Id {
      get;
      set;
    }

    public List<Word> Words {
      get;
      internal set;
    }
  }
}
