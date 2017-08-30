#tool "nuget:?package=NUnit.ConsoleRunner"

var target = Argument("target", "Default");
Environment.CurrentDirectory = Directory("../../../");
var solutionName = "sample-web-api.sln";

Task("Restore-NuGet-Packages")
   .Does(() =>
{
   NuGetRestore(solutionName);
});

Task("Build")
  .IsDependentOn("Restore-NuGet-Packages")
  .Does(() => {
    MSBuild(solutionName);
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    NUnit3("test/sample-web-api-test/bin/debug/sample-web-api-test.dll");
  });


Task("Default")
  .IsDependentOn("Test");

RunTarget(target);