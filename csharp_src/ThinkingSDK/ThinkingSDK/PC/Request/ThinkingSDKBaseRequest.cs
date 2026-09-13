using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.Utils;
using UnityEngine.Networking;

namespace ThinkingSDK.PC.Request;

public abstract class ThinkingSDKBaseRequest
{
	private string mAppid;

	private string mURL;

	private IList<Dictionary<string, object>> mData;

	public ThinkingSDKBaseRequest(string appId, string url, IList<Dictionary<string, object>> data)
	{
		mAppid = appId;
		mURL = url;
		mData = data;
	}

	public ThinkingSDKBaseRequest(string appId, string url)
	{
		mAppid = appId;
		mURL = url;
	}

	public void SetData(IList<Dictionary<string, object>> data)
	{
		mData = data;
	}

	public string APPID()
	{
		return mAppid;
	}

	public string URL()
	{
		return mURL;
	}

	public IList<Dictionary<string, object>> Data()
	{
		return mData;
	}

	public static void GetConfig(string url, ResponseHandle responseHandle)
	{
		if (!ThinkingSDKUtil.IsValiadURL(url))
		{
			ThinkingSDKLogger.Print("invalid url");
		}
		HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(url);
		obj.Method = "GET";
		string text = new StreamReader(((HttpWebResponse)obj.GetResponse()).GetResponseStream()).ReadToEnd();
		if (text != null)
		{
			ThinkingSDKLogger.Print("Request URL=" + url);
			ThinkingSDKLogger.Print("Response:=" + text);
		}
	}

	public bool MyRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		bool result = true;
		if (sslPolicyErrors != SslPolicyErrors.None)
		{
			for (int i = 0; i < chain.ChainStatus.Length; i++)
			{
				if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
				{
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					if (!chain.Build((X509Certificate2)certificate))
					{
						result = false;
						break;
					}
				}
			}
		}
		return result;
	}

	public abstract IEnumerator SendData_2(ResponseHandle responseHandle, IList<Dictionary<string, object>> data);

	public static IEnumerator GetWithFORM_2(string url, string appId, Dictionary<string, object> param, ResponseHandle responseHandle)
	{
		string text = url + "?appid=" + appId;
		if (param != null)
		{
			text = text + "&data=" + ThinkingSDKJSON.Serialize(param);
		}
		using UnityWebRequest webRequest = UnityWebRequest.Get(text);
		webRequest.timeout = 30;
		webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		ThinkingSDKLogger.Print("Request URL=" + text);
		yield return webRequest.SendWebRequest();
		Dictionary<string, object> result = null;
		if (webRequest.isHttpError || webRequest.isNetworkError)
		{
			ThinkingSDKLogger.Print("Error response : " + webRequest.error);
		}
		else
		{
			ThinkingSDKLogger.Print("Response : " + webRequest.downloadHandler.text);
			if (!string.IsNullOrEmpty(webRequest.downloadHandler.text))
			{
				result = ThinkingSDKJSON.Deserialize(webRequest.downloadHandler.text);
			}
		}
		responseHandle?.Invoke(result);
	}
}
