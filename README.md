# university-books-concordance
A University project in which we had to take txt books files from Gutenberg and then parse and index the words in each book.  I used CoreNLP (.NET wrapper) to help parsing documents.

Disclaimers: 

At first I had time to create clean code - but as the project 
deadline approached, as usual, I had to take some (many) shortcuts.

Use it at your own risk - I am not reliable for anything ;)

If you do use it for your own University project - please don't just change 
variables and submit - because you will be caught...  Use it only to help you
understand how one may approach such a task.

# Open Issues
- books with a . (period) in their names will not be loaded (fix by preprocessing)
- some word parsing won't work as expected:
-- ain't = ai
-- some words that start with ’ (’s)

# Getting Started
- You need Visual Studio 2015 (express is good enough) and the project uses 
NuGet for dependencies
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
