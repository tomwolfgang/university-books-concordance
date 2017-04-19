using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace books.business_logic.common {
  //-------------------------------------------------------------------------
  /// <summary>
  /// A background thread that runs jobs (AsyncCallback(s)) from a queue.
  /// 
  /// Usage:
  /// 
  /// ThreadWorkerQueue worker = new ThreadWorkerQueue("My Worker");
  /// worker.Start();
  /// worker.PostTask(_ => {
  ///     Console.Writeline("blah");
  /// }
  /// </summary>
  public class ThreadWorkerQueue {
    private static readonly string THREAD_WORKER_DEFAULT_NAME = "Books Thread";
    private static readonly int THREAD_STOP_TIMEOUT_MILLISECONDS = 4000;

    private Thread _threadWorker;
    private Queue<AsyncCallback> _callbacks;
    private EventWaitHandle _eventWait;
    private bool _cancel;

    //-------------------------------------------------------------------------
    public ThreadWorkerQueue(string threadName) {
      _callbacks = new Queue<AsyncCallback>();
      _eventWait = new EventWaitHandle(false, EventResetMode.AutoReset);
      _threadWorker = new Thread(new ThreadStart(ThreadWorker));
      _threadWorker.Name = (threadName.Length > 0) ? 
        threadName : THREAD_WORKER_DEFAULT_NAME;
      _threadWorker.IsBackground = true;

      _cancel = false;
    }

    //-------------------------------------------------------------------------
    public bool Start() {
      if (_threadWorker.IsAlive) {
        return false;
      }

      _cancel = false;
      _threadWorker.Start();
      return true;
    }

    //-------------------------------------------------------------------------
    public bool Stop() {
      if (!_threadWorker.IsAlive) {
        return true;
      }

      _cancel = true;
      _eventWait.Set();
      return _threadWorker.Join(THREAD_STOP_TIMEOUT_MILLISECONDS);
    }

    //-------------------------------------------------------------------------
    public bool PostTask(AsyncCallback callback) {
      if (!_threadWorker.IsAlive) {
        return false;
      }

      lock (_callbacks) {
        _callbacks.Enqueue(callback);
      }

      _eventWait.Set();
      return true;
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// returns true if the current instruction pointer is running in the 
    /// context of this ThreadWorker
    /// </summary>
    /// <returns></returns>
    public bool InContext {
      get {
        if (Thread.CurrentThread.ManagedThreadId ==
          _threadWorker.ManagedThreadId) {
          return true;
        }

        return false;
      }
    }

    //-------------------------------------------------------------------------
    private void ThreadWorker() {
      while (!_cancel) {
        _eventWait.WaitOne();
        bool hasWork = true;

        while (hasWork) {
          hasWork = false;

          AsyncCallback callback = null;

          lock (_callbacks) {
            if (_callbacks.Count > 0) {
              callback = _callbacks.Dequeue();
              hasWork = (_callbacks.Count > 0);
            }
          }

          if (null != callback) {
            try {
              callback.Invoke(null);
            } catch (Exception ex) {
              Console.WriteLine(ex);
              Debug.Assert(false);
            }
          }
        }
      }
    }

  }
}
