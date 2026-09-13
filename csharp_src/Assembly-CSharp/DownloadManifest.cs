using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using BestHTTP;
using GameFramework;
using UnityEngine;
using UnityEngine.Networking;
using VEngine;

internal class DownloadManifest
{
	private enum Status
	{
		Loading,
		CheckVersion,
		Downloading,
		Success,
		Failed,
		WaitRetry
	}

	private Manifest _manifest;

	private ManifestVersionFile _versionFile;

	private string _versionName;

	private string _pathOrURL;

	private Status _status;

	private HTTPRequest _bestHttpRequest;

	private UnityWebRequest _unityWebRequest;

	private UnityWebRequestAsyncOperation _unityWebRequestOperation;

	private string _error;

	private int _retryTime;

	public const int MAX_RETRY_TIME = 5;

	private const float RETRY_INTERVAL = 1f;

	private float _retryDelay;

	private float _retryElapseTime;

	private Action<int> _retryAction;

	public bool isDone
	{
		get
		{
			if (_status != Status.Success)
			{
				return _status == Status.Failed;
			}
			return true;
		}
	}

	public string error => _error;

	public Manifest manifest => _manifest;

	public float downloadProgress { get; private set; }

	public void SetRetryAction(Action<int> retryAction)
	{
		_retryAction = retryAction;
	}

	public DownloadManifest(string name)
	{
		_manifest = new Manifest
		{
			name = name.ToLower(),
			onReadAsset = Versions.OnReadAsset
		};
	}

	public void Dispose()
	{
		_retryAction = null;
		AbortAllRequest();
	}

	public static DownloadManifest LoadAsync(string name)
	{
		DownloadManifest downloadManifest = new DownloadManifest(name);
		downloadManifest.Load();
		return downloadManifest;
	}

	private void Load()
	{
		_versionName = Manifest.GetVersionFile(_manifest.name);
		string tempDownloadPath = GameEntry.Resource.GetTempDownloadPath(_versionName);
		if (!File.Exists(tempDownloadPath))
		{
			Finish("version not exist.");
			return;
		}
		Log.Info("[DownloadManifest] download manifest -> version path:" + tempDownloadPath);
		_versionFile = ManifestVersionFile.Load(tempDownloadPath);
		int version = _versionFile.version;
		_pathOrURL = Versions.GetDownloadURL(string.Format("{0}{1}_v{2}", _manifest.name, "_small", version));
		_status = Status.CheckVersion;
		_retryTime = 0;
		_retryDelay = 0f;
	}

	public void OnUpdate()
	{
		switch (_status)
		{
		case Status.CheckVersion:
			UpdateVersion();
			break;
		case Status.Downloading:
			UpdateDownloading();
			break;
		case Status.Loading:
		{
			string tempDownloadPath = GameEntry.Resource.GetTempDownloadPath(_manifest.name);
			_manifest.Load(tempDownloadPath);
			Finish();
			break;
		}
		case Status.WaitRetry:
			UpdateRetry();
			break;
		case Status.Success:
		case Status.Failed:
			break;
		}
	}

	private void UpdateVersion()
	{
		string tempDownloadPath = GameEntry.Resource.GetTempDownloadPath(_manifest.name);
		if (Versions.Manifests.Exists((Manifest m) => m.version == _versionFile.version && _manifest.name.Contains(m.name)))
		{
			Log.Info("[DownloadManifest] Skip to download {0}, local version == remote version", _manifest.name);
			if (File.Exists(tempDownloadPath))
			{
				File.Delete(tempDownloadPath);
			}
			Finish();
			return;
		}
		if (File.Exists(tempDownloadPath))
		{
			using FileStream stream = File.OpenRead(tempDownloadPath);
			if (Utility.ComputeCRC32(stream) == _versionFile.crc)
			{
				Log.Info($"[DownloadManifest] Skip to download {_manifest.name},(same crc {_versionFile.crc})  {_manifest.version} == {_versionFile.version}");
				_status = Status.Loading;
				return;
			}
		}
		if (File.Exists(tempDownloadPath))
		{
			File.Delete(tempDownloadPath);
		}
		StartUnityWebRequestDownload();
	}

