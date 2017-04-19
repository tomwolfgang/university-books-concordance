using books.business_logic.models;
using books.business_logic.services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace books.business_logic.data_access_layer {
  //----------------------------------------------------------------------------
  public class RelationsDao {

    #region Constants

    internal const string kFieldId = "id";
    internal const string kFieldName = "name";
    internal const string kFieldRelationId = "relation_id";
    internal const string kFieldFirstWordId = "first_word_id";
    internal const string kFieldSecondWordId = "second_word_id";

    private const string kTableRelation = "relation";
    private const string kTableRelationsWords = "relations_words";

    private const string kSQLSelectAll = "SELECT * FROM " + kTableRelation;
    private const string kSQLSelectAllRelationsWords =
      "SELECT * FROM " + kTableRelationsWords;

    private const string kSQLSelectCountAll =
      "SELECT COUNT(*) FROM " + kTableRelation;

    private const string kSQLInsertRelation =
      "INSERT INTO " + kTableRelation + "(" + kFieldName + ") " +
      "VALUES(@" + kFieldName + ")";

    private const string kSQLImportRelation =
      "INSERT INTO " + kTableRelation + "(" + kFieldId + ", " + 
                                              kFieldName + ") " +
      "VALUES(@" + kFieldId + ", @" + kFieldName + ")";

    private const string kSQLInsertRelationWords =
      "INSERT INTO " + kTableRelationsWords +
                      "(" + kFieldRelationId + ", " + 
                            kFieldFirstWordId + ", " +
                            kFieldSecondWordId + ") " +
      "VALUES(@" + kFieldRelationId + ", " +
             "@" + kFieldFirstWordId + ", " +
             "@" + kFieldSecondWordId + ")";

    private const string kSQLImportRelationWords = kSQLInsertRelationWords;

    private const string kSQLSelectByRelationId =
      "SELECT * FROM " + kTableRelation + " " +
      "WHERE " + kFieldId + " = " + "@" + kFieldId;

    private const string kSQLDeleteAllRelationsWordsById =
      "DELETE FROM " + kTableRelationsWords + " " +
      "WHERE " + kFieldRelationId + " = " + "@" + kFieldRelationId;

    private const string kSQLDeleteRelationWords =
      "DELETE FROM " + kTableRelationsWords + " " +
      "WHERE " + kFieldRelationId + " = " + "@" + kFieldRelationId + " AND " +
                 kFieldFirstWordId + " = " + "@" + kFieldFirstWordId + " AND " +
                 kFieldSecondWordId + " = " + "@" + kFieldSecondWordId;

    private const string kSQLDeleteById =
      "DELETE FROM " + kTableRelation + " " +
      "WHERE " + kFieldId + " = " + "@" + kFieldId;

    private const string kSQLSelectRelationWords =
      "SELECT * FROM " + kTableRelationsWords + " " +
      "WHERE " + kFieldRelationId + " = " + "@" + kFieldRelationId;

    /*
        private const string kSQLSelectByName =
          "SELECT " + kTableGroup + "." + kFieldId + ", " +
                      kTableGroup + "." + kFieldName + " " +
          "FROM " + kTableGroup + " " +
          "WHERE LOWER(" + kTableGroup + ".name) like = LOWER(%{0}%)";
        private const string kSQLSelectRelationWords =
          "SELECT DISTINCT " + kFieldRelationId + " " +
          "FROM groups_words " +
          "WHERE " + kFieldGroupId + " IN ({0})";
    */

    private const string kSQLCreateTableRelation =
      "CREATE TABLE `relation` ( " +
        "`id` int(11) NOT NULL AUTO_INCREMENT, " +
        "`name` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL, " +
        "PRIMARY KEY(`name`), " +
        "UNIQUE KEY `id_UNIQUE` (`id`) " +
      ") ENGINE=InnoDB AUTO_INCREMENT = 0 DEFAULT CHARSET = utf8 COLLATE=utf8_bin;";

    private const string kSQLCreateTableRelationsWords =
      "CREATE TABLE `relations_words` ( " +
        "`relation_id` int(11) NOT NULL, " +
        "`first_word_id` int(11) NOT NULL, " +
        "`second_word_id` int(11) NOT NULL, " +
        "PRIMARY KEY(`relation_id`,`first_word_id`,`second_word_id`), " +
        "KEY `first_word_id_idx` (`first_word_id`), " +
        "KEY `second_word_id_idx` (`second_word_id`), " +
        "KEY `relation_id_idx` (`relation_id`), " +
        "CONSTRAINT `fwid` FOREIGN KEY(`first_word_id`) REFERENCES `word` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION, " +
        "CONSTRAINT `rid` FOREIGN KEY(`relation_id`) REFERENCES `relation` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION, " +
        "CONSTRAINT `swid` FOREIGN KEY(`second_word_id`) REFERENCES `word` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION " +
      ") ENGINE=InnoDB DEFAULT CHARSET=utf8;";

    #endregion

    //--------------------------------------------------------------------------
    // used to export to xml
    public static void FillDataSet(DataSet ds) {
      DaoUtils.FillDataSet(kSQLSelectAll, kTableRelation, ds);
      DaoUtils.FillDataSet(kSQLSelectAllRelationsWords, 
                           kTableRelationsWords, 
                           ds);
    }

    //--------------------------------------------------------------------------
    public static void DropTable() {
      DaoUtils.DropTable(kTableRelationsWords);
      DaoUtils.DropTable(kTableRelation);
    }

    //--------------------------------------------------------------------------
    public static void CreateTable() {
      DaoUtils.ExecuteNonQuery(kSQLCreateTableRelation);
      DaoUtils.ExecuteNonQuery(kSQLCreateTableRelationsWords);
    }

    //--------------------------------------------------------------------------
    private static Relation MapRelation(MySqlDataReader reader) {
      Relation relation = new Relation() {
        Id = reader.GetInt32(kFieldId),
        Name = reader.GetString(kFieldName)
      };

      return relation;
    }

    //--------------------------------------------------------------------------
    public static List<Relation> GetAll() {
      List<Relation> relations = new List<Relation>();

      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectAll, DatabaseConnectionService.Instance.Connection);
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Relation relation = MapRelation(reader);
          relations.Add(relation);
        }

        return relations;
      } catch (Exception ex) {
        throw ex;
      } finally {
        if (reader != null) {
          reader.Close();
        }
      }
    }

    //--------------------------------------------------------------------------
    public static void AddRelationWords(Relation relation, 
                                        Word firstWord, 
                                        Word secondWord) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLInsertRelationWords, 
          DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldRelationId, relation.Id);
        cmd.Parameters.AddWithValue("@" + kFieldFirstWordId, firstWord.Id);
        cmd.Parameters.AddWithValue("@" + kFieldSecondWordId, secondWord.Id);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static void DeleteRelationWords(Relation relation,
                                           Word firstWord,
                                           Word secondWord) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLDeleteRelationWords, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldRelationId, relation.Id);
        cmd.Parameters.AddWithValue("@" + kFieldFirstWordId, firstWord.Id);
        cmd.Parameters.AddWithValue("@" + kFieldSecondWordId, secondWord.Id);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }


    //--------------------------------------------------------------------------
    /*public static List<long> GetWordIds(List<Group> groups) {
      StringBuilder groupsInClauseTemp = new StringBuilder();
      foreach (Group group in groups) {
        groupsInClauseTemp.Append(group.Id.ToString()).Append(',');
      }

      string groupsInClause = "";
      if (groupsInClauseTemp.Length > 0) {
        groupsInClause = groupsInClauseTemp.ToString(
          0, groupsInClauseTemp.Length - 1);
      }

      List<long> wordIds = new List<long>();

      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          String.Format(kSQLSelectGroupsWords, groupsInClause),
          DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          wordIds.Add(reader.GetInt32(0));
        }

        return wordIds;
      } catch (Exception ex) {
        throw ex;
      } finally {
        if (reader != null) {
          reader.Close();
        }
      }
    }*/

    //--------------------------------------------------------------------------
    public static void Insert(ref Relation relation) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLInsertRelation, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldName, relation.Name);

        cmd.ExecuteNonQuery();
        relation.Id = cmd.LastInsertedId;
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static Relation GetRelationById(long id) {
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectByRelationId, 
          DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, id);
        reader = cmd.ExecuteReader();

        if (reader.Read()) {
          Relation relation = MapRelation(reader);
          return relation;
        }

        return null;
      } catch (Exception ex) {
        throw ex;
      } finally {
        if (reader != null) {
          reader.Close();
        }
      }
    }

    //--------------------------------------------------------------------------
    public static void DeleteAllRelationsWords (Relation relation) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLDeleteAllRelationsWordsById,
          DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldRelationId, relation.Id);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static bool Delete(Relation relation) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLDeleteById, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, relation.Id);

        return (0 < cmd.ExecuteNonQuery());
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static List<Tuple<long, long>> GetWordIdPairs(Relation relation) {
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectRelationWords,
          DatabaseConnectionService.Instance.Connection);

        cmd.Parameters.AddWithValue("@" + kFieldRelationId, relation.Id);


        List<Tuple<long, long>> result = new List<Tuple<long, long>>();
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Tuple<long, long> wordsPair =
            new Tuple<long, long>(reader.GetInt32(1),
                                  reader.GetInt32(2));
          result.Add(wordsPair);
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
    public static Relation ImportRelation(XmlNode xmlRelation) {
      /*
        <id>1</id>
        <name>rhymes</name>
      */
      Relation relation = new Relation() {
        Id = Int32.Parse(
          xmlRelation.SelectSingleNode("./" + kFieldId).InnerText),
        Name = xmlRelation.SelectSingleNode("./" + kFieldName).InnerText
      };

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLImportRelation, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, relation.Id);
        cmd.Parameters.AddWithValue("@" + kFieldName, relation.Name);

        cmd.ExecuteNonQuery();

        return relation;
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static void ImportRelationWord(XmlNode xmlRelationWord) {
      /*
        <relation_id>1</relation_id>
        <first_word_id>2779</first_word_id>
        <second_word_id>3673</second_word_id>
      */
      long relationId = Int32.Parse(
        xmlRelationWord.SelectSingleNode("./" + kFieldRelationId).InnerText);
      long firstWordId = Int32.Parse(
        xmlRelationWord.SelectSingleNode("./" + kFieldFirstWordId).InnerText);
      long secondWordId = Int32.Parse(
        xmlRelationWord.SelectSingleNode("./" + kFieldSecondWordId).InnerText);

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLImportRelationWords, 
          DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldRelationId, relationId);
        cmd.Parameters.AddWithValue("@" + kFieldFirstWordId, firstWordId);
        cmd.Parameters.AddWithValue("@" + kFieldSecondWordId, secondWordId);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

  }
}
