// using System.Diagnostics;

// using Microsoft.ML;
// using Microsoft.ML.Data;
// using Microsoft.ML.AutoML;

// using Patrikakis;
// using Patrikakis.Types;
// using Patrikakis.Tests;
// using Patrikakis.Models;


// // The Directory Structure should be like this:
// // To Access the Author_2's Paper_1: C:\...\RootPath\Author_2\Papers_1.pdf
//     /*
//     |-RootPath
//     |---Author_1
//     |-----Papers_1.pdf
//     |-----Papers_2.pdf
//     |-----Papers_N.pdf
//     |---Author_2
//     |-----Papers_1.pdf
//     |-----Papers_2.pdf
//     |-----Papers_N.pdf
//     |...
//     */

// string authorsFolderPath = @"C:\FastPaths\AuthorsAndPapers\";
// string projectPath = @"C:\Users\Jimzord12\Documents\GitHub\MscRes-PaperReviewerFinder\";

// Stopwatch stopwatch = new Stopwatch();
// stopwatch.Start();

//     (bool forceExec, bool testMode, bool useTrainedModel) = GetUserInput();

//     // 1. Collects the Data from the PDF files (Authors' Papers)
//     // List<Author> authors = CollectData.From(authorsFolderPath, isTestMode, forceExec);
//     List<Author> authors = CollectData.From(authorsFolderPath, false, true);

//     if (useTrainedModel) {

//         string authorName = "Vitalik Buterin";

//          //TODO: For production, take this from User Input:
//         PaperData inputDocument = TestingSuite.CreateTestData(authorName, -1);

//         Author currentAuthor = AuthorHandler.GetAuthorObjectByName(authorName);

//         TrainedModel.LoadModel(projectPath, "TestModel_01");
        
//         PaperPrediction prediction = TrainedModel.Predict(inputDocument);
        
//         Console.WriteLine();
//         Console.WriteLine("====================================");
//         Console.WriteLine(" Best Reviewer is: (" + prediction.Author + ")");
//         Console.WriteLine(" Where the 2 Authors in the same Field? => (" + currentAuthor.InTheSameField(prediction.Author) + ")");
//         Console.WriteLine("====================================");
//         Console.WriteLine();

//         return;
//     }

//     // 2. Prepares the retrieved Data in a format that works well with ML.NET
//     // The type PaperData: { string Author {set; get;} string Keywords {set; get} 
//     // where Keywords are simalar to this: "blockchain, cryptography, security, ..."
//     List<PaperData> paperList = PrepareDataForML.Refine(authors);

//     bool success = PerformChecks(); 

//     // Do the following:
//     // 1. Trains the Model by feeding it the PaperData
//     // 2. Saves the Trained Model in the App's directory
//     // 3. Names the Saved Trained Model based on the 3rd argument
    
//     //TODO: For production, take this from User Input:

//     UntrainedModel.TrainModel(paperList, 30, modelName);

//     PaperData inputDocument = TestingSuite.CreateTestData(authorName, -1);

//     Author currentAuthor = AuthorHandler.GetAuthorObjectByName(authorName);

//     TrainedModel.LoadModel(projectPath, modelName);
    
//     PaperPrediction prediction = TrainedModel.Predict(inputDocument);
    
//     Console.WriteLine();
//     Console.WriteLine("====================================");
//     Console.WriteLine(" Best Reviewer is: (" + prediction.Author + ")");
//     Console.WriteLine(" Where the 2 Authors in the same Field? => (" + currentAuthor.InTheSameField(prediction.Author) + ")");
//     Console.WriteLine("====================================");
//     Console.WriteLine();
    
//     stopwatch.Stop();
//     Console.WriteLine("Elapsed Time (in seconds) is: {0}", (double)stopwatch.ElapsedMilliseconds / 1000);

// (bool, bool, bool) GetUserInput() {
//     Console.WriteLine("Do you want to force execution? Yes: (y or Y) | No: Whatever");
//     bool forceExec = Console.ReadLine() == "y" || Console.ReadLine() == "Y"; 

//     Console.WriteLine("Run in [TEST MODE]? Yes: (y or Y) | No: Whatever");
//     bool isTestMode = Console.ReadLine() == "y" || Console.ReadLine() == "Y"; 

//     Console.WriteLine("Use Trained Model? Yes: (y or Y) | No: Whatever");
//     bool useTrainedModel = Console.ReadLine() == "y" || Console.ReadLine() == "Y"; 

//     return (forceExec, isTestMode, useTrainedModel);
// }

// bool PerformChecks() {
        
//         bool success = false;

//         if (paperList == null || !paperList.Any()) 
//         {
//             Console.WriteLine("paperList is null or empty");
//             return false;
//         } else {
//             Console.WriteLine("1) ALL GOOD!");
//             success = true;
//         }

//         if (paperList.Any(p => string.IsNullOrEmpty(p.Author) || string.IsNullOrEmpty(p.Keywords)))
//         {
//             Console.WriteLine("Author or Keywords field is null or empty.");
//             return false;
//         }
//         else {
//             Console.WriteLine("2) ALL GOOD!");
//             success = true;
//         }
//         return success;
// }
// public class PaperPrediction
// {
//     [ColumnName("PredictedLabel")]
//     public string Author;

//     public float[] Score; // probabilities
// }




