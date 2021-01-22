-- Exports.Lua from Helios F-14B Interface
print("Helios Aircraft Exports:  F-14B\n")


function ProcessHighImportance(mainPanelDevice)
	-- Send Altimeter Values	
	SendData(2051, string.format("%0.4f;%0.4f;%0.4f", mainPanelDevice:get_argument_value(220), mainPanelDevice:get_argument_value(219), mainPanelDevice:get_argument_value(218)))
	SendData(2059, string.format("%0.2f;%0.2f;%0.3f", mainPanelDevice:get_argument_value(223), mainPanelDevice:get_argument_value(222), mainPanelDevice:get_argument_value(221)))		
	-- Calcuate HSI Value
	-- SendData(2029, string.format("%0.2f;%0.2f;%0.4f", mainPanelDevice:get_argument_value(29), mainPanelDevice:get_argument_value(30), mainPanelDevice:get_argument_value(31)))


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