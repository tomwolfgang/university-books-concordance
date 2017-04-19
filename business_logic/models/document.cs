using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.models {
  public class Document {
    public enum LoadState { NotComplete = 0, Complete };

    // only allow data_access classes to create a Word
    internal Document() {
    }

    public override string ToString() {
      return String.Format("({0},{1})", GutenbergId, Title);
    }

    internal long Id {
      get;
      set;
    }

    // based on the name part of the file
    public string GutenbergId {
      get;
      internal set;
    }

    public string Title {
      get;
      internal set;
    }

    public string Author {
      get;
      internal set;
    }

    public FileInfo LocalFile {
      get;
      internal set;
    }

    public DateTime? ReleaseDate {
      get;
      internal set;
    }

    public LoadState TableLoadState {
      get;
      internal set;
    }
  }
}
