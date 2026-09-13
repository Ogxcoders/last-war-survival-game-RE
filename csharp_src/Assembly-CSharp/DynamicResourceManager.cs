using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using UnityEngine.Networking;

public class DynamicResourceManager : MonoBehaviour
{
	public delegate void OnLoadComplete(string assetKey, UnityEngine.Object asset, object userdata, bool isLastCallback);

	private class LoadTask
	{
		private class Callback
		{
			public OnLoadComplete callback;

			public object userdata;

			public Callback(OnLoadComplete callback, object userdata)
			{
				this.callback = callback;
				this.userdata = userdata;
			}

			public void Invoke(string assetKey, UnityEngine.Object asset, bool isLastCallback)
			{
				callback?.Invoke(assetKey, asset, userdata, isLastCallback);
			}
		}

		public readonly string assetKey;

		public readonly Type assetType;

		public readonly string cdnPath;

		public readonly string cacheFolder;

		public readonly bool ignoreCache;

		private readonly List<Callback> callbacks = new List<Callback>();

		public string url
		{
			get
			{
				string text = (ignoreCache ? string.Empty : (Application.persistentDataPath + "/" + cacheFolder + "/" + assetKey));
				if (!File.Exists(text))
				{
					return cdnPath + "/" + assetKey;
				}
				return "file://" + text;
			}
		}

		public LoadTask(string assetKey, Type assetType, string cdnPath, string cacheFolder, bool ignoreCache = false)
		{
			this.assetKey = assetKey;
			this.assetType = assetType;
			this.cdnPath = cdnPath;
			this.cacheFolder = cacheFolder;
			this.ignoreCache = ignoreCache;
		}

		public virtual UnityEngine.Object GetContent(UnityWebRequest request)
		{
			throw new NotImplementedException();
		}

		public virtual void DoRequest(WebRequestManager.OnWebRequestCallback OnWebRequestDone)
		{
			throw new NotImplementedException();
		}

		public void AddCallback(OnLoadComplete callback, object userdata)
		{
			foreach (Callback callback2 in callbacks)
			{
				if (callback2.callback == callback && callback2.userdata == userdata)
				{
					return;
				}
			}
			callbacks.Add(new Callback(callback, userdata));
		}

		public void InvokeCallbacks(UnityEngine.Object asset)
		{
			foreach (Callback callback in callbacks)
			{
				callback.Invoke(assetKey, asset, callback == callbacks[callbacks.Count - 1]);
			}
		}

		public bool RemoveCallback(OnLoadComplete callback)
		{
			for (int num = callbacks.Count - 1; num >= 0; num--)
			{
				if (callbacks[num].callback == callback)
				{
					callbacks.RemoveAt(num);
				}
			}
			return callbacks.Count == 0;
		}

		public int GetCallbacksCount()
		{
			return callbacks.Count;
		}
	}

	private class Texture2DTask : LoadTask
	{
		public readonly bool nonReadable;

		public Texture2DTask(string assetKey, string cdnPath, string cacheFolder, bool nonReadable, bool ignoreCache = false)
			: base(assetKey, typeof(Texture2D), cdnPath, cacheFolder, ignoreCache)
		{
			this.nonReadable = nonReadable;
		}

		public override UnityEngine.Object GetContent(UnityWebRequest request)
		{
			return DownloadHandlerTexture.GetContent(request);
		}

		public override void DoRequest(WebRequestManager.OnWebRequestCallback OnWebRequestDone)
		{
			SingletonBehaviour<WebRequestManager>.Instance.LoadTexture(base.url, nonReadable, OnWebRequestDone, 0, 10, assetKey);
		}
	}

	private class AudioClipTask : LoadTask
	{
		public readonly AudioType audioType;

		public AudioClipTask(string assetKey, string cdnPath, string cacheFolder, AudioType audioType, bool ignoreCache = false)
			: base(assetKey, typeof(AudioClip), cdnPath, cacheFolder, ignoreCache)
		{
			this.audioType = audioType;
		}

		public override UnityEngine.Object GetContent(UnityWebRequest request)
		{
			return DownloadHandlerAudioClip.GetContent(request);
		}

		public override void DoRequest(WebRequestManager.OnWebRequestCallback OnWebRequestDone)
		{
			SingletonBehaviour<WebRequestManager>.Instance.LoadMultimedia(base.url, audioType, OnWebRequestDone, 0, 0, assetKey);
		}
	}

	private static DynamicResourceManager _instance;

	public static string downloadPathForUrl = "LocalImages";

	private readonly Dictionary<string, UnityEngine.Object> m_assetCache = new Dictionary<string, UnityEngine.Object>();

