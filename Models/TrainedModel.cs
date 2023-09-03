using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.AutoML;
using Patrikakis.Types;

namespace Patrikakis.Models
{
    using PredictionEngine = PredictionEngine<PaperData, PaperPrediction>;
    static class TrainedModel
    {
        private static PredictionEngine Engine { get; set; }
        public static void LoadModel(string pathToModel, string modelName) {

            string completePath = Path.Combine(pathToModel,modelName + ".zip");

            var mlContext = new MLContext(); 
            
            ITransformer loadedModel = mlContext.Model.Load(completePath, out var modelSchema);

            PredictionEngine pEngine = mlContext.Model.CreatePredictionEngine<PaperData, PaperPrediction>(loadedModel);

            Engine = pEngine;

        }
        public static PaperPrediction Predict(PaperData paperData) {
            return Engine.Predict(paperData);
        }
    }
}