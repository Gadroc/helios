--
-- The Aircraft specifc Exports file lives in a sub directory of the saved games Scripts directory
-- and the name of the directory is the same as the DCS name for the aircraft.
--
local lAircraft = "AV8BNA"
if Helios.aircraft == lAircraft and not Helios.vr then
do
local lfs = require "lfs"
local PrevExport = {}
PrevExport.LuaExportStart = LuaExportStart
PrevExport.LuaExportStop = LuaExportStop
PrevExport.LuaExportBeforeNextFrame = LuaExportBeforeNextFrame
PrevExport.LuaExportAfterNextFrame = LuaExportAfterNextFrame
PrevExport.LuaExportActivityNextEvent = LuaExportActivityNextEvent

local lHost
local lPort
local lInterval
local lLowTickInterval
local lConn
local lEveryFrameArguments
local lArguments
local parse_indication
local ProcessHighImportance
local ProcessLowImportance
local FlushData
local SendData
local ProcessArguments
local ResetChangeValues
local ProcessInput
local StrSplit
local roundS
local check
local checkTexture
local Heliosdump
local scriptDebug
local thisScript


LuaExportStart =nil
LuaExportBeforeNextFrame =nil
LuaExportAfterNextFrame =nil
LuaExportStop =nil
LuaExportActivityNextEvent =nil

scriptDebug = 0
thisScript = debug.getinfo(1,'S').short_src:gsub("\\","/"):match('^.*/(.*).lua"]$')

