using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.models {
  //---------------------------------------------------------------------------
  // 
  public class WordLocationDetails {
    internal WordLocationDetails() {
    }

    public Word Word {
      get;
      internal set;
    }

    public List<LocationDetail> LocationDetails {
      get;
      internal set;
    }
  }
}
