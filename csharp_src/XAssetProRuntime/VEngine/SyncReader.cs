using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using UnityEngine.Networking;

namespace VEngine;

internal class SyncReader
{
	private static bool initd = false;

	private static string readApkPath = "";

	private static ZipFile _zipFileCache;

	private static FileStream _apkStreamCache;

	public const bool readFileFallback = true;

	private static Dictionary<string, Stream> _steamCache = new Dictionary<string, Stream>();

	private static Dictionary<string, ZipEntry> _entryCache = new Dictionary<string, ZipEntry>();

	public static bool CheckAndInit()
	{
		bool num = Application.platform == RuntimePlatform.Android;
		bool flag = false;
		if (!num)
		{
			flag = true;
			initd = true;
		}
		else
		{
			try
			{
				string[] files = Directory.GetFiles(Path.GetDirectoryName(Application.dataPath), "*.apk", SearchOption.TopDirectoryOnly);
				string[] array = files;
				foreach (string text in array)
				{
					Log.Info("found apkName " + text);
				}
				if (files.Length >= 1)
				{
					if (files.Length == 1)
					{
						if (Path.GetFileName(files[0]) == "base.apk")
						{
							readApkPath = files[0];
						}
					}
					else if (files.Length > 1)
					{
						array = files;
						foreach (string path in array)
						{
							if (Path.GetFileName(path).Contains("install_time_pack"))
							{
								readApkPath = path;
								break;
							}
						}
					}
				}
				if (!string.IsNullOrEmpty(readApkPath))
				{
					Init();
					ZipFile zipFile = GetZipFile();
					string playerDataPath = Versions.GetPlayerDataPath(CommonUtils.BUNDLE_OFFSET_TABLE_FILE);
					string name = "assets" + playerDataPath.Substring(Application.streamingAssetsPath.Length);
					ZipEntry entry = zipFile.GetEntry(name);
					Log.Info($"Check offsetTable exist {entry != null}");
					if (entry != null)
					{
						flag = true;
					}
				}
				Log.Info($"Use read apk path: {readApkPath}, isOk: {flag}");
			}
			catch (Exception arg)
			{
				flag = false;
				Log.Info($"CheckAndInitError {arg}");
			}
		}
		Log.Info($"Check and init SyncReader is ok {flag}");
		return flag;
	}

	public static byte[] ReadAllBytesByAssetPath(string assetPath)
	{
		if (!Versions.GetDependencies(assetPath, out var bundle, out var _))
		{
			Debug.LogError("raw file dependency not found: " + assetPath);
			return null;
		}
		LocalBundle.Mapper.GetFileInfo(bundle.name, out var mappingRelativePath, out var offset);
		bool flag = Application.platform == RuntimePlatform.Android;
		string text = "";
		bool cacheStream = false;
		if (offset >= 0)
		{
			cacheStream = true;
			text = ((!flag) ? mappingRelativePath : ("assets" + mappingRelativePath.Replace(Application.streamingAssetsPath, "")));
		}
		else
		{
			text = Versions.GetBundlePathOrURL(bundle);
		}
		byte[] array = ReadAllBytesByFilePath(text, offset, (int)bundle.size, flag, cacheStream);
		if (array == null)
		{
			Debug.LogError("read bytes failed, " + assetPath);
		}
		return array;
	}

	private static void Init()
	{
		if (!initd)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				_apkStreamCache = new FileStream(readApkPath, FileMode.Open, FileAccess.Read);
				_zipFileCache = new ZipFile(_apkStreamCache);
			}
			initd = true;
		}
	}

	private static void Dispose()
	{
		_zipFileCache?.Close();
		_zipFileCache = null;
		_apkStreamCache?.Dispose();
		_apkStreamCache = null;
		initd = false;
	}

	private static ZipFile GetZipFile()
	{
		return _zipFileCache;
	}

	public static bool ExistFile(string filePath)
	{
		if (filePath.StartsWith("jar:file://"))
		{
			filePath = "assets" + filePath.Substring(Application.streamingAssetsPath.Length);
			if (_entryCache.ContainsKey(filePath))
			{
				return true;
			}
			ZipEntry entry = GetZipFile().GetEntry(filePath);
			if (entry != null)
			{
				_entryCache.Add(filePath, entry);
				return true;
			}
			Debug.LogError("entry not found:" + filePath);
			return false;
		}
		return File.Exists(filePath);
	}

	public static byte[] ReadAllBytesByFilePath(string filePath)
	{
		int offset = -1;
		bool readZip = false;
		if (filePath.StartsWith("jar:file://"))
		{
			offset = 0;
			readZip = true;
			filePath = "assets" + filePath.Substring(Application.streamingAssetsPath.Length);
		}
		return ReadAllBytesByFilePath(filePath, offset, -1, readZip);
	}

	public static string ReadAllTextByFilePath(string filePath)
	{
		byte[] array = ReadAllBytesByFilePath(filePath);
		if (array == null)
		{
			return null;
		}
		return Encoding.UTF8.GetString(array);
	}

	public static byte[] ReadAllBytesByFilePath(string filePath, int offset, int length, bool readZip, bool cacheStream = false)
	{
		byte[] array = null;
		if (offset < 0)
		{
			array = File.ReadAllBytes(filePath);
		}
		else
		{
			Stream value = null;
			if (!_steamCache.TryGetValue(filePath, out value))
			{
				if (readZip)
				{
					ZipFile zipFile = GetZipFile();
					ZipEntry entry = zipFile.GetEntry(filePath);
					if (entry != null)
					{
						if (length < 0)
						{
							length = (int)entry.Size;
						}
						if (length != -1)
						{
							value = zipFile.GetInputStream(entry.ZipFileIndex);
							if (!value.CanSeek)
							{
								cacheStream = false;
							}
							array = ReadStreamOffset(value, offset, length);
						}
						else
						{
							Debug.LogError("entry unknown size:" + filePath);
						}
					}
					else
					{
						Debug.LogError("entry not found:" + filePath);
					}
					if (!cacheStream)
					{
						value?.Dispose();
					}
				}
				else
				{
					value = new FileStream(filePath, FileMode.Open, FileAccess.Read);
					array = ReadStreamOffset(value, offset, length);
					if (!cacheStream)
					{
						value.Dispose();
					}
				}
				if (cacheStream && value != null)
				{
					_steamCache.Add(filePath, value);
				}
			}
			else
			{
				array = ReadStreamOffset(value, offset, length);
			}
		}
		if (array == null && offset == 0)
		{
			_ = Application.streamingAssetsPath + "/" + filePath;
			Log.Error("read failed, fallback to unitywebrequest:" + filePath);
			UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = UnityWebRequest.Get(filePath).SendWebRequest();
			while (!unityWebRequestAsyncOperation.isDone)
			{
			}
			if (unityWebRequestAsyncOperation.webRequest.isNetworkError || unityWebRequestAsyncOperation.webRequest.isHttpError)
			{
				Debug.LogError("fallback failed:" + filePath + ", error: " + unityWebRequestAsyncOperation.webRequest.error);
			}
			else
			{
				array = unityWebRequestAsyncOperation.webRequest.downloadHandler.data;
			}
		}
		return array;
	}

	private static byte[] ReadStreamOffset(Stream stream, int offset, int length)
	{
		if (stream.CanSeek)
		{
			stream.Seek(offset, SeekOrigin.Begin);
		}
		byte[] array = new byte[length];
		stream.Read(array, 0, length);
		return array;
	}
}
