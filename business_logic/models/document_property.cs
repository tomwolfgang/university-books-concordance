using books.business_logic.data_access_layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.models {
  //---------------------------------------------------------------------------
  /// <summary>
  /// Used for querying documents - you can create multiple DocumentProperty
  /// objects per query - this will make the query stricter
  /// </summary>
  public class DocumentProperty {
    public DocumentProperty() {

    }

    public enum Property {
      GutenbergId,
      Title,
      Author,
      ReleaseDate 
    };

    public Property Field {
      get; set;
    }

    public string StrValue {
      get; set;
    }

    public DateTime DateTimeValue {
      get; set;
    }
  }
}
