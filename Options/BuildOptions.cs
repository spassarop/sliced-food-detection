using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace SlicesClassifier.Options
{
    [Verb("build", HelpText = "Builds a model for an image classifier for sliced/unsliced food.")]
    class BuildOptions
    {
        [Option('p', "project-path", Required = false, HelpText = "Project base path.")]
        public string ProjectPath { get; internal set; }

        [Option('d', "dataset", Required = false, HelpText = "Dataset TSV file with images paths.")]
        public string DatasetFilePath { get; internal set; }

        [Usage(ApplicationAlias = "SlicesClassifier")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                    new Example("Build classifier specifying project folder path", new BuildOptions {
                        ProjectPath = "./",
                        DatasetFilePath = "dataset.tsv",
                    }),
                };
            }
        }

    }
}
