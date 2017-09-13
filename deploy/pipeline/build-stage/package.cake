#addin "Cake.AWS.S3"

Environment.CurrentDirectory = Directory("../../../");

var target = Argument("target", "Default");

Task("Default")
    .IsDependentOn("AWS-Upload");

Task("Create-Directory")
    .Does(()=>
    {
        CreateDirectory("Packaging");
    });

Task("Copy-WebSite-Package")
    .IsDependentOn("Create-Directory")
    .Does(()=>
    {
        CopyFile("build/bin/_PublishedWebsites/sample-web-api_Package/sample-web-api.zip", "Packaging/sample-web-api.zip");
    });

Task("Copy-Configs")
    .IsDependentOn("Copy-WebSite-Package")
    .Does(()=>
    {
        CopyDirectory("deploy/config", "Packaging/config");
    });

Task("Zip-Package")
    .IsDependentOn("Copy-Configs")
    .Does(()=>
    {
        Zip("Packaging", "robroe.zip");
    });

Task("AWS-Upload")
    .IsDependentOn("Zip-Package")
    .Does(()=>
        {
           var context = Context.CreateUploadSettings();
            context.Region = RegionEndpoint.EUWest2;
            context.BucketName = "zuto-aws-workshop-build-artifacts"; 

            S3Upload("robroe.zip", "robroe.zip", context);
        }
    );

RunTarget(target);