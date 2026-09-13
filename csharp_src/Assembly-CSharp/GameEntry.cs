using System;
using BaseUtils;
using GameFramework;
using GameKit.Base;
using Sfs2XLw.Entities.Data;
using UnityEngine;
using UnityEngine.Networking;
using UnityGameFramework.Runtime;

public static class GameEntry
{
	public static PowerVRExtensions pvrExtension;

	public static ISFSObject _placeHolderProp;

	public static BaseComponent GameBase { get; private set; }

	public static EventComponent Event { get; private set; }

	public static Transform UIContainer { get; private set; }

	public static Camera UICamera { get; private set; }

	public static ResourceManager Resource { get; private set; }

	public static LocalizationManager Localization { get; private set; }

	public static NetworkManager Network { get; private set; }

	public static CrossServerComponent NetworkCross { get; private set; }

	public static SoundComponent Sound { get; private set; }

	public static SettingManager Setting { get; private set; }

	public static XLuaManager Lua { get; private set; }

	public static CustomDataManager Data { get; private set; }

	public static PBController pb { get; private set; }

	public static TimerComponent Timer { get; private set; }

	public static GlobalDataManager GlobalData { get; private set; }

	public static SDKManager Sdk { get; private set; }

	public static DeviceManager Device { get; private set; }

	public static BuildAnimatorManager BuildAnimatorManager { get; private set; }

	public static ConfigCache ConfigCache { get; private set; }

	public static GameLODManager LOD { get; private set; }

	public static PayOrderDataManager PayOrderData { get; private set; }

	public static void Init()
	{
		Log.Info("GameEntry Init Begin.");
		Event = new EventComponent((EventPoolMode)3);
		Localization = new LocalizationManager();
		Network = new NetworkManager();
		NetworkCross = new CrossServerComponent();
		Sound = new SoundComponent();
		Setting = new SettingManager();
		Setting.UpdateFirstLaunchFlag();
		Data = new CustomDataManager();
		pb = new PBController();
		Timer = new TimerComponent();
		GlobalData = new GlobalDataManager();
		Resource = new ResourceManager();
		Lua = new XLuaManager();
		Sdk = new SDKManager();
		Sdk.Initialize();
		Device = new DeviceManager();
		ConfigCache = new ConfigCache();
		BuildAnimatorManager = new BuildAnimatorManager();
		PayOrderData = new PayOrderDataManager();
		LOD = new GameLODManager();
		UIContainer = GameObject.Find("GameFramework/UI/UIContainer").transform;
		UICamera = GameObject.Find("GameFramework/UI/UICamera").GetComponent<Camera>();
		pvrExtension = new PowerVRExtensions();
		_ = MainThreadDispatcher.Instance;
		RegisterAction();
		string text = null;
		if (StringUtils.VersionCompare(Sdk.Version, "1.0.170") >= 0 || CommonUtils.IsDebug())
		{
			text = Sdk.GetRestartData();
			Log.Info("GameEntry::Get restart data:" + text);
			if (!string.IsNullOrEmpty(text))
			{
				Setting.gameSessionId = text;
			}
		}
		Sdk.InitTrackingData("");
		PostEventLog.TrackMap("app_launch", SDKManager.AddBILaunchTimeProperty());
		Log.Info("GameEntry Init End.");
	}

