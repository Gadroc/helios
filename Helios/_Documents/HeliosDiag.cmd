@rem ******************************************************************************************
@rem ******************************************************************************************
@rem ***                                                                                    ***
@rem ***  This script saves away basic interface that might be needed to diagnose Helios    ***
@rem ***  problems.                                                                         ***
@rem ***                                                                                    ***
@rem ***  Different Windows Locales name "Saved Games" in local languages so there is an    ***
@rem ***  environment variable to name it.  There is another variable to indicate the type  ***
@rem ***  of DCS installation                                                               ***
@rem ***                                                                                    ***
@rem ***                                                                                    ***
@rem ***                                                                                    ***
@rem ******************************************************************************************
@rem ******************************************************************************************
set savedgames=Juegos guardados
set savedgames=Saved Games
set DCSType=DCS.openbeta
set heliosDiag=%userprofile%
set userdir=%userprofile%
@echo >"%heliosDiag%\Documents\Helios\heliosDiags.txt" V1
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Diagnostics for Helios
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * %date% - %time%
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * for user %userdir%
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
@echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt"  
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Environment Variables                             * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
set >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" 
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * UDP Ports -needs elevation to get creating process* * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
netstat >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" 2>&1 -an -p UDP -b -o
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
netstat >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" 2>&1 -an -p UDP
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * DCS Scripts Directory Listing                     * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Dir >>"%heliosDiag%\Documents\Helios\heliosDiags.txt"  %userdir%
Dir >>"%heliosDiag%\Documents\Helios\heliosDiags.txt"  "%userdir%\%savedgames%\%DCSType%\scripts"  /s
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Export.Lua                                        * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
type >>"%heliosDiag%\Documents\Helios\heliosDiags.txt"  "%userdir%\%savedgames%\%DCSType%\scripts\Export.lua"
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * DCS log File                                      * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
type >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" "%userdir%\%savedgames%\%DCSType%\logs\dcs.log"
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Helios log File                                   * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
type >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" "%userdir%\%savedgames%\%DCSType%\logs\Helios.log"
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Helios Directory                                  * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
dir >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" "%userdir%\Documents\Helios\" /s
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Helios Control Center log File                    * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
type >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" "%userdir%\Documents\Helios\ControlCenter.log"
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Helios Profile Editor log File                    * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
type >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" "%userdir%\Documents\Helios\ProfileEditor.log"
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * End Diags                                         * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
set userdir=C:\Users\Default
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Diagnostics for Helios
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * %date% - %time%
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * for user %userdir%
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
@echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt"  
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * DCS Scripts Directory Listing                     * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Dir >>"%heliosDiag%\Documents\Helios\heliosDiags.txt"  %userdir%
Dir >>"%heliosDiag%\Documents\Helios\heliosDiags.txt"  "%userdir%\%savedgames%\%DCSType%\scripts"  /s
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Export.Lua                                        * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
type >>"%heliosDiag%\Documents\Helios\heliosDiags.txt"  "%userdir%\%savedgames%\%DCSType%\scripts\Export.lua"
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * DCS log File                                      * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
type >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" "%userdir%\%savedgames%\%DCSType%\logs\dcs.log"
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Helios log File                                   * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
type >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" "%userdir%\%savedgames%\%DCSType%\logs\Helios.log"
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Helios Directory                                  * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
dir >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" "%userdir%\Documents\Helios\" /s
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Helios Control Center log File                    * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
type >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" "%userdir%\Documents\Helios\ControlCenter.log"
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * Helios Profile Editor log File                    * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
type >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" "%userdir%\Documents\Helios\ProfileEditor.log"
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * End Diags                                         * * *
echo >>"%heliosDiag%\Documents\Helios\heliosDiags.txt" * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
