using System;
using Sfs2X.Requests;

public class GameEntryProxy
{
	public class LocalizationManagerProxy
	{
		public Func<bool> GetIsArabicAutoMirrorOpen;
	}

	public class EventComponentProxy
	{
		public Action<EventId, object> Fire;
	}

	public class NetworkManagerProxy
	{
		public Action<IRequest> Send;

		public Func<string> getCurLine;
	}

	public class CrossServerComponentProxy
	{
		public Action<IRequest> SendIRequest;

		public void Send(IRequest request)
		{
			SendIRequest(request);
		}
	}

	public class SoundComponentProxy
	{
		public Action<string> PlayEffect;
	}

	public class SettingManagerProxy
	{
		public Func<string, string> GetPublicString;

		public Func<string, string, string> GetPublicString2;

		public Func<string> GetGameSessionId;

		public Action<string> SetGameSessionId;

		public Func<string, bool, bool> GetBool;

		public string gameSessionId
		{
			get
			{
				return GetGameSessionId();
			}
			set
			{
				SetGameSessionId(value);
			}
		}
	}

	public class LuaManagerProxy
	{
		public Action<string, object> DispatchResponse;

		public Action DataCenterInit;

		public Action PreloadAssets;

		public Action<long> UpdateUITimeStamp;

		public Action<string, string, string> ShowTips;

		public Action<string, string, string> SetValue;
	}

	public class DeviceManagerProxy
	{
		public Func<string> GetNetworkTypeDesc;

		public Func<string> GetDeviceUid;
	}

	public class SDKManagerProxy
	{
		public string GetDeviceUid;

		public Func<string> GetVersion;

		public Func<string> GetVersionCode;

		public Func<string> GetPf_displayname;

		public string Version => GetVersion();

		public string VersionCode => GetVersionCode();

		public string pf_displayname => GetPf_displayname();
	}

	public class GlobalDataManagerProxy
	{
		public Func<string> GetFromCountry;

		public Func<string> GetAnalyticID;

		public string fromCountry => GetFromCountry();

		public string analyticID => GetAnalyticID();
	}

	public class TimerComponentProxy
	{
		public Func<long> GetServerTime;

		public Action<long> UpdateServerMilliseconds;

		public Action<long, long, long, bool> SyncServerTime;
	}

	public static LocalizationManagerProxy Localization;

	public static EventComponentProxy Event;

	public static NetworkManagerProxy Network;

	public static CrossServerComponentProxy NetworkCross;

	public static SoundComponentProxy Sound;

	public static LuaManagerProxy Lua;

	public static DeviceManagerProxy Device;

	public static SDKManagerProxy Sdk;

	public static SettingManagerProxy Setting;

	public static GlobalDataManagerProxy GlobalData;

	public static TimerComponentProxy Timer;

	public static void Initialize()
	{
		Localization = new LocalizationManagerProxy();
		Event = new EventComponentProxy();
		Network = new NetworkManagerProxy();
		NetworkCross = new CrossServerComponentProxy();
		Sound = new SoundComponentProxy();
		Setting = new SettingManagerProxy();
		Timer = new TimerComponentProxy();
		GlobalData = new GlobalDataManagerProxy();
		Lua = new LuaManagerProxy();
		Sdk = new SDKManagerProxy();
		Device = new DeviceManagerProxy();
	}

	public static void Dispose()
	{
		Localization = null;
		Event = null;
		Network = null;
		NetworkCross = null;
		Sound = null;
		Setting = null;
		Timer = null;
		GlobalData = null;
		Lua = null;
		Sdk = null;
		Device = null;
	}
}
