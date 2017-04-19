using System;
using System.Collections.Generic;

namespace books.business_logic.models {
  internal class InternalPhrase {
    internal InternalPhrase() {
    }

    public override string ToString() {
      return String.Format("({0})", Id);
    }

    internal long Id {
      get;
      set;
    }

    public List<long> WordIds {
      get;
      internal set;
    }
  }
}
