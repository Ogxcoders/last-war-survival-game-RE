using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using VEngine;

public class DownloadValidatorTask : IQueuedThreadTask
{
	public List<DownloadInfo> checkList;

	public List<DownloadInfo> downloadList;

	public DownloadValidatorTask(List<DownloadInfo> downloadInfos)
	{
		checkList = new List<DownloadInfo>(downloadInfos);
	}

	public void Process()
	{
		downloadList = new List<DownloadInfo>();
		foreach (DownloadInfo check in checkList)
		{
			try
			{
				string downloadDataPath = Versions.GetDownloadDataPath(Path.GetFileName(check.url));
				FileInfo fileInfo = new FileInfo(downloadDataPath);
				if (fileInfo.Exists && fileInfo.Length == (long)check.size)
				{
					continue;
				}
				if (fileInfo.Exists)
				{
					fileInfo.Delete();
				}
				if (downloadDataPath == check.savePath)
				{
					downloadList.Add(check);
					continue;
				}
				FileInfo fileInfo2 = new FileInfo(check.savePath);
				if (!fileInfo2.Exists || fileInfo2.Length != (long)check.size)
				{
					if (fileInfo2.Exists)
					{
						fileInfo2.Delete();
					}
					downloadList.Add(check);
				}
			}
			catch (Exception ex)
			{
				Log.Error($"{ex}, downloadInfo: {check.url}, {check.savePath}, {check.size}, {check.crc}");
			}
		}
	}
}
