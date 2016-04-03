function Find-Package{
	# Generic helper function to locate a package doing the listing | sort | select last approach
	[CmdletBinding()]
	param(
		[Parameter(Position=0, Mandatory=1)]$packagesPath,
		[Parameter(Position=1, Mandatory=1)]$packageName
	)
	Write-Host "pkgpath:" $packagesPath
	Write-Host "pkgname:" $packageName
	Write-Host "Find-Pkg:" (Get-ChildItem ($packagesPath + "\" + $packageName + ".*")).FullName `
		| Sort-Object $_ | select  -last 1

	return (Get-ChildItem ($packagesPath + "\" + $packageName + ".*")).FullName `
		| Sort-Object $_ | select  -last 1
}