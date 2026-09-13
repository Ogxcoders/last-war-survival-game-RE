using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using VEngine;

public class LWLuaFile : IDisposable
{
	public class FileEntry
	{
		public string name;

		public long offset;

		public int length;

		public byte[] data;
	}

	public const string s_FileFolder = "lwScripts";

	public static string s_DataPath = "lwScripts";

	public static string s_Workspace = "lwScripts";

	private Stream _tmpFile;

	private BinaryReader _tmpFileReader;

	private Dictionary<string, FileEntry> _fileEntries = new Dictionary<string, FileEntry>();

	public int version { get; private set; }

	public int fileVersion { get; private set; }

	public ulong size { get; private set; }

	public uint crc { get; private set; }

	public static bool IsU440()
	{
		return true;
	}

	public static void ConfigPath()
	{
		s_DataPath = Application.streamingAssetsPath + "/lwScripts";
		s_Workspace = Application.persistentDataPath + "/lwScripts";
	}

	public static LWLuaFile Load(bool initSucceed, MemoryStream scriptFileMemory)
	{
		LWLuaFile lWLuaFile = null;
		bool flag = false;
		try
		{
			lWLuaFile = new LWLuaFile();
			flag = lWLuaFile._Load(initSucceed, scriptFileMemory);
		}
		catch (Exception arg)
		{
			Log.Error($"[LWLuaFile] load error: {arg}");
		}
		if (!flag)
		{
			return null;
		}
		return lWLuaFile;
	}

	private LWLuaFile()
	{
	}

	private bool _Load(bool initSucceed, MemoryStream scriptFileMemory)
	{
		_fileEntries.Clear();
		string text = s_Workspace + "/LWScripts.data";
		Close();
		Log.Info($"[LWLuaFile] begin load, initSucceed: {initSucceed}.");
		if (initSucceed)
		{
			if (!File.Exists(text))
			{
				Log.Error("[LWLuaFile] " + text + " is not exist.");
				return false;
			}
			GetSizeAndCrc(text, out var num, out var num2);
			size = num;
			crc = num2;
			_tmpFile = File.OpenRead(text);
			Log.Info("[LWLuaFile] load file: " + text);
		}
		else
		{
			if (scriptFileMemory == null)
			{
				Log.Error("[LWLuaFile] memoryFile is not exist.");
				return false;
			}
			_tmpFile = scriptFileMemory;
			_tmpFile.Seek(0L, SeekOrigin.Begin);
			Log.Info("[LWLuaFile] load stream.");
		}
		_tmpFileReader = new BinaryReader(_tmpFile);
		_tmpFileReader.ReadBytes(4);
		fileVersion = _tmpFileReader.ReadInt32();
		version = _tmpFileReader.ReadInt32();
		try
		{
			File.WriteAllText(s_Workspace + "/version.txt", version.ToString());
		}
		catch (Exception)
		{
		}
		ClientConfig.LWLuaVersion = version;
		Log.Info($"[LWLuaFile] file version: {version}.");
		Log.Info($"[LWLuaFile] file format version: {fileVersion}.");
		int num3 = 0;
		string text2 = string.Empty;
		int num4 = 0;
		bool flag = false;
		try
		{
			num4 = _tmpFileReader.ReadInt32();
			for (int i = 0; i < num4; i++)
			{
				string text3 = _tmpFileReader.ReadString();
				int num5 = _tmpFileReader.ReadInt32();
				num3 = i;
				text2 = text3;
				_fileEntries.Add(text3, new FileEntry
				{
					name = text3,
					offset = _tmpFile.Position,
					length = num5
				});
				_tmpFile.Seek(num5, SeekOrigin.Current);
			}
			flag = true;
		}
		catch (Exception arg)
		{
			Log.Error($"[LWLuaFile] load error: {arg}");
			Log.Error($"[LWLuaFile] load error: {num4}, {num3}, {text2}, {size}, {crc}");
			Close();
			LWLuaFileUpdate.SetClearFlag();
		}
		Log.Info($"[LWLuaFile] finish load, succeed: {flag}.");
		return flag;
	}

	public void Dispose()
	{
		Close();
	}

	public void Close()
	{
		_tmpFileReader?.Dispose();
		_tmpFileReader = null;
		_tmpFile?.Dispose();
		_tmpFile = null;
		_fileEntries.Clear();
	}

	public static bool InWhiteList(string fileName)
	{
		return fileName.Equals("xlua.util");
	}

	public byte[] LoadFile(string fileName)
	{
		fileName = fileName.Replace(".", "/") + ".luac";
		byte[] array = null;
		if (_fileEntries.TryGetValue(fileName, out var value))
		{
			_tmpFile.Seek(value.offset, SeekOrigin.Begin);
			array = _tmpFileReader.ReadBytes(value.length);
			if (fileVersion == LWLuaFileUtil.EncryptedFileVersion)
			{
				EncryptUtils.SuperDecrypt(array);
			}
		}
		return array;
	}

	private static void GetSizeAndCrc(string file, out ulong size, out uint crc)
	{
		using FileStream fileStream = File.OpenRead(file);
		size = (ulong)fileStream.Length;
		crc = Utility.ComputeCRC32(fileStream);
	}
}
