#addin "Cake.AWS.S3"

Environment.CurrentDirectory = Directory("../../../");
var target = Argument("target", "Default");

var UploadSettings = Context.CreateUploadSettings();
UploadSettings.Region = RegionEndpoint.EUWest2;
UploadSettings.BucketName = "zuto-aws-workshop-build-artifacts";

Task("Copy-and-create")
    .Does(() =>
    {
        CreateDirectory("packaging");
        CopyDirectory("./deploy/config", "./packaging");
        CopyFiles("./build/bin/_PublishedWebsites/sample-web-api_Package/sample-web-api.zip", "./packaging");
    });

Task("Zip")
    .IsDependentOn("Copy-and-create")
    .Does(() =>
    {
        Zip("./packaging", "./rods-packaging.zip");
    });

Task("Upload-File")
    .IsDependentOn("Zip")
    .Does(() =>
{
    S3Upload("./rods-packaging.zip", "rods-packaging.zip", UploadSettings);
});

Task("Default")
    .IsDependentOn("Upload-File");

RunTarget(target);