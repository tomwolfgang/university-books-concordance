using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic {
  public interface IStatsUpdates {
    void CollectStats();

    void UpdateWords();
    void UpdateGroups();
    void UpdateRelations();
  }
}
