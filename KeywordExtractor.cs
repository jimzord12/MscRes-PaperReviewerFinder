using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using System.Text.RegularExpressions;
using Patrikakis.Types;

namespace Patrikakis
{
    public static class KeywordExtractor
    {
        public static Keywords ExtractSingle(string text)
        {
            Keywords keywordsSet = new()
            {
                // The keywords from extraction
                // RAKE.RakeRunner returns List<string>
                SingleWord = new HashSet<string>(RAKE.RakeRunner(1, 1, text)),
                DoubleWord = new HashSet<string>(RAKE.RakeRunner(2, 2, text)),
                TripleWord = new HashSet<string>(RAKE.RakeRunner(3, 3, text)),
                QuadWord = new HashSet<string>(RAKE.RakeRunner(4, 4, text)),
            };

            DisplayKeywords(keywordsSet);

            return keywordsSet;
        } 
        public static void ExtractMulti(List<string> papers)
        {
            // The keywords from extraction
            foreach (string paper in papers)
            ExtractSingle(paper);
        }
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

