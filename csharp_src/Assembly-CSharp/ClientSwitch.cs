using GameFramework;
using UnityEngine;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public class ClientSwitch
{
	public const int ENABLE_TABLE_PATCH = 0;

	public const int ENABLE_UNUSE_NOTDONE_ASSET = 1;

	public const int ENABLE_BUNDLE_FAST_VALIDATION = 2;

	public const int ENABLE_WORLD_DYNAMIC_POOL = 3;

	public const int DISABLE_NEW_NET_PACKET = 4;

	public const int DISABLE_ZSTD = 5;

	public const int ENABLE_SMART_FOX_CROSS_SERVER = 8;

	public const int ENABLE_DELAY_PAUSE_RESUME = 10;

	public const int ENABLE_SAMSUNG_SOFTKEYBOARD_MOD = 11;

	public const int ENABLE_LW_LUA_BINARY_PATCH = 12;

	public const int ENABLE_FIX_DOWNLOADSIZE = 13;

	public const int ENABLE_COPPA_VERIFY = 14;

	public const int ENABLE_DISPOSE_OLD_LUA_ENV = 15;

	public const int DISABLE_BUNDLE_ALIAS = 17;

	public const int ENABLE_ASSET_LOAD_CACHE = 18;

	public const int ENABLE_TRACEROUTE = 19;

	public const int ENABLE_CDN_SPEED_TEST = 20;

	public const int ENABLE_ACCOUNT_SELECT_STATE = 21;

	public const int ENABLE_ASSETS_USE_TRACK = 22;

	public const int ENABLE_BUNDLE_USE_TRACK = 23;

	public const int DISABLE_FIRST_LAUNCH_SKIP_UPDATE = 24;

	public const int ENABLE_SHUMEI_SDK = 25;

	public const int ENABLE_SERVER_WS_CONNECTION = 26;

	public const int SM_INPUT_HEIGHT = 27;

	public const int LUA_TABLE_UTIL_CONTAINS_KEY_USE_NEW_JUDGE = 28;

	public const int ENABLE_CROSS_FORWARD = 29;

	public const int ENABLE_ASYNC_LOAD_IMAGE_CALLBACK_CHECK = 30;

	public const int EVENT_FRAME_BLEND = 32;

	public const int ENABLE_SOUND_RESOURCE_DOWNLOAD = 33;

	public const int ENABLE_DYNAMIC_ATLAS_V2 = 34;

	public const int ENABLE_REPLACE_GPU_SKIN_PREFAB = 35;

	public const int ENABLE_SHUMEI_CREATE_ACCOUNT_RISK_BAN = 36;

	public const int ENABLE_WORLD_MARCH_STEP_EXECUTE = 37;

	public const int ENABLE_BUNDLE_LOAD_TRACK = 38;

	public const int ENABLE_WORLD_MARCH_MESSAGE_MERGE = 39;

	public const int ENABLE_WORLD_MARCH_MESSAGE_STEP = 40;

	public const int ENABLE_NEW_GRAPHIC_LEVEL = 41;

	public const int ENABLE_OPT_PVE_RVO = 42;

	public const int ENABLE_PAY_PRODUCT_DATA_PATCH = 43;

	public const int ENABLE_CONNECT_GAME_WEBSOCKET_FALLBACK = 44;

	public const int INPUT_FIX_ISON = 45;

	public const int ENABLE_IOS_NATIVE_SOCKET = 46;

	public const int ENABLE_PC_UNINSTALL_ACE_DRIVERS = 47;

	public const int ENABLE_PARALLEL_INIT = 48;

	private static bool[] _switch;

	public const string ENABLE_DISPOSE_OLD_LUA_ENV_KEY = "ENABLE_DISPOSE_OLD_LUA_ENV_KEY";

	public static void Parse(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return;
		}
		string[] array = str.Split(new char[1] { '|' });
		if (array.Length != 0)
		{
			_switch = new bool[array.Length];
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				_switch[i] = array[i] == "1";
				PlayerPrefs.SetInt($"CLIENT_SWITCH_CACHE_ON_{i}", _switch[i] ? 1 : 0);
			}
		}
		PlayerPrefs.Save();
		Log.Info("ClientSwitch " + str);
	}

	public static bool IsOn(int index)
	{
		if (_switch != null && index >= 0 && index < _switch.Length)
		{
			return _switch[index];
		}
		return false;
	}

	public static bool IsCacheOn(int index)
	{
		return PlayerPrefs.GetInt($"CLIENT_SWITCH_CACHE_ON_{index}", 0) != 0;
	}

	public static bool IsOff(int index)
	{
		if (_switch != null && index >= 0 && index < _switch.Length)
		{
			return !_switch[index];
		}
		return true;
	}

	public static bool HasSwitchData(int index)
	{
		if (_switch != null && index >= 0)
		{
			return index < _switch.Length;
		}
		return false;
	}

	public static void ForceSetSwitch(int index, bool isOn)
	{
		if (_switch != null && index >= 0 && index < _switch.Length)
		{
			_switch[index] = isOn;
		}
	}

	public static void ForceSetSwitchAndSave(int index, bool isOn)
	{
		if (_switch != null && index >= 0 && index < _switch.Length)
		{
			_switch[index] = isOn;
			PlayerPrefs.SetInt($"CLIENT_SWITCH_CACHE_ON_{index}", _switch[index] ? 1 : 0);
			PlayerPrefs.Save();
		}
	}
}
