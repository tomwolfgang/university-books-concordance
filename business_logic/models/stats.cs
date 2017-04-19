using document_parser.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.models {
  //----------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  public class Stats {
    //--------------------------------------------------------------------------
    internal Stats() {
    }

    public Int64 Documents {
      get;
      internal set;
    }

    public Int64 IndexedWords {
      get;
      internal set;
    }

    public Int64 UniqueWords {
      get;
      internal set;
    }

    public Int64 Groups {
      get;
      internal set;
    }

    public Int64 Relations {
      get;
      internal set;
    }
  }
}