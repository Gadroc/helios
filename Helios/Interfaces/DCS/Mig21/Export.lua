------------------------------------------------------------------------------
--Needs a real good tidy up, includes exports from CaptZeen, Gadroc and mine--
--Once mig 21 interface complete will go through and condense properly--------
------------------------------------------------------------------------------

gHighImportanceArguments = {}
gLowImportanceArguments = {}
ProcessHighImportance = ProcessNoHighImportance
ProcessLowImportance  = ProcessNoLowImportance

---------------
-- DEBUGGING --
---------------

debug_output_file = nil

----------
-- DATA --
----------

gHost = "127.0.0.1"
gPort = 9089
gExportInterval = 0.1        -- frequency of export events (sec)
gExportLowTickInterval = 2   -- export events call ProcessLowImportance every this many events.

-- Find argument values in the relevant mods\aircrafts\??\Cockpit\Scripts\mainpanel_inits.lua, "arg_number" values.
gA10HighImportanceArguments =
{
	[4] = "%.4f",		-- AOA
	[12] = "%.4f",		-- Variometer
	[13] = "%.4f",		--
	[14] = "%.4f",		--
	[17] = "%.4f",		-- ADI Pitch
	[18] = "%.4f",		-- ADI Bank
	[19] = "%0.1f",		-- ADI Course Warning Flag
	[20] = "%.4f",		-- ADI Bank Steering Bar
	[21] = "%.4f",		-- ADI Pitch Steering Bar
	[23] = "%.4f",		-- ADI Turn Needle
	[24] = "%.4f",		-- ADI Slip Ball
	[25] = "%0.1f",		-- ADI Attitude Warning Flag
	[26] = "%0.1f",		-- ADI Glide-Slope Warning Flag
	[27] = "%.4f",		-- ADI Glide-Slope Indicator
	[32] = "%0.1f",		-- HSI Range Flag
	[33] = "%.4f",		-- HSI Bearing #1
	[34] = "%.4f",		-- HSI Heading
	[35] = "%.4f",		-- HSI Bearing #2
	[36] = "%.4f",		-- HSI Heading Marker
	[40] = "%0.1f",		-- HSI Power Flag
	[41] = "%.4f",		-- HSI Deviation
	[46] = "%0.1f",		-- HSI Bearing Flag
	[47] = "%.4f",		-- HSI Course Arrow
	[48] = "%.4f",		-- Airspeed
	[55] = "%0.1f",		-- AOA Power Flag
	[63] = "%.4f",		-- Standby Attitude Indicator pitch
	[64] = "%.4f",		-- Standby Attitude Indicator bank
	[65] = "%0.1f",		-- Standby Attitude Indicator warning flag
	[70] = "%.4f",		--
	[73] = "%.4f",		--
	[76] = "%.4f",		-- EngineLeftFanSpeed
	[77] = "%.4f",		-- EngineRightFanSpeed
	[78] = "%.4f",		-- EngineLeftCoreSpeedTenth
	[80] = "%.4f",		-- EngineRightCoreSpeedTenth
	[82] = "%.4f",		--
	[83] = "%.4f",		--
	[84] = "%.4f",		-- EngineLeftFuelFlow
	[85] = "%.4f",		-- EngineRightFuelFlow
	[88] = "%.4f",		--
	[89] = "%.4f",		--
	[129] = "%1d",		--
	[178] = "%0.1f",	--
	[179] = "%0.1f",	--
	[181] = "%0.1f",	--
	[182] = "%0.1f",	--
	[185] = "%1d",		--
	[186] = "%1d",		--
	[187] = "%1d",		--
	[188] = "%1d",		--
	[191] = "%0.1f",	--
	[215] = "%0.1f",	--
	[216] = "%0.1f",	--
	[217] = "%0.1f",	--
	[260] = "%0.1f",	--
	[269] = "%.4f",		-- HARS Sync
	[274] = "%.4f",		--
	[281] = "%.4f",		--
	[289] = "%1d",		--
	[372] = "%0.1f",	-- CMSC Missile Launch Indicator
	[373] = "%0.1f",	--
	[374] = "%0.1f",	--
	[404] = "%0.1f",	--
	[480] = "%0.1f",	--
	[481] = "%0.1f",	--
	[482] = "%0.1f",	--
	[483] = "%0.1f",	--
	[484] = "%0.1f",	--
	[485] = "%0.1f",	--
	[486] = "%0.1f",	--
	[487] = "%0.1f",	--
	[488] = "%0.1f",	--
	[489] = "%0.1f",	--
	[490] = "%0.1f",	--
	[491] = "%0.1f",	--
	[492] = "%0.1f",	--
	[493] = "%0.1f",	--
	[494] = "%0.1f",	--
	[495] = "%0.1f",	--
	[496] = "%0.1f",	--
	[497] = "%0.1f",	--
	[498] = "%0.1f",	--
	[499] = "%0.1f",	--
	[500] = "%0.1f",	--
	[501] = "%0.1f",	--
	[502] = "%0.1f",	--
	[503] = "%0.1f",	--
	[504] = "%0.1f",	--
	[505] = "%0.1f",	--
	[506] = "%0.1f",	--
	[507] = "%0.1f",	--
	[508] = "%0.1f",	--
	[509] = "%0.1f",	--
	[510] = "%0.1f",	--
	[511] = "%0.1f",	--
	[512] = "%0.1f",	--
	[513] = "%0.1f",	--
	[514] = "%0.1f",	--
	[515] = "%0.1f",	--
	[516] = "%0.1f",	--
	[517] = "%0.1f",	--
	[518] = "%0.1f",	--
	[519] = "%0.1f",	--
	[520] = "%0.1f",	--
	[521] = "%0.1f",	--
	[522] = "%0.1f",	--
	[523] = "%0.1f",	--
	[524] = "%0.1f",	--
	[525] = "%0.1f",	--
	[526] = "%0.1f",	--
	[527] = "%0.1f",	--
	[540] = "%0.1f",	-- AOA_INDEXER_HIGH
	[541] = "%0.1f",	-- AOA_INDEXER_NORM
	[542] = "%0.1f",	-- AOA_INDEXER_LOW
	[600] = "%0.1f",	--
	[604] = "%.4f",		--
	[606] = "%0.1f",	--
	[608] = "%0.1f",	--
	[610] = "%0.1f",	--
	[612] = "%0.1f",	--
	[614] = "%0.1f",	--
	[616] = "%0.1f",	--
	[618] = "%0.1f",	--
	[619] = "%0.1f",	--
	[620] = "%0.1f",	--
	[647] = "%.4f",		--
	[648] = "%.4f",		--
	[653] = "%.4f",		--
	[654] = "%1d",		--
	[659] = "%0.1f",	--
	[660] = "%0.1f",	--
	[661] = "%0.1f",	--
	[662] = "%0.1f",	--
	[663] = "%0.1f",	--
	[664] = "%0.1f",	--
	[665] = "%0.1f",	--
	[715] = "%.4f",		-- Standby Attitude Indicator manual pitch adjustment
	[730] = "%0.1f",	-- AIR_REFUEL_READY
	[731] = "%0.1f",	-- AIR_REFUEL_LATCHED
	[732] = "%0.1f",	-- AIR_REFUEL_DISCONNECT
	[737] = "%0.1f",	--
	[798] = "%0.1f",	--
	[799] = "%0.1f"		--
}
gA10LowImportanceArguments =
{
	[22] = "%.3f",
	[101] = "%.1f",
	[102] = "%1d",
	[103] = "%1d",
	[104] = "%1d",
	[105] = "%1d",
	[106] = "%1d",
	[107] = "%1d",
	[108] = "%1d",
	[109] = "%1d",
	[110] = "%1d",
	[111] = "%1d",
	[112] = "%1d",
	[113] = "%1d",
	[114] = "%1d",
	[115] = "%.1f",
	[116] = "%.3f",
	[117] = "%1d",
	[118] = "%1d",
	[119] = "%1d",
	[120] = "%1d",
	[121] = "%1d",
	[122] = "%1d",
	[123] = "%1d",
	[124] = "%1d",
	[125] = "%1d",
	[126] = "%1d",
	[127] = "%.1f",
	[130] = "%1d",
	[131] = "%.1f",
	[132] = "%1d",
	[133] = "%.3f",
	[134] = "%1d",
	[135] = "%0.1f",
	[136] = "%.1f",
	[137] = "%0.3f",
	[138] = "%0.1f",
	[139] = "%0.2f",
	[140] = "%0.2f",
	[141] = "%0.2f",
	[142] = "%0.2f",
	[147] = "%.3f",
	[148] = "%1d",
	[149] = "%0.1f",
	[150] = "%.1f",
	[151] = "%0.3f",
	[152] = "%0.1f",
	[153] = "%0.2f",
	[154] = "%0.2f",
	[155] = "%0.2f",
	[156] = "%0.2f",
	[161] = "%0.2f",
	[162] = "%0.1f",
	[163] = "%0.2f",
	[164] = "%0.2f",
	[165] = "%0.2f",
	[166] = "%0.2f",
	[167] = "%0.1f",
	[168] = "%0.1f",
	[169] = "%1d",
	[170] = "%1d",
	[171] = "%.3f",
	[172] = "%.1f",
	[173] = "%.1f",
	[174] = "%1d",
	[175] = "%1d",
	[176] = "%0.1f",
	[177] = "%1d",
	[180] = "%1d",
	[183] = "%1d",
	[184] = "%1d",
	[189] = "%1d",
	[190] = "%.1f",
	[192] = "%.3f",
	[193] = "%.3f",
	[194] = "%0.1f",
	[195] = "%.3f",
	[196] = "%1d",
	[197] = "%.1f",
	[198] = "%.1f",
	[199] = "%0.1f",
	[200] = "%0.1f",
	[201] = "%1d",
	[202] = "%1d",
	[203] = "%1d",
	[204] = "%1d",
	[205] = "%1d",
	[206] = "%1d",
	[207] = "%1d",
	[208] = "%1d",
	[209] = "%0.2f",
	[210] = "%0.2f",
	[211] = "%0.2f",
	[212] = "%0.2f",
	[213] = "%0.2f",
	[214] = "%0.2f",
	[221] = "%.3f",
	[222] = "%1d",
	[223] = "%.3f",
	[224] = "%1d",
	[225] = "%.3f",
	[226] = "%1d",
	[227] = "%.3f",
	[228] = "%1d",
	[229] = "%.3f",
	[230] = "%1d",
	[231] = "%.3f",
	[232] = "%1d",
	[233] = "%.3f",
	[234] = "%1d",
	[235] = "%.3f",
	[236] = "%1d",
	[237] = "%1d",
	[238] = "%.3f",
	[239] = "%0.1f",
	[240] = "%.1f",
	[241] = "%1d",
	[242] = "%1d",
	[243] = "%1d",
	[244] = "%1d",
	[245] = "%1d",
	[246] = "%1d",
	[247] = "%1d",
	[248] = "%0.1f",
	[249] = "%.3f",
	[250] = "%0.1f",
	[251] = "%0.1f",
	[252] = "%0.1f",
	[258] = "%0.2f",
	[259] = "%.1f",
	[261] = "%.3f",
	[262] = "%0.1f",
	[266] = "%1d",
	[267] = "%.1f",
	[268] = "%.3f",
	[270] = "%1d",
	[271] = "%.3f",
	[272] = "%1d",
	[273] = "%1d",
	[275] = "%.1f",
	[276] = "%1d",
	[277] = "%.3f",
	[278] = "%1d",
	[279] = "%1d",
	[280] = "%1d",
	[282] = "%1d",
	[283] = "%1d",
	[284] = "%.3f",
	[287] = "%1d",
	[288] = "%.3f",
	[290] = "%.3f",
	[291] = "%1d",
	[292] = "%.3f",
	[293] = "%.3f",
	[294] = "%1d",
	[295] = "%1d",
	[296] = "%.3f",
	[297] = "%.3f",
	[300] = "%.1f",
	[301] = "%.1f",
	[302] = "%.1f",
	[303] = "%.1f",
	[304] = "%.1f",
	[305] = "%.1f",
	[306] = "%.1f",
	[307] = "%.1f",
	[308] = "%.1f",
	[309] = "%.1f",
	[310] = "%.1f",
	[311] = "%.1f",
	[312] = "%.1f",
	[313] = "%.1f",
	[314] = "%.1f",
	[315] = "%.1f",
	[316] = "%.1f",
	[317] = "%.1f",
	[318] = "%.1f",
	[319] = "%.1f",
	[320] = "%1d",
	[321] = "%1d",
	[322] = "%1d",
	[323] = "%1d",
	[324] = "%1d",
	[325] = "%0.1f",
	[326] = "%.1f",
	[327] = "%.1f",
	[328] = "%.1f",
	[329] = "%.1f",
	[330] = "%.1f",
	[331] = "%.1f",
	[332] = "%.1f",
	[333] = "%.1f",
	[334] = "%.1f",
	[335] = "%.1f",
	[336] = "%.1f",
	[337] = "%.1f",
	[338] = "%.1f",
	[339] = "%.1f",
	[340] = "%.1f",
	[341] = "%.1f",
	[342] = "%.1f",
	[343] = "%.1f",
	[344] = "%.1f",
	[345] = "%.1f",
	[346] = "%1d",
	[347] = "%1d",
	[348] = "%1d",
	[349] = "%1d",
	[350] = "%1d",
	[351] = "%0.1f",
	[352] = "%.1f",
	[353] = "%.1f",
	[354] = "%.1f",
	[355] = "%.1f",
	[356] = "%1d",
	[357] = "%.1f",
	[358] = "%1d",
	[359] = "%.3f",
	[360] = "%0.1f",
	[361] = "%0.1f",
	[362] = "%0.1f",
	[363] = "%0.1f",
	[364] = "%0.1f",
	[365] = "%.1f",
	[366] = "%.1f",
	[367] = "%.3f",
	[368] = "%.3f",
	[369] = "%.1f",
	[370] = "%.1f",
	[371] = "%.1f",
	[375] = "%0.1f",
	[376] = "%0.1f",
	[377] = "%0.1f",
	[378] = "%1d",
	[379] = "%0.1f",
	[380] = "%1d",
	[381] = "%1d",
	[382] = "%1d",
	[383] = "%1d",
	[384] = "%0.1f",
	[385] = "%.1f",
	[386] = "%.1f",
	[387] = "%.1f",
	[388] = "%.1f",
	[389] = "%.1f",
	[390] = "%.1f",
	[391] = "%.1f",
	[392] = "%.1f",
	[393] = "%.1f",
	[394] = "%.1f",
	[395] = "%.1f",
	[396] = "%.1f",
	[397] = "%.1f",
	[398] = "%.1f",
	[399] = "%.1f",
	[400] = "%.1f",
	[401] = "%.1f",
	[402] = "%.1f",
	[403] = "%.1f",
	[405] = "%1d",
	[406] = "%1d",
	[407] = "%1d",
	[408] = "%1d",
	[409] = "%1d",
	[410] = "%.1f",
	[411] = "%.1f",
	[412] = "%.1f",
	[413] = "%.1f",
	[414] = "%.1f",
	[415] = "%.1f",
	[416] = "%.1f",
	[417] = "%.1f",
	[418] = "%.1f",
	[419] = "%.1f",
	[420] = "%.1f",
	[421] = "%.1f",
	[422] = "%.1f",
	[423] = "%.1f",
	[424] = "%1d",
	[425] = "%.1f",
	[426] = "%.1f",
	[427] = "%.1f",
	[428] = "%.1f",
	[429] = "%.1f",
	[430] = "%.1f",
	[431] = "%.1f",
	[432] = "%.1f",
	[433] = "%.1f",
	[434] = "%.1f",
	[435] = "%.1f",
	[436] = "%.1f",
	[437] = "%.1f",
	[438] = "%.1f",
	[439] = "%.1f",
	[440] = "%.1f",
	[441] = "%.1f",
	[442] = "%.1f",
	[443] = "%.1f",
	[444] = "%.1f",
	[445] = "%.1f",
	[446] = "%.1f",
	[447] = "%.1f",
	[448] = "%.1f",
	[449] = "%.1f",
	[450] = "%.1f",
	[451] = "%.1f",
	[452] = "%.1f",
	[453] = "%.1f",
	[454] = "%.1f",
	[455] = "%.1f",
	[456] = "%.1f",
	[457] = "%.1f",
	[458] = "%.1f",
	[459] = "%.1f",
	[460] = "%.1f",
	[461] = "%.1f",
	[462] = "%.1f",
	[463] = "%1d",
	[466] = "%.1f",
	[467] = "%.1f",
	[468] = "%.1f",
	[469] = "%1d",
	[470] = "%.1f",
	[471] = "%.1f",
	[472] = "%1d",
	[473] = "%0.1f",
	[474] = "%1d",
	[475] = "%0.1f",
	[476] = "%1d",
	[477] = "%1d",
	[531] = "%.1f",
	[532] = "%.1f",
	[533] = "%.1f",
	[601] = "%1d",
	[602] = "%1d",
	[603] = "%1d",
	[605] = "%.1f",
	[607] = "%.1f",
	[609] = "%.1f",
	[611] = "%.1f",
	[613] = "%.1f",
	[615] = "%.1f",
	[617] = "%.1f",
	[621] = "%1d",
	[622] = "%0.1f",
	[623] = "%1d",
	[624] = "%.3f",
	[626] = "%.3f",
	[628] = "%.1f",
	[630] = "%.1f",
	[632] = "%.1f",
	[634] = "%.1f",
	[636] = "%0.2f",
	[638] = "%0.2f",
	[640] = "%0.2f",
	[642] = "%0.2f",
	[644] = "%1d",
	[645] = "%0.1f",
	[646] = "%.1f",
	[651] = "%.1f",
	[655] = "%0.1f",
	[704] = "%.3f",
	[705] = "%.3f",
	[711] = "%.1f",
	[712] = "%0.2f",
	[716] = "%1d",		-- Gear Handle Position
	[718] = "%1d",
	[722] = "%.1f",
	[733] = "%1d}",
	[734] = "%1d",
	[735] = "%.1f",
	[772] = "%1d",
	[778] = "%1d",
	[779] = "%1d",
	[780] = "%1d",
	[781] = "%0.1f",
	[782] = "%0.1f",
	[783] = "%0.1f",
	[784] = "%1d",
}

