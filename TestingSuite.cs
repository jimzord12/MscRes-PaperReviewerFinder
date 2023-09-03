using Patrikakis;
using Patrikakis.Types;

namespace Patrikakis.Tests;
public static class TestingSuite
{
    public static Author Author { get; set; }
    public static PaperData CreateTestData(string desiredAuthorName, int paperNum = -1) {

        Author author = CollectData.AllAuthors.Where(author => author.Name == desiredAuthorName).Single();

        if (paperNum == -1) 
        {
            Random rand = new Random();
            int randomNumber = rand.Next(1, author.Papers.Count + 1);
            paperNum = randomNumber;
        }

        KeywordExtractor.ReadTxtFile(author.Papers[paperNum - 1].KeywordsRaw);

        Display(desiredAuthorName, paperNum);
        Author = author;

        return new PaperData() {Author = author.Name, Keywords = KeywordExtractor.ReadTxtFile(author.Papers[paperNum - 1].KeywordsRaw)};
    }

    public static void Display(string authorName, int paperNum) {
        Console.WriteLine($"Selected Author for Testing: [{authorName}]");
        Console.WriteLine($"The No.({paperNum}) Paper from ({authorName}) was Selected!");
        Console.WriteLine();
    }
}
