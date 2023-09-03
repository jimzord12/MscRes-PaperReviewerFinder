using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using System.Text.RegularExpressions;
using Patrikakis.Types;
using static Patrikakis.PDFHandler;
using static Patrikakis.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Patrikakis
{
    public static class KeywordExtractor
    {
    public static string? AuthorName { get; set; }
    public static string? AuthorPath { get; set; }
    public static int? TotalKeywordsCount { get; set; }
        public static Keywords ExtractSingle(string document)
        {
            Keywords keywordsSet = new()
            {
                // The keywords from extraction
                // RAKE.RakeRunner returns List<string>
                SingleWord = new HashSet<string>(RAKE.RakeRunner(1, 1, document)),
                DoubleWord = new HashSet<string>(RAKE.RakeRunner(2, 2, document)),
                TripleWord = new HashSet<string>(RAKE.RakeRunner(3, 3, document)),
                QuadWord = new HashSet<string>(RAKE.RakeRunner(4, 4, document)),

            };

            TotalKeywordsCount = keywordsSet.SingleWord.Count + keywordsSet.DoubleWord.Count + keywordsSet.TripleWord.Count + keywordsSet.QuadWord.Count;
            // DisplayKeywords(keywordsSet);

            return keywordsSet;
        } 
        public static List<Paper> ExtractMulti()
        {
            // 1. Converts multiple PDF files to text files (single strings)
            List<string> papers = GetPapersFromDir(AuthorPath);

            int documentNum = 1;
            List<Paper> formatedPapers = new List<Paper>();
            // The keywords from extraction
            foreach (string paper in papers) 
            {
                Console.WriteLine();
                Console.WriteLine("+========================================+"); 
                Console.WriteLine($"| *** Document No.: ({documentNum}) *** |");
                Console.WriteLine("+========================================+"); 
                Console.WriteLine();
                Keywords k = ExtractSingle(paper);

                string fileName = $"doc_{documentNum}";
                string tsvPath = $@"{AuthorPath}\GeneratedKeywords\{fileName}.tsv";
                string rawPath = $@"{AuthorPath}\RawKeywords\{fileName}.txt";
                string jsonPath = $@"{AuthorPath}\KeywordsInJSON\{fileName}.json";
                // Creates a .tsv file, where keyword phrases are seperated by "\t"
                WriteKeywordsToTSV(k, tsvPath);
                
                // Creates a .txt file, where keyword phrases are seperated by " "
                WriteRawKeywordsToTxt(k, fileName);

                // Creates a .json file, where ....
                WriteKeywordsInJSON(k, fileName);

                formatedPapers.Add(new Paper(Name: fileName, KeywordsTSV: tsvPath, KeywordsRaw: rawPath, KeywordsJSON: jsonPath, Hash: Helpers.HashThis(AuthorName + ":" + fileName)));

                documentNum++;
            }
            
            Console.WriteLine();
            Console.WriteLine("+========================================+"); 
            Console.WriteLine($"| *** Total Keywords Extracted: {TotalKeywordsCount} *** |");
            Console.WriteLine("+========================================+"); 
            Console.WriteLine();

            return formatedPapers;
        }
        
        public static void WriteKeywordsToTSV(Keywords keywords, string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                // Write headers
                sw.WriteLine("SingleWord\tDoubleWord\tTripleWord\tQuadWord");

                // Get enumerators for each set
                var singleEnum = keywords.SingleWord.GetEnumerator();
                var doubleEnum = keywords.DoubleWord.GetEnumerator();
                var tripleEnum = keywords.TripleWord.GetEnumerator();
                var quadEnum = keywords.QuadWord.GetEnumerator();

                // Loop and write
                while (singleEnum.MoveNext() || doubleEnum.MoveNext() || tripleEnum.MoveNext() || quadEnum.MoveNext())
                {
                    var singleWord = singleEnum.MoveNext() ? singleEnum.Current : "";
                    var doubleWord = doubleEnum.MoveNext() ? doubleEnum.Current : "";
                    var tripleWord = tripleEnum.MoveNext() ? tripleEnum.Current : "";
                    var quadWord = quadEnum.MoveNext() ? quadEnum.Current : "";

                    sw.WriteLine($"{singleWord}\t{doubleWord}\t{tripleWord}\t{quadWord}");
                }
            }
        }

        public static void WriteRawKeywordsToTxt(Keywords keywords, string fileName)
        {
            // Define the directory and file path
            string directoryName = $@"{AuthorPath}\RawKeywords";
            string filePath = $@"{directoryName}\{fileName}.txt";
            
            // Create the directory if it does not exist
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            // Get the keywords as a single string
            string keywordString = keywords.GetStringVersion();

            // Remove leading ", " segments
            // while (keywordString.StartsWith(", "))
            // {
            //     keywordString = keywordString[2..];
            // }

            // Write the keywords string to the .txt file
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(keywordString);
            }
        }

        public static void WriteKeywordsInJSON(Keywords keywords, string fileName)
        {
            // Define the directory and file path
            string directoryName = $@"{AuthorPath}\KeywordsInJSON";
            string filePath = $@"{directoryName}\{fileName}";
            
            // Create the directory if it does not exist
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            
            var tf_idf_format = keywords.GetListVersion();
            DataHandler.ExportToJSON(filePath, tf_idf_format);
        }

        public static void CombineAllKeywordsIntoSingleFile(Author author) 
        {
            // Define the directory and file path
            string directoryName = $@"{AuthorPath}\RawKeywords";
            string filePath = $@"{directoryName}\AllKeywords.txt";
            
            // Create the directory if it does not exist
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            var combinatedKeywords = new StringBuilder();

            foreach (Paper paper in author.Papers) {
                string keywordString = ReadTxtFile(paper.KeywordsRaw);
                combinatedKeywords.Append(" " + keywordString);
                // string keywordString = keyword.GetStringVersion();
            }

            // Write the keywords string to the .txt file
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(combinatedKeywords.ToString());
            }
        }

        public static string ReadTxtFile(string filePath)
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("!!! File not found: " + filePath);
                return "File not found.";
            }

            string content;

            // Read the file contents
            using (StreamReader reader = new StreamReader(filePath))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }









        /// ///////////////////////////////////////////////////////////////////////
        static void DisplayKeywords(Keywords keywords) {
            DisplaySingleWord(keywords);
            DisplayDoubleWord(keywords);
            DisplayTripleWord(keywords);
            DisplayQuadWord(keywords);
        }
        static void DisplaySingleWord(Keywords keywords) {

            int counter = 1;
            DisplayFrame("Single");
            foreach(string keyword in keywords.SingleWord) {
                Console.WriteLine($"{counter}. {keyword}");
                counter++;
            }
        }
        static void DisplayDoubleWord(Keywords keywords) {

            int counter = 1;
            DisplayFrame("Double");
            foreach(string keyword in keywords.DoubleWord) {
                Console.WriteLine($"{counter}. {keyword}");
                counter++;
            }
        }
        static void DisplayTripleWord(Keywords keywords) {

            int counter = 1;
            DisplayFrame("Triple");
            foreach(string keyword in keywords.TripleWord) {
                Console.WriteLine($"{counter}. {keyword}");
                counter++;
            }
        }
        static void DisplayQuadWord(Keywords keywords) {

            int counter = 1;
            DisplayFrame("Quad");
            foreach(string keyword in keywords.QuadWord) {
                Console.WriteLine($"{counter}. {keyword}");
                counter++;
            }
        }
        static void DisplayFrame(string text) {
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine($"===  {text} - Word | Keyword Phrase  ===");
            Console.WriteLine("========================================"); 
            Console.WriteLine();
        }
    }
}

