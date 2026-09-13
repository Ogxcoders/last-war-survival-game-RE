using System;
using GameFramework;
using GameKit.Base;
using SFSLitJson;
using UnityEngine.Networking;
using VEngine;

public class PushNoticeManager
{
	public static void PushNotice(string noticeJson)
	{
		if (!noticeJson.IsNullOrEmpty())
		{
			GameEntry.Sdk.SendDataToNative("PUSH_PushNotice", noticeJson);
		}
	}

	public static void CancelNotice(string typeJson)
	{
		if (!typeJson.IsNullOrEmpty())
		{
			GameEntry.Sdk.SendDataToNative("PUSH_CancleNotice", typeJson);
		}
	}

	public static void ClearAllNotice()
	{
		GameEntry.Sdk.SendDataToNative("PUSH_ClearAllNotice", string.Empty);
	}

	public static int GetPushCountById(string pushId)
	{
		int num = 0;
		try
		{
			JsonData jsonData = new JsonData();
			jsonData["pushId"] = pushId;
			return GameEntry.Sdk.GetDataFromNative("PUSH_getPushCountById", jsonData.ToJson()).ToInt();
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
	}

	public static int GetPushSecondTimeById(string pushId)
	{
		int num = 0;
		try
		{
			JsonData jsonData = new JsonData();
			jsonData["pushId"] = pushId;
			Log.Info("PUSH_PushNotice");
			return GameEntry.Sdk.GetDataFromNative("PUSH_getPushTimeById", jsonData.ToJson()).ToInt();
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
	}

	public static bool GetIsNotifyOpen()
	{
		string text = "";
		try
		{
			text = GameEntry.Sdk.GetDataFromNative("PUSH_isNotifyOpen", "");
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
		return text.Equals("true");
	}

	public static long GetCurrentTimeUnix()
	{
		return (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds;
	}

	public static void pushDataToHttpServer()
	{
		string clickPushTag = GameEntry.Sdk.getClickPushTag();
		string clickPushId = GameEntry.Sdk.getClickPushId();
		if (clickPushTag.IsNullOrEmpty() || clickPushId.IsNullOrEmpty())
		{
			return;
		}
		string text = "aps";
		string publicString = GameEntry.Setting.GetPublicString("Setting.GAME_UID", "");
		string text2 = GameEntry.Setting.GetPublicInt("PUSHMARK").ToString();
		long currentTimeUnix = GetCurrentTimeUnix();
		string arg = new JsonData
		{
			["project"] = text,
			["uid"] = publicString,
			["pushtag"] = clickPushTag,
			["playermark"] = text2,
			["pushId"] = clickPushId,
			["ispushget"] = true,
			["ispushopen"] = true,
			["type"] = "click",
			["versionCode"] = GameEntry.Sdk.VersionCode,
			["time"] = currentTimeUnix * 1000
		}.ToJson();
		string arg2 = "http://analyse-lw.readygo.tech/postback.php?param=";
		string uri = $"{arg2}{arg}";
		SingletonBehaviour<WebRequestManager>.Instance.Get(uri, delegate(UnityWebRequest request, bool hasErr, object userdata)
		{
			_ = (DownloadInfo)userdata;
			if (!hasErr)
			{
				_ = request.isDone;
			}
		});
		GameEntry.Sdk.ClearAllPushData();
	}
}
