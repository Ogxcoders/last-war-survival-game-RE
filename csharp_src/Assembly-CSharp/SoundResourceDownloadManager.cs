using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using VEngine;

public class SoundResourceDownloadManager
{
	private static SoundResourceDownloadManager _instance;

	private Dictionary<int, DownloadResGroupCommonData> _downloadGroupDict = new Dictionary<int, DownloadResGroupCommonData>();

	private Dictionary<int, float> _downloadTimeDict = new Dictionary<int, float>();

	private Dictionary<int, bool> _needPostDict = new Dictionary<int, bool>();

	private List<List<int>> _lang2CfgList;

	private string _currentLang = string.Empty;

	private const int _minId = 50000;

	private const int _maxId = 52000;

	public static SoundResourceDownloadManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SoundResourceDownloadManager();
			}
			return _instance;
		}
	}

	public void Init()
	{
		GameEntry.Event.Subscribe(EventId.CommonResourceGroupDownloadFinish, OnDownloadFinish);
	}

	public void UnInit()
	{
		GameEntry.Event.Unsubscribe(EventId.CommonResourceGroupDownloadFinish, OnDownloadFinish);
		_currentLang = string.Empty;
		_downloadGroupDict.Clear();
		_downloadTimeDict.Clear();
		_needPostDict.Clear();
		_lang2CfgList = null;
	}

	public void CheckLangResourceAutoDownload(int langIndex, string langName)
	{
		if (!ClientSwitch.IsOn(33))
		{
			return;
		}
		_currentLang = langName;
		if (_lang2CfgList == null)
		{
			_lang2CfgList = new List<List<int>>();
			string text = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "voice_selection", "k2");
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[1] { '|' });
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[1] { ';' });
					List<int> list = new List<int>();
					string[] array3 = array2;
					for (int j = 0; j < array3.Length; j++)
					{
						if (int.TryParse(array3[j], out var result))
						{
							list.Add(result);
						}
					}
					_lang2CfgList.Add(list);
				}
			}
		}
		if (langIndex < 0 || langIndex >= _lang2CfgList.Count)
		{
			return;
		}
		List<int> list2 = _lang2CfgList[langIndex];
		if (list2.Count == 0)
		{
			Log.Error("[LWSoundResourceDownloadManager] not langIndex " + langIndex);
			return;
		}
		foreach (int key in _downloadGroupDict.Keys)
		{
			DownloadResGroupCommonManager.Instance.StopDownload(key);
		}
		foreach (int item in list2)
		{
			if (item > 0 && !DownloadResGroupCommonManager.Instance.IsDownload(item))
			{
				_downloadGroupDict[item] = DownloadResGroupCommonManager.Instance.StartDownload(item);
				_downloadTimeDict[item] = Time.realtimeSinceStartup;
				_needPostDict[item] = true;
				Log.Info("[LWSoundResourceDownloadManager] DownloadStart " + item);
			}
			else
			{
				Log.Info("[LWSoundResourceDownloadManager] IsDownloaded " + item);
			}
		}
	}

	public void OnDownloadFinish(object key)
	{
		if (ClientSwitch.IsOn(33) && int.TryParse(key?.ToString(), out var result) && _downloadGroupDict.TryGetValue(result, out var _))
		{
			_downloadGroupDict[result] = null;
			if (_downloadTimeDict.TryGetValue(result, out var value2))
			{
				float num = (Time.realtimeSinceStartup - value2) / 60f;
				Log.Info("[LWSoundResourceDownloadManager] DownloadFinish Id:" + result + "Time:" + num.ToString("F2"));
			}
		}
	}

	private int GetRemoteSoundId(string soundAssetName, string soundGroupName)
	{
		switch (soundGroupName)
		{
		case "Dub":
		case "Effect":
		case "Hero":
		case "Music":
		{
			_ = Time.realtimeSinceStartup;
			if (!Versions.GetDependencies(soundAssetName, out var bundle, out var _) || bundle.downloadModes == Utility.IntArrayEmpty)
			{
				break;
			}
			for (int i = 0; i < bundle.downloadModes.Length; i++)
			{
				if (bundle.downloadModes[i] >= 50000 && bundle.downloadModes[i] <= 52000)
				{
					return bundle.downloadModes[i];
				}
			}
			break;
		}
		}
		return -1;
	}

	public bool IsCanAsync(string soundAssetName, string soundGroupName)
	{
		if (!ClientSwitch.IsOn(33))
		{
			return true;
		}
		int remoteSoundId = GetRemoteSoundId(soundAssetName, soundGroupName);
		if (remoteSoundId != -1)
		{
			bool flag = GameEntry.Resource.PrefabAssetsDownloaded(soundAssetName);
			if (_needPostDict.ContainsKey(remoteSoundId))
			{
				_needPostDict.Remove(remoteSoundId);
				PostEventLog.TrackMap("c_sound_dub_remote_success", new Dictionary<string, object>
				{
					{ "s_user_lang", _currentLang },
					{
						"event_type",
						remoteSoundId.ToString()
					},
					{ "eventname", soundAssetName },
					{
						"count",
						flag ? 1 : 0
					}
				});
			}
			if (soundGroupName == "Music")
			{
				flag = true;
			}
			return flag;
		}
		return true;
	}
}
