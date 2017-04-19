using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using books.business_logic.common;

namespace books.business_logic.services {
  //-------------------------------------------------------------------------
  /// <summary>
  /// Used to share global parmeters with all services
  /// </summary>
  class GlobalParamatersService {
    static public Configuration Configuration { get; internal set; }
    static public BusinessLogicDelegate Delegate { get; internal set; }
    static public bool InitializationFailed { get; internal set; }
  }
}
