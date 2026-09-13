using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VEngine;

public class LocalBundleOffsetMapper
{
	private Dictionary<string, long> _offsetTable;

	private string[] _fragmentPathList;

	public static LocalBundleOffsetMapper Init()
	{
		string text = null;
		string text2 = null;
		if (Versions.syncLoadPackageManifest)
		{
			text = Versions.GetPlayerDataPath(CommonUtils.BUNDLE_OFFSET_TABLE_FILE);
			text2 = Versions.GetPlayerDataPath(CommonUtils.BUNDLE_ALIAS_OFFSET_TABLE_FILE);
		}
		else
		{
			text = Path.Combine(Application.persistentDataPath, CommonUtils.BUNDLE_OFFSET_TABLE_FILE);
			text2 = Path.Combine(Application.persistentDataPath, CommonUtils.BUNDLE_ALIAS_OFFSET_TABLE_FILE);
		}
		return Init(text, text2);
	}

	public static LocalBundleOffsetMapper Init(string bundleOffsetPath, string aliasOffsetPath)
	{
		string[] fragmentPathList = null;
		string[] fragmentPathList2 = null;
		byte[] tableBytes = null;
		byte[] tableBytes2 = null;
		bool num = SyncReader.ExistFile(bundleOffsetPath);
		if (num)
		{
			Debug.Log("LocalBundle::Read Bundle Offset Table: " + bundleOffsetPath);
			ReadOffsetTable(bundleOffsetPath, out fragmentPathList, out tableBytes);
		}
		bool flag = SyncReader.ExistFile(aliasOffsetPath);
		if (flag)
		{
			Debug.Log("LocalBundle::Read Alias Offset Table: " + aliasOffsetPath);
			ReadOffsetTable(aliasOffsetPath, out fragmentPathList2, out tableBytes2);
		}
		if (num || flag)
		{
			return new LocalBundleOffsetMapper(fragmentPathList, tableBytes, fragmentPathList2, tableBytes2);
		}
		Debug.LogError("LocalBundle::LocalBundleOffsetMapper 初始化失败, " + bundleOffsetPath + ", " + aliasOffsetPath);
		return new LocalBundleOffsetMapper();
	}

	private static void ReadOffsetTable(string dstPath, out string[] fragmentPathList, out byte[] tableBytes)
	{
		byte[] buffer = SyncReader.ReadAllBytesByFilePath(dstPath);
		fragmentPathList = null;
		tableBytes = null;
		using MemoryStream input = new MemoryStream(buffer);
		using BinaryReader binaryReader = new BinaryReader(input);
		int num = binaryReader.ReadInt32();
		if (num > 0)
		{
			fragmentPathList = new string[num];
			for (int i = 0; i < num; i++)
			{
				fragmentPathList[i] = binaryReader.ReadString();
			}
		}
		int num2 = binaryReader.ReadInt32();
		if (num2 > 0)
		{
			tableBytes = binaryReader.ReadBytes(num2);
		}
		Debug.Log($"LocalBundle::InitMapper:FragmentCount:{num}: bytes:{num2}");
	}

	public LocalBundleOffsetMapper()
	{
		_offsetTable = new Dictionary<string, long>();
	}

	public LocalBundleOffsetMapper(string[] bundleOffsetFragmentsList, byte[] bundleOffsetTableBytes, string[] aliasOffsetFragmentsList, byte[] aliasOffsetTableBytes)
	{
		if (bundleOffsetTableBytes != null)
		{
			_fragmentPathList = bundleOffsetFragmentsList;
			Debug.Log("LocalBundle::LocalBundleOffsetMapper 初始化成功 use bundleOffsetTableBytes");
			Debug.Log($"LocalBundle::_fragmentPathList {_fragmentPathList.Length}:{_fragmentPathList[0]} ");
			using MemoryStream input = new MemoryStream(bundleOffsetTableBytes);
			using BinaryReader binaryReader = new BinaryReader(input);
			int num = binaryReader.ReadInt32();
			_offsetTable = new Dictionary<string, long>((aliasOffsetTableBytes != null) ? (num * 2) : num);
			for (int i = 0; i < num; i++)
			{
				string key = binaryReader.ReadString();
				long value = binaryReader.ReadInt64();
				_offsetTable.Add(key, value);
			}
		}
		if (aliasOffsetTableBytes == null)
		{
			return;
		}
		_fragmentPathList = aliasOffsetFragmentsList;
		Debug.Log("LocalBundle::LocalBundleOffsetMapper 初始化成功 use aliasOffsetTableBytes");
		Debug.Log($"LocalBundle::_fragmentPathList {_fragmentPathList.Length}:{_fragmentPathList[0]} ");
		using MemoryStream input2 = new MemoryStream(aliasOffsetTableBytes);
		using BinaryReader binaryReader2 = new BinaryReader(input2);
		int num2 = binaryReader2.ReadInt32();
		if (_offsetTable == null)
		{
			_offsetTable = new Dictionary<string, long>(num2);
		}
		for (int j = 0; j < num2; j++)
		{
			string key2 = binaryReader2.ReadString();
			long value2 = binaryReader2.ReadInt64();
			_offsetTable.Add(key2, value2);
		}
	}

	public void GetFileInfo(string relativePath, out string mappingRelativePath, out int offset)
	{
		mappingRelativePath = relativePath;
		offset = -1;
		if (_offsetTable.TryGetValue(relativePath, out var value) && value != -1)
		{
			offset = (int)(value & 0xFFFFFFFFu);
			int num = (int)(value >> 32);
			mappingRelativePath = Versions.GetPlayerDataPath(_fragmentPathList[num]);
		}
	}
}
