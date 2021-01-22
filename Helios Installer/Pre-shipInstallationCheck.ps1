# ******************************************************************************************
# ******************************************************************************************
# ***                                                                                    ***
# ***  This powershell script saves away directory listings from the installation        ***
# ***  and upgrade.  It should be run before a new release of Helios is delivered.       ***
# ***                                                                                    ***
# ***                                                                                    ***
# ***  If scripts are disabled, refer to https:/go.microsoft.com/fwlink/?LinkID=135170   ***
# ***                                                                                    ***
# ***   Set-ExecutionPolicy Unrestricted  (from admin Powershell to allow this to run)   ***
# ***                                                                                    ***
# ***   Set-ExecutionPolicy Default  (from admin Powershell to return to normal)         ***
# ***                                                                                    ***
# ***                                                                                    ***
# ***                                                                                    ***
# ******************************************************************************************
# ******************************************************************************************
$HeliosRelease="Helios.1.4." + $(get-date -Format yyyy.MMdd)
write-host "Release being tested is " $HeliosRelease
write-host "First test is a clean installation"
write-host "Perform a fresh install of the new " $HeliosRelease
write-host "Press a key when complete..."
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
get-childitem ($env:ProgramFiles + "\Gadrocs Workshop\Helios\") -Recurse | format-list >($home + "\" + $HeliosRelease + ".Install.Footprint.txt") -property name,length,Creationtime -groupby directory
# Second Test is following an upgrade installation from an earlier Helios release
write-host "Perform the following: "
write-host
write-host " 1) Uninstall Helios"
write-host " 2) Install an old Helios (eg 1.4.2019.1005)"
write-host " 3) Install the new " $HeliosRelease
write-host
write-host "Press a key when complete..."
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
get-childitem ($env:ProgramFiles + "\Gadrocs Workshop\Helios\") -Recurse | format-list >($home + "\" + $HeliosRelease + ".Upgrade.Footprint.txt") -property name,length,Creationtime -groupby directory
write-host "Now perform a comparison between " $HeliosRelease ".Upgrade.Footprint.txt and " $HeliosRelease ".Install.Footprint.txt"
write-host
write-host "Press a key when complete..."
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');

