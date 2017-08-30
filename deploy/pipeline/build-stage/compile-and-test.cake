#tool "nuget:?package=NUnit.ConsoleRunner"

var target = Argument("target", "Default");
var path = "sample-web-api.sln";
var build = "build";
var test = "Test";
Environment.CurrentDirectory = Directory("../../../");


Task("Restore-NuGet-Packages")
   .Does(() =>
{
   NuGetRestore(path);
});

Task(build)
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        MSBuild(path);
    });

Task(test)
    .IsDependentOn(build)
    .Does(() =>
    {
        NUnit3("test/sample-web-api-test/bin/Debug/sample-web-api-test.dll");
    });

Task("Default")
    .IsDependentOn(test);

RunTarget(target);