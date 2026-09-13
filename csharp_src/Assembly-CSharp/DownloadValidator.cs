using System;
using System.Collections.Generic;
using GameFramework;
using VEngine;

public class DownloadValidator
{
	private static List<Download> _downloads;

	private static bool _hasUpdateTask;

	private static QueuedThread _queuedThread;

	private static bool _validTaskFinish;

	private static bool _deleteTaskFinish;

	private static bool _downloadTaskFinish;

	public static void Start()
	{
		Log.Info("[DownloadValidator] Start.");
		Manifest manifest = Versions.GetManifest(GameEntry.Resource.GameResManifestName);
		if (manifest == null)
		{
			return;
		}
		List<DownloadInfo> downloadInfos = new List<DownloadInfo>();
		GameEntry.Resource.GetDownloadList(manifest, downloadInfos, 0);
		int[] requiredPackages = ResourcePackageManager.GetRequiredPackages();
		if (requiredPackages != null)
		{
			int i = 0;
			for (int num = requiredPackages.Length; i < num; i++)
			{
				GameEntry.Resource.GetDownloadList(manifest, downloadInfos, requiredPackages[i]);
			}
		}
		_downloads = new List<Download>();
		_validTaskFinish = false;
		_deleteTaskFinish = false;
		_downloadTaskFinish = false;
		_queuedThread = new QueuedThread("DownloadValidator", useCompleteTaskQueue: true);
		_queuedThread.Start();
		DownloadValidatorTask task = new DownloadValidatorTask(downloadInfos);
		_queuedThread.AddTask(task);
		DeleteUnversionedFileTask task2 = new DeleteUnversionedFileTask();
		_queuedThread.AddTask(task2);
		if (!_hasUpdateTask)
		{
			Updater.AddUpdateCallback(UpdateTask);
			_hasUpdateTask = true;
		}
	}

	public static void Stop()
	{
		Log.Info("[DownloadValidator] stop.");
		if (_hasUpdateTask)
		{
			Updater.RemoveUpdateCallback(UpdateTask);
			_hasUpdateTask = false;
		}
		if (_queuedThread != null)
		{
			_queuedThread.Stop();
			_queuedThread = null;
		}
		if (_downloads != null && _downloads.Count > 0)
		{
			for (int num = _downloads.Count - 1; num >= 0; num--)
			{
				_downloads[num].Cancel();
			}
			_downloads.Clear();
		}
	}

	private static void UpdateTask()
	{
		if (_queuedThread == null)
		{
			return;
		}
		if (_queuedThread.TryGetCompletedTask(out var task))
		{
			try
			{
				if (task is DownloadValidatorTask)
				{
					_validTaskFinish = true;
					DownloadValidatorTask downloadValidatorTask = (DownloadValidatorTask)task;
					Log.Info($"[DownloadValidator] UpdateTask check count:{downloadValidatorTask.checkList.Count}, download count: {downloadValidatorTask.downloadList.Count}.");
					StartDownload(downloadValidatorTask.downloadList);
					Log.Info("[DownloadValidator] DownloadValidatorTask is done.");
				}
				else if (task is DeleteUnversionedFileTask)
				{
					_deleteTaskFinish = true;
					foreach (KeyValuePair<string, string> valid in ((DeleteUnversionedFileTask)task).validList)
					{
						if (valid.Key.Contains("PVELevel"))
						{
							Log.Error("DownloadValidator SetBundlePathOrURl key:" + valid.Key + ". value:" + valid.Value);
						}
						Versions.SetBundlePathOrURl(valid.Key, valid.Value);
					}
					GameEntry.Event.Fire(EventId.EnterGameDeleingThreadComplete);
					Log.Info("[DownloadValidator] DeleteUnversionedFileTask is done.");
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
			}
		}
		if (_validTaskFinish && !_downloadTaskFinish)
		{
			for (int num = _downloads.Count - 1; num >= 0; num--)
			{
				if (_downloads[num].isDone)
				{
					_downloads.RemoveAt(num);
				}
			}
			_downloadTaskFinish = _downloads.Count <= 0;
			if (_downloadTaskFinish)
			{
				Log.Info("[DownloadValidator] DownloadTask is done.");
			}
		}
		if (_validTaskFinish && _downloadTaskFinish && _deleteTaskFinish)
		{
			Log.Info("[DownloadValidator] all task is done, remove update.");
			Stop();
		}
	}

	private static void StartDownload(List<DownloadInfo> downloadInfos)
	{
		bool flag = GameEntry.Setting.CheckFirstLaunchSkipUpdate();
		bool skipUpdateBundle = GameEntry.Resource.SkipUpdateBundle;
		bool isReview = GameEntry.Setting.IsReview;
		if (!(flag || skipUpdateBundle || isReview))
		{
			foreach (DownloadInfo downloadInfo in downloadInfos)
			{
				_downloads.Add(Download.DownloadAsync(downloadInfo, 0));
			}
		}
		else
		{
			Log.Info($"[DownloadValidator] StartDownload skip {flag}, {skipUpdateBundle}. {isReview}.");
		}
		_downloadTaskFinish = _downloads.Count <= 0;
	}
}
