using document_parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using document_parser.models;

namespace business_logic_tester {
  class Program {
    // expects 1 mandatory command line argument and 1 optional
    // mandatory - txt file
    // optional - storage location (folder)
    static void Main(string[] args) {
      // 1 - [, {, (, ...
      // 2 - 's, n't, 'll ...
      // 3 - class normalize high-ascii chars

      //Open the stream and read it back.

      // test parsing a phrase
      DocumentParser phrase = DocumentParser.FromPhrase(
        "tolerance and who wrangles with everybody who does not" + "\n" +
        "do as he would like them to");

      DocumentWord aWord = null;
      while (phrase.GetNextWord(out aWord)) {
        
      }

      // test parsing a file
      if (args.Length < 1) {
        Console.WriteLine("You need to pass a Gutenberg text file as a parameter");
        return;
      }

      FileInfo documentFile = new FileInfo(args[0]);
      string workDirectory = documentFile.DirectoryName;
      if (args.Length > 1) {
        workDirectory = args[1];
      }

      DocumentParser document = null;

      try {
        document = DocumentParser.FromFile(documentFile);
      } catch (Exception ex) {
        Console.WriteLine("Failed to parse document: ", ex);
        return;
      }

      if (document == null) {
        Console.WriteLine("Failed to parse document");
        return;
      }

      Console.WriteLine("");
      Console.WriteLine("Document parsed successfully");
      Console.WriteLine("");
      Console.WriteLine("The meta-data:");
      //Console.WriteLine("Filename: " + document.Filename);

      foreach (var metaData in document.MetaData) {
        Console.WriteLine(
          String.Format("{0}:\t{1}", metaData.Key, metaData.Value));
      }

      Dictionary<string, int> concordance = new Dictionary<string, int>();

      int wordCounter = 0;

      document.Save(new FileInfo(@"D:\Release\txt files\temp.txt"), true);
      FileStream fs = new FileStream(@"D:\Release\txt files\temp.txt", FileMode.Open);
      //string fileData = File.ReadAllText(@"D:\Release\txt files\temp.txt");

      DocumentWord word = null;
      while (document.GetNextWord(out word)) {
        wordCounter++;

        //if (fileData[(int)word.OffsetInFile] != word.Text[0]) {
        //fs.Seek(word.OffsetInFile+utf8Offset, SeekOrigin.Begin);
        fs.Seek(word.OffsetInFile, SeekOrigin.Begin);

        int count = System.Text.UTF8Encoding.UTF8.GetByteCount(word.Text);
        char firstChar = (char)System.Text.UTF8Encoding.UTF8.GetBytes(word.Text)[0];

        char b = (char)fs.ReadByte();
        if (b != firstChar) { 
          Console.WriteLine("");
          Console.WriteLine("Problematic word found - differnt from file:");
          Console.WriteLine("text: " + word.Text);
          Console.WriteLine("offset: " + word.OffsetInFile.ToString());
          Console.WriteLine("page (100 lines per page): " + (word.Page + 1).ToString());
          Console.WriteLine("paragraph: " + (word.Paragraph + 1).ToString());
          Console.WriteLine("sentence: " + (word.Sentence + 1).ToString());
          Console.WriteLine("word in sentence: " + (word.IndexInSentence + 1).ToString());
          Console.WriteLine("line: " + (word.Line + 1).ToString());

          Console.WriteLine("\r\nPress any key to continue...");
          Console.ReadKey();
        }

        string lowerCaseWord = word.Text.ToLower();

        if (!concordance.ContainsKey(lowerCaseWord)) {
          concordance.Add(lowerCaseWord, 1);
        } else {
          concordance[lowerCaseWord]++;
        }
      }

      Console.WriteLine("\r\nPress any key to continue...");
      Console.ReadKey();

      // sort it
      var list = concordance.ToList();
      list.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

      Console.WriteLine("");
      Console.WriteLine("Top 50 words:");
      for (int i = 0; i < list.Count; i++) {
        if (list[i].Value > 100) {
          Console.WriteLine(
            String.Format("{0}. {1} - {2}", i + 1, list[i].Key, list[i].Value));
        }
      }

      Console.WriteLine("");
      Console.WriteLine("Unknown suffixes found in the document:");
      foreach (var suffix in document.UnknownSuffixes) {
        Console.WriteLine(suffix);
      }


      Console.WriteLine("\r\nPress any key to continue...");
      Console.ReadKey();
    }
  }
}
