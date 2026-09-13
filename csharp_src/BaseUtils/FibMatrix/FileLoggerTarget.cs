using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace FibMatrix;

public class FileLoggerTarget : IRemoteLoggerTarget, ILoggerTarget
{
	public static RemoteLoggerTarget.ReportLevel reportLevel = RemoteLoggerTarget.ReportLevel.Info;

	private JsonSerializerSettings _settings = new JsonSerializerSettings();

	private FileStream _logFileStream;

	private StreamWriter _streamWriter;

	private static string FileLogsFolder = Application.persistentDataPath + "/FileLogs";

	private static List<UploadRequest> requests = new List<UploadRequest>();

	private static string uploadKey = "101EDB40E1005FD7AC288A2385D658EF";

	public static FileLoggerTarget instance { get; private set; }

	public LogTargetType targetType => LogTargetType.Network;

	public static bool uploading => requests.Count > 0;

	public static FileLoggerTarget InitAndUpload(string uid, string guid, Func<string, string> hashFunc)
	{
		if (instance != null)
		{
			return instance;
		}
		if (!Directory.Exists(FileLogsFolder))
		{
			Directory.CreateDirectory(FileLogsFolder);
		}
		string[] files = Directory.GetFiles(FileLogsFolder, "*.*", SearchOption.AllDirectories);
		foreach (string text in files)
		{
			byte[] contents = File.ReadAllBytes(text);
			string fileName = Path.GetFileName(text);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			string value = hashFunc(uid + "_" + fileNameWithoutExtension + "_" + uploadKey);
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("uid", uid);
			wWWForm.AddField("uuid", fileNameWithoutExtension);
			wWWForm.AddField("sign", value);
			wWWForm.AddBinaryData("file", contents, fileName, "text/plain");
			UnityEngine.Debug.LogWarning("[FileLoggerTarget] Uploading file " + text + ", " + fileName);
			UnityWebRequest unityWebRequest = UnityWebRequest.Post("https://lastwar-upload-surfing-aws.lastwargame.com/upload_client_log.php", wWWForm);
			requests.Add(new UploadRequest
			{
				webRequest = unityWebRequest,
				logPath = text
			});
			unityWebRequest.SendWebRequest();
		}
		instance = new FileLoggerTarget(uid, guid);
		return instance;
	}

	public static void UpdateUploadRequests()
	{
		for (int num = requests.Count - 1; num >= 0; num--)
		{
			UnityWebRequest webRequest = requests[num].webRequest;
			string logPath = requests[num].logPath;
			if (webRequest.isDone)
			{
				if (webRequest.isHttpError || webRequest.isNetworkError)
				{
					UnityEngine.Debug.LogError("[FileLoggerTarget] Uploading delete file " + logPath + ", error: " + webRequest.error);
				}
				else
				{
					UnityEngine.Debug.LogWarning("[FileLoggerTarget] Uploading delete file " + logPath);
					File.Delete(logPath);
				}
				requests.RemoveAt(num);
			}
		}
	}

	private FileLoggerTarget(string uid, string guid)
	{
		_logFileStream = new FileStream(FileLogsFolder + "/" + guid + ".log", FileMode.OpenOrCreate, FileAccess.Write);
		_streamWriter = new StreamWriter(_logFileStream);
		_streamWriter.WriteLine("uid: " + uid);
		_settings.NullValueHandling = NullValueHandling.Ignore;
	}

	public void Debug(string message, object context, LogImportance importance)
	{
	}

	public void Info(string message, object context, LogImportance importance)
	{
		if (reportLevel <= RemoteLoggerTarget.ReportLevel.Info)
		{
			Dictionary<string, string> args = context as Dictionary<string, string>;
			PushQueueHttps(LogLevel.Info, args, importance == LogImportance.Normal, message ?? "EMPTY");
		}
	}

	public void Warning(string message, object context, LogImportance importance)
	{
		if (reportLevel <= RemoteLoggerTarget.ReportLevel.Warning)
		{
			Dictionary<string, string> args = context as Dictionary<string, string>;
			PushQueueHttps(LogLevel.Warning, args, importance == LogImportance.Normal, message ?? "EMPTY");
		}
	}

	public void Error(string message, string stackTrace, object context, LogImportance importance)
	{
		if (reportLevel <= RemoteLoggerTarget.ReportLevel.Error)
		{
			Dictionary<string, string> args = context as Dictionary<string, string>;
			PushQueueHttps(LogLevel.Error, args, batch: false, message ?? "EMPTY");
		}
	}

	public void Update()
	{
	}

	public void UpdateUserInfo(string userId, string serverId)
	{
	}

	public void UpdateResVersion(string resVersion)
	{
	}

	public void UpdateCountry(string country)
	{
	}

	public void Dispose()
	{
		_streamWriter?.Flush();
		_streamWriter?.Close();
		_logFileStream?.Close();
		_streamWriter = null;
		_logFileStream = null;
	}

	private void PushQueueHttps(LogLevel level, Dictionary<string, string> args, bool batch, string message = null)
	{
		if (_logFileStream != null && _logFileStream.CanWrite)
		{
			string arg = JsonConvert.SerializeObject(args, _settings);
			_streamWriter.WriteLine($"[{level}] resp: {message}, args: {arg}");
			_streamWriter.Flush();
		}
	}
}
