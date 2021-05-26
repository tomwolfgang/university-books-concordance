# university-books-concordance
A University project in which we had to take txt book files from Gutenberg and 
then parse and index the words in each book.  I used CoreNLP (.NET wrapper) to 
help parsing documents.

# Disclaimers: 

- Use it at your own risk - I am not reliable for anything

- If you do use it for your own University project - please don't just change 
variables and submit - because you will be caught...  Use it only to help you
understand how one may approach such a task.

# Screenshots

![Loading documents](/screenshots/loading.jpg?raw=true "Loading documents")
![Words Inspector](/screenshots/words_inspector.jpg?raw=true "Words Inspector")
![Stats](/screenshots/stats.jpg?raw=true "Stats")
![Phrases](/screenshots/phrases_inspector.jpg?raw=true "Phrases")

# Open Issues
- books with a . (period) in their names will not be loaded (fix by 
preprocessing the documents - see |high_ascii_normalization.cs| for an example)
- some word parsing won't work as expected:
  - ain't = ai (fix by updating code in |qualified_words.cs|)
  - some words that start with ’ (’s) (fix by preprocessing document and 
changing/removing these ’)

# Getting Started
- You need Visual Studio 2015 (express is good enough) and the project uses 
NuGet for dependencies
- You also need Java JRE 8 (32bit) - otherwise you might get an error such as "failed to
  initialize CoreNLP"
- Install MySql server (community edition is good enough)
- Create a new (empty) schema called: books (can be any name)
- Compile the project
- Edit the application.exe.config and set:
    'storage_folder' and  'connection_string' to valid values
- Run application.exe (/output/release/application/application.exe)
- Press the ResetDB and you can start

You can download txt book files from: https://www.gutenberg.org/
Also included in the project (under /database) are sample documents and the 
full DB schema

# Features (assignment tasks)
- load txt documents
- support meta-data about the txt documents (title, author...)
- query documents by meta-data and/or words
- show all words in the database or in specific documents
- show a word's context in documents (a few lines/sentences before/after)
- support indexing words by: document, line, sentence, paragraph, page ...
- support querying words by their index (line, sentence, paragraph ...)
- support grouping of words (i.e. by their meaning) (e.g. countries, animals)
- support querying by groups of words (the group is our index)
- support for word relations (e.g. words the ryhme, synonyms ...)
- support adding phrases and querying the database by these phrases
- show some statistics: avg chars/words per line, sentence, document etc... and
word frequencies in the database
- support exporting/importing the entire database using XML
