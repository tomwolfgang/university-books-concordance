using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.models {
  //---------------------------------------------------------------------------
  // part of WordLocationDetails - holds a Document and it's coresponding 
  // Contains object
  public class LocationDetail {
    internal LocationDetail() {
    }

    public Document Document {
      get;
      internal set;
    }

    public Contains Location {
      get;
      internal set;
    }
  }
}
