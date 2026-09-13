using System;
using System.IO;
using GameFramework;
using UnityEngine;

public class CommonUtils
{
	public static string VEngine_Utility_BuildPath = "AssetBundles";

	public static string BUNDLE_OFFSET_TABLE_FILE = "BundleOffsetTable.bytes";

	public static string BUNDLE_ALIAS_OFFSET_TABLE_FILE = "AliasOffsetTable.bytes";

	public static bool IsDebug()
	{
		return false;
	}

	public static bool IsWriteLog()
	{
		return false;
	}

	public static bool CheckIsOverridePackage()
	{
		bool result = false;
		string text = "";
		text = GameEntryProxy.Sdk.Version + ";" + GameEntryProxy.Sdk.VersionCode;
		Log.Info("[OverlayInstall] cur version{0}", text);
		string path = Application.persistentDataPath + "/CheckVersion.txt";
		if (File.Exists(path))
		{
			string text2 = File.ReadAllText(path);
			Log.Info("[OverlayInstall] CheckVersion.txt : " + text2);
			if (!text2.Contains(text))
			{
				result = true;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static void DeleteCache(bool cacheBundle)
	{
		string path = Application.persistentDataPath + "/" + VEngine_Utility_BuildPath;
		if (Directory.Exists(path) && !cacheBundle)
		{
			Directory.Delete(path, recursive: true);
		}
		string path2 = Application.temporaryCachePath + "/Builtin";
		if (Directory.Exists(path2))
		{
			Directory.Delete(path2, recursive: true);
		}
		string path3 = Application.temporaryCachePath + "/Download";
		if (Directory.Exists(path3))
		{
			Directory.Delete(path3, recursive: true);
		}
		string path4 = Application.persistentDataPath + "/table";
		if (Directory.Exists(path4))
		{
			Directory.Delete(path4, recursive: true);
		}
		string path5 = Application.persistentDataPath + "/locale";
		if (Directory.Exists(path5))
		{
			Directory.Delete(path5, recursive: true);
		}
		string path6 = Application.persistentDataPath + "/warmup";
		if (Directory.Exists(path6))
		{
			Directory.Delete(path6, recursive: true);
		}
		string path7 = Application.persistentDataPath + "/lwScripts";
		if (Directory.Exists(path7))
		{
			Directory.Delete(path7, recursive: true);
		}
		string text = Application.persistentDataPath + "/Assemblies";
		if (Directory.Exists(text))
		{
			Directory.Delete(text, recursive: true);
			Log.Info("delete " + text);
		}
		string text2 = Application.persistentDataPath + "/AssetBundles";
		string text3 = text2 + "/dllres";
		string text4 = text3 + ".version";
		if (File.Exists(text3))
		{
			File.Delete(text3);
			Log.Info("delete " + text3);
		}
		if (File.Exists(text4))
		{
			File.Delete(text4);
			Log.Info("delete " + text4);
		}
		string text5 = text2 + "/gameres";
		string text6 = text5 + ".version";
		if (File.Exists(text5))
		{
			File.Delete(text5);
			Log.Info("delete " + text5);
		}
		if (File.Exists(text6))
		{
			File.Delete(text6);
			Log.Info("delete " + text6);
		}
		string text7 = Application.persistentDataPath + "/thread.lock";
		if (File.Exists(text7))
		{
			File.Delete(text7);
			Log.Info("delete " + text7);
		}
	}

	public static void DeleteOffsetFile()
	{
		string path = Path.Combine(Application.persistentDataPath, BUNDLE_OFFSET_TABLE_FILE);
		if (File.Exists(path))
		{
			File.Delete(path);
			Debug.Log("ResourceManager::Delete offset file " + BUNDLE_OFFSET_TABLE_FILE);
		}
		path = Path.Combine(Application.persistentDataPath, BUNDLE_ALIAS_OFFSET_TABLE_FILE);
		if (File.Exists(path))
		{
			File.Delete(path);
			Debug.Log("ResourceManager::Delete offset file " + BUNDLE_ALIAS_OFFSET_TABLE_FILE);
		}
	}

	public static void WriteVersion()
	{
		string path = Application.persistentDataPath + "/CheckVersion.txt";
		string text = "";
		text = GameEntryProxy.Sdk.Version + ";" + GameEntryProxy.Sdk.VersionCode;
		File.WriteAllText(path, text);
	}

	public static bool Lua_File_Read(string path, out byte[] data, out string error)
	{
		data = null;
		error = "";
		if (string.IsNullOrEmpty(path))
		{
			error = "path is null or empty";
			return false;
		}
		if (!File.Exists(path))
		{
			error = "File " + path + " does not exist";
			return false;
		}
		try
		{
			data = File.ReadAllBytes(path);
			return true;
		}
		catch (Exception ex)
		{
			error = ex.Message;
			return false;
		}
	}

	public static bool Lua_File_Write(string path, byte[] data, out string error)
	{
		error = "";
		if (string.IsNullOrEmpty(path))
		{
			error = "Path is null or empty";
			return false;
		}
		if (data == null || data.Length == 0)
		{
			error = "Data is null or empty";
			return false;
		}
		try
		{
			string directoryName = Path.GetDirectoryName(path);
			if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(data, 0, data.Length);
			}
			return true;
		}
		catch (Exception ex)
		{
			error = ex.Message;
			return false;
		}
	}

	public static bool Lua_File_Delete(string path, out string error)
	{
		error = "";
		if (string.IsNullOrEmpty(path))
		{
			error = "Path is null or empty";
			return false;
		}
		try
		{
			if (!File.Exists(path))
			{
				error = "File does not exist: " + path;
				return false;
			}
			File.Delete(path);
			return true;
		}
		catch (Exception ex)
		{
			error = "Delete failed: " + ex.Message;
			return false;
		}
	}
}