	private void StartBestHttpDownload()
	{
		AbortAllRequest();
		_status = Status.Downloading;
		string text = _pathOrURL;
		int num = _retryTime % ConstURLConfig.cdns.Length;
		if (num != 0 && text.StartsWith(ConstURLConfig.onlineDownloadURL_))
		{
			text = text.Replace(ConstURLConfig.onlineDownloadURL_, ConstURLConfig.cdns[num]);
		}
		Log.Info("[DownloadManifest] begin to download manifest use besthttp: " + text + " ");
		_bestHttpRequest = new HTTPRequest(new Uri(text), HTTPMethods.Get);
		_bestHttpRequest.Timeout = TimeSpan.FromSeconds(45.0);
		_bestHttpRequest.Callback = OnSendReportCallback;
		_bestHttpRequest.OnProgress = OnProgressCallback;
		_bestHttpRequest.Send();
		PostEventLog.TrackMap("DOWNLOAD_MANIFEST_START", new Dictionary<string, object> { 
		{
			"errMsg",
			_manifest.name ?? ""
		} });
	}

	private void OnProgressCallback(HTTPRequest req, long downloaded, long downloadLength)
	{
		if (downloadLength <= 0)
		{
			downloadProgress = 0f;
		}
		else
		{
			downloadProgress = (float)downloaded * 1f / (float)downloadLength;
		}
	}

	private void OnSendReportCallback(HTTPRequest req, HTTPResponse resp)
	{
		req.Callback = null;
		if (resp != null && resp.IsSuccess)
		{
			PostEventLog.TrackMap("DOWNLOAD_MANIFEST_SUCCESS", new Dictionary<string, object>
			{
				{ "errMsg", _manifest.name },
				{
					"curNetName",
					req?.Uri.ToString() ?? ""
				}
			});
			Log.Info("[DownloadManifest] finish use BestHttp: " + _manifest.name);
			try
			{
				string tempDownloadPath = GameEntry.Resource.GetTempDownloadPath(_manifest.name);
				Unzip(resp.Data, tempDownloadPath);
			}
			catch (Exception ex)
			{
				byte[] data = resp.Data;
				int num = ((data != null) ? data.Length : 0);
				string arg = ex.Message + "\n" + ex.StackTrace;
				PostEventLog.TrackMap("DOWNLOAD_MANIFEST_FAILED", new Dictionary<string, object> { 
				{
					"errMsg",
					$"{_manifest.name}. len: {num}. {arg}"
				} });
				Finish(arg);
				return;
			}
			_status = Status.Loading;
		}
		else if (_retryTime < 5)
		{
			Retry();
		}
		else
		{
			if (req.State == HTTPRequestStates.TimedOut)
			{
				PostEventLog.TrackMap("DOWNLOAD_MANIFEST_TIMEOUT", new Dictionary<string, object> { 
				{
					"errMsg",
					_manifest.name ?? ""
				} });
			}
			else
			{
				PostEventLog.TrackMap("DOWNLOAD_MANIFEST_FAILED", new Dictionary<string, object> { 
				{
					"errMsg",
					_manifest.name + " " + error
				} });
			}
			string text = "err";
			if (resp != null)
			{
				text = $"{resp.StatusCode}:{resp.Message}";
			}
			Finish(text);
		}
		_bestHttpRequest = null;
	}

	private void AbortAllRequest()
	{
		downloadProgress = 0f;
		if (_bestHttpRequest != null)
		{
			_bestHttpRequest.Abort();
			_bestHttpRequest.Dispose();
			_bestHttpRequest = null;
		}
		if (_unityWebRequestOperation != null)
		{
			_unityWebRequestOperation.completed -= OnUnityWebRequestOnCompleted;
			_unityWebRequestOperation = null;
		}
		if (_unityWebRequest != null)
		{
			_unityWebRequest.Abort();
			_unityWebRequest.Dispose();
			_unityWebRequest = null;
		}
	}

