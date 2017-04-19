﻿using System;

namespace books.business_logic.models {
  public class Relation {
    internal Relation() {
    }

    public override string ToString() {
      return String.Format("({0},{1})", Id, Name);
    }

    internal long Id {
      get;
      set;
    }

    public string Name {
      get;
      internal set;
    }
  }
}
