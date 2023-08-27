using System.Reflection;

namespace Patrikakis.Types
{
    public class Author
    {
        string Name { get; set; }
        Keywords keywords { get; set; }
        List<Paper> Papers { get; set; }
        List<Paper> ReviewedPapers { get; set; }
        float Rating { get; set; }
        bool IsAvailable { get; set; }

    }

    public record class Paper
    (
        string? Title, Keywords Keywords,  string? Pages, HashCode Hash
        /*
            Paper[] data = new Paper[] 
            {
                new Paper(Title: "a", "b", "c", "d")
            };
        */
    );

    public record class Keywords
    {
        public HashSet<string> SingleWord { get; set; }
        public HashSet<string> DoubleWord { get; set; }
        public HashSet<string> TripleWord { get; set; }
        public HashSet<string> QuadWord { get; set; }
    }

    
}