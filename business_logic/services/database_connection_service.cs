using books.business_logic.data_access_layer;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace books.business_logic.services {
  //-------------------------------------------------------------------------
  /// <summary>
  /// Manages the connections with the database - in our case only a single
  /// connection is required.
  /// 
  /// Also provides a method to initialize the connection
  /// </summary>
  public class DatabaseConnectionService : IDisposable {
    // member variables
    private static DatabaseConnectionService _instance =
      new DatabaseConnectionService();

    private MySqlConnection _connection = null;

    //-------------------------------------------------------------------------
    /// <summary>
    /// singleton - so we block the constructor
    /// </summary>
    private DatabaseConnectionService() {
    }

    //-------------------------------------------------------------------------
    static DatabaseConnectionService() {
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// IDisposable
    /// </summary>
    public void Dispose() {
      if (_connection != null) {
        _connection.Close();
        _connection = null;
      }
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// Get singelton instance
    /// </summary>
    public static DatabaseConnectionService Instance {
      get {
        return _instance;
      }
    }

    //--------------------------------------------------------------------------
    public void CreateTables() {
      WordsService.Instance.CreateTable();
      DocumentsService.Instance.CreateTable();
      ContainsService.Instance.CreateTable();
      PhrasesService.Instance.CreateTable();
      RelationsService.Instance.CreateTable();
      GroupsService.Instance.CreateTable();

      //StatisticsDao.CreateTable();
    }

    //--------------------------------------------------------------------------
    public void DropTables() {
      //StatisticsDao.DropTable();
      GroupsService.Instance.DropTable();
      RelationsService.Instance.DropTable();
      PhrasesService.Instance.DropTable();
      ContainsService.Instance.DropTable();
      DocumentsService.Instance.DropTable();
      WordsService.Instance.DropTable();
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    public bool Initialized {
      get {
        return (_connection != null);
      }
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool Initialize() {
      if (this.Initialized) {
        return true;
      }

      try {
        _connection = new MySqlConnection(
          GlobalParamatersService.Configuration.ConnectionString);
        _connection.Open();
        if (_connection.State != ConnectionState.Open) {
          _connection = null;
          throw new Exception("Failed to open DB connection");
        }
      } catch (Exception e) {
        _connection = null;
        throw e;
      }

      return true;
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    internal MySqlConnection Connection {
      get {
        return _connection;
      }
    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// Wraps a function with a transaction - use to improve performance of
    /// bulk tasks (like bulk inserts).
    /// If the function wrapped throws an exception, we rollback the 
    /// transaction
    /// </summary>
    internal void SafeTransaction(AsyncCallback action) {
      var dbTransaction = this.Connection.BeginTransaction();
      try {
        action.Invoke(null);
        dbTransaction.Commit();
      } catch (Exception ex) {
        dbTransaction.Rollback();
        throw ex;
      }
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// The function uses all information in SQL tables in the program mySQL and
    /// builds a new XML file containing all that information.
    /// The XML file describes all the books'information
    /// </summary>
    public void ExportDatabase(FileInfo filename) {
      Initialize();

      DataSet ds = new DataSet();

      ds.DataSetName = "books-concordance";
      DocumentsDao.FillDataSet(ds);
      WordsDao.FillDataSet(ds);
      ContainsDao.FillDataSet(ds);
      RelationsDao.FillDataSet(ds);
      GroupsDao.FillDataSet(ds);
      PhrasesDao.FillDataSet(ds);

      ds.WriteXml(filename.FullName);

      DocumentsService.Instance.ExportStorage(filename);
    }

    //--------------------------------------------------------------------------
    public void ImportDatabase(FileInfo filename, IStatsUpdates statsUpdate) {
      GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
        "Loading xml file ...");
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(filename.FullName);

      GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
        "Importing Words [1/6] ...");
      WordsService.Instance.Import(xmlDocument);
      statsUpdate.CollectStats();

      GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
        "Importing Documents [2/6] ...");
      DocumentsService.Instance.Import(xmlDocument, filename);
      statsUpdate.CollectStats();

      GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
        "Importing Contains [3/6] ...");
      ContainsService.Instance.Import(xmlDocument);
      statsUpdate.CollectStats();

      GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
        "Importing Groups [4/6] ...");
      GroupsService.Instance.Import(xmlDocument);
      statsUpdate.CollectStats();

      GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
        "Importing Relations [5/6] ...");
      RelationsService.Instance.Import(xmlDocument);
      statsUpdate.CollectStats();

      GlobalParamatersService.Delegate.OnDatabaseImportStatusChanged(
        "Importing Relations [6/6] ...");
      PhrasesService.Instance.Import(xmlDocument);
      statsUpdate.CollectStats();
    }

  }
}
