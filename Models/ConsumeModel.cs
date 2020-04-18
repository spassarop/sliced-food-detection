using Microsoft.ML;

namespace SlicesClassifier.Models
{
    public class ConsumeModel
    {
        private readonly MLContext Context;
        private ITransformer Model;
        private readonly DataViewSchema ModelInputSchema;
        private readonly PredictionEngine<ModelInput, ModelOutput> PredictionEngine;

        public ConsumeModel(string modelPath)
        {
            // Create new MLContext
            Context = new MLContext();
            LoadModel(Context, modelPath);
            PredictionEngine = Context.Model.CreatePredictionEngine<ModelInput, ModelOutput>(Model);
        }

        private void LoadModel(MLContext mlContext, string modelPath)
        {
            Model =  mlContext.Model.Load(modelPath, out var ModelInputSchema);
        }
        public ModelOutput Predict(ModelInput input)
        {
            // Use model to make prediction on input data
            ModelOutput result = PredictionEngine.Predict(input);
            return result;
        }
    }
}
