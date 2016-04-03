Include ".\Modules\helpers.ps1"

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

		$testResultsDirectory = "$outputDirectory\TestResults"

		$publishedNUnitTestsDirectory = "$tempDirectory\_PublishedNUnitTests"
		$NUnitTestResultsDirectory = "$testResultsDirectory\NUnit"

		# Install-Package NUnit.Runners // Add this pkg to the Build project. This makes the Runner avl
		# Setup the path to the NUnit runner

		#	$NUnitExe = ((Get-ChildItem ("$solutionDirectory\packages\NUnit.Runners*")).FullName `
		#	| Sort-Object $_ | select  -last 1) + "Tools\nunit-console-x86.exe"

		# Setup the path to the NUnit runner using my helper.ps1
		$packagesPath = "$solutionDirectory\packages"
		$NUnitExe = (Find-Package $packagesPath "NUnit.ConsoleRunner") + "\Tools\nunit3-console.exe" # "nunit-console-x86.exe"

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
			 
		Assert (Test-Path $NUnitExe) "NUnit console could not be found: $NUnitExe"

		if(Test-Path $outputDirectory){
			Write-Host "Removing output dir at: $outputDirectory"
			Remove-Item $outputDirectory -Force -Recurse
		}
		if(Test-Path $tempDirectory){
			Write-Host "Removing output dir at: $tempDirectory"
			Remove-Item $tempDirectory -Force -Recurse
		}

		if(!(Test-Path $outputDirectory)){
			Write-Host "Creating output dir at: $outputDirectory"
			New-Item $outputDirectory -ItemType Directory | Out-Null  # Note- to echo off redir to Out-Null
		}
		if(!(Test-Path $tempDirectory)){
			Write-Host "Creating temp dir at: $tempDirectory"
			New-Item $tempDirectory -ItemType Directory | Out-Null
		}
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

	# ========== Testing Tasks ===========
	# Precondition test that the dir exists, only then run the NUnit tests..
	task TestNUnit -depends Compile -description "Run NUnit Tests" `
		-precondition { return Test-Path $publishedNUnitTestsDirectory } `
	{
		$projects = Get-ChildItem $publishedNUnitTestsDirectory

		if($projects.Count -eq 1)
		{
			Write-Host "1 NUnit project has been found"
		}
		else
		{
			Write-Host $projects.Count " NUnit projects have been found"
		}
		Write-Host ($projects | Select $_.Name)

		if(!(Test-Path $NUnitTestResultsDirectory)){
			Write-Host "Creating NUnit Test Results dir: $NUnitTestResultsDirectory"
			mkdir $NUnitTestResultsDirectory | Out-Null 
		}

		# Get list of Test Dlls
		$testAssemblies = $projects | ForEach-Object {$_.FullName + "\bin\" + $_.Name + ".dll"}

		# Build a space delimited list of dlls
		$testAssembliesParameters = [string]::Join(" ", $testAssemblies)

		Write-Host "testassmeplies:" $testAssembliesParameters 

		# Fireup the Runner ( noshadow will disable copying and locking of dlls)
		# No longer used in new NUnit -> Exec { &$NUnitExe $testAssembliesParameters /xml:$NUnitTestResultsDirectory\NUnit.xml /nologo /noshadow }
		
		# New syntax for Nunit3-console (as per Nunit3.2.0), --work option stores results in specified dir.. --noheader suppresses startup logo.. etc
		Exec { &$NUnitExe $testAssembliesParameters --noheader --work=$NUnitTestResultsDirectory }
	}
	task TestMSTests -depends Compile -description "Run MSTest Tests"
	{

	}
	# ========== End Testing Tasks ===========



	# Goes to Compile first as it is listed first in -depends
	# but Executes Clean as Compile depends on Clean
	# then executes Compile
	# then executes SomeTask..
	task Test -depends Compile, TestNUnit, TestMSTests `
		-description "Run Unit tests"{
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


	
