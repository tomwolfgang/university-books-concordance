using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.models {
  //---------------------------------------------------------------------------
  /// <summary>
  /// Used for querying documents - you can create multiple WordLocationProperty
  /// objects per query - this will make the query stricter
  /// 
  /// Common uses: 
  ///   1. <Line, Page>
  ///   2. <Sentence, Paragraph>
  /// </summary>
  public class WordLocationProperty {
    public WordLocationProperty() {
    }

    public enum Property {
      Line,
      Page,
      Sentence,
      SentenceIndex,
      Paragraph,
    };

    public Property Field {
      get; set;
    }

    public long Value {
      get; set;
    }
  }
}
