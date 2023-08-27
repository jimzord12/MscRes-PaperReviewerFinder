using Patrikakis;
using static Patrikakis.PDFHandler;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

string testPath = @"C:\Users\BlockHead\Desktop\MscRes\PATRIKAKIS_KeywordsProject\AuthorsAndPapers\Blockchain\Vitalik Buterin";
List<string> papers = GetPapersFromDir(testPath);

KeywordExtractor.ExtractMulti(papers);



