using System;
using System.IO;
using Microsoft.ML.Data;

namespace SlicesClassifier.Models
{
    public class ModelInput
    {
        [ColumnName("Label"), LoadColumn(0)]
        public string Label { get; set; }

        [ColumnName("ImageSource"), LoadColumn(1)]
        public string ImageSource { 
            get {
                return BasePath + _ImageSource; 
            } 
            set {
                _ImageSource = value;
            } 
        }

        private string _ImageSource;
        private static string BasePath { get; set; }

        internal static void SetDataBasePath(string basePath)
        {
            BasePath = ModelBuilder.GetAbsolutePath(Directory.GetParent(basePath).ToString()) + "/";
        }
    }
}
