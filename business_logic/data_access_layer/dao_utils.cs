using books.business_logic.services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic.data_access_layer {
  class DaoUtils {
    private static readonly string kSQLDropTable = "DROP TABLE {0}";

    //--------------------------------------------------------------------------
    public static DateTime? SafeGetDateTime(
      MySqlDataReader reader, string fieldName) {
      int ordinal = reader.GetOrdinal(fieldName);

      if (!reader.IsDBNull(ordinal)) {
        return reader.GetDateTime(ordinal);
      }

      return null;
    }

    //--------------------------------------------------------------------------
    // used to export to xml
    public static void FillDataSet(string query, string table, DataSet ds) {
      MySqlDataAdapter adapter = new MySqlDataAdapter(
        query, DatabaseConnectionService.Instance.Connection);
      DataTable dt = new DataTable(table);
      adapter.Fill(dt);
      ds.Tables.Add(dt);
    }

    //--------------------------------------------------------------------------
    public static void DropTable(string table) {
      ExecuteNonQuery(String.Format(kSQLDropTable, table));
    }

    //--------------------------------------------------------------------------
    public static void ExecuteNonQuery(string sql) {
      try {
        MySqlCommand cmd = new MySqlCommand(
          sql,
          DatabaseConnectionService.Instance.Connection);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        throw ex;
      }
    }

  }
}