gKa50HighImportanceArguments =
{
	[44]="%0.1f",
	[46]="%0.1f",
	[47]="%0.1f",
	[48]="%0.1f",
	[78]="%0.1f",
	[79]="%0.1f",
	[80]="%0.1f",
	[81]="%0.1f",
	[82]="%0.1f",
	[83]="%0.1f",
	[84]="%0.1f",
	[85]="%0.1f",
	[86]="%0.1f",
	[24]="%.4f",
	[100]="%.4f",
	[101]="%.4f",
	[102]="%0.1f",
	[109]="%0.1f",
	[107]="%.4f",
	[106]="%.4f",
	[111]="%.4f",
	[103]="%.4f",
	[526]="%.4f",
	[108]="%.4f",
	[87]="%.4f",
	[88]="%0.2f",
	[89]="%.4f",
	[112]="%.4f",
	[118]="%.4f",
	[124]="%.4f",
	[115]="%.4f",
	[119]="%0.1f",
	[114]="%0.1f",
	[125]="%0.1f",
	[117]="%0.4f",
	[527]="%0.4f",
	[528]="%0.4f",
	[127]="%.4f",
	[128]="%.4f",
	[116]="%0.1f",
	[121]="%0.1f",
	[53]="%.4f",
	[52]="%.4f",
	[94]="%.4f",
	[93]="%.4f",
	[95]="%0.1f",
	[92]="%0.1f",
	[51]="%.4f",
	[97]="%0.2f",
	[98]="%0.2f",
	[99]="%0.2f",
	[68]="%.4f",
	[69]="%.4f",
	[70]="%.4f",
	[75]="%0.1f",
	[72]="%.4f",
	[531]="%.4f",
	[73]="%.4f",
	[532]="%.4f",
	[142]="%.4f",
	[143]="%.4f",
	[144]="%.4f",
	[145]="%0.1f",
	[133]="%.4f",
	[134]="%.4f",
	[135]="%.4f",
	[136]="%.4f",
	[138]="%.4f",
	[137]="%.4f",
	[139]="%0.1f",
	[140]="%0.1f",
	[392]="%0.1f",
	[393]="%0.1f",
	[394]="%0.1f",
	[395]="%0.1f",
	[388]="%0.1f",
	[389]="%0.1f",
	[390]="%0.1f",
	[391]="%0.1f",
	[63]="%0.1f",
	[64]="%0.1f",
	[61]="%0.1f",
	[62]="%0.1f",
	[59]="%0.1f",
	[60]="%0.1f",
	[170]="%0.1f",
	[175]="%0.1f",
	[172]="%0.1f",
	[165]="%0.1f",
	[171]="%0.1f",
	[176]="%0.1f",
	[166]="%0.1f",
	[164]="%0.1f",
	[178]="%0.1f",
	[173]="%0.1f",
	[177]="%0.1f",
	[211]="%0.1f",
	[187]="%0.1f",
	[204]="%0.1f",
	[213]="%0.1f",
	[11]="%.4f",
	[12]="%.4f",
	[14]="%.4f",
	[167]="%0.1f",
	[180]="%0.1f",
	[179]="%0.1f",
	[188]="%0.1f",
	[189]="%0.1f",
	[206]="%0.1f",
	[212]="%0.1f",
	[205]="%0.1f",
	[181]="%0.1f",
	[190]="%0.1f",
	[207]="%0.1f",
	[183]="%0.1f",
	[182]="%0.1f",
	[191]="%0.1f",
	[208]="%0.1f",
	[184]="%0.1f",
	[200]="%0.1f",
	[209]="%0.1f",
	[185]="%0.1f",
	[202]="%0.1f",
	[201]="%0.1f",
	[210]="%0.1f",
	[186]="%0.1f",
	[203]="%0.1f",
	[159]="%0.1f",
	[150]="%0.1f",
	[161]="%0.1f",
	[15]="%0.1f",
	[16]="%0.1f",
	[17]="%0.1f",
	[18]="%0.1f",
	[19]="%0.1f",
	[20]="%0.1f",
	[21]="%0.1f",
	[22]="%0.1f",
	[23]="%0.1f",
	[50]="%0.1f",
	[25]="%0.1f",
	[28]="%0.1f",
	[26]="%0.1f",
	[27]="%0.1f",
	[31]="%0.1f",
	[32]="%0.1f",
	[33]="%0.1f",
	[34]="%0.1f",
	[582]="%0.1f",
	[541]="%0.1f",
	[542]="%0.1f",
	[315]="%0.1f",
	[519]="%0.1f",
	[316]="%0.1f",
	[520]="%0.1f",
	[317]="%0.1f",
	[521]="%0.1f",
	[318]="%0.1f",
	[313]="%0.1f",
	[314]="%0.1f",
	[522]="%0.1f",
	[319]="%0.1f",
	[320]="%0.1f",
	[321]="%0.1f",
	[322]="%0.1f",
	[323]="%0.1f",
	[330]="%0.1f",
	[332]="%0.1f",
	[331]="%0.1f",
	[333]="%0.1f",
	[334]="%0.1f",
	[375]="%0.1f",
	[419]="%0.1f",
	[577]="%.3f",
	[574]="%.2f",
	[575]="%.2f",
	[576]="%.2f",
	[437]="%0.1f",
	[438]="%0.1f",
	[439]="%0.1f",
	[440]="%0.1f",
	[441]="%0.1f",
	[163]="%0.1f",
	[162]="%0.1f",
	[168]="%0.1f",
	[169]="%0.1f",
	[174]="%0.1f",
	[6]="%.4f",
	[586]="%0.1f",
	[261]="%0.1f",
	[461]="%0.1f",
	[237]="%0.1f",
	[239]="%0.1f",
	[568]="%0.1f",
	[241]="%0.1f",
	[243]="%0.1f",
	[244]="%0.1f",
	[245]="%0.1f",
	[592]="%.4f",
	[234]="%0.2f",
	[235]="%0.2f",
	[252]="%.4f",
	[253]="%.4f",
	[254]="%.4f",
	[255]="%.4f",
	[256]="%.4f",
	[257]="%.4f",
	[469]="%0.1f",
	[470]="%0.1f",
	[471]="%.4f",
	[472]="%.4f",
	[473]="%.4f",
	[474]="%.4f",
	[475]="%.4f",
	[476]="%.4f",
	[342]="%0.1f",
	[339]="%0.4f",
	[594]="%0.4f",
	[337]="%0.4f",
	[596]="%0.4f"
}
gKa50LowImportanceArguments =
{
	[110]="%.1f",
	[113]="%.1f",
	[54]="%1d",
	[56]="%1d",
	[57]="%1d",
	[55]="%.1f",
	[96]="%.1f",
	[572]="%.1f",
	[45]="%.1f",
	[230]="%1d",
	[131]="%.1f",
	[132]="%.1f",
	[616]="%.1f",
	[512]="%.1f",
	[513]="%.1f",
	[514]="%.1f",
	[515]="%.1f",
	[516]="%.1f",
	[523]="%.1f",
	[517]="%.3f",
	[130]="%0.1f",
	[8]="%.3f",
	[9]="%1d",
	[7]="%.1f",
	[510]="%0.1f",
	[387]="%1d",
	[402]="%.1f",
	[396]="%1d",
	[403]="%1d",
	[399]="%1d",
	[400]="%0.1f",
	[398]="%1d",
	[397]="%.1f",
	[404]="%1d",
	[406]="%.3f",
	[407]="%.3f",
	[405]="%.3f",
	[408]="%0.1f",
	[409]="%1d",
	[382]="%0.1f",
	[383]="%1d",
	[381]="%0.2f",
	[384]="%.1f",
	[385]="%.1f",
	[386]="%0.1f",
	[442]="%.1f",
	[65]="%1d",
	[66]="%1d",
	[67]="%1d",
	[146]="%0.1f",
	[147]="%0.1f",
	[539]="%1d",
	[151]="%1d",
	[153]="%1d",
	[154]="%0.1f",
	[156]="%.1f",
	[35]="%.1f",
	[583]="%1d",
	[584]="%.1f",
	[36]="%0.1f",
	[37]="%0.1f",
	[38]="%.1f",
	[39]="%.1f",
	[41]="%.1f",
	[43]="%.1f",
	[42]="%.1f",
	[40]="%.1f",
	[496]="%1d",
	[497]="%1d",
	[498]="%1d",
	[499]="%1d",
	[312]="%0.1f",
	[303]="%0.1f",
	[304]="%0.1f",
	[305]="%0.1f",
	[306]="%0.1f",
	[307]="%0.1f",
	[308]="%0.1f",
	[309]="%0.1f",
	[310]="%0.1f",
	[311]="%0.1f",
	[324]="%0.1f",
	[325]="%1d",
	[326]="%1d",
	[327]="%.3f",
	[328]="%0.1f",
	[329]="%0.1f",
	[335]="%0.1f",
	[336]="%0.1f",
	[355]="%.1f",
	[354]="%1d",
	[353]="%.3f",
	[356]="%1d",
	[357]="%0.1f",
	[371]="%0.1f",
	[372]="%.3f",
	[373]="%.1f",
	[374]="%1d",
	[376]="%.1f",
	[377]="%.1f",
	[378]="%.1f",
	[379]="%.1f",
	[380]="%1d",
	[418]="%.1f",
	[417]="%1d",
	[421]="%1d",
	[422]="%1d",
	[420]="%1d",
	[423]="%1d",
	[432]="%1d",
	[431]="%0.1f",
	[436]="%1d",
	[433]="%1d",
	[435]="%1d",
	[434]="%1d",
	[412]="%.1f",
	[413]="%.1f",
	[414]="%.1f",
	[415]="%0.1f",
	[416]="%0.1f",
	[428]="%0.2f",
	[554]="%1d",
	[555]="%1d",
	[556]="%1d",
	[301]="%0.1f",
	[224]="%.1f",
	[262]="%1d",
	[263]="%1d",
	[543]="%1d",
	[544]="%1d",
	[264]="%1d",
	[265]="%1d",
	[267]="%1d",
	[268]="%1d",
	[269]="%1d",
	[270]="%01.f",
	[271]="%1d",
	[272]="%1d",
	[273]="%1d",
	[274]="%1d",
	[275]="%1d",
	[276]="%1d",
	[277]="%1d",
	[278]="%1d",
	[279]="%1d",
	[280]="%1d",
	[281]="%1d",
	[282]="%1d",
	[283]="%1d",
	[284]="%1d",
	[285]="%1d",
	[286]="%1d",
	[287]="%1d",
	[288]="%1d",
	[289]="%1d",
	[547]="%1d",
	[548]="%1d",
	[214]="%1d",
	[215]="%1d",
	[216]="%1d",
	[217]="%1d",
	[462]="%0.1f",
	[460]="%.1f",
	[220]="%1d",
	[221]="%1d",
	[218]="%1d",
	[219]="%1d",
	[222]="%1d",
	[229]="%0.1f",
	[228]="%1d",
	[296]="%1d",
	[297]="%0.1f",
	[290]="%1d",
	[291]="%1d",
	[292]="%1d",
	[293]="%1d",
	[294]="%1d",
	[569]="%1d",
	[295]="%0.1f",
	[570]="%0.1f",
	[457]="%.1f",
	[458]="%.1f",
	[459]="%.1f",
	[300]="%1d",
	[299]="%1d",
	[298]="%1d",
	[236]="%.1f",
	[238]="%.1f",
	[240]="%.1f",
	[242]="%.1f",
	[248]="%0.1f",
	[249]="%0.1f",
	[250]="%1d",
	[246]="%1d",
	[247]="%1d",
	[258]="%0.1f",
	[259]="%1d",
	[483]="%0.1f",
	[484]="%0.1f",
	[485]="%1d",
	[486]="%1d",
	[489]="%.1f",
	[490]="%1d",
	[491]="%1d",
	[492]="%1d",
	[487]="%1d",
	[488]="%1d",
	[452]="%1d",
	[453]="%1d",
	[340]="%.3f",
	[341]="%1d",
	[338]="%.3f"
}

