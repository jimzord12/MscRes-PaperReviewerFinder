using Patrikakis.Types;
using static Patrikakis.Helpers.DataHandler;

namespace Patrikakis;
static class AuthorHandler
{
    public static List<Author> AllAuthors { get; set; }
    static public List<Author> GetAuthorsFrom(string rootPath) {

        List<Author> authors = new List<Author>();

        // 1. Go to the provided root directory
        if (Directory.Exists(rootPath))
        {
            int authorNum = 1;
            // 2. Start to loop through all the authors folders
            foreach (var authorFolderPath in Directory.GetDirectories(rootPath))
            {
                Author author;
                var authorName = new DirectoryInfo(authorFolderPath).Name;

                string filePath = Path.Combine(authorFolderPath, $"{authorName}.json");

                if(JsonFileExists(authorFolderPath, $"{authorName}.json"))
                {
                    author = ImportFromJSON<Author>(filePath);
                    // Console.WriteLine($"|-> ({authorNum}) Author: [{authorName}] Already Exists! Therefore is getting Imported");
                } else {
                    author = new Author(authorName, authorFolderPath);
                }

                authors.Add(author);
                authorNum++;
            }

            AllAuthors = authors;
            // 5. Finally return a List<Author>
            return authors;
        }
        else
        {
            throw new DirectoryNotFoundException($"The directory {rootPath} was not found.");
        }
        
    }
    public static Author GetAuthorObjectByName(string name) {
        
        foreach (Author author in AllAuthors)
        {
            if (author.Name == name) return author;
        };
        
        throw new InvalidOperationException();
    }
}