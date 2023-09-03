using Google.Protobuf.WellKnownTypes;
using Patrikakis.Types;
using System;

namespace Patrikakis
{
    public static class PrepareDataForML
    {
        public static List<PaperData> Refine(List<Author> authors) {
            List<PaperData> data = new();
            Console.WriteLine("- Received Authors: {0}", authors.Count);

            foreach (Author author in authors)
            {
                Console.WriteLine("- Selected Author: [{0}]", author.Name);
                Console.WriteLine("- Author Papers Amount: [{0}]", author.Papers.Count);
                foreach (Paper paper in author.Papers) {
                    Console.WriteLine("*** Author: [{0}] Paper Name: [{1}]", author.Name, paper.Name);

                    string keywordString = KeywordExtractor.ReadTxtFile(paper.KeywordsRaw);

                    if (keywordString == string.Empty) continue; // <- This line Skips Files that have no keywords
                    // TODO: Optimize this by not creating empty files in the first place!

                    data.Add(new PaperData() { Author = author.Name, Keywords = keywordString });

                    Console.WriteLine("*** Data Length: [{0}]", data.Count);
                }

                Console.WriteLine($"[{author.Name}]: Number of PaperData Elements: ({data.Count})");
            }
            return data;
        }
    }
}

