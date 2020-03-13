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
   dim msiPackage:msiPackage = Wscript.Arguments(0)
   Wscript.Echo "Starting Post Build Script to correct: " & msiPackage
   Dim sNewRel,sOldRel,sUpgradeRel,KeraLuaFileGUID
   sUpgradeRel = "1.4." & Year(Date)
   KeraLuaFileGUID = "_161114B3210C9125BCADDBB596A52A7A"
   dim reloffset:reloffset = cint(inputbox("Offset in days for the release number." & vbcrlf & sUpgradeRel & ".xxxx minus this offset","Release Offset","0"))
   sOldRel = sUpgradeRel & "." & Month(Date-reloffset) & RIGHT("0" & day(date-reloffset),2)
   sNewRel = sUpgradeRel & "." & RIGHT("0" & Month(Date-reloffset),2) & RIGHT("0" & day(date-reloffset),2)
   WScript.Echo "Current Dir:     " & CreateObject("WScript.Shell").CurrentDirectory
   Wscript.Echo "Upgrade Release: " & sUpgradeRel
   Wscript.Echo "Old Release:     " & sOldRel
   Wscript.Echo "New Release:     " & sNewRel
   Dim oShell 
   Set oShell = Wscript.CreateObject("WScript.Shell")
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO Property(Property, Value) VALUES ('DISABLEADVTSHORTCUTS', '1')""",0,true
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""UPDATE Property SET `Value` = '" & sNewRel & "' WHERE Property = 'ProductVersion'""",0,true
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""UPDATE File SET `Version` = '" & sNewRel & "' WHERE `Version` = '" & sOldRel & "'""",0,true
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""UPDATE MsiAssemblyName SET `Value` = '" & sNewRel & "' WHERE `Value` = '" & sOldRel & "'""",0,true
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""SELECT Version from File WHERE File = '" & KeraLuaFileGUID & "'""",0,true
   Dim result, fso, fs, keraluaRel
   Const ForReading = 1, ForWriting = 2, ForAppending = 8
   Const TristateFalse = 0
   Set fso = CreateObject("Scripting.FileSystemObject")
   Set fs  = fso.OpenTextFile("..\select_output.txt", ForReading, TristateFalse)
   keraluaRel = "1" & fs.ReadLine
   fs.Close
   Wscript.Echo "Setting the Installer's Version number for KeraLua.dll to a dummy because current NuGet versions have a lower version number than an earlier dll!  This will probably bite us at some point.  " & keraluaRel
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""UPDATE File SET Version = '" & keraluaRel & "' WHERE File = '" & KeraLuaFileGUID & "'""",0,true
   Wscript.Echo "Set KeraLua.dll file to be removed during installation.(this does not work)"
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO RemoveFile(FileKey, Component_, FileName,DirProperty,InstallMode) VALUES ('DELETE_KERALUADLL','C__746C69616E6711D38E0D00C04F6837D0','KeraLua.dll','TARGETDIR', '3')""",0,true
   Wscript.Echo "Set Phidgets.dll file to be removed during installation."
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO RemoveFile(FileKey, Component_, FileName,DirProperty,InstallMode) VALUES ('DELETE_PHIDGETSDLL','C__746C69616E6711D38E0D00C04F6837D0','Phidget*.dll','TARGETDIR', '3')""",0,true
   Wscript.Echo "Set Lua52.dll file to be removed during installation."
   oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO RemoveFile(FileKey, Component_, FileName,DirProperty,InstallMode) VALUES ('DELETE_LUA52DLL','C__746C69616E6711D38E0D00C04F6837D0','Lua52.dll','TARGETDIR', '3')""",0,true

   ' This next one is almost certain to fail, probably because it is attempting to update the primary key.
   'oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""INSERT INTO Upgrade(UpgradeCode, VersionMin, VersionMax,Language, Attribute,ActionProperty) VALUES ('{589D8667-3ED9-478B-8F67-A56E4FADBC63}', '1.4.2018','1.4.20199999','En',256,'NEWERPRODUCTFOUND')""",0,true
   'oShell.Run "..\WiRunSQL.vbs """ & msiPackage & """  ""UPDATE Upgrade SET VersionMin = '" & sNewRel & "' WHERE UpgradeCode = '{589D8667-3ED9-478B-8F67-A56E4FADBC63}'""",0,true
   Set oShell = Nothing 
end if
Wscript.Quit 0



 

