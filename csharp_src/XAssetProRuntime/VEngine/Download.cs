using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using GameFramework;
using UnityEngine;
using UnityEngine.Networking;

namespace VEngine;

public class Download : CustomYieldInstruction
{
	private class CdnPerformance
	{
		public int DownloadCount;

		public double TotalDownloadTime;

		public ulong TotalDownloadBytes;

		public int FailureCount;

		public double AverageSpeed
		{
			get
			{
				if (TotalDownloadBytes == 0L || TotalDownloadTime <= 0.0)
				{
					return 0.0;
				}
				return (double)TotalDownloadBytes / TotalDownloadTime;
			}
		}

		public double SuccessRate
		{
			get
			{
				if (DownloadCount <= 0)
				{
					return 0.0;
				}
				int val = DownloadCount - FailureCount;
				return (double)Math.Max(0, val) * 1.0 / (double)DownloadCount;
			}
		}

		public bool HasSufficientData => DownloadCount >= CDN_SPEED_TEST_COUNT;

		public double CalculateScore()
		{
			if (DownloadCount == 0)
			{
				return 0.0;
			}
			double successRate = SuccessRate;
			double averageSpeed = AverageSpeed;
			if (successRate <= 0.0)
			{
				return -1.0;
			}
			if (averageSpeed <= 0.0 && successRate > 0.0)
			{
				return successRate * 0.1;
			}
			return Math.Min(averageSpeed / 1024.0, 1000000.0) * successRate;
		}

		public void AddDownload(double downloadTime, ulong bytes, bool success)
		{
			DownloadCount++;
			TotalDownloadTime += downloadTime;
			TotalDownloadBytes += bytes;
			if (!success)
			{
				FailureCount++;
			}
		}
	}

	public const int QUEUE_Start = 0;

	public const int QUEUE_HotUpdate = 0;

	public const int QUEUE_RemoteAsset = 1;

	public const int QUEUE_DownloadCenterAsset = 2;

	public const int QUEUE_SeasonAsset = 3;

	public const int QUEUE_OtherPackageAsset = 4;

	public const int QUEUE_Warmup = 5;

	public const int QUEUE_MAX = 6;

	public static bool USE_DOWNLOAD_QUEUE = true;

	public static ulong MaxBandwidth = 0uL;

	public static int MaxRetryTimes = 5;

	public static uint ReadBufferSize = 4096u;

	private static DownloadPrepareQueue[] PrepareQueues;

	public static readonly List<Download> Progressing = new List<Download>();

	public static readonly Dictionary<string, Download> Cache = new Dictionary<string, Download>();

	public static string FtpUserID;

	public static string FtpPassword;

	private static float lastSampleTime;

	private static ulong lastTotalDownloadedBytes;

	public static bool ASYNC_CALL = false;

	private readonly byte[] _readBuffer = new byte[ReadBufferSize];

	private ulong _bandWidth;

	private Thread _thread;

	private bool _asyncCall;

	private float _warmupTime = 0.006f;

	private float _warmupETA = 0.006f;

	private int _retryTimes;

	private FileStream _writer;

	private int _usedCdnIndex;

	private Stopwatch _downloadStopwatch;

	public int waitPrepareQueue = -1;

	private static readonly string[] cdns = new string[4] { "https://lastwar-cdn.akamaized.net/hotupdate/", "https://lastwar-cdn.lastwarapp.com/hotupdate/", "https://cdn.lastwar.com/hotupdate/", "https://lastwar.asia-cdn.com/hotupdate/" };

	public static bool USE_CDN_SPEED_TEST = false;

	public static int CDN_SPEED_TEST_COUNT = 5;

	private static readonly Dictionary<int, CdnPerformance> _cdnPerformances = new Dictionary<int, CdnPerformance>();

	private static int _preferredCdnIndex = 0;

	private static bool _speedTestCompleted = false;

	private static int _cdnTestCounter = 0;

