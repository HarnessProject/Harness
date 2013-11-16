filter Initialize-Null {
	param(
		[PSObject]$NullValue,
		[Parameter(ValueFromPipeline=$true)]
		[PSObject]$Value
	)
	if ($Value -ne $null) {
		$Value`
	}
	else {
		$NullValue
	}
}

Set-Alias ?? Initialize-Null -Scope Global

function New-PSObject {
    [CmdletBinding()]
    param(
        [HashTable]$Properties = @{},
        [HashTable]$Methods = @{}
    )

    $o = New-Object PSObject -Property $Properties

    $Methods.Keys | %{
        $o | Add-Member -MemberType ScriptMethod -Name $_ -Value $Methods[$_]
    }
    
    return $o
}

function New-ProjectShell {
	[CmdletBinding()]
	param(
		[HashTable]$Project
	)
	
	$Project.Host = @{
		Version = [String]::Join(".",$Project.Version)
		ProjectVersion = [String]::Join(".",$Project.ProjectVersion)
	}
	
	#Stop-Pipeline 
	
	$s = New-PSObject -Properties $Project -Methods @{
		WriteBanner = {
			Write-Host "$($Shell.ProjectName) Shell "
			Write-Host "Version $($Shell.Host.Version), Project Version $($Shell.Host.ProjectVersion)"
			Write-Host "(c) 2013 $($Shell.TeamName)"
			Write-Host ""
			Write-Host "For help type HELP, Help, help or ?"
		};
	}
	
	$s;
}

function New-Packages {
	param(
		[System.IO.DirectoryInfo]$Destination,
		[System.IO.DirectoryInfo]$Source
	)
	$returnPath = $PWD
	$dir = $Shell.BaseDir | ?? $(Get-Item .\)
	
	$packagesFolder = "$($Destination.FullName | ?? $dir.FullName)\Packages" 
	$sourceFolder = "$($Source.FullName | ?? $dir.FullName)\Source"
	
	[System.IO.Directory]::GetDirectories($sourceFolder) | %{
		cd $_
		$nuspecCount = $(dir *.nuspec | Measure-Object).Count
		if ($nuspecCount -gt 0 ) { dir *.csproj | % { nuget pack "$($_.FullName)" -IncludeReferencedProjects } }
		move *.nupkg $packagesFolder -force -erroraction silentlycontinue
	}
	
	cd $returnPath
}
function New-Package {
	param(
		[string]$Package,
		[System.IO.DirectoryInfo]$Destination,
		[System.IO.DirectoryInfo]$Source
	)
	$returnPath = $PWD
	$dir = $Shell.BaseDir | ?? $(Get-Item .\)
	
	$packagesFolder = "$($Destination.FullName | ?? $dir.FullName)\Packages" 
	$sourceFolder = "$($Source.FullName | ?? $dir.FullName)\Source"
		
	cd $sourceFolder
	$nuspec = dir *.csproj -Recurse | % { 
		$f = $_
		[System.IO.Path]::GetFileNameWithoutExtension($f.FullName) | % { if ($_ -eq $Package) { $f } }
	} | take 1
	
	$nuspecCount = $( $nuspec | Measure-Object).Count
	
	if ($nuspecCount -gt 0 ) { 
		"$nuspec" | Out-Host
		$nuspec | % { 
			$f = [System.IO.Path]::GetFileNameWithoutExtension($_.FullName); 
			nuget pack "$($_.FullName)" -IncludeReferencedProjects 
		} 
		move *.nupkg $packagesFolder -force -erroraction silentlycontinue
	}
	
	cd $returnPath
}
function Set-ClipboardText {
        
        param([parameter(valuefrompipeline=$true)]$text)
 
        # need to use temp file to avoid exceeding command-line length limit
        #$temp = [io.path]::GetTempFileName()
 
        try {
            #set-content -Path $temp -Value $text
 
            $command = {
                    add-type -an system.windows.forms
                    [System.Windows.Forms.Clipboard]::SetText($text)
            }
            &$command 
            #powershell -sta -noprofile -command $command -args $temp
 
        } finally {
            #if ((test-path $temp)) {
               # remove-item $temp
            #}
        }
}
 
function Get-ClipboardText {
        $command = {
                add-type -an system.windows.forms
                [System.Windows.Forms.Clipboard]::GetText()
        }
        &$command
        #powershell -sta -noprofile -command $command
}

function Get-ClipboardData{
    param(
        [string]$format,
        [switch]$GetFormats`
    )
    if ($GetFormats) {
        [System.Windows.Forms.Clipboard]::GetDataObject().GetFormats();
        return;
    }
    $clipdata = [System.Windows.Forms.Clipboard]::GetDataObject()

    $data = $clipdata.GetData($format);

    $([System.IO.TextReader](new-object System.IO.StreamReader $data)).ReadToEnd()
}