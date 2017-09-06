#addin "Cake.AWS.S3"
#addin "MagicChunks"

Environment.CurrentDirectory = Directory("../../../");

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var webProjectName = "sample-web-api";
var appName = EnvironmentVariable("app_name") ?? webProjectName;
var goPipelineLabel = EnvironmentVariable("GO_PIPELINE_LABEL") ?? "UNKNOWN";

var buildDir = MakeAbsolute(Directory("build"));
var webProjectBuildPath = string.Format("{0}/bin/_PublishedWebsites/{1}_Package/{1}.zip", buildDir, webProjectName);
var packagingDir = string.Format("{0}/{1}", buildDir, "packaging");
var s3Path = appName + "/" + goPipelineLabel;
var applicationPackageName = string.Format("{0}_{1}.zip", appName, goPipelineLabel);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Packaging-Preparation-Copy")
    .Does(() =>
{
    CreateDirectory(packagingDir);
    CopyFile(webProjectBuildPath, packagingDir + "/build.zip");
    CopyDirectory("../scripts/deploy-scripts/.ebextensions", packagingDir + "/.ebextensions");
    CopyDirectory("../scripts/deploy-scripts/eb-web-app-install-scripts", packagingDir);
    CopyDirectory("./deploy/config/", packagingDir + "/config");
});

Task("Create-Deployment-Package")
    .IsDependentOn("Packaging-Preparation-Copy")
    .Does(() =>
{
    Zip(packagingDir + "/", buildDir + "/" + applicationPackageName);
});

Task("Create-Deployment-Scripts-Package")
    .Does(() =>
{
    Zip("deploy/terraform", buildDir + "/terraform.zip");
});

var s3UploadSettings = Context.CreateUploadSettings();
s3UploadSettings.Region = RegionEndpoint.EUWest2;
s3UploadSettings.BucketName = "zuto-build-artifacts";

Task("Upload-Deployment-Package")
    .IsDependentOn("Create-Deployment-Package")
    .Does(() =>
{
    S3Upload(buildDir + "/" + applicationPackageName, s3Path + "/" + applicationPackageName, s3UploadSettings);
});

Task("Upload-Deployment-Scripts")
    .IsDependentOn("Create-Deployment-Scripts-Package")
    .Does(() =>
{
    S3Upload(buildDir + "/terraform.zip", s3Path + "/terraform.zip", s3UploadSettings);
});

Task("TransformQaConfig")
    .Does(() =>
{
    var whateveryouwantsecure = EnvironmentVariable("whateveryouwantsecure_qa") ?? "UNKNOWN";
    TransformConfig(@"./deploy/config/set-parameters-qa.xml", new TransformationCollection {  
       { "parameters/setParameter[@name='whateveryouwantsecure']/@value", whateveryouwantsecure }
    });
});

Task("TransformProdConfig")
    .Does(() =>
{
    TransformConfig(@"./deploy/config/set-parameters-prod.xml", new TransformationCollection {  });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("TransformQaConfig")
    .IsDependentOn("TransformProdConfig")
    .IsDependentOn("Upload-Deployment-Package")
    .IsDependentOn("Upload-Deployment-Scripts");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);