using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AppsFlyerSDK;
using Balaso;
using BestHTTP.JSON;
using FM_Mono;
using FibMatrix;
using GameFramework;
using GameFramework.Localization;
using RiverBISDK;
using RiverGame.PerformanceAnalysis;
using SFSLitJson;
using Sfs2X.Entities.Data;
using ThinkingAnalytics;
using ThinkingAnalytics.Utils;
using UnityEngine;
using UnityGameFramework.SDK;
using Zendesk;

public class SDKManager : IGameController
{
	private class LWDynamicSuperProperties : IDynamicSuperProperties
	{
		public Dictionary<string, object> GetDynamicSuperProperties()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int? num = GameEntry.Network?.GetPing();
			if (num > 0)
			{
				dictionary.Add("latency", num);
			}
			float lazyTimeSinceStartupWithPause = ForegroundTimer.lazyTimeSinceStartupWithPause;
			dictionary["launch_time"] = lazyTimeSinceStartupWithPause;
			float lazyTimeSinceStartup = ForegroundTimer.lazyTimeSinceStartup;
			dictionary["fore_ground_time"] = lazyTimeSinceStartup;
			dictionary["reload_game_count"] = ApplicationLaunch.Instance.reloadGameCount;
			int? num2 = GameEntry.Network?.GetLastPingPongTime();
			if (num2 > 0)
			{
				dictionary.Add("lastPingPongTime", num2);
			}
			return dictionary;
		}
	}

	private Action<string, string> payCallback_;

	public string pf_displayname = "";

	public bool alreadyStartAF;

	public RuntimeInfoManager runtimeInfo;

	public const int writeAlbumCode = 1001;

	public const int calendarEventCode = 1002;

	public static string writeAlbumFileName = "";

	public static string writeAlbumFilePath = "";

	public static bool isUploadImageFix = false;

	private string m_launchPushId = string.Empty;

	private string calendarEventJsonParam;

	private Dictionary<string, List<string>> m_Events = new Dictionary<string, List<string>>();

	public static bool hadCallHideSplash = false;

	private PerformanceBIScheduler m_PerformanceBIScheduler;

	private int m_AssembliesUpdated = -1;

	private EnumToName<SceneManager.SceneID> m_SceneID2Name = new EnumToName<SceneManager.SceneID>();

	public static bool UWAInitialized = false;

	private const string PREFS_GAME_CENTER_DECLINED = "GameCenterDeclinedByUser";

	public string IPCountry { get; set; } = "DEFAULT";

	public string RegCountry { get; set; } = "DEFAULT";

	public IPlatformNative Platform { get; private set; }

	public PlatformAndroid Android
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return Platform as PlatformAndroid;
			}
			return null;
		}
	}

	public string DistinctId { get; private set; }

	public string Version { get; private set; }

	public string VersionCode { get; private set; }

	public bool IsGoogleAvailable { get; private set; }

	public string AndroidScreenNotch { get; private set; }

	public string LaunchPushId
	{
		get
		{
			return m_launchPushId;
		}
		set
		{
			if (value != m_launchPushId)
			{
				m_launchPushId = value;
				if (!string.IsNullOrEmpty(m_launchPushId))
				{
					PostEventLog.TrackMap("IM_LAUNCH_GAME_FROM_PUSH", new Dictionary<string, object> { { "pushId", m_launchPushId } });
				}
			}
		}
	}

	public int CurrentThermalState { get; private set; }

	public void Initialize()
	{
		Platform = new PlatformAndroid("com.sdkmanager.SdkListener");
		Platform.InitPlatform("");
		LaunchPushId = Platform.GetLaunchPushID();
		Version = Application.version;
		VersionCode = Platform.GetDataFromNative("PM_getVersionCode", "");
		SetAndroidScreenNotch();
		runtimeInfo = new RuntimeInfoManager();
		runtimeInfo.Init();
		InitializePerformanceBI();
		InitShumeiSdk();
	}

	public void ApplicationWillEnterForeground()
	{
		LaunchPushId = Platform.GetLaunchPushID();
	}

	public void Shutdown()
	{
		payCallback_ = null;
		LogoutShumeiSdk();
	}

	public void SetPayMangerCallback(Action<string, string> action)
	{
		payCallback_ = action;
	}

	public string GetPublishRegion()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return GameEntry.Sdk.getChannel();
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return "AppStore";
		}
		return "";
	}

	public string GetPackageName()
	{
		if (NetworkURLConfig.IsOnline)
		{
			return "com.fun.lastwar.gp";
		}
		return Application.identifier;
	}

	public string GetPackageSign()
	{
		string packageName = GetPackageName();
		return BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(packageName))).Replace("-", string.Empty).ToLower();
	}

	public string getChannel()
	{
		return Platform.GetDataFromNative("PM_getPublishChannel", "");
	}

	public void saveDataToSdcard(string data, string path)
	{
		JsonData jsonData = new JsonData();
		jsonData["data"] = data;
		jsonData["filename"] = path;
		Platform.SendDataToNative("PM_saveDataToSdCard", jsonData.ToJson());
	}

	public void RequestSdCardPermission()
	{
		Platform.GetDataFromNative("PM_requestSdPermit", "");
	}

	public void DoInitGooglePay()
	{
		Platform.GetDataFromNative("PM_DoInitGooglePay", "");
	}

	public void DoInitShop()
	{
		if (IS_UNITY_ANDROID())
		{
			Platform.GetDataFromNative("PM_DoInitGooglePay", "");
		}
		else if (IS_UNITY_IOS())
		{
			Platform.GetDataFromNative("Pay_getBillingConfig", "");
		}
	}

	public bool IsSimulator()
	{
		if (IS_UNITY_ANDROID())
		{
			string dataFromNative = Platform.GetDataFromNative("IsSimulator", "");
			if (!string.IsNullOrEmpty(dataFromNative))
			{
				if (dataFromNative.Equals("True"))
				{
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void CheckDownloadGoogleApk()
	{
		Platform.GetDataFromNative("PM_checkDownloadApk", "");
	}

	public string GetDeviceUDID()
	{
		return Platform.GetDataFromNative("PM_getDeviceUDID", "");
	}

	public string GetSerialID()
	{
		return Platform.GetDataFromNative("PM_getSerialID", "");
	}

	public string GetDeviceInfo()
	{
		return Platform.GetDataFromNative("PM_getDeviceInfo", "");
	}

	public void SetAndroidScreenNotch()
	{
		AndroidScreenNotch = Platform.GetDataFromNative("PM_getAndroidScreenNotch", "");
	}

	public string GetHandSetInfo()
	{
		return Platform.GetDataFromNative("PM_getHandSetInfo", "");
	}

	public string GenerateHighVersionUUID()
	{
		return Platform.GetDataFromNative("PM_generateHighVersionUUID", "");
	}

	public string GetSimOperator()
	{
		return Platform.GetDataFromNative("PM_getSimOperator", "");
	}

	public string GetSimOperatorName()
	{
		return Platform.GetDataFromNative("PM_getSimOperatorName", "");
	}

	public string getClickPushTag()
	{
		return Platform.GetDataFromNative("PUSH_getPushTag", "");
	}

	public string getClickPushId()
	{
		return Platform.GetDataFromNative("PUSH_getPushId", "");
	}

	public string getClickPushTime()
	{
		return Platform.GetDataFromNative("PUSH_getPushTime", "");
	}

	public void ClearAllPushData()
	{
		Platform.SendDataToNative("PUSH_clearAllCache", "{}");
	}

	public void CopyTextToClipboard(string _content)
	{
		try
		{
			string data = new JsonData { ["content"] = _content }.ToJson();
			Platform.SendDataToNative("PM_copyTextToClipboard", data);
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
		}
	}

	public bool GetIsNotifyOpen()
	{
		return PushNoticeManager.GetIsNotifyOpen();
	}

	public void AskForNotifyPermission()
	{
		Platform.SendDataToNative("ACT_AskForNotifyPermission", "");
	}

	public bool IsTrackingEnabled()
	{
		return AppTrackingTransparency.TrackingAuthorizationStatus == AppTrackingTransparency.AuthorizationStatus.AUTHORIZED;
	}

	public void OpenSettings()
	{
		Platform.GetDataFromNative("PM_OpenSettings", "");
	}

	private void RequestTrackingAuthorization()
	{
		AppTrackingTransparency.RegisterAppForAdNetworkAttribution();
		if (AppTrackingTransparency.TrackingAuthorizationStatus != AppTrackingTransparency.AuthorizationStatus.AUTHORIZED)
		{
			AppTrackingTransparency.RequestTrackingAuthorization();
		}
	}

	public void LogEvent(string eventName, params object[] datas)
	{
		if (!CommonUtils.IsDebug())
		{
			string data = new JsonData
			{
				["uid"] = GameEntry.Data.Player.Uid,
				["key"] = eventName
			}.ToJson();
			SendDataToNative("FB_RecordEvent", data);
			RecordAppsflyer(eventName);
		}
	}

	public string GetBuildInfo()
	{
		return Platform.GetDataFromNative("LW_GetBuildInfo", "");
	}

	public string GetPermissionByType(string data)
	{
		return Platform.GetPermissionByType(data);
	}

	public void LogEventLevelUp(int lv)
	{
		if (!CommonUtils.IsDebug())
		{
			JsonData jsonData = new JsonData();
			jsonData["lv"] = lv;
			Platform.SendDataToNative("FB_LevelUp", jsonData.ToJson());
		}
	}

	public void Login(LoginPlatform loginPF, bool changeAccount = false, bool isBind = false)
	{
		JsonData jsonData = new JsonData();
		jsonData["platform"] = (int)loginPF;
		jsonData["changeAccount"] = changeAccount;
		jsonData["isBind"] = isBind;
		Platform.LoginPlatform = loginPF;
		Platform.SignIn(jsonData.ToJson());
	}

	public void SetAccountFunc(LoginPlatform loginPF, int accountFuncType)
	{
		JsonData jsonData = new JsonData();
		jsonData["platform"] = (int)loginPF;
		jsonData["accountFuncType"] = accountFuncType;
		Platform.LoginPlatform = loginPF;
		Platform.SendDataToNative("SetAccountFunc", jsonData.ToJson());
	}

	public void Logout()
	{
		Platform.SignOut();
	}

	public void Pay(string data)
	{
		Platform.Pay((int)Platform.PaymentChannel, data);
	}

	public int CheckPayEnv()
	{
		return Platform.GetDataFromNative("PAY_CheckPayEnv", "").ToInt();
	}

	public string Get_PF_DisplayName()
	{
		return Platform.GetDataFromNative("PF_DisplayName", "");
	}

	public void ConsumeProduct(string orderId, int state)
	{
		Platform.ConsumeProduct(orderId, state);
	}

	public void GotoMarket(string url, string urlCDN)
	{
		Android?.Call("GotoMarket", url, urlCDN);
	}

	public void SendDataToGame(string funcName, string data)
	{
		lock (m_Events)
		{
			if (m_Events.TryGetValue(funcName, out var value))
			{
				value.Add(data);
				return;
			}
			m_Events.Add(funcName, new List<string> { data });
		}
	}

	public void OnUpdate(float elapseSeconds)
	{
		Dictionary<string, List<string>> dictionary = null;
		lock (m_Events)
		{
			if (m_Events.Count > 0)
			{
				dictionary = m_Events;
				m_Events = new Dictionary<string, List<string>>();
			}
		}
		if (dictionary == null)
		{
			return;
		}
		bool isGM = GrayUtils.isGM;
		foreach (KeyValuePair<string, List<string>> item in dictionary)
		{
			for (int i = 0; i < item.Value.Count; i++)
			{
				if (isGM || GrayUtils.hasFileLog)
				{
					Log.Info("[SDK] HandleEvent " + item.Key + ", " + item.Value[i]);
				}
				HandleEvent(item.Key, item.Value[i]);
			}
		}
	}

	public void HandleEvent(string funcName, string data)
	{
		try
		{
			if (funcName.StartsWith("Zendesk_"))
			{
				ZendeskCore.OnNativeCallback(funcName, data);
				return;
			}
			if (funcName.StartsWith("GameCenter_"))
			{
				GameCenterBridge.OnNativeCallback(funcName, data);
				return;
			}
			if (funcName.StartsWith("PGS_"))
			{
				PlayGamesBridge.OnNativeCallback(funcName, data);
				return;
			}
			if (funcName.StartsWith("Applovin_"))
			{
				ApplovinManager.Instance.OnNativeCallback(funcName, data);
				return;
			}
			switch (funcName)
			{
			case "onSDKInit":
			{
				JsonData jsonData12 = JsonMapper.ToObject(data);
				IsGoogleAvailable = (bool)jsonData12["1"];
				GameEntry.GlobalData.SetAnalyticID();
				HelpManager.Instance.init();
				break;
			}
			case "onSignInCallback":
			{
				JsonData result3 = JsonMapper.ToObject(data);
				OnLoginCallBack(result3);
				break;
			}
			case "OnSetAccountFuncCallback":
			{
				JsonData result2 = JsonMapper.ToObject(data);
				OnSetAccountFuncCallback(result2);
				break;
			}
			case "PostEvent":
				PostEventLog.Track(data, null);
				break;
			case "PostEventData":
				try
				{
					JsonData jsonData2 = JsonMapper.ToObject(data);
					if (!string.IsNullOrEmpty((string)jsonData2["eventName"]))
					{
						string eventName = (string)jsonData2["eventName"];
						if (!string.IsNullOrEmpty((string)jsonData2["data"]))
						{
							PostEventLog.TrackMap(eventName, new Dictionary<string, object> { 
							{
								"errMsg",
								(string)jsonData2["data"]
							} });
						}
						else
						{
							PostEventLog.TrackMap(eventName, null);
						}
					}
					break;
				}
				catch (Exception)
				{
					Log.Error("PostEventData  error:" + data);
					break;
				}
			case "onSignOutCallback":
			{
				if (int.TryParse(JsonUtility.FromJson<PlatformResult>(data).code, out var result4))
				{
					switch (result4)
					{
					case 1:
						Log.Info("登出成功~");
						break;
					case -2:
						Log.Info("没有登陆～");
						break;
					default:
						Log.Info("登出失败~");
						break;
					}
				}
				break;
			}
			case "PermissionRecv":
				if ((bool)ApplicationLaunch.Instance && ApplicationLaunch.Instance.Loading != null)
				{
					ApplicationLaunch.Instance.Loading.PermissionRecv = true;
				}
				break;
			case "Get_Install_Referrer":
				BIManager.SendToBI("install_referer", new Dictionary<string, object> { { "referer", data } });
				break;
			case "getHeadImgUrl":
			{
				JsonData jsonData13 = JsonMapper.ToObject(data);
				string filePath2 = "";
				if (jsonData13 != null)
				{
					filePath2 = (string)jsonData13["1"];
				}
				UploadImageManager.Instance.OnImagePathOk(filePath2);
				break;
			}
			case "GetChatPhotoUrl":
			{
				JsonData jsonData11 = JsonMapper.ToObject(data);
				string filePath = "";
				string s = "";
				string s2 = "";
				if (jsonData11 != null)
				{
					filePath = (string)jsonData11["photoUrl"];
					s = (string)jsonData11["compressedWidth"];
					s2 = (string)jsonData11["compressedHeight"];
				}
				UploadImageManager.Instance.FinishedSelectSingleImage(filePath, int.Parse(s), int.Parse(s2));
				break;
			}
			case "setGaid":
			{
				JsonData jsonData4 = JsonMapper.ToObject(data);
				string text3 = (string)jsonData4["1"];
				((string)jsonData4["2"]).Equals("true");
				Log.Info("gaid {0}, gaidCache {1}", text3, GameEntry.GlobalData.gaidCache);
				if (GameEntry.GlobalData.gaidCache != null && GameEntry.GlobalData.gaidCache.Equals("missed"))
				{
					GameEntry.GlobalData.gaid = text3;
					UserBindGaidMessage.Instance.Send(new UserBindGaidMessage.Request
					{
						gaid = text3
					});
				}
				else
				{
					GameEntry.GlobalData.gaidCache = text3;
				}
				break;
			}
			case "registerdParseAccount":
			{
				JsonData jsonData = JsonMapper.ToObject(data);
				string text2 = (string)jsonData["1"];
				string strPlatform = (string)jsonData["2"];
				if (!string.IsNullOrEmpty(text2))
				{
					GameEntry.GlobalData.parseRegisterId = text2;
				}
				PushManager.Instance.registerdParseAccount(text2, strPlatform);
				break;
			}
			case "SetFireBaseId":
			{
				string fireBaseId = (string)JsonMapper.ToObject(data)["firebaseId"];
				PushManager.Instance.SetFireBaseId(fireBaseId);
				break;
			}
			case "setFromCountry":
			{
				string text = (string)JsonMapper.ToObject(data)["1"];
				Log.Info("setFromCountry fromCountry {0}", text);
				GameEntry.GlobalData.fromCountry = text;
				PrivacyFuncUtil.Instance.Country = text;
				Debug.Log("[COPPA] setFromCountry: " + text);
				ZendeskDefine.UserConfig.Update("Country_Code", GameEntry.GlobalData.fromCountry);
				FibMatrix.Logger.CurrentRemoteLoggerTarget?.UpdateCountry(text);
				break;
			}
			case "setVersionAndCode":
			{
				JsonData jsonData14 = JsonMapper.ToObject(data);
				Version = (string)jsonData14["1"];
				VersionCode = (string)jsonData14["2"];
				break;
			}
			case "onGooglePayInitResult":
				if (payCallback_ != null)
				{
					payCallback_("onGooglePayInitResult", data);
				}
				break;
			case "onCallPayInfoFail":
				try
				{
					PostEventLog.Track("gp_pay_callinfo_fail", data);
					break;
				}
				catch (Exception)
				{
					Log.Error("gp_pay_callinfo_fail  res:" + data);
					break;
				}
			case "onPurchaseQueried":
				if (payCallback_ != null)
				{
					payCallback_("onPurchaseQueried", data);
				}
				else if (ClientSwitch.IsOn(43))
				{
					GameEntry.PayOrderData.SaveNativeQueryPriceResult(data);
				}
				break;
			case "onPurchaseCallback":
				if (payCallback_ != null)
				{
					payCallback_("onPurchaseCallback", data);
				}
				break;
			case "OnThermalStateChange":
				CurrentThermalState = int.Parse(data);
				break;
			case "onRequestPermissionsResult":
				try
				{
					JsonData jsonData7 = JsonMapper.ToObject(data);
					if (jsonData7.Count < 3)
					{
						Log.Error("onRequestPermissionsResult recvData error:" + data);
						break;
					}
					switch (jsonData7[0].ToInt())
					{
					case 1001:
					{
						JsonData jsonData9 = jsonData7[1];
						JsonData jsonData10 = jsonData7[2];
						if (jsonData9 != null)
						{
							for (int j = 0; j < jsonData9.Count; j++)
							{
								string text4 = jsonData9[j].ToString();
								Debug.Log("onRequestPermissionsResult Permission " + j + ": " + text4);
							}
						}
						else
						{
							Log.Error("onRequestPermissionsResult permissionsJson error:" + data);
						}
						if (jsonData10 != null)
						{
							bool flag2 = true;
							for (int k = 0; k < jsonData10.Count; k++)
							{
								int num = jsonData10[k].ToInt();
								Debug.Log("onRequestPermissionsResult GrantResult " + k + ": " + num);
								if (num == -1)
								{
									flag2 = false;
								}
							}
							if (flag2)
							{
								if (!string.IsNullOrEmpty(writeAlbumFileName) && !string.IsNullOrEmpty(writeAlbumFilePath))
								{
									SaveImageToPhotoAndroid(writeAlbumFilePath, writeAlbumFileName);
								}
								else
								{
									Log.Error("onRequestPermissionsResult writeAlbumFilePath is NullOrEmpty:");
								}
							}
							else
							{
								UIUtils.ShowTips("season_alliance_photo_tips_29", 3f);
							}
						}
						else
						{
							Log.Error("onRequestPermissionsResult grantResultJson error:" + data);
						}
						writeAlbumFileName = string.Empty;
						writeAlbumFilePath = string.Empty;
						break;
					}
					case 1002:
					{
						_ = jsonData7[1];
						JsonData jsonData8 = jsonData7[2];
						if (jsonData8 == null)
						{
							break;
						}
						bool flag = true;
						for (int i = 0; i < jsonData8.Count; i++)
						{
							if (jsonData8[i].ToInt() == -1)
							{
								flag = false;
							}
						}
						if (flag)
						{
							TrySendAddCalendarEvent();
						}
						else
						{
							UIUtils.ShowTips("calendar_tips3", 3f);
						}
						break;
					}
					}
					break;
				}
				catch (Exception)
				{
					Log.Error("onRequestPermissionsResult jsonData  error:" + data);
					break;
				}
			case "AddCalendarEventBackFunc":
				UIUtils.ShowTips("calendar_tips4", 3f);
				if (calendarEventJsonParam != null)
				{
					JsonData jsonData6 = JsonMapper.ToObject(calendarEventJsonParam);
					PostEventLog.TrackMap("c_calendar_use", new Dictionary<string, object>
					{
						{
							"id",
							(int)jsonData6["cfgId"]
						},
						{
							"source_type",
							(string)jsonData6["sourcePath"]
						},
						{ "data", "success" }
					});
				}
				break;
			case "AddCalendarEventCalIdNull":
				UIUtils.ShowTips("calendar_tips6", 3f);
				if (calendarEventJsonParam != null)
				{
					JsonData jsonData5 = JsonMapper.ToObject(calendarEventJsonParam);
					PostEventLog.TrackMap("c_calendar_use", new Dictionary<string, object>
					{
						{
							"id",
							(int)jsonData5["cfgId"]
						},
						{
							"source_type",
							(string)jsonData5["sourcePath"]
						},
						{ "data", "fail" }
					});
				}
				break;
			case "AddCalendarEventFail":
				UIUtils.ShowTips("calendar_tips5", 3f);
				if (calendarEventJsonParam != null)
				{
					JsonData jsonData3 = JsonMapper.ToObject(calendarEventJsonParam);
					PostEventLog.TrackMap("c_calendar_use", new Dictionary<string, object>
					{
						{
							"id",
							(int)jsonData3["cfgId"]
						},
						{
							"source_type",
							(string)jsonData3["sourcePath"]
						},
						{ "data", "fail" }
					});
				}
				break;
			case "FB_Login_Callback":
			{
				JsonData result = JsonMapper.ToObject(data);
				OnLoginCallBack(result);
				break;
			}
			case "Log_Info":
				if (data != null)
				{
					Log.Info(data);
				}
				break;
			case "ExceedResolutionLimit":
				UIUtils.ShowTips("picture_reject_toast_size", 3f);
				break;
			case "ExceedFileSizeLimit":
				UIUtils.ShowTips("picture_reject_toast_size1", 3f);
				break;
			case "onCallPayConfig":
				if (GameEntry.PayOrderData != null)
				{
					Log.Info("Native country : " + data);
					GameEntry.PayOrderData.SaveStorefrontCode(data);
				}
				if (payCallback_ != null)
				{
					payCallback_("onCallPayConfig", data);
				}
				break;
			case "onCallPayConfigFail":
				if (payCallback_ != null)
				{
					payCallback_("onCallPayConfigFail", data);
				}
				break;
			case "onGetFormattedPrice":
				if (payCallback_ != null)
				{
					payCallback_("onGetFormattedPrice", data);
				}
				break;
			case "OnShumeiSdkInitSuccess":
				ShumeiSdkManager.Instance.OnShumeiSdkInitSuccess();
				break;
			case "OnShumeiSdkInitError":
				ShumeiSdkManager.Instance.OnShumeiSdkInitError();
				break;
			case "Odm2_PostAggregateConversionInfo":
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append('{');
				stringBuilder.Append("\"data\":\"");
				stringBuilder.Append(data);
				stringBuilder.Append("\"");
				stringBuilder.Append('}');
				PostEventLog.Track("c_odm_info", stringBuilder.ToString());
				break;
			}
			}
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
		}
	}

	public void SendDataToNative(string funcName, string data)
	{
		if (GrayUtils.isGM || GrayUtils.hasFileLog)
		{
			Log.Info("[SDK] SendDataToNative " + funcName + ", " + data);
		}
		Platform.SendDataToNative(funcName, data);
	}

	public string GetDataFromNative(string funcName, string data)
	{
		if (GrayUtils.hasFileLog)
		{
			Log.Info("[SDK] GetDataFromNative " + funcName + ", " + data);
		}
		return Platform.GetDataFromNative(funcName, data);
	}

	private void OnLoginCallBack(JsonData result)
	{
		Log.Info($"OnLoginCallBack loginPlatform={Platform.LoginPlatform} json={result.ToJson()}");
		LoginPlatform loginPlatform;
		if (!result.Keys.Contains("errorNo"))
		{
			Platform.UID = (string)result["uid"];
			bool flag = (bool)result["isBind"];
			Log.Info($"SDK登录成功~{flag}");
			ISFSObject iSFSObject = SFSObject.NewInstance();
			loginPlatform = Platform.LoginPlatform;
			if (loginPlatform == LoginPlatform.GooglePlay && flag)
			{
				iSFSObject.PutUtfString("msgId", "login_sucess_google");
			}
			if (flag)
			{
				iSFSObject.PutUtfString("userId", (string)result["uid"]);
				if (!result.Keys.Contains("displayName"))
				{
					Log.Error("OnLoginCallBack  从原生返回的Json字符串不包含GP姓名！json=" + result.ToJson());
					return;
				}
				iSFSObject.PutUtfString("userName", (string)result["displayName"]);
				iSFSObject.PutUtfString("email", result.Keys.Contains("email") ? ((string)result["email"]) : "");
				iSFSObject.PutUtfString("idToken", result.Keys.Contains("idToken") ? ((string)result["idToken"]) : "");
				iSFSObject.PutUtfString("accessToken", result.Keys.Contains("accessToken") ? ((string)result["accessToken"]) : "");
				iSFSObject.PutUtfString("picUrl", result.Keys.Contains("picUrl") ? ((string)result["picUrl"]) : "");
				iSFSObject.PutUtfString("gender", result.Keys.Contains("gender") ? ((string)result["gender"]) : "");
				Log.Info("OnLoginCallBack   LwAccount C# Fire " + iSFSObject.ToJson());
				GameEntry.Event.Fire(EventId.MSG_RESPONSED3RDPLATFORM, iSFSObject);
			}
			return;
		}
		string text = result["errorNo"].ToString();
		Log.Info("OnLoginCallBack  SDK登录失败~errorNo=" + text);
		bool flag2 = (bool)result["isBind"];
		loginPlatform = Platform.LoginPlatform;
		if (loginPlatform != LoginPlatform.GooglePlay)
		{
			return;
		}
		Debug.Log("OnLoginCallBack  google c# loginfail");
		int num = GameEntry.Setting.GetInt("google_login_fail_cnt", 0);
		num++;
		GameEntry.Setting.SetInt("google_login_fail_cnt", num);
		GameEntry.Setting.Save();
		if (flag2)
		{
			ISFSObject iSFSObject2 = SFSObject.NewInstance();
			if (text == "12501")
			{
				iSFSObject2.PutUtfString("msgId", "login_canceled_google");
			}
			else
			{
				iSFSObject2.PutUtfString("msgId", "login_failed_google");
			}
			iSFSObject2.PutUtfString("errorNo", text);
			GameEntry.Event.Fire(EventId.MSG_RESPONSED3RDPLATFORM, iSFSObject2);
		}
	}

	private void OnSetAccountFuncCallback(JsonData result)
	{
		Log.Info($"OnSetAccountFuncCallback loginPlatform={Platform.LoginPlatform} json={result.ToJson()}");
		LoginPlatform loginPlatform;
		if (!result.Keys.Contains("errorNo"))
		{
			Platform.UID = (string)result["uid"];
			int num = (int)result["accountFuncType"];
			Log.Info($"SDK登录成功~{num}");
			ISFSObject iSFSObject = SFSObject.NewInstance();
			loginPlatform = Platform.LoginPlatform;
			if (loginPlatform == LoginPlatform.GooglePlay)
			{
				iSFSObject.PutUtfString("msgId", "login_sucess_google");
			}
			iSFSObject.PutInt("accountFuncType", (int)result["accountFuncType"]);
			iSFSObject.PutUtfString("userId", (string)result["uid"]);
			if (!result.Keys.Contains("displayName"))
			{
				Log.Error("OnSetAccountFuncCallback   从原生返回的Json字符串不包含GP姓名！json=" + result.ToJson());
				return;
			}
			iSFSObject.PutUtfString("userName", (string)result["displayName"]);
			iSFSObject.PutUtfString("email", result.Keys.Contains("email") ? ((string)result["email"]) : "");
			iSFSObject.PutUtfString("idToken", result.Keys.Contains("idToken") ? ((string)result["idToken"]) : "");
			Log.Info("OnSetAccountFuncCallback C# Fire " + iSFSObject.ToJson());
			GameEntry.Event.Fire(EventId.Respond_From_3rd_Platform, iSFSObject);
			return;
		}
		string text = result["errorNo"].ToString();
		Log.Info("OnSetAccountFuncCallback  SDK登录失败~errorNo=" + text);
		loginPlatform = Platform.LoginPlatform;
		if (loginPlatform == LoginPlatform.GooglePlay)
		{
			Debug.Log("OnSetAccountFuncCallback  google c# loginfail");
			ISFSObject iSFSObject2 = SFSObject.NewInstance();
			if (text == "12501")
			{
				iSFSObject2.PutUtfString("msgId", "login_canceled_google");
			}
			else
			{
				iSFSObject2.PutUtfString("msgId", "login_failed_google");
			}
			iSFSObject2.PutUtfString("errorNo", text);
			GameEntry.Event.Fire(EventId.Respond_From_3rd_Platform, iSFSObject2);
		}
	}

	public void HideSplash()
	{
		if (!hadCallHideSplash)
		{
			hadCallHideSplash = true;
			PostEventLog.TrackMap("hide_splash", AddBILaunchTimeProperty(StartupConfig.Inst.AddBIProperty()));
		}
		Android?.Call("HideSplash");
	}

	public bool IsShowLogoOk()
	{
		bool? flag = true;
		return new bool?(Time.realtimeSinceStartup - ApplicationLaunch.awakeTime >= 0.5f).Value;
	}

	public static bool IS_UNITY_ANDROID()
	{
		return true;
	}

	public static bool IS_UNITY_IOS()
	{
		return false;
	}

	public static bool IS_UNITY_IPHONE()
	{
		return false;
	}

	public static bool IS_UNITY_EDITOR()
	{
		return false;
	}

	public static bool IS_UNITY_STANDALONE()
	{
		return false;
	}

	public static bool IS_IPhonePlayer()
	{
		return Application.platform == RuntimePlatform.IPhonePlayer;
	}

	public static bool IS_Android()
	{
		return Application.platform == RuntimePlatform.Android;
	}

	public bool IsKoreaRegion()
	{
		string storefrontCode = GameEntry.PayOrderData.GetStorefrontCode();
		if (!string.IsNullOrEmpty(storefrontCode))
		{
			storefrontCode = storefrontCode.ToUpperInvariant();
			if (storefrontCode == "KR" || storefrontCode == "KOR")
			{
				return true;
			}
		}
		if (RegCountry == "KR")
		{
			return true;
		}
		if (GameEntry.Localization.Language == Language.Korean)
		{
			return true;
		}
		return false;
	}

	public void requestFCMToken()
	{
		SendDataToNative("FireBase_getFCMToken", "");
	}

	public void CrashlyticsSetCustomValue(string key, string value)
	{
		string data = new JsonData
		{
			["key"] = key,
			["value"] = value
		}.ToJson();
		SendDataToNative("FireBase_CrashlyticsSetCustomValue", data);
	}

	public void CrashlyticsAddLog(string log)
	{
		SendDataToNative("FireBase_CrashlyticsAddCustomLog", log);
	}

	public void CrashlyticsSetUserId(string userId)
	{
		SendDataToNative("FireBase_CrashlyticsSetUserId", userId);
	}

	public void AnalyticsAccountEmail(string accountEmail)
	{
		SendDataToNative("FireBase_AccountEmailAnalytics", accountEmail);
	}

	public void initAppsFlyer(string gameUid)
	{
		try
		{
			string data = new JsonData { ["uid"] = gameUid }.ToJson();
			SendDataToNative("PM_InitAppFlyer", data);
		}
		catch (Exception message)
		{
			Log.Warning(message);
		}
	}

	public void InitTrackingData(string discard)
	{
		try
		{
			ThinkingAnalyticsAPI.SetSuperProperties(new Dictionary<string, object>
			{
				{
					"lw_first_launch",
					GameEntry.Setting.IsFirstLaunch()
				},
				{
					"lw_game_session_id",
					GameUtility.GetGameSessionId()
				}
			});
			ThinkingAnalyticsAPI.SetDynamicSuperProperties(new LWDynamicSuperProperties());
			ThinkingAnalyticsAPI.TAMode mode = ThinkingAnalyticsAPI.TAMode.NORMAL;
			ThinkingAnalyticsAPI.TATimeZone timeZone = ThinkingAnalyticsAPI.TATimeZone.Local;
			ThinkingAnalyticsAPI.Token token = new ThinkingAnalyticsAPI.Token("4d9a658f7f364b069804f7fd68fcda54", "https://te-receiver.lastwar.com", mode, timeZone);
			new GameObject("ThinkingAnalytics", typeof(ThinkingAnalyticsAPI));
			ThinkingAnalyticsAPI.StartThinkingAnalytics(token);
			if (CommonUtils.IsDebug())
			{
				ThinkingAnalyticsAPI.EnableAutoTrack(AUTO_TRACK_EVENTS.APP_START | AUTO_TRACK_EVENTS.APP_END | AUTO_TRACK_EVENTS.APP_INSTALL);
				ThinkingAnalyticsAPI.EnableLog(enable: true);
			}
			else
			{
				ThinkingAnalyticsAPI.EnableAutoTrack(AUTO_TRACK_EVENTS.APP_START | AUTO_TRACK_EVENTS.APP_END | AUTO_TRACK_EVENTS.APP_INSTALL | AUTO_TRACK_EVENTS.APP_SCENE_LOAD | AUTO_TRACK_EVENTS.APP_SCENE_UNLOAD);
				ThinkingAnalyticsAPI.EnableLog(enable: false);
			}
			string text = (DistinctId = ThinkingAnalyticsAPI.GetDistinctId());
			ThinkingAnalyticsAPI.EnableThirdPartySharing(TAThirdPartyShareType.APPSFLYER);
			if (IsNativeAppsFlyerAvailable())
			{
				string data = new JsonData { ["ta_distinct_id"] = text }.ToJson();
				SendDataToNative("LW_AF_SetAdditionalData", data);
			}
			else
			{
				AppsFlyer.setAdditionalData(new Dictionary<string, string> { { "ta_distinct_id", text } });
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> item in JsonMapper.ToObject<Dictionary<string, object>>(JsonUtility.ToJson(new DeviceInfo())))
			{
				string key = item.Key;
				object value = item.Value;
				dictionary["dv_" + key] = value.ToString();
			}
			ThinkingAnalyticsAPI.Track("system_info", dictionary);
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
	}

	public void SetDeviceIdSuperProperty()
	{
		string value = PlayerPrefs.GetString("LW_CacheShumeiDeviceId", "");
		Dictionary<string, object> obj = ThinkingAnalyticsAPI.GetSuperProperties() ?? new Dictionary<string, object>();
		obj["lw_device_id"] = GameEntry.Device.GetDeviceUid();
		obj["lw_airKey"] = GameEntry.Device.GetDeviceUid_Transcoding();
		obj["lw_shumei_id"] = value;
		ThinkingAnalyticsAPI.SetSuperProperties(obj);
	}

	public void SetDeviceLevelSuperProperty()
	{
		Dictionary<string, object> obj = ThinkingAnalyticsAPI.GetSuperProperties() ?? new Dictionary<string, object>();
		obj["lw_device_level"] = (int)runtimeInfo.GetDeviceLevel();
		ThinkingAnalyticsAPI.SetSuperProperties(obj);
	}

	public void LoginTracking(string gameUid)
	{
		try
		{
			string distinctId = ThinkingAnalyticsAPI.GetDistinctId();
			ThinkingAnalyticsAPI.Login(gameUid);
			ThinkingAnalyticsAPI.EnableThirdPartySharing(TAThirdPartyShareType.APPSFLYER);
			if (IsNativeAppsFlyerAvailable())
			{
				string data = new JsonData
				{
					["ta_account_id"] = gameUid,
					["ta_distinct_id"] = distinctId
				}.ToJson();
				SendDataToNative("LW_AF_SetAdditionalData", data);
			}
			else
			{
				AppsFlyer.setAdditionalData(new Dictionary<string, string>
				{
					{ "ta_account_id", gameUid },
					{ "ta_distinct_id", distinctId }
				});
			}
			string value = GameEntry.Setting.GetString("SERVER_ZONE", "");
			ThinkingAnalyticsAPI.UserSet(new Dictionary<string, object>
			{
				{
					"lwu_version",
					GameEntry.Sdk.Version
				},
				{
					"lwu_buildcode",
					GameEntry.Sdk.VersionCode
				},
				{
					"lwu_platform",
					string.IsNullOrEmpty(GameEntry.GlobalData.analyticID) ? "\"\"" : GameEntry.GlobalData.analyticID
				},
				{
					"lwu_main_level",
					GameEntry.Data.Building.GetMainLv()
				},
				{
					"lwu_device_id",
					GameEntry.Device.GetDeviceUid()
				},
				{
					"lwu_airKey",
					GameEntry.Device.GetDeviceUid_Transcoding()
				}
			});
			string value2 = PlayerPrefs.GetString("LW_CacheShumeiDeviceId", "");
			ThinkingAnalyticsAPI.SetSuperProperties(new Dictionary<string, object>
			{
				{ "lw_zone", value },
				{
					"lw_version",
					GameEntry.Sdk.Version
				},
				{
					"lw_res_version",
					GameEntry.Resource.GetResVersion()
				},
				{
					"lw_buildcode",
					GameEntry.Sdk.VersionCode
				},
				{
					"lw_net",
					GameEntry.Device.GetNetworkTypeDesc()
				},
				{
					"lw_line",
					GameEntry.Network.getCurLine()
				},
				{
					"lw_platform",
					string.IsNullOrEmpty(GameEntry.GlobalData.analyticID) ? "\"\"" : GameEntry.GlobalData.analyticID
				},
				{
					"lw_main_level",
					GameEntry.Data.Building.GetMainLv()
				},
				{
					"lw_device_id",
					GameEntry.Device.GetDeviceUid()
				},
				{
					"lw_airKey",
					GameEntry.Device.GetDeviceUid_Transcoding()
				},
				{
					"lw_first_launch",
					GameEntry.Setting.IsFirstLaunch()
				},
				{
					"pd_dl",
					PerformanceMetrics.DeviceLevel.Value
				},
				{ "lw_shumei_id", value2 }
			});
		}
		catch (Exception message)
		{
			Log.Warning(message);
		}
	}

	public void Restart(string restartData)
	{
		Platform.Restart(restartData);
	}

	public string GetRestartData()
	{
		return Platform.GetRestartData();
	}

	public static bool IsNativeAppsFlyerAvailable()
	{
		Log.Info(string.Format("IsNativeAppsFlyerAvailable {0}  ver:{1}", StringUtils.VersionCompare(Application.version, "1.0.188") >= 0, Application.version));
		return StringUtils.VersionCompare(Application.version, "1.0.188") >= 0;
	}

	public static Dictionary<string, object> AddBILaunchTimeProperty(Dictionary<string, object> prop = null)
	{
		if (prop == null)
		{
			prop = new Dictionary<string, object>(2);
		}
		prop["disconnect_retry_count"] = ApplicationLaunch.Instance.disconnectRetryCount;
		prop["hotUpdate_count"] = ApplicationLaunch.Instance.hotUpdateCount;
		return prop;
	}

	public static string GetTaPresetProp()
	{
		string text = Json.Encode(ThinkingAnalyticsAPI.GetPresetProperties().ToEventPresetProperties().Concat(ThinkingAnalyticsAPI.GetSuperProperties())
			.ToDictionary((KeyValuePair<string, object> x) => x.Key, (KeyValuePair<string, object> x) => x.Value));
		if (text == null)
		{
			return "{}";
		}
		return text;
	}

	public static string GetTaDistinctId()
	{
		return ThinkingAnalyticsAPI.GetDistinctId();
	}

	public void RecordAppsflyer(string key)
	{
		try
		{
			string data = new JsonData
			{
				["uid"] = GameEntry.Data.Player.Uid,
				["key"] = key
			}.ToJson();
			SendDataToNative("PM_RecordAppsflyer", data);
		}
		catch (Exception message)
		{
			Log.Warning(message);
		}
	}

	public void SetAppsFlyerPurchase(string cost, string itemId)
	{
	}

	public void SetFacebookPurchaseEvent(string cost, string itemId)
	{
		if (CommonUtils.IsDebug())
		{
			return;
		}
		try
		{
			string data = new JsonData
			{
				["cost"] = cost,
				["itemId"] = itemId
			}.ToJson();
			SendDataToNative("FB_SetPurchaseEventNew", data);
		}
		catch (Exception message)
		{
			Log.Warning(message);
		}
	}

	public string GetAppsFlyerUid()
	{
		return GetDataFromNative("AF_getAppsFlyerUid", "");
	}

	public void SetUserId(string uid)
	{
		SendDataToNative("LW_SetUserId", uid);
	}

	public void OnUploadPhoto(string uid, int code, int idx)
	{
		try
		{
			string data = new JsonData
			{
				["uid"] = uid,
				["code"] = code,
				["idx"] = idx
			}.ToJson();
			if (IS_IPhonePlayer() && isUploadImageFix && StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.320") >= 0)
			{
				SendDataToNative("PM_OnUploadPhotoNew", data);
			}
			else
			{
				SendDataToNative("PM_OnUploadPhoto", data);
			}
		}
		catch (Exception message)
		{
			Log.Warning(message);
		}
	}

	public void OnUploadPhoto_Chat(int code, int photoResolutionLimit, int photoFileSizeLimit, int suitableResolutionSizeBig, int suitableResolutionSizeSmall)
	{
		try
		{
			string data = new JsonData
			{
				["code"] = code,
				["photoResolutionLimit"] = photoResolutionLimit,
				["photoFileSizeLimit"] = photoFileSizeLimit,
				["suitableResolutionSizeBig"] = suitableResolutionSizeBig,
				["suitableResolutionSizeSmall"] = suitableResolutionSizeSmall
			}.ToJson();
			if (IS_IPhonePlayer() && isUploadImageFix && StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.320") >= 0)
			{
				SendDataToNative("PM_OnUploadPhotoNew", data);
			}
			else
			{
				SendDataToNative("PM_OnUploadPhoto", data);
			}
		}
		catch (Exception message)
		{
			Log.Warning(message);
		}
	}

	public void Odm2_PostAggregateConversionInfo()
	{
		SendDataToNative("Odm2_PostAggregateConversionInfo", "");
	}

	public string GetPlatformNameForBI()
	{
		string packageName = GetPackageName();
		if (packageName == "com.fun.lastwar.gp")
		{
			return "googleplay";
		}
		if (packageName == "com.lastwar.ios")
		{
			return "appiosglobal";
		}
		return "undefine";
	}

	public bool IsVNPlatform()
	{
		string packageName = GetPackageName();
		if (!(packageName == "com.fun.lastwar.vn.gp"))
		{
			return packageName == "com.fun.lastwar.vn.ios";
		}
		return true;
	}

	public void DMAPrivacyAllowed()
	{
		InitGameCenter();
		PlayGamesBridge.AutoLogin();
		SendDataToNative("LW_DMAPrivacyAllowed", "");
	}

	public void RequestInAppReview()
	{
		SendDataToNative("RequestStoreReview", "");
	}

	private void InitializePerformanceBI()
	{
		if (m_AssembliesUpdated == -1)
		{
			string path = Path.Combine(Runtime.persistentDataPath, "Assemblies");
			m_AssembliesUpdated = ((Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any()) ? 1 : 0);
		}
		PerformanceMetrics.AssembliesUpdated.CustomProvider = () => Convert.ToBoolean(m_AssembliesUpdated);
		PerformanceMetrics.LogicModuleTag.CustomProvider = () => m_SceneID2Name[(SceneManager.SceneID)SceneManager.CurrSceneID];
		PerformanceMetrics.LogicModuleSourceTag.CustomProvider = () => SceneManager.CurrentSceneSubType;
		PerformanceMetrics.DeviceLevel.CustomProvider = () => (!PlayerPrefs.HasKey("SCENE_FPS_LEVEL")) ? (-1) : PlayerPrefs.GetInt("SCENE_FPS_LEVEL");
		PerformanceMetrics.PowerSaving.CustomProvider = () => PlayerPrefs.HasKey("POWER_SAVING_MODE") && Convert.ToBoolean(PlayerPrefs.GetInt("POWER_SAVING_MODE"));
		PerformanceMetrics.Platform.CustomProvider = () => GetPlatformNameForBI();
		PerformanceMetrics.DeviceScore.CustomProvider = () => runtimeInfo.GetDeviceScore();
		PerformanceMetrics.MemoryLuaVMLong.CustomProvider = () => (GameEntry.Lua?.Env != null) ? ((long)GameEntry.Lua.Env.Memroy << 10) : 0;
		if (StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.293") >= 0 || CommonUtils.IsDebug())
		{
			PerformanceMetrics.BatteryVoltage.CustomProvider = () => float.TryParse(GetDataFromNative("PM_getBatteryVoltage", ""), out var result) ? result : 0f;
			PerformanceMetrics.BatteryCurrent.CustomProvider = () => float.TryParse(GetDataFromNative("PM_getBatteryCurrent", ""), out result) ? result : 0f;
			PerformanceMetrics.PowerNativeSaveMode.CustomProvider = () => GetDataFromNative("PM_getPowerSaveMode", "");
		}
		if (StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.304") >= 0 || CommonUtils.IsDebug())
		{
			CurrentThermalState = -1;
		}
		m_PerformanceBIScheduler = new PerformanceBIScheduler();
		m_PerformanceBIScheduler.Start();
	}

	private void DeinitializePerformanceBI()
	{
		m_PerformanceBIScheduler.Destroy();
		PerformanceMetrics.AssembliesUpdated.CustomProvider = null;
		PerformanceMetrics.LogicModuleTag.CustomProvider = null;
		PerformanceMetrics.LogicModuleSourceTag.CustomProvider = null;
		PerformanceMetrics.DeviceLevel.CustomProvider = null;
		PerformanceMetrics.PowerSaving.CustomProvider = null;
		PerformanceMetrics.Platform.CustomProvider = null;
		PerformanceMetrics.MemoryLuaVMLong.CustomProvider = null;
		PerformanceMetrics.CurrentMemoryLong.CustomProvider = null;
	}

	public void ReportIOSCustomDylibNames()
	{
	}

	private void InitShumeiSdk()
	{
		ShumeiSdkManager.Instance.Init();
	}

	private void LogoutShumeiSdk()
	{
		ShumeiSdkManager.Instance.Logout();
	}

	public long GetRealtime()
	{
		return Platform.GetRealtime();
	}

	public void GetFormattedPrice(double price)
	{
		Platform.SendDataToNative("Pay_getFormattedCurrency", price.ToString());
	}

	public static void SaveToAlbum(string filePath, string fileName)
	{
		if (StringUtils.VersionCompare(Application.version, "1.0.282") < 0)
		{
			UIUtils.ShowTips("season_alliance_photo_tips_31", 3f);
			return;
		}
		string dataFromNative = GameEntry.Sdk.GetDataFromNative("HasWriteAlbumPermission", "");
		Log.Info("SaveToAlbum HasWriteAlbumPermission: {0}", dataFromNative);
		if (dataFromNative == "true")
		{
			Log.Info("SaveImageToPhotoAndroid,filePath {0}; fileName {1}", filePath, fileName);
			SaveImageToPhotoAndroid(filePath, fileName);
		}
		else
		{
			writeAlbumFileName = fileName;
			writeAlbumFilePath = filePath;
			GameEntry.Sdk.SendDataToNative("RequestWriteAlbumPermission", 1001.ToString());
		}
	}

	private void SaveCalendarEventJsonParam(string title, string location, long startTime, long endTime, int haveAlarm, int alarmTime, string calendarItemExternalIdentifier, int cfgId, string sourcePath)
	{
		JsonData jsonData = new JsonData();
		jsonData["title"] = title;
		jsonData["location"] = location;
		jsonData["startTime"] = startTime;
		jsonData["endTime"] = endTime;
		jsonData["haveAlarm"] = haveAlarm;
		jsonData["alarmTime"] = alarmTime;
		jsonData["calendarItemExternalIdentifier"] = calendarItemExternalIdentifier;
		jsonData["cfgId"] = cfgId;
		jsonData["sourcePath"] = sourcePath;
		calendarEventJsonParam = jsonData.ToJson();
	}

	public void AddCalendarEvent(string title, string location, long startTime, long endTime, int haveAlarm, int alarmTime, string calendarItemExternalIdentifier, int cfgId, string sourcePath)
	{
		if (StringUtils.VersionCompare(Application.version, "1.0.310") < 0)
		{
			return;
		}
		SaveCalendarEventJsonParam(title, location, startTime, endTime, haveAlarm, alarmTime, calendarItemExternalIdentifier, cfgId, sourcePath);
		if (GameEntry.Sdk.GetDataFromNative("HasCalendarEventPermission", "") == "true")
		{
			try
			{
				TrySendAddCalendarEvent();
				return;
			}
			catch (Exception message)
			{
				Log.Warning(message);
				return;
			}
		}
		SendDataToNative("RequestCalendarEventPermission", 1002.ToString());
	}

	private void TrySendAddCalendarEvent()
	{
		if (!string.IsNullOrEmpty(calendarEventJsonParam))
		{
			SendDataToNative("AddCalendarEvent", calendarEventJsonParam);
		}
	}

	public static void SaveImageToPhotoIOS(string filePath)
	{
		if (StringUtils.VersionCompare(Application.version, "1.0.282") < 0)
		{
			UIUtils.ShowTips("season_alliance_photo_tips_31", 3f);
		}
	}

	public static void SaveImageToPhotoAndroid(string filePath, string fileName)
	{
		int length = Application.persistentDataPath.IndexOf("Android");
		string text = string.Concat(Application.persistentDataPath.Substring(0, length) + "DCIM/", fileName);
		byte[] bytes = File.ReadAllBytes(filePath);
		File.WriteAllBytes(text, bytes);
		Log.Info("SaveImageToPhotoAndroid: {0}", text);
		GameEntry.Sdk.Platform.SendDataToNative("RefreshPhoto", text);
		UIUtils.ShowTips("season_alliance_photo_tips_8", 3f);
	}

	public static void SaveRTToAlbum(RenderTexture rt)
	{
		if (StringUtils.VersionCompare(Application.version, "1.0.282") < 0)
		{
			UIUtils.ShowTips("season_alliance_photo_tips_31", 3f);
			return;
		}
		Texture2D texture2D = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, mipChain: false);
		RenderTexture.active = rt;
		texture2D.ReadPixels(new Rect(0f, 0f, rt.width, rt.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = null;
		byte[] bytes = texture2D.EncodeToJPG();
		string text = string.Format("lastwarPic{0}.jpg", DateTime.Now.ToString("yyyyMMddHHmmss"));
		string text2 = Path.Combine(Application.persistentDataPath, text);
		File.WriteAllBytes(text2, bytes);
		Log.Info("lastwarPic saved to: " + text2);
		UnityEngine.Object.Destroy(texture2D);
		SaveToAlbum(text2, text);
	}

	public static void OpenURL(string url)
	{
		url = url.Trim();
		if (StringUtils.VersionCompare(Application.version, "1.0.286") >= 0)
		{
			Application.OpenURL(url);
		}
		else
		{
			Application.OpenURL(url);
		}
	}

	public static void InitUWAGotOnline()
	{
	}

	private void InitGameCenter()
	{
		if (!GameCenterBridge.IsGameCenterAvailable())
		{
			return;
		}
		if (GameCenterBridge.IsIOSVersionOrGreater(26))
		{
			GameCenterBridge.AutoLogin();
			return;
		}
		GameCenterBridge.SetAutoLoginCallback(HandleAutoLoginResult);
		if (PlayerPrefs.GetInt("GameCenterDeclinedByUser", 0) != 1)
		{
			GameCenterBridge.AutoLogin();
		}
	}

	private void HandleAutoLoginResult(bool success, GameCenterAuthData data)
	{
		GameCenterBridge.DebugLogError($"[GameManager] AutoLogin Result: Success = {success}");
		if (success)
		{
			PlayerPrefs.SetInt("GameCenterDeclinedByUser", 0);
		}
		else if (data.displayName != null && data.displayName.Contains("player has not been authenticated"))
		{
			PlayerPrefs.SetInt("GameCenterDeclinedByUser", 1);
		}
	}
}
