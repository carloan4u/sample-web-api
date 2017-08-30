#tool "nuget:?package=NUnit.ConsoleRunner"

var target = Argument("target", "Default");
Environment.CurrentDirectory = Directory("../../../");
string solutionName = "sample-web-api.sln";

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        var testAssembly = "test/sample-web-api-test/bin/Debug/sample-web-api-test.dll";
        NUnit3(testAssembly);
    });    


Task("Restore-NuGet-Packages")
   .Does(() =>
{
   NuGetRestore(solutionName);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        MSBuild(solutionName);
    });

Task("Default")
  .IsDependentOn("Test");

RunTarget(target);