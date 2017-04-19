using books.business_logic.models;
using books.business_logic.services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace books.business_logic.data_access_layer {
  //----------------------------------------------------------------------------
  public class GroupsDao {

    #region Constants

    internal const string kFieldId = "id";
    internal const string kFieldName = "name";
    internal const string kFieldGroupId = "group_id";
    internal const string kFieldWordId = "word_id";

    internal const string kTableGroup = "`group`";
    internal const string kTableGroupsWords = "`groups_words`";

    private const string kSQLSelectAll = "SELECT * FROM " + kTableGroup;
    private const string kSQLSelectAllGroupWords = 
      "SELECT * FROM " + kTableGroupsWords;

    private const string kSQLSelectCountAll = 
      "SELECT COUNT(*) FROM " + kTableGroup;

    private const string kSQLInsertGroup =
      "INSERT INTO " + kTableGroup + "(" + kFieldName + ") " +
      "VALUES(@" + kFieldName + ")";
    
    private const string kSQLImportGroup =
      "INSERT INTO " + kTableGroup + "(" + kFieldId + ", " + kFieldName + ") " +
      "VALUES(@" + kFieldId + ", @" + kFieldName + ")";
    
    private const string kSQLInsertGroupWord =
      "INSERT INTO " + kTableGroupsWords + 
                      "(" + kFieldGroupId + ", " + kFieldWordId + ") " +
      "VALUES(@" + kFieldGroupId + ", @" + kFieldWordId + ")";

    private const string kSQLImportGroupWord = kSQLInsertGroupWord;

    private const string kSQLUpdateGroup =
      "UPDATE " + kTableGroup + " " +
      "SET " + kFieldName + "=" + "@" + kFieldName + " " +
      "WHERE " + kFieldId + " = " + "@" + kFieldId;

    private const string kSQLSelectByGroupId =
      "SELECT * FROM " + kTableGroup + " " +
      "WHERE " + kFieldId + " = " + "@" + kFieldId;

    private const string kSQLDeleteAllWordsById =
      "DELETE FROM " + kTableGroupsWords + " " +
      "WHERE " + kFieldGroupId + " = " + "@" + kFieldGroupId;

    private const string kSQLDeleteGroupWord =
      "DELETE FROM " + kTableGroupsWords + " " +
      "WHERE " + kFieldGroupId + " = " + "@" + kFieldGroupId + " AND " +
                 kFieldWordId + " = " + "@" + kFieldWordId;

    private const string kSQLDeleteById =
      "DELETE FROM " + kTableGroup + " " +
      "WHERE " + kFieldId + " = " + "@" + kFieldId;

    private const string kSQLSelectByName =
      "SELECT " + kTableGroup + "." + kFieldId + ", " +
                  kTableGroup + "." + kFieldName + " " +
      "FROM " + kTableGroup + " " +
      "WHERE LOWER(" + kTableGroup + ".name) like = LOWER(%{0}%)";

    private const string kSQLSelectGroupsWords =
      "SELECT DISTINCT " + kFieldWordId + " " +
      "FROM groups_words " +
      "WHERE " + kFieldGroupId + " IN ({0})";

    private const string kSQLCreateTableGroup =
      "CREATE TABLE `group` ( " +
        "`id` int(11) NOT NULL AUTO_INCREMENT, " +
        "`name` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL, " +
        "PRIMARY KEY(`name`), " +
        "UNIQUE KEY `id_UNIQUE` (`id`) " +
      ") ENGINE=InnoDB AUTO_INCREMENT = 0 DEFAULT CHARSET = utf8 COLLATE=utf8_bin;";

    private const string kSQLCreateTableGroupsWords =
      "CREATE TABLE `groups_words` ( " +
        "`group_id` int(11) NOT NULL, " +
        "`word_id` int(11) NOT NULL, " +
        "KEY `word_id_idx` (`word_id`), " +
        "KEY `group_id_idx` (`group_id`), " +
        "CONSTRAINT `gid` FOREIGN KEY(`group_id`) REFERENCES `group` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION, " +
        "CONSTRAINT `wid` FOREIGN KEY(`word_id`) REFERENCES `word` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION " +
      ") ENGINE=InnoDB DEFAULT CHARSET=utf8;";

    #endregion

    //--------------------------------------------------------------------------
    // used to export to xml
    public static void FillDataSet(DataSet ds) {
      DaoUtils.FillDataSet(kSQLSelectAll, kTableGroup.Replace("`", ""), ds);
      DaoUtils.FillDataSet(kSQLSelectAllGroupWords, 
                           kTableGroupsWords.Replace("`", ""), ds);
    }

    //--------------------------------------------------------------------------
    public static void DropTable() {
      DaoUtils.DropTable(kTableGroupsWords);
      DaoUtils.DropTable(kTableGroup);
    }

    //--------------------------------------------------------------------------
    public static void CreateTable() {
      DaoUtils.ExecuteNonQuery(kSQLCreateTableGroup);
      DaoUtils.ExecuteNonQuery(kSQLCreateTableGroupsWords);
    }

    //--------------------------------------------------------------------------
    private static Group MapGroup(MySqlDataReader reader) {
      Group group = new Group() {
        Id = reader.GetInt32(kFieldId),
        Name = reader.GetString(kFieldName)
      };

      return group;
    }

    //--------------------------------------------------------------------------
    public static List<Group> GetAll() {
      List<Group> groups = new List<Group>();

      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectAll, DatabaseConnectionService.Instance.Connection);
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Group group = MapGroup(reader);
          groups.Add(group);
        }

        return groups;
      } catch (Exception ex) {
        throw ex;
      } finally {
        if (reader != null) {
          reader.Close();
        }
      }
    }

    //--------------------------------------------------------------------------
    public static void AddGroupWord(Group group, Word word) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLInsertGroupWord, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldGroupId, group.Id);
        cmd.Parameters.AddWithValue("@" + kFieldWordId, word.Id);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static void DeleteGroupWord(Group group, Word word) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLDeleteGroupWord, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldGroupId, group.Id);
        cmd.Parameters.AddWithValue("@" + kFieldWordId, word.Id);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }


    //--------------------------------------------------------------------------
    public static List<long> GetWordIds(List<Group> groups) {
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
    }

    //--------------------------------------------------------------------------
    public static void Insert(ref Group group) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLInsertGroup, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldName, group.Name);

        cmd.ExecuteNonQuery();
        group.Id = cmd.LastInsertedId;
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static void Update(Group group) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLUpdateGroup, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldName, group.Name);
        cmd.Parameters.AddWithValue("@" + kFieldId, group.Id);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static Group GetGroupById(long id) {
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectByGroupId, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, id);
        reader = cmd.ExecuteReader();

        if (reader.Read()) {
          Group group = MapGroup(reader);
          return group;
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
    public static void DeleteAllGroupWords(Group group) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLDeleteAllWordsById, 
          DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldGroupId, group.Id);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static bool Delete(Group group) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLDeleteById, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, group.Id);

        return (0 < cmd.ExecuteNonQuery());
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static List<Group> Query(string name) { 
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          String.Format(kSQLSelectByName, name),
          DatabaseConnectionService.Instance.Connection);

        List<Group> result = new List<Group>();
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Group group = MapGroup(reader);
          result.Add(group);
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
    public static Group ImportGroup(XmlNode xmlGroup) {
      /*
        <id>1</id>
        <name>countries</name>
      */
      Group group = new Group() {
        Id = Int32.Parse(
          xmlGroup.SelectSingleNode("./" + kFieldId).InnerText),
        Name = xmlGroup.SelectSingleNode("./" + kFieldName).InnerText
      };

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLImportGroup, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, group.Id);
        cmd.Parameters.AddWithValue("@" + kFieldName, group.Name);

        cmd.ExecuteNonQuery();

        return group;
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static void ImportGroupWord(XmlNode xmlGroupWord) {
      /*
        <id>1</id>
        <name>countries</name>
      */
      long groupId = Int32.Parse(
        xmlGroupWord.SelectSingleNode("./" + kFieldGroupId).InnerText);
      long wordId = Int32.Parse(
        xmlGroupWord.SelectSingleNode("./" + kFieldWordId).InnerText);

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLImportGroupWord, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldGroupId, groupId);
        cmd.Parameters.AddWithValue("@" + kFieldWordId, wordId);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

  }
}