gMIG21HighImportanceArguments =
{
	[110]="%.3f",		--accelerometer guage
	[113]="%.3f",		--accelterometer max g
	[114]="%.3f",		--accelterometer min g
	[105]="%.4f",		--UUA (aoa) meter
	[52]="%.3f",		--fuel gauge
	[50]="%.3f",		--Engine rpm 1
	[670]="%.3f",		--Engine rpm 2 
	[100]="%.4f",		--IAS
	[101]="%.3f",		--TAS
	[102]="%.3f",		--M
	[66]="%.3f",		--Nosecone position (UPES3)
	[108]="%.4f",		--kpp bank
	[109]="%.4f",		--kpp pitch
	[565]="%.3f",		--kpp bank steering bar
	[566]="%.3f",		--kpp pitch steering bar
	[51]="%.3f",		--Engine exhaust temp
	[106]="%.4f",		--da200 vvi
	[31]="%.3f",		--da200 slip
	[107]="%.4f",		--da200 turn
	[104]="%.3f",		--Baro Alt meters
	[112]="%.3f",		--Baro Alt Kilometers
	[652] = "%.3f",		--Baro Alt Triangle KM
	[658] = "%.3f",		--Baro Alt Triangle M
	[590]="%.3f",		--RSBN_NPP_kurs_needle (used for KPP aux too) course
	[589]="%.3f",		--RSBN_NPP_glisada_needle (used for kpp aux too) glideslope
	[111]="%.4f",		--NPP Heading
	[68]="%.4f",		--NPP commanded course
	[36]="%.4f",		--NPP Heading (Ark rsbn needle)
	[103] = "%.3f",		--Radio Altimeter
	[64] = "%.3f",		--ARU3VM
	[126] = "%.3f",		--Hydraulic Pressure Main
	[125] = "%.3f",		--Hydraulic Pressure Secondary
	[124] = "%.3f",		--DC Voltmeter
	[627] = "%.3f",		--Oil Pressure gauge
	[59] = "%.3f",		--Pilot O2 level
	[60] = "%1d",		--Pilot O2 lungs
	[58] = "%.4f",		--O2 Pressure
	[413] = "%.4f",		--Main airpressure
	[414] = "%.4f",		--Aux airpressure??? this doesnt change??
	[357] = "%.3f",		--RSBN Distance singles
	[356] = "%.3f",		--RSBN Distance Tens
	[355] = "%.3f",		--RSBN Distance Hundreds
	[61] = "%.4f",		--Engine oxygen feed meter
	[55] = "%.3f",		--Battery capacity meter gauge
}