	private UnityWebRequestAsyncOperation _request;

	private float _lastUpdateTime;

	private ulong _lastDownloadedBytes;

	private int _stuck;

	private const int TIME_OUT = 10;

	public DownloadInfo info { get; private set; }

	public DownloadStatus status { get; private set; }

	public string error { get; private set; }

	public Action<Download> completed { get; set; }

	public static Action<Download> kCompleted { get; set; }

	public bool isDone
	{
		get
		{
			if (status != DownloadStatus.Failed)
			{
				return status == DownloadStatus.Success;
			}
			return true;
		}
	}

	public bool isRetry => _retryTimes > 0;

	public float createTime { get; set; }

	public float progress => (float)downloadedBytes * 1f / (float)info.size;

	public ulong downloadedBytes { get; private set; }

	public override bool keepWaiting => !isDone;

	public static bool Working => Progressing.Count > 0;

	public static ulong TotalDownloadedBytes
	{
		get
		{
			ulong num = 0uL;
			foreach (KeyValuePair<string, Download> item in Cache)
			{
				num += item.Value.downloadedBytes;
			}
			return num;
		}
	}

	public static ulong TotalSize
	{
		get
		{
			ulong num = 0uL;
			foreach (KeyValuePair<string, Download> item in Cache)
			{
				num += item.Value.info.size;
			}
			return num;
		}
	}

	public static ulong TotalBandwidth { get; private set; }

	private static string ori_cdn => cdns[0];

	public static int total_download_count { get; private set; }

	private Download()
	{
		status = DownloadStatus.Wait;
		downloadedBytes = 0uL;
		createTime = Time.realtimeSinceStartup;
	}

	public static void InitStatic()
	{
		if (PrepareQueues == null)
		{
			PrepareQueues = new DownloadPrepareQueue[6];
			PrepareQueues[0] = new DownloadPrepareQueue(5u);
			PrepareQueues[1] = new DownloadPrepareQueue(5u);
			PrepareQueues[2] = new DownloadPrepareQueue(1u);
			PrepareQueues[3] = new DownloadPrepareQueue(1u);
			PrepareQueues[4] = new DownloadPrepareQueue(1u);
			PrepareQueues[5] = new DownloadPrepareQueue(1u);
		}
	}

	public static void ClearAllDownloads()
	{
		foreach (Download item in Progressing)
		{
			item.Cancel();
		}
		DownloadPrepareQueue[] prepareQueues = PrepareQueues;
		for (int i = 0; i < prepareQueues.Length; i++)
		{
			prepareQueues[i].Prepared.Clear();
		}
		Progressing.Clear();
		Cache.Clear();
	}

	public static Download DownloadAsync(string url, string savePath, Action<Download> completed = null, ulong size = 0uL, uint crc = 0u)
	{
		return DownloadAsync(new DownloadInfo
		{
			url = url,
			savePath = savePath,
			crc = crc,
			size = size
		}, completed);
	}

	public static Download DownloadAsync(string url, string savePath, int queueID, Action<Download> completed = null, ulong size = 0uL, uint crc = 0u)
	{
		return DownloadAsync(new DownloadInfo
		{
			url = url,
			savePath = savePath,
			crc = crc,
			size = size
		}, queueID, completed);
	}

	public static Download DownloadAsync(DownloadInfo info, Action<Download> completed = null)
	{
		if (!Cache.TryGetValue(info.url, out var value))
		{
			value = new Download
			{
				info = info,
				waitPrepareQueue = 0
			};
			PrepareQueues[0].Prepared.Add(value);
			Cache.Add(info.url, value);
		}
		else
		{
			Logger.W("Download url {0} already exist.", info.url);
		}
		if (completed != null)
		{
			Download download = value;
			download.completed = (Action<Download>)Delegate.Combine(download.completed, completed);
		}
		return value;
	}

