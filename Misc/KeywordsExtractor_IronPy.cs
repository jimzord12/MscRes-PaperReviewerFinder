// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using IronPython.Hosting;
// using Microsoft.Scripting.Hosting;
// using System.Diagnostics;
// using IronPython.Modules;
// using static IronPython.Modules.PythonCsvModule;

// namespace Patrikakis_NS
// {
//     public class KeywordExtractor
//     {
//         private ScriptEngine engine;
//         private dynamic rake;

//         public KeywordExtractor()
//         {
//             //engine = Python.CreateEngine();
//             dynamic sys = engine.GetSysModule();
//             engine.GetSearchPaths().Add(@"C:\Python310\Lib\site-packages"); // Update this path
//             engine.SetSearchPaths(engine.GetSearchPaths());
//             engine.Execute("from rake_nltk import Rake");
//             rake = engine.Execute("Rake()");
//         }

//         public string[] ExtractTopKeywords(string text, int topCount = 20)
//         {
//             rake.extract_keywords_from_text(text);
//             dynamic keywords = rake.get_ranked_phrases_with_scores();
//             string[] topKeywords = new string[Math.Min(topCount, keywords.Count)];

//             for (int i = 0; i < topKeywords.Length; i++)
//             {
//                 topKeywords[i] = keywords[i][1];
//             }

//             return topKeywords;
//         }
//     }
// }
