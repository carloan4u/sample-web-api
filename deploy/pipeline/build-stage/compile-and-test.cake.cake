#tool "nuget:?package=NUnit.ConsoleRunner"

string solution = "sample-web-api.sln";
var target = Argument("target", "Default");
Environment.CurrentDirectory = Directory("../../../");

Task("Restore-NuGet-Packages")
   .Does(() =>
{
   NuGetRestore(solution);
});

Task("Build")
  .IsDependentOn("Restore-NuGet-Packages")
  .Does(() =>
{
  MSBuild(solution);
});

Task("Test")
  .IsDependentOn("Build")
  .Does(() =>
{
  NUnit3("test/sample-web-api-test/bin/Debug/sample-web-api-test.dll");
});

Task("Default")
  .IsDependentOn("Test");

RunTarget(target);