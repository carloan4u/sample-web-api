$env:Path=$env:Path+";C:\Program Files\IIS\Microsoft Web Deploy V3\;";

& C:\Windows\System32\iisreset.exe /stop
& Msdeploy.exe -verb:delete -dest:iisApp=`'Default Web Site/`'

exit $lastExitCode;
