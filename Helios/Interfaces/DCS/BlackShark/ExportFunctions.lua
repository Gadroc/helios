-- Lookup tables for weapons store type display
gStationTypes = {["9A4172"] = "NC", ["S-8KOM"] = "HP", ["S-13"] = "HP", ["UPK-23-250"] = "NN", ["AO-2.5RT"] = "A6", ["PTAB-2.5KO"] = "A6",
				 ["FAB-250"] = "A6", ["FAB-500"] = "A6" }

-- State data
gTrigger = 0

function ProcessHighImportance(mainPanelDevice)

end

function ProcessLowImportance(mainPanelDevice)
	local lWeaponSystem = GetDevice(12)
	local lEKRAN = GetDevice(10)
	local lCannonAmmoCount = " "
	local lStationNumbers = lWeaponSystem:get_selected_weapon_stations()
	local lStationCount = " "
	local lStationType = " "	
	local lTargetingPower = mainPanelDevice:get_argument_value(433)
	local lTrigger = mainPanelDevice:get_argument_value(615)
	if lTrigger == 0 then
		gTrigger = 1
	end
	if lTrigger == -1 then
		gTrigger = 0
	end

	if lTargetingPower == 1 then
		lCannonAmmoCount = string.format("%02d",string.match(lWeaponSystem:get_selected_gun_ammo_count() / 10,"(%d+)"))
	
		if #lStationNumbers ~= 0 and gTrigger == 0 then
			lStationCount = 0
			for i=1,#lStationNumbers do
				lStationCount = lStationCount + lWeaponSystem:get_weapon_count_on_station(lStationNumbers[i])
			end
			
			lStationCount = string.format("%02d", lStationCount);
			
			lStationType = gStationTypes[lWeaponSystem:get_weapon_type_on_station(lStationNumbers[1])]
			if lStationType == nil then
				lStationType = " "
			end
		end
	end

	local lEkranText = lEKRAN:get_actual_text_frame()
	local lEkranSendString = string.sub(lEkranText,1,8).."\n"..string.sub(lEkranText,12,19).."\n"..string.sub(lEkranText,23,30).."\n"..string.sub(lEkranText,34,41) 
	
	SendData("2001",lStationType)
	SendData("2002",lStationCount)
	SendData("2003",lCannonAmmoCount)
	SendData("2004",lEkranSendString)
end