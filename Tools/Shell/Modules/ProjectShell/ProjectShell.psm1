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
