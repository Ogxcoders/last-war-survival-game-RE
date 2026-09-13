using System.Collections;
using System.Collections.Generic;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace ThinkingSDK.PC.Request;

public class ThinkingSDKDebugRequest : ThinkingSDKBaseRequest
{
	private int mDryRun;

	private string mDeviceID = ThinkingSDKDeviceInfo.DeviceID();

	public void SetDryRun(int dryRun)
	{
		mDryRun = dryRun;
	}

	public ThinkingSDKDebugRequest(string appId, string url, IList<Dictionary<string, object>> data)
		: base(appId, url, data)
	{
	}

	public ThinkingSDKDebugRequest(string appId, string url)
		: base(appId, url)
	{
	}

	public override IEnumerator SendData_2(ResponseHandle responseHandle, IList<Dictionary<string, object>> data)
	{
		SetData(data);
		string text = URL();
		string text2 = ThinkingSDKJSON.Serialize(Data()[0]);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("appid", APPID());
		wWWForm.AddField("source", "client");
		wWWForm.AddField("dryRun", mDryRun);
		wWWForm.AddField("deviceId", mDeviceID);
		wWWForm.AddField("data", text2);
		using UnityWebRequest webRequest = UnityWebRequest.Post(text, wWWForm);
		webRequest.timeout = 30;
		webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		ThinkingSDKLogger.Print("Post event: " + text2 + "\n  Request URL: " + text);
		yield return webRequest.SendWebRequest();
		Dictionary<string, object> dictionary = null;
		if (webRequest.isHttpError || webRequest.isNetworkError)
		{
			ThinkingSDKLogger.Print("Error response : " + webRequest.error);
		}
		else
		{
			ThinkingSDKLogger.Print("Response : " + webRequest.downloadHandler.text);
			if (!string.IsNullOrEmpty(webRequest.downloadHandler.text))
			{
				dictionary = ThinkingSDKJSON.Deserialize(webRequest.downloadHandler.text);
			}
		}
		if (responseHandle != null)
		{
			dictionary?.Add("flush_count", data.Count);
			responseHandle(dictionary);
		}
	}
}
