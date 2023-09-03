using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using System.Text.RegularExpressions;

using Patrikakis;

namespace Patrikakis;

//TODO: Ask User to provide PATH for "python311.dll"
public class RAKE
{
    // public static string DLL_PATH {get; set;}
    static bool PyModulesOnlyOnce {get; set;} = false;
    static RAKE()
    {
        // string inputPath = Helpers.GetSafeUserInput(true);
        // Initialize Python runtime once for the entire lifetime of this class
        try
        {
            Runtime.PythonDLL = @"C:\Users\Jimzord12\AppData\Local\Programs\Python\Python311\python311.dll";
            PythonEngine.Initialize();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Initialization failed: {ex.Message}");
        }
    }
    public static void LetTheShakeRest() {
        // Shutdown the Python Runtime
        PythonEngine.Shutdown();
    }
    public static List<string> RakeRunner(int min, int max, string text) {

        using (Py.GIL())
        {
            // Import NLTK and download the punkt tokenizer
            dynamic nltk = Py.Import("nltk");
            if (!PyModulesOnlyOnce) {
                nltk.download("punkt");
                nltk.download("stopwords");
                nltk.download("averaged_perceptron_tagger");
                PyModulesOnlyOnce = true;
            }

            // Import rake_nltk
            dynamic rakeNltk = Py.Import("rake_nltk");
            dynamic Rake = rakeNltk.Rake;

            // Initialize a Rake object
            dynamic r = Rake(min_length: min, max_length: max);

            // Filter Text
            string filteredText = FilterText(text, nltk);

            // Extract keywords
            r.extract_keywords_from_text(filteredText);
            // dynamic rankedKeywords = r.get_ranked_phrases_with_scores();
            dynamic keywords = r.get_ranked_phrases();

            // Convert dynamic 'keywords' to List<string> manually
            HashSet<string> uniqueKeywords = new();
            foreach (PyObject pyObj in keywords)
            {
                uniqueKeywords.Add(pyObj.ToString() ?? "XX_ERROR_XX");
            }
            
            List<string> filteredKeywords = uniqueKeywords.ToList();

            return filteredKeywords;
        }
    }
    static string FilterText(string originalText, dynamic nltk)
        {
            // Remove non-ASCII characters
            string filteredText = Regex.Replace(originalText, @"[^\x20-\x7E]", string.Empty);

            // Remove text inside parentheses
            filteredText = Regex.Replace(filteredText, @"\([^)]*\)", "");

            // Remove numbers and symbols
            filteredText = Regex.Replace(filteredText, @"[^a-zA-Z\s]", "");

            // Now we remove verbs; for this, we will use NLTK in Python
            
            // dynamic nltk = Py.Import("nltk");
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
                if (!tag.StartsWith("VB") && word.Length <= 60 && word != " ")
                {
                    filteredWords.Add(word);
                }
            }
            return string.Join(" ", filteredWords);
        }
}
