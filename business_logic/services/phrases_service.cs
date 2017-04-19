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
  class PhrasesService {
    // a phrase should have, at least, 3 words - to make it interesting
    private readonly int kMinimalWordsInPhrase = 3;

    private static PhrasesService _instance = new PhrasesService();

    //--------------------------------------------------------------------------
    /// <summary>
    /// </summary>
    private PhrasesService() {
    }

    //--------------------------------------------------------------------------
    static PhrasesService() {
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// Get singelton instance
    /// </summary>
    public static PhrasesService Instance {
      get {
        return _instance;
      }
    }

    //--------------------------------------------------------------------------
    public Int64 GetCount() {
      return PhrasesDao.GetCount();
    }

    //--------------------------------------------------------------------------
    public Phrase AddPhrase(string phrase) {
      DocumentParser parsedPhrase = DocumentParser.FromPhrase(phrase);

      DocumentWord documentWord = null;
      List<Word> words = new List<Word>();

      while (parsedPhrase.GetNextWord(out documentWord)) {
        Word word = WordsService.Instance.GetWord(documentWord.Text);
        words.Add(word);
      }

      if (words.Count < kMinimalWordsInPhrase) {
        throw new Exception("Phrases must have, at least, " + 
                            kMinimalWordsInPhrase + " words!");
      }

      Phrase resultPhrase = new Phrase() {
        Words = words
      };

      PhrasesDao.Insert(ref resultPhrase);
      return resultPhrase;
    }

    //--------------------------------------------------------------------------
    public void CreateTable() {
      PhrasesDao.CreateTable();
    }

    //--------------------------------------------------------------------------
    public void RemovePhrase(Phrase phrase) {
      PhrasesDao.Delete(phrase);
    }

    //--------------------------------------------------------------------------
    public void DropTable() {
      PhrasesDao.DropTable();
    }

    //--------------------------------------------------------------------------
    public List<Phrase> GetAll() {
      // we first get InternalPhrases and then map them to Phrases
      List<InternalPhrase> internalPhrases = PhrasesDao.GetAll();

      List<Phrase> phrases = new List<Phrase>();
      foreach (InternalPhrase internalPhrase in internalPhrases) {
        Phrase phrase = new Phrase() {
          Id = internalPhrase.Id,
          Words = new List<Word>()
        };

        foreach (long wordId in internalPhrase.WordIds) {
          Word word = WordsService.Instance.GetWordById(wordId);
          phrase.Words.Add(word);
        }

        phrases.Add(phrase);
      }

      return phrases;
    }

    //--------------------------------------------------------------------------
    // we return a list of location pairs - each pair contains the first and
    // last word in the phrase with their location details
    public List<Tuple<LocationDetail, LocationDetail>> Query(Phrase phrase) {
      // first, get a list of Contains which build potential phrases
      List<Contains> potentialPhrases = ContainsDao.Query(phrase);

      // break the list into sublists (by unique doc,sentence)
      Dictionary<Tuple<long, long>, List<Contains>> phraseContainsMap = 
        new Dictionary<Tuple<long, long>, List<Contains>>();

      foreach (Contains item in potentialPhrases) {
        Tuple<long, long> currentKey = 
          new Tuple<long, long>(item.DocumentId, item.Sentence);

        if (!phraseContainsMap.ContainsKey(currentKey)) {
          phraseContainsMap[currentKey] = new List<Contains>();
        }

        phraseContainsMap[currentKey].Add(item);
      }

      List<Tuple<LocationDetail, LocationDetail>> result = 
        new List<Tuple<LocationDetail, LocationDetail>>();

      // now check which phrase contains all our words
      foreach (var containsPhrase in phraseContainsMap.Values) {
        int startIndex = 0;
        int foundIndex = 0;
        // we have a while to support sentences that have our phrase more than
        // once in them
        while (ContainsPhraseEqualsPhrase(containsPhrase, 
                                          startIndex, 
                                          phrase, 
                                          out foundIndex)) {
          Tuple<LocationDetail, LocationDetail> pair = 
            new Tuple<LocationDetail, LocationDetail>(
              new LocationDetail() { 
                Document = DocumentsService.Instance.GetById(
                  containsPhrase[0].DocumentId),
                Location = containsPhrase[foundIndex]
              }, 
              new LocationDetail() {
                Document = DocumentsService.Instance.GetById(
                  containsPhrase[0].DocumentId),
                Location = containsPhrase[foundIndex + phrase.Words.Count-1]
              });
          result.Add(pair);

          startIndex = foundIndex + phrase.Words.Count;
        }
      }

      return result;
    }

    //--------------------------------------------------------------------------
    // find the first wordId of phrase inside containsPhrase - then see it is
    // followed by the right words - if yes, return true with index, otherwise
    // find the next "first wordId" and do the same - until we find it or 
    // reach the end
    private bool ContainsPhraseEqualsPhrase(List<Contains> containsPhrase, 
                                            int startIndex,
                                            Phrase phrase,
                                            out int foundIndex) {
      if (containsPhrase.Count < phrase.Words.Count) {
        foundIndex = -1;
        return false;
      }

      foundIndex = startIndex;
      for (; foundIndex < containsPhrase.Count; ++foundIndex) {
        if (containsPhrase[foundIndex].WordId == phrase.Words[0].Id) {
          bool match = true;
          Contains currentWord = containsPhrase[foundIndex];
          // now we iterate all the other words following the first one to see
          // if we have a match
          for (int j = 1; j < phrase.Words.Count; ++j) {
            // did we reach the end?
            if (foundIndex + j >= containsPhrase.Count) {
              return false;
            }
        
            Contains nextWord = containsPhrase[foundIndex + j];
            if ((nextWord.WordId != phrase.Words[j].Id) ||
               (nextWord.IndexInSentence != (currentWord.IndexInSentence + 1))){
              // we don't have a matching word or the words aren't in sequence
              match = false;
              break;
            }
            currentWord = nextWord;
          }

          if (match) {
            return true;
          }
        }
      }

      return false;
    }

    //--------------------------------------------------------------------------
    public void Import(XmlDocument document) {
      GlobalParamatersService.Delegate.OnDatabaseImportProgress(0);

      DatabaseConnectionService.Instance.SafeTransaction(_ => {
        XmlNodeList xmlPhraseList =
          document.DocumentElement.SelectNodes(".//phrase");
        XmlNodeList xmlPraseWordsList =
          document.DocumentElement.SelectNodes(".//phrases_words");

        int total = xmlPhraseList.Count + xmlPraseWordsList.Count;
        int processed = 0;

        foreach (XmlNode xmlPhrase in xmlPhraseList) {
          Phrase phrase = PhrasesDao.ImportPhrase(xmlPhrase);

          processed++;
          float percent = (float)processed / (float)total;
          percent *= 100;
          GlobalParamatersService.Delegate.OnDatabaseImportProgress(
            (int)percent);
        }

        foreach (XmlNode xmlPhraseWord in xmlPraseWordsList) {
          PhrasesDao.ImportPhraseWord(xmlPhraseWord);

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
