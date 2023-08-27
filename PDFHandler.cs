using System.Linq;

namespace Patrikakis
{
    static public class PDFHandler
    {
        static public string GetTextFromPDF(string pdfPath) {

            PdfReader pdf = new(pdfPath);
            return pdf.ExtractTextFromPdf();
        }
        static public List<string> GetPapersFromDir(string dirPath) {
            List<string> paths = GetABSPaths(dirPath);
            return (from path in paths select GetTextFromPDF(path)).ToList();
        }
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
    }
}
