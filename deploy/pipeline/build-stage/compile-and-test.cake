#tool "nuget:?package=NUnit.ConsoleRunner"

var target = Argument("target", "Default");

Environment.CurrentDirectory = Directory("../../../");

Task("Restore-NuGet-Packages")
.Does(() =>
{
NuGetRestore("sample-web-api.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(()=> MSBuild("sample-web-api.sln"));

Task("Test")
.IsDependentOn("Build")
.Does(()=>{
var testAssemblies = GetFiles("test/sample-web-api-test/bin/Debug/sample-web-api-test.dll");
NUnit3(testAssemblies);
});


Task("Default")
.IsDependentOn("Test");

RunTarget(target);