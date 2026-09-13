using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace VEngine;

[RequireComponent(typeof(Updater))]
[DisallowMultipleComponent]
public class Startup : MonoBehaviour
{
	[Tooltip("资源下载地址，指向平台目录的父目录")]
	public string downloadURL = "http://127.0.0.1/Bundles/";

	[Tooltip("是否启动后更新服务器版本信息")]
	public bool autoUpdate;

	[Tooltip("是否开启日志")]
	public bool loggable;

	[Tooltip("通过关键字进行路径匹配，为路径生成短链接，可以按需使用")]
	public string[] keys = new string[2] { "Scenes", "Prefabs" };

	[Tooltip("加载模式")]
	public LoadMode loadMode = LoadMode.LoadByNameWithoutExtension;

	public UnityEvent onFinished;

	private IEnumerator Start()
	{
		switch (loadMode)
		{
		case LoadMode.LoadByName:
			Versions.customLoadPath = LoadByName;
			break;
		case LoadMode.LoadByNameWithoutExtension:
			Versions.customLoadPath = LoadByNameWithoutExtension;
			break;
		case LoadMode.LoadByCustom:
			Versions.customLoadPath = null;
			break;
		default:
			Versions.customLoadPath = null;
			break;
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		InitializeVersions operation = Versions.InitializeAsync();
		yield return operation;
		if (operation.status == OperationStatus.Failed)
		{
			Logger.E("Failed to initialize Runtime with error: {0}", operation.error);
		}
		else
		{
			Logger.I("Success to initialize Runtime with:");
		}
		Versions.DownloadURL = downloadURL;
		Logger.I("API Version: {0}", "6.1.5");
		Logger.I("Manifests Version: {0}", Versions.ManifestsVersion);
		Logger.I("PlayerDataPath: {0}", Versions.PlayerDataPath);
		Logger.I("DownloadDataPath: {0}", Versions.DownloadDataPath);
		Logger.I("DownloadURL: {0}", Versions.DownloadURL);
		if (autoUpdate && !Versions.SkipUpdate)
		{
			string[] array = new string[operation.manifests.Length];
			char[] separator = new char[1] { '_' };
			for (int i = 0; i < operation.manifests.Length; i++)
			{
				string text = operation.manifests[i];
				string[] array2 = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				if (array2.Length > 1)
				{
					array[i] = array2[0];
				}
				else
				{
					array[i] = text;
				}
			}
			UpdateVersions update = Versions.UpdateAsync(array);
			yield return update;
			update.Override();
			if (update.status == OperationStatus.Failed)
			{
				Logger.E("Failed to initialize Runtime with error: {0}", update.error);
			}
			else
			{
				Logger.I("Success to update versions with version: {0}", Versions.ManifestsVersion);
			}
			update.Dispose();
		}
		if (onFinished != null)
		{
			onFinished.Invoke();
		}
	}

	private void Update()
	{
		Logger.Loggable = loggable;
	}

	private string LoadByNameWithoutExtension(string assetPath)
	{
		if (keys == null || keys.Length == 0)
		{
			return null;
		}
		if (!Array.Exists(keys, assetPath.Contains))
		{
			return null;
		}
		return Path.GetFileNameWithoutExtension(assetPath);
	}

	private string LoadByName(string assetPath)
	{
		if (keys == null || keys.Length == 0)
		{
			return null;
		}
		if (!Array.Exists(keys, assetPath.Contains))
		{
			return null;
		}
		return Path.GetFileName(assetPath);
	}
}
