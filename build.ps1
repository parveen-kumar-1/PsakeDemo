cls

Remove-Module [p]sake 	# unregisters any psake modules at start of our build session..

#Import-Module  ..\packages\psake.4.6.0\tools\psake.psm1  	# not future proof as there could be multiple versions of psake in this path.
	
# Fix for that: sort psake package folders by version and use latest
# Problem - this isnt working for now..
$psakeModule = (Get-ChildItem (".\packages\psake.*\tools\psake.psm1")).FullName | Sort-Object $_ | select -last 1

Write-Host 'Module:' $psakeModule # debug/trace what package was selected for import

Import-Module $psakeModule

#	Invoke-psake 	# better usage is explicit params like so, 
					# and also see passing in property arguments see e.g. "testMessage" etc.

Invoke-psake -buildFile .\Build\default.ps1 `
			-TaskList Test `
			-framework 4.5.2 `
			-properties @{
				"testMessage" = "Outside param from Build.ps1 as testMessage"
				"cleanMessage" = "build.ps1 clean msg"
				"buildConfiguration" = "release" # Validation of possible values see default.ps1 Assert -condi..
				"buildPlatform" = "Any CPU"
			} `
			-parameters @{
				"solutionFile" = "..\MVCWeb.sln" # Note- this is still w.r.t. the location of the default.ps1 script
			} `

Write-Host "Build exit code:" $LASTEXITCODE

# Propagating the exit code so that builds actually fail when there is a problem
exit $LASTEXITCODE

