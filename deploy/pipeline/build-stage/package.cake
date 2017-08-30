#addin "Cake.AWS.S3"

Environment.CurrentDirectory = Directory("../../../");

var target = Argument("target", "Default");

Task("Create-Output-Directory").Does(() => 
{
    CreateDirectory("./packaging");
});

Task("Copy-Artifacts")
.IsDependentOn("Create-Output-Directory")
.Does(() => 
{
    CopyFiles("./build/bin/_PublishedWebsites/sample-web-api_Package/sample-web-api.zip", "./packaging");
    CopyDirectory("./deploy/config", "./packaging");
});

Task("Package")
.IsDependentOn("Copy-Artifacts")
.Does(() => 
{
    Zip("./packaging", "./packaging.zip");
});

Task("Upload-To-S3")
.IsDependentOn("Package")
.Does(() => 
{
    var settings = Context.CreateUploadSettings();
    settings.Region = RegionEndpoint.EUWest2;
    settings.BucketName = "zuto-aws-workshop-build-artifacts";
    S3Upload("./packaging.zip", "rwolff-packaging.zip", settings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Upload-To-S3");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);