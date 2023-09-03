using Newtonsoft.Json.Bson;
using Patrikakis.Types;
using static Patrikakis.Helpers.DataHandler;

namespace Patrikakis
{
    public static class CollectData
    {
        public static List<Author> AllAuthors { get; set; }
        public static List<Author> From(string pathToData, bool testMode = false, bool forceExecution = false) {
   
            // This Methods checks if there are saved Author Data, and if there are it loads those.
            List<Author> authors = AuthorHandler.GetAuthorsFrom(pathToData);
            AllAuthors = authors;

            // In Test mode, we provide a subset of the availabe author data
            if (testMode) 
            {
                List<Author> testAuthors = new();
                testAuthors.AddRange(authors.GetRange(0, 6));
                // testAuthors.AddRange(authors.Take(3)); [using LINQ]

                return workToBeDone(testAuthors, forceExecution);
            }
            else 
            {
                return workToBeDone(authors, forceExecution);
            }

            // Local Function
            // Here, The Rake Algorithm is used and the keywords are extracted and stored
            // in multiple formats. .tsv, .txt and .json
        }
        private static List<Author> workToBeDone(List<Author> _authors, bool forceExecution) {
                foreach (Author author in _authors) {

                    if(!Directory.Exists(author.Path + "\\" + "GeneratedKeywords") || forceExecution)
                    {
                        
                        KeywordExtractor.AuthorName = author.Name;
                        KeywordExtractor.AuthorPath = author.Path;

                        List<Paper> papers = KeywordExtractor.ExtractMulti();
                        author.Papers = papers;

                        bool EndsWithSlash = author.Path.EndsWith('\\');
                        string fileName = $"{author.Name}.json";

                        string pathToExport = EndsWithSlash ? 
                        author.Path + fileName : 
                        author.Path + @"\" + fileName;

                        KeywordExtractor.CombineAllKeywordsIntoSingleFile(author);

                        ExportToJSON(pathToExport, author);
                    }
                }
                RAKE.LetTheShakeRest(); // Shuts Down Python Runtime

                Console.WriteLine();
                Console.WriteLine("The Number of Available Authors: {0}", _authors.Count);
                Console.WriteLine();
                
                foreach ( Author author in _authors ) 
                {
                    Console.WriteLine($" ===> {author.Name,-25} | {author.GetField()}");
                }
                Console.WriteLine();

                return _authors;
            }
    }
}