	public static Download DownloadAsync(DownloadInfo info, int queueID, Action<Download> completed = null)
	{
		if (!USE_DOWNLOAD_QUEUE)
		{
			queueID = 0;
		}
		if (queueID < 0 || queueID >= 6)
		{
			queueID = 0;
		}
		if (!Cache.TryGetValue(info.url, out var value))
		{
			value = new Download
			{
				info = info,
				waitPrepareQueue = queueID
			};
			PrepareQueues[queueID].Prepared.Add(value);
			Cache.Add(info.url, value);
		}
		else if (value.waitPrepareQueue > queueID && value.waitPrepareQueue >= 0)
		{
			List<Download> prepared = PrepareQueues[value.waitPrepareQueue].Prepared;
			for (int i = 0; i < prepared.Count; i++)
			{
				if (prepared[i] == value)
				{
					prepared.RemoveAt(i);
					break;
				}
			}
			PrepareQueues[queueID].Prepared.Add(value);
			value.waitPrepareQueue = queueID;
		}
		if (completed != null)
		{
			Download download = value;
			download.completed = (Action<Download>)Delegate.Combine(download.completed, completed);
		}
		return value;
	}

	public static Download RemoveDownload(Download download)
	{
		DownloadPrepareQueue[] prepareQueues = PrepareQueues;
		for (int i = 0; i < prepareQueues.Length; i++)
		{
			prepareQueues[i].Prepared.Remove(download);
		}
		Progressing.Remove(download);
		if (Cache.ContainsKey(download.info.url))
		{
			Cache.Remove(download.info.url);
		}
		else
		{
			Logger.W("RemoveDownload url {0} not exist.", download.info.url);
		}
		return download;
	}

	public static void UpdateDownloads()
	{
		DownloadPrepareQueue[] prepareQueues = PrepareQueues;
		foreach (DownloadPrepareQueue downloadPrepareQueue in prepareQueues)
		{
			if (downloadPrepareQueue.Prepared.Count > 0)
			{
				int num = 0;
				while ((float)num < Mathf.Min(downloadPrepareQueue.Prepared.Count, downloadPrepareQueue.MaxDownloads - Progressing.Count))
				{
					Download download = downloadPrepareQueue.Prepared[num];
					downloadPrepareQueue.Prepared.RemoveAt(num);
					num--;
					Progressing.Add(download);
					download.Start(ASYNC_CALL);
					num++;
				}
				break;
			}
		}
		if (Progressing.Count > 0)
		{
			for (int j = 0; j < Progressing.Count; j++)
			{
				Download download2 = Progressing[j];
				download2.Update();
				if (download2.status == DownloadStatus.Failed || download2.status == DownloadStatus.DownloadFinsih || download2.status == DownloadStatus.Success)
				{
					if (download2.status == DownloadStatus.Failed)
					{
						Log.Error("Unable to download {0} with error {1}", download2.info.url, download2.error);
					}
					Progressing.RemoveAt(j);
					j--;
					download2.Complete();
				}
			}
			if (Time.realtimeSinceStartup - lastSampleTime >= 1f)
			{
				TotalBandwidth = TotalDownloadedBytes - lastTotalDownloadedBytes;
				lastTotalDownloadedBytes = TotalDownloadedBytes;
				lastSampleTime = Time.realtimeSinceStartup;
			}
		}
		else if (Cache.Count > 0)
		{
			Cache.Clear();
			lastTotalDownloadedBytes = 0uL;
			lastSampleTime = Time.realtimeSinceStartup;
		}
	}

	public void Retry()
	{
		status = DownloadStatus.Wait;
		Start();
	}

	public void UnPause()
	{
		Retry();
	}

	public void Pause()
	{
		status = DownloadStatus.Wait;
	}

	public void Cancel()
	{
		error = "User Cancel.";
		status = DownloadStatus.Failed;
		StopAsyncCall();
	}

