using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using VEngine;

public class StartupConfig
{
	private static StartupConfig _inst;

	public string configJson;

	public bool turnOnOpt;

	public bool syncLoadPackageManifest = true;

	public bool resInitdLoadRightnow = true;

	public bool delayLuaInit;

	public bool hideBackgroundPrior;

	public bool syncUI;

	public bool asyncLoadUI;

	public bool useLongSlice;

	public int longSliceMillSecond = 20;

	public bool useAllSlice;

	public bool reverseAddLoadable = true;

	public bool earlyHideSplash;

	public bool lateHideSplash = true;

	public static StartupConfig Inst
	{
		get
		{
			if (_inst == null)
			{
				LoadConfig();
			}
			return _inst;
		}
	}

	public Dictionary<string, object> AddBIProperty(Dictionary<string, object> prop = null)
	{
		if (prop == null)
		{
			prop = new Dictionary<string, object>();
		}
		if (Versions.syncLoadPackageManifest != syncLoadPackageManifest)
		{
			configJson = JsonUtility.ToJson(_inst);
		}
		prop["StartupConfig"] = configJson;
		prop["TurnOnOpt"] = turnOnOpt;
		prop["SyncLoadApk"] = Versions.syncLoadPackageManifest;
		return prop;
	}

	public static void LoadConfig()
	{
		if (_inst == null)
		{
			_inst = new StartupConfig();
		}
		_inst.syncLoadPackageManifest = true;
		_inst.syncLoadPackageManifest &= !Application.isEditor;
		_inst.turnOnOpt = true;
		_inst.TurnOnDefault();
		if (!_inst.turnOnOpt)
		{
			_inst.TurnOffAll();
		}
		_inst.configJson = JsonUtility.ToJson(_inst);
		Log.Info("StartupConfig::LoadConfig " + _inst.configJson);
	}

	public void SwitchOptOffAll()
	{
		if (Inst.turnOnOpt)
		{
			Inst.turnOnOpt = false;
			Inst.TurnOffAll();
			_inst.configJson = JsonUtility.ToJson(_inst);
		}
	}

	public void TurnOffAll()
	{
		resInitdLoadRightnow = false;
		delayLuaInit = false;
		hideBackgroundPrior = false;
		syncUI = false;
		asyncLoadUI = false;
		useLongSlice = false;
		longSliceMillSecond = 10;
		useAllSlice = false;
		reverseAddLoadable = false;
		earlyHideSplash = false;
		lateHideSplash = false;
	}

	public void TurnOnDefault()
	{
		resInitdLoadRightnow = false;
		delayLuaInit = true;
		hideBackgroundPrior = false;
		syncUI = false;
		asyncLoadUI = true;
		useLongSlice = true;
		longSliceMillSecond = 20;
		useAllSlice = false;
		reverseAddLoadable = true;
		earlyHideSplash = false;
		lateHideSplash = true;
	}
}
