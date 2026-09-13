using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using UnityEngine.Networking;

public class PVELogManager
{
	private struct UploadParams
	{
		public Action<bool> callback;

		public WWWForm form;

		public int retryCount;

		public int startLineIndex;
	}

	private struct DownloadParams
	{
		public Action<bool> callback;

		public string filePath;

		public string subPath;

		public int retryCount;

		public int startLineIndex;
	}

	private enum PVELogFuncType
	{
		Surfing = 1,
		GhostParkour
	}

	[Serializable]
	private class Resp
	{
		public bool status;

		public int code;
	}

	private static PVELogManager _instance;

	private static DateTime _1970_01_01 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private static readonly string[] _uploadURLs = new string[2] { "https://lastwar-upload-surfing-aws.lastwargame.com/upload_surfing_oss.php", "https://lastwar-upload-surfing-gcp.lastwargame.com/upload_surfing_oss.php" };

	private static readonly string[] _downloadURLs = new string[2] { "https://lastwar-surfing.akamaized.net", "https://lastwar-surfing.oss-accelerate.aliyuncs.com" };

	private static readonly int _uploadURLCount = _uploadURLs.Length;

	private static readonly int _downloadURLCount = _downloadURLs.Length;

	private static int _lastUploadSucceedLineIndex;

	private static readonly Dictionary<UnityWebRequest, UploadParams> _uploadParamsMap = new Dictionary<UnityWebRequest, UploadParams>();

	private static int _lastDownloadSucceedLineIndex;

	private static readonly Dictionary<UnityWebRequest, DownloadParams> _downloadParamsMap = new Dictionary<UnityWebRequest, DownloadParams>();

