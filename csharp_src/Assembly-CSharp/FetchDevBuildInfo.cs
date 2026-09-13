using System;
using System.Collections.Generic;
using GameFramework;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using VEngine;

public class FetchDevBuildInfo
{
	private class _BuildList
	{
		public List<string> manifest_list;

		public List<string> lua_list;
	}

	public UnityWebRequestAsyncOperation tableRequest;

	public UnityWebRequestAsyncOperation buildRequest;

	public bool succeed;

	public Action<FetchDevBuildInfo, List<string>, List<string>, List<string>> completed;

	public void SendRequest()
	{
		string text = ConstURLConfig.LocalCheckVersionHostList[0] + "/gameservice/gettable.php";
		Log.Info("FetchDevBuildInfo request table version " + text);
		UnityWebRequest unityWebRequest = UnityWebRequest.Get(text);
		unityWebRequest.timeout = 3;
		tableRequest = unityWebRequest.SendWebRequest();
		tableRequest.completed += TableRequestOnCompleted;
	}

	private void TableRequestOnCompleted(AsyncOperation obj)
	{
		if (tableRequest.isDone && string.IsNullOrEmpty(tableRequest.webRequest.error))
		{
			string packageName = NetworkURLConfig.PackageName;
			string platformName = Utility.GetPlatformName();
			string text = ConstURLConfig.LocalCheckVersionHostList[0] + "/gameservice/getallversion.php?packageName=" + packageName + "&platform=" + platformName + "&returnJson=1";
			Log.Info("FetchDevBuildInfo request build version " + text);
			UnityWebRequest unityWebRequest = UnityWebRequest.Get(text);
			unityWebRequest.timeout = 3;
			buildRequest = unityWebRequest.SendWebRequest();
			buildRequest.completed += BuildRequestOnCompleted;
		}
		else
		{
			Log.Error("FetchDevBuildInfo tableRequest error: " + tableRequest.webRequest.error);
			FailedCallback();
		}
	}

	private static int CompareNumbers(string x, string y)
	{
		int value = int.Parse(x);
		return int.Parse(y).CompareTo(value);
	}

	private void BuildRequestOnCompleted(AsyncOperation obj)
	{
		if (buildRequest.isDone && string.IsNullOrEmpty(buildRequest.webRequest.error))
		{
			try
			{
				List<string> arg = JsonConvert.DeserializeObject<List<string>>(tableRequest.webRequest.downloadHandler.text);
				_BuildList? buildList = JsonConvert.DeserializeObject<_BuildList>(buildRequest.webRequest.downloadHandler.text);
				List<string> manifest_list = buildList.manifest_list;
				List<string> lua_list = buildList.lua_list;
				if (!manifest_list.Contains(GameEntry.Sdk.VersionCode))
				{
					manifest_list.Add(GameEntry.Sdk.VersionCode);
				}
				if (!lua_list.Contains(GameEntry.Sdk.VersionCode))
				{
					lua_list.Add(GameEntry.Sdk.VersionCode);
				}
				manifest_list.Sort((string x, string y) => CompareNumbers(x, y));
				lua_list.Sort((string x, string y) => CompareNumbers(x, y));
				succeed = true;
				completed?.Invoke(this, arg, manifest_list, lua_list);
				return;
			}
			catch (Exception message)
			{
				Log.Error(message);
			}
		}
		else
		{
			Log.Error("FetchDevBuildInfo buildRequest error: " + buildRequest.webRequest.error);
		}
		FailedCallback();
	}

	private void FailedCallback()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		list.Add(ClientConfig.TableEnvName);
		if (PlayerPrefs.HasKey(UIChooseLocalUpdate.tableEnvLocalUpdateKey))
		{
			list.Add(PlayerPrefs.GetString(UIChooseLocalUpdate.tableEnvLocalUpdateKey));
		}
		list2.Add(GameEntry.Sdk.VersionCode);
		if (PlayerPrefs.HasKey(UIChooseLocalUpdate.bundleBuildIdLocalUpdateKey))
		{
			list2.Add(PlayerPrefs.GetString(UIChooseLocalUpdate.bundleBuildIdLocalUpdateKey));
		}
		list3.Add(GameEntry.Sdk.VersionCode);
		if (PlayerPrefs.HasKey(UIChooseLocalUpdate.luaBuildIdLocalUpdateKey))
		{
			list3.Add(PlayerPrefs.GetString(UIChooseLocalUpdate.luaBuildIdLocalUpdateKey));
		}
		succeed = false;
		completed?.Invoke(this, list, list2, list3);
	}
}
