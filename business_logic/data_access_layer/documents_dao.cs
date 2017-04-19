using books.business_logic.models;
using books.business_logic.services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;

namespace books.business_logic.data_access_layer {
  //----------------------------------------------------------------------------
  public class DocumentsDao {

    #region Constants

    private const string kTableDocument = "document";

    internal const string kFieldId = "id";
    internal const string kFieldGutenbergId = "gutenberg_id";
    internal const string kFieldTitle = "title";
    internal const string kFieldAuthor = "author";
    internal const string kFieldLocalFilename = "local_filename";
    internal const string kFieldReleaseDate = "release_date";
    internal const string kFieldLoadState = "load_state";

    private const string kSQLSelectAll = 
      "SELECT * FROM document " + 
      "WHERE document.load_state = 1";
    private const string kSQLSelectCountAll =
      "SELECT COUNT(*) FROM document " + 
       "WHERE document.load_state = 1";

    private const string kSQLInsertDocument =
      "INSERT INTO document(" +
        kFieldGutenbergId + ", " +
        kFieldTitle + ", " +
        kFieldAuthor + ", " +
        kFieldLocalFilename + ", " +
        kFieldReleaseDate + ", " + 
        kFieldLoadState + ") " +
      "VALUES(" +
        "@" + kFieldGutenbergId + ", " +
        "@" + kFieldTitle + ", " +
        "@" + kFieldAuthor + ", " +
        "@" + kFieldLocalFilename + ", " +
        "@" + kFieldReleaseDate + ", " +
        "@" + kFieldLoadState + ")";

    private const string kSQLImportDocument =
      "INSERT INTO document(" +
        kFieldId + ", " +
        kFieldGutenbergId + ", " +
        kFieldTitle + ", " +
        kFieldAuthor + ", " +
        kFieldLocalFilename + ", " +
        kFieldReleaseDate + ", " +
        kFieldLoadState + ") " +
      "VALUES(" +
        "@" + kFieldId + ", " +
        "@" + kFieldGutenbergId + ", " +
        "@" + kFieldTitle + ", " +
        "@" + kFieldAuthor + ", " +
        "@" + kFieldLocalFilename + ", " +
        "@" + kFieldReleaseDate + ", " +
        "@" + kFieldLoadState + ")";

    private const string kSQLUpdateDocument =
      "UPDATE document " +
      "SET " + kFieldLoadState + "=" + "@" + kFieldLoadState + " " +
      "WHERE " + kFieldId + " = " + "@" + kFieldId;

    private const string kSQLSelectByDocumentId =
      "SELECT * FROM document " +
      "WHERE " + kFieldId + " = " + "@" + kFieldId;

    private const string kSQLSelectByGutenbergId = 
      "SELECT * FROM document " + 
      "WHERE " + kFieldGutenbergId + " = " + "@" + kFieldGutenbergId;

    private const string kSQLDeleteById =
      "DELETE FROM document " +
      "WHERE " + kFieldGutenbergId + " = " + "@" + kFieldGutenbergId;

    // we want distinct documents that contain specific words in them
    private const string kSQLSelectByWords =
      "SELECT DISTINCT document." + kFieldId + ", " +
                      "document." + kFieldGutenbergId + ", " +
                      "document." + kFieldTitle + ", " + 
                      "document." + kFieldAuthor + ", " +
                      "document." + kFieldLocalFilename + ", " +
                      "document." + kFieldReleaseDate + ", " +
                      "document." + kFieldLoadState + " " +
      "FROM document, word, contains " +
      "WHERE document.load_state = 1 AND " + // only fully loaded documents
            "document.id = contains.doc_id AND " +
            "word.id = contains.word_id AND " +
            "word.value IN ({0})";

    // we want distinct documents that contain specific properties
    // and/or words
    private const string kSQLQuery =
      "SELECT DISTINCT document." + kFieldId + ", " +
                      "document." + kFieldGutenbergId + ", " +
                      "document." + kFieldTitle + ", " +
                      "document." + kFieldAuthor + ", " +
                      "document." + kFieldLocalFilename + ", " +
                      "document." + kFieldReleaseDate + ", " +
                      "document." + kFieldLoadState + " " +
      "FROM document, word, contains " +
      "WHERE document.load_state = 1 AND " + // only fully loaded documents
            "document.id = contains.doc_id AND " +
            "word.id = contains.word_id";

