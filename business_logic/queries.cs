using books.business_logic.common;
using books.business_logic.models;
using books.business_logic.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.business_logic {
  //----------------------------------------------------------------------------
  public class Queries {
    // should we extract sorrounding lines or sentences
    private bool _useLinesForContentRetreival = false;

    // how many lines/sentences sorrounding our word/phrase do we want to 
    // retreive
    private uint _contentRetreivalDelta = 1;

    //--------------------------------------------------------------------------
    // callbacks for query functions - we don't want to use the global delegate
    // interface as it is less convenient with queries
    public delegate void GetDocumentsCallback(List<Document> documents, 
                                              bool status, 
                                              string message);

    public delegate void GetWordsCallback(List<Word> words, 
                                          bool status, 
                                          string message);

    public delegate void GetWordLocationDetailsCallback(
      WordLocationDetails locations, 
      bool status, 
      string message);

    public delegate void GetWordsLocationDetailsCallback(
      List<WordLocationDetails> wordsLocations,
      bool status,
      string message);

    public delegate void GetContentsCallback(string content, 
                                             long wordOffsetBegin, 
                                             long wordOffsetEnd, 
                                             bool status, 
                                             string message);

    //--------------------------------------------------------------------------
    private ThreadWorkerQueue _workerThread = null;

    //--------------------------------------------------------------------------
    public Queries(ThreadWorkerQueue workerThread) {
      this._workerThread = workerThread;

      // if lines is true, we return surrounding lines, if false, we return
      // sorrounding sentences
      _useLinesForContentRetreival = 
        GlobalParamatersService.Configuration
                               .ContentRetreivalResolution
                               .UseLines;

      // delta is a whole number of how many lines/sentences to retreive 
      // before and after the line/sentence in which the given word is in
      _contentRetreivalDelta = 
        GlobalParamatersService.Configuration.ContentRetreivalResolution.Delta;
    }

    //--------------------------------------------------------------------------
    public bool GetDocuments(
      List<string> words, GetDocumentsCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          List<Document> result = 
            DocumentsService.Instance.QueryByWords(words);
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool GetDocuments(
      List<string> words,
      List<DocumentProperty> properties, 
      GetDocumentsCallback callback) {
      if (callback == null) {
        return false;
      }
 
      return _workerThread.PostTask(_ => {
        try {
          List<Document> result =
            DocumentsService.Instance.Query(words, properties);
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool GetWords(
      List<Document> documents, 
      List<WordLocationProperty> properties,
      GetWordsCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          List<Word> result =
            WordsService.Instance.Query(documents, properties);
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool GetWords(
      List<Group> groups,
      GetWordsCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          List<Word> result =
            GroupsService.Instance.GetWords(groups);
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    // given a word and document(s), return the exact position of the word
    // in the document(s)
    public bool GetWordLocationDetails(Word word,
                                       List<Document> documents,
                                       GetWordLocationDetailsCallback callback) {
      return _workerThread.PostTask(_ => {
        try {
          WordLocationDetails result =
            ContainsService.Instance.Query(documents, word);
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    // given document properties and word location properties - we retreive a
    // list of words locations that match the query
    public bool GetWordsLocationDetails(
      List<DocumentProperty> documentProperties,
      List<WordLocationProperty> wordLocationProperties,
      GetWordsLocationDetailsCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          // first query documents
          List<Document> documents = new List<Document>();
          // an empty documentProperties list means ALL documents, so we can
          // skip querying the documents and pass an empty list of documents to
          // the next query (ContainsService)
          if (documentProperties.Count > 0) {
            documents = DocumentsService.Instance.Query(new List<string>(), 
                                                        documentProperties);
            if (documents.Count == 0) {
              // if we didn't get any document, then we can stop
              // otherwise, we'll query the entire DB...
              callback(new List<WordLocationDetails>(), true, null);
              return;
            }
          }

          // now query the locations
          List<WordLocationDetails> result = 
            ContainsService.Instance.Query(documents, wordLocationProperties);
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    // given a group we retreive a list of words locations that match the query
    // words in the group
    public bool GetWordsLocationDetails(
      Group group,
      GetWordsLocationDetailsCallback callback) {
      if (callback == null) {
        return false;
      }

      return _workerThread.PostTask(_ => {
        try {
          // now query the locations
          List<WordLocationDetails> result =
            ContainsService.Instance.Query(group);
          callback(result, true, null);
        } catch (Exception e) {
          callback(null, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool GetContents(LocationDetail locationDetail, 
                            GetContentsCallback callback) {
      return _workerThread.PostTask(_ => {
        try {         
          Tuple<long, long> surroundingOffsets = 
            ContainsService.Instance.GetSurroundingOffsets(
              locationDetail.Location, 
              _useLinesForContentRetreival, 
              _contentRetreivalDelta);

          long contentsWordOffsetBegin = 0;
          long contentsWordOffsetEnd = 0;
          string contents = DocumentsService.Instance.GetContents(
            locationDetail,
            surroundingOffsets.Item1,
            surroundingOffsets.Item2,
            out contentsWordOffsetBegin,
            out contentsWordOffsetEnd);

          callback(contents, 
                   contentsWordOffsetBegin, 
                   contentsWordOffsetEnd, 
                   true, 
                   null);
        } catch (Exception e) {
          callback(null, 0, 0, false, e.Message);
        }
      });
    }

    //--------------------------------------------------------------------------
    public bool GetContents(Tuple<LocationDetail, LocationDetail> phrase,
                            GetContentsCallback callback) {
      return _workerThread.PostTask(_ => {
        try {
          // we extract sorrounding lines/sentences based only on the first
          // word of the phrase (simplifies our code)
          Tuple<long, long> surroundingOffsets =
            ContainsService.Instance.GetSurroundingOffsets(
              phrase.Item1.Location,
              _useLinesForContentRetreival,
              _contentRetreivalDelta);

          long contentsWordOffsetBegin = 0;
          long contentsWordOffsetEnd = 0;
          string contents = DocumentsService.Instance.GetContents(
            phrase,
            surroundingOffsets.Item1,
            surroundingOffsets.Item2,
            out contentsWordOffsetBegin,
            out contentsWordOffsetEnd);

          callback(contents,
                   contentsWordOffsetBegin,
                   contentsWordOffsetEnd,
                   true,
                   null);
        } catch (Exception e) {
          callback(null, 0, 0, false, e.Message);
        }
      });
    }
  }
}
