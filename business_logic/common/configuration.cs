using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.common {
  //----------------------------------------------------------------------------
  public class ContentResolution {
    // when false, we use Sentences
    public bool UseLines {
      get;set;
    }

    // how many lines (or sentences) do we retrieve sorrounding our word
    public uint Delta {
      get;set;
    }

  }

  //----------------------------------------------------------------------------
  public class Configuration {
    public string ConnectionString {
      get;set;
    }

    public DirectoryInfo Storage {
      get; set;
    }

    public ContentResolution ContentRetreivalResolution {
      get;set;
    }
  
    // when true, we perform itegrity validations when inserting documents into
    // the database - this is mainly good for debugging, because it will 
    // seriously hit document insertion performance
    public bool PerformIntegrityValidations { 
      get; set; 
    }

    public int StatisticsWordFrequenciesLimit {
      get; set;
    }
  }
}
