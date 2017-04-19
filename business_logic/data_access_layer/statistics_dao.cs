using books.business_logic.models;
using books.business_logic.services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.data_access_layer {
  //---------------------------------------------------------------------------
  public class StatisticsDao {

    #region Constants

    private const string kSQLQueryWordFrequencies =
      "SELECT " + ContainsDao.kFieldWordId + ", COUNT(*) w_count " +
      "FROM contains " +
      "GROUP BY " + ContainsDao.kFieldWordId + " " +
      "ORDER BY " + ContainsDao.kFieldWordCount + " DESC " +
      "LIMIT {0}";

    private const string kSQLQueryAvgWordsPerLine =
      "SELECT AVG(w_count) FROM ( " +
        "SELECT COUNT(" + ContainsDao.kFieldWordId + ") w_count " +
        "FROM contains " +
        "GROUP BY " + ContainsDao.kFieldDocumentId + ", " +
                      ContainsDao.kFieldLine + ") AS temp";

    private const string kSQLQueryAvgCharsPerLine =
      "SELECT AVG(sum_length) FROM (" +
        "SELECT SUM(" + WordsDao.kFieldLength + ") sum_length " +
        "FROM contains, word " +
        "WHERE contains." + ContainsDao.kFieldWordId + " = word." + 
                                                       WordsDao.kFieldId + " " +
        "GROUP BY " + ContainsDao.kFieldDocumentId + ", " +
                      ContainsDao.kFieldLine + ") AS temp";

    private const string kSQLQueryAvgWordsPerSentence =
      "SELECT AVG(w_count) FROM (" +
        "SELECT COUNT(" + ContainsDao.kFieldWordId + ") w_count " +
        "FROM contains " +
        "GROUP BY " + ContainsDao.kFieldDocumentId + ", " +
                      ContainsDao.kFieldSentence + ") AS temp";

    private const string kSQLQueryAvgCharsPerSentence =
      "SELECT AVG(sum_length) from ( " +
        "SELECT " + ContainsDao.kFieldSentence + ", " +
                    "SUM(" + WordsDao.kFieldLength + ") sum_length " +
        "FROM contains, word " +
        "WHERE contains." + ContainsDao.kFieldWordId + " = word." +
                                                       WordsDao.kFieldId + " " +
        "GROUP BY " + ContainsDao.kFieldDocumentId + ", " +
                      ContainsDao.kFieldSentence + ") AS temp";

    private const string kSQLQueryAvgWordsPerParagraph =
      "SELECT AVG(w_count) FROM (" +
        "SELECT COUNT(" + ContainsDao.kFieldWordId + ") w_count " + 
        "FROM contains " +
        "GROUP BY " + ContainsDao.kFieldDocumentId + ", " + 
                      ContainsDao.kFieldParagraph + ") AS temp";

    private const string kSQLQueryAvgCharsPerParagraph =
      "SELECT AVG(sum_length) FROM ( " +
        "SELECT " + ContainsDao.kFieldParagraph + ", " +
                    "SUM(" + WordsDao.kFieldLength + ") sum_length " +
        "FROM contains, word " +
        "WHERE contains." + ContainsDao.kFieldWordId + " = word." +
                                                       WordsDao.kFieldId + " " +
        "GROUP BY " + ContainsDao.kFieldDocumentId + ", " +
                      ContainsDao.kFieldParagraph + ") AS temp";

    private const string kSQLQueryAvgWordsPerPage =
      "SELECT AVG(w_count) FROM (" +
        "SELECT COUNT(" + ContainsDao.kFieldWordId + ") w_count " +
        "FROM contains " +
        "GROUP BY " + ContainsDao.kFieldDocumentId + ", " +
                      ContainsDao.kFieldPage + ") AS temp";

    private const string kSQLQueryAvgCharsPerPage =
      "SELECT AVG(sum_length) FROM ( " +
        "SELECT " + ContainsDao.kFieldPage + ", " +
                    "SUM(" + WordsDao.kFieldLength + ") sum_length " +
        "FROM contains, word " +
        "WHERE contains." + ContainsDao.kFieldWordId + " = word." +
                                                       WordsDao.kFieldId + " " +
        "GROUP BY " + ContainsDao.kFieldDocumentId + ", " +
                      ContainsDao.kFieldPage + ") AS temp";

    private const string kSQLQueryAvgWordsPerDocument =
      "SELECT AVG(w_count) FROM ( " +
        "SELECT COUNT(" + ContainsDao.kFieldWordId + ") w_count " +
        "FROM contains " +
        "GROUP BY " + ContainsDao.kFieldDocumentId + ") AS temp";

    private const string kSQLQueryAvgCharsPerDocument =
      "SELECT AVG(sum_length) FROM ( " +
        "SELECT SUM(" + WordsDao.kFieldLength + ") sum_length " +
        "FROM contains, word " +
        "WHERE contains." + ContainsDao.kFieldWordId + " = word." +
                                                       WordsDao.kFieldId + " " +
        "GROUP BY " + ContainsDao.kFieldDocumentId + ") AS temp";

    #endregion


    //--------------------------------------------------------------------------
    // get the top |limit| words
    // return list of <word_id, frequency>
    public static List<Tuple<long, uint>> GetWordFrequencies(int limit) {
      MySqlDataReader reader = null;

      try {
        string query = String.Format(kSQLQueryWordFrequencies, limit);
        MySqlCommand cmd = new MySqlCommand(
          query,
          DatabaseConnectionService.Instance.Connection);

        List<Tuple<long, uint>> result = new List<Tuple<long, uint>>();
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Tuple<long, uint> word = new Tuple<long, uint>(
            reader.GetInt32(ContainsDao.kFieldWordId),
            reader.GetUInt32(ContainsDao.kFieldWordCount));
          result.Add(word);
        }

        return result;
      } catch (Exception ex) {
        throw ex;
      } finally {
        if (reader != null) {
          reader.Close();
        }
      }
    }

    //--------------------------------------------------------------------------
    public static double AvgWordsPerLine() {
      return AverageQuery(kSQLQueryAvgWordsPerLine);
    }

    //--------------------------------------------------------------------------
    public static double AvgWordsPerSentence() {
      return AverageQuery(kSQLQueryAvgWordsPerSentence);
    }

    //--------------------------------------------------------------------------
    public static double AvgWordsPerParagraph() {
      return AverageQuery(kSQLQueryAvgWordsPerParagraph);
    }

    //--------------------------------------------------------------------------
    public static double AvgWordsPerPage() {
      return AverageQuery(kSQLQueryAvgWordsPerPage);
    }

    //--------------------------------------------------------------------------
    public static double AvgWordsPerDocument() {
      return AverageQuery(kSQLQueryAvgWordsPerDocument);
    }

    //--------------------------------------------------------------------------
    public static double AvgCharsPerLine() {
      return AverageQuery(kSQLQueryAvgCharsPerLine);
    }

    //--------------------------------------------------------------------------
    public static double AvgCharsPerSentence() {
      return AverageQuery(kSQLQueryAvgCharsPerSentence);
    }

    //--------------------------------------------------------------------------
    public static double AvgCharsPerParagraph() {
      return AverageQuery(kSQLQueryAvgCharsPerParagraph);
    }

    //--------------------------------------------------------------------------
    public static double AvgCharsPerPage() {
      return AverageQuery(kSQLQueryAvgCharsPerPage);
    }

    //--------------------------------------------------------------------------
    public static double AvgCharsPerDocument() {
      return AverageQuery(kSQLQueryAvgCharsPerDocument);
    }

    //--------------------------------------------------------------------------
    private static double AverageQuery(string query) {
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          query,
          DatabaseConnectionService.Instance.Connection);
        cmd.CommandTimeout = 
          GlobalParamatersService.Configuration
                                 .Statistics
                                 .LongQueriesTimeoutInSeconds;

        reader = cmd.ExecuteReader();

        if (!reader.Read()) {
          return 0;
        }

        return reader.GetDouble(0);
      } catch (Exception ex) {
        throw ex;
      } finally {
        if (reader != null) {
          reader.Close();
        }
      }
    }
  }
}
