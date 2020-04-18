# sliced-food-detection
ML.NET project to detect sliced from unsliced food via an image classifier.

## Execution

For building a model:

`dotnet SliceClassifier.dll build`

For making predictions with a trained model:

`dotnet SliceClassifier.dll predict -m path/to/model/MLModel.zip`

For help about parameters:

`dotnet SliceClassifier.dll build help`

`dotnet SliceClassifier.dll predict help`

## Some points to consider

* Model training options must be changed inside the code, on `ModelBuilder.cs`.
* When no parameters are used the dafaults values in the code are written to make everything work. Except for the model ZIP that needs to be placed next to the DLL.
* To this day, I could not debug the project in Visual Studio, had to run the generated DLL to read the errors.
* Best obtained model is `MLModel.zip`, with labels named `Sliced` and `NonSliced` (instead of `Unsliced`). However, new models will be generated next to the executable.
* Python script `rename-pics.py` is just an auxiliary script to change the name of the pictures in each data set folder when adding new ones.
