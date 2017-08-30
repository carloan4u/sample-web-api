#addin "Cake.AWS.S3"

var target = Argument("target", "Default");
Environment.CurrentDirectory = Directory("../../../");

Task("Default")
    .IsDependentOn("Upload zip to S3");

Task("Zip Artifacts")
  .Does(() => {
    try
    {
      DeleteDirectory("build/output");
    }
    catch(Exception ex){
    }
    CreateDirectory("build/output");
    CopyFiles(GetFiles("build/bin/_PublishedWebsites/sample-web-api_Package/*.zip"), "build/output");    
    CopyFiles(GetFiles("deploy/config/*.xml"), "build/output");
    Zip("build/output", "build/Output.zip", GetFiles("build/output/*"));
});

Task("Upload zip to S3")
    .IsDependentOn("Zip Artifacts")
    .Description("Upload artifact to S3")
    .Does(() =>
{
    var settings = Context.CreateUploadSettings();
    settings.Region = RegionEndpoint.EUWest2;
    settings.BucketName = "zuto-aws-workshop-build-artifacts";
    settings.CannedACL = S3CannedACL.Private;
    S3Upload("build/output.zip", "craig-sample-web-api-workshop.zip", settings);
});

RunTarget(target);