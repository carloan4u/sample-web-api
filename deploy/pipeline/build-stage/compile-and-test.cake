#tool "nuget:?package=NUnit.ConsoleRunner"

var target = Argument("target", "Default");
var testAssemblies = "test/sample-web-api-test/bin/Debug/sample-web-api-test.dll";
Environment.CurrentDirectory = Directory("../../../");

Task("Restore-NuGet-Packages")
    .Does(() => {
        NuGetRestore("sample-web-api.sln");
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() => {
        MSBuild("sample-web-api.sln");
    });

Task("test")
    .IsDependentOn("Build")
    .Does(() => {
        NUnit3(testAssemblies);
    });

Task("Default")
    .IsDependentOn("test");

RunTarget(target);