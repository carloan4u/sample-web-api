var target = Argument("target", "Default");

Environment.CurrentDirectory = Directory("../../../");

Task("Restore-NuGet-Packages")
  .Does(() =>
{
  NuGetRestore("sample-web-api.sln");
});

Task("Default")
  .IsDependentOn("Restore-NuGet-Packages");

RunTarget(target);