using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.models {
  public class Word {
    // only allow data_access classes to create a Word
    internal Word() {
    }

    public override string ToString() {
      return String.Format("{0}", Value);
    }

    internal long Id {
      get;
      set;
    }

    public string Value {
      get;
      internal set;
    }

    public int Length {
      get;
      internal set;
    }
  }
}
