var target = Argument("target","Default");
Environment.CurrentDirectory = Directory("../../../");

Task("Restore-NuGet-Packages").Does(()=>{
   NuGetRestore("sample-web-api")
; });

Task("Default").IsDependentOn("Restore-NuGet-Packages");

RunTarget(target);
