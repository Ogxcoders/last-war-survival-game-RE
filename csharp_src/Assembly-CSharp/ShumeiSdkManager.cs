using UnityEngine;

public class ShumeiSdkManager
{
	private ShumeiSdkState _sdkState;

	private bool _isSendRequested;

	private bool _hasBeenUsedByLogin;

	private static ShumeiSdkManager _instance;

	public IShumeiSdkPlatform _platform;

	private string _mostNewVersion = string.Empty;

	public static ShumeiSdkManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ShumeiSdkManager();
			}
			return _instance;
		}
	}

	public IShumeiSdkPlatform Platform
	{
		get
		{
			if (_platform == null)
			{
				_platform = new ShumeiSdkPlatformAndroid();
			}
			return _platform;
		}
	}

	public bool IsFunctionOpen => ClientSwitch.IsOn(25);

	public void Init()
	{
		_sdkState = ShumeiSdkState.None;
		_isSendRequested = false;
		_hasBeenUsedByLogin = false;
		_mostNewVersion = string.Empty;
	}

	public void Logout()
	{
		Init();
		PrintInfoLog("ShumeiSdk Logout called, state reset.");
	}

	public void CallCreate()
	{
		if (IsFunctionOpen)
		{
			if (_sdkState != ShumeiSdkState.None)
			{
				PrintInfoLog("CallCreate ignored, SDK is already initializing or initialized.");
				return;
			}
			_sdkState = ShumeiSdkState.Initializing;
			Platform.CallCreate();
			PrintInfoLog("call create, state changed to Initializing.");
		}
	}

	public void OnShumeiSdkInitSuccess()
	{
		PrintInfoLog("ShumeiSdk Init Success");
		if (_sdkState == ShumeiSdkState.Initializing)
		{
			_sdkState = ShumeiSdkState.InitSuccess;
			TrySendDeviceId();
		}
	}

	public void OnShumeiSdkInitError()
	{
		PrintInfoLog("ShumeiSdk Init Error");
		if (_sdkState == ShumeiSdkState.Initializing)
		{
			_sdkState = ShumeiSdkState.InitError;
			TrySendDeviceId();
		}
	}

	private string GetDeviceId()
	{
		string deviceId = Platform.GetDeviceId();
		PrintInfoLog("device id:" + deviceId);
		return deviceId;
	}

	public void SendDeviceIdToServer()
	{
		if (IsFunctionOpen)
		{
			PrintInfoLog("SendDeviceIdToServer called.");
			_isSendRequested = true;
			TrySendDeviceId();
		}
	}

	private void TrySendDeviceId()
	{
		if (!_isSendRequested)
		{
			return;
		}
		if (_hasBeenUsedByLogin)
		{
			PrintInfoLog("TrySendDeviceId: Aborted, device ID has been used by login flow.");
		}
		else if (_sdkState == ShumeiSdkState.InitSuccess)
		{
			PrintInfoLog("TrySendDeviceId: SDK init success, sending device ID.");
			string deviceId = GetDeviceId();
			if (!string.IsNullOrEmpty(deviceId))
			{
				ShumeiSendDeviceIdMessage.Instance.Send(new ShumeiSendDeviceIdMessage.Request
				{
					boxId = deviceId
				});
				_sdkState = ShumeiSdkState.SentToServerSuccess;
			}
			else
			{
				PrintInfoLog("Device ID is empty, cannot send to server.");
			}
		}
		else if (_sdkState == ShumeiSdkState.InitError)
		{
			PrintInfoLog("TrySendDeviceId: SDK init error, sending empty device ID.");
			PostEventLog.TrackMap("ShumeiSdkInitError", null);
			ShumeiSendDeviceIdMessage.Instance.Send(new ShumeiSendDeviceIdMessage.Request
			{
				boxId = string.Empty
			});
			_sdkState = ShumeiSdkState.SentToServerError;
		}
	}

	public string GetLoginSmsdkId()
	{
		if (!IsFunctionOpen)
		{
			return string.Empty;
		}
		if (_sdkState == ShumeiSdkState.InitSuccess || _sdkState == ShumeiSdkState.SentToServerSuccess)
		{
			string deviceId = GetDeviceId();
			PrintInfoLog("GetLoginSmsdkId called, got DeviceId: " + deviceId);
			_hasBeenUsedByLogin = true;
			return deviceId;
		}
		PrintInfoLog("GetLoginSmsdkId: SDK not ready or init failed.");
		return string.Empty;
	}

	public void SetMostNewVersion(string version)
	{
		_mostNewVersion = version;
		PrintInfoLog("SetMostNewVersion called, version: " + version);
	}

	public bool IsForbidCreateRole()
	{
		if (!IsFunctionOpen)
		{
			return false;
		}
		if (GameEntry.Lua == null || GameEntry.Data == null || GameEntry.Data.Player == null)
		{
			PrintInfoLog("IsForbidCreateRole: GameEntry Lua or Data or Player is null, return false");
			return false;
		}
		bool flag = GameEntry.Data?.Player?.CheckSwitch("shumei_new_user_check", defaultVal: false) ?? false;
		PrintInfoLog("IsForbidCreateRole called, switchOn: " + flag);
		if (!flag)
		{
			return false;
		}
		if (_mostNewVersion.IsNullOrEmpty())
		{
			PrintInfoLog("IsForbidCreateRole: _mostNewVersion is empty, return false");
			return false;
		}
		int num = GameEntry.Lua.CallWithReturnInt("CSharpCallLuaInterface.GetConfigNum", "login_shumei_level_limit", "k6");
		PrintInfoLog("IsForbidCreateRole: maxVersionLimit: " + num);
		if (num <= 0)
		{
			PrintInfoLog("IsForbidCreateRole: maxVersionLimit <= 0, return false");
			return false;
		}
		string[] array = Application.version.Split(new char[1] { '.' });
		string[] array2 = _mostNewVersion.Split(new char[1] { '.' });
		if (array.Length < 3 || array2.Length < 3)
		{
			PrintInfoLog("IsForbidCreateRole: version format error, return false");
			return false;
		}
		int num2 = int.Parse(array[0]);
		int num3 = int.Parse(array[1]);
		int num4 = int.Parse(array[2]);
		int num5 = int.Parse(array2[0]);
		int num6 = int.Parse(array2[1]);
		int num7 = int.Parse(array2[2]);
		int num8 = (num5 - num2) * 10000 + (num6 - num3) * 100 + (num7 - num4);
		PrintInfoLog($"IsForbidCreateRole: versionDiff: {num8}, maxVersionLimit: {num}");
		if (num8 <= num)
		{
			PrintInfoLog("IsForbidCreateRole: versionDiff <= maxVersionLimit, return false");
			return false;
		}
		return true;
	}

	public void GotoStoreToUpgrade()
	{
		string text = "Google Play";
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			text = "AppStore";
		}
		UIUtils.ShowMessage(GameEntry.Localization.GetString("shumei_version_gap", text), 2, "110003", "", delegate
		{
			if (!GameEntry.GlobalData.downloadurl.IsNullOrEmpty())
			{
				SDKManager.OpenURL(GameEntry.GlobalData.downloadurl);
			}
		}, delegate
		{
		});
	}

	public void PrintInfoLog(string msg)
	{
	}
}
