using books.business_logic.models;
using books.business_logic.services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace books.business_logic.data_access_layer {
  //---------------------------------------------------------------------------
  public class WordsDao {
    #region Constants

    public const string kTableWord = "word";

    public const string kFieldId = "id";
    public const string kFieldValue = "value";
    public const string kFieldLength = "length";

    private const string kSQLSelectAll = "SELECT * FROM word";
    private const string kSQLSelectCountAll = "SELECT COUNT(*) FROM word";
    private const string kSQLInsertWord =
      "INSERT INTO word(" + kFieldValue + ", " + kFieldLength + ") " +
      "VALUES(@" + kFieldValue + ", @" + kFieldLength + ")";

    private const string kSQLImportWord =
      "INSERT INTO word(" + kFieldId + ", " + 
                            kFieldValue + ", " + 
                            kFieldLength + ") " +
      "VALUES(@" + kFieldId + ", @" + kFieldValue + ", @" + kFieldLength + ")";

    private const string kSQLCreateTable =
      "CREATE TABLE `word` ( " +
        "`id` int(11) NOT NULL AUTO_INCREMENT, " +
        "`value` varchar(100) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL, " +
        "`length` int(11) DEFAULT NULL, " +
        "PRIMARY KEY(`value`), " +
        "UNIQUE KEY `id_UNIQUE` (`id`) " +
      ") ENGINE=InnoDB AUTO_INCREMENT = 0 DEFAULT CHARSET = utf8 COLLATE=utf8_bin;";
    
    #endregion

    //--------------------------------------------------------------------------
    // used to export to xml
    public static void FillDataSet(DataSet ds) {
      DaoUtils.FillDataSet(kSQLSelectAll, kTableWord, ds);
    }

    //--------------------------------------------------------------------------
    public static void DropTable() {
      DaoUtils.DropTable(kTableWord);
    }

    //--------------------------------------------------------------------------
    public static void CreateTable() {
      DaoUtils.ExecuteNonQuery(kSQLCreateTable);
    }

    //-------------------------------------------------------------------------
    /// <exception cref="InvalidOperationException">Why it's thrown.</exception>
    public static void GetAll(out List<Word> words) {
      words = new List<Word>();

      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectAll, DatabaseConnectionService.Instance.Connection);
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Word word = new Word() {
            Id = reader.GetUInt32(kFieldId),
            Value = reader.GetString(kFieldValue),
            Length = reader.GetInt32(kFieldLength)
          };

          words.Add(word);
        }
      } catch (Exception ex) {
        throw ex;
      } finally {
        if (reader != null) {
          reader.Close();
        }
      }
    }

    //--------------------------------------------------------------------------
    public static Int64 GetCount() {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectCountAll, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        return (Int64)cmd.ExecuteScalar();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// inserts a new word (the id is ignored) and sets the id if succeeds
    /// </summary>
    /// <param name="word"></param>
    public static Word Insert(string value) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLInsertWord, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldValue, value);
        cmd.Parameters.AddWithValue("@" + kFieldLength, value.Length);
        cmd.ExecuteNonQuery();

        return new Word() {
          Id = cmd.LastInsertedId,
          Value = value,
          Length = value.Length
        };

      } catch (Exception ex) {
        throw ex;
      }
    }

    //-------------------------------------------------------------------------
    public static Word Import(XmlNode xmlWord) {
      Word word = new Word() {
        Id = Int32.Parse(xmlWord.SelectSingleNode("./" + kFieldId).InnerText),
        Value = xmlWord.SelectSingleNode("./" + kFieldValue).InnerText,
        Length = Int32.Parse(
          xmlWord.SelectSingleNode("./" + kFieldLength).InnerText)
      };

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLImportWord, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, word.Id);
        cmd.Parameters.AddWithValue("@" + kFieldValue, word.Value);
        cmd.Parameters.AddWithValue("@" + kFieldLength, word.Length);
        cmd.ExecuteNonQuery();

        return word;
      } catch (Exception ex) {
        throw ex;
      }
    }

  }
}
