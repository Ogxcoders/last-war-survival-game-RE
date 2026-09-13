using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BestHTTP;
using GameFramework;

public class MailBattleReportDownloadManager
{
	public struct DownloadData
	{
		public string uuid;

		public string extra;

		public bool isAddressMode;

		public string address;

		public bool isGetMode;

		public int cancelIndex;

		public bool isFull;
	}

	public class RetryDownloadData
	{
		public string uuid;

		public string backUpline;

		public int retryCount;

		public bool isDownloading;

		public DownloadData downloadData;
	}

	private const int SwitchLineRetryCount = 3;

	private static string BattleReportFile = ".bin";

	public static bool ForceUseOnlineCDN = false;

	private int _battleReportCancelIndex = -1;

	private int _maxDownloadNum = 10;

	private int _maxEventProcessPerFrame = 200;

	private int _maxSchedulePerFrame = 10;

	private Queue<DownloadData> downloadQueue;

	private List<HTTPRequest> downloadingQueue;

	private readonly Dictionary<string, RetryDownloadData> _failedDownloadDict;

	private readonly ConcurrentQueue<string> _successQueue;

	private readonly ConcurrentQueue<DownloadData> _failQueue;

	private bool _ifDownloadBattleReportSwitchLineOpen;

	private bool _initDownloadBattleReportSwitchLineOpen;

	private bool _hasStartedDownload;

	private bool _init;

	private bool _isOn;

	public MailBattleReportDownloadManager()
	{
		downloadQueue = new Queue<DownloadData>();
		downloadingQueue = new List<HTTPRequest>();
		_failedDownloadDict = new Dictionary<string, RetryDownloadData>();
		_successQueue = new ConcurrentQueue<string>();
		_failQueue = new ConcurrentQueue<DownloadData>();
	}

	private bool IfSwitchLineOn()
	{
		if (!_hasStartedDownload)
		{
			return false;
		}
		if (_initDownloadBattleReportSwitchLineOpen)
		{
			return _ifDownloadBattleReportSwitchLineOpen;
		}
		if (GameEntry.Lua != null && GameEntry.Lua.HasGameStart)
		{
			_ifDownloadBattleReportSwitchLineOpen = GameEntry.Lua.CallWithReturn<bool, string>("CSharpCallLuaInterface.CheckSwitch", "report_download_switchLine");
			_initDownloadBattleReportSwitchLineOpen = true;
		}
		return _ifDownloadBattleReportSwitchLineOpen;
	}

	public void OnUpdate()
	{
		ExecuteCallbackEvents();
		for (int num = downloadingQueue.Count - 1; num >= 0; num--)
		{
			if (downloadingQueue[num].State >= HTTPRequestStates.Finished)
			{
				downloadingQueue.RemoveAt(num);
			}
		}
		while (downloadQueue.Count > 0 && downloadingQueue.Count < _maxDownloadNum)
		{
			DownloadData data = downloadQueue.Dequeue();
			_DownloadBattleReport(data);
		}
		if (downloadingQueue.Count < _maxDownloadNum)
		{
			TryDownloadFromBackupLine();
		}
	}

	private void ExecuteCallbackEvents()
	{
		int i;
		for (i = 0; i < _maxEventProcessPerFrame; i++)
		{
			if (!_successQueue.TryDequeue(out var result))
			{
				break;
			}
			lock (_failedDownloadDict)
			{
				if (_failedDownloadDict.ContainsKey(result))
				{
					_failedDownloadDict.Remove(result);
				}
			}
		}
		for (; i < _maxEventProcessPerFrame; i++)
		{
			if (!_failQueue.TryDequeue(out var result2))
			{
				break;
			}
			lock (_failedDownloadDict)
			{
				if (_failedDownloadDict.TryGetValue(result2.uuid, out var value))
				{
					value.retryCount++;
					value.isDownloading = false;
					value.downloadData.cancelIndex = result2.cancelIndex;
				}
				else
				{
					value = new RetryDownloadData
					{
						uuid = result2.uuid,
						backUpline = "",
						retryCount = 1,
						isDownloading = false,
						downloadData = new DownloadData
						{
							uuid = result2.uuid,
							extra = result2.extra,
							isAddressMode = result2.isAddressMode,
							address = result2.address,
							cancelIndex = result2.cancelIndex,
							isGetMode = result2.isGetMode,
							isFull = result2.isFull
						}
					};
				}
				_failedDownloadDict[result2.uuid] = value;
			}
		}
	}

