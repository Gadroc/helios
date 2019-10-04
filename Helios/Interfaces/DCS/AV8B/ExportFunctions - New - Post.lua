
if Helios.debug then
    Helios.log.write(thisScript,string.format("intends to communicate on " .. lHost .. ":" .. lPort .. "\n"))
	Helios.log.write(thisScript,string.format("Aircraft: " .. Helios.aircraft))
	Helios.log.write(thisScript,string.format("Local Mods - Running " .. thisScript))
	Helios.log.write(thisScript,string.format("Local Mods - Writedir " .. lfs.writedir()))
end

-- Exports.Lua from Helios AV-8B interface
function ProcessHighImportance(mainPanelDevice)
	-- Send Altimeter Values	
	SendData(2051, string.format("%0.4f;%0.4f;%0.4f", mainPanelDevice:get_argument_value(355), mainPanelDevice:get_argument_value(354), mainPanelDevice:get_argument_value(352)))
	SendData(2059, string.format("%0.2f;%0.2f;%0.2f;%0.3f", mainPanelDevice:get_argument_value(356), mainPanelDevice:get_argument_value(357), mainPanelDevice:get_argument_value(358), mainPanelDevice:get_argument_value(359)))		
end

function ProcessLowImportance(mainPanelDevice)

	li = parse_indication(5)
	if li then
		--Helios.log.write(thisScript,string.format("UFC Dump " .. Heliosdump(li)))
		--Helios.log.write(thisScript,string.format("UFC Comm 1: " .. check(li.ufc_chnl_1_m) .. check(li.ufc_chnl_1_v)))
        SendData("2092", string.format("%2s",check(li.ufc_left_position)))
        SendData("2094", string.format("%7s",check(li.ufc_right_position)))
        SendData("2095", string.format("%2s",check(li.ufc_chnl_1_m) .. check(li.ufc_chnl_1_v)))
		SendData("2096", string.format("%2s",check(li.ufc_chnl_2_m) .. check(li.ufc_chnl_2_v)))
	--[ufc_left_position]  string: "ON"
    --[ufc_right_position] = string: "16"
	end

 	local li = parse_indication(6)
	if li then
		SendData("2082", string.format("%4s",check(li.ODU_Option_1_Text)))
		SendData("2083", string.format("%4s",check(li.ODU_Option_2_Text)))
		SendData("2084", string.format("%4s",check(li.ODU_Option_3_Text)))
		SendData("2085", string.format("%4s",check(li.ODU_Option_4_Text)))
		SendData("2086", string.format("%4s",check(li.ODU_Option_5_Text)))
		SendData("2087", string.format("%1s",check(li.ODU_Option_1_Slc)):gsub(":","!"))  -- ":" is reserved
		SendData("2088", string.format("%1s",check(li.ODU_Option_2_Slc)):gsub(":","!"))  -- ":" is reserved
		SendData("2089", string.format("%1s",check(li.ODU_Option_3_Slc)):gsub(":","!"))  -- ":" is reserved
		SendData("2090", string.format("%1s",check(li.ODU_Option_4_Slc)):gsub(":","!"))  -- ":" is reserved
		SendData("2091", string.format("%1s",check(li.ODU_Option_5_Slc)):gsub(":","!"))  -- ":" is reserved
		-- -- test command 00000000*2096=20:2095=13:2087=!:2088=!:2089=!:2090=!:2091=!:2082=BLUE:2083=FIN :2084=BIMA:2085=2019:2086=test:2094=123.567:2092=~0:2093=-:326=1:336=1:197=1:365=1:196=1: 
	end

	local li = parse_indication(7)  --V/UHF Radio and ACNIP
	if li then
		Helios.log.write(thisScript,string.format("V/UHF Radio and ACNIP " .. Heliosdump(li)))

		--[UVHF_DISPLAY] = string: ""
		--[uvhf_channel] = string: "01"
		--[uvhf_freq_left] = string: "177.000"
		--[acnip_1_label_mode] = string: "MODE"
		--[acnip_1_mode] = string: "PLN"
		--[acnip_1_label_code] = string: "CODE"
		--[acnip_1_code] = string: "00"
		--[acnip_2_label_mode] = string: "MODE"
		--[acnip_2_mode] = string: "CY"
		--[acnip_2_label_code] = string: "CODE"
		--[acnip_2_code] = string: "03"
		
		SendData("2100", string.format("%2s",check(li.uvhf_channel)))
		SendData("2101", string.format("%7s",check(li.uvhf_freq_left)))
		SendData("2102", string.format("%s",check(li.acnip_1_label_mode)))
		SendData("2103", string.format("%s",check(li.acnip_1_mode)))
		SendData("2104", string.format("%s",check(li.acnip_1_label_code)))
		SendData("2105", string.format("%s",check(li.acnip_1_code)))		
		SendData("2106", string.format("%s",check(li.acnip_2_label_mode)))
		SendData("2107", string.format("%s",check(li.acnip_2_mode)))
		SendData("2108", string.format("%s",check(li.acnip_2_label_code)))
		SendData("2109", string.format("%s",check(li.acnip_2_code)))

	end

    SendData(2001, string.format("%.0f",mainPanelDevice:get_argument_value(253) * 1000+mainPanelDevice:get_argument_value(254) * 100+mainPanelDevice:get_argument_value(255) * 10))     -- Engine Duct
    SendData(2002, string.format("%.0f",mainPanelDevice:get_argument_value(256) * 10000+mainPanelDevice:get_argument_value(257) * 1000+mainPanelDevice:get_argument_value(258) * 100+mainPanelDevice:get_argument_value(259) * 10))     -- Engine RPM
    SendData(2003, string.format("%.0f",mainPanelDevice:get_argument_value(260) * 1000+mainPanelDevice:get_argument_value(261) * 100+mainPanelDevice:get_argument_value(262) * 10))    -- Engine FF
    SendData(2004, string.format("%.0f",mainPanelDevice:get_argument_value(263) * 1000+mainPanelDevice:get_argument_value(264) * 100+mainPanelDevice:get_argument_value(265) * 10)) -- Engine JPT
    SendData(2005, string.format("%.0f",mainPanelDevice:get_argument_value(267) * 100+mainPanelDevice:get_argument_value(268) * 10)) -- Engine Stab
    SendData(2006, string.format("%.0f",mainPanelDevice:get_argument_value(269) * 100+mainPanelDevice:get_argument_value(270) * 10)) -- Engine H2O
    SendData(2019, string.format("%.4f",mainPanelDevice:get_argument_value(386) * 100+mainPanelDevice:get_argument_value(387) * 10)) -- SMC Fuze
    SendData(2020, string.format("%.0f",mainPanelDevice:get_argument_value(392) * 1000+mainPanelDevice:get_argument_value(393) * 100+mainPanelDevice:get_argument_value(394) * 10))    -- SMC Interval
    SendData(2022, string.format("%.0f",mainPanelDevice:get_argument_value(389) * 100+mainPanelDevice:get_argument_value(390) * 10))    -- SMC Quantity
    SendData(2021, string.format("%.0f",mainPanelDevice:get_argument_value(391) * 10))    -- SMC Mult
    SendData(2010, string.format("%.0f",mainPanelDevice:get_argument_value(367) * 10000+mainPanelDevice:get_argument_value(368) * 1000+mainPanelDevice:get_argument_value(369) * 100+mainPanelDevice:get_argument_value(370) * 10))    -- Fuel Total
    SendData(2011, string.format("%.0f",mainPanelDevice:get_argument_value(371) * 10000+mainPanelDevice:get_argument_value(372) * 1000+mainPanelDevice:get_argument_value(373) * 100+mainPanelDevice:get_argument_value(374) * 10))    -- Fuel Left Tank
    SendData(2012, string.format("%.0f",mainPanelDevice:get_argument_value(375) * 10000+mainPanelDevice:get_argument_value(376) * 1000+mainPanelDevice:get_argument_value(377) * 100+mainPanelDevice:get_argument_value(378) * 10))    -- Fuel Right Tank
    SendData(2013, string.format("%.0f",mainPanelDevice:get_argument_value(381) * 10000+mainPanelDevice:get_argument_value(382) * 1000+mainPanelDevice:get_argument_value(383) * 100+mainPanelDevice:get_argument_value(384) * 10))    -- Fuel Bingo
    SendData(2014, string.format("%.0f",mainPanelDevice:get_argument_value(455) * 100+mainPanelDevice:get_argument_value(456) * 10))    -- Flap Position
    SendData(2015, string.format("%.0f",mainPanelDevice:get_argument_value(550) * 1000+mainPanelDevice:get_argument_value(551) * 100+mainPanelDevice:get_argument_value(552) * 10))    -- Pressure Brake
    SendData(2016, string.format("%.0f",mainPanelDevice:get_argument_value(553) * 1000+mainPanelDevice:get_argument_value(554) * 100+mainPanelDevice:get_argument_value(555) * 10))    -- Pressure Hyd 1
    SendData(2017, string.format("%.0f",mainPanelDevice:get_argument_value(556) * 1000+mainPanelDevice:get_argument_value(557) * 100+mainPanelDevice:get_argument_value(558) * 10))    -- Pressure Hyd 2
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
