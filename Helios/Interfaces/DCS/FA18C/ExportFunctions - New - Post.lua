
if Helios.debug then
    Helios.log.write(thisScript,string.format("intends to communicate on " .. lHost .. ":" .. lPort .. "\n"))
	Helios.log.write(thisScript,string.format("Aircraft: " .. Helios.aircraft))
	Helios.log.write(thisScript,string.format("Local Mods - Running " .. thisScript))
	Helios.log.write(thisScript,string.format("Local Mods - Writedir " .. lfs.writedir()))
end

ProcessHighImportance = function(mainPanelDevice)


--    --IFEI textures
--    local IFEI_Textures_table = {}
--    for i=1,16 do IFEI_Textures_table[i] = 0 end
--
--
--    -- getting the IFEI data
	local li = parse_indication(5)  -- 5 for IFEI
    --Helios.log.write(thisScript,string.format("IFEI Dump 5" .. Heliosdump(li)))
	if li then
--
--        --IFEI data
--
        SendData("2052", string.format("%s",check(li.txt_BINGO)))
        SendData("2053", string.format("%s",check(li.txt_CLOCK_H)))
        SendData("2054", string.format("%s",check(li.txt_CLOCK_M)))
        SendData("2055", string.format("%s",check(li.txt_CLOCK_S)))
        SendData("2056", string.format("%s",checkTexture(li.txt_DD_1)))
        SendData("2057", string.format("%s",checkTexture(li.txt_DD_2)))
        SendData("2058", string.format("%s",checkTexture(li.txt_DD_3)))
        SendData("2060", string.format("%s",checkTexture(li.txt_DD_4)))
        SendData("2061", string.format("%s",check(li.txt_FF_L)))
        SendData("2062", string.format("%s",check(li.txt_FF_R)))
        SendData("2063", string.format("%s",check(li.txt_FUEL_DOWN)))
        SendData("2064", string.format("%s",check(li.txt_FUEL_UP)))
        SendData("2065", string.format("%s",check(li.txt_OilPress_L)))
        SendData("2066", string.format("%s",check(li.txt_OilPress_R)))
        SendData("2067", string.format("%s",check(li.txt_RPM_L)))
        SendData("2068", string.format("%s",check(li.txt_RPM_R)))
        SendData("4019", string.format("%s",check(li.txt_FF_L))) --  Used for the nozzle position needles
        SendData("4020", string.format("%s",check(li.txt_FF_R))) --  Used for the nozzle position needles
        SendData("2069", string.format("%s",check(li.txt_TEMP_L)))
        SendData("2070", string.format("%s",check(li.txt_TEMP_R)))
        SendData("2073", string.format("%s",check(li.txt_TIMER_H)))
        SendData("2072", string.format("%s",check(li.txt_TIMER_M)))
        SendData("2071", string.format("%s",check(li.txt_TIMER_S)))
        SendData("2074", string.format("%s",check(li.txt_Codes)))
        SendData("2075", string.format("%s",check(li.txt_SP)))
        SendData("2076", string.format("%s",check(li.txt_DrawChar)))
        SendData("2077", string.format("%s",check(li.txt_T)))
        SendData("2078", string.format("%s",check(li.txt_TimeSetMode)))
--
--        --IFEI textures
--
        SendData("4000", string.format("%s",checkTexture(li.RPMTexture)))
        SendData("4001", string.format("%s",checkTexture(li.TempTexture)))
        SendData("4002", string.format("%s",checkTexture(li.FFTexture)))
        SendData("4003", string.format("%s",checkTexture(li.NOZTexture)))
        SendData("4004", string.format("%s",checkTexture(li.OILTexture)))
        SendData("4005", string.format("%s",checkTexture(li.BINGOTexture)))
        SendData("4006", string.format("%s",checkTexture(li.LScaleTexture)))
        SendData("4007", string.format("%s",checkTexture(li.RScaleTexture)))
        SendData("4008", string.format("%s",checkTexture(li.L0Texture)))
        SendData("4009", string.format("%s",checkTexture(li.R0Texture)))
        SendData("4010", string.format("%s",checkTexture(li.L50Texture)))
        SendData("4011", string.format("%s",checkTexture(li.R50Texture)))
        SendData("4012", string.format("%s",checkTexture(li.L100Texture)))
        SendData("4013", string.format("%s",checkTexture(li.R100Texture)))
        SendData("4014", string.format("%s",checkTexture(li.LPointerTexture)))
        SendData("4015", string.format("%s",checkTexture(li.RPointerTexture)))
		SendData("4016", string.format("%s",checkTexture(li.ZTexture)))
		SendData("4017", string.format("%s",checkTexture(li.LTexture)))
		SendData("4018", string.format("%s",checkTexture(li.RTexture)))