	private void Complete()
	{
		CheckStatus();
		kCompleted?.Invoke(this);
		if (completed != null)
		{
			completed(this);
			completed = null;
		}
	}

	private void Run()
	{
		try
		{
			Downloading();
			if (_writer != null)
			{
				_writer.Flush();
				_writer.Close();
				_writer = null;
			}
		}
		catch (Exception ex)
		{
			if (_writer != null)
			{
				_writer.Flush();
				_writer.Close();
				_writer = null;
			}
			error = $"Exception in Downloading Run {info.url} {ex.Message}";
			if (_retryTimes < MaxRetryTimes)
			{
				Thread.Sleep(1000);
				Retry();
				_retryTimes++;
			}
			else
			{
				status = DownloadStatus.Failed;
			}
		}
	}

	private void CheckStatus()
	{
		if (status != DownloadStatus.DownloadFinsih)
		{
			return;
		}
		if (downloadedBytes != info.size)
		{
			error = $"{info.url} 长度 {downloadedBytes} 不符合期望 {info.size}";
			status = DownloadStatus.Failed;
			return;
		}
		if (info.crc != 0)
		{
			uint num = Utility.ComputeCRC32(info.savePath + ".bak");
			if (info.crc != num)
			{
				error = $"{info.url} crc {num} 不符合期望 {info.crc}";
				status = DownloadStatus.Failed;
				return;
			}
		}
		if (File.Exists(info.savePath))
		{
			File.Delete(info.savePath);
		}
		File.Move(info.savePath + ".bak", info.savePath);
		status = DownloadStatus.Success;
		total_download_count++;
		_downloadStopwatch?.Stop();
		double downloadTime = _downloadStopwatch?.Elapsed.TotalSeconds ?? 0.0;
		UpdateCdnStats(_usedCdnIndex, downloadTime, downloadedBytes, success: true);
	}

