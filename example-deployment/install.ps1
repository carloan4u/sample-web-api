$env:Path=$env:Path+";C:\Program Files\IIS\Microsoft Web Deploy V3\;";

function Get-Ec2-Tag($tagKey) {
    $instanceId = (Invoke-RestMethod -Method Get -Uri http://169.254.169.254/latest/meta-data/instance-id)
    $key = Get-EC2Tag | ` Where-Object {$_.ResourceId -eq $instanceId -and $_.Key -eq $tagKey }

    return $key.Value
}

$environment = Get-Ec2-Tag("Environment")
$buildPackagePath = "$PSScriptRoot/sample-website-api.zip"
$parametersFilePath = "$PSScriptRoot/config/set-parameters-$environment.xml"

& Msdeploy.exe -verb:sync -source:package=$buildPackagePath -dest:auto -setParamFile:$parametersFilePath
& C:\Windows\System32\iisreset.exe /start

exit $lastExitCode;
