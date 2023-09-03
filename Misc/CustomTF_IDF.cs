/*
using System;
using System.Collections.Generic;
using System.Linq;

using Patrikakis.Types;

namespace Patrikakis;

public static class TF_IDF
{
    public static List<TfIdfData> Get_TF_IDF_From(List<DocumentData> documents)
    {
        // Example data (multiple documents from multiple authors)

            // List<DocumentData> documents = new List<DocumentData>
            // {
            //     new DocumentData { Author = "Author1", Keywords = new List<string> { "keyword1", "keyword2" } },
            //     new DocumentData { Author = "Author2", Keywords = new List<string> { "keyword1", "keyword3" } },
            //     // add more...
            // };


        // Calculate IDF for all terms
        Dictionary<string, double> idfScores = CalculateIDF(documents);

        List<TfIdfData> result = new List<TfIdfData>();

        // Calculate TF-IDF for each document
        foreach (var document in documents)
        {
            result = CalculateTFIDF(document.Keywords, idfScores);
        }

        return result;
    }

    static Dictionary<string, double> CalculateIDF(List<DocumentData> documents)
    {
        var idfScores = new Dictionary<string, double>();
        var totalDocuments = documents.Count;

        // Flatten all document keywords and get distinct ones
        var allKeywords = documents.SelectMany(x => x.Keywords).Distinct();

        foreach (var keyword in allKeywords)
        {
            // Count the number of documents that contain the keyword
            var numberOfDocumentsWithKeyword = documents.Count(d => d.Keywords.Contains(keyword));
            // Calculate IDF score
            idfScores[keyword] = Math.Log((double)totalDocuments / (1 + numberOfDocumentsWithKeyword));
        }

        return idfScores;
    }

    static List<TfIdfData> CalculateTFIDF(List<string> documentKeywords, Dictionary<string, double> idfScores)
    {
        var tfIdfScores = new List<TfIdfData>();
        var totalTerms = documentKeywords.Count;

        // Count the frequency of each term in the document
        var termCounts = documentKeywords.GroupBy(x => x)
                                         .ToDictionary(gdc => gdc.Key, gdc => gdc.Count());

        // Calculate TF-IDF
        foreach (var term in termCounts.Keys)
        {
            var tf = (double)termCounts[term] / totalTerms;
            var idf = idfScores.ContainsKey(term) ? idfScores[term] : 0;
            var tfIdf = tf * idf;

            tfIdfScores.Add(new TfIdfData { Term = term, Score = tfIdf });
        }

        return tfIdfScores;
    }
}
*/