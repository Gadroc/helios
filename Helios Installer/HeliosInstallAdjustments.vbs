' This script actions the changes needed to the MSI package.
Option Explicit
Dim argNum, argCount:argCount = Wscript.Arguments.Count
if argCount = 0  then
   Wscript.Echo "Helios Installer Post Build changes requires an argument which is the" &_
   vbLf & " msi file to have the releases changed, and also set certain flags." &_
   vbLf & " No argument was passed into the HeliosInstallAdjustments.vbs file." &_
   vbLf & " SELECT queries will display the rows of the result list specified in the query"
    Wscript.Quit 1
else
   Dim msiPackage:msiPackage = Wscript.Arguments(0)
   Dim oShell 
   Dim TypeLib
   Dim version:version = Wscript.Arguments(1)
   Dim infinity:infinity = "65535" & trim(MID(replace(version,vbcrlf,""), Instr(version,".")))
   Wscript.Echo "Starting Post Build Script to set up " & msiPackage & " for version " & version
   Wscript.Echo "Setting file versions to " & infinity
   Set oShell = Wscript.CreateObject("WScript.Shell")
   
   'Disable Advertised Shortcuts which stops installation on demand 
   Wscript.Echo "Disable Advertised Shortcuts"
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO Property(Property, Value) VALUES ('DISABLEADVTSHORTCUTS', '1')""",0,true
   
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""UPDATE Property SET `Value` = '" & version & "' WHERE `Property` = 'ProductVersion'""",0,true
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""UPDATE File SET `Version` = '" & infinity & "' WHERE `Version` <> ''""",0,true

   ' make all upgrades major upgrades
   Set TypeLib = CreateObject("Scriptlet.TypeLib")
   Dim newGuid:newGuid = TypeLib.Guid
   newGuid = Left(newGuid, Len(newGuid)-2)
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""UPDATE Property SET `Value` = '" & newGuid & "' WHERE `Property` = 'ProductCode'""",0,true

   ' allow any version to upgrade
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""DELETE FROM Upgrade""",0,true
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO Upgrade(UpgradeCode, VersionMin, Attributes, ActionProperty) VALUES ('{589D8667-3ED9-478B-8F67-A56E4FADBC63}', '" & version & "', '258', 'NEWERPRODUCTFOUND')""",0,true
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO Upgrade(UpgradeCode, VersionMax, Attributes, ActionProperty) VALUES ('{589D8667-3ED9-478B-8F67-A56E4FADBC63}', '" & version & "', '0', 'PREVIOUSVERSIONINSTALLED')""",0,true

   ' oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""UPDATE MsiAssemblyName SET Value = '" & version & "' WHERE `Value` = '" & sOldRel & "'""",0,true
   Wscript.Echo "Set KeraLua.dll, Phidgets.dll, Lua52.dll, changelog.txt files to be removed during installation."
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO RemoveFile(FileKey, Component_, FileName,DirProperty,InstallMode) VALUES ('DELETE_KERALUADLL','C__746C69616E6711D38E0D00C04F6837D0','KeraLua.dll','TARGETDIR', '3')""",0,true
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO RemoveFile(FileKey, Component_, FileName,DirProperty,InstallMode) VALUES ('DELETE_PHIDGETSDLL','C__746C69616E6711D38E0D00C04F6837D0','Phidget*.dll','TARGETDIR', '3')""",0,true
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO RemoveFile(FileKey, Component_, FileName,DirProperty,InstallMode) VALUES ('DELETE_LUA52DLL','C__746C69616E6711D38E0D00C04F6837D0','Lua52.dll','TARGETDIR', '3')""",0,true
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO RemoveFile(FileKey, Component_, FileName,DirProperty,InstallMode) VALUES ('DELETE_OLD_CHANGELOG','C__746C69616E6711D38E0D00C04F6837D0','changelog.txt','TARGETDIR', '3')""",0,true
   
   Set oShell = Nothing 
end if
Wscript.Quit 0