gMIG21LowImportanceArguments =
{
	[211] = "%.3f",		--Radio Channel
	[351] = "%.3f",		--RSBN Nav Channel
	[352] = "%.3f",		--RSBN Land Channel 
	[192] = "%.3f",		--SRZO Channel
	[587] = "%1d",		--RSBN_NPP_kurs_blinker (course blinker) K flag
	[588] = "%1d",		--RSBN_NPP_glisada_blinker G flag
	[173] = "%1d",		-- Radio on/off
	[208] = "%1d",		-- Radio Compass Sound on/off
	[209] = "%1d",		-- Squelch on/off
	[304] = "%1d",		--drag chute safety cover
	[165] = "%1d",		--Battery on/off
	[155] = "%1d",		--Battery heat
	[166] = "%1d",		--DC generator
	[169] = "%1d",		--AC Generator On/Off
	[153] = "%1d",		--PO-750 Inverter #1
	[154] = "%1d",		--PO-750 Inverter #2
	[164] = "%1d",		--Emergency Inverter
	[162] = "%1d",		--Giro, NPP, SAU, RLS Signal, KPP Power
	[163] = "%1d",		--DA-200 Signal, Giro, NPP, RLS, SAU Power On/Off
	[159] = "%1d",		--Fuel Tanks 3rd Group, Fuel Pump
	[160] = "%1d",		--Fuel Tanks 1st Group, Fuel Pump
	[161] = "%1d",		--Drain Fuel Tank, Fuel Pump
	[302] = "%1d",		--apu on/off
	[288] = "%1d",		--Engine Cold / Normal Start
	[301] = "%1d",		--Engine Emergency Air Start
	[229] = "%1d",		--Pitot tube Selector Main/Emergency
	[279] = "%1d",		--Pitot tube/Periscope/Clock Heat
	[280] = "%1d",		--Secondary Pitot Tube Heat
	[308] = "%1d",		--Anti surge doors - Auto/Manual
	[300] = "%1d",		--Afterburner/Maximum Off/On
	[320] = "%1d",		--Emergency Afterburner Off/On
	[303] = "%1d",		--Fire Extinguisher Off/On
	[324] = "%1d",		--Fire Extinguisher Cover
	[325] = "%1d",		--Fire Extinguisher Button
	[174] = "%1d",		--ARK On/Off
	[197] = "%1d",		--ARK Mode - Antenna / Compass
	[254] = "%1d",		--Marker Far/Near
	[176] = "%1d",		--RSBN On/Off
	[340] = "%1d",		--RSBN / ARK switch
	[367] = "%1d",		--RSBN Bearing
	[368] = "%1d",		--RSBN Distance
	[179] = "%1d",		--SAU on/off
	[180] = "%1d",		--SAU Pitch on/off
	[344] = "%1d",		--SAU Preset - Limit Altitude
	[202] = "%1d",		--SPO-10 RWR On/Off
	[188] = "%1d",		--SRZO IFF Coder/Decoder On/Off
	[346] = "%1d",		--IFF System 'Type 81' On/Off
	[190] = "%1d",		--Emergency Transmitter Cover
	[191] = "%1d",		--Emergency Transmitter On/Off
	[427] = "%1d",		--SRZO Self Destruct Cover
	[428] = "%1d",		--SRZO Self Destruct
	[200] = "%1d",		--SOD IFF On/Off
	[199] = "%1d",		--SOD Identify
	[207] = "%1d",		--Locked Beam On/Off
	[167] = "%1d",		--SPRD (RATO) System On/Off
	[168] = "%1d",		--SPRD (RATO) Drop System On/Off
	[252] = "%1d",		--SPRD (RATO) Start Cover
	[317] = "%1d",		--SPRD (RATO)t Drop Cover
	[293] = "%1d",		--SPS System Off/On
	[295] = "%1d",		--ARU System - Manual/Auto
	[299] = "%1d",		--ABS off/on
	[238] = "%1d",		--Nosegear Brake Off/On
	[237] = "%1d",		--Emergency brake
	[326] = "%1d",		--Gear Handle Fixator
	[281] = "%1d",		--Nose Gear Emergency Release Handle
	[311] = "%1d",		--Flaps Neutral
	[312] = "%1d",		--Flaps Take-Off
	[313] = "%1d",		--Flaps Landing
	[172] = "%1d",		--Trimmer On/Off
	[379] = "%1d",		--Trimmer Pitch Up/Down
	[315] = "%1d",		--Radio PTT 
	[376] = "%1d",		--SAU cancel current mode
	[377] = "%1d",		--SAU - Recovery
	[378] = "%1d",		--Lock Target
	[170] = "%1d",		--Nosecone On/Off
	[309] = "%1d",		--Nosecone Control - Manual/Auto
	[236] = "%.4f",		--Nosecone manual position controller. Also used as input for Manual needle
	[291] = "%1d",		--Engine Nozzle 2 Position Emergency Control
	[171] = "%1d",		--Hydro Emergency Hydraulic Pump On/Off
	[319] = "%1d",		--Aileron Booster - Off/On
	[177] = "%1d",		--KPP Main/Emergency
	[260] = "%1d",		--KPP Set
	[178] = "%1d",		--NPP On/Off
	[175] = "%1d",		--Radio Altimeter/Marker On/Off
	[285] = "%1d",		--Helmet Air Condition Off/On
	[286] = "%1d",		--Emergency Oxygen Off/On
	[287] = "%1d",		--Mixture/Oxygen select
	[328] = "%1d",		--Hermetize Canopy
	[329] = "%1d",		--Secure Canopy - Canopy Lock Handle
	[224] = "%1d",		--Canopy Emergency Release Handle
	[431] = "%1d",		--Canopy Ventilation System
	[186] = "%1d",		--ASP Optical sight On/Off
	[241] = "%1d",		--ASP Main Mode - Manual/Auto
	[242] = "%1d",		--ASP Mode - Bombardment/Shooting
	[243] = "%1d",		--ASP Mode - Missiles-Rockets/Gun
	[244] = "%1d",		--ASP Mode - Giro/Missile
	[249] = "%1d",		--ASP Pipper On/Off
	[250] = "%1d",		--ASP Fix net On/Off
	[384] = "%1d",		--TDC Range / Pipper Span control -stick
	[181] = "%1d",		--Missiles - Rockets Heat On/Off
	[182] = "%1d",		--Missiles - Rockets Launch On/Off
	[183] = "%1d",		--Pylon 1-2 Power On/Off
	[184] = "%1d",		--Pylon 3-4 Power On/Off
	[185] = "%1d",		--GS-23 Gun On/Off
	[187] = "%1d",		--Guncam On/Off
	[227] = "%1d",		--Tactical Drop Cover
	[278] = "%1d",		--Tactical Drop switch
	[275] = "%1d",		--Emergency Missile/Rocket Launcher Cover
	[256] = "%1d",		--Drop Wing Fuel Tanks Cover
	[386] = "%1d",		--Drop Center Fuel Tank
	[269] = "%1d",		--Drop Payload - Outer Pylons Cover
	[271] = "%1d",		--Drop Payload - Inner Pylons Cover
	[230] = "%.1f",		--Weapon Mode - Air/Ground
	[381] = "%1d",		--Fire Gun - stick - virtual switch only
	[382] = "%1d",		--Release Weapon
	[383] = "%1d",		--Release Weapon Cover
	[306] = "%1d",		--Helmet Heat - Manual/Auto
	[369] = "%1d",		--Helmet visor - off/on
	[193] = "%1d",		--SARPP-12 Flight Data Recorder On/Off
	[290] = "%1d",		--Seat Height Down/Neutral/Up
	[264] = "%1d",		--Mech clock left lever
	[268] = "%1d",		--Mech clock right lever
	[292] = "%1d",		--Cockpit Air Condition Off/Cold/Auto/Warm
	[235] = "%1d",		--Weapon Selector
	[231] = "%.1f",		--Weapon Mode - IR Missile/Neutral/SAR Missile
	--Dummy button switches section may not be required============
	[632] = "%1d",		--Radar emission - Cover
	[633] = "%1d",		--Radar emission - Combat/Training
	[635] = "%1d",		--Electric Bus Nr.1 - Cover
	[636] = "%1d",		--Electric Bus Nr.1
	[637] = "%1d",		--Electric Bus Nr.2
	[638] = "%1d",		--1.7 Mach Test Button - Cover
	[644] = "%1d",		--Ejection Seat Emergency Oxygen
	[645] = "%1d",		--UK-2M Mic Amplifier M/L
	[646] = "%1d",		--UK-2M Mic Amplifier GS/KM
	[648] = "%1d",		--Harness Separation
	[650] = "%1d",		--Harness Loose/Tight
	[651] = "%1d",		--Throttle Fixation
	--============================================
	[387] = "%1d",		--Nuke control -Emergency Jettison
	[388] = "%1d",		--Nuke control, Emergency Jettison Armed / Not Armed
	[389] = "%1d",		--Nuke control Tactical Jettison
	[390] = "%1d",		--Special AB / Missile-Rocket-Bombs-Cannon
	[391] = "%1d",		--nuke control Brake Chute
	[392] = "%1d",		--nuke control Detonation Air / Ground
	[393] = "%1d",		--SPS Control On / Off
	[394] = "%1d",		--sps control Transmit / Receive
	[395] = "%1d",		--sps control Program I / II
	[396] = "%1d",		--sps control Continuous / Continuous
	[397] = "%1d",		--sps test
	[398] = "%1d",		--sps Dispenser Auto / Manual
	[399] = "%1d",		--sps Off / Parallel / Full
	[400] = "%1d",		--sps Manual Activation button - Cover
	[420] = "%1d",		--weapon control GUV Manual Activation button - Cover
	[421] = "%1d",		--GUV MAIN GUN / UPK Guns
	[422] = "%1d",		--GUV LOAD 1
	[425] = "%1d",		--GUV LOAD 2
	[424] = "%1d",		--GUV LOAD 3
	[240] = "%1d",		--rsbn mode
	[189] = "%1d",		--ark zone
	--cockpit lights/warnings
	[9] = "%1d",		--GEAR_NOSE_UP_LIGHT
	[12] = "%1d",		--GEAR_NOSE_DOWN_LIGHT
	[10] = "%1d",		--GEAR_LEFT_UP_LIGHT
	[13] = "%1d",		--GEAR_LEFT_DOWN_LIGHT
	[11] = "%1d",		--GEAR_RIGHT_UP_LIGHT
	[14] = "%1d",		--GEAR_RIGHT_DOWN_LIGHT
	[548] = "%1d",		--RSBN_azimut_korekcija_LIGHT (rsbn azimuth correction light)
	[549] = "%1d",		--RSBN_dalnost_korekcija_LIGHT (range correction)
	[500] = "%1d",		--LOW_ALT_LIGHT
	[537] = "%1d",		--AOA_WARNING_LIGHT
	[535] = "%1d",		--KPP_ARRETIR_light (arrested)
	[519] = "%1d",		--TRIMMER_light
	[510] = "%1d",		--DC_GEN_LIGHT
	[511] = "%1d",		--AC_GEN_LIGHT
	[501] = "%1d",		--FUEL_PODC
	[502] = "%1d",		--FUEL_1GR
	[503] = "%1d",		--FUEL_450
	[504] = "%1d",		--Fuel 3GR light
	[505] = "%1d",		--Fuel PODW light
	[506] = "%1d",		--Fuel Rashod (Expence) light
	[509] = "%1d",		--START_DEVICE_ZAZIG_LIGHT (doused)
	[507] = "%1d",		--FORSAZ_1_LIGHT (afterburner)
	[508] = "%1d",		--FORSAZ_2_LIGHT (afterburner)
	[512] = "%1d",		--Nozzle light
	[517] = "%1d",		--Cone light
	[513] = "%1d",		--Oil light
	[534] = "%1d",		--Fire light
	[515] = "%1d",		--Check Hydraulic pressure light
	[514] = "%1d",		--Check Buster pressure light
	[541] = "%1d",		--Canopy warning light
	[542] = "%1d",		--SORC light
	[407] = "%1d",		--check state light
	[516] = "%1d",		--marker light
	[518] = "%1d",		--Stabilizator light
	[520] = "%1d",		--Check gear light
	[521] = "%1d",		--Flaps light
	[522] = "%1d",		--Airbrake light
	[523] = "%1d",		--Central Pylon light
	[524] = "%1d",		--Rato L light
	[525] = "%1d",		--Rato R light
	[526] = "%1d",		--Pylon 1 On light
	[527] = "%1d",		--Pylon 2 On light
	[528] = "%1d",		--Pylon 3 On light
	[529] = "%1d",		--Pylon 4 On light
	[530] = "%1d",		--Pylon 1 Off light
	[531] = "%1d",		--Pylon 2 Off light
	[532] = "%1d",		--Pylon 3 Off light
	[533] = "%1d",		--Pylon 4 Off light
	[539] = "%1d",		--ASP Target acquired light
	[581] = "%1d",		--IAB light 1
	[582] = "%1d",		--IAB light 2
	[583] = "%1d",		--IAB light 3
	[593] = "%1d",		--SPS Illumination
	[546] = "%1d",		--SAU Stabilisation light
	[547] = "%1d",		--SAU feeding light(SAU_privedenie_LIGHT)
	[544] = "%1d",		--SAU landing command light
	[545] = "%1d",		--SAU landing auto light
	[550] = "%1d",		--GUN gotovn LIGHT
	[613] = "%1d",		--ASP backlight on
	[205] = "%1d",		--Radar off/prep/on
	[206] = "%1d",		--Radar off/comp/on (low alt)
	[653] = "%1d",		--alt press reset
	[262] = "%.4f",		--alt press axis. also used as input for qfe card.
	[601] = "%1d",		--spo lf
	[602] = "%1d",		--spo rf
	[603] = "%1d",		--spo rb
	[604] = "%1d",		--spo lb
	[605] = "%1d",		--spo muted
	[258] = "%1d",		--npp adjust
	[259] = "%1d",		--kpp cage

}

