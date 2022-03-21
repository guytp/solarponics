Write-Output "Deploying version $version"
$version = (Get-Date).ToString("yyyyMMddHHmmss")

Write-Output "Getting existing updater script content"
$depsContent = Get-Content "P:\Solarponics.ProductionManager.deps.json"
$runClientContent = Get-Content "P:\runClient.bat"
$updateSelfContent = Get-Content "P:\updateSelf.bat"

Write-Output "Deleting current folder contents"
Get-ChildItem -Path P:\ -Include *.* -File -Recurse | foreach { $_.Delete()}

$myPath = $MyInvocation.MyCommand.Path
$myPath = Split-Path $myPath -Parent
$path = Join-Path -Path $myPath -ChildPath "ProductionManager\bin\Debug\netcoreapp3.1"

Write-Output "Removing local logs"
Remove-Item ($path + "\*.log")

Write-Output "Copying latest release from $path"
Copy-Item ($path + "\*") -Destination "P:\" -Recurse

Write-Output "Updating config"
$appSettingsContent = Get-Content "P:\appsettings.json"
for ($i = 0; $i -lt $appSettingsContent.Length; $i++)
{
	$line = $appSettingsContent[$i]
	$line = $line.Replace("localhost", "10.200.0.33")
	$appSettingsContent[$i] = $line
}
Set-Content "P:\appsettings.json" $appSettingsContent

Write-Output "Creating updater, version and deps.json"
Set-Content "P:\Solarponics.ProductionManager.deps.json" $depsContent
Set-Content "P:\runClient.bat" $runClientContent
Set-Content "P:\updateSelf.bat" $updateSelfContent
Set-Content "P:\version.txt" $version
