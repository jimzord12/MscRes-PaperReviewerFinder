using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using System.Text.RegularExpressions;

namespace Patrikakis;

public class RAKE
{
    static RAKE()
    {
        // Initialize Python runtime once for the entire lifetime of this class
        try
        {
            Runtime.PythonDLL = @"C:\Python310\python310.dll";
            PythonEngine.Initialize();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Initialization failed: {ex.Message}");
        }
    }
    public static List<string> RakeRunner(int min, int max, string text) {

    using (Py.GIL())
    {
        // Import NLTK and download the punkt tokenizer
        dynamic nltk = Py.Import("nltk");
        nltk.download("punkt");
        nltk.download("averaged_perceptron_tagger");

        // Import rake_nltk
        dynamic rakeNltk = Py.Import("rake_nltk");
        dynamic Rake = rakeNltk.Rake;

        // Initialize a Rake object
        dynamic r = Rake(min_length: min, max_length: max);

        // Filter Text
        string filteredText = FilterText(text);

        // Extract keywords
        r.extract_keywords_from_text(filteredText);
        dynamic rankedKeywords = r.get_ranked_phrases_with_scores();
        dynamic keywords = r.get_ranked_phrases();

        // Convert dynamic 'keywords' to List<string> manually
        List<string> keywordList = new();
        foreach (PyObject pyObj in keywords)
        {
            keywordList.Add(pyObj.ToString() ?? "XX_ERROR_XX");
        }

        // Remove duplicates manually
        HashSet<string> uniqueKeywords = new HashSet<string>();
        foreach (PyObject keyword in keywords)
        {
            uniqueKeywords.Add(keyword.As<string>());
        }

        HashSet<string> stopWords = new HashSet<string> { "often", "within", "may", "also", "september" };
        List<string> filteredKeywords = uniqueKeywords.Where(w => !stopWords.Contains(w.ToLower())).ToList();

        // Output the filtered keywords
        Console.WriteLine();
        Console.WriteLine("****************************");
        Console.WriteLine("****  Filtered Keywords ****");
        Console.WriteLine("****************************");
        Console.WriteLine();

        for (int i = 0; i < filteredKeywords.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {filteredKeywords[i]}");
        }

        return filteredKeywords;
    }

    // Shutdown the Python Runtime
    try
    {
        PythonEngine.Shutdown();
    }
    catch (Exception)
    {
        Console.WriteLine("Python Runtime was Shutting down");
    }
}
    static string FilterText(string originalText)
        {
            // Remove non-ASCII characters
            string filteredText = Regex.Replace(originalText, @"[^\x20-\x7E]", string.Empty);

            // Remove text inside parentheses
            filteredText = Regex.Replace(filteredText, @"\([^)]*\)", "");

            // Remove numbers and symbols
            filteredText = Regex.Replace(filteredText, @"[^a-zA-Z\s]", "");

            // Now we remove verbs; for this, we will use NLTK in Python
            dynamic nltk = Py.Import("nltk");
            dynamic words = nltk.word_tokenize(filteredText);
            dynamic posTagged = nltk.pos_tag(words);

            List<string> filteredWords = new List<string>();
            foreach (PyObject taggedWord in posTagged)
            {
                PyObject wordObj = taggedWord[0];
                PyObject tagObj = taggedWord[1];
                
                string word = wordObj.ToString();
                string tag = tagObj.ToString();

                // Filtering out Verbs (VB, VBD, VBG, VBN, VBP, VBZ are POS tags for verbs in the Penn Treebank POS Tags)
                if (!tag.StartsWith("VB"))
                {
                    filteredWords.Add(word);
                }
            }
            return string.Join(" ", filteredWords);
        }
}
