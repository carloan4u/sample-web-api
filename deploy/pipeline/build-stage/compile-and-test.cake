#tool nuget:?package=NUnit.ConsoleRunner

Environment.CurrentDirectory = Directory("../../../");

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = MakeAbsolute(Directory("build"));
var buildBinDir = buildDir + Directory("/bin");
var solutionPath = "sample-web-api.sln";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionPath);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
      MSBuild(solutionPath, settings =>
        settings.SetConfiguration(configuration)
          .SetVerbosity(Verbosity.Minimal)
          .WithProperty("OutputPath", "\"" + buildBinDir + "\"")
          .WithProperty("DeployOnBuild", "True")
          .WithProperty("AutoParameterizationWebConfigConnectionStrings", "false")
          .WithProperty("DeployIISAppPath", "\"Default Web Site/\""));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
   NUnit3(buildBinDir +  "/*-test.dll", new NUnit3Settings {
        Results = new[] { new NUnit3Result { FileName = "TestResults.nunit.xml" } }
      });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
