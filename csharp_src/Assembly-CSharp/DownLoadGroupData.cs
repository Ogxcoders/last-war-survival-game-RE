using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using VEngine;

public class DownLoadGroupData
{
	public int configId = -1;

	public int[] packageIds;

	public Manifest manifest;

	private DownloadVersions _downloadVersions;

	public bool isDone;

	public string errMsg;

	private List<DownloadInfo> downloadInfos;

	public ulong totalSize;

	public string totalSizeMBStr;

	public ulong alreadyDownloadSize;

	private float delayFinishTime = 10f;

	private float delayFinishTimer;

	public ulong downloadSize
	{
		get
		{
			if (_downloadVersions != null)
			{
				return _downloadVersions.downloadedBytes;
			}
			if (isDone)
			{
				return totalSize;
			}
			return 0uL;
		}
	}

	public float TotalProgress
	{
		get
		{
			if (isDone)
			{
				return 1f;
			}
			if (_downloadVersions != null)
			{
				return (float)((double)(_downloadVersions.downloadedBytes + alreadyDownloadSize) / (double)totalSize);
			}
			if (totalSize != 0 && alreadyDownloadSize != 0)
			{
				return (float)((double)alreadyDownloadSize / (double)totalSize);
			}
			return 0f;
		}
	}

	public bool IsPaused
	{
		get
		{
			if (!isDone)
			{
				if (_downloadVersions != null)
				{
					return _downloadVersions.status != OperationStatus.Processing;
				}
				return true;
			}
			return false;
		}
	}

	public static float ByteToMegaByte(float byteSize)
	{
		return byteSize / 1048576f;
	}

	public void CalcDownloadInfo()
	{
		if (downloadInfos == null)
		{
			DownloadChecker downloadChecker = new DownloadChecker();
			downloadChecker.Collect(Versions.DownloadDataPath);
			Log.Info($"[DownloadManifestManager]  CalcDownloadInfo groupsName: : {manifest}");
			downloadInfos = new List<DownloadInfo>();
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			totalSize = 0uL;
			alreadyDownloadSize = 0uL;
			int i = 0;
			for (int num = packageIds.Length; i < num; i++)
			{
				(ulong, ulong) downloadSizeByPackage = GameEntry.Resource.GetDownloadSizeByPackage(manifest, downloadInfos, packageIds[i], null, downloadChecker);
				totalSize += downloadSizeByPackage.Item1;
				alreadyDownloadSize += downloadSizeByPackage.Item1 - downloadSizeByPackage.Item2;
			}
			totalSizeMBStr = ByteToMegaByte(totalSize).ToString("F2");
			realtimeSinceStartup = Time.realtimeSinceStartup - realtimeSinceStartup;
			Log.Info($"Calc download Info: {realtimeSinceStartup}");
		}
	}

	public void SendRequest()
	{
		if (_downloadVersions == null)
		{
			delayFinishTimer = 10f;
			if (downloadInfos == null)
			{
				CalcDownloadInfo();
			}
			isDone = false;
			_downloadVersions = GameEntry.Resource.DownloadUpdates(downloadInfos, 3);
			GameEntry.Event.Fire(EventId.LWSeasonResourceDownloadStart, configId);
		}
	}

	public void Stop()
	{
		_downloadVersions?.Stop();
		_downloadVersions = null;
		downloadInfos = null;
		delayFinishTimer = 0f;
	}

	public bool CheckFinish()
	{
		if (!isDone)
		{
			if (_downloadVersions != null && _downloadVersions.isDone)
			{
				isDone = true;
				errMsg = _downloadVersions.error;
				_downloadVersions = null;
				return true;
			}
			return false;
		}
		return true;
	}
}
