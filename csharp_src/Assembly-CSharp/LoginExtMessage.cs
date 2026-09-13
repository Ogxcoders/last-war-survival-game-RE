using System;
using GameFramework;
using SFSLitJson;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using ThinkingSDK.PC.Utils;
using UnityEngine;
using XLua;

public class LoginExtMessage : BaseMessage
{
	private static LoginExtMessage _instance;

	private string funcMessage;

	private string a;

	private string b;

	public static LoginExtMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<LoginExtMessage>());

	public override string GetMsgId()
	{
		return "login.ext";
	}

	public void ClearData()
	{
		funcMessage = (a = (b = null));
	}

	public void SetData(string func, string a, string b)
	{
		funcMessage = func;
		this.a = a;
		this.b = b;
	}

	protected override IRequest CSSetData(params object[] args)
	{
		ISFSObject iSFSObject = new SFSObject();
		string val = CollectBuildInfo();
		iSFSObject.PutUtfString("info", val);
		try
		{
			if (!string.IsNullOrEmpty(funcMessage))
			{
				string chunk = AESHelper.AESDecrypt(funcMessage, "7a7611b0efc334a7cc229fe5d89c5997");
				GameEntry.Lua.Env.DoString(chunk);
				object[] array = GameEntry.Lua.Env.Global.Get<LuaFunction>("addInfo").Call(a, b);
				if (array != null && array.Length != 0)
				{
					string val2 = array[0] as string;
					iSFSObject.PutUtfString("add", val2);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("login ext Exception:" + ex.Message);
		}
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	private string CollectBuildInfo()
	{
		try
		{
			JsonData jsonData = new JsonData();
			jsonData["deviceName"] = SystemInfo.deviceName;
			jsonData["deviceModel"] = SystemInfo.deviceModel;
			jsonData["os_version"] = SystemInfo.operatingSystem;
			jsonData["networkType"] = ThinkingSDKDeviceInfo.NetworkType();
			jsonData["graphicsDeviceName"] = SystemInfo.graphicsDeviceName;
			jsonData["graphicsDeviceVersion"] = SystemInfo.graphicsDeviceVersion;
			jsonData["processorType"] = SystemInfo.processorType;
			string buildInfo = GameEntry.Sdk.GetBuildInfo();
			if (!string.IsNullOrEmpty(buildInfo))
			{
				jsonData["native_raw_info"] = buildInfo;
			}
			return jsonData.ToJson();
		}
		catch (Exception ex)
		{
			return ex.ToString();
		}
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}
}
