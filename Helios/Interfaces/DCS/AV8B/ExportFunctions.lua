function ProcessHighImportance(mainPanelDevice)

	-- Send Altimeter Values	
	SendData(2051, string.format("%0.4f;%0.4f;%0.4f", mainPanelDevice:get_argument_value(355), mainPanelDevice:get_argument_value(354), mainPanelDevice:get_argument_value(352)))
	SendData(2059, string.format("%0.2f;%0.2f;%0.2f;%0.3f", mainPanelDevice:get_argument_value(356), mainPanelDevice:get_argument_value(357), mainPanelDevice:get_argument_value(358), mainPanelDevice:get_argument_value(359)))		
	-- Calcuate HSI Value
	--SendData(2029, string.format("%0.2f;%0.2f;%0.4f", mainPanelDevice:get_argument_value(29), mainPanelDevice:get_argument_value(30), mainPanelDevice:get_argument_value(31)))
end


function ProcessLowImportance(mainPanelDevice)
    -- SMC Mode
    local SMC_Mode = mainPanelDevice:get_argument_value(385)
	if SMC_Mode >= 1.0 then SendData(2018, "AGM")
	elseif SMC_Mode >= 0.8 then SendData(2018, "DIR")    -- SMC Mode
	elseif SMC_Mode >= 0.6 then SendData(2018, "DSL")    -- SMC Mode
	elseif SMC_Mode >= 0.4 then SendData(2018, "CIP")    -- SMC Mode
	elseif SMC_Mode >= 0.2 then SendData(2018, "AUT")    -- SMC Mode
	elseif SMC_Mode >= 0 then SendData(2018, " - ")      -- SMC Mode
	end
  
    SendData(2001, string.format("%.0f",mainPanelDevice:get_argument_value(253) * 1000+mainPanelDevice:get_argument_value(254) * 100+mainPanelDevice:get_argument_value(255) * 10))     -- Engine Duct
    SendData(2002, string.format("%.0f",mainPanelDevice:get_argument_value(256) * 10000+mainPanelDevice:get_argument_value(257) * 1000+mainPanelDevice:get_argument_value(258) * 100+mainPanelDevice:get_argument_value(259) * 10))     -- Engine RPM
    SendData(2003, string.format("%.0f",mainPanelDevice:get_argument_value(260) * 1000+mainPanelDevice:get_argument_value(261) * 100+mainPanelDevice:get_argument_value(262) * 10))    -- Engine FF
    SendData(2004, string.format("%.0f",mainPanelDevice:get_argument_value(263) * 1000+mainPanelDevice:get_argument_value(264) * 100+mainPanelDevice:get_argument_value(265) * 10)) -- Engine JPT
    SendData(2005, string.format("%.0f",mainPanelDevice:get_argument_value(267) * 100+mainPanelDevice:get_argument_value(268) * 10)) -- Engine Stab
    SendData(2006, string.format("%.0f",mainPanelDevice:get_argument_value(269) * 100+mainPanelDevice:get_argument_value(270) * 10)) -- Engine H2O
    SendData(2019, string.format("%.0f",mainPanelDevice:get_argument_value(386) * 10) .. string.format("%.0f",mainPanelDevice:get_argument_value(387) * 10)) -- SMC Fuze
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