	private readonly Dictionary<string, LoadTask> m_taskDict = new Dictionary<string, LoadTask>();

	public static DynamicResourceManager Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject obj = new GameObject("(Singleton) " + typeof(DynamicResourceManager));
				_instance = obj.AddComponent<DynamicResourceManager>();
				UnityEngine.Object.DontDestroyOnLoad(obj);
			}
			return _instance;
		}
	}

	public event Action OnUpdate;

	public event Action OnLowMemory;

	private void Start()
	{
		if (_instance == this)
		{
			Application.lowMemory += OnSysLowMemory;
		}
	}

	private void OnDestroy()
	{
		if (_instance == this)
		{
			Application.lowMemory -= OnSysLowMemory;
			_instance = null;
		}
	}

	private void Update()
	{
		this.OnUpdate?.Invoke();
	}

	private void OnSysLowMemory()
	{
		this.OnLowMemory?.Invoke();
	}

	public void CreateTexture2DTask(string assetKey, OnLoadComplete callback, object userdata, string cdnPath, string cacheFolder, bool nonReadable, bool ignoreCache = false)
	{
		if (__CreateTask<Texture2D>(assetKey, callback, userdata, cdnPath, cacheFolder))
		{
			LoadTask loadTask = new Texture2DTask(assetKey, cdnPath, cacheFolder, nonReadable, ignoreCache);
			m_taskDict.Add(assetKey, loadTask);
			loadTask.AddCallback(callback, userdata);
			loadTask.DoRequest(__OnWebRequestUpdate);
		}
	}

	public void CreateAudioClipTask(string assetKey, OnLoadComplete callback, object userdata, string cdnPath, string cacheFolder, AudioType audioType, bool ignoreCache = false)
	{
		if (__CreateTask<AudioClip>(assetKey, callback, userdata, cdnPath, cacheFolder))
		{
			LoadTask loadTask = new AudioClipTask(assetKey, cdnPath, cacheFolder, audioType, ignoreCache);
			m_taskDict.Add(assetKey, loadTask);
			loadTask.AddCallback(callback, userdata);
			loadTask.DoRequest(__OnWebRequestUpdate);
		}
	}

	public bool IsTaskExist(string assetKey)
	{
		return m_taskDict.ContainsKey(assetKey);
	}

	public void ReleaseAsset(string assetKey)
	{
		if (!string.IsNullOrEmpty(assetKey) && m_assetCache.TryGetValue(assetKey, out var value))
		{
			m_assetCache.Remove(assetKey);
			UnityEngine.Object.Destroy(value);
		}
	}

	public int GetCacheCount()
	{
		return m_assetCache.Count;
	}

	public string DumpCache()
	{
		if (CommonUtils.IsDebug())
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"Count: {m_assetCache.Count}");
			stringBuilder.AppendLine("Assets: ");
			foreach (KeyValuePair<string, UnityEngine.Object> item in m_assetCache)
			{
				stringBuilder.AppendLine(item.Key);
			}
			return stringBuilder.ToString();
		}
		return "Not Supported In Release";
	}

	private bool __CreateTask<T>(string assetKey, OnLoadComplete callback, object userdata, string cdnPath, string cacheFolder) where T : UnityEngine.Object
	{
		if (string.IsNullOrEmpty(cdnPath))
		{
			Log.Error("DynamicResourceManager.CreateTask error. CdnPath is null or empty");
			return false;
		}
		if (string.IsNullOrEmpty(assetKey))
		{
			Log.Error("DynamicResourceManager.CreateTask error. CacheKey is null or empty");
			return false;
		}
		if (callback == null)
		{
			Log.Error("DynamicResourceManager.CreateTask error. Callback is null");
			return false;
		}
		if (m_assetCache.TryGetValue(assetKey, out var value))
		{
			if (value is T)
			{
				callback?.Invoke(assetKey, value, userdata, isLastCallback: true);
				return false;
			}
			Log.Error("DynamicResourceManager.CreateTask error. AssetKey conflict -> " + assetKey);
			return false;
		}
		if (m_taskDict.TryGetValue(assetKey, out var value2))
		{
			if (value2.assetType != typeof(T))
			{
				Log.Error("DynamicResourceManager.CreateTask error. AssetKey conflict -> " + assetKey);
				return false;
			}
			value2.AddCallback(callback, userdata);
			return false;
		}
		return true;
	}

	public void CancelTask(string assetKey, OnLoadComplete callback)
	{
		LoadTask value;
		if (string.IsNullOrEmpty(assetKey))
		{
			Log.Error("DynamicResourceManager.CancelTexture error. CacheKey is null or empty");
		}
		else if (callback == null)
		{
			Log.Error("DynamicResourceManager.CancelTexture error. Callback is null");
		}
		else if (m_taskDict.TryGetValue(assetKey, out value) && value.RemoveCallback(callback))
		{
			SingletonBehaviour<WebRequestManager>.Instance.Cancel(value.url);
			m_taskDict.Remove(assetKey);
		}
	}

	public void AbortTask(string assetKey, OnLoadComplete callback)
	{
		LoadTask value;
		if (string.IsNullOrEmpty(assetKey))
		{
			Log.Error("DynamicResourceManager.CancelTexture error. CacheKey is null or empty");
		}
		else if (callback == null)
		{
			Log.Error("DynamicResourceManager.CancelTexture error. Callback is null");
		}
		else if (m_taskDict.TryGetValue(assetKey, out value) && value.RemoveCallback(callback))
		{
			SingletonBehaviour<WebRequestManager>.Instance.Abort(value.url);
			m_taskDict.Remove(assetKey);
		}
	}

	private void __OnWebRequestUpdate(UnityWebRequest request, bool isErr, object userdata)
	{
		string key = userdata as string;
		if (!m_taskDict.TryGetValue(key, out var value))
		{
			return;
		}
		if (isErr)
		{
			Log.Error(request.error + ": " + request.url);
			m_taskDict.Remove(value.assetKey);
			value.InvokeCallbacks(null);
		}
		else
		{
			if (!request.isDone)
			{
				return;
			}
			if (SingletonBehaviour<WebRequestManager>.Instance.IsDownloadResult(request, "file no exists"))
			{
				Log.Error(request.url + " is not exist!");
				m_taskDict.Remove(value.assetKey);
				value.InvokeCallbacks(null);
				return;
			}
			UnityEngine.Object content = value.GetContent(request);
			if (content != null)
			{
				m_assetCache[value.assetKey] = content;
				SaveAssetAsync(request, value.assetKey, value.cacheFolder);
			}
			else
			{
				Log.Error(request.url + " content is invalid!");
			}
			m_taskDict.Remove(value.assetKey);
			value.InvokeCallbacks(content);
		}
	}

	private static async void SaveAssetAsync(UnityWebRequest request, string assetKey, string cacheFolder)
	{
		if (request == null || string.IsNullOrEmpty(assetKey) || string.IsNullOrEmpty(cacheFolder) || request.url.StartsWith("file://", StringComparison.Ordinal))
		{
			return;
		}
		string tempPath = Path.Combine(Application.temporaryCachePath, cacheFolder, assetKey);
		if (File.Exists(tempPath))
		{
			return;
		}
		try
		{
			string directoryName = Path.GetDirectoryName(tempPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
			{
				byte[] data = request.downloadHandler.data;
				await fs.WriteAsync(data, 0, data.Length);
			}
			string saveAssetPath = GetSaveAssetPath(cacheFolder, assetKey);
			if (File.Exists(saveAssetPath))
			{
				File.Delete(saveAssetPath);
			}
			string directoryName2 = Path.GetDirectoryName(saveAssetPath);
			if (!Directory.Exists(directoryName2))
			{
				Directory.CreateDirectory(directoryName2);
			}
			File.Move(tempPath, saveAssetPath);
			if (cacheFolder == UIChatSendPhoto.chatPhotoFloderName)
			{
				double num = Math.Round((double)new FileInfo(saveAssetPath).Length / 1024.0, 2);
				PostEventLog.TrackMap("DOWNLOAD_PAGE_SUCCESS", new Dictionary<string, object> { { "photoSize", num } });
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message);
		}
		finally
		{
			if (File.Exists(tempPath))
			{
				File.Delete(tempPath);
			}
		}
	}

	public static string GetSaveAssetPath(string cacheFolder, string assetKey)
	{
		return Path.Combine(Application.persistentDataPath, cacheFolder, assetKey);
	}

	public string GetBigPhotoPathBySmall(string filePathSmall)
	{
		if (string.IsNullOrEmpty(filePathSmall))
		{
			return "";
		}
		string extension = Path.GetExtension(Path.GetFileName(filePathSmall));
		string result = "";
		int num = filePathSmall.LastIndexOf('/');
		if (num != -1)
		{
			result = filePathSmall.Substring(0, num + 1) + Path.GetFileNameWithoutExtension(filePathSmall) + "_big" + extension;
		}
		return result;
	}

	public void DeleteCacheCompressedPhoto(string filePathSmall)
	{
		if (!string.IsNullOrEmpty(filePathSmall))
		{
			DeleteFileByPath(filePathSmall);
			string bigPhotoPathBySmall = GetBigPhotoPathBySmall(filePathSmall);
			DeleteFileByPath(bigPhotoPathBySmall);
		}
	}

	public void DeleteFileByPath(string filePath)
	{
		if (!Directory.Exists(Path.GetDirectoryName(filePath)) || !File.Exists(filePath))
		{
			return;
		}
		try
		{
			File.Delete(filePath);
		}
		catch (Exception ex)
		{
			Log.Error("#UploadChatPhoto#  删除文件时出错: " + ex.Message);
		}
	}

	public void DeleteCacheCompressedPhotoFolder()
	{
		DateTime now = DateTime.Now;
		string text = now.Year + "-" + now.Month + "-" + now.Day;
		string publicString = GameEntry.Setting.GetPublicString("ClearChatPhotoFolderFlag", "");
		if (text == publicString)
		{
			return;
		}
		GameEntry.Setting.SetPublicString("ClearChatPhotoFolderFlag", text);
		string path = Application.persistentDataPath + "/" + UIChatSendPhoto.chatPhotoFloderName;
		if (Directory.Exists(path))
		{
			try
			{
				Directory.Delete(path, recursive: true);
			}
			catch (Exception ex)
			{
				Log.Error("#UploadChatPhoto#  删除文件夹时出错: " + ex.Message);
			}
		}
		path = Application.persistentDataPath + "/" + UIChatSendPhoto.momentPhotoFloderName;
		if (!Directory.Exists(path))
		{
			return;
		}
		try
		{
			Directory.Delete(path, recursive: true);
		}
		catch (Exception ex2)
		{
			Log.Error("#UploadMomentPhoto#  删除文件夹时出错: " + ex2.Message);
		}
	}

	public void MoveCompressedPhotoToTargetPath(string filePathSmall, string assetKey, string assetKeyBig)
	{
		if (!string.IsNullOrEmpty(filePathSmall) && !string.IsNullOrEmpty(assetKey) && !string.IsNullOrEmpty(assetKeyBig))
		{
			string saveAssetPath = GetSaveAssetPath(UIChatSendPhoto.momentPhotoFloderName, assetKey);
			MovePhotoToTargetPath(filePathSmall, saveAssetPath);
			saveAssetPath = GetSaveAssetPath(UIChatSendPhoto.momentPhotoFloderName, assetKeyBig);
			string bigPhotoPathBySmall = GetBigPhotoPathBySmall(filePathSmall);
			MovePhotoToTargetPath(bigPhotoPathBySmall, saveAssetPath);
		}
	}

	public void MovePhotoToTargetPath(string sourcePath, string destinationPath)
	{
		if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(destinationPath) || !File.Exists(sourcePath))
		{
			return;
		}
		try
		{
			string directoryName = Path.GetDirectoryName(destinationPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (File.Exists(destinationPath))
			{
				File.Delete(destinationPath);
			}
			File.Move(sourcePath, destinationPath);
		}
		catch (Exception ex)
		{
			Log.Error("#UploadChatPhoto#  压缩照片挪动文件时出错: " + ex.Message);
		}
	}

	public void CopyCompressedPhotoToTargetPath(string filePathSmall, string assetKey, string assetKeyBig)
	{
		if (!string.IsNullOrEmpty(filePathSmall) && !string.IsNullOrEmpty(assetKey) && !string.IsNullOrEmpty(assetKeyBig))
		{
			string saveAssetPath = GetSaveAssetPath(UIChatSendPhoto.momentPhotoFloderName, assetKey);
			CopyPhotoToTargetPath(filePathSmall, saveAssetPath);
			saveAssetPath = GetSaveAssetPath(UIChatSendPhoto.momentPhotoFloderName, assetKeyBig);
			string bigPhotoPathBySmall = GetBigPhotoPathBySmall(filePathSmall);
			CopyPhotoToTargetPath(bigPhotoPathBySmall, saveAssetPath);
		}
	}

	public void CopyPhotoToTargetPath(string sourcePath, string destinationPath)
	{
		if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(destinationPath) || !File.Exists(sourcePath))
		{
			return;
		}
		try
		{
			string directoryName = Path.GetDirectoryName(destinationPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (File.Exists(destinationPath))
			{
				File.Delete(destinationPath);
			}
			File.Copy(sourcePath, destinationPath);
		}
		catch (Exception ex)
		{
			Log.Error("#UploadChatPhoto#  压缩照片挪动文件时出错: " + ex.Message);
		}
	}
}
