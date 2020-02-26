local PrevExport = {}
PrevExport.LuaExportStart = LuaExportStart
PrevExport.LuaExportStop = LuaExportStop
PrevExport.LuaExportBeforeNextFrame = LuaExportBeforeNextFrame
PrevExport.LuaExportAfterNextFrame = LuaExportAfterNextFrame
PrevExport.LuaExportActivityNextEvent = LuaExportActivityNextEvent

LuaExportStart =nil
LuaExportBeforeNextFrame =nil
LuaExportAfterNextFrame =nil
LuaExportStop =nil
LuaExportActivityNextEvent =nil

-- for some reason, this causes a failure on my system so commenting it
-- out in the hope that others don't see a problem with it.
-- os.setlocale("ISO-8559-1", "numeric")

-- Simulation id
gSimID = string.format("%08x*",os.time())

-- State data for export
gPacketSize = 0
gSendStrings = {}
gLastData = {}

-- Frame counter for non important data
gTickCount = 0

-- Check the Aircraft is right for the interface
local DCSInfo = LoGetSelfData()
if DCSInfo ~= nil and lAircraft ~= nil then
	if (DCSInfo.Name == lAircraft) then
		log.write('USERMOD',log.INFO," Helios Exports.Lua Loaded for " .. DCSInfo.Name )
	else
		log.write('USERMOD',log.WARNING," Helios Exports.Lua Loaded for " .. lAircraft .. " but " .. DCSInfo.Name .. " has been started" )
	end
end
-- DCS Export Functions
LuaExportStart = function()
-- Works once just before mission start.
	
    -- 2) Setup udp sockets to talk to helios
    package.path  = package.path..";.\\LuaSocket\\?.lua"
    package.cpath = package.cpath..";.\\LuaSocket\\?.dll"
   
    socket = require("socket")
    
    c = socket.udp()
	c:setsockname("*", 0)
	c:setoption('broadcast', true)
    c:settimeout(.001) -- set the timeout for reading the socket 

	if PrevExport.LuaExportStart then
        PrevExport.LuaExportStart()
    end
end

LuaExportBeforeNextFrame = function()
	ProcessInput()
	
	if PrevExport.LuaExportBeforeNextFrame then
       PrevExport.LuaExportBeforeNextFrame()
    end
end

LuaExportAfterNextFrame = function()	
    if PrevExport.LuaExportAfterNextFrame  then
        PrevExport.LuaExportAfterNextFrame()
    end

end

LuaExportStop = function()
-- Works once just after mission stop.
-- Send DISCONNECT message so we can fire the Helios Disconnect event
    socket.try(c:sendto("DISCONNECT\n", gHost, gPort))
    c:close()
	
	if PrevExport.LuaExportStop  then
        PrevExport.LuaExportStop()
    end

end

function ProcessInput()
    local lInput = c:receive()
    local lCommand, lCommandArgs, lDevice, lArgument, lLastValue
    
    if lInput then
	
        lCommand = string.sub(lInput,1,1)
        
		if lCommand == "R" then
			ResetChangeValues()
		end
	
		if (lCommand == "C") then
			lCommandArgs = StrSplit(string.sub(lInput,2),",")
			lDevice = GetDevice(lCommandArgs[1])
			if type(lDevice) == "table" then
				lDevice:performClickableAction(lCommandArgs[2],lCommandArgs[3])	
			end
		end
    end 
end

LuaExportActivityNextEvent = function(t)
	local lt = t + gExportInterval
    local lot = lt

	gTickCount = gTickCount + 1

	local lDevice = GetDevice(0)
	if type(lDevice) == "table" then
		lDevice:update_arguments()

		ProcessArguments(lDevice, gEveryFrameArguments)
		ProcessHighImportance(lDevice)

		if gTickCount >= gExportLowTickInterval then
			ProcessArguments(lDevice, gArguments)
			ProcessLowImportance(lDevice)
			gTickCount = 0
		end

		FlushData()
	end

    if PrevExport.LuaExportActivityNextEvent then
        lot = PrevExport.LuaExportActivityNextEvent(t)  -- if we were given a value then pass it on
    end
    if  lt > lot then
        lt = lot -- take the lesser of the next event times
    end
	return lt
end

-- Helper Functions
function StrSplit(str, delim, maxNb)
    -- Eliminate bad cases...
    if string.find(str, delim) == nil then
        return { str }
    end
    if maxNb == nil or maxNb < 1 then
        maxNb = 0    -- No limit
    end
    local result = {}
    local pat = "(.-)" .. delim .. "()"
    local nb = 0
    local lastPos
    for part, pos in string.gfind(str, pat) do
        nb = nb + 1
        result[nb] = part
        lastPos = pos
        if nb == maxNb then break end
    end
    -- Handle the last field
    if nb ~= maxNb then
        result[nb + 1] = string.sub(str, lastPos)
    end
    return result
end

function round(num, idp)
  local mult = 10^(idp or 0)
  return math.floor(num * mult + 0.5) / mult
end

function check(s)
    if type(s) == "string" then 
        return s
    else
	    return ""
    end
end
-- Status Gathering Functions
function ProcessArguments(device, arguments)
	local lArgument , lFormat , lArgumentValue
		
	for lArgument, lFormat in pairs(arguments) do 
		lArgumentValue = string.format(lFormat,device:get_argument_value(lArgument))
		SendData(lArgument, lArgumentValue)
	end
end

function parse_indication(indicator_id)  -- Thanks to [FSF]Ian code
	local ret = {}
	local li = list_indication(indicator_id)
	if li == "" then return nil end
	local m = li:gmatch("-----------------------------------------\n([^\n]+)\n([^\n]*)\n")
	while true do
	local name, value = m()
	if not name then break end
		ret[name] = value
	end
	return ret
end

-- Network Functions
function SendData(id, value)	
	if string.len(value) > 3 and value == string.sub("-0.00000000",1, string.len(value)) then
		value = value:sub(2)
	end
	
	if gLastData[id] == nil or gLastData[id] ~= value then
		local data =  id .. "=" .. value
		local dataLen = string.len(data)

		if dataLen + gPacketSize > 576 then
			FlushData()
		end

		table.insert(gSendStrings, data)
		gLastData[id] = value	
		gPacketSize = gPacketSize + dataLen + 1
	end	
end

function FlushData()
	if #gSendStrings > 0 then
		local packet = gSimID .. table.concat(gSendStrings, ":") .. "\n"
		socket.try(c:sendto(packet, gHost, gPort))
		gSendStrings = {}
		gPacketSize = 0
	end
end

function ResetChangeValues()
	gLastData = {}
	gTickCount = 10
end
