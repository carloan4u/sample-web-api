#tool "nuget:?package=NUnit.ConsoleRunner"

var target = Argument("target", "Default");
Environment.CurrentDirectory = Directory("../../../");

const string restoreNugetTask = "Restore-NuGet-Packages";
const string buildTask = "Build";
const string testTask = "Test";
const string path = "sample-web-api.sln";

Task(restoreNugetTask)
   .Does(() =>
{
   NuGetRestore(path);
});

Task(buildTask)
  .IsDependentOn(restoreNugetTask)
  .Does(()=> 
{
  MSBuild(path);
});

Task(testTask)
  .IsDependentOn(buildTask)
  .Does(()=> 
{
  NUnit3("test/sample-web-api-test/bin/Debug/sample-web-api-test.dll");
});

Task("Default")
  .IsDependentOn(testTask);
RunTarget(target);