#tool "nuget:?package=NUnit.ConsoleRunner"
var target = Argument("target", "Default");
Environment.CurrentDirectory = Directory("../../../");

Task("Restore-NuGet-Packages")
.Does(() =>
{
NuGetRestore("sample-web-api.sln");
});

Task("Default")
    .IsDependentOn("NUnit3");


Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() => 
    {
        MSBuild("sample-web-api.sln");
    });

Task("NUnit3")
    .IsDependentOn("Build")
    .Does(() =>
    {
        NUnit3("test/sample-web-api-test/bin/debug/sample-web-api-test.dll");
    });    

RunTarget(target);