gP51HighImportanceArguments = 
{
}
gP51LowImportanceArguments = 
{
}

-- Lookup tables for weapons store type display
gKa50StationTypes =
{
	["9A4172"] = "NC",
	["S-8KOM"] = "HP",
	["S-13"] = "HP",
	["UPK-23-250"] = "NN",
	["AO-2.5RT"] = "A6",
	["PTAB-2.5KO"] = "A6",
	["FAB-250"] = "A6",
	["FAB-500"] = "A6"
}

-- State data
gKa50Trigger = 0

gCurrentAircraft = "none"  -- not set yet.
gFlamingCliffsAircraft = false
gHawk = false
gMiG21 = false
gKnownAircraft = false


--------------------------------------------------------------------------------------------------------------
-- SCRIPT --
-------------------------------------------------------------------------------------------------------------------

os.setlocale("ISO-8559-1", "numeric")

-- Simulation id
gSimID = string.format("%08x*",os.time())

-- State data for export
gPacketSize = 0
gSendStrings = {}
gLastData = {}

-- Frame counter for non important data
gTickCount = 0



----------------------------------------
-- AIRCRAFT-SPECIFIC EXPORT FUNCTIONS --
----------------------------------------

-----------------------------
-- HIGH IMPORTANCE EXPORTS --
-- done every export event --
-----------------------------

