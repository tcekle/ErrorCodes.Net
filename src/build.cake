// Cake settings
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Build settings
var solutionFolder = ".";
var outputFolder = "./artifacts";
var errorCodeNetProject = System.IO.Path.Combine(solutionFolder, @"ErrorCodes.Net.Analyzers\ErrorCodes.Net.Analyzers.csproj");
var errorCodeCliProject = System.IO.Path.Combine(solutionFolder, @"errorcodes-net-cli\errorcodes-net-cli.csproj");
var helpLocation = "../docs/help";

var cleanTask = Task("Clean")
  .Does(() => {
    CleanDirectory(outputFolder);
    DotNetClean(solutionFolder, new DotNetCleanSettings()
    {
      Configuration = configuration
    });
  });

var restoreTask = Task("Restore")
  .Does(() =>
  {
    DotNetRestore(solutionFolder);
  });

var validateErrorsTask = Task("ValidateErrors")
  .Does(() => {
    var result = StartProcess("dotnet", $"errorcode-net-cli validate --solution {solutionFolder}\\ErrorCodes.Net.sln --validation-type CheckUniqueness");

    if (result != 0)
    {
      throw new Exception("Error validating error codes");
    }
  });

var buildTask = Task("Build")
  .IsDependentOn(cleanTask)
  .IsDependentOn(restoreTask)
  .IsDependentOn(validateErrorsTask)
  .Does(() => {
    // ErrorCodes.Net solution
    DotNetBuild(solutionFolder, new DotNetBuildSettings{
      NoRestore = true,
      Configuration = configuration,
      MSBuildSettings = new DotNetMSBuildSettings()
      {
        TreatAllWarningsAs = MSBuildTreatAllWarningsAs.Error
      }
    });
  });

var testTask = Task("Test")
  .IsDependentOn(buildTask)
  .Does(() => {
    DotNetTest(solutionFolder, 
      new DotNetTestSettings {
        NoRestore = true,
        Configuration = configuration,
        NoBuild = true,
        VSTestReportPath = @"artifacts/TestResults.trx",
      });
  });

var nugetPack = Task("NugetPack")
  .IsDependentOn(testTask)
  .Does(() => {

    var packSettings = new DotNetPackSettings {
        Configuration = configuration,
        NoBuild = true,
        NoRestore = true,
        OutputDirectory = outputFolder
    };

    // ErrorCodes.Net.Analyzers nuget package
    DotNetPack(errorCodeNetProject, packSettings);

    // errorcodes-net-cli nuget package
    DotNetPack(errorCodeCliProject, packSettings);
  });

  // Set default task to run
Task("Default")
  .IsDependentOn(nugetPack);

RunTarget(target);