-- Exports.Lua from Helios Mi-8 Interface

function ProcessHighImportance(mainPanelDevice)

	local altBar = LoGetAltitudeAboveSeaLevel()
	local altRad = LoGetAltitudeAboveGroundLevel()
	local pitch, bank, yaw = LoGetADIPitchBankYaw()
	local engine = LoGetEngineInfo()
	local hsi    = LoGetControlPanel_HSI()
	local vvi = LoGetVerticalVelocity()
	local ias = LoGetIndicatedAirSpeed()
	local route = LoGetRoute()
	local myData = LoGetSelfData()
	local aoa = LoGetAngleOfAttack()
	local distanceToWay = 999
	
	local glide = LoGetGlideDeviation()
	local side = LoGetSideDeviation()
	
	if (myData ~= nil) then
		local myLoc = LoGeoCoordinatesToLoCoordinates(myData.LatLongAlt.Long, myData.LatLongAlt.Lat)
		distanceToWay = math.sqrt((myLoc.x - route.goto_point.world_point.x)^2 + (myLoc.y -  route.goto_point.world_point.y)^2)
	end

	if (pitch ~= nill) then
		SendData("1", pitch * 57.3, "%.2f")
		SendData("2", bank * 57.3, "%.2f")
		SendData("3", yaw * 57.3, "%.2f")
		SendData("4", altBar, "%.2f")
		SendData("5", altRad, "%.2f")
		SendData("6", 360 - (hsi.ADF * 57.3), "%.2f")
		SendData("7", 360 - (hsi.RMI * 57.3), "%.2f")
		SendData("8", 360 - (hsi.Compass * 57.3), "%.2f")
		SendData("9", engine.RPM.left, "%.2f")
		SendData("10", engine.RPM.right, "%.2f")
		SendData("11", engine.Temperature.left, "%.2f")
		SendData("12", engine.Temperature.right, "%.2f")
		SendData("13", vvi, "%.2f")
		SendData("14", ias, "%.2f")
		SendData("15", distanceToWay, "%.2f")
		SendData("16", 12 + (aoa * 57.3), "%.2f")
		SendData("17", glide, "%.2f")
		SendData("18", side, "%.2f")
	end

end

function ProcessLowImportance(mainPanelDevice)

end
