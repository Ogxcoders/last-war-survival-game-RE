using System;
using System.Collections.Generic;
using System.IO;
using BestHTTP;
using GameFramework;
using VEngine;

public class MailRankDataDownloadManager
{
	private static MailRankDataDownloadManager _instance;

	private static string MailRankDataFile = ".bin";

	private static string DataSaveFolder = "rankMail/";

	private static readonly long CdTime = 600000L;

	private List<string> _downloadingList = new List<string>();

	private Dictionary<string, long> _downloadCDDict = new Dictionary<string, long>();

	public static MailRankDataDownloadManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new MailRankDataDownloadManager();
			}
			return _instance;
		}
	}

	public void TryDownloadMailRankData(string uuid, ulong size, uint crc, string address)
	{
		if (string.IsNullOrWhiteSpace(uuid))
		{
			Log.Error("TryDownloadMailRankData uuid is empty");
			return;
		}
		bool flag = !string.IsNullOrEmpty(address);
		string mailRankDataHostByCurGroupType = NetworkURLConfig.GetMailRankDataHostByCurGroupType(flag, address);
		if (flag)
		{
			address = NetworkURLConfig.ModifyAddressStr(address);
		}
		string url = (flag ? (mailRankDataHostByCurGroupType + address) : (mailRankDataHostByCurGroupType + uuid + MailRankDataFile));
		if (string.IsNullOrEmpty(url))
		{
			Log.Error("url is empty");
		}
		else
		{
			if (_downloadingList.Contains(url))
			{
				return;
			}
			long serverTime = GameEntry.Timer.GetServerTime();
			if ((_downloadCDDict.ContainsKey(url) && serverTime < _downloadCDDict[url]) || Download.Cache.ContainsKey(url))
			{
				return;
			}
			string savePath = GetSavePath(uuid);
			HTTPRequest httpRequest = new HTTPRequest(new Uri(url), HTTPMethods.Get);
			if (GameEntry.Network.IfDownloadBattleReportDisableCache())
			{
				httpRequest.DisableCache = true;
			}
			httpRequest.Callback = delegate(HTTPRequest req, HTTPResponse resp)
			{
				byte[] array = null;
				if (resp != null)
				{
					if (resp.IsSuccess)
					{
						array = resp.Data;
					}
					else
					{
						_ = resp.StatusCode;
						long serverTime2 = GameEntry.Timer.GetServerTime();
						_downloadCDDict[url] = serverTime2 + CdTime;
					}
				}
				if (_downloadingList.Contains(url))
				{
					_downloadingList.Remove(url);
				}
				if (GameEntry.Lua != null && GameEntry.Lua.HasGameStart)
				{
					if (GameEntry.Network.IfUseZstdCompress(resp, uuid))
					{
						array = GameEntry.Network.ExecuteZstdDecompressor(array, uuid);
					}
					if (array != null)
					{
						try
						{
							string downloadDataPath = Versions.GetDownloadDataPath(DataSaveFolder);
							if (!Directory.Exists(downloadDataPath))
							{
								Directory.CreateDirectory(downloadDataPath);
							}
							File.WriteAllBytes(savePath, array);
						}
						catch (Exception ex)
						{
							Log.Error("WriteAllBytes Rank Report failed, reportId : {0}, error: {1}", uuid, ex.Message);
						}
					}
					GameEntry.Event.Fire(EventId.GetMailRankDataDownLoad, savePath);
				}
				httpRequest.Dispose();
			};
			httpRequest.Send();
			_downloadingList.Add(url);
		}
	}

	public string GetSavePath(string uuid)
	{
		return Versions.GetDownloadDataPath(DataSaveFolder + uuid);
	}
}