--
----
    end


	-- getting the UFC data
	local li = parse_indication(6)  -- 6 for UFC
	--Helios.log.write(thisScript,string.format("UFC Dump 6" .. Heliosdump(li)))

	if li then
		SendData("2080", string.format("%s",check(li.UFC_MainDummy)))
        SendData("2081", string.format("%s",check(li.UFC_mask)))
        SendData("2082", string.format("%s",check(li.UFC_OptionDisplay1)))
        SendData("2083", string.format("%s",check(li.UFC_OptionDisplay2)))
        SendData("2084", string.format("%s",check(li.UFC_OptionDisplay3)))
        SendData("2085", string.format("%s",check(li.UFC_OptionDisplay4)))
        SendData("2086", string.format("%s",check(li.UFC_OptionDisplay5)))
        SendData("2087", string.format("%1s",check(li.UFC_OptionCueing1)):gsub(":","!"))  -- ":" is reserved
        SendData("2088", string.format("%1s",check(li.UFC_OptionCueing2)):gsub(":","!"))  -- ":" is reserved
        SendData("2089", string.format("%1s",check(li.UFC_OptionCueing3)):gsub(":","!"))  -- ":" is reserved
        SendData("2090", string.format("%1s",check(li.UFC_OptionCueing4)):gsub(":","!"))  -- ":" is reserved
        SendData("2091", string.format("%1s",check(li.UFC_OptionCueing5)):gsub(":","!"))  -- ":" is reserved
        SendData("2092", string.format("%2s",check(li.UFC_ScratchPadString1Display)))
        SendData("2093", string.format("%2s",check(li.UFC_ScratchPadString2Display)))
        SendData("2094", string.format("%7s",check(li.UFC_ScratchPadNumberDisplay)))
        SendData("2095", string.format("%2s",check(li.UFC_Comm1Display)))
        SendData("2096", string.format("%2s",check(li.UFC_Comm2Display)))

		-- -- test command 00000000*2096=~0:2095=`3:2087=!:2088=!:2089=!:2090=!:2091=!:2082=BLUE:2083=FIN :2084=BIMA:2085=2019:2086=test:2094=123 567:2092=~0:2093=-:

	end
	
	--SendData("2098", string.format("%s", table.concat(IFEI_Textures_table,", ") ) )    -- IFEI Textures
	--local li = parse_indication(0)  -- 0
    --Helios.log.write(thisScript,string.format("Dump 0" .. Heliosdump(li)))
	--local li = parse_indication(1)  -- 1
    --Helios.log.write(thisScript,string.format("Dump 1" .. Heliosdump(li)))
	--local li = parse_indication(2)  -- 2
    --Helios.log.write(thisScript,string.format("Dump 2" .. Heliosdump(li)))
	--local li = parse_indication(3)  -- 3
    --Helios.log.write(thisScript,string.format("Dump 3" .. Heliosdump(li)))
	--local li = parse_indication(4)  -- 4
    --Helios.log.write(thisScript,string.format("Dump 4" .. Heliosdump(li)))
	--local li = parse_indication(5)  -- 5 for IFEI
    --Helios.log.write(thisScript,string.format("IFEI Dump 5" .. Heliosdump(li)))
	--local li = parse_indication(6)  -- 6 for IFEI
    --Helios.log.write(thisScript,string.format("IFEI Dump 6" .. Heliosdump(li)))

end
function Heliosdump(var, depth)
        depth = depth or 0
        if type(var) == "string" then
            return 'string: "' .. var .. '"\n'
        elseif type(var) == "nil" then
            return 'nil\n'
        elseif type(var) == "number" then
            return 'number: "' .. var .. '"\n'
        elseif type(var) == "boolean" then
            return 'boolean: "' .. tostring(var) .. '"\n'
        elseif type(var) == "function" then
            if debug and debug.getinfo then
                fcnname = tostring(var)
                local info = debug.getinfo(var, "S")
                if info.what == "C" then
                    return string.format('%q', fcnname .. ', C function') .. '\n'
                else
                    if (string.sub(info.source, 1, 2) == [[./]]) then
                        return string.format('%q', fcnname .. ', defined in (' .. info.linedefined .. '-' .. info.lastlinedefined .. ')' .. info.source) ..'\n'
                    else
                        return string.format('%q', fcnname .. ', defined in (' .. info.linedefined .. '-' .. info.lastlinedefined .. ')') ..'\n'
                    end
                end
            else
                return 'a function\n'
            end
        elseif type(var) == "thread" then
            return 'thread\n'
        elseif type(var) == "userdata" then
            return tostring(var)..'\n'
        elseif type(var) == "table" then
                depth = depth + 1
                out = "{\n"
                for k,v in pairs(var) do
                        out = out .. (" "):rep(depth*4).. "["..k.."] = " .. Heliosdump(v, depth)
                end
                return out .. (" "):rep((depth-1)*4) .. "}\n"
        else
                return tostring(var) .. "\n"
        end
end


ProcessLowImportance = function(mainPanelDevice)
	-- Get Radio Frequencies
	--local lUHFRadio = GetDevice(54)
	--SendData(2000, string.format("%7.3f", lUHFRadio:get_frequency()/1000000))
	-- ILS Frequency
	--SendData(2251, string.format("%0.1f;%0.1f", mainPanelDevice:get_argument_value(251), mainPanelDevice:get_argument_value(252)))
	-- TACAN Channel
	--SendData(2263, string.format("%0.2f;%0.2f;%0.2f", mainPanelDevice:get_argument_value(263), mainPanelDevice:get_argument_value(264), mainPanelDevice:get_argument_value(265)))

end

-- for some reason, this causes a failure on my system so commenting it
-- out in the hope that others don't see a problem with it.
--assert(os.setlocale'en_US.ISO-8859-1')

-- Simulation id
local lID = string.format("%08x*",os.time())

-- State data for export
local lPacketSize = 0
local lSendStrings = {}
local lLastData = {}

-- Frame counter for non important data
local lTickCount = 0


-- DCS Export Functions
LuaExportStart= function()
if scriptDebug > 0 then Helios.log.write(thisScript,"LuaExportStart() invoked.") end
-- Works once just before mission start.

    -- 2) Setup udp sockets to talk to helios
    package.path  = package.path..";.\\LuaSocket\\?.lua"
    package.cpath = package.cpath..";.\\LuaSocket\\?.dll"

    socket = require("socket")

    lConn = socket.udp()
	lConn:setsockname("*", 0)
	lConn:setoption('broadcast', true)
    lConn:settimeout(.001) -- set the timeout for reading the socket
    if lConn~= nil then
		Helios.log.write(thisScript,"LuaExportStart() socket open for communication.")
	else
		Helios.log.write(thisScript,"LuaExportStart() socket failed to open.")
	end
    if PrevExport.LuaExportStart then
        PrevExport.LuaExportStart()
    end
end

LuaExportBeforeNextFrame= function()
if scriptDebug > 0 then Helios.log.write(thisScript,"LuaExportBeforeNextFrame() invoked.") end
	ProcessInput()
    if PrevExport.LuaExportBeforeNextFrame then
       PrevExport.LuaExportBeforeNextFrame()
    end
end

LuaExportAfterNextFrame= function()
if scriptDebug > 0 then Helios.log.write(thisScript,"LuaExportAfterNextFrame() invoked.") end

    if PrevExport.LuaExportAfterNextFrame  then
        PrevExport.LuaExportAfterNextFrame()
    end

end

LuaExportStop= function()
if scriptDebug > 0 then Helios.log.write(thisScript,"LuaExportStop() invoked.") end
-- Works once just after mission stop.
    lConn:close()
    if PrevExport.LuaExportStop  then
        PrevExport.LuaExportStop()
    end

end

LuaExportActivityNextEvent= function(t)
	if scriptDebug > 0 then Helios.log.write(thisScript,"LuaExportActivityNextEvent() invoked.") end
	if scriptDebug > 0 and lConn == nil then Helios.log.write(thisScript,"Connection object is Nil in LuaExportActivityNextEvent().") end

	local lt = t + lInterval
    local lot = lt

	lTickCount = lTickCount + 1
	local lDevice = GetDevice(0)
	if type(lDevice) == "table" then
		lDevice:update_arguments()

		ProcessArguments(lDevice, lEveryFrameArguments)
		ProcessHighImportance(lDevice)

		if lTickCount >= lLowTickInterval then
			ProcessArguments(lDevice, lArguments)
			ProcessLowImportance(lDevice)
			lTickCount = 0
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

-- Network Functions
FlushData = function()
	if #lSendStrings > 0 then
		local packet = lID .. table.concat(lSendStrings, ":") .. "\n"
		socket.try(lConn:sendto(packet, lHost, lPort))
		lSendStrings = {}
		lPacketSize = 0
	end
end

SendData = function(id, value)
    if scriptDebug > 4 then Helios.log.write(thisScript,"Pre SendData: " .. id .. "=" .. value) end
    --if tonumber(id) >= 2052 then Helios.log.write(thisScript,"Pre SendData: " .. id .. "=" .. value) end


	if string.len(value) > 3 and value == string.sub("-0.00000000",1, string.len(value)) then
		value = value:sub(2)
	end

	if lLastData[id] == nil or lLastData[id] ~= value then
		local data =  id .. "=" .. value:gsub(":","::") -- escape any colons in the command's value
		local dataLen = string.len(data)

		if dataLen + lPacketSize > 576 then
			FlushData()
		end
        --Helios.log.write(thisScript,"SendData: " .. data)

		table.insert(lSendStrings, data)
		lLastData[id] = value
		lPacketSize = lPacketSize + dataLen + 1
	end
end

-- Status Gathering Functions
ProcessArguments = function(device, arguments)
	local lArgument , lFormat , lArgumentValue
	for lArgument, lFormat in pairs(arguments) do
		lArgumentValue = string.format(lFormat,device:get_argument_value(lArgument))
		SendData(lArgument, lArgumentValue)
	end
end

-- Data Processing Functions

parse_indication = function(indicator_id)  -- Thanks to [FSF]Ian code
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

ProcessInput = function()
    local lInput = lConn:receive()
    local lCommand, lCommandArgs, lDevice, lArgument, lLastValue

    if lInput then

        lCommand = string.sub(lInput,1,1)

		if lCommand == "R" then
            Helios.log.write(thisScript,"Reset Received - " .. lInput)
			ResetChangeValues()
		end

		if (lCommand == "C") then
            --Helios.log.write(thisScript,"Command Received - " .. lInput)
			lCommandArgs = StrSplit(string.sub(lInput,2),",")
			lDevice = GetDevice(lCommandArgs[1])
			if type(lDevice) == "table" then
				lDevice:performClickableAction(lCommandArgs[2],lCommandArgs[3])
			end
		end
    end
end

-- Helper Functions
StrSplit = function(str, delim, maxNb)
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

round = function(num, idp)
  local mult = 10^(idp or 0)
  return math.floor(num * mult + 0.5) / mult
end

check = function(s)
    if type(s) == "string" then
        print("Variable type is "..type(s))
        return s
    else
	    return ""
    end
end
checkTexture = function(s)
    if s == nil then return "0" else return "1" end
end
ResetChangeValues = function()
	lLastData = {}
	lTickCount = 10
end
end
end
