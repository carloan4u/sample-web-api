#tool "nuget:?package=NUnit.ConsoleRunner"

var target = Argument("target", "Default");
Environment.CurrentDirectory = Directory("../../../");

var solutionPath = "sample-web-api.sln";

Task("Restore-NuGet-Packages")
   .Does(() =>
{
   NuGetRestore(solutionPath);
});

Task("Default")
  .IsDependentOn("RunTests");

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() => {
        MSBuild(solutionPath);
    });

Task("RunTests")
    .IsDependentOn("Build")
    .Does(() => {
        var testAssemblyPath = "test/sample-web-api-test/bin/Debug/sample-web-api-test.dll";
        var testAssemblies = GetFiles(testAssemblyPath);
        NUnit3(testAssemblies);
    });
    
RunTarget(target);