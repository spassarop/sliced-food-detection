using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace SlicesClassifier.Options
{
    [Verb("predict", HelpText = "Builds a model for an image classifier for sliced/unsliced food.")]
    class PredictOptions
    {
        [Option('t', "test-data", Required = false, HelpText = "TSV file with test images paths.", SetName = "multiple")]
        public string TestDataFilePath { get; internal set; }

        [Option('i', "image", Required = false, HelpText = "Single image file path for prediction.", SetName = "single")]
        public string TestImageFilePath { get; internal set; }

        [Option('l', "label", Required = false, HelpText = "Single image label to compare after prediction.", SetName = "single")]
        public string TestImageLabel { get; internal set; }

        [Option('p', "project-path", Required = false, HelpText = "Project base path.")]
        public string ProjectPath { get; internal set; }

        [Option('m', "model", Required = false, HelpText = "ZIP file with trained model.")]
        public string ModelFilePath { get; internal set; }

        [Usage(ApplicationAlias = "SlicesClassifier")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                    new Example("Predict for single image", new PredictOptions {
                        TestImageFilePath = "test-image-sliced.jpg",
                        TestImageLabel = "Sliced",
                        ModelFilePath = "ML.zip",
                        ProjectPath = "./",
                    }),
                    new Example("Predict for images in a specific TSV file", new PredictOptions {
                        TestDataFilePath = "TestingFiles.tsv",
                        ModelFilePath = "ML.zip",
                        ProjectPath = "./",
                    }),
                };
            }
        }
    }
}
