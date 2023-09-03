using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;

namespace Patrikakis;
public static class Helpers
{
    public static string HashThis(string textToHash) {

        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(textToHash));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            Console.WriteLine("SHA256 Hash: " + sBuilder.ToString());
            return sBuilder.ToString();
        }
    }
    public static List<string> GetZipFilesInDirectory(string directoryPath)
    {
        List<string> zipFiles = new List<string>();

        if (Directory.Exists(directoryPath))
        {
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToLower() == ".zip")
                {
                    zipFiles.Add(Path.GetFileName(file));
                }
            }
        }
        else
        {
            Console.WriteLine("Directory does not exist.");
        }

        return zipFiles;
    }
    public static bool CheckFileInSubDirectories(string directoryPath, string fileName)
    {
        // Get all subdirectories
        string[] subDirectories = Directory.GetDirectories(directoryPath);

        // Check each subdirectory
        foreach (string subDir in subDirectories)
        {
            string fullPath = Path.Combine(subDir, fileName);
            if (File.Exists(fullPath))
            {
                return true;
            }
        }
        return false;
    }
    public static double CalculateKeywordRatio(int documentAmount, int keywords)
    {
        // Check for zero keywords
        if (keywords == 0)
        {
            return -1.0;
        }

        // Calculate the expected number of keywords for the given number of documents
        int expectedKeywords = documentAmount * 500;

        // Calculate and return the ratio
        return (double) keywords / expectedKeywords;
    }
    public static string GetSafeUserInput(bool isDir = false) {
        string? input;
        
        do {
            input = Console.ReadLine();

            if (string.IsNullOrEmpty(input)) {
                Console.WriteLine("Wrong Input, please try again.");
            }
            
        } while (string.IsNullOrEmpty(input) || (isDir && !Directory.Exists(input)));

        return input;
    }
    public static uint GetSafeUserInputAsUInt()
    {
        string? input;
        uint output;

        do
        {
            input = Console.ReadLine();

            if (string.IsNullOrEmpty(input) || !uint.TryParse(input, out output))
            {
                Console.WriteLine("Wrong Input, please enter a valid unsigned integer.");
            }

        } while (string.IsNullOrEmpty(input) || !uint.TryParse(input, out output));

        return output;
    }
    public static int GetSafeIntegerWithinRange(int start, int end)
    {
        int result;

        while (true)
        {
            string input = Console.ReadLine();

            if (int.TryParse(input, out result) && result >= start && result <= end)
            {
                break;
            }
            else
            {
                Console.WriteLine($"Invalid input. Please enter an integer between {start} and {end}.");
            }
        }

        return result;
    }
    public static string GetSafeUserInputABS_Dir(bool isDir = false) {
        string? input;
        bool isABSPath;
        
        do {
            input = GetSafeUserInput(true);
            isABSPath = Path.IsPathRooted(input);

            if (string.IsNullOrEmpty(input)) {
                Console.WriteLine("Wrong Input, please try again.");
                continue;
            }

            if (!isABSPath || !Directory.Exists(input)) {
                Console.WriteLine("Path is not absolute OR it does not exist, please try again.");
            }
            
        } while (string.IsNullOrEmpty(input) || (isDir && !Directory.Exists(input)));

        return input;
    }
    public static string GetLastDirectoryName(string directoryPath) {
        // Normalize the path to ensure it ends without a backslash
        string normalizedPath = directoryPath.TrimEnd('\\', '/');

        // Get the last folder name
        return Path.GetFileName(normalizedPath);
        }
    public static bool WaitForUserKeyPress()
    {
        ConsoleKeyInfo keyInfo;
        
        do
        {
            
            keyInfo = Console.ReadKey(true);
            
            if (keyInfo.Key == ConsoleKey.Q)
            {
                return true;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                return false;
            }

            Console.WriteLine("Press 'q' for (Main Menu) or 'Enter' to (repeat the process)");

        } while (true);
    }
    public static string GetRunningDirectory()
    {
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        string directory = Path.GetDirectoryName(assemblyLocation);
        return directory;
    }
    public static class DataHandler
    {
        public static void ExportToJSON<T>(string filePath, T dataToExport)
        {
            string json = JsonConvert.SerializeObject(dataToExport, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static T ImportFromJSON<T>(string filePath)
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
        public static bool JsonFileExists(string directoryPath, string fileName)
        {
            string fullPath = Path.Combine(directoryPath, fileName);
            return File.Exists(fullPath);
        }
        public static void DeleteAllTxtFiles(string directoryPath)
        {
            // Check if the directory exists
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
            }

            // Get all .txt files in the directory
            string[] txtFiles = Directory.GetFiles(directoryPath, "*.txt");

            // Delete each .txt file
            foreach (var file in txtFiles)
            {
                File.Delete(file);
            }
        }
        public static void DeleteSpecificTxtFile(string directoryPath, string fileName)
        {
            // Check if the directory exists
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
            }

            // Construct the full path of the file
            string filePath = Path.Combine(directoryPath, fileName);

            // Check if the file is a .txt file
            if (Path.GetExtension(filePath) != ".txt")
            {
                throw new InvalidOperationException("The specified file is not a .txt file");
            }

            // Check if the file exists and delete
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
        }
    }
}