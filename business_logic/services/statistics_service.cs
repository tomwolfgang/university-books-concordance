using books.business_logic.data_access_layer;
using books.business_logic.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.services {
  class StatisticsService {
    //--------------------------------------------------------------------------
    private static StatisticsService _instance = new StatisticsService();

    //--------------------------------------------------------------------------
    /// <summary>
    /// </summary>
    private StatisticsService() {
    }

    //--------------------------------------------------------------------------
    static StatisticsService() {
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// Get singelton instance
    /// </summary>
    public static StatisticsService Instance {
      get {
        return _instance;
      }
    }

    //--------------------------------------------------------------------------
    public List<Tuple<Word, uint>> GetWordFrequencies() {
      int limit = 
        GlobalParamatersService.Configuration.StatisticsWordFrequenciesLimit;

      List<Tuple<long, uint>> wordIdsFrequencies = 
        StatisticsDao.GetWordFrequencies(limit);

      List<Tuple<Word, uint>> result = new List<Tuple<Word, uint>>();
      foreach (var item in wordIdsFrequencies) {
        Tuple<Word, uint> wordFrequency = new Tuple<Word, uint>(
          WordsService.Instance.GetWordById(item.Item1), 
          item.Item2);
        result.Add(wordFrequency);
      }

      return result;
    }

    //--------------------------------------------------------------------------
    public double AvgWordsPerLine() {
      return StatisticsDao.AvgWordsPerLine();
    }

    //--------------------------------------------------------------------------
    public double AvgWordsPerSentence() {
      return StatisticsDao.AvgWordsPerSentence();
    }

    //--------------------------------------------------------------------------
    public double AvgWordsPerParagraph() {
      return StatisticsDao.AvgWordsPerParagraph();
    }

    //--------------------------------------------------------------------------
    public double AvgWordsPerPage() {
      return StatisticsDao.AvgWordsPerPage();
    }

    //--------------------------------------------------------------------------
    public double AvgWordsPerDocument() {
      return StatisticsDao.AvgWordsPerDocument();
    }

    //--------------------------------------------------------------------------
    public double AvgCharsPerLine() {
      return StatisticsDao.AvgCharsPerLine();
    }

    //--------------------------------------------------------------------------
    public double AvgCharsPerSentence() {
      return StatisticsDao.AvgCharsPerSentence();
    }

    //--------------------------------------------------------------------------
    public double AvgCharsPerParagraph() {
      return StatisticsDao.AvgCharsPerParagraph();
    }

    //--------------------------------------------------------------------------
    public double AvgCharsPerPage() {
      return StatisticsDao.AvgCharsPerPage();
    }

    //--------------------------------------------------------------------------
    public double AvgCharsPerDocument() {
      return StatisticsDao.AvgCharsPerDocument();
    }


  }
}