	private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors spe)
	{
		return true;
	}

	private void Downloading()
	{
		using WebResponse webResponse = CreateWebRequest().GetResponse();
		if (webResponse.ContentLength > 0)
		{
			if (info.size == 0L)
			{
				info.size = (ulong)webResponse.ContentLength + downloadedBytes;
			}
			using Stream reader = webResponse.GetResponseStream();
			if (downloadedBytes < info.size)
			{
				DateTime startTime = DateTime.Now;
				while (status == DownloadStatus.Progressing && !ReadToEnd(reader))
				{
					UpdateBandwidth(ref startTime);
				}
			}
			if (_writer != null)
			{
				_writer.Flush();
				_writer.Close();
				_writer = null;
			}
			status = DownloadStatus.DownloadFinsih;
			return;
		}
		if (_writer != null)
		{
			_writer.Flush();
			_writer.Close();
			_writer = null;
		}
		status = DownloadStatus.DownloadFinsih;
	}

	private void UpdateBandwidth(ref DateTime startTime)
	{
		double totalMilliseconds = (DateTime.Now - startTime).TotalMilliseconds;
		while (MaxBandwidth != 0 && status == DownloadStatus.Progressing && _bandWidth >= MaxBandwidth / (ulong)Progressing.Count && totalMilliseconds < 1000.0)
		{
			Thread.Sleep(Mathf.Clamp((int)(1000.0 - totalMilliseconds), 1, 33));
			totalMilliseconds = (DateTime.Now - startTime).TotalMilliseconds;
		}
		if (totalMilliseconds >= 1000.0)
		{
			startTime = DateTime.Now;
			TotalBandwidth = _bandWidth;
			_bandWidth = 0uL;
		}
	}

	private WebRequest CreateWebRequest()
	{
		if (info.url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
		{
			ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
			return GetHttpWebRequest();
		}
		if (info.url.StartsWith("ftp", StringComparison.OrdinalIgnoreCase))
		{
			FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(info.url);
			ftpWebRequest.Method = "RETR";
			if (!string.IsNullOrEmpty(FtpUserID))
			{
				ftpWebRequest.Credentials = new NetworkCredential(FtpUserID, FtpPassword);
			}
			if (downloadedBytes != 0)
			{
				ftpWebRequest.ContentOffset = (int)downloadedBytes;
			}
			return ftpWebRequest;
		}
		return GetHttpWebRequest();
	}

	public static void ResetCounter()
	{
		total_download_count = 0;
		_cdnPerformances.Clear();
		_speedTestCompleted = false;
		_preferredCdnIndex = 0;
		_cdnTestCounter = 0;
	}

	private WebRequest GetHttpWebRequest()
	{
		string text = info.url;
		int num = (_usedCdnIndex = SelectCdnIndex());
		if (num != 0 && text.StartsWith(ori_cdn))
		{
			text = text.Replace(ori_cdn, cdns[num]);
		}
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
		httpWebRequest.ProtocolVersion = HttpVersion.Version10;
		if (downloadedBytes != 0)
		{
			httpWebRequest.AddRange((int)downloadedBytes);
		}
		return httpWebRequest;
	}

	private bool ReadToEnd(Stream reader)
	{
		if (reader != null)
		{
			int num = reader.Read(_readBuffer, 0, _readBuffer.Length);
			if (num > 0)
			{
				_writer.Write(_readBuffer, 0, num);
				downloadedBytes += (ulong)num;
				_bandWidth += (ulong)num;
				return false;
			}
			return true;
		}
		error = "reader == null";
		status = DownloadStatus.Failed;
		return true;
	}

	private void Start(bool asyncCall = false)
	{
		if (status != DownloadStatus.Wait)
		{
			return;
		}
		status = DownloadStatus.Progressing;
		FileInfo fileInfo = new FileInfo(info.savePath);
		if (fileInfo.Exists && fileInfo.Length > 0 && info.size != 0 && fileInfo.Length == (long)info.size)
		{
			downloadedBytes = info.size;
			status = DownloadStatus.Success;
			return;
		}
		if (!string.IsNullOrEmpty(info.warmupPath))
		{
			FileInfo fileInfo2 = new FileInfo(info.warmupPath);
			if (fileInfo2.Exists && fileInfo2.Length > 0 && info.size != 0 && fileInfo2.Length == (long)info.size)
			{
				downloadedBytes = 0uL;
				_warmupTime = UnityEngine.Random.Range(0.004f, 0.008f);
				_warmupETA = _warmupTime;
				status = DownloadStatus.UseWarmup;
				return;
			}
		}
		downloadedBytes = 0uL;
		if (asyncCall || _asyncCall)
		{
			StartAsync();
			return;
		}
		string directoryName = Path.GetDirectoryName(info.savePath);
		if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		_writer = File.Create(info.savePath + ".bak");
		_downloadStopwatch = Stopwatch.StartNew();
		_thread = new Thread(Run)
		{
			IsBackground = true
		};
		_thread.Start();
	}

	private void StartAsync()
	{
		_asyncCall = true;
		string path = info.savePath + ".bak";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		string text = info.url;
		int num = (_usedCdnIndex = SelectCdnIndex());
		_downloadStopwatch = Stopwatch.StartNew();
		if (num != 0 && text.StartsWith(ori_cdn))
		{
			text = text.Replace(ori_cdn, cdns[num]);
		}
		UnityWebRequest unityWebRequest = new UnityWebRequest(text, "GET");
		unityWebRequest.downloadHandler = new DownloadHandlerFile(path);
		_request = unityWebRequest.SendWebRequest();
		_request.completed += OnRequestCompleted;
		_lastUpdateTime = Time.realtimeSinceStartup;
		_lastDownloadedBytes = 0uL;
		_stuck = 0;
	}

	private void StopAsyncCall()
	{
		if (_asyncCall && _request != null)
		{
			_request.completed -= OnRequestCompleted;
			_request.webRequest.Dispose();
			_request = null;
		}
	}

	private bool TryRestart()
	{
		if (_retryTimes < MaxRetryTimes)
		{
			_retryTimes++;
			Retry();
			return false;
		}
		return true;
	}

	private void OnRequestCompleted(AsyncOperation op)
	{
		op.completed -= OnRequestCompleted;
		if (isDone || _request == null || _request != op)
		{
			return;
		}
		UnityWebRequest webRequest = _request.webRequest;
		downloadedBytes = webRequest.downloadedBytes;
		status = DownloadStatus.DownloadFinsih;
		if (!webRequest.isNetworkError && !webRequest.isHttpError)
		{
			if (info.size == 0 && long.TryParse(webRequest.GetResponseHeader("Content-Length"), out var result))
			{
				info.size = (ulong)result;
			}
			StopAsyncCall();
			CheckDownloadFile();
			return;
		}
		StopAsyncCall();
		_downloadStopwatch?.Stop();
		double downloadTime = _downloadStopwatch?.Elapsed.TotalSeconds ?? 0.0;
		UpdateCdnStats(_usedCdnIndex, downloadTime, downloadedBytes, success: false);
		if (!TryRestart())
		{
			return;
		}
		string text = "unknow err";
		try
		{
			text = webRequest.error;
		}
		finally
		{
			error = "download failed error: " + text + ", url: " + info.url;
			status = DownloadStatus.Failed;
		}
	}

	private void CheckDownloadFile()
	{
		if (downloadedBytes != info.size)
		{
			if (TryRestart())
			{
				error = $"{info.url} 长度 {downloadedBytes} 不符合期望 {info.size}";
				status = DownloadStatus.Failed;
			}
		}
		else if (info.crc != 0)
		{
			uint num = Utility.ComputeCRC32(info.savePath + ".bak");
			if (info.crc != num && TryRestart())
			{
				error = $"{info.url} crc {num} 不符合期望 {info.crc}";
				status = DownloadStatus.Failed;
			}
		}
	}

	private void Update()
	{
		if (_asyncCall)
		{
			UpdateAsyncCall();
		}
		else if (status == DownloadStatus.UseWarmup)
		{
			_warmupETA -= Time.unscaledDeltaTime;
			if (_warmupETA <= 0f)
			{
				_warmupETA = 0f;
				File.Move(info.warmupPath, info.savePath);
				downloadedBytes = info.size;
				status = DownloadStatus.Success;
			}
			else
			{
				downloadedBytes = (ulong)((1f - _warmupETA / _warmupTime) * (float)info.size);
			}
		}
	}

	private void UpdateAsyncCall()
	{
		if (_request == null)
		{
			return;
		}
		downloadedBytes = _request.webRequest.downloadedBytes;
		if (Time.realtimeSinceStartup - _lastUpdateTime > 1f)
		{
			if (downloadedBytes != _lastDownloadedBytes)
			{
				_lastDownloadedBytes = downloadedBytes;
				_stuck = 0;
			}
			else
			{
				_stuck++;
			}
			_lastUpdateTime = Time.realtimeSinceStartup;
		}
		if (_stuck >= 10)
		{
			StopAsyncCall();
			_downloadStopwatch?.Stop();
			double downloadTime = _downloadStopwatch?.Elapsed.TotalSeconds ?? 0.0;
			UpdateCdnStats(_usedCdnIndex, downloadTime, downloadedBytes, success: false);
			if (TryRestart())
			{
				error = "download failed timeout url: " + info.url;
				status = DownloadStatus.Failed;
			}
		}
	}

	private int SelectCdnIndex()
	{
		if (!USE_CDN_SPEED_TEST)
		{
			return _retryTimes % cdns.Length;
		}
		if (!_speedTestCompleted)
		{
			if (_retryTimes == 0)
			{
				_usedCdnIndex = _cdnTestCounter++ % cdns.Length;
				return _usedCdnIndex;
			}
			return (_usedCdnIndex + _retryTimes) % cdns.Length;
		}
		if (_retryTimes == 0)
		{
			_usedCdnIndex = _preferredCdnIndex;
			return _preferredCdnIndex;
		}
		return (_preferredCdnIndex + _retryTimes) % cdns.Length;
	}

	private void UpdateCdnStats(int cdnIndex, double downloadTime, ulong bytes, bool success)
	{
		if (USE_CDN_SPEED_TEST)
		{
			if (!_cdnPerformances.ContainsKey(cdnIndex))
			{
				_cdnPerformances[cdnIndex] = new CdnPerformance();
			}
			_cdnPerformances[cdnIndex].AddDownload(downloadTime, bytes, success);
			if (!_speedTestCompleted && ShouldCompleteTesting(out var bestCdnIndex))
			{
				_preferredCdnIndex = bestCdnIndex;
				_speedTestCompleted = true;
			}
		}
	}

	private static bool ShouldCompleteTesting(out int bestCdnIndex)
	{
		bestCdnIndex = 0;
		int num = cdns.Length * CDN_SPEED_TEST_COUNT;
		bool flag = false;
		if (total_download_count >= num)
		{
			Log.Info($"[cdn] 达到预期测试次数 {num}，结束测速");
			flag = true;
		}
		else if (CheckAllCdnsHaveSufficientData())
		{
			Log.Info("[cdn] 所有CDN都完成测速，结束测速");
			flag = true;
		}
		else if (CheckCompletedRounds())
		{
			int num2 = _cdnTestCounter / cdns.Length;
			Log.Info($"[cdn] 完成 {num2} 轮测试尝试，成功下载 {total_download_count} 次，基于有限数据结束测速");
			flag = true;
		}
		else if (_cdnTestCounter >= num * 2)
		{
			Log.Info($"[cdn] 尝试次数过多 {_cdnTestCounter}，强制结束测速");
			flag = true;
		}
		if (flag)
		{
			bestCdnIndex = SelectBestCdn();
		}
		return flag;
	}

	private static bool CheckAllCdnsHaveSufficientData()
	{
		int num = 0;
		foreach (KeyValuePair<int, CdnPerformance> cdnPerformance in _cdnPerformances)
		{
			if (cdnPerformance.Value.HasSufficientData)
			{
				num++;
			}
		}
		return num >= cdns.Length;
	}

	private static bool CheckCompletedRounds()
	{
		if ((double)(_cdnTestCounter / cdns.Length) >= (double)CDN_SPEED_TEST_COUNT * 1.5)
		{
			return total_download_count > cdns.Length;
		}
		return false;
	}

	private static int SelectBestCdn()
	{
		double num = -1.0;
		int num2 = 0;
		if (_cdnPerformances.Count == 0)
		{
			Log.Warning("[cdn] no data use CDN[0]");
			return 0;
		}
		for (int i = 0; i < cdns.Length; i++)
		{
			CdnPerformance value = null;
			double num3;
			if (_cdnPerformances.TryGetValue(i, out value) && value.DownloadCount > 0)
			{
				if (value.SuccessRate <= 0.0)
				{
					num3 = -2.0;
					Log.Info($"[cdn] CDN[{i}] all failed: download={value.DownloadCount}, score={num3:F2}");
				}
				else
				{
					num3 = value.CalculateScore();
					Log.Info($"[cdn] CDN[{i}]: download={value.DownloadCount} failed={value.FailureCount} avgSpd={value.AverageSpeed:F2}B/s, rate={value.SuccessRate:F2}, score={num3:F2}");
				}
			}
			else
			{
				num3 = -1.0;
				Log.Info($"[cdn] CDN[{i}] no data, score={num3:F2}");
			}
			if (num3 > num)
			{
				num = num3;
				num2 = i;
			}
		}
		Log.Info($"[cdn] select best cdn v3:[{num2}]");
		return num2;
	}
}
