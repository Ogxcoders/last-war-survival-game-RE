using System.Collections.Generic;
using System.Text;

namespace VEngine;

public sealed class DownloadVersions : Operation
{
	private readonly List<Download> downloads = new List<Download>();

	public DownloadInfo[] groups;

	public int queueID;

	public ulong downloadByteWithFakeStream;

	public ulong totalSize { get; private set; }

	public ulong downloadedBytes { get; private set; }

	public override void Start()
	{
		base.Start();
		DownloadInfo[] array = groups;
		foreach (DownloadInfo downloadInfo in array)
		{
			downloads.Add(Download.DownloadAsync(downloadInfo, queueID));
			totalSize += downloadInfo.size;
		}
		downloadedBytes = 0uL;
		downloadByteWithFakeStream = 0uL;
	}

	protected override void Update()
	{
		OperationStatus operationStatus = base.status;
		if (operationStatus != OperationStatus.Processing)
		{
			return;
		}
		downloadedBytes = 0uL;
		bool flag = true;
		foreach (Download download in downloads)
		{
			downloadedBytes += download.downloadedBytes;
			if (!download.isDone)
			{
				flag = false;
			}
		}
		if (downloadByteWithFakeStream < downloadedBytes)
		{
			downloadByteWithFakeStream = downloadedBytes;
		}
		base.progress = (float)downloadedBytes / (float)totalSize;
		if (!flag)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Download download2 in downloads)
		{
			if (!string.IsNullOrEmpty(download2.error))
			{
				stringBuilder.AppendLine(download2.error);
			}
		}
		Finish(stringBuilder.ToString());
	}

	public void Stop()
	{
		if (base.status != OperationStatus.Processing)
		{
			return;
		}
		foreach (Download download in downloads)
		{
			download.Cancel();
			Download.RemoveDownload(download);
		}
		Finish();
	}

	public void StopSpecialQueueDownload(int queue)
	{
		if (base.status != OperationStatus.Processing)
		{
			return;
		}
		foreach (Download download in downloads)
		{
			if (download.waitPrepareQueue == queue)
			{
				download.Cancel();
				Download.RemoveDownload(download);
			}
		}
		Finish();
	}
}
