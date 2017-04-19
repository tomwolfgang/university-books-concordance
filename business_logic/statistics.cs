using books.business_logic.common;
using books.business_logic.models;
using books.business_logic.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic {
  //----------------------------------------------------------------------------
  public interface StatisticsDelegate {
    void OnAvgWordsPerLine(double value);
    void OnAvgWordsPerSentence(double value);
    void OnAvgWordsPerParagraph(double value);
    void OnAvgWordsPerPage(double value);
    void OnAvgWordsPerDocument(double value);

    void OnAvgCharsPerLine(double value);
    void OnAvgCharsPerSentence(double value);
    void OnAvgCharsPerParagraph(double value);
    void OnAvgCharsPerPage(double value);
    void OnAvgCharsPerDocument(double value);

    void OnWordFrequenciesReady(List<Tuple<Word, uint>> frequencies);
  };

  //----------------------------------------------------------------------------
  public class Statistics {
    public delegate void GetStatisticsCallback(bool status, string message);

    //--------------------------------------------------------------------------
    private ThreadWorkerQueue _workerThread = null;

    //--------------------------------------------------------------------------
    public Statistics(ThreadWorkerQueue workerThread) {
      this._workerThread = workerThread;
    }

    //--------------------------------------------------------------------------
    public bool GetAll(StatisticsDelegate statsDelegate, 
                       GetStatisticsCallback callback) {
      return _workerThread.PostTask(_ => {
        try {
          // word frequencies - start with this so that the user can
          // play with the frequencies while we get other stats
          List<Tuple<Word, uint>> frequencies =
            StatisticsService.Instance.GetWordFrequencies();
          statsDelegate.OnWordFrequenciesReady(frequencies);

          // average words
          statsDelegate.OnAvgWordsPerLine(
            StatisticsService.Instance.AvgWordsPerLine());
          statsDelegate.OnAvgWordsPerSentence(
            StatisticsService.Instance.AvgWordsPerSentence());
          statsDelegate.OnAvgWordsPerParagraph(
            StatisticsService.Instance.AvgWordsPerParagraph());
          statsDelegate.OnAvgWordsPerPage(
            StatisticsService.Instance.AvgWordsPerPage());
          statsDelegate.OnAvgWordsPerDocument(
            StatisticsService.Instance.AvgWordsPerDocument());

          // average chars
          statsDelegate.OnAvgCharsPerLine(
            StatisticsService.Instance.AvgCharsPerLine());
          statsDelegate.OnAvgCharsPerSentence(
            StatisticsService.Instance.AvgCharsPerSentence());
          statsDelegate.OnAvgCharsPerParagraph(
            StatisticsService.Instance.AvgCharsPerParagraph());
          statsDelegate.OnAvgCharsPerPage(
            StatisticsService.Instance.AvgCharsPerPage());
          statsDelegate.OnAvgCharsPerDocument(
            StatisticsService.Instance.AvgCharsPerDocument());

          callback(true, null);
        } catch (Exception e) {
          callback(false, e.Message);
        }
      });
    }

  }
}
