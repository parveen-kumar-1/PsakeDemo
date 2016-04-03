	# a comment in PS 
	# task default -depends Test 	// Entry point for Psake execution (no body {} ).. (tells CI pipeline to run Test task starting with all its dependencies..)

	# properties are a good mechanism to share data beween all tasks in this script and passing in params
	properties{
		$cleanMessage = 'Executed Clean!'
		$compileMessage = "Building solution $solutionFile" #'Executed Compile'
		$testMessage = 'Executed Test!'
		$solutionDirectory = (Get-Item $solutionFile).DirectoryName
		$outputDirectory = "$solutionDirectory\.build"    #	"..\.build"
		$tempDirectory = "$solutionDirectory\temp"    
		$buildConfiguration = "Release"
		$buildPlatform = "Any CPU"
	}

	FormatTaskName "`r`n`r`n----- Executing {0} Task -----"

	task Init `
		-description "Initialization of build by removing prev build artifacts and creating output dirs" `
		-requiredVariables outputDirectory, tempDirectory { 		# validating the "needed vars" are provided

		# Validate possible values to be correct for BuildConfig and BuildPlatform
		# Powershell is NON-Case sensitive lowercase is same as SentenceCase
		Assert -conditionToCheck ("Debug", "Release" -contains $buildConfiguration) `
				-failureMessage	"Invalid build configuration =$BuildConfiguration, Valid values = Debug, Release"

		Assert -conditionToCheck ("x86", "x64", "Any CPU" -contains $buildPlatform) `
					-failureMessage "Invalid build configuration =$buildPlatform, Valid values = x86, x64, Any CPU"
			  
		if(Test-Path $outputDirectory){
			Write-Host "Removing output dir at: $outputDirectory"
			Remove-Item $outputDirectory -Force -Recurse

			Write-Host "Removing output dir at: $tempDirectory"
			Remove-Item $tempDirectory -Force -Recurse
		}
		Write-Host "Creating output dir at: $outputDirectory"
		New-Item $outputDirectory -ItemType Directory | Out-Null  # Note- to echo off redir to Out-Null

		Write-Host "Creating temp dir at: $tempDirectory"
		New-Item $tempDirectory -ItemType Directory | Out-Null
	}

	task Clean -description "Remove temporary files" {
		Write-Host $cleanMessage
	}

	task Compile -depends Init -description "Compile the code" `
		-requiredVariables solutionFile, buildConfiguration, buildPlatform, tempDirectory {
		Write-Host $compileMessage

		# Note Exe return codes can be used to see if build fails (0 = success, >0 = failure, e.g. 1)
		# Tip: Wrap exe calls in Exec { .. }, which fails the task if exe returns non-zero
		Exec{
			msbuild $solutionFile "/p:Configuration=$buildConfiguration;Platform=$buildPlatform;OutDir=$tempDirectory"
		}	
	}
	# Goes to Compile first as it is listed first in -depends
	# but Executes Clean as Compile depends on Clean
	# then executes Compile
	# then executes SomeTask..
	task Test -depends Compile, Clean, SomeTask -description "Run Unit tests"{
		Write-Host $testMessage
	}
	
    task SomeTask -description "Post compile tasks" {
		Write-Host 'Executed SomeTask!'
	}

	# Document your tasks with -description. Now "Invoke-psake -docs" will show you what your script does.. see:
	# Invoke-psake -buildFile .\default.ps1 -TaskList Test -docs
	
	<# Output:
	Name     Alias Depends On               Default Description           
	----     ----- ----------               ------- -----------           
	Clean                                           Remove temporary files
	Compile        Clean                            Compile the code      
	SomeTask                                        Post compile tasks    
	Test           Compile, Clean, SomeTask         Run Unit tests        
	#>


	