-- Pointed to by ProcessHighImportance, if the player aircraft is an A-10
function ProcessA10HighImportance(mainPanelDevice)
	-- Send Altimeter Values
	SendData(2051, string.format("%0.4f;%0.4f;%0.5f", mainPanelDevice:get_argument_value(52),
		mainPanelDevice:get_argument_value(53),
		mainPanelDevice:get_argument_value(51)))
	-- Barometric Pressure Setting
	SendData(2059, string.format("%0.2f;%0.2f;%0.2f;%0.3f", mainPanelDevice:get_argument_value(56),
		mainPanelDevice:get_argument_value(57),
		mainPanelDevice:get_argument_value(58),
		mainPanelDevice:get_argument_value(59)))
	-- Calcuate HSI Value
	SendData(2029, string.format("%0.2f;%0.2f;%0.4f", mainPanelDevice:get_argument_value(29),
		mainPanelDevice:get_argument_value(30),
		mainPanelDevice:get_argument_value(31)))
	-- Calculate Total Fuel
	SendData(2090, string.format("%0.2f;%0.2f;%0.5f", mainPanelDevice:get_argument_value(90),
		mainPanelDevice:get_argument_value(91),
		mainPanelDevice:get_argument_value(92)))
end


-- Pointed to by ProcessHighImportance, if the player aircraft is something else
function ProcessNoHighImportance(mainPanelDevice)
end


-----------------------------------------------------
-- LOW IMPORTANCE EXPORTS                          --
-- done every gExportLowTickInterval export events --
-----------------------------------------------------


-- Pointed to by ProcessLowImportance, if the player aircraft is an A-10
function ProcessA10LowImportance(mainPanelDevice)
	-- Get Radio Frequencies
	local lUHFRadio = GetDevice(54)
	SendData(2000, string.format("%7.3f", lUHFRadio:get_frequency()/1000000))

	  -- TACAN Channel
	SendData(2263, string.format("%0.2f;%0.2f;%0.2f", mainPanelDevice:get_argument_value(263),
		mainPanelDevice:get_argument_value(264),
		mainPanelDevice:get_argument_value(265)))
end


-- Pointed to by ProcessLowImportance, if the player aircraft is a Ka-50
function ProcessKa50LowImportance(mainPanelDevice)
	local lWeaponSystem = GetDevice(12)
	local lEKRAN = GetDevice(10)
	local lCannonAmmoCount = " "
	local lStationNumbers = lWeaponSystem:get_selected_weapon_stations()
	local lStationCount = " "
	local lStationType = " "
	local lTargetingPower = mainPanelDevice:get_argument_value(433)
	local lTrigger = mainPanelDevice:get_argument_value(615)
	if lTrigger == 0 then
		gKa50Trigger = 1
	end
	if lTrigger == -1 then
		gKa50Trigger = 0
	end

	if lTargetingPower == 1 then
		lCannonAmmoCount = string.format("%02d",string.match(lWeaponSystem:get_selected_gun_ammo_count() / 10,"(%d+)"))

		if #lStationNumbers ~= 0 and gKa50Trigger == 0 then
			lStationCount = 0
			for i=1,#lStationNumbers do
				lStationCount = lStationCount + lWeaponSystem:get_weapon_count_on_station(lStationNumbers[i])
			end

			lStationCount = string.format("%02d", lStationCount);

			lStationType = gKa50StationTypes[lWeaponSystem:get_weapon_type_on_station(lStationNumbers[1])]
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


-- Pointed to by ProcessLowImportance, if the player aircraft is something else
function ProcessNoLowImportance(mainPanelDevice)
end


-----------------------------------------
-- FLAMING CLIFFS AIRCRAFT             --
-- FC aircraft don't support GetDevice --
-----------------------------------------
function ProcessFCExports ()

	local myData = LoGetSelfData()

	if (myData) then

		local altBar = LoGetAltitudeAboveSeaLevel()
		local altRad = LoGetAltitudeAboveGroundLevel()
		local pitch, bank, yaw = LoGetADIPitchBankYaw()
		local engine = LoGetEngineInfo()
		local hsi    = LoGetControlPanel_HSI()
		local vvi = LoGetVerticalVelocity()
		local ias = LoGetIndicatedAirSpeed()
		local route = LoGetRoute()
		local aoa = LoGetAngleOfAttack()
		local accelerometer = LoGetAccelerationUnits()
		local glide = LoGetGlideDeviation()
		local side = LoGetSideDeviation()
		local distanceToWay = 999
		local navInfo = LoGetNavigationInfo()

		if engine then
					local fuelLeftKG = engine.fuel_internal  + engine.fuel_external
					local fuelConsumptionKGsec = engine.FuelConsumption.left + engine.FuelConsumption.right
		end

		if (myData and route) then -- if neither are nil
			local myLoc = LoGeoCoordinatesToLoCoordinates(myData.LatLongAlt.Long, myData.LatLongAlt.Lat)
			distanceToWay = math.sqrt((myLoc.x - route.goto_point.world_point.x)^2 + (myLoc.z -  route.goto_point.world_point.z)^2)
		end

		if (myData) then
			if myData.Name=="F-15C" then   --- F-15C  prepared for Capt Zeen helios profile

				SendData("1", string.format("%.2f", pitch * 57.3) )
				SendData("2", string.format("%.2f", bank * 57.3) )
				SendData("3", string.format("%.2f", yaw * 57.3) )
				SendData("4", string.format("%.2f", altBar) )
				SendData("7", string.format("%.2f", 360 - (hsi.RMI_raw * 57.3) ))
				SendData("8", string.format("%.2f", (myData.Heading * 57.3)) )
				SendData("13", string.format("%.2f", vvi) )
				SendData("16", string.format("%.2f", aoa * 57.3 ) )
				SendData("17", string.format("%.2f", glide) )
				SendData("18", string.format("%.2f", side) )

				-- prepare pairs of data to send more info to helios........
				SendData("9", string.format("%.4f", (math.floor(engine.fuel_internal) + (engine.RPM.left /1000))  ) ) --fuel int + rpm left in rpm.left import data
				SendData("10", string.format("%.4f", (math.floor(engine.fuel_external) + (engine.RPM.right /1000))  ) ) --fuel ext + rpm right in rpm.right import data
				SendData("11", string.format("%.4f", (math.floor(engine.fuel_internal+engine.fuel_external) + (engine.Temperature.left/1000)) ) ) --fuel TOTAL +  eng temp left in eng temp left import data
				if math.floor(route.goto_point.world_point.x)<0 then
					SendData("12", string.format("%.4f", (math.floor(route.goto_point.world_point.x) - (engine.Temperature.right/1000)) ) ) --x coord +  eng temp left in eng temp rifgt import data
					else
					SendData("12", string.format("%.4f", (math.floor(route.goto_point.world_point.x) + (engine.Temperature.right/1000)) ) ) --x coord +  eng temp left in eng temp rifgt import data
				end

				SendData("14", string.format("%.5f", (math.floor(distanceToWay) + (ias /10000))  ) ) --distance to way + ias in IAS import data
				-- end of pairs

				SendData("5", string.format("%.2f", accelerometer.y ))   -- acelerometer in Radar altidud import data
				SendData("15", string.format("%s", navInfo.SystemMode.master .." / ".. navInfo.SystemMode.submode))  -- HUD MODE and SUBMODE in distancetoway import data
				SendData("6", string.format("%.2f", 360 - (hsi.ADF_raw * 57.3)) )  --HSI in f15 format

			else			--- OTHER AIRPLANES
				SendData("1", string.format("%.2f", pitch * 57.3) )
				SendData("2", string.format("%.2f", bank * 57.3) )
				SendData("3", string.format("%.2f", yaw * 57.3) )
				SendData("4", string.format("%.2f", altBar) )
				SendData("5", string.format("%.2f", altRad) )
				SendData("6", string.format("%.2f", (360 - (hsi.ADF_raw * 57.3))+(360 - (myData.Heading * 57.3)) ))
				SendData("7", string.format("%.2f", 360 - (hsi.RMI_raw * 57.3) ))
				SendData("8", string.format("%.2f", (myData.Heading * 57.3)) )
				SendData("9", string.format("%.2f", engine.RPM.left) )
				SendData("10", string.format("%.2f", engine.RPM.right) )
				SendData("11", string.format("%.2f", engine.Temperature.left) )
				SendData("12", string.format("%.2f", engine.Temperature.right) )
				SendData("13", string.format("%.2f", vvi) )
				SendData("14", string.format("%.2f", ias) )
				SendData("15", string.format("%.2f", distanceToWay) )
				SendData("16", string.format("%.2f", aoa * 57.3 ) )
				SendData("17", string.format("%.2f", glide) )
				SendData("18", string.format("%.2f", side) )
			end
		end

		FlushData()

	else
		--if debug_output_file then
		--	debug_output_file:write("ProcessFCExports(4.2) called but myData is nil\r\n")
		--end
	end

