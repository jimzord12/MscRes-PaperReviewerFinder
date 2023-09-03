using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.AutoML;
using Patrikakis.Types;

namespace Patrikakis.Models;

static class UntrainedModel
    {
        // private static PredictionEngine Engine { get; set; }
        public static void TrainModel(List<PaperData> paperList, uint trainTime, string nameForModel) {

            // 3. Load the Prepared data to ML Context
            var mlContext = new MLContext();

            // 4. Load the Data
            var data = mlContext.Data.LoadFromEnumerable(paperList);

            var columnNames = data.Schema.Select(c => c.Name).ToList();
            if (!columnNames.Contains("Author")) {
                Console.WriteLine("[Error]: Author column is missing");
                return;
            } else {
                // Console.WriteLine("3) ALL GOOD!");
            }

            // Define settings for AutoML experiment
            var experimentSettings = new MulticlassExperimentSettings
            {
                MaxExperimentTimeInSeconds = trainTime  // Experiment in seconds
            };

            // Console.WriteLine($"Training Model...");

            // Execute AutoML experiment
            var experiment = mlContext.Auto().CreateMulticlassClassificationExperiment(experimentSettings);
            
            var result = experiment.Execute(data, labelColumnName: "Author");

            // Get the best model from the experiment
            var bestModel = result.BestRun.Model;
            
            mlContext.Model.Save(bestModel, data.Schema, $@"StoredModels\{nameForModel}.zip");

        }
    }