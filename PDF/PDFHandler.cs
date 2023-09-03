using System.Linq;

namespace Patrikakis
{
    static public class PDFHandler
    {
        // *** Main Method ***
        static public List<string> GetPapersFromDir(string dirPath) {
            List<string> paths = GetABSPaths(dirPath);
            return (from path in paths select GetTextFromPDF(path)).ToList();
        }
        static public string GetTextFromPDF(string pdfPath) {
            PdfReader pdf = new(pdfPath);
            return pdf.ExtractTextFromPdf();
        }

        // -= Helper Methods =-
        private static List<string> GetABSPaths(string dirPath)
        {
            List<string> absPaths = new List<string>();

            try
            {
                // Get an array of PDF file names (absolute paths) in the directory
                string[] files = Directory.GetFiles(dirPath, "*.pdf");

                // Add each file to the list
                absPaths.AddRange(files);
            }
            catch (Exception e)
            {
                // Handle exceptions such as unauthorized access or invalid path
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            return absPaths;
        }
        public static string GetPDFPathFromUser() 
        {
            bool isABSPath;
            bool isPDF = false;
            bool fileExists = false;
            string input;

            do
            {
                input = Helpers.GetSafeUserInput();
                isABSPath = Path.IsPathRooted(input);
                
                if (!isABSPath)  {
                    Console.WriteLine("This path does not appear to be an Absolute path, please try again.");
                    continue;
                }

                isPDF = Path.GetExtension(input).ToLower() == ".pdf";
                // Check if the file has a '.pdf' extension
                if (!isPDF)
                {
                    Console.WriteLine("The provided path does not point to a PDF file, please try again.");
                    continue;
                }

                fileExists = File.Exists(input);

                if (!fileExists)
                {
                    Console.WriteLine("No PDF file exists in the provided path, please try again.");
                }
                
            } while (!isABSPath || !isPDF || !fileExists);

            return input;
        }
    }
}
