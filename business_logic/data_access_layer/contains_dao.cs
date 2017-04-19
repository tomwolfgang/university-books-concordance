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
  //---------------------------------------------------------------------------
  public class ContainsDao {

    #region Constants

    internal const string kTableContains = "contains";

    internal const string kFieldDocumentId = "doc_id";
    internal const string kFieldWordId = "word_id";
    internal const string kFieldLine = "line";
    internal const string kFieldFileOffset = "file_offset";
    internal const string kFieldSentence = "sentence";
    internal const string kFieldIndexInSentence = "index_in_sentence";
    internal const string kFieldParagraph = "paragraph";
    internal const string kFieldPage = "page";

    internal const string kFieldWordCount = "w_count";

    private const string kSQLInsertContains =
      "INSERT INTO contains(" +
        kFieldDocumentId + ", " +
        kFieldWordId + ", " +
        kFieldLine + ", " +
        kFieldFileOffset + ", " +
        kFieldIndexInSentence + ", " +
        kFieldSentence + ", " +
        kFieldParagraph + ", " +
        kFieldPage + ") " +
      "VALUES(" +
        "@" + kFieldDocumentId + ", " +
        "@" + kFieldWordId + ", " +
        "@" + kFieldLine + ", " +
        "@" + kFieldFileOffset + ", " +
        "@" + kFieldIndexInSentence + ", " +
        "@" + kFieldSentence + ", " +
        "@" + kFieldParagraph + ", " +
        "@" + kFieldPage + ")";

    private const string kSQLImportContains = kSQLInsertContains;


    private const string kSQLDeleteByDocumentId =
      "DELETE FROM contains " +
      "WHERE " + kFieldDocumentId + " = " + "@" + kFieldDocumentId;

    private const string kSQLSelectCountAll = "SELECT COUNT(*) FROM contains";

    private const string kSQLSelectUniqueWordsCount = 
      "SELECT count(distinct " + kFieldWordId + ") FROM contains";

    private const string kSQLSelectAll =
      "SELECT * " +
      "FROM contains";

    private const string kSQLSelectDistinctWordIds =
      "SELECT DISTINCT " + kFieldWordId + " " +
      "FROM contains, document " +
      "WHERE contains." + kFieldDocumentId + " = document." + DocumentsDao.kFieldId + 
      " AND document." + DocumentsDao.kFieldLoadState + " = 1";

    private const string kSQLQueryWordInDocuments =
      "SELECT * " +
      "FROM contains " +
      "WHERE " + kFieldWordId + " = @" + kFieldWordId;

    private const string kSQLQueryGroup =
      "SELECT contains.* " +
      "FROM contains, " + GroupsDao.kTableGroupsWords + " " +
      "WHERE " + GroupsDao.kTableGroupsWords + "." + GroupsDao.kFieldGroupId +
              " = @" + GroupsDao.kFieldGroupId + " AND " +
              "contains." + kFieldWordId + " = " +
                GroupsDao.kTableGroupsWords + "." + GroupsDao.kFieldWordId;

    /*
      SELECT * FROM CONTAINS
      WHERE 
	      DOC_ID = 4 AND 
        ((SENTENCE >= 116 AND SENTENCE <= 118) OR 
         (SENTENCE = 119 AND file_offset IN (
              SELECT MIN(file_offset) 
              FROM CONTAINS 
              WHERE DOC_ID = 4 AND SENTENCE = 119)))
     */
    private const string kSQLQuerySurroundingSentencesOffsets =
      "SELECT MIN(" + kFieldFileOffset + "), MAX(" + kFieldFileOffset + ") " +
      "FROM contains " +
      "WHERE " +
        kFieldDocumentId + " = @" + kFieldDocumentId + " AND " +
        "(" +
          "(" + kFieldSentence + " >= @sentence_before AND " +
                kFieldSentence + " <= @sentence_after) OR " +
            "(" + kFieldSentence + " = @sentence_after_sentence_after AND " +
              kFieldFileOffset + " IN (" +
                      "SELECT MIN(" + kFieldFileOffset + ") " +
                      "FROM CONTAINS " +
                      "WHERE " +
                        kFieldDocumentId + " = @" + kFieldDocumentId + " AND " +
                        kFieldSentence + " = @sentence_after_sentence_after))" +
        ")";

    // NOTE: this trick doesn't work as well with lines because, if for example,
    // we have a line that is followed by an empty line (a blank line) then we
    // won't have any @line_after_line_after in the DB and thus, we'll be 
    // missing the offset of the ending of the last word (which should have been
    // the offset of the first word on the next line)
    // we could fix this by taking a range of lines after the @line_after - but
    // I think that using the sentences approach is more "elgant" than lines,
    // so I decided not to implement it
    private const string kSQLQuerySurroundingLinesOffsets =
      "SELECT MIN(" + kFieldFileOffset + "), MAX(" + kFieldFileOffset + ") " +
      "FROM contains " +
      "WHERE " +
        kFieldDocumentId + " = @" + kFieldDocumentId + " AND " +
        "(" +
          "(" + kFieldLine + " >= @line_before AND " +
                kFieldLine + " <= @line_after) OR " +
          "(" + kFieldLine + " = @line_after_line_after AND " +
            kFieldFileOffset + " IN (" +
                    "SELECT MIN(" + kFieldFileOffset + ") " +
                    "FROM CONTAINS " +
                    "WHERE " +
                      kFieldDocumentId + " = @" + kFieldDocumentId + " AND " +
                      kFieldLine + " = @line_after_line_after))" +
        ")";

    /*
    We basically want to search for an entire phrase (built of N words) in
    the contains table.
    When approach would be to have an N joins query - however, since we have a
    very large table (tested with a million indexed words) - this would take
    a very long time to query.

    Instead, we take a different approach: first filter documents with sentences 
    that contain ALL of the phrase words (each word should appear at least once 
    in the sentence).

    This should give us a much smaller table to work with (of sentences with
    the potential phrase) - we then query the entire sentence (all the words of
    the sentences) and, with code, check that they match the phrase we want

    For example - say we have a phrase of 6 words:

    SELECT * FROM contains
    WHERE (doc_id, sentence) IN (
      SELECT doc_id, sentence
      FROM (
        SELECT doc_id, sentence, COUNT(distinct word_id) w_count
        FROM contains
        WHERE word_id in (5377,16295,...) // which exact 6 words
        GROUP BY doc_id, sentence
      ) AS docs_sentences_with_words
      WHERE docs_sentences_with_words.w_count = 6) // contains ALL of the 6
    AND word_id IN (5377,16295,...)
    ORDER BY doc_id, sentence, index_in_sentence
    */
    private const string kSQLQueryPotentialPhrases =
      "SELECT * FROM contains " +
      "WHERE(" + kFieldDocumentId + ", " + kFieldSentence + ") IN (" +
        "SELECT " + kFieldDocumentId + ", " + kFieldSentence + " " +
        "FROM (" +
          "SELECT " + kFieldDocumentId + ", " + kFieldSentence + ", " +
                "COUNT(distinct " + kFieldWordId + ") w_count " +
          "FROM contains " +
          "WHERE " + kFieldWordId + " IN ({0}) " +
          "GROUP BY " + kFieldDocumentId + ", " + kFieldSentence + " " +
        ") AS docs_sentences_with_words " +
        "WHERE docs_sentences_with_words.w_count = {1}) " +
      "AND " + kFieldWordId + " IN ({0}) " +
      "ORDER BY " + kFieldDocumentId + ", " + 
                    kFieldSentence + ", " + 
                    kFieldIndexInSentence;

    private const string kSQLCreateTable =
      "CREATE TABLE `contains` ( " +
        "`doc_id` int(11) NOT NULL, " +
        "`word_id` int(11) NOT NULL, " +
        "`line` int(11) NOT NULL, " +
        "`file_offset` int(11) NOT NULL, " +
        "`sentence` int(11) NOT NULL, " +
        "`index_in_sentence` int(11) NOT NULL, " +
        "`paragraph` int(11) NOT NULL, " +
        "`page` int(11) NOT NULL, " +
        "KEY `word_id_idx` (`word_id`), " +
        "KEY `doc_id_idx` (`doc_id`), " +
        "CONSTRAINT `doc_id` FOREIGN KEY(`doc_id`) REFERENCES `document` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION, " +
        "CONSTRAINT `word_id` FOREIGN KEY(`word_id`) REFERENCES `word` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION " +
      ") ENGINE=InnoDB DEFAULT CHARSET=utf8;";
    
    #endregion

    //--------------------------------------------------------------------------
    // used to export to xml
    public static void FillDataSet(DataSet ds) {
      DaoUtils.FillDataSet(kSQLSelectAll, kTableContains, ds);
    }

    //--------------------------------------------------------------------------
    public static void DropTable() {
      DaoUtils.DropTable(kTableContains);
    }

    //--------------------------------------------------------------------------
    public static void CreateTable() {
      DaoUtils.ExecuteNonQuery(kSQLCreateTable);
    }

    //--------------------------------------------------------------------------
    public static Contains Import(XmlNode xmlContains) {
/*    <doc_id>1</doc_id>
    <word_id>1</word_id>
    <line>24</line>
    <file_offset>643</file_offset>
    <sentence>9</sentence>
    <index_in_sentence>0</index_in_sentence>
    <paragraph>9</paragraph>
    <page>0</page>*/

      Contains contains = new Contains() {
        DocumentId = Int32.Parse(
          xmlContains.SelectSingleNode("./" + kFieldDocumentId).InnerText),
        WordId = Int32.Parse(
          xmlContains.SelectSingleNode("./" + kFieldWordId).InnerText),
        Line = Int32.Parse(
          xmlContains.SelectSingleNode("./" + kFieldLine).InnerText),
        FileOffset = Int32.Parse(
          xmlContains.SelectSingleNode("./" + kFieldFileOffset).InnerText),
        IndexInSentence = Int32.Parse(
          xmlContains.SelectSingleNode("./" + kFieldIndexInSentence).InnerText),
        Sentence = Int32.Parse(
          xmlContains.SelectSingleNode("./" + kFieldSentence).InnerText),
        Paragraph = Int32.Parse(
          xmlContains.SelectSingleNode("./" + kFieldParagraph).InnerText),
        Page = Int32.Parse(
          xmlContains.SelectSingleNode("./" + kFieldPage).InnerText),
      };

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLImportContains, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue(
          "@" + kFieldDocumentId, contains.DocumentId);
        cmd.Parameters.AddWithValue("@" + kFieldWordId, contains.WordId);
        cmd.Parameters.AddWithValue("@" + kFieldLine, contains.Line);
        cmd.Parameters.AddWithValue(
          "@" + kFieldFileOffset, contains.FileOffset);
        cmd.Parameters.AddWithValue(
          "@" + kFieldIndexInSentence, contains.IndexInSentence);
        cmd.Parameters.AddWithValue("@" + kFieldSentence, contains.Sentence);
        cmd.Parameters.AddWithValue("@" + kFieldParagraph, contains.Paragraph);
        cmd.Parameters.AddWithValue("@" + kFieldPage, contains.Page);

        cmd.ExecuteNonQuery();

        return contains;
      } catch (Exception ex) {
        throw ex;
      }
    }

    //-------------------------------------------------------------------------
    private static Contains MapContains(MySqlDataReader reader) {
      Contains contains = new Contains() {
        DocumentId = reader.GetInt32(kFieldDocumentId),
        WordId = reader.GetInt32(kFieldWordId),
        Line = reader.GetInt32(kFieldLine),
        FileOffset = reader.GetInt32(kFieldFileOffset),
        IndexInSentence = reader.GetInt32(kFieldIndexInSentence),
        Sentence = reader.GetInt32(kFieldSentence),
        Paragraph = reader.GetInt32(kFieldParagraph),
        Page = reader.GetInt32(kFieldPage),
      };

      return contains;
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
    public static Int64 GetUniqueWordsCount() {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectUniqueWordsCount, 
          DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        return (Int64)cmd.ExecuteScalar();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //-------------------------------------------------------------------------
    public static void Insert(Contains contains) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLInsertContains, DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue(
          "@" + kFieldDocumentId, contains.DocumentId);
        cmd.Parameters.AddWithValue("@" + kFieldWordId, contains.WordId);
        cmd.Parameters.AddWithValue("@" + kFieldLine, contains.Line);
        cmd.Parameters.AddWithValue(
          "@" + kFieldFileOffset, contains.FileOffset);
        cmd.Parameters.AddWithValue(
          "@" + kFieldIndexInSentence, contains.IndexInSentence);
        cmd.Parameters.AddWithValue("@" + kFieldSentence, contains.Sentence);
        cmd.Parameters.AddWithValue("@" + kFieldParagraph, contains.Paragraph);
        cmd.Parameters.AddWithValue("@" + kFieldPage, contains.Page);
        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

    //-------------------------------------------------------------------------
    public static bool DeleteByDocument(Document document) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLDeleteByDocumentId, 
          DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();

        cmd.Parameters.AddWithValue("@" + kFieldDocumentId, document.Id);
        return (0 < cmd.ExecuteNonQuery());
      } catch (Exception ex) {
        throw ex;
      }
    }

    //-------------------------------------------------------------------------
    public static List<long> QueryWordIds(
      List<Document> documents, List<WordLocationProperty> properties) {

      // build our query
      StringBuilder whereClause = new StringBuilder();
      if (documents.Count > 0) {
        string inDocuments = String.Concat(
          documents.Select(o => "," + o.Id.ToString())).Substring(1);

        whereClause.Append(" AND ").Append(kFieldDocumentId)
                   .Append(" IN (").Append(inDocuments).Append(")");
      }

      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectDistinctWordIds + whereClause.ToString(),
          DatabaseConnectionService.Instance.Connection);

        List<long> result = new List<long>();
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          result.Add(reader.GetInt32(kFieldWordId));
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
    public static List<Contains> Query(List<Document> documents, Word word) {
      // build our query
      StringBuilder whereClause = new StringBuilder();
      if ((documents != null) && (documents.Count > 0)) {
        string inDocuments = String.Concat(
          documents.Select(o => "," + o.Id.ToString())).Substring(1);

        whereClause.Append(" AND ").Append(kFieldDocumentId)
                   .Append(" IN (").Append(inDocuments).Append(")");
      }

      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLQueryWordInDocuments + whereClause.ToString(),
          DatabaseConnectionService.Instance.Connection);

        cmd.Parameters.AddWithValue("@" + kFieldWordId, word.Id);

        List<Contains> result = new List<Contains>();
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Contains contains = MapContains(reader);
          result.Add(contains);
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
    public static List<Contains> Query(Group group) {
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLQueryGroup,
          DatabaseConnectionService.Instance.Connection);

        cmd.Parameters.AddWithValue("@" + GroupsDao.kFieldGroupId, group.Id);

        List<Contains> result = new List<Contains>();
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Contains contains = MapContains(reader);
          result.Add(contains);
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
    // Query for a list of "Contains" objects that are in "documents" and 
    // filtered by wordLocationPropertiesd
    public static List<Contains> Query(
      List<Document> documents, 
      List<WordLocationProperty> wordLocationProperties) {

      // our "WHERE" clause.  The 0 = 0 is a dummy so that our code doesn't
      // have to handle an extra if...else...
      StringBuilder whereClause = new StringBuilder(" WHERE 0 = 0");

      // start with the "documents" filter
      if (documents.Count > 0) {
        string inDocuments = String.Concat(
          documents.Select(o => "," + o.Id.ToString())).Substring(1);

        whereClause.Append(" AND ").Append(kFieldDocumentId)
                   .Append(" IN (").Append(inDocuments).Append(")");
      }

      // now go over the wordLocationProperties
      foreach (WordLocationProperty property in wordLocationProperties) {
        whereClause.Append(" AND ")
                   .Append(GetWhereClauseFromProperty(property));
      }

      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLSelectAll + whereClause.ToString(),
          DatabaseConnectionService.Instance.Connection);

        List<Contains> result = new List<Contains>();
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Contains contains = MapContains(reader);
          result.Add(contains);
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
    public static List<Contains> Query(Phrase phrase) {
      MySqlDataReader reader = null;

      StringBuilder words = new StringBuilder();
      HashSet<long> uniqueWordIds = new HashSet<long>();
      foreach (Word word in phrase.Words) {
        if (!uniqueWordIds.Contains(word.Id)) {
          uniqueWordIds.Add(word.Id);
          words.Append(word.Id.ToString()).Append(",");
        }
      }
      string strWords = words.ToString().Substring(0, words.Length - 1);

      try {
        string query = String.Format(kSQLQueryPotentialPhrases,
                                     strWords,
                                     uniqueWordIds.Count); 
        MySqlCommand cmd = new MySqlCommand(
          query,
          DatabaseConnectionService.Instance.Connection);

        List<Contains> result = new List<Contains>();
        reader = cmd.ExecuteReader();

        while (reader.Read()) {
          Contains contains = MapContains(reader);
          result.Add(contains);
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

    //-------------------------------------------------------------------------
    public static Tuple<long, long> GetSurroundingLinesOffsets(
      Contains location, long linesBefore, long linesAfter) {
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLQuerySurroundingLinesOffsets,
          DatabaseConnectionService.Instance.Connection);

        cmd.Parameters.AddWithValue("@" + kFieldDocumentId, 
                                    location.DocumentId);
        cmd.Parameters.AddWithValue("@line_before", 
                                    location.Line - linesBefore);
        cmd.Parameters.AddWithValue("@line_after",
                                    location.Line + linesAfter);
        cmd.Parameters.AddWithValue("@line_after_line_after",
                                    location.Line + linesAfter + 1);

        reader = cmd.ExecuteReader();

        if (reader.Read()) {
          Tuple<long, long> sorroundingOffsets = 
            new Tuple<long, long>(reader.GetInt32(0),
                                  reader.GetInt32(1));
          return sorroundingOffsets;
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

    //-------------------------------------------------------------------------
    public static Tuple<long, long> GetSurroundingSentencesOffsets(
      Contains location, long sentenceBefore, long sentenceAfter) {
      MySqlDataReader reader = null;

      try {
        MySqlCommand cmd = new MySqlCommand(
          kSQLQuerySurroundingSentencesOffsets,
          DatabaseConnectionService.Instance.Connection);

        cmd.Parameters.AddWithValue("@" + kFieldDocumentId,
                                    location.DocumentId);
        cmd.Parameters.AddWithValue("@sentence_before",
                                    location.Sentence - sentenceBefore);
        cmd.Parameters.AddWithValue("@sentence_after",
                                    location.Sentence + sentenceAfter);
        cmd.Parameters.AddWithValue("@sentence_after_sentence_after",
                                    location.Sentence + sentenceAfter + 1);

        reader = cmd.ExecuteReader();

        if (reader.Read()) {
          Tuple<long, long> surroundingOffsets =
            new Tuple<long, long>(reader.GetInt32(0),
                                  reader.GetInt32(1));
          return surroundingOffsets;
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
    private static string GetWhereClauseFromProperty(
      WordLocationProperty property) {
      string fieldName = "";
      // startup with taking the string value, we override it if necessary
      long value = property.Value;

      switch (property.Field) {
        case WordLocationProperty.Property.Line:
          fieldName = kFieldLine;
          break;
        case WordLocationProperty.Property.SentenceIndex:
          fieldName = kFieldIndexInSentence;
          break;
        case WordLocationProperty.Property.Page:
          fieldName = kFieldPage;
          break;
        case WordLocationProperty.Property.Paragraph:
          fieldName = kFieldParagraph;
          break;
        case WordLocationProperty.Property.Sentence:
          fieldName = kFieldSentence;
          break;
      }

      return String.Format("{0} = {1}", fieldName, value);
    }

  }
}
