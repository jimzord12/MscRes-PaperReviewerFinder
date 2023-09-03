using Microsoft.ML.Data;

namespace Patrikakis.Types
{
    public enum Fields { Artificial_Intelligence, Blockchain, Cybersecurity, DataScience_and_BigData }
    public class Author
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<Paper> Papers { get; set; }

        public static readonly Dictionary<Fields,string[]> fieldToAuthors = new Dictionary<Fields,string[]>() 
        {
            [Fields.Artificial_Intelligence] = new string[] 
            {"Andrew Ng", "Geoffrey Hinton", "Judea Pearl", "Yann Lecun", "Yoshua Bengio"},

            [Fields.Blockchain] = new string[] 
            {"Elaine Shi", "Emin Gün Sirer", "Joseph Bonneau", "Madhusanka Liyanage", "Vitalik Buterin"},
            
            [Fields.Cybersecurity] = new string[] 
            {"Adi Shamir", "Angelos Stavrou", "Bhavani Thuraisingham", "Josh Pauli", "Wenjing Lou"},
            
            [Fields.DataScience_and_BigData] = new string[] 
            {"Jeffrey Ullman", "Jiawei Han", "Jure Leskovec", "Trevor Hastie", "Vipin Kumar"}
        };
        public static readonly Dictionary<string,Fields> authorToField = new() 
        {
            ["Andrew Ng"] = Fields.Artificial_Intelligence,
            ["Geoffrey Hinton"] = Fields.Artificial_Intelligence,
            ["Judea Pearl"] = Fields.Artificial_Intelligence,
            ["Yann Lecun"] = Fields.Artificial_Intelligence,
            ["Yoshua Bengio"] = Fields.Artificial_Intelligence,

            ["Elaine Shi"] = Fields.Blockchain,
            ["Emin Gün Sirer"] = Fields.Blockchain,
            ["Joseph Bonneau"] = Fields.Blockchain,
            ["Madhusanka Liyanage"] = Fields.Blockchain,
            ["Vitalik Buterin"] = Fields.Blockchain,

            ["Adi Shamir"] = Fields.Cybersecurity,
            ["Angelos Stavrou"] = Fields.Cybersecurity,
            ["Bhavani Thuraisingham"] = Fields.Cybersecurity,
            ["Josh Pauli"] = Fields.Cybersecurity,
            ["Wenjing Lou"] = Fields.Cybersecurity,

            ["Jeffrey Ullman"] = Fields.DataScience_and_BigData,
            ["Jiawei Han"] = Fields.DataScience_and_BigData,
            ["Jure Leskovec"] = Fields.DataScience_and_BigData,
            ["Trevor Hastie"] = Fields.DataScience_and_BigData,
            ["Vipin Kumar"] = Fields.DataScience_and_BigData,

            ["Problematic"] = Fields.Artificial_Intelligence, // Geoffrey Hinton
            ["noProblem"] = Fields.Blockchain, // Emin Gün Sirer


        };

        public Author(string _name, string _path) {
            Name = _name;
            Path = _path;
        }

        public bool InTheSameField(string authorName) {
            return authorToField[this.Name] == authorToField[authorName];
        }
        public Fields GetField() {
            return authorToField[this.Name];
        }
        public static Fields GetField(string authorName) {
            return authorToField[authorName];
        } 
    }

    public record class Paper
    (
        string Name, string KeywordsTSV, string KeywordsRaw, string KeywordsJSON,string Hash
        /*
            Paper[] data = new Paper[] 
            {
                new Paper(Title: "a", "b", "c", "d")
            };
        */
    );

    public class PaperData
    {
        public string Author { get; set; }
        public string Keywords { get; set; }  // "blockchain, network, cryptography"
    }

    public class Keywords
    {
        public HashSet<string> SingleWord { get; set; }
        public HashSet<string> DoubleWord { get; set; }
        public HashSet<string> TripleWord { get; set; }
        public HashSet<string> QuadWord { get; set; }

        public string GetStringVersion() { 

            string s = ConvertToString(SingleWord); // 0
            string d = ConvertToString(DoubleWord); // 1
            string t = ConvertToString(TripleWord); // 0
            string q = ConvertToString(QuadWord);   // 0

            List<string> keywordsSetList = new() {
                s, d, t, q
            };
            List<string> temp = new();

            foreach (string keywordSet in keywordsSetList) {
                if (keywordSet != string.Empty) {
                    temp.Add(keywordSet);
                    // keywordsSetList.Remove(keywordSet);
                    // Console.WriteLine($"[GetStringVersion]: Single: {}")
                }
            }


            return string.Join(", ", temp);
        }
        private static string ConvertToString(HashSet<string> keywords) {
            // Console.WriteLine($"Keywords Number: {keywords.Count}");
            if(keywords.Count == 0) return "";

            return string.Join(", ", keywords);
        }

        public List<string> GetListVersion() { 
            List<string> s = ConvertToList(SingleWord);
            List<string> d = ConvertToList(DoubleWord);
            List<string> t = ConvertToList(TripleWord);
            List<string> q = ConvertToList(QuadWord);

            // Initialize a new list to store the combined values
            List<string> combined = new List<string>();

            // Add each list to the combined list
            combined.AddRange(s);
            combined.AddRange(d);
            combined.AddRange(t);
            combined.AddRange(q);

            return combined;
        }

        private List<string> ConvertToList( HashSet<string> keywords) {
            return keywords.ToList();
        }
    }
    public class PaperPrediction
        {
            [ColumnName("PredictedLabel")]
            public string Author;

            public float[] Score; // probabilities
        }

    // public class DocumentData
    // {
    //     public string Author { get; set; }
    //     public List<string> Keywords { get; set; }  // "blockchain, network, cryptography"
    // }

    // public class TfIdfData
    // {
    //     public string Term { get; set; }
    //     public double Score { get; set; }
    // }
}


    
