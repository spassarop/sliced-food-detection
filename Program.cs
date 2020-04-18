using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using Microsoft.ML;
using Microsoft.ML.Vision;
using SlicesClassifier.Models;
using SlicesClassifier.Options;

namespace SlicesClassifier
{
    class Program
    {
        private static readonly string DEFAULT_PROJECT_PATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
        private static readonly string MODEL_FILEPATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "MLModel.zip"));

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<BuildOptions, PredictOptions>(args)
             .WithParsed<BuildOptions>(options => StartBuildModule(options))
             .WithParsed<PredictOptions>(options => StartPredictionModule(options));
        }

        private static void StartBuildModule(BuildOptions options)
        {
            string projectPath = string.IsNullOrEmpty(options.ProjectPath) ? DEFAULT_PROJECT_PATH : options.ProjectPath;
            string trainDataFilePath = string.IsNullOrEmpty(options.DatasetFilePath) ? 
                Path.Combine(projectPath, "Dataset/TrainAndValidation/DatasetFiles.tsv") : options.DatasetFilePath;

            var trainerOptions = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = "Image",
                LabelColumnName = "LabelAsKey",
                Arch = ImageClassificationTrainer.Architecture.ResnetV2101,
                BatchSize = 10,
                Epoch = 200,
                MetricsCallback = Console.WriteLine,
                LearningRate = 0.01f,
                ReuseTrainSetBottleneckCachedValues = true,
                ReuseValidationSetBottleneckCachedValues = true,
                WorkspacePath = Path.Combine(projectPath, "Workspace")
            };

            ModelBuilder.CreateModel(trainerOptions, trainDataFilePath);
        }

        private static void StartPredictionModule(PredictOptions options)
        {
            string projectPath = string.IsNullOrEmpty(options.ProjectPath) ? DEFAULT_PROJECT_PATH : options.ProjectPath;
            string testDataFilePath = string.IsNullOrEmpty(options.TestDataFilePath) ?
                Path.Combine(projectPath, "Dataset/Testing/TestingFiles.tsv") : options.TestDataFilePath;
            string modelFilePath = string.IsNullOrEmpty(options.ModelFilePath) ?
                MODEL_FILEPATH : options.ModelFilePath;

            var modelConsumer = new ConsumeModel(modelFilePath);

            if (!string.IsNullOrEmpty(options.TestImageFilePath))
            {
                // Make a single prediction on the sample data and print results
                Console.WriteLine("Using model to make single prediction...\n\n");
                ModelInput sampleData = CreateSingleDataSample(
                    options.TestImageFilePath, 
                    string.IsNullOrEmpty(options.TestImageLabel) ? "N/A" : options.TestImageLabel);
                PredictAndPrintResult(modelConsumer, sampleData);
            }
            else
            {
                ModelInput.SetDataBasePath(testDataFilePath);
                List<ModelInput> testData = CreateDataSamples(testDataFilePath);

                Console.WriteLine("Using model to make multiple predictions...\n");
                foreach (ModelInput sampleData in testData)
                {
                    PredictAndPrintResult(modelConsumer, sampleData);
                }
            }

            Console.WriteLine("=============== End of process ===============");
        }

        private static void PredictAndPrintResult(ConsumeModel modelConsumer, ModelInput sampleData)
        {
            var predictionResult = modelConsumer.Predict(sampleData);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"ImageSource: {sampleData.ImageSource}");
            Console.WriteLine($"Actual label: {sampleData.Label}");
            Console.WriteLine($"Predicted Label value {predictionResult.Prediction}");
            Console.WriteLine($"Predicted Label scores: [{string.Join(",", predictionResult.Score)}]");
        }

        #region Create Data Samples
        // Method to load single image to try a single prediction
        private static ModelInput CreateSingleDataSample(string imagePath, string label) => new ModelInput()
        {
            ImageSource = imagePath,
            Label = label
        };

        // Method to load single row of dataset to try a single prediction
        private static ModelInput CreateSingleDataSampleFromTrainingData(string dataFilePath) => CreateDataSamples(dataFilePath).First();

        // Method to load rows of dataset to try multiple predictions
        private static List<ModelInput> CreateDataSamples(string dataFilePath)
        {
            // Create MLContext
            MLContext mlContext = new MLContext();

            // Load dataset
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: dataFilePath,
                                            hasHeader: true,
                                            separatorChar: '\t',
                                            allowQuoting: true,
                                            allowSparse: false);

            return mlContext.Data.CreateEnumerable<ModelInput>(dataView, false).ToList();
        }
        #endregion
    }
}
