using books.business_logic.data_access_layer;
using books.business_logic.models;
using document_parser;
using document_parser.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace books.business_logic.services {
  //----------------------------------------------------------------------------
  class GroupsService{
    private static GroupsService _instance = new GroupsService();
    private Dictionary<long, Group> _groupCache = new Dictionary<long, Group>();

    //--------------------------------------------------------------------------
    /// <summary>
    /// </summary>
    private GroupsService() {
    }

    //--------------------------------------------------------------------------
    static GroupsService() {
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// Get singelton instance
    /// </summary>
    public static GroupsService Instance {
      get {
        return _instance;
      }
    }

    //--------------------------------------------------------------------------
    public Int64 GetCount() {
      return GroupsDao.GetCount();
    }

    //--------------------------------------------------------------------------
    public Group AddGroup(string group) {
      Group resultGroup = new Group() {
        Name = group
      };

      GroupsDao.Insert(ref resultGroup);
      return resultGroup;
    }

    //--------------------------------------------------------------------------
    public bool RemoveGroup(Group group) {
      GroupsDao.DeleteAllGroupWords(group);
      return GroupsDao.Delete(group);
    }

    //--------------------------------------------------------------------------
    public List<Group> GetAll() {
      return GroupsDao.GetAll();
    }

    //--------------------------------------------------------------------------
    public List<Group> Query(string name) {
      return GroupsDao.Query(name);
    }

    //--------------------------------------------------------------------------
    public void CreateTable() {
      GroupsDao.CreateTable();
      _groupCache.Clear();
    }

    //--------------------------------------------------------------------------
    public Group GetById(long groupId) {
      if (_groupCache.ContainsKey(groupId)) {
        return _groupCache[groupId];
      }

      Group group = GroupsDao.GetGroupById(groupId);
      _groupCache[group.Id] = group;
      return group;
    }

    //--------------------------------------------------------------------------
    public void DropTable() {
      GroupsDao.DropTable();
      _groupCache.Clear();
    }

    //--------------------------------------------------------------------------
    public List<Word> GetWords(List<Group> groups) {
      // TODO: we can change to GetWords and have a join query
      List<long> wordIds = GroupsDao.GetWordIds(groups);

      List<Word> words = new List<Word>();
      foreach (long wordId in wordIds) {
        words.Add(WordsService.Instance.GetWordById(wordId));
      }

      return words;
    }

    //--------------------------------------------------------------------------
    public Word AddGroupWord(Group group, string word) {
      Word result = WordsService.Instance.GetWord(word);
      GroupsDao.AddGroupWord(group, result);
      return result;
    }

    //--------------------------------------------------------------------------
    public void RemoveGroupWord(Group group, Word word) {
      GroupsDao.DeleteGroupWord(group, word);
    }


    //--------------------------------------------------------------------------
    private Group Insert(string name) {
      List<Group> groups = GroupsDao.Query(name);
      if (groups.Count > 0) {
        throw new Exception("a group with this name already exists!");
      }

      // check we have the minimal set of meta data
      Group group = new Group() {
        Name = name
      };

      GroupsDao.Insert(ref group);

      _groupCache[group.Id] = group;
      return group;
    }

    //--------------------------------------------------------------------------
    public void Import(XmlDocument document) {
      GlobalParamatersService.Delegate.OnDatabaseImportProgress(0);

      DatabaseConnectionService.Instance.SafeTransaction(_ => {
        XmlNodeList xmlGroupList =
          document.DocumentElement.SelectNodes(".//group");
        XmlNodeList xmlGroupWordsList =
          document.DocumentElement.SelectNodes(".//groups_words");

        int total = xmlGroupList.Count + xmlGroupWordsList.Count;
        int processed = 0;

        foreach (XmlNode xmlGroup in xmlGroupList) {
          Group group = GroupsDao.ImportGroup(xmlGroup);
          _groupCache[group.Id] = group;

          processed++;
          float percent = (float)processed / (float)total;
          percent *= 100;
          GlobalParamatersService.Delegate.OnDatabaseImportProgress(
            (int)percent);
        }

        foreach (XmlNode xmlGroupWord in xmlGroupWordsList) {
          GroupsDao.ImportGroupWord(xmlGroupWord);

          processed++;
          float percent = (float)processed / (float)total;
          percent *= 100;
          GlobalParamatersService.Delegate.OnDatabaseImportProgress(
            (int)percent);
        }
      });
    }

  }
}
