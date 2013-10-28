filter Invoke-Ternary ([scriptblock]$decider, [scriptblock]$ifTrue, [scriptblock]$ifFalse)
{
   if (&$decider) {
      &$ifTrue
   } else {
      &$ifFalse
   }
}



function New-List {
    [CmdletBinding()]
    param(
        [Parameter(ValueFromPipeline=$true)]
        [PSObject]$InputObject,
        [Parameter(Position=0,Mandatory=$false)]
        [PSObject[][]]$Items
    )
    BEGIN {
        $list = @()
        ?: { $Items -ne $null } { $Items | % { $list.AddRange($_) } } {} 
    }
    PROCESS {
        $list += $InputObject | out-null
    }
    END {
        return [System.Collections.IList]$list
    }
}




function Limit-Objects {
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=$true)]
        [int]$Number,
        [Parameter(ValueFromPipeline=$true)]
        [PSObject]$InputObject,
        [Parameter(Position=1,Mandatory=$false)]
        [PSObject[][]]$Items
    )
    BEGIN {
        $list = $Items 
        ?: {$Items -ne $null} {
                $items | % {
                    $i = $_
                    ?: { $c -gt 0 } { $i } {}
                    $c--;
                }
            } {}
        $c = $number
    }
    PROCESS {
        ?: { $c -gt 0 } { $InputObject } {}
        $c--;
    }
    END {
        
    }
    
    
}
function Skip-Objects {
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=$true)]
        [int]$Number,
        [Parameter(ValueFromPipeline=$true)]
        [PSObject]$InputObject,
        [Parameter(Position=1,Mandatory=$false)]
        [PSObject[][]]$Items
    )
    BEGIN {
        $list = $Items 
        ?: {$Items -ne $null} {
                $items | % {
                    $i = $_
                    ?: { $c -gt 0 } {  } {$i}
                    $c--;
                    
                }
            } {}
        $c = $number
    }
    PROCESS {
        ?: { $c -gt 0 } {  } {$InputObject}
        $c--;
    }
    END {
        
    }
    
    
}

function Measure-Pipeline {
	param(
		[Parameter(ValueFromPipeline=$true)]
		[PSObject]$Thru
	)
	Begin {
		$list = @()
	}
	Process {
		$list += $thru
	}
	End {
		$list | Measure-Object | out-host
		$list | % { $_ }
	}
}

function Stop-Pipeline {
	
	param(
		[ScriptBlock]$Condition,
		[Parameter(ValueFromPipeline=$true)]
		[PSObject]$Thru
	)
	Begin {
		$list = @()
	}
	Process {
		$list += $thru
	}
	End {
		<# #StartCommentStory# #>
		<#
			#CommentStory#
			OK, here is how this works...
			halt-pipeline (obviously) doesn't do anything, right?...
		#>
		<# #Footnote# Yes I am well aware halt is NOT an approved Verb)#>
		function halt-pipeline { <# Pipeline Halted #> }
		<#
			#CommentStory#
			WRONG! We're going to tell the debugger to break the current script
			every time we call it...
		#>
		Set-PSBreakpoint -Command halt-pipeline | out-null
		
		<#
			#CommentStory#
			If we don't define a break condition just break...
		#>
		if ($Condition -eq $null) { 
			halt-pipeline
			return $list
		}
		<# 
			#CommentStory#
			If we do process the list... 
		#>
		else {
			
			$list | % { 
				<#
					#BugHint##Footnote#
					#$ 
						Powershell: 4,
						OS: Windows 8.1
						VisualStudio: 2013,2012
					#
					The need to create a #Type:ScriptBlock# from a #Type:ScriptBlock#
					Makes no sense...
				#>
				$f = [ScriptBlock]::Create($Condition)
				$r = [bool]$( $_ | & { process { & $f } } )
				if ($r -eq $true) { halt-pipeline }
				return $_
			}
		}
		<# #EndCommentStory# #>
	}
}


Set-Alias take Limit-Objects -Scope Global -Description "Limit Objects"
Set-Alias skip Skip-Objects -Scope Global -Description "Skip Objects"
set-alias ?: Invoke-Ternary -Scope Global -Description "Ternary Filter"

