using books.business_logic.models;
using books.business_logic.services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace books.business_logic.data_access_layer {
  //----------------------------------------------------------------------------
  public class PhrasesDao {

    #region Constants

    internal const string kFieldId = "id";
    internal const string kFieldPhraseId = "phrase_id";
    internal const string kFieldWordId = "word_id";
    internal const string kFieldOrdinal = "ordinal";

    private const string kTablePhrase = "phrase";
    private const string kTablePhrasesWords = "phrases_words";

    private const string kSQLSelectCountAll =
      "SELECT COUNT(*) FROM " + kTablePhrase;

    // notice we order by phrase id and ordinal - this is important so that
    // we build the phrases with the original words order
    private const string kSQLSelectAll =
      "SELECT " + kTablePhrasesWords + ".* " +
      "FROM " + kTablePhrase + ", " + kTablePhrasesWords + " " +
      "WHERE " + kTablePhrase + "." + kFieldId + " = " +
                 kTablePhrasesWords + "." + kFieldPhraseId + " " +
      "ORDER BY " + kFieldPhraseId + ", " + kFieldOrdinal;

    // we just get a phrase id
    private const string kSQLInsertPhrase = 
      "INSERT INTO " + kTablePhrase + " " +
      "VALUES()";

    private const string kSQLImportPhrase =
      "INSERT INTO " + kTablePhrase + "(" + kFieldId + ") " +
      "VALUES(@" + kFieldId + ")";
  
    private const string kSQLInsertPhraseWord =
      "INSERT INTO " + kTablePhrasesWords +
                      "(" + kFieldPhraseId + ", " +
                            kFieldWordId + ", " +
                            kFieldOrdinal + ") " +
      "VALUES(@" + kFieldPhraseId + ", " +
             "@" + kFieldWordId + ", " +
             "@" + kFieldOrdinal + ")";

    private const string kSQLImportPhraseWord = kSQLInsertPhraseWord;

    private const string kSQLDeletePhraseWords =
      "DELETE FROM " + kTablePhrasesWords + " " +
      "WHERE " + kFieldPhraseId + " = " + "@" + kFieldPhraseId;

    private const string kSQLDeleteById =
      "DELETE FROM " + kTablePhrase + " " +
      "WHERE " + kFieldId + " = " + "@" + kFieldId;

    private const string kSQLCreateTablePhrase =
      "CREATE TABLE `phrase` ( " +
        "`id` int(11) NOT NULL AUTO_INCREMENT, " +
        "UNIQUE KEY `id_UNIQUE` (`id`) " +
      ") ENGINE=InnoDB AUTO_INCREMENT = 0 DEFAULT CHARSET = utf8 COLLATE=utf8_bin;";

    private const string kSQLCreateTablePhrasesWords =
      "CREATE TABLE `phrases_words` ( " +
        "`phrase_id` int(11) NOT NULL, " +
        "`word_id` int(11) NOT NULL, " +
        "`ordinal` int(11) NOT NULL, " +
        "PRIMARY KEY(`phrase_id`,`word_id`,`ordinal`), " +
        "KEY `ordinal_idx` (`ordinal`), " +
        "KEY `word_id_idx` (`word_id`), " +
        "KEY `phrase_id_idx` (`phrase_id`), " +
        "CONSTRAINT `pwid` FOREIGN KEY(`word_id`) REFERENCES `word` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION, " +
        "CONSTRAINT `pid` FOREIGN KEY(`phrase_id`) REFERENCES `phrase` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION " +
    ") ENGINE=InnoDB DEFAULT CHARSET=utf8;";

    #endregion

    //--------------------------------------------------------------------------
    // used to export to xml
    public static void FillDataSet(DataSet ds) {
      DaoUtils.FillDataSet("SELECT * FROM " + kTablePhrase, 
                           kTablePhrase, 
                           ds);
      DaoUtils.FillDataSet("SELECT * FROM " + kTablePhrasesWords, 
                           kTablePhrasesWords, 
                           ds);
    }

    //--------------------------------------------------------------------------
    public static void DropTable() {
      DaoUtils.DropTable(kTablePhrasesWords);
      DaoUtils.DropTable(kTablePhrase);
    }

    //--------------------------------------------------------------------------
    public static void CreateTable() {
      DaoUtils.ExecuteNonQuery(kSQLCreateTablePhrase);
      DaoUtils.ExecuteNonQuery(kSQLCreateTablePhrasesWords);
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

    //--------------------------------------------------------------------------
    internal static List<InternalPhrase> GetAll() {
      List<InternalPhrase> phrases = new List<InternalPhrase>();

      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectAll, DatabaseConnectionService.Instance.Connection);
        reader = cmd.ExecuteReader();

        int lastId = -1;
        InternalPhrase currentPhrase = null;

        // we will need to read a few rows per phrase
        while (reader.Read()) {
          int currentId = reader.GetInt32(kFieldPhraseId);
          if (currentId != lastId) {
            lastId = currentId;
            if (currentPhrase != null) {
              phrases.Add(currentPhrase);
            }

            currentPhrase = new InternalPhrase();
            currentPhrase.Id = currentId;
            currentPhrase.WordIds = new List<long>();
          }

          int wordId = reader.GetInt32(kFieldWordId);
          currentPhrase.WordIds.Add(wordId);
        }

        if (currentPhrase != null) {
          phrases.Add(currentPhrase);
        }

        return phrases;
      } catch (Exception ex) {
        throw ex;
      } finally {
        if (reader != null) {
          reader.Close();
        }
      }
    }

    //--------------------------------------------------------------------------
    public static void Insert(ref Phrase phrase) {
      Phrase lambdaPhrase = phrase;

      DatabaseConnectionService.Instance.SafeTransaction(_ => {
        // first we add the new phrase and get an id
        AddNewPhrase(ref lambdaPhrase);

        // now add the words
        int currentOrdinal = 0;
        foreach (Word word in lambdaPhrase.Words) {
          AddPhraseWord(lambdaPhrase, word, currentOrdinal++);
        }
      });
    }

    //--------------------------------------------------------------------------
    public static void Delete(Phrase phrase) {
      DatabaseConnectionService.Instance.SafeTransaction(_ => {
        RemovePhraseWords(phrase);
        RemovePhrase(phrase);
      });
    }

    //--------------------------------------------------------------------------
    private static void AddNewPhrase(ref Phrase phrase) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLInsertPhrase, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.ExecuteNonQuery();
        phrase.Id = cmd.LastInsertedId;
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    private static void AddPhraseWord(Phrase phrase, Word word, int ordinal) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLInsertPhraseWord, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldPhraseId, phrase.Id);
        cmd.Parameters.AddWithValue("@" + kFieldWordId, word.Id);
        cmd.Parameters.AddWithValue("@" + kFieldOrdinal, ordinal);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    private static void RemovePhraseWords(Phrase phrase) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLDeletePhraseWords, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldPhraseId, phrase.Id);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    private static void RemovePhrase(Phrase phrase) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLDeleteById, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, phrase.Id);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static Phrase ImportPhrase(XmlNode xmlPhrase) {
      /*
        <id>1</id>
      */
      Phrase phrase = new Phrase() {
        Id = Int32.Parse(
          xmlPhrase.SelectSingleNode("./" + kFieldId).InnerText)
      };

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLImportPhrase, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, phrase.Id);
      
        cmd.ExecuteNonQuery();

        return phrase;
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static void ImportPhraseWord(XmlNode xmlPhraseWord) {
      /*
        <phrase_id>1</phrase_id>
        <word_id>564</word_id>
        <ordinal>2</ordinal>
      */
      long phraseId = Int32.Parse(
        xmlPhraseWord.SelectSingleNode("./" + kFieldPhraseId).InnerText);
      long wordId = Int32.Parse(
        xmlPhraseWord.SelectSingleNode("./" + kFieldWordId).InnerText);
      long ordinal = Int32.Parse(
        xmlPhraseWord.SelectSingleNode("./" + kFieldOrdinal).InnerText);

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLImportPhraseWord, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldPhraseId, phraseId);
        cmd.Parameters.AddWithValue("@" + kFieldWordId, wordId);
        cmd.Parameters.AddWithValue("@" + kFieldOrdinal, ordinal);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

  }
}