	private void TryDownloadFromBackupLine()
	{
		if (!IfSwitchLineOn())
		{
			return;
		}
		List<DownloadData> list = new List<DownloadData>();
		List<string> list2 = new List<string>();
		lock (_failedDownloadDict)
		{
			int num = Math.Max(0, _maxDownloadNum - downloadingQueue.Count);
			if (_maxSchedulePerFrame > 0)
			{
				num = Math.Min(num, _maxSchedulePerFrame);
			}
			if (num <= 0)
			{
				return;
			}
			KeyValuePair<string, RetryDownloadData>[] array = _failedDownloadDict.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<string, RetryDownloadData> keyValuePair = array[i];
				RetryDownloadData value = keyValuePair.Value;
				string[] backUpLineArr = GetBackUpLineArr(value.downloadData.address);
				int num2 = value.retryCount / 3;
				if (num2 == 0)
				{
					if (value.isDownloading)
					{
						continue;
					}
					value.isDownloading = true;
					_failedDownloadDict[keyValuePair.Key] = value;
					list.Add(value.downloadData);
				}
				else if (backUpLineArr != null && num2 <= backUpLineArr.Length)
				{
					if (value.isDownloading)
					{
						continue;
					}
					int num3 = num2 - 1;
					string text = backUpLineArr[num3];
					if (string.IsNullOrEmpty(text))
					{
						list2.Add(keyValuePair.Key);
						continue;
					}
					value.isDownloading = true;
					value.backUpline = text;
					_failedDownloadDict[keyValuePair.Key] = value;
					list.Add(value.downloadData);
				}
				else
				{
					list2.Add(keyValuePair.Key);
				}
				if (list.Count >= num)
				{
					break;
				}
			}
			foreach (string item in list2)
			{
				_failedDownloadDict.Remove(item);
			}
		}
		foreach (DownloadData item2 in list)
		{
			if (item2.isGetMode)
			{
				GetBattleReport(item2.uuid, item2.cancelIndex, item2.isFull, item2.isAddressMode, item2.address);
			}
			else
			{
				DownloadBattleReport(item2);
			}
		}
	}

	private string[] GetBackUpLineArr(string address)
	{
		if (string.IsNullOrEmpty(address))
		{
			return new string[0];
		}
		BattleReportOSSURLType battleReportOSSURLType = NetworkURLConfig.GetBattleReportOSSURLType(address);
		ConstURLConfig.BattleReportAddressBackupMap.TryGetValue(battleReportOSSURLType, out var value);
		return value;
	}

	private string GetBackUpLineById(string uuid)
	{
		if (!IfSwitchLineOn())
		{
			return "";
		}
		if (string.IsNullOrEmpty(uuid))
		{
			Log.Error("GetBackUpLineById uuid is empty");
			return "";
		}
		lock (_failedDownloadDict)
		{
			if (_failedDownloadDict.TryGetValue(uuid, out var value))
			{
				return value.backUpline;
			}
		}
		return "";
	}

	public void Shutdown()
	{
		_battleReportCancelIndex = -1;
	}

	public bool IsOpen()
	{
		return IsUnlockNewDownloader();
	}

	public bool IsUnlockNewDownloader()
	{
		if (_init)
		{
			return _isOn;
		}
		if (GameEntry.Lua != null && GameEntry.Lua.HasGameStart)
		{
			_isOn = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsUnlockNewBattleReportDownloader");
			_init = true;
		}
		return _isOn;
	}

	public void CancelBattleReport(int cancelIndex)
	{
		if (cancelIndex > _battleReportCancelIndex)
		{
			_battleReportCancelIndex = cancelIndex;
		}
	}

	public void GetBattleReport(string uuid, int cancelIndex, bool isFull, bool isAddressMode, string address)
	{
		_hasStartedDownload = true;
		if (string.IsNullOrWhiteSpace(uuid))
		{
			Log.Error("GetBattleReport reportId is empty");
			if (_battleReportCancelIndex < cancelIndex)
			{
				GameEntry.Lua.Call<byte[], int, string>("BattleReportUtil.Handle", null, cancelIndex, null);
			}
			return;
		}
		string backUpLineById = GetBackUpLineById(uuid);
		string text = (string.IsNullOrEmpty(backUpLineById) ? NetworkURLConfig.GetBattleReportHostByCurGroupType(ForceUseOnlineCDN, isFull, isAddressMode, address) : backUpLineById);
		string text2 = address;
		if (isAddressMode)
		{
			text2 = NetworkURLConfig.ModifyAddressStr(address);
		}
		string text3 = (isAddressMode ? (text + text2) : (text + uuid + BattleReportFile));
		if (string.IsNullOrEmpty(text3))
		{
			Log.Error("url is null ");
			return;
		}
		Log.Info("GetBattleReport send, reportId : {0}", uuid);
		SendHttpRequest(text3, delegate(HTTPRequest req, HTTPResponse resp)
		{
			bool flag = false;
			byte[] array = null;
			if (resp == null)
			{
				flag = true;
			}
			else if (resp.IsSuccess)
			{
				array = resp.Data;
			}
			else
			{
				flag = true;
			}
			if (IfSwitchLineOn())
			{
				if (flag)
				{
					_failQueue.Enqueue(new DownloadData
					{
						uuid = uuid,
						extra = "",
						isAddressMode = isAddressMode,
						address = address,
						cancelIndex = cancelIndex,
						isGetMode = true,
						isFull = isFull
					});
				}
				else
				{
					_successQueue.Enqueue(uuid);
				}
			}
			if (_battleReportCancelIndex < cancelIndex)
			{
				if (GameEntry.Network.IfUseZstdCompress(resp, uuid))
				{
					array = GameEntry.Network.ExecuteZstdDecompressor(array, uuid);
				}
				GameEntry.Lua.Call("BattleReportUtil.Handle", array, cancelIndex, uuid);
			}
		});
	}

	public void DownloadBattleReport(DownloadData data)
	{
		DownloadBattleReport(data.uuid, data.extra, data.isAddressMode, data.address, immediate: false);
	}

	public void DownloadBattleReport(string uuid, string extra, bool isAddressMode, string address, bool immediate)
	{
		_hasStartedDownload = true;
		if (!immediate && downloadingQueue.Count > _maxDownloadNum)
		{
			downloadQueue.Enqueue(new DownloadData
			{
				uuid = uuid,
				extra = extra,
				isAddressMode = isAddressMode,
				address = address,
				cancelIndex = -1,
				isGetMode = false,
				isFull = false
			});
		}
		else
		{
			_DownloadBattleReport(uuid, extra, isAddressMode, address);
		}
	}

	private void _DownloadBattleReport(DownloadData data)
	{
		_DownloadBattleReport(data.uuid, data.extra, data.isAddressMode, data.address);
	}

	private void _DownloadBattleReport(string uuid, string extra, bool isAddressMode, string address)
	{
		if (string.IsNullOrWhiteSpace(uuid))
		{
			return;
		}
		string backUpLineById = GetBackUpLineById(uuid);
		string text = (string.IsNullOrEmpty(backUpLineById) ? NetworkURLConfig.GetBattleReportDownloadHostByCurGroupType(isAddressMode, address, ForceUseOnlineCDN) : backUpLineById);
		string text2 = address;
		if (isAddressMode)
		{
			text2 = NetworkURLConfig.ModifyAddressStr(address);
		}
		string url = (isAddressMode ? (text + text2) : (text + uuid + BattleReportFile));
		SendHttpRequest(url, delegate(HTTPRequest req, HTTPResponse resp)
		{
			bool flag = false;
			byte[] array = null;
			int param = -1;
			if (resp == null)
			{
				flag = true;
			}
			else if (resp.IsSuccess)
			{
				array = resp.Data;
			}
			else
			{
				flag = true;
				param = resp.StatusCode;
			}
			if (IfSwitchLineOn())
			{
				if (flag)
				{
					_failQueue.Enqueue(new DownloadData
					{
						uuid = uuid,
						extra = extra,
						isAddressMode = isAddressMode,
						address = address,
						cancelIndex = -1,
						isGetMode = false,
						isFull = false
					});
				}
				else
				{
					_successQueue.Enqueue(uuid);
				}
			}
			if (GameEntry.Lua != null && GameEntry.Lua.HasGameStart)
			{
				if (GameEntry.Network.IfUseZstdCompress(resp, uuid))
				{
					array = GameEntry.Network.ExecuteZstdDecompressor(array, uuid);
				}
				GameEntry.Lua.Call("BattleReportUtil.OnBattleReportDownload", param, array, uuid, extra);
			}
		});
	}

	private void SendHttpRequest(string url, OnRequestFinishedDelegate callback)
	{
		HTTPRequest httpRequest = new HTTPRequest(new Uri(url), HTTPMethods.Get);
		if (GameEntry.Network.IfDownloadBattleReportDisableCache())
		{
			httpRequest.DisableCache = true;
		}
		httpRequest.Callback = delegate(HTTPRequest req, HTTPResponse resp)
		{
			callback?.Invoke(req, resp);
			httpRequest.Dispose();
		};
		httpRequest.Send();
		downloadingQueue.Add(httpRequest);
	}
}
