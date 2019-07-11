-- Exports.Lua from Helios F/A-18C Interface
print("Helios Aircraft Exports:  F/A-18C\n")


function ProcessHighImportance(mainPanelDevice)
	-- Send Altimeter Values	
	SendData(2051, string.format("%0.4f;%0.4f;%0.4f", mainPanelDevice:get_argument_value(220), mainPanelDevice:get_argument_value(219), mainPanelDevice:get_argument_value(218)))
	SendData(2059, string.format("%0.2f;%0.2f;%0.3f", mainPanelDevice:get_argument_value(223), mainPanelDevice:get_argument_value(222), mainPanelDevice:get_argument_value(221)))		
	-- Calcuate HSI Value
	-- SendData(2029, string.format("%0.2f;%0.2f;%0.4f", mainPanelDevice:get_argument_value(29), mainPanelDevice:get_argument_value(30), mainPanelDevice:get_argument_value(31)))


--    --IFEI textures
--    local IFEI_Textures_table = {}
--    for i=1,16 do IFEI_Textures_table[i] = 0 end
--
--
--    -- getting the IFEI data
    local li = parse_indication(5)  -- 5 for IFEI
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
        SendData("2069", string.format("%s",check(li.txt_TEMP_L)))
        SendData("2070", string.format("%s",check(li.txt_TEMP_R)))
        SendData("2073", string.format("%s",check(li.txt_TIMER_H)))		
        SendData("2072", string.format("%s",check(li.txt_TIMER_M)))		
        SendData("2071", string.format("%s",check(li.txt_TIMER_S)))		
        SendData("2074", string.format("%s",check(li.txt_Codes)))
        SendData("2075", string.format("%s",check(li.txt_SP)))
        SendData("2076", string.format("%s",check(li.txt_DrawChar)))  -- not seen this used
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
--
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

end


function ProcessLowImportance(mainPanelDevice)
	-- Get Radio Frequencies
	--local lUHFRadio = GetDevice(54)
	--SendData(2000, string.format("%7.3f", lUHFRadio:get_frequency()/1000000))
	-- ILS Frequency
	--SendData(2251, string.format("%0.1f;%0.1f", mainPanelDevice:get_argument_value(251), mainPanelDevice:get_argument_value(252)))
	-- TACAN Channel
	--SendData(2263, string.format("%0.2f;%0.2f;%0.2f", mainPanelDevice:get_argument_value(263), mainPanelDevice:get_argument_value(264), mainPanelDevice:get_argument_value(265)))

end

checkTexture = function(s)
    if s == nil then return "0" else return "1" end
end