    // we select all documents that contain all the given words in them
    // and then filter on top of that
    private const string kSQLQueryWithWordsFilter =
      "SELECT * FROM (" +
        "SELECT document.*, COUNT(DISTINCT word.id) w_count " +
        "FROM document, word, contains " +
        "WHERE document.load_state = 1 AND " +
        "document.id = contains.doc_id AND " +
        "word.id = contains.word_id AND " +
        "word.value IN ({0}) " +
        "GROUP BY document.id) AS temp " +
      "WHERE temp.w_count = {1}";

    private const string kSQLCreateTable =
      "CREATE TABLE `document` ( " +
        "`id` int(11) NOT NULL AUTO_INCREMENT, " +
        "`gutenberg_id` varchar(255) NOT NULL, " +
        "`title` varchar(2000) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL, " +
        "`author` varchar(200) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL, " +
        "`local_filename` varchar(255) DEFAULT NULL, " +
        "`release_date` date DEFAULT NULL, " +
        "`load_state` int(11) NOT NULL, " +
        "PRIMARY KEY(`gutenberg_id`), " +
        "UNIQUE KEY `id_UNIQUE` (`id`) " +
      ") ENGINE=InnoDB AUTO_INCREMENT = 0 DEFAULT CHARSET = utf8 COLLATE=utf8_bin;";

    #endregion

    //--------------------------------------------------------------------------
    private static Document MapDocument(MySqlDataReader reader) {
      Document document = new Document() {
        Id = reader.GetInt32(kFieldId),
        GutenbergId = reader.GetString(kFieldGutenbergId),
        Title = reader.GetString(kFieldTitle),
        Author = reader.GetString(kFieldAuthor),
        LocalFile = new FileInfo(reader.GetString(kFieldLocalFilename)),
        ReleaseDate = DaoUtils.SafeGetDateTime(reader, kFieldReleaseDate),
        TableLoadState = (Document.LoadState)reader.GetInt32(kFieldLoadState)
      };

      return document;
    }

    //--------------------------------------------------------------------------
    // used to export to xml
    public static void FillDataSet(DataSet ds) {
      DaoUtils.FillDataSet(kSQLSelectAll, kTableDocument, ds);
    }

    //--------------------------------------------------------------------------
    public static void DropTable() {
      DaoUtils.DropTable(kTableDocument);
    }

    //--------------------------------------------------------------------------
    public static void CreateTable() {
      DaoUtils.ExecuteNonQuery(kSQLCreateTable);
    }

    //--------------------------------------------------------------------------
    public static void GetAll(out List<Document> documents) {
      documents = new List<Document>();

      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectAll, DatabaseConnectionService.Instance.Connection);
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Document document = MapDocument(reader);
          documents.Add(document);
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
    /// <summary>
    /// inserts a new document (the id is ignored) and sets the id if succeeds
    /// </summary>
    /// <param name="document"></param>
    public static void Insert(Document document) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLInsertDocument, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue(
          "@" + kFieldGutenbergId, document.GutenbergId);
        cmd.Parameters.AddWithValue(
          "@" + kFieldTitle, document.Title);
        cmd.Parameters.AddWithValue(
          "@" + kFieldAuthor, document.Author);
        cmd.Parameters.AddWithValue(
          "@" + kFieldLocalFilename, document.LocalFile.FullName);
        cmd.Parameters.AddWithValue(
          "@" + kFieldReleaseDate, 
            (document.ReleaseDate.HasValue ?
              document.ReleaseDate.Value.Date.ToString("yyyy-MM-dd") : null));
        cmd.Parameters.AddWithValue(
          "@" + kFieldLoadState, ((int)document.TableLoadState));

        cmd.ExecuteNonQuery();
        document.Id = cmd.LastInsertedId;
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static void Update(Document document) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLUpdateDocument, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue(
          "@" + kFieldLoadState, ((int)document.TableLoadState));
        cmd.Parameters.AddWithValue(
          "@" + kFieldId, document.Id);

        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static Document GetDocumentById(string gutenbergId) {
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectByGutenbergId, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldGutenbergId, gutenbergId);
        reader = cmd.ExecuteReader();

