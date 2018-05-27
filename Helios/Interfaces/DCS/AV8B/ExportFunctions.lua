gEveryFrameArguments = {[271]="%0.2f",[347]="%0.4f",[348]="%0.4f",[349]="%0.4f",[345]="%0.4f",[346]="%0.3f",[362]="%0.3f",[360]="%0.1f",[361]="%0.4f",[460]="%0.1f",[461]="%0.1f",[457]="%0.1f",[459]="%0.1f",[380]="%0.4f",
                        [196]="%0.1f",[197]="%0.1f",[276]="%0.1f",[277]="%0.1f",[278]="%0.1f",[279]="%0.1f",[281]="%0.1f",[283]="%0.1f",[285]="%0.1f",[326]="%0.1f",[327]="%0.1f",[328]="%0.1f",[329]="%0.1f",[330]="%0.1f",
                        [331]="%0.1f",[334]="%0.1f",[335]="%0.1f",[336]="%0.1f",[337]="%0.1f",[338]="%0.1f",[339]="%0.1f",[340]="%0.1f",[341]="%0.1f",[342]="%0.1f",[343]="%0.1f",[344]="%0.1f",[446]="%0.1f",[451]="%0.1f",
                        [452]="%0.1f",[453]="%0.1f",[462]="%0.1f",[463]="%0.1f",[464]="%0.1f",[465]="%0.1f",[466]="%0.1f",[467]="%0.1f",[468]="%0.1f",[469]="%0.1f",[560]="%0.1f",[561]="%0.1f",[562]="%0.1f",[563]="%0.1f",
                        [564]="%0.1f",[565]="%0.1f",[566]="%0.1f",[567]="%0.1f",[568]="%0.1f",[569]="%0.1f",[570]="%0.1f",[571]="%0.1f",[572]="%0.1f",[573]="%0.1f",[574]="%0.1f",[575]="%0.1f",[576]="%0.1f",[577]="%0.1f",
                        [578]="%0.1f",[579]="%0.1f",[580]="%0.1f",[581]="%0.1f",[582]="%0.1f",[583]="%0.1f",[584]="%0.1f",[585]="%0.1f",[586]="%0.1f",[587]="%0.1f",[588]="%0.1f",[589]="%0.1f",[590]="%0.1f",[591]="%0.1f",
                        [592]="%0.1f",[593]="%0.1f",[594]="%0.1f",[595]="%0.1f",[596]="%0.1f",[597]="%0.1f",[598]="%0.1f",[599]="%0.1f",[600]="%0.1f",[601]="%0.1f",[602]="%0.1f",[603]="%0.1f",[604]="%0.1f",[605]="%0.1f",
                        [606]="%0.1f",[750]="%0.1f",[751]="%0.1f",[752]="%0.1f",[406]="%0.1f",[408]="%0.1f",[410]="%0.1f",[412]="%0.1f",[414]="%0.1f",[416]="%0.1f",[418]="%0.1f",[179]="%0.1f"}

gArguments = {[266]="%0.1f",[559]="%0.4f",[607]="%0.4f",[608]="%0.4f",[473]="%.4f",[474]="%.4f",[363]="%.4f",[23]="%.4f",[24]="%.4f",[385]="%0.1f",[386]="%0.1f",[387]="%0.1f",[653]="%0.4f",[364]="%0.4f",[471]="%0.4f",[472]="%0.4f",
              [475]="%0.4f",[476]="%0.4f",[477]="%0.4f",[478]="%0.4f",[479]="%0.4f",[480]="%0.4f",[759]="%0.4f",[760]="%0.4f",[761]="%0.4f"}


function ProcessHighImportance(mainPanelDevice)
	-- Send Altimeter Values	
	SendData(2051, string.format("%0.4f;%0.4f;%0.4f", mainPanelDevice:get_argument_value(355), mainPanelDevice:get_argument_value(354), mainPanelDevice:get_argument_value(352)))
	SendData(2059, string.format("%0.2f;%0.2f;%0.2f;%0.3f", mainPanelDevice:get_argument_value(356), mainPanelDevice:get_argument_value(357), mainPanelDevice:get_argument_value(358), mainPanelDevice:get_argument_value(359)))		
	-- Calcuate HSI Value
	--SendData(2029, string.format("%0.2f;%0.2f;%0.4f", mainPanelDevice:get_argument_value(29), mainPanelDevice:get_argument_value(30), mainPanelDevice:get_argument_value(31)))
end


function ProcessLowImportance(mainPanelDevice)
	-- Get Radio Frequencies
	--local lUHFRadio = GetDevice(54)
	--SendData(2000, string.format("%7.3f", lUHFRadio:get_frequency()/1000000))
	-- ILS Frequency
	--SendData(2251, string.format("%0.1f;%0.1f", mainPanelDevice:get_argument_value(251), mainPanelDevice:get_argument_value(252)))
	-- TACAN Channel
	--SendData(2263, string.format("%0.2f;%0.2f;%0.2f", mainPanelDevice:get_argument_value(263), mainPanelDevice:get_argument_value(264), mainPanelDevice:get_argument_value(265)))

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
