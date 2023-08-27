using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using Python.Runtime;
using Patrikakis.Types;

namespace Patrikakis;
public class TransformedData
{
    public float[] Features { get; set; }
}
public class GPT
{
    string paper1 = "a";
    string paper2 = "a";
    string paper3 = "a";
    string paper4 = "a";
    string paper5 = "a";
    List<string> keywordList = new List<string> { "a", "b", "c", "d", "e",};
    public void A() {

        // Initialize MLContext and Python Runtime
        var context = new MLContext();
        Runtime.PythonDLL = @"C:\Python310\python310.dll";
        PythonEngine.Initialize();

        // Load Papers and Extract Keywords Using RAKE
        // List<string> keywordList = KeywordExtractor.Extract(paperText);

        // Calculate TF-IDF
        var papers = new List<string> { paper1, paper2, paper3, paper4, paper5};
        var trainData = context.Data.LoadFromEnumerable(papers);
        var pipeline = context.Transforms.Text.FeaturizeText("Features", "Text")
                        .Append(context.Transforms.NormalizeLpNorm("Features"));
        var transformer = pipeline.Fit(trainData);
        var transformedData = transformer.Transform(trainData);

        // Rank Keywords Using TF-IDF
        var featuresColumn = context.Data.CreateEnumerable<TransformedData>(transformedData, reuseRowObject: false).First().Features;

        Dictionary<string, float> keywordScores = new Dictionary<string, float>();
        for (int i = 0; i < keywordList.Count; i++)
        {
            keywordScores[keywordList[i]] = featuresColumn[i]; // Assuming the index aligns with the feature vector
        }

        var sortedKeywords = keywordScores.OrderByDescending(k => k.Value).Select(k => k.Key).ToList();

    } 


}