end

------------------------------------------------
-- P-51D and TF-51D exported as a FC aircraft --
-- 				by Capt Zeen  --
------------------------------------------------

function ProcessP51Exports ()

		
						-- read from main panel
						local MainPanel = GetDevice(0)
							local AirspeedNeedle = MainPanel:get_argument_value(11)*1000
							local Altimeter_10000_footPtr = MainPanel:get_argument_value(96)*100000
							local Variometer = MainPanel:get_argument_value(29)   
							local TurnNeedle = MainPanel:get_argument_value(27)   
							local Slipball = MainPanel:get_argument_value(28)
							local CompassHeading = MainPanel:get_argument_value(1) 
							local CommandedCourse = MainPanel:get_argument_value(2) 							
							local Manifold_Pressure = MainPanel:get_argument_value(10) 
							local Engine_RPM = MainPanel:get_argument_value(23)
							local AHorizon_Pitch = MainPanel:get_argument_value(15) 
							local AHorizon_Bank = MainPanel:get_argument_value(14) 
							local AHorizon_PitchShift = MainPanel:get_argument_value(16) * 10.0 * math.pi/180.0
							local AHorizon_Caged = MainPanel:get_argument_value(20) 
							local GyroHeading = MainPanel:get_argument_value(12) 
							local vaccum_suction = MainPanel:get_argument_value(9)
							local carburator_temp = MainPanel:get_argument_value(21)
							local coolant_temp = MainPanel:get_argument_value(22)
							local Acelerometer = MainPanel:get_argument_value(175)
							local OilTemperature = MainPanel:get_argument_value(30)
							local OilPressure = MainPanel:get_argument_value(31)
							local FuelPressure = MainPanel:get_argument_value(32)
							local Clock_hours = MainPanel:get_argument_value(4)
							local Clock_minutes = MainPanel:get_argument_value(5)
							local Clock_seconds = MainPanel:get_argument_value(6)
							local LandingGearGreenLight = MainPanel:get_argument_value(80) 
							local LandingGearRedLight = MainPanel:get_argument_value(82)
							local Hight_Blower_Lamp = MainPanel:get_argument_value(59) 						
							local Acelerometer_Min = MainPanel:get_argument_value(177)
							local Acelerometer_Max = MainPanel:get_argument_value(178)
							local Ammeter = MainPanel:get_argument_value(101)	
							local hydraulic_Pressure = MainPanel:get_argument_value(78)  
							local Oxygen_Flow_Blinker = MainPanel:get_argument_value(33)
							local Oxygen_Pressure = MainPanel:get_argument_value(34)
							local Fuel_Tank_Left = MainPanel:get_argument_value(155)
							local Fuel_Tank_Right = MainPanel:get_argument_value(156)
							local Fuel_Tank_Fuselage = MainPanel:get_argument_value(160)
							local Tail_radar_warning = MainPanel:get_argument_value(161)
							local Channel_A = MainPanel:get_argument_value(122)
							local Channel_B = MainPanel:get_argument_value(123)
							local Channel_C = MainPanel:get_argument_value(124)
							local Channel_D = MainPanel:get_argument_value(125)
							local transmit_light = MainPanel:get_argument_value(126)
							local RocketCounter = MainPanel:get_argument_value(77)
													
						--- preparing landing gear and High Blower lights, all together, in only one value	
							local gear_lights = 0
							if LandingGearGreenLight > 0 then gear_lights = gear_lights +100 end
							if LandingGearRedLight > 0 then gear_lights = gear_lights +10 end
							if Hight_Blower_Lamp > 0 then gear_lights = gear_lights +1 end
						------------------------------------------------------------	
						
						--- preparing radio lights, all together, in only one value	
							local radio_active = 0
							if Channel_A > 0 then radio_active = 1 end
							if Channel_B >0 then radio_active= 2 end
							if Channel_C >0 then radio_active= 3 end
							if Channel_D >0 then radio_active= 4 end
							if transmit_light >0 then radio_active = radio_active + 10 end
						------------------------------------------------------------
						
						
						---- sending P51 and tf51 data across fc2 interface
						
							SendData("1", string.format("%.5f", math.floor((AHorizon_Pitch+1)*1000) + ((AHorizon_Bank+1)/100) ) ) 	-- pitch
							SendData("2", string.format("%.3f", math.floor(Oxygen_Flow_Blinker*100) + (Oxygen_Pressure/100) ) )		-- bank
							SendData("3", string.format("%.4f", math.floor(OilTemperature*100) + (vaccum_suction/100) ) )			-- yaw
							SendData("4", string.format("%.3f", math.floor(Altimeter_10000_footPtr) + (AHorizon_Caged/100) ) )		-- barometric altitude 
							SendData("5", string.format("%.5f", math.floor(Clock_hours*1000000) + (Tail_radar_warning/100) ) )		-- radar altitude 
							SendData("6", string.format("%.5f", math.floor(CompassHeading*1000) + (CommandedCourse/100) ) )			-- adf
							SendData("7", string.format("%.4f", math.floor(Clock_seconds*100) + (hydraulic_Pressure/100) ) )		-- rmi
							SendData("8", string.format("%.2f", math.floor(GyroHeading*1000) + (radio_active/100) ) )				-- heading
							SendData("9", string.format("%.4f", math.floor(Engine_RPM*100) + (Manifold_Pressure/100) ) )			-- left rpm
							SendData("10", string.format("%.4f", math.floor(Fuel_Tank_Left*100) + (Fuel_Tank_Right/100) ) )			-- right rpm
							SendData("11", string.format("%.4f", math.floor(carburator_temp*100) + (coolant_temp/100) ) )			-- left temp
							SendData("12", string.format("%.4f", math.floor(gear_lights) + (Acelerometer_Min/100 ) ) )				-- right temp
							SendData("13", string.format("%.2f", Variometer) )														-- vvi
							SendData("14", string.format("%.5f", math.floor(AirspeedNeedle)+ (RocketCounter/100) ) )				-- ias
							SendData("15", string.format("%.4f", math.floor(OilPressure*100) + (FuelPressure/100) ) )				-- distance to way
							SendData("16", string.format("%.3f", math.floor(Acelerometer*1000) + (Acelerometer_Max/100 ) ) )		-- aoa
							SendData("17", string.format("%.4f", math.floor((TurnNeedle+1)*100) + ((Slipball+1)/100) ) )			-- glide
							SendData("18", string.format("%.4f", math.floor(Fuel_Tank_Fuselage*100) + (Ammeter/100) ) )				-- side
						

	FlushData()
end

-----------------------------------------
----------------HAWK AIRCRAFT -----------
-----------------------------------------
function ProcessHawkExports()

		local altBar = LoGetAltitudeAboveSeaLevel()
		local altRad = LoGetAltitudeAboveGroundLevel()
-- this line crashes it.. local hsi    = LoGetControlPanel_HSI()
		local pitch, bank, yaw = LoGetADIPitchBankYaw()
		local aoa = LoGetAngleOfAttack()
		local ias = LoGetIndicatedAirSpeed()
		local vvi = LoGetVerticalVelocity()
		local engine = LoGetEngineInfo()
		local glide = LoGetGlideDeviation()
		local side = LoGetSideDeviation()

		SendData("1", string.format("%.2f", pitch * 57.3) )
		SendData("2", string.format("%.2f", bank * 57.3) )
		SendData("3", string.format("%.2f", yaw * 57.3) )
		SendData("4", string.format("%.2f", altBar) )
		SendData("14", string.format("%.2f", ias) )
		SendData("13", string.format("%.2f", vvi) )
		SendData("16", string.format("%.2f", aoa * 57.3 ) )

		SendData("5", string.format("%.2f", altRad) )

		if (engine) then
		  SendData("9", string.format("%.2f", engine.RPM.left) )
		  SendData("10", string.format("%.2f", engine.RPM.right) )
		  SendData("11", string.format("%.2f", engine.Temperature.left) )
		  SendData("12", string.format("%.2f", engine.Temperature.right) )
		end
		SendData("17", string.format("%.2f", glide) )
		SendData("18", string.format("%.2f", side) )

		FlushData()

end
--------------------------------------------------------------------------------------
----------------MIG-21Bis AIRCRAFT old using FC2 interface  now just for testing------
--------------------------------------------------------------------------------------

function ProcessMiG21Exports()
	MainPanel = GetDevice(0)

--compressed air aux
--local o= MainPanel:get_argument_value(351)
--log_file:write("349:")
--log_file:write(o)
--log_file:write("\n")

	FlushData()

end

---------------------------------------------
-- DCS Export API Function Implementations --
---------------------------------------------

