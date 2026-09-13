using System;
using System.Collections.Generic;
using GameFramework;
using VEngine;

public class DownloadResGroupCommonData
{
	public int configId = -1;

	public int packageId;

	public Manifest manifest;

	private DownloadVersions _downloadVersions;

	public OperationStatus status;

	public string errMsg;

	private List<DownloadInfo> downloadInfos;

	private ulong downloadByteWithFakeStream;

	public ulong totalSize;

	public string totalSizeMBStr;

	public ulong alreadyDownloadSize;

	public List<BundleInfo> bundles;

	private List<BundleInfo> waitDeleteBundleInfoList = new List<BundleInfo>();

	public bool isDownloadSuccess => status == OperationStatus.Success;

	public ulong downloadSize
	{
		get
		{
			if (status == OperationStatus.Idle || status == OperationStatus.Processing)
			{
				if (_downloadVersions != null)
				{
					return _downloadVersions.downloadByteWithFakeStream;
				}
				return downloadByteWithFakeStream;
			}
			return downloadByteWithFakeStream;
		}
	}

	public float TotalProgress
	{
		get
		{
			if (status == OperationStatus.Idle)
			{
				if (totalSize != 0 && alreadyDownloadSize != 0)
				{
					return (float)((double)alreadyDownloadSize / (double)totalSize);
				}
			}
			else
			{
				if (status == OperationStatus.Success)
				{
					return 1f;
				}
				if (totalSize != 0)
				{
					return (downloadSize + alreadyDownloadSize) / totalSize;
				}
			}
			return 0f;
		}
	}

	public bool IsPaused
	{
		get
		{
			if (status != OperationStatus.Failed)
			{
				return status == OperationStatus.Success;
			}
			return true;
		}
	}

	public event Action<DownloadResGroupCommonData> completed;

	public event Action<DownloadResGroupCommonData> deletedCompleted;

	public void ClearCompleted()
	{
		this.completed = null;
	}

	public void ClearDeletedCompleted()
	{
		this.deletedCompleted = null;
	}

	public void InitBundleInfo()
	{
		if (bundles == null)
		{
			bundles = Versions.GetBundlesWithPackages(new Manifest[1] { manifest }, packageId);
			if (bundles.Count == 0)
			{
				Log.Error("InitBundleInfo  Package：" + configId + " 的所有资源都在本包体内！分包部分没有额外资源！请检查！可能是因为逻辑中不包含该PackageId，也可能是资源分的有问题！");
			}
		}
	}

	public List<BundleInfo> CalcCanDeleteBundleInfoList(HashSet<int> tmpDeletePackageIdSet)
	{
		waitDeleteBundleInfoList.Clear();
		if (bundles == null)
		{
			return waitDeleteBundleInfoList;
		}
		for (int i = 0; i < bundles.Count; i++)
		{
			BundleInfo bundleInfo = bundles[i];
			if (!bundleInfo.isSplitBundle)
			{
				continue;
			}
			bool flag = true;
			for (int j = 0; j < bundleInfo.downloadModes.Length; j++)
			{
				if (bundleInfo.downloadModes[j] != 0 && !tmpDeletePackageIdSet.Contains(bundleInfo.downloadModes[j]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				waitDeleteBundleInfoList.Add(bundleInfo);
			}
		}
		return waitDeleteBundleInfoList;
	}

	public bool IsDeleteTotallyCompleted()
	{
		if (waitDeleteBundleInfoList.Count <= 0)
		{
			return true;
		}
		for (int i = 0; i < waitDeleteBundleInfoList.Count; i++)
		{
			if (ResourcePackageManager.IsDownloaded(waitDeleteBundleInfoList[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static float ByteToMegaByte(float byteSize)
	{
		return byteSize / 1048576f;
	}

	public void RecalcDownloadInfo()
	{
		totalSize = 0uL;
		alreadyDownloadSize = 0uL;
		downloadInfos = new List<DownloadInfo>();
		(ulong, ulong) downloadSizeByPackage = GameEntry.Resource.GetDownloadSizeByPackage2(manifest, downloadInfos, packageId);
		totalSize = downloadSizeByPackage.Item1;
		alreadyDownloadSize = downloadSizeByPackage.Item1 - downloadSizeByPackage.Item2;
		totalSizeMBStr = ByteToMegaByte(totalSize).ToString("F2");
	}

	public void SendRequest()
	{
		if (_downloadVersions == null)
		{
			DownloadResGroupCommonManager.Instance.StopDeletingPackage(configId);
			downloadInfos = new List<DownloadInfo>();
			GameEntry.Resource.GetDownloadSizeByPackage2(manifest, downloadInfos, packageId);
			if (packageId == 20243)
			{
				_downloadVersions = GameEntry.Resource.DownloadUpdates(downloadInfos, 2);
			}
			else
			{
				_downloadVersions = GameEntry.Resource.DownloadUpdates(downloadInfos, 4);
			}
			List<DownloadInfo> list = new List<DownloadInfo>();
			(ulong, ulong) downloadSizeByPackage = GameEntry.Resource.GetDownloadSizeByPackage2(manifest, list, packageId);
			ulong num = downloadSizeByPackage.Item1 - downloadSizeByPackage.Item2;
			if (alreadyDownloadSize < num)
			{
				alreadyDownloadSize = num;
			}
			_downloadVersions.downloadByteWithFakeStream = alreadyDownloadSize - num;
			downloadByteWithFakeStream = _downloadVersions.downloadByteWithFakeStream;
			status = _downloadVersions.status;
			GameEntry.Event.Fire(EventId.CommonResourceGroupDownloadStart, configId);
			DownloadResGroupCommonManager.RemoveWaitDeletePackageId(packageId);
			DownloadResGroupCommonManager.SetPackageDeleteTimes(packageId, 0);
		}
	}

	public void Stop()
	{
		if (_downloadVersions != null)
		{
			alreadyDownloadSize += _downloadVersions.downloadByteWithFakeStream;
			downloadByteWithFakeStream = 0uL;
		}
		if (packageId == 20243)
		{
			_downloadVersions?.StopSpecialQueueDownload(2);
		}
		else
		{
			_downloadVersions?.StopSpecialQueueDownload(4);
		}
		_downloadVersions = null;
		downloadInfos = null;
	}

	public bool CheckFinish()
	{
		if (status == OperationStatus.Success || status == OperationStatus.Failed)
		{
			return true;
		}
		if (_downloadVersions != null && _downloadVersions.isDone)
		{
			status = _downloadVersions.status;
			errMsg = _downloadVersions.error;
			if (status == OperationStatus.Success)
			{
				RecalcDownloadInfo();
			}
			else if (status == OperationStatus.Failed)
			{
				alreadyDownloadSize += _downloadVersions.downloadByteWithFakeStream;
			}
			_downloadVersions.downloadByteWithFakeStream = 0uL;
			downloadByteWithFakeStream = 0uL;
			_downloadVersions = null;
			if (this.completed != null)
			{
				this.completed(this);
				this.completed = null;
			}
			return true;
		}
		return false;
	}

	public void InvokeDeletedCompleted()
	{
		status = OperationStatus.Idle;
		RecalcDownloadInfo();
		if (this.deletedCompleted != null)
		{
			this.deletedCompleted(this);
			this.deletedCompleted = null;
		}
		downloadByteWithFakeStream = 0uL;
	}

	public void Destroy()
	{
	}
}
