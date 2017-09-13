(* -- Fake Dependencies paket-inline
source https://nuget.org/api/v2

nuget Fake.Core.Targets prerelease
-- Fake Dependencies -- *)
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core.Targets

Target.Create "MyBuild" (fun _ ->
    printfn "MyBuild"
)

Target.RunOrDefault "MyBuild"
