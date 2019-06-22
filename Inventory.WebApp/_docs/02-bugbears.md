# Bugbears

Bugbear's definition: a cause of obsessive fear, anxiety, or irritation.

## SET MSBuildSDKsPath

This is an issue that is related to OmniSharp.

Sometimes OmniSharp will report error loading projects and the following error message is given:
"The SDK 'Microsoft.Net.Sdk' specified could not be found."

The way to fix is is to manually(?) specify the SDK using the `DOTNET_MSBUILD_SDK_RESOLVER_SDKS_DIR` environment variable.

For example:

```CMD
SET MSBuildSDKsPath=C:\Program Files\dotnet\sdk\<sdk-version>\Sdks

SET MSBuildSDKsPath=C:\Program Files\dotnet\sdk\2.1.603\Sdks

SET MSBuildSDKsPath=C:\Program Files\dotnet\sdk\3.0.100-preview4-011223\Sdks
```

ZX: It maybe desirable to set an environment variable to the underlying SDK path.
    For example set it to `DOTNET_SDK`, then the above can be re-written as:

```CMD
    SET DOTNET_SDK=C:\Program Files\dotnet\sdk
    SET MSBuildSDKsPath=%DOTNET_SDK%\2.1.603\Sdks
```

A full example of the error message that you may receive:

```CMD
[warn]: OmniSharp.MSBuild.ProjectManager
        Failed to load project file 'd:\src\github.com\me\inventory\Inventory.WebApp\Inventory.WebApp.csproj'.
d:\src\github.com\me\inventory\Inventory.WebApp\Inventory.WebApp.csproj(1,1)
Microsoft.Build.Exceptions.InvalidProjectFileException: The SDK 'Microsoft.NET.Sdk.Web' specified could not be found.  d:\src\github.com\me\inventory\Inventory.WebApp\Inventory.WebApp.csproj

   at Microsoft.Build.Shared.ProjectErrorUtilities.ThrowInvalidProject(String errorSubCategoryResourceName, IElementLocation elementLocation, String resourceName, Object[] args)
   at Microsoft.Build.Evaluation.Evaluator`4.ExpandAndLoadImportsFromUnescapedImportExpressionConditioned(String directoryOfImportingFile, ProjectImportElement importElement, List`1& projects, SdkResult& sdkResult, Boolean throwOnFileNotExistsError)
   at Microsoft.Build.Evaluation.Evaluator`4.ExpandAndLoadImports(String directoryOfImportingFile, ProjectImportElement importElement, SdkResult& sdkResult)
   at Microsoft.Build.Evaluation.Evaluator`4.EvaluateImportElement(String directoryOfImportingFile, ProjectImportElement importElement)
   at Microsoft.Build.Evaluation.Evaluator`4.PerformDepthFirstPass(ProjectRootElement currentProjectOrImport)
   at Microsoft.Build.Evaluation.Evaluator`4.Evaluate(ILoggingService loggingService, BuildEventContext buildEventContext)
   at Microsoft.Build.Evaluation.Project.Reevaluate(ILoggingService loggingServiceForEvaluation, ProjectLoadSettings loadSettings, EvaluationContext evaluationContext)
   at Microsoft.Build.Evaluation.Project.ReevaluateIfNecessary(ILoggingService loggingServiceForEvaluation, ProjectLoadSettings loadSettings, EvaluationContext evaluationContext)
   at Microsoft.Build.Evaluation.Project.Initialize(IDictionary`2 globalProperties, String toolsVersion, String subToolsetVersion, ProjectLoadSettings loadSettings, EvaluationContext evaluationContext)
   at Microsoft.Build.Evaluation.Project..ctor(String projectFile, IDictionary`2 globalProperties, String toolsVersion, String subToolsetVersion, ProjectCollection projectCollection, ProjectLoadSettings loadSettings, EvaluationContext evaluationContext)
   at Microsoft.Build.Evaluation.ProjectCollection.LoadProject(String fileName, IDictionary`2 globalProperties, String toolsVersion)
   at OmniSharp.MSBuild.ProjectLoader.EvaluateProjectFileCore(String filePath)
   at OmniSharp.MSBuild.ProjectLoader.BuildProject(String filePath)
   at OmniSharp.MSBuild.ProjectFile.ProjectFileInfo.Load(String filePath, ProjectLoader loader)
   at OmniSharp.MSBuild.ProjectManager.LoadOrReloadProject(String projectFilePath, Func`1 loadFunc)
```
