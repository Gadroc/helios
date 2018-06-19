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

-- DCS Export Functions
function LuaExportStart()
-- Works once just before mission start.
	
    -- 2) Setup udp sockets to talk to helios
    package.path  = package.path..";.\\LuaSocket\\?.lua"
    package.cpath = package.cpath..";.\\LuaSocket\\?.dll"
   
    socket = require("socket")
    
    c = socket.udp()
	c:setsockname("*", 0)
	c:setoption('broadcast', true)
    c:settimeout(.001) -- set the timeout for reading the socket 
end

function LuaExportBeforeNextFrame()
	ProcessInput()
end

function LuaExportAfterNextFrame()	
end

function LuaExportStop()
-- Works once just after mission stop.
    c:close()
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

function LuaExportActivityNextEvent(t)
	t = t + gExportInterval

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

	return t
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

-- Status Gathering Functions
function ProcessArguments(device, arguments)
	local lArgument , lFormat , lArgumentValue
		
	for lArgument, lFormat in pairs(arguments) do 
		lArgumentValue = string.format(lFormat,device:get_argument_value(lArgument))
		SendData(lArgument, lArgumentValue)
	end
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
