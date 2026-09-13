using System;
using System.IO;
using GameFramework;
using UnityEngine;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public static class FileUtils
{
	public static bool ExistDirectory(string path)
	{
		return Directory.Exists(path);
	}

	public static string GetScript(string protoPath)
	{
		return "";
	}

	public static bool ExistFile(string path)
	{
		return File.Exists(path);
	}

	public static void DeleteDirectoryIfExists(string path, bool recursive = true)
	{
		if (Directory.Exists(path))
		{
			Directory.Delete(path, recursive);
		}
	}

	public static void DeleteFileIfExists(string path)
	{
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static void CreateFileDirectoryIfNotExists(string path)
	{
		string directoryName = Path.GetDirectoryName(path);
		if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
	}

	public static FileStream CreateFile(string path)
	{
		DeleteFileIfExists(path);
		CreateFileDirectoryIfNotExists(path);
		return File.Create(path);
	}

	public static StreamWriter CreateText(string path)
	{
		DeleteFileIfExists(path);
		CreateFileDirectoryIfNotExists(path);
		return File.CreateText(path);
	}

	public static void WriteFile(string path, byte[] bytedata, bool overwrite = true)
	{
		CreateFileDirectoryIfNotExists(path);
		if (overwrite && File.Exists(path))
		{
			File.Delete(path);
		}
		File.WriteAllBytes(path, bytedata);
	}

	public static void WriteFile(string path, string content)
	{
		try
		{
			CreateFileDirectoryIfNotExists(path);
			File.WriteAllText(path, content);
		}
		catch (Exception ex)
		{
			Log.Error("Write file to " + path + " failed. E:" + ex.Message);
		}
	}

	public static void CopyFile(string srcPath, string dstPath, bool overwrite = true)
	{
		if (File.Exists(srcPath))
		{
			CreateFileDirectoryIfNotExists(dstPath);
			File.Copy(srcPath, dstPath, overwrite);
		}
		else
		{
			Debug.LogErrorFormat("File not exsits: {0}", srcPath);
		}
	}

	public static long GetFileSize(string path)
	{
		using FileStream fileStream = File.OpenRead(path);
		return fileStream.Length;
	}

	public static void CopyFilesRecursively(string sourcePath, string targetPath)
	{
		string[] directories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
		for (int i = 0; i < directories.Length; i++)
		{
			Directory.CreateDirectory(directories[i].Replace(sourcePath, targetPath));
		}
		directories = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
		foreach (string obj in directories)
		{
			File.Copy(obj, obj.Replace(sourcePath, targetPath), overwrite: true);
		}
	}
}
