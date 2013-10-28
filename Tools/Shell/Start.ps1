<#
	Project Shell
	Everything in the shell is altered elsewhere...
	SO DON'T CHANGE THIS UNLESS YOU ARE SURE THATS WHAT YOU WANT TO DO...
#>


#we define a custom module path...
$env:PSModulePath = ";" + $(Get-Item .\Shell\Modules).FullName
#we add our tools folder to the path...
$env:Path += $(Get-Item .\Shell\Tools).FullName + ";"
#Add all types in the assemblies directory...
Get-Item .\Shell\Assemblies\*.dll | % { Add-Type -Path $_ }

Import-Module ProjectShell -Force
Import-Module Lists -Force

. .\Shell\Project.ps1 #Our Environment

$Global:Shell = New-ProjectShell -Project $project

$Shell.WriteBanner()

#All scripts in Shell\Startup will run...
Get-Item .\Shell\Startup\*.ps1 | % { . $_.FullName }