	public static void RegisterAction()
	{
		GameEntryProxy.Initialize();
		GameEntryProxy.Setting.GetPublicString = Setting.GetPublicString;
		GameEntryProxy.Setting.GetPublicString2 = Setting.GetPublicString;
		GameEntryProxy.Setting.GetBool = Setting.GetBool;
		GameEntryProxy.Lua.DispatchResponse = Lua.DispatchResponse;
		GameEntryProxy.Device.GetNetworkTypeDesc = Device.GetNetworkTypeDesc;
		GameEntryProxy.Device.GetDeviceUid = Device.GetDeviceUid;
		GameEntryProxy.Sdk.GetVersion = () => Sdk.Version;
		GameEntryProxy.Sdk.GetVersionCode = () => Sdk.VersionCode;
		GameEntryProxy.Timer.GetServerTime = Timer.GetServerTime;
		GameEntryProxy.Timer.UpdateServerMilliseconds = Timer.UpdateServerMilliseconds;
		GameEntryProxy.Timer.SyncServerTime = Timer.SyncServerTime;
		GameEntryProxy.Setting.GetGameSessionId = () => Setting.gameSessionId;
		GameEntryProxy.Setting.SetGameSessionId = delegate(string v)
		{
			Setting.gameSessionId = v;
		};
		GameEntryProxy.Event.Fire = Event.Fire;
		GameEntryProxy.Network.Send = Network.Send;
		GameEntryProxy.GlobalData.GetFromCountry = () => GlobalData.fromCountry;
		GameEntryProxy.GlobalData.GetAnalyticID = () => GlobalData.analyticID;
		GameEntryProxy.Network.getCurLine = Network.getCurLine;
		GameEntryProxy.Sdk.GetPf_displayname = () => Sdk.pf_displayname;
		GameEntryProxy.NetworkCross.SendIRequest = NetworkCross.Send;
		GameEntryProxy.Lua.SetValue = Lua.SetValue;
		GameEntryProxy.Localization.GetIsArabicAutoMirrorOpen = () => MirrorVersionConfig.IsMirrorVersionOpen;
		SceneManagerProxy.Initialize();
		SceneManagerProxy.IsInWorld = SceneManager.IsInWorld;
		MessageFactoryProxy.Instance.DispatchResponse1 = MessageFactory.Instance.DispatchResponse;
		MessageFactoryProxy.Instance.DispatchResponse2 = MessageFactory.Instance.DispatchResponse;
		WebRequestManagerProxy.Instance.GetAction = delegate(string uri, Action<UnityWebRequest, bool, object> cb, int priority, int timeout, object userdata)
		{
			SingletonBehaviour<WebRequestManager>.Instance.Get(uri, delegate(UnityWebRequest request, bool hasErr, object userData)
			{
				cb(request, hasErr, userData);
			}, priority, timeout, userdata);
		};
	}

	public static void ResetCrossServerComponent()
	{
		if (NetworkCross != null)
		{
			try
			{
				NetworkCross.Shutdown();
			}
			catch (Exception ex)
			{
				Log.Error("GameEntry.ResetCrossServerComponent error : " + ex.Message);
			}
		}
		NetworkCross = new CrossServerComponent();
		Log.Info("GameEntry.ResetCrossServerComponent netRawProxy");
		if (GameEntryProxy.NetworkCross != null)
		{
			GameEntryProxy.NetworkCross.SendIRequest = NetworkCross.Send;
		}
	}

	public static void RegisterComponent(GameFrameworkComponent com)
	{
		if (com.GetType() == typeof(BaseComponent))
		{
			GameBase = com as BaseComponent;
		}
	}

	public static void OnDisconnectRetry()
	{
		ConfigCache.reset();
	}

	public static void Shutdown()
	{
		LOD.Shutdown();
		Lua.ExitGame();
		Event.Shutdown();
		Network.Shutdown();
		Timer.Shutdown();
		Sdk.Shutdown();
		BuildAnimatorManager.Shutdown();
		Resource.Clear();
		ConfigCache.reset();
		Lua.Shutdown();
		pvrExtension.Shutdown();
		DisposeAction();
	}

	public static void DisposeAction()
	{
		GameEntryProxy.Dispose();
		SceneManagerProxy.Dispose();
		MessageFactoryProxy.Instance.Dispose();
		WebRequestManagerProxy.Instance.Dispose();
		GMSwitch.Dispose();
	}

	public static void Update(float elapseSeconds)
	{
		Event.OnUpdate(elapseSeconds);
		Network.OnUpdate(elapseSeconds);
		NetworkCross.OnUpdate(elapseSeconds);
		Timer.OnUpdate(elapseSeconds);
		Sound.OnUpdate(elapseSeconds);
		Sdk.OnUpdate(elapseSeconds);
		Lua.Update();
		Resource.Update();
		GMSwitch.Update(elapseSeconds);
		ChatService.Instance.OnUpdate();
		CrossServerUtil.Update();
	}

	public static void DebugLuaFile(string filename)
	{
		Lua.DebugLuaFile(filename);
	}
}
