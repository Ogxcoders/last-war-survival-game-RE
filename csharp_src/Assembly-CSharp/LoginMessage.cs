using System;
using System.Text;
using BaseUtils;
using GameFramework;
using ProtoBufNet;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;

public class LoginMessage : BaseMessage
{
	public enum DelAccountStatus
	{
		Default,
		ConfirmDel,
		CancelApply
	}

	private static LoginMessage _instance;

	public Action<ISFSObject> onLoginResponse;

	public static LoginMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<LoginMessage>());

	public override string GetMsgId()
	{
		return "login";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		DeviceManager device = GameEntry.Device;
		GlobalDataManager globalData = GameEntry.GlobalData;
		SettingManager setting = GameEntry.Setting;
		LocalizationManager localization = GameEntry.Localization;
		string text = (string)args[0];
		string password = (string)args[1];
		string text2 = (string)args[2];
		int val = 0;
		if (args.Length >= 4 && args[3] is int)
		{
			val = (int)args[3];
		}
		SFSObject sFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		sFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		sFSObject.PutInt("netType", device.GetNetworkStatus());
		sFSObject.PutUtfString("ta", SDKManager.GetTaPresetProp());
		sFSObject.PutUtfString("distinct_id", SDKManager.GetTaDistinctId());
		sFSObject.PutUtfString("phone_screen", $"{Screen.width}*{Screen.height}");
		SFSObject val2 = new SFSObject();
		sFSObject.PutSFSObject("configVersion", val2);
		string text3 = GameEntry.Timer.GetLocalSeconds().ToString();
		if (string.IsNullOrEmpty(text))
		{
			globalData.loginServerInfo = new GlobalDataManager.LoginServerInfo
			{
				country = 1
			};
			sFSObject.PutInt("country", globalData.loginServerInfo.country);
			sFSObject.PutBool("suggestCountry", val: false);
			sFSObject.PutUtfString("timeoffset", (DateTime.Now - DateTime.UtcNow).TotalSeconds.ToString());
			sFSObject.PutUtfString("gcmRegisterId", globalData.gcmRegisterId);
			sFSObject.PutUtfString("referrer", globalData.referrer);
		}
		sFSObject.PutUtfString("AndroidID", device.GetSerialID());
		sFSObject.PutUtfString("IMEI", device.GetDeviceInfo());
		string dataFromNative = GameEntry.Sdk.GetDataFromNative("LW_PSH", text3);
		sFSObject.PutUtfString("psh", dataFromNative);
		sFSObject.PutUtfString("mt", device.GetHandSetInfo());
		device.GetDeviceUid();
		sFSObject.PutUtfString("deviceId", device.GetDeviceUid());
		sFSObject.PutUtfString("airKey", device.GetDeviceUid_Transcoding());
		string msg = text3 + "4d1c383ccbedf3d98320d6ea06d8dedc" + text;
		sFSObject.PutUtfString("cmdBaseTime", text3);
		sFSObject.PutUtfString("SecurityCode", BaseUtils.StringUtils.GetMD5(msg));
		string text4 = BaseUtils.StringUtils.GenerateRandomStr(32);
		string mD = BaseUtils.StringUtils.GetMD5(text4);
		char[] array = text4.ToCharArray();
		char[] array2 = mD.ToCharArray();
		char[] array3 = new char[64];
		for (int i = 0; i < 32; i++)
		{
			array3[i * 2] = array2[i];
			array3[i * 2 + 1] = array[i];
		}
		sFSObject.PutUtfString("OneCode", new string(array3));
		char[] array4 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text4)).ToCharArray();
		Array.Reverse((Array)array4);
		string mD2 = BaseUtils.StringUtils.GetMD5(new string(array4));
		string mD3 = BaseUtils.StringUtils.GetMD5(mD2 + text4);
		for (int j = 0; j < 32; j++)
		{
			array3[j * 2] = mD3[j];
			array3[j * 2 + 1] = mD2[j];
		}
		sFSObject.PutUtfString("CoreV", new string(array3));
		sFSObject.PutUtfString("googlePlay", GameEntry.Sdk.pf_displayname);
		sFSObject.PutUtfString("androidDid", device.GetNewAndroidDeviceID());
		sFSObject.PutUtfString("googleName", setting.GetString("googleName", ""));
		sFSObject.PutUtfString("deeplinkParams", globalData.deeplinkParams);
		sFSObject.PutUtfString("pfId", globalData.platformUID);
		string text5 = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetConfigMd5");
		if (!text5.IsNullOrEmpty())
		{
			sFSObject.PutUtfString("dataConfigMd5", text5);
		}
		sFSObject.PutInt("google_available", GameEntry.Sdk.IsGoogleAvailable ? 1 : 0);
		sFSObject.PutUtfString("fromCountry", globalData.fromCountry);
		sFSObject.PutUtfString("packageName", GameEntry.Sdk.GetPackageName());
		sFSObject.PutUtfString("packageSign", GameEntry.Sdk.GetPackageSign());
		string packageSign = GameEntry.Sdk.GetPackageSign();
		Debug.Log(">>> packageSign: " + packageSign);
		sFSObject.PutUtfString("platform", "1");
		sFSObject.PutInt("lat", 0);
		sFSObject.PutUtfString("device_string", device.GetDeviceString());
		sFSObject.PutUtfString("pf", GameEntry.Sdk.getChannel());
		string fireBaseId = PushManager.Instance.GetFireBaseId();
		sFSObject.PutUtfString("firebaseId", fireBaseId);
		string appsFlyerUid = GameEntry.Sdk.GetAppsFlyerUid();
		Log.Info("LoginMessage " + text);
		sFSObject.PutUtfString("afuid", appsFlyerUid);
		sFSObject.PutUtfString("phone_model", device.GetDeviceModel());
		sFSObject.PutInt("configNumber", 0);
		sFSObject.PutUtfString("gaid", globalData.gaid);
		sFSObject.PutUtfString("osVersion", device.GetOSVersion());
		sFSObject.PutUtfString("parseRegisterId", globalData.parseRegisterId);
		sFSObject.PutUtfString("gameUid", text);
		sFSObject.PutUtfString("appVersion", GameEntry.Sdk.Version);
		sFSObject.PutUtfString("resVersion", GameEntry.Resource.GetResVersion());
		sFSObject.PutUtfString("versionCode", GameEntry.Sdk.VersionCode);
		sFSObject.PutUtfString("lang", localization.GetLanguageName());
		sFSObject.PutUtfString("serverId", text2.Substring(3));
		sFSObject.PutInt("gmLogin", 0);
		sFSObject.PutUtfString("KCPMode", "0");
		sFSObject.PutInt("forbidden_froce_merge", 1);
		sFSObject.PutUtfString("shumeiBoxId", ShumeiSdkManager.Instance.GetLoginSmsdkId());
		sFSObject.PutUtfString("simOp", GameEntry.Sdk.GetSimOperator());
		sFSObject.PutUtfString("simOpName", GameEntry.Sdk.GetSimOperatorName());
		sFSObject.PutInt("delete_account_status", val);
		if (CommonUtils.IsDebug())
		{
			Log.Info("psh:" + dataFromNative);
		}
		else
		{
			string text6 = (string.IsNullOrEmpty(dataFromNative) ? "1.0.300" : "1.0.301");
			Log.Info("av_p :" + text6);
		}
		if (NetPacketConst.useNewPacket && NetPacketConst.useZStd)
		{
			sFSObject.PutBool("isUseLz4", val: true);
		}
		string text7 = GameEntry.Setting.GetString("Login.access_token", "");
		if (!string.IsNullOrEmpty(text7))
		{
			sFSObject.PutUtfString("at", text7);
		}
		return new LoginRequest(text, password, text2, sFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		Log.Info("[Login] handle LoginMessage");
		GlobalDataManager globalData = GameEntry.GlobalData;
		_ = GameEntry.Setting;
		PostEventLog.Record("LOGIN_FINISH");
		if (message.ContainsKey("errorMessage"))
		{
			Log.Error(message.GetUtfString("errorMessage"));
			onLoginResponse?.Invoke(message);
			return;
		}
		if (message.ContainsKey("serverInfo"))
		{
			ISFSObject sFSObject = message.GetSFSObject("serverInfo");
			string utfString = sFSObject.GetUtfString("ip");
			string utfString2 = sFSObject.GetUtfString("zone");
			int port = sFSObject.TryGetInt("port");
			string utfString3 = sFSObject.GetUtfString("uid");
			Log.Info("[Login] override serverinfo");
			Log.Info("[AT]SetGUID_LoginMsg:" + utfString3);
			ApplicationLaunch.Instance.Loading.SaveGameServerSetting(utfString, port, utfString2, utfString3, "", 0);
			GameEntry.Network.GameServerUrlList = null;
		}
		if (message.ContainsKey("db_timezone_offset"))
		{
			long num = (long)message.GetInt("db_timezone_offset") * 1000L;
			GameEntry.Timer.UpdateServerMilliseconds(num);
			GameEntry.Lua.UpdateUITimeStamp(num);
		}
		globalData.downloadurlcdn = message.GetUtfString("downloadurlcdn");
		if (message.ContainsKey("xmlVersion"))
		{
			globalData.xmlVersion = message.GetUtfString("xmlVersion");
		}
		else
		{
			globalData.xmlVersion = "";
		}
		if (message.ContainsKey("serverVersion"))
		{
			globalData.serverVersion = message.GetUtfString("serverVersion");
		}
		else
		{
			globalData.serverVersion = "";
		}
		if (message.ContainsKey("updateType"))
		{
			int num2 = (globalData.updateType = message.GetInt("updateType"));
			globalData.downloadurl = message.GetUtfString("downloadurl");
			if (string.IsNullOrEmpty(globalData.downloadurlcdn))
			{
				globalData.downloadurlcdn = globalData.downloadurl;
			}
			Log.Info("gameServer UpdateType = {0}, downloadurl = {1}", num2, globalData.downloadurl);
		}
		if (globalData.updateType == 3)
		{
			globalData.updateType = 0;
		}
		if (message.ContainsKey("new_version"))
		{
			string utfString4 = message.GetUtfString("new_version");
			ShumeiSdkManager.Instance.SetMostNewVersion(utfString4);
		}
		onLoginResponse?.Invoke(message);
	}

	public void SendDelAccountMsg()
	{
		Instance.Send(GameEntry.Network.Uid, "", GameEntry.Network.ZoneName, 1);
	}

	public void SendDelAccountApplyCancelMsg()
	{
		Instance.Send(GameEntry.Network.Uid, "", GameEntry.Network.ZoneName, 2);
	}
}
