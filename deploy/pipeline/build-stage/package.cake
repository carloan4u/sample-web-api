#addin "Cake.AWS.S3"

Environment.CurrentDirectory = Directory("../../../");
var target = Argument("target", "Default");

var qaConfig = "/set-parameters-qa.xml";
var prodConfig = "/set-parameters-prod.xml";

var buildDir = MakeAbsolute(Directory("build"));
var buildBinDir = buildDir + Directory("/bin");
var packageDir = buildBinDir + Directory("/_PublishedWebsites/sample-web-api_Package");
var configDir = MakeAbsolute(Directory("deploy/config"));

CreateDirectory(buildDir + Directory("/temp"));

var tempDir = buildDir + Directory("/temp");
var solutionPath = "sample-web-api.sln";

Task("Copy-Package")
    .Does(() =>
{
CopyFile(packageDir + File("/sample-web-api.zip"), tempDir + "/sample-web-api.zip");
});

Task("Copy-Config")
    .IsDependentOn("Copy-Package")
    .Does(() => 
    {
        CopyFile(configDir + File(qaConfig), tempDir + File(qaConfig));
        CopyFile(configDir + File(prodConfig), tempDir + File(prodConfig));
    });

Task("Zip")
    .IsDependentOn("Copy-Config")
    .Does(() => 
    {
        Zip(tempDir, tempDir + File("/mjordan-api-package4.zip"));
    });    

Task("Upload")
    .IsDependentOn("Zip")
    .Does(() => 
    {
        var uploadSettings = Context.CreateUploadSettings();
        uploadSettings.BucketName = "zuto-aws-workshop-build-artifacts";
        uploadSettings.Region = RegionEndpoint.EUWest2;

        S3Upload(tempDir + File("/mjordan-api-package4.zip"), "/mjordan-api-package4.zip", uploadSettings);
    });

Task("Default")
    .IsDependentOn("Upload");


RunTarget(target);

