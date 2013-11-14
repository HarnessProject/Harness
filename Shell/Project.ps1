<#
	Project Shell
	
	Everything in the shell is altered elsewhere...
	Except Environment Variables which you change here...
	SO DON'T CHANGE THIS UNLESS YOU ARE SURE THATS WHAT YOU WANT TO DO...
#>
function halt-pipeline { 
    param(
        [Parameter(ValueFromPipeline=$true)]
        [PSObject]$Context,
        [Switch]$Enable
    ) 
    function all-stop { <# Pipeline Halted #> }
    $Context | out-host
    
    if ($global:PSHalt -ne $null) { 
        all-stop
        Remove-PSBreakpoint -Breakpoint $global:PSHalt
        $global:PSHalt = $null;

    }
    if ($global:PSHalt -eq $null) {
        $global:PSHalt = Set-PSBreakPoint -Command all-stop
    }
    
}

function get-data (
	[string]$Provider,
	[string]$ConnectionString,
	[string]$Query
) 
{
	$factory = [System.Data.Common.DbProviderFactories]::GetFactory($provider); #obtain the provider factory
	
	$connection = $factory.CreateConnection(); #get a connection object
	$connection.ConnectionString = $ConnectionString; #set connection string
	$connection.Open(); #Open database connection
	
	$command = $connection.CreateCommand(); #create command object
	$command.CommandText = $Query; #set query
	
	$adapter = $factory.CreateDataAdapter(); #ready dataadapter
	$adapter.SelectCommand = $command; #set command
	$dataset = New-Object System.Data.DataSet; #createa an empty dataset
	
	[void]$adapter.Fill($dataset); #fill the dataset
	$connection.Close();#close the data connection
	
	return $dataset;#return the dataset.
};

halt-pipeline -Enable
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


function Capture-Matches { 
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=$true)]
        $Expression,
        [Parameter(Position=1, Mandatory=$true, ValueFromPipeline=$true)] 
        $Text
    ) 
    $([regex]$Expression).Matches($Text) | % { 
        $_.Captures | % { $_.Value } 
    }
}
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
function Where-True {
    param([ScriptBlock]$Expression, [Parameter(ValueFromPipeline=$true)][PSObject]$InputObject)
    
    Process {
        $result = $false
        #[Bool]::Parse(
        $result = $($InputObject | &$Expression)
        if ($result) {
            $InputObject
        }
    }
    
}

function When-True {
	param([ScriptBlock]$Expression, [ScriptBlock]$TriggeredExpression,[Parameter(ValueFromPipeline=$true)][PSObject]$InputObject,[switch]$PassThru)
	Process {
		$result = $false
        #[Bool]::Parse(
        $result = $($InputObject | &$Expression)
        if ($result) {
			$InputObject | &$TriggeredExpression | Out-Null
			if ($PassThru) { $InputObject }
        }
	}
}
Set-Alias ?? Initialize-Null -Scope Global

set-alias ?: Invoke-Ternary -Option AllScope -Description "Ternary Filter"
filter Invoke-Ternary ([scriptblock]$decider, [scriptblock]$ifTrue, [scriptblock]$ifFalse)
{
   if (&$decider) {
      &$ifTrue
   } else {
      &$ifFalse
   }
}

$project = @{
	Version = @(0,0,0,1)
	ProjectName = "Harness"
	ProjectVersion = @(1,0,0,0)
	TeamName = "The Harness Project"
	BuildMenu = $true
	BaseDir = get-item .\
}

