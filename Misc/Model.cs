/* <- DELETE this to remove the Comment
using Microsoft.ML;
using System;
using System.Collections.Generic;

using Patrikakis.Types;
using Patrikakis;

public class Model
{
    public void TF_IDF(Author author) {

        Console.WriteLine("####################################");
        Console.WriteLine();
        Console.WriteLine("Creating ML Context...");

        var mlContext = new MLContext();

        Console.WriteLine("Getting Data from .txt files...");
        // Assume we have this data from your .tsv files
        var paperList = GetFormatedData(author);

        Console.WriteLine("Loading the data into ML.NET data structures...");
        // Load the data into ML.NET data structures
        var data = mlContext.Data.LoadFromEnumerable(paperList);

        Console.WriteLine("Defining pipeline and add TF-IDF transformation...");
        // Define pipeline and add TF-IDF transformation
        var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", "Keywords");

        Console.WriteLine("Training the model...");
        // Train the model
        var model = pipeline.Fit(data);

        Console.WriteLine("Transforming the data...");
        // Transform the data
        var transformedData = model.Transform(data);

        Console.WriteLine("Saving the model...");
        // Inspect or save the model
        var preview = transformedData.Preview();

        Console.WriteLine("Data Preview");
        Console.WriteLine();
        Console.WriteLine(preview);

        mlContext.Model.Save(model, transformedData.Schema, "tfidfModel.zip");

        // Create an enumerable from the transformed data
        var transformedDataEnum = mlContext.Data.CreateEnumerable<TransformedPaperData>(transformedData, reuseRowObject: false);

        // Loop through each row and print the length of the Features vector
        foreach (var row in transformedDataEnum)
        {
            Console.WriteLine("Length of Features Vector: " + row.Features.Length);
        }

        // Print TF-IDF values
        // foreach (var row in transformedDataEnum)
        // {
        //     Console.WriteLine($"Author: {row.Author}, Features: {String.Join(",", row.Features)}\n");
        // }
    }

    public class TransformedPaperData : PaperData
    {
        public float[] Features { get; set; }
    }   

    public List<PaperData> GetFormatedData(Author author) {

        int sum = 0;

        List<PaperData> papers = new List<PaperData>();
        List<Paper> authorsPapers = author.Papers;
        for (int i = 0; i < author.Papers.Count; i++)
        {
            string keywords = KeywordExtractor.ReadTxtFile(authorsPapers[i].KeywordsRaw);

            papers.Add(new PaperData { Author = author.Name, Keywords = keywords });

            Console.WriteLine($"Document ({i + 1}): " + keywords.Split(' ').Length);
            sum += keywords.Split(' ').Length;    
            // Console.WriteLine($"Document ({i + 1}):" + keywords);
        }
            Console.WriteLine($"Total Keywords from Corpus: " + sum);
        return papers;
    }
}
*/ //<- DELETE this to remove the Comment