function LuaExportStart()
-- Works once just before mission start.
-- (and before player selects their aircraft, if there is a choice!)

    -- DEBUGGING STUFF: uncomment to enable debug log file
    debug_output_file = io.open("./ExportDebug.log", "wa")

	log_file = io.open("C:/Users/Admin/Saved Games/DCS/Logs/Export.log", "w")
	log_file:write("Test 22\n")

    -- 2) Setup udp sockets to talk to helios
    package.path  = package.path..";.\\LuaSocket\\?.lua"
    package.cpath = package.cpath..";.\\LuaSocket\\?.dll"

    socket = require("socket")
    c = socket.udp()

    if c == nil then
      -- DEBUGGING STUFF:
	   if debug_output_file then
	    debug_output_file:write("ERROR CREATING HELIOS SOCKET!\r\n")
      end
    else
      c:setsockname("*", 0)
      c:setoption('broadcast', true)
      c:settimeout(.001) -- set the timeout for reading the socket
      -- DEBUGGING STUFF:
	  if debug_output_file then
	    debug_output_file:write("HELIOS SOCKET CREATED!\r\n")
      end
    end

end


function LuaExportBeforeNextFrame()
	ProcessInput()
end

function LuaExportAfterNextFrame()
end


function LuaExportStop()
-- Works once just after mission stop.
    -- c:close()

    -- DEBUGGING STUFF:
	if debug_output_file then
	  debug_output_file:write("LuaExportStop called.\r\n")
	  --io.close(debug_output_file)
	end

	if log_file then
		log_file:write("Closing log file.")
		log_file:close()
		log_file = nil
	end

end

-- Take any inputs from Helios and pass them to DCS World
function ProcessInput()

    local lInput = c:receive()

	 log_file:write( lIinput )

    local lCommand, lCommandArgs, lDevice, lArgument, lLastValue

    if lInput then

			lCommand = string.sub(lInput,1,1)

			if lCommand == "R" then
				ResetChangeValues()
			end

			if (lCommand == "C") then
				lCommandArgs = StrSplit(string.sub(lInput,2),",")
				lDevice = GetDevice(lCommandArgs[1])

	log_file:write( lCommandArgs[1] )
	log_file:write("\n")

				if type(lDevice) == "table" then
					lDevice:performClickableAction(lCommandArgs[2],lCommandArgs[3])
		log_file:write( lCommandArgs[2] )
		log_file:write("\n")
		log_file:write( lCommandArgs[3] )
		log_file:write("\n")
				end
			end
    end
end


function LuaExportActivityNextEvent(t)

	t = t + gExportInterval

	gTickCount = gTickCount + 1

	SelectAircraft() -- point globals appropriate functions and data.
	if gHawk then
	    ProcessHawkExports()
	end
	if gMiG21 then
		ProcessMiG21Exports()
	end
	if gCurrentAircraft == "P-51D" or gCurrentAircraft == "TF-51D" then	
				ProcessP51Exports ()  --process P51 as FC
	end
	if gKnownAircraft then

	  if gFlamingCliffsAircraft then

	    ProcessFCExports ()
		else

	  	local lDevice = GetDevice(0)
			if type(lDevice) == "table" then

			lDevice:update_arguments()

			-- Handle the simple-case data that can be simply read via device:get_argument_value
			ProcessArguments(lDevice, gHighImportanceArguments) -- A10 or Ka50 arguments as appropriate
			-- Handle the more complex calculations that need special logic...
			ProcessHighImportance(lDevice) -- A10 or Ka-50, as appropriate; determined in SelectAircraft()

			if gTickCount >= gExportLowTickInterval then
				-- Handle the simple-case data that can be simply read via device:get_argument_value
				ProcessArguments(lDevice, gLowImportanceArguments) -- A10 or Ka50 arguments as appropriate
				-- Handle the more complex calculations that need special logic...
				ProcessLowImportance(lDevice) -- A10 or Ka-50 as appropriate; determined in SelectAircraft()
				gTickCount = 0 -- start Low Importance tick counting again
			end

			FlushData()

		end

	   end

	else

	   -- unknown aircraft - do nothing.

    end

	return t
end


----------------------
-- Helper Functions --
----------------------

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


--------------------------------
-- Status Gathering Functions --
--------------------------------

-- Handles simple-case data that can be simply read via device:get_argument_value
function ProcessArguments(device, arguments)
	local lArgument , lFormat , lArgumentValue

	for lArgument, lFormat in pairs(arguments) do
		lArgumentValue = string.format(lFormat,device:get_argument_value(lArgument))
		SendData(lArgument, lArgumentValue)
	end
end


-----------------------
-- Network Functions --
-----------------------

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
		local 	packet = gSimID .. table.concat(gSendStrings, ":") .. "\n"
		socket.try(c:sendto(packet, gHost, gPort))
		gSendStrings = {}
		gPacketSize = 0
	end
end

function ResetChangeValues()
	gLastData = {}
	gTickCount = 10
end

-- Print contents of `tbl`, with indentation.
-- `indent` sets the initial level of indentation.
function debugDumpTable (tbl, indent)
  	if debug_output_file then
		if not indent then indent = 0 end
		for k, v in pairs(tbl) do
			formatting = string.rep("  ", indent) .. k .. ": "
			if type(v) == "table" then
				debug_output_file:write(formatting.."\r\n")
				debugDumpTable(v, indent+1)
			elseif type(v) == 'boolean' then
				debug_output_file:write(formatting .. tostring(v).."\r\n")
			else
				debug_output_file:write(formatting .. v.."\r\n")
			end
		end
    end
end


-------------------------------
-- HANDLE DIFFERENT AIRCRAFT --
-------------------------------

function SelectAircraft()

  -- Select aircraft...

  local myInfo = LoGetSelfData()

  if myInfo == nil then

    gCurrentAircraft = ""
    gKnownAircraft = false
	 gFlamingCliffsAircraft = false
	 gHawk = false
	 gMiG21 = false

    -- DEBUGGING STUFF:
    if debug_output_file then
      debug_output_file:write("LoGetSelfData() returned nil" )
    end

  elseif myInfo.Name ~= gCurrentAircraft then

    gCurrentAircraft = myInfo.Name

    -- DEBUGGING STUFF:
    if debug_output_file then
      debug_output_file:write(string.format("Selecting %s\r\n", gCurrentAircraft ) )
    end

    if gCurrentAircraft == "A-10C" then

		gKnownAircraft = true
		gFlamingCliffsAircraft = false
		gHighImportanceArguments = gA10HighImportanceArguments
		gLowImportanceArguments = gA10LowImportanceArguments
		ProcessHighImportance = ProcessA10HighImportance
		ProcessLowImportance =  ProcessA10LowImportance

    elseif gCurrentAircraft == "Ka-50" then

		gKnownAircraft = true
		gFlamingCliffsAircraft = false
		gHighImportanceArguments = gKa50HighImportanceArguments
		gLowImportanceArguments = gKa50LowImportanceArguments
		ProcessHighImportance = ProcessNoHighImportance
		ProcessLowImportance =  ProcessKa50LowImportance

    elseif gCurrentAircraft == "A-10A"
  	    or gCurrentAircraft == "F-15C"
	    or gCurrentAircraft == "F-15C"
	    or gCurrentAircraft == "MiG-29A"
	    or gCurrentAircraft == "MiG-29G"
	    or gCurrentAircraft == "MiG-29K"
	    or gCurrentAircraft == "MiG-29S"
	    or gCurrentAircraft == "Su-25"
	    or gCurrentAircraft == "Su-25T"
	    or gCurrentAircraft == "Su-25TM"
	    or gCurrentAircraft == "Su-27"
	    or gCurrentAircraft == "Su-33" then

		gKnownAircraft = true
		gFlamingCliffsAircraft = true
		gHighImportanceArguments = {}
		gLowImportanceArguments = {}
		ProcessHighImportance = ProcessNoHighImportance
		ProcessLowImportance =  ProcessNoLowImportance

elseif gCurrentAircraft == "Hawk" then
		gHawk = true
		gKnownAircraft = true
		gFlamingCliffsAircraft = false -- this isnt where it calls it then
		gHighImportanceArguments = {}
		gLowImportanceArguments = {}
		ProcessHighImportance = ProcessNoHighImportance
		ProcessLowImportance =  ProcessNoLowImportance

elseif gCurrentAircraft == "MiG-21Bis" then  --name is correct i checked the export
		--log_file:write("MiG21\n")
		gMiG21 = true
	        gHawk = false
		gKnownAircraft = true
		gFlamingCliffsAircraft = false -- this isnt where it calls it then
		gHighImportanceArguments = gMIG21HighImportanceArguments
		gLowImportanceArguments = gMIG21LowImportanceArguments
		ProcessHighImportance = ProcessNoHighImportance
		ProcessLowImportance =  ProcessNoLowImportance

 else -- Unknown aircraft; fail safe

		if debug_output_file then
			debug_output_file:write("Unknown aircraft - disabling exports.\r\n")
		end
		gKnownAircraft = false
		gFlamingCliffsAircraft = false --was false changed to true to see if hawk works
		gHighImportanceArguments = {}
		gLowImportanceArguments = {}
		ProcessHighImportance = ProcessNoHighImportance
		ProcessLowImportance =  ProcessNoLowImportance

    end
  end
end

--dofile("./AriesWings/AriesRadio.luac")
local Tacviewlfs=require('lfs');dofile(Tacviewlfs.writedir()..'Scripts/TacviewExportDCS.lua')
