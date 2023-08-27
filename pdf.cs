using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using System.IO;

namespace Patrikakis
{
    public class PdfReader
    {
        private readonly string pdfPath;

        public PdfReader(string pathOrUrl)
        {
            pdfPath = pathOrUrl;
        }

        public string ExtractTextFromPdf()
        {
            StringBuilder textContent = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(pdfPath))
            {
                foreach (Page page in document.GetPages())
                {
                    string pageText = page.Text;
                    textContent.AppendLine(pageText);
                }
            }

            return textContent.ToString();
        }
    }
}
