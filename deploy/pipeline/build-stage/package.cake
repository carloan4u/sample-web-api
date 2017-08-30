#addin "Cake.AWS.S3"

var projectZipLocation = @"C:\Code\sample-web-api\build\bin\_PublishedWebsites\sample-web-api_Package\";

var projectZipFile = "sample-web-api.zip";

var configs = @"C:\Code\sample-web-api\deploy\config";

var qaConfig = @"C:\Code\sample-web-api\deploy\config\set-parameters-qa.xml";
var prodConfig = @"C:\Code\sample-web-api\deploy\config\set-parameters-prod.xml";

var outputFolder = @"C:\Code\sample-web-api\deploy\all-the-things\";

var target = Argument("target", "Default");

Task("CreateDirectory")
    .Does(() =>
{
    CreateDirectory(outputFolder);
});

Task("CopyFiles")
    .IsDependentOn("CreateDirectory")
    .Does(() =>
{
    CopyFile(projectZipLocation + projectZipFile, outputFolder + projectZipFile);

    var files = GetFiles(configs + "/*.xml");
    CopyFiles(files, outputFolder);
});

Task("ZipFiles")
    .IsDependentOn("CopyFiles")
    .Does(() =>
{
    Zip(outputFolder, outputFolder + "packaging.zip");
});

Task("PushToS3")
    .IsDependentOn("ZipFiles")
    .Does(() =>
{
    var uploadSettings = Context.CreateUploadSettings();
    uploadSettings.BucketName = "zuto-aws-workshop-build-artifacts";
    uploadSettings.Region = RegionEndpoint.EUWest2;

    S3Upload(outputFolder + "packaging.zip", "liams-package.zip", uploadSettings);
});


Task("CleanUp")
    .IsDependentOn("PushToS3")
    .Does(() =>
{
    DeleteDirectory(outputFolder, recursive:true);
});


Task("Default")
    .IsDependentOn("CleanUp");


RunTarget(target);