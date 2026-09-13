using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.Utils;
using UnityEngine.Networking;

namespace ThinkingSDK.PC.Request;

public class ThinkingSDKNormalRequest : ThinkingSDKBaseRequest
{
	public ThinkingSDKNormalRequest(string appId, string url, IList<Dictionary<string, object>> data)
		: base(appId, url, data)
	{
	}

	public ThinkingSDKNormalRequest(string appId, string url)
		: base(appId, url)
	{
	}

	public override IEnumerator SendData_2(ResponseHandle responseHandle, IList<Dictionary<string, object>> data)
	{
		SetData(data);
		string text = URL();
		string text2 = ThinkingSDKJSON.Serialize(new Dictionary<string, object>
		{
			[ThinkingSDKConstant.APPID] = APPID(),
			["data"] = Data(),
			["#flush_time"] = ThinkingSDKUtil.GetTimeStamp()
		});
		string s = Encode(text2);
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		using UnityWebRequest webRequest = new UnityWebRequest(text, "POST");
		webRequest.timeout = 30;
		webRequest.SetRequestHeader("Content-Type", "text/plain");
		webRequest.SetRequestHeader("appid", APPID());
		webRequest.SetRequestHeader("TA-Integration-Type", "PC");
		webRequest.SetRequestHeader("TA-Integration-Version", "2.3.0");
		webRequest.SetRequestHeader("TA-Integration-Count", "1");
		webRequest.SetRequestHeader("TA-Integration-Extra", "PC");
		webRequest.uploadHandler = new UploadHandlerRaw(bytes);
		webRequest.downloadHandler = new DownloadHandlerBuffer();
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

	private static string Encode(string inputStr)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(inputStr);
		using MemoryStream memoryStream = new MemoryStream();
		using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
		{
			gZipStream.Write(bytes, 0, bytes.Length);
		}
		return Convert.ToBase64String(memoryStream.ToArray());
	}
}
