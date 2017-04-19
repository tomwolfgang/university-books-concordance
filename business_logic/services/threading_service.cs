using books.business_logic.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.services {
  class ThreadingService {
    private static ThreadingService _instance = new ThreadingService();

    public ThreadWorkerQueue DBThread { get;  }
    public ThreadWorkerQueue IOThread { get; internal set; }

    private ThreadingService() {
      DBThread = new ThreadWorkerQueue("Database Thread");
      if (!DBThread.Start()) {
        throw new Exception("Couldn't start DB Thread!");
      }

      IOThread = new ThreadWorkerQueue("IO Thread");
      if (!IOThread.Start()) {
        throw new Exception("Couldn't start DB Thread!");
      }
    }

    public static ThreadingService Instance {
      get {
        return _instance;
      }
    }
  }
}