	private void StartUnityWebRequestDownload()
	{
		AbortAllRequest();
		_status = Status.Downloading;
		string text = _pathOrURL;
		int num = _retryTime % ConstURLConfig.cdns.Length;
		if (num != 0 && text.StartsWith(ConstURLConfig.onlineDownloadURL_))
		{
			text = text.Replace(ConstURLConfig.onlineDownloadURL_, ConstURLConfig.cdns[num]);
		}
		Log.Info("[DownloadManifest] begin to download manifest use UnityWebRequest: " + text + " ");
		_unityWebRequest = UnityWebRequest.Get(text);
		_unityWebRequest.timeout = 45;
		_unityWebRequestOperation = _unityWebRequest.SendWebRequest();
		_unityWebRequestOperation.completed += OnUnityWebRequestOnCompleted;
		PostEventLog.TrackMap("DOWNLOAD_MANIFEST_START", new Dictionary<string, object> { 
		{
			"errMsg",
			_manifest.name ?? ""
		} });
	}

	private void OnUnityWebRequestOnCompleted(AsyncOperation obj)
	{
		UnityWebRequest webRequest = (obj as UnityWebRequestAsyncOperation).webRequest;
		_unityWebRequestOperation = null;
		if (!string.IsNullOrEmpty(webRequest.error))
		{
			Log.Error($"[DownloadManifest]request failed. url: {webRequest.url}, code: {webRequest.responseCode}, error: {webRequest.error}");
			if (_retryTime < 5)
			{
				Retry();
			}
			else
			{
				Finish(webRequest.error);
			}
			return;
		}
		PostEventLog.TrackMap("DOWNLOAD_MANIFEST_SUCCESS", new Dictionary<string, object>
		{
			{ "errMsg", _manifest.name },
			{
				"curNetName",
				webRequest.url ?? ""
			}
		});
		Log.Info("[DownloadManifest] finish use UnityWebRequest: " + _manifest.name);
		try
		{
			string tempDownloadPath = GameEntry.Resource.GetTempDownloadPath(_manifest.name);
			Unzip(webRequest.downloadHandler.data, tempDownloadPath);
		}
		catch (Exception ex)
		{
			int num = webRequest?.downloadHandler?.data?.Length ?? 0;
			string arg = ex.Message + "\n" + ex.StackTrace;
			PostEventLog.TrackMap("DOWNLOAD_MANIFEST_FAILED", new Dictionary<string, object> { 
			{
				"errMsg",
				$"{_manifest.name}. len: {num}. {arg}"
			} });
			Finish(arg);
			return;
		}
		_status = Status.Loading;
	}

	private void UpdateDownloading()
	{
		if (_unityWebRequestOperation != null)
		{
			downloadProgress = _unityWebRequestOperation.progress;
		}
	}

	private void Retry()
	{
		_status = Status.WaitRetry;
		_retryTime++;
		_retryDelay += 1f;
		_retryElapseTime = 0f;
		_retryAction?.Invoke(_retryTime);
		Log.Info($"[DownloadManifest] download manifest:{_manifest.name} retry: {_retryTime}/{5}");
	}

	private void UpdateRetry()
	{
		_retryElapseTime += Time.deltaTime;
		if (_retryElapseTime >= _retryDelay)
		{
			PostEventLog.TrackMap("DOWNLOAD_MANIFEST_RETRY", new Dictionary<string, object> { 
			{
				"errMsg",
				$"{_retryTime}/{5} {_manifest.name}"
			} });
			if (_retryTime % 2 == 1)
			{
				StartBestHttpDownload();
			}
			else
			{
				StartUnityWebRequestDownload();
			}
		}
	}

	private void Finish(string error = null)
	{
		_error = error;
		_status = (string.IsNullOrEmpty(_error) ? Status.Success : Status.Failed);
		if (_status == Status.Success)
		{
			Log.Info("[DownloadManifest] download manifest Success: " + _manifest.name);
		}
		else
		{
			Log.Error("[DownloadManifest] download manifest Error: " + _manifest.name + " " + error);
		}
	}

	private void Unzip(byte[] data, string path)
	{
		using MemoryStream stream = new MemoryStream(data);
		using ZipArchive zipArchive = new ZipArchive(stream);
		string directoryName = Path.GetDirectoryName(path);
		foreach (ZipArchiveEntry entry in zipArchive.Entries)
		{
			string path2 = Path.Combine(directoryName, entry.FullName);
			Directory.CreateDirectory(Path.GetDirectoryName(path2));
			using Stream destination = File.Open(path2, FileMode.Create, FileAccess.Write, FileShare.None);
			using Stream stream2 = entry.Open();
			stream2.CopyTo(destination);
		}
	}
}