	public static PVELogManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PVELogManager();
			}
			return _instance;
		}
	}

	public void UploadPVESurfingLog(string uid, string uuid, byte[] data, Action<bool> callback)
	{
		if (string.IsNullOrEmpty(uid))
		{
			callback?.Invoke(obj: false);
			Log.Info($"PVELogUploadManager UploadPVESurfingLog Request. uid is nil. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
			return;
		}
		if (string.IsNullOrEmpty(uuid))
		{
			callback?.Invoke(obj: false);
			Log.Info($"PVELogUploadManager UploadPVESurfingLog Request. uuid is nil. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
			return;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("uid", uid);
		wWWForm.AddField("uuid", uuid);
		wWWForm.AddField("funcType", 1);
		string md5Hash = AESHelper.GetMd5Hash(AESHelper.GetMd5Hash($"{uid}_{uuid}_{1}"));
		wWWForm.AddField("sign", md5Hash);
		wWWForm.AddBinaryData("file", data);
		int num = 0;
		int lastUploadSucceedLineIndex = _lastUploadSucceedLineIndex;
		string uri = _uploadURLs[lastUploadSucceedLineIndex];
		UnityWebRequest key = SingletonBehaviour<WebRequestManager>.Instance.Post(uri, wWWForm, OnWebRequestCallback, 0, 30, uuid);
		UploadParams value = new UploadParams
		{
			callback = callback,
			form = wWWForm,
			retryCount = num,
			startLineIndex = lastUploadSucceedLineIndex
		};
		_uploadParamsMap[key] = value;
		Log.Info($"PVELogUploadManager UploadPVESurfingLog Request. uid : {uid}. uuid : {uuid}. retryCount : {num}. startLineIndex : {lastUploadSucceedLineIndex}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
	}

	private static void OnWebRequestCallback(UnityWebRequest request, bool err, object userdata)
	{
		if (!request.isDone)
		{
			return;
		}
		bool num = err || request.responseCode < 200 || request.responseCode >= 300;
		string text = request.downloadHandler.text;
		bool flag = false;
		if (!num && !string.IsNullOrEmpty(text))
		{
			flag = JsonUtility.FromJson<Resp>(text).code == 0;
		}
		string text2 = (userdata as string) ?? "";
		UploadParams value2;
		if (num)
		{
			if (_uploadParamsMap.TryGetValue(request, out var value))
			{
				int num2 = value.retryCount + 1;
				if (num2 >= _uploadURLCount)
				{
					Action<bool> callback = value.callback;
					ClearRequest(request);
					callback?.Invoke(obj: false);
					Log.Info($"PVELogUploadManager UploadPVESurfingLog Callback Error. uuid : {text2}. data : {text}. response : {request.responseCode}. error : {request.error}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
				}
				else
				{
					WWWForm form = value.form;
					int num3 = (value.startLineIndex + num2) % _uploadURLCount;
					string uri = _uploadURLs[num3];
					UnityWebRequest key = SingletonBehaviour<WebRequestManager>.Instance.Post(uri, form, OnWebRequestCallback, 0, 30, text2);
					value.retryCount = num2;
					_uploadParamsMap[key] = value;
					Log.Info($"PVELogUploadManager UploadPVESurfingLog Request Retry. uuid : {text2}. retryCount : {num2}. lineIndex : {num3}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
					ClearRequest(request);
				}
			}
			else
			{
				ClearRequest(request);
				Log.Error($"PVELogUploadManager UploadPVESurfingLog Invalid Error. uuid : {text2}. data : {text}. response : {request.responseCode}. error : {request.error}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
			}
		}
		else if (_uploadParamsMap.TryGetValue(request, out value2))
		{
			_lastUploadSucceedLineIndex = (value2.startLineIndex + value2.retryCount) % _uploadURLCount;
			Action<bool> callback2 = value2.callback;
			ClearRequest(request);
			callback2?.Invoke(flag);
			Log.Info($"PVELogUploadManager UploadPVESurfingLog Callback Success : {flag}. retryCount : {value2.retryCount}. uuid : {text2}. response : {request.responseCode}. error : {request.error}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
		}
		else
		{
			ClearRequest(request);
			Log.Error($"PVELogUploadManager UploadPVESurfingLog Invalid Error. uuid : {text2}. data : {text}. response : {request.responseCode}. error : {request.error}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
		}
	}

	private static void ClearRequest(UnityWebRequest request)
	{
		_uploadParamsMap.Remove(request);
	}

	public void UploadPVESurfingLogFile(string uid, string uuid, string filePath, Action<bool> callback, int funcType = 1, string beginTime = "")
	{
		if (string.IsNullOrEmpty(uid))
		{
			callback?.Invoke(obj: false);
			Log.Info($"PVELogUploadManager UploadPVESurfingLog Request. uid is nil. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
			return;
		}
		if (string.IsNullOrEmpty(uuid))
		{
			callback?.Invoke(obj: false);
			Log.Info($"PVELogUploadManager UploadPVESurfingLog Request. uuid is nil. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
			return;
		}
		if (!File.Exists(filePath))
		{
			Log.Error($"PVELogUploadManager UploadPVESurfingLog Error. file : {filePath} does not exist. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
			return;
		}
		byte[] contents = File.ReadAllBytes(filePath);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("uid", uid);
		wWWForm.AddField("uuid", uuid);
		wWWForm.AddField("funcType", funcType);
		string md5Hash = AESHelper.GetMd5Hash(AESHelper.GetMd5Hash($"{uid}_{uuid}_{funcType}"));
		wWWForm.AddField("sign", md5Hash);
		wWWForm.AddBinaryData("file", contents);
		if (funcType == 2)
		{
			wWWForm.AddField("beginTime", beginTime);
		}
		int num = 0;
		int lastUploadSucceedLineIndex = _lastUploadSucceedLineIndex;
		string uri = _uploadURLs[lastUploadSucceedLineIndex];
		UnityWebRequest key = SingletonBehaviour<WebRequestManager>.Instance.Post(uri, wWWForm, OnWebRequestCallback, 0, 30, uuid);
		UploadParams value = new UploadParams
		{
			callback = callback,
			form = wWWForm,
			retryCount = num,
			startLineIndex = lastUploadSucceedLineIndex
		};
		_uploadParamsMap[key] = value;
		Log.Info($"PVELogUploadManager UploadPVESurfingLog Request. uid : {uid}. uuid : {uuid}. retryCount : {num}. startLineIndex : {lastUploadSucceedLineIndex}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
	}

	public void UploadPVELog(string uid, string uuid)
	{
		string text = "C:\\Users\\admin\\AppData\\LocalLow\\LCYD\\Last War\\logs\\1285506317884108756";
		if (!File.Exists(text))
		{
			Log.Error($"PVELogUploadManager UploadPVELog Error. file : {text} does not exist. ms : {DateTime.Now.Ticks / 10000}");
			return;
		}
		byte[] contents = File.ReadAllBytes(text);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("uid", uid);
		wWWForm.AddField("uuid", uuid);
		wWWForm.AddField("funcType", "2");
		string md5Hash = AESHelper.GetMd5Hash(AESHelper.GetMd5Hash(uid + "_" + uuid + "_2"));
		wWWForm.AddField("sign", md5Hash);
		wWWForm.AddBinaryData("file", contents);
		int num = 0;
		int lastUploadSucceedLineIndex = _lastUploadSucceedLineIndex;
		string uri = _uploadURLs[lastUploadSucceedLineIndex];
		UnityWebRequest key = SingletonBehaviour<WebRequestManager>.Instance.Post(uri, wWWForm, OnWebRequestCallback, 0, 30, uuid);
		UploadParams value = new UploadParams
		{
			callback = null,
			form = wWWForm,
			retryCount = num,
			startLineIndex = lastUploadSucceedLineIndex
		};
		_uploadParamsMap[key] = value;
		Log.Info($"PVELogUploadManager UploadPVELog Request. file : {text}. retryCount : {num}. startLineIndex : {lastUploadSucceedLineIndex}. ms : {DateTime.Now.Ticks / 10000}");
	}

	public void DownloadPVELog(string uid, string uuid)
	{
		string text = uid.Substring(uid.Length - 6, 6);
		string text2 = "surfingLog/2/" + text + "/" + uuid;
		string text3 = "D:\\ossTest\\testSurfingLog6";
		string directoryName = Path.GetDirectoryName(text3);
		if (!string.IsNullOrEmpty(directoryName))
		{
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			int num = 0;
			int lastDownloadSucceedLineIndex = _lastDownloadSucceedLineIndex;
			string uri = _downloadURLs[lastDownloadSucceedLineIndex] + "/" + text2;
			UnityWebRequest key = SingletonBehaviour<WebRequestManager>.Instance.DownFile(uri, text3, OnDownloadCallback, 0, 15, uuid);
			DownloadParams value = new DownloadParams
			{
				callback = null,
				filePath = text3,
				retryCount = num,
				startLineIndex = lastDownloadSucceedLineIndex,
				subPath = text2
			};
			_downloadParamsMap[key] = value;
			Log.Info($"PVELogUploadManager DownloadPVELog Request. file : {text3}. retryCount : {num}. startLineIndex : {lastDownloadSucceedLineIndex}. ms : {DateTime.Now.Ticks / 10000}");
		}
	}

	public void DownloadPVESurfingLog(string uid, string uuid, string downloadFilePath, Action<bool> callback, int funcType = 1)
	{
		if (string.IsNullOrEmpty(uid))
		{
			callback?.Invoke(obj: false);
			Log.Info($"PVELogUploadManager DownloadPVESurfingLog Request. uid is nil. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
			return;
		}
		if (string.IsNullOrEmpty(uuid))
		{
			callback?.Invoke(obj: false);
			Log.Info($"PVELogUploadManager DownloadPVESurfingLog Request. uuid is nil. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
			return;
		}
		if (string.IsNullOrEmpty(downloadFilePath))
		{
			callback?.Invoke(obj: false);
			Log.Info($"PVELogUploadManager DownloadPVESurfingLog Request. uid : {uid}. uuid : {uuid}. downloadFilePath is nil. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
			return;
		}
		string directoryName = Path.GetDirectoryName(downloadFilePath);
		if (string.IsNullOrEmpty(directoryName))
		{
			callback?.Invoke(obj: false);
			return;
		}
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		string arg = uid.Substring(uid.Length - 6, 6);
		string text = $"surfingLog/{funcType}/{arg}/{uuid}";
		int num = 0;
		int lastDownloadSucceedLineIndex = _lastDownloadSucceedLineIndex;
		string uri = _downloadURLs[lastDownloadSucceedLineIndex] + "/" + text;
		UnityWebRequest key = SingletonBehaviour<WebRequestManager>.Instance.DownFile(uri, downloadFilePath, OnDownloadCallback, 0, 15, uuid);
		DownloadParams value = new DownloadParams
		{
			callback = callback,
			filePath = downloadFilePath,
			retryCount = num,
			startLineIndex = lastDownloadSucceedLineIndex,
			subPath = text
		};
		_downloadParamsMap[key] = value;
		Log.Info($"PVELogUploadManager DownloadPVESurfingLog Request. uid : {uid}. uuid : {uuid} file : {downloadFilePath}. retryCount : {num}. startLineIndex : {lastDownloadSucceedLineIndex}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
	}

	private static void OnDownloadCallback(UnityWebRequest request, bool err, object userdata)
	{
		if (!request.isDone)
		{
			return;
		}
		string text = (userdata as string) ?? "";
		DownloadParams value2;
		if (err)
		{
			if (_downloadParamsMap.TryGetValue(request, out var value))
			{
				int num = value.retryCount + 1;
				if (num >= _downloadURLCount)
				{
					Action<bool> callback = value.callback;
					ClearDownload(request);
					request.Dispose();
					TryDeleteInvalidFile(value.filePath);
					callback?.Invoke(obj: false);
					Log.Info($"PVELogUploadManager DownloadPVESurfingLog Callback Error. uuid : {text}. response : {request.responseCode}. error : {request.error}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
				}
				else
				{
					request.Dispose();
					TryDeleteInvalidFile(value.filePath);
					int num2 = (value.startLineIndex + num) % _downloadURLCount;
					string uri = _downloadURLs[num2] + "/" + value.subPath;
					UnityWebRequest key = SingletonBehaviour<WebRequestManager>.Instance.DownFile(uri, value.filePath, OnDownloadCallback, 0, 15, text);
					value.retryCount = num;
					_downloadParamsMap[key] = value;
					Log.Info($"PVELogUploadManager DownloadPVESurfingLog Request Retry. retryCount : {num}. lineIndex : {num2}. uuid : {text} file : {value.filePath}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
					ClearDownload(request);
				}
			}
			else
			{
				ClearDownload(request);
				Log.Info($"PVELogUploadManager DownloadPVESurfingLog Invalid Error. uuid : {text}. response : {request.responseCode}. error : {request.error}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
			}
		}
		else if (_downloadParamsMap.TryGetValue(request, out value2))
		{
			_lastDownloadSucceedLineIndex = (value2.startLineIndex + value2.retryCount) % _downloadURLCount;
			Action<bool> callback2 = value2.callback;
			ClearDownload(request);
			callback2?.Invoke(obj: true);
			Log.Info($"PVELogUploadManager DownloadPVELog PVELogDownloadCallback Success. uuid : {text}. retryCount : {value2.retryCount}. response : {request.responseCode}. error : {request.error}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
		}
		else
		{
			ClearDownload(request);
			Log.Info($"PVELogUploadManager DownloadPVESurfingLog Invalid Error. uuid : {text}. response : {request.responseCode}. error : {request.error}. ms : {(DateTime.Now.Ticks - _1970_01_01.Ticks) / 10000}");
		}
	}

	private static void TryDeleteInvalidFile(string filePath)
	{
		try
		{
			if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}
		catch (Exception ex)
		{
			Log.Error("PVELogManager.TryDeleteInvalidFile error. filePath : " + filePath + ". msg : " + ex.Message);
		}
	}

	private static void ClearDownload(UnityWebRequest request)
	{
		_downloadParamsMap.Remove(request);
	}

	public void ClearAll()
	{
		_uploadParamsMap.Clear();
		_downloadParamsMap.Clear();
	}
}