        if (reader.Read()) {
          Document document = MapDocument(reader);
          return document;
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
    public static Document GetDocumentById(long documentId) {
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectByDocumentId, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, documentId);
        reader = cmd.ExecuteReader();

        if (reader.Read()) {
          Document document = MapDocument(reader);
          return document;
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
    public static bool Delete(Document document) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLDeleteById, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue(
          "@" + kFieldGutenbergId, document.GutenbergId);
        return (0 < cmd.ExecuteNonQuery());
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    public static List<Document> QueryByWords(List<string> words) {
      var strWords = "\"" + words.Aggregate((x, y) => x + "\",\"" + y) + "\"";
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          String.Format(kSQLSelectByWords, strWords), 
          DatabaseConnectionService.Instance.Connection);

        List<Document> result = new List<Document>();
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Document document = MapDocument(reader);
          result.Add(document);
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
    public static List<Document> Query(
      List<string> words,
      List<DocumentProperty> properties) {
      // build our query
      StringBuilder whereClause = new StringBuilder();

      foreach (DocumentProperty property in properties) {
        whereClause.Append(" AND ")
                   .Append(GetWhereClauseFromProperty(property));
      }

      // the basic query without words filtering is kSQLQuery
      string sqlQuery = kSQLQuery;

      // possible word constraints
      if (words.Count > 0) {
        var strWords = "\"" + words.Aggregate((x, y) => x + "\",\"" + y) + "\"";
        // if we do have words, we use another query
        sqlQuery = String.Format(kSQLQueryWithWordsFilter, 
                                 strWords, 
                                 words.Count);
      }

      // ad the document filtering anyway
      sqlQuery += whereClause.ToString();

      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          sqlQuery,
          DatabaseConnectionService.Instance.Connection);

        List<Document> result = new List<Document>();
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Document document = MapDocument(reader);
          result.Add(document);
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

    //-------------------------------------------------------------------------
    public static Document Import(XmlNode xmlDocument) {
      /*  
        <id>12</id>
        <gutenberg_id>1342</gutenberg_id>
        <title>Pride and Prejudice</title>
        <author>Jane Austen</author>
        <local_filename>d:\release\txt files\storage\1342.txt</local_filename>
        <release_date>1998-06-01T00:00:00+03:00</release_date>
        <load_state>1</load_state>
      */
      Document document = new Document() {
        Id = Int32.Parse(xmlDocument.SelectSingleNode("./" + kFieldId).InnerText),
        GutenbergId = xmlDocument.SelectSingleNode("./" + kFieldGutenbergId).InnerText,
        Title = xmlDocument.SelectSingleNode("./" + kFieldTitle).InnerText,
        Author = xmlDocument.SelectSingleNode("./" + kFieldAuthor).InnerText,
        LocalFile = new FileInfo(xmlDocument.SelectSingleNode("./" + kFieldLocalFilename).InnerText),
        ReleaseDate = DateTime.Parse(xmlDocument.SelectSingleNode("./" + kFieldReleaseDate).InnerText),
        TableLoadState = (Document.LoadState)Int32.Parse(xmlDocument.SelectSingleNode("./" + kFieldLoadState).InnerText)
      };

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLImportDocument, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldId, document.Id);
        cmd.Parameters.AddWithValue(
          "@" + kFieldGutenbergId, document.GutenbergId);
        cmd.Parameters.AddWithValue(
          "@" + kFieldTitle, document.Title);
        cmd.Parameters.AddWithValue(
          "@" + kFieldAuthor, document.Author);
        cmd.Parameters.AddWithValue(
          "@" + kFieldLocalFilename, document.LocalFile.FullName);
        cmd.Parameters.AddWithValue(
          "@" + kFieldReleaseDate,
            (document.ReleaseDate.HasValue ?
              document.ReleaseDate.Value.Date.ToString("yyyy-MM-dd") : null));
        cmd.Parameters.AddWithValue(
          "@" + kFieldLoadState, ((int)document.TableLoadState));

        cmd.ExecuteNonQuery();

        return document;
      } catch (Exception ex) {
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    private static string GetWhereClauseFromProperty(
      DocumentProperty property) {
      string fieldName = "";
      // startup with taking the string value, we override it if necessary
      string value = property.StrValue;

      switch(property.Field) {
        case DocumentProperty.Property.GutenbergId:
          fieldName = kFieldGutenbergId;
          break;
        case DocumentProperty.Property.Title:
          fieldName = kFieldTitle;
          break;
        case DocumentProperty.Property.Author:
          fieldName = kFieldAuthor;
          break;
        case DocumentProperty.Property.ReleaseDate:
          fieldName = kFieldReleaseDate;
          value = property.DateTimeValue.Date.ToString("yyyy-MM-dd");
          break;
      }

      return String.Format("LOWER({0}) like \"%{1}%\"", 
                           fieldName, 
                           value.ToLower());
    }

  }
}
