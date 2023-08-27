using System;
using System.IO;
internal static class CSVHandler {

    internal static void Read(string filePath) {
        
        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            string[] columns = line.Split(',');
            foreach (string column in columns)
            {
                Console.Write(column + " ");
            }
            Console.WriteLine();
        }
    }

        internal static void Write(string filePath) {
        
        string[][] output = new string[][] {
            new string[] {"Column 1", "Column 2", "Column 3"},
            new string[] {"Data 1", "Data 2", "Data 3"}
        };

        using StreamWriter writer = new StreamWriter(filePath);
        foreach (string[] line in output)
        {
            writer.WriteLine(string.Join(",", line));
        }
    }

}