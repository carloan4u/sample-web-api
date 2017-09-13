
Environment.CurrentDirectory = Directory("../../../");

var target = Argument("target", "Default");
var directoryName = "storagefors3";


// Create a new directory
Task("Create-New-Directory")
    .Does(() =>
{
    CreateDirectory(directoryName);
});


//Copy zip into it
Task("CopyZip")
    .IsDependentOn("Create-New-Directory")
    .Does(() => 
{
    CopyFileToDirectory("build/bin/_PublishedWebsites/sample-web-api_Package/sample-web-api.zip", directoryName);
});

//Copy deploy/config into it
Task("Copy-Deploy-Config")
    .IsDependentOn("CopyZip")
    .Does(() => 
{
    var files = GetFiles("./**/deploy/config/*");
    CopyFiles(files, directoryName);
});

//Zip the directory
Task("Zip-Directory")
    .IsDependentOn("Copy-Deploy-Config")
    .Does(() => 
{
    var files = GetFiles(directoryName + "/*");
    Zip("./", "cakeassemblies.zip", files);
});

//Upload to S3
//Requires AWS addin
#addin "Cake.AWS.S3"

Task("Upload-File")
    .Description("Upload a file to S3")
    .IsDependentOn("Zip-Directory")
    .Does(() =>
{
    var uploadSettings = Context.CreateUploadSettings();

    uploadSettings.BucketName = "zuto-aws-workshop-build-artifacts";
    uploadSettings.Region = RegionEndpoint.EUWest2;

    S3Upload("cakeassemblies.zip", "test123.zip", uploadSettings);
});
//Bucket; zuto-aws-workshop-build-artifacts


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Upload-File");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);