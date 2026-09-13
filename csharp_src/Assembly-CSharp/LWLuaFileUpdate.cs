using System;
using System.Collections.Generic;
using System.IO;
using BsDiff;
using GameFramework;
using GameKit.Base;
using ICSharpCode.SharpZipLib.BZip2;
using UnityEngine;
using UnityEngine.Networking;
using VEngine;

public static class LWLuaFileUpdate
{
	private class VersionInfo
	{
		public int version;

		public ulong bzSize;

		public uint bzCrc;

		public Dictionary<int, (ulong size, uint crc)> patches = new Dictionary<int, (ulong, uint)>();
	}

	public static bool initSucceed = false;

	public static MemoryStream scriptFileMemory = null;

	public static string CLEAR_KEY = "LWLUAFILE_CLEAR";

	public static string LWLUAFILE_UPDATE_KEY = "LWLUAFILE_UPDATE";

	public static string LWLUAFILE_UPDATE_FILE_PATH = "lwScripts/lwFileUp.info";

	private static VersionInfo _remoteVersionInfo = null;

	private static ulong _remoteSize = 0uL;

	private static uint _remoteCrc = 0u;

	private static UpdateFlag _updateFlag = UpdateFlag.None;

	private static string _downloadFilePath = string.Empty;

	public const int MaxRetryTimes = 5;

	public static bool hasUpdate => _updateFlag != UpdateFlag.None;

	public static void ConfigPath()
	{
		LWLUAFILE_UPDATE_FILE_PATH = Application.persistentDataPath + "/lwScripts/lwFileUp.info";
	}

	public static void SetClearFlag()
	{
		PlayerPrefs.SetInt(CLEAR_KEY, 1);
	}

	public static void RemoveClearFlag()
	{
		PlayerPrefs.DeleteKey(CLEAR_KEY);
	}

	public static bool HasClearFlag()
	{
		return PlayerPrefs.GetInt(CLEAR_KEY, 0) != 0;
	}

	public static void SaveUpdateKey()
	{
		if (_updateFlag != UpdateFlag.None)
		{
			int num = ((_updateFlag == UpdateFlag.Replace) ? 1 : 2);
			string text = $"{num}|{_downloadFilePath}|{_remoteSize}|{_remoteCrc}";
			PlayerPrefs.SetString(LWLUAFILE_UPDATE_KEY, text);
			Log.Info("[LWLuaFileUpdate] SaveUpdateKey value: " + text);
		}
		else
		{
			PlayerPrefs.DeleteKey(LWLUAFILE_UPDATE_KEY);
			Log.Info("[LWLuaFileUpdate] SaveUpdateKey updateFlag is none.");
		}
		PlayerPrefs.Save();
	}

	public static void ClearUpdateKey()
	{
		PlayerPrefs.DeleteKey(LWLUAFILE_UPDATE_KEY);
		Log.Info("[LWLuaFileUpdate] ClearUpdateKey.");
	}

	private static UpdateFlag HasUpdateKey(ref string fileName, ref ulong size, ref uint crc)
	{
		if (PlayerPrefs.HasKey(LWLUAFILE_UPDATE_KEY))
		{
			try
			{
				string text = PlayerPrefs.GetString(LWLUAFILE_UPDATE_KEY);
				Log.Info("[LWLuaFileUpdate] HasUpdateKey value: " + text + ".");
				string[] array = text.Split(new char[1] { '|' });
				int num = int.Parse(array[0]);
				fileName = array[1];
				size = ulong.Parse(array[2]);
				crc = uint.Parse(array[3]);
				switch (num)
				{
				case 1:
					return UpdateFlag.Replace;
				case 2:
					return UpdateFlag.Patch;
				}
			}
			catch (Exception message)
			{
				Log.Error(message);
			}
		}
		return UpdateFlag.None;
	}

	public static void SaveUpdateFile()
	{
		if (_updateFlag != UpdateFlag.None)
		{
			int num = ((_updateFlag == UpdateFlag.Replace) ? 1 : 2);
			string text = $"{num}|{_downloadFilePath}|{_remoteSize}|{_remoteCrc}";
			File.WriteAllText(LWLUAFILE_UPDATE_FILE_PATH, text);
			Log.Info("[LWLuaFileUpdate] SaveUpdateFile value: " + LWLUAFILE_UPDATE_FILE_PATH + ", " + text);
		}
		else
		{
			if (File.Exists(LWLUAFILE_UPDATE_FILE_PATH))
			{
				File.Delete(LWLUAFILE_UPDATE_FILE_PATH);
			}
			Log.Info("[LWLuaFileUpdate] SaveUpdateFile updateFlag is none.");
		}
	}

	public static void ClearUpdateFile()
	{
		if (File.Exists(LWLUAFILE_UPDATE_FILE_PATH))
		{
			File.Delete(LWLUAFILE_UPDATE_FILE_PATH);
		}
		Log.Info("[LWLuaFileUpdate] ClearUpdateFile.");
	}

	public static UpdateFlag HasUpdateFile(ref string fileName, ref ulong size, ref uint crc)
	{
		if (File.Exists(LWLUAFILE_UPDATE_FILE_PATH))
		{
			try
			{
				string text = File.ReadAllText(LWLUAFILE_UPDATE_FILE_PATH);
				Log.Info("[LWLuaFileUpdate] HasUpdateFile value: " + text + ".");
				string[] array = text.Split(new char[1] { '|' });
				int num = int.Parse(array[0]);
				fileName = array[1];
				size = ulong.Parse(array[2]);
				crc = uint.Parse(array[3]);
				switch (num)
				{
				case 1:
					return UpdateFlag.Replace;
				case 2:
					return UpdateFlag.Patch;
				}
			}
			catch (Exception message)
			{
				Log.Error(message);
			}
		}
		return UpdateFlag.None;
	}

	public static void InitFileOnAppStart()
	{
		try
		{
			initSucceed = false;
			if (!Directory.Exists(LWLuaFile.s_Workspace))
			{
				Directory.CreateDirectory(LWLuaFile.s_Workspace);
			}
			scriptFileMemory?.Dispose();
			scriptFileMemory = null;
			string text = LWLuaFile.s_Workspace + "/LWScripts.data";
			string text2 = LWLuaFile.s_Workspace + "/LWScripts.txt";
			string fileName = LWLuaFile.s_DataPath + "/LWScripts.bz2";
			string text3 = LWLuaFile.s_Workspace + "/LWScripts.bz2";
			if (HasClearFlag())
			{
				RemoveClearFlag();
				if (File.Exists(text))
				{
					Log.Info("[LWLuaFileUpdate] clear flag is set, delete scriptFile.");
					File.Delete(text);
				}
				else
				{
					Log.Warning("[LWLuaFileUpdate] clear flag is set, but scriptFile is not exist.");
				}
			}
			string fileName2 = string.Empty;
			ulong size = 0uL;
			uint crc = 0u;
			UpdateFlag updateFlag = HasUpdateFile(ref fileName2, ref size, ref crc);
			if (updateFlag != UpdateFlag.None)
			{
				ApplyUpdateOnStartup(updateFlag, fileName2, size, crc);
				ClearUpdateFile();
			}
			else
			{
				updateFlag = HasUpdateKey(ref fileName2, ref size, ref crc);
				if (updateFlag != UpdateFlag.None)
				{
					ApplyUpdateOnStartup(updateFlag, fileName2, size, crc);
					ClearUpdateKey();
				}
			}
			if (File.Exists(text))
			{
				if (File.Exists(text2))
				{
					LWLuaFileUtil.ReadSizeAndCrc(text2, out var size2, out var crc2);
					LWLuaFileUtil.GetSizeAndCrc(text, out var size3, out var crc3);
					if (size3 != size2 || crc3 != crc2)
					{
						Log.Info("[LWLuaFileUpdate] version file size or crc mismatch, delete scriptFile.");
						File.Delete(text);
					}
					else
					{
						Log.Info($"[LWLuaFileUpdate] version file size: {size2}, crc: {crc2}.");
					}
				}
				else
				{
					Log.Info("[LWLuaFileUpdate] version file not exist, delete scriptFile.");
					File.Delete(text);
				}
			}
			else
			{
				Log.Info("[LWLuaFileUpdate] scriptFile file is not exist.");
			}
			if (!File.Exists(text))
			{
				if (File.Exists(text3))
				{
					File.Delete(text3);
				}
				string fileName3 = LWLuaFile.s_DataPath + "/LWScripts.txt";
				ulong num = 0uL;
				uint num2 = 0u;
				if (StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.303") >= 0)
				{
					try
					{
						if (BuiltinFileReader.ReadyFileFromBuiltIn(fileName3, out var _, out var handler))
						{
							string[] array = handler.text.Split(new char[1] { '|' });
							num = ulong.Parse(array[3]);
							num2 = uint.Parse(array[4]);
							Log.Info($"[LWLuaFileUpdateV2] version file size: {num}, crc: {num2}.");
						}
						else
						{
							Log.Error("[LWLuaFileUpdateV2] read version file failed.");
						}
					}
					catch (Exception arg)
					{
						Log.Error($"[LWLuaFileUpdateV2] version file read has error, {arg}");
					}
					Log.Info("[LWLuaFileUpdateV2] copy uncompressed script file to " + text + ".");
					string fileName4 = LWLuaFile.s_DataPath + "/LWScripts.data";
					BuiltinFileReader.CopyFileFromBuiltIn(fileName4, text);
					ulong size4 = 0uL;
					uint crc4 = 0u;
					if (File.Exists(text))
					{
						LWLuaFileUtil.GetSizeAndCrc(text, out size4, out crc4);
						initSucceed = true;
						if (num != 0 && num != size4)
						{
							Log.Error($"[LWLuaFileUpdateV2] script file check size {num} != {size4}.");
							initSucceed = false;
						}
						if (num2 != 0 && num2 != crc4)
						{
							Log.Error($"[LWLuaFileUpdateV2] script file check crc {num2} != {crc4}.");
							initSucceed = false;
						}
					}
					string error2;
					DownloadHandler handler2;
					if (initSucceed)
					{
						Log.Info("[LWLuaFileUpdateV2] copy script file to " + text + ", initSucceed.");
						LWLuaFileUtil.WriteSizeAndCrc(text2, size4, crc4);
					}
					else if (BuiltinFileReader.ReadyFileFromBuiltIn(fileName4, out error2, out handler2))
					{
						Log.Info("[LWLuaFileUpdateV2] copy script file to memory stream");
						scriptFileMemory = new MemoryStream(handler2.data);
					}
					else
					{
						Log.Error("[LWLuaFileUpdateV2] copy script file to memory stream");
					}
				}
				else if (StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.191") >= 0)
				{
					Log.Info("[LWLuaFileUpdate] copy compressed script file to " + text3 + ".");
					BuiltinFileReader.CopyFileFromBuiltIn(fileName, text3);
					if (!File.Exists(text3))
					{
						Log.Error("[LWLuaFileUpdate] copy builtin LWFile has error, isn`t exist.");
						return;
					}
					try
					{
						if (BuiltinFileReader.ReadyFileFromBuiltIn(fileName3, out var _, out var handler3))
						{
							string[] array2 = handler3.text.Split(new char[1] { '|' });
							num = ulong.Parse(array2[3]);
							num2 = uint.Parse(array2[4]);
							Log.Info($"[LWLuaFileUpdate] version file size: {num}, crc: {num2}.");
						}
						else
						{
							Log.Error("[LWLuaFileUpdate] read version file failed.");
						}
					}
					catch (Exception arg2)
					{
						Log.Error($"[LWLuaFileUpdate] version file read has error, {arg2}");
					}
					try
					{
						int num3 = 0;
						do
						{
							num3++;
							if (File.Exists(text))
							{
								Log.Info($"[LWLuaFileUpdate] delete decompress script file, [{num3}/{5}].");
								File.Delete(text);
							}
							if (num3 == 1)
							{
								Log.Info($"[LWLuaFileUpdate] copy uncompressed script file to {text}, [{num3}/{5}].");
								BuiltinFileReader.CopyFileFromBuiltIn(LWLuaFile.s_DataPath + "/LWScripts.data", text);
							}
							else
							{
								Log.Info($"[LWLuaFileUpdate] decompress script file to {text}, [{num3}/{5}].");
								byte[] array3 = File.ReadAllBytes(text3);
								if (EncryptUtils.IsChachaPackage(array3))
								{
									EncryptUtils.FromChachaToBZip(array3);
									MemoryStream memoryStream = new MemoryStream(array3);
									MemoryStream memoryStream2 = new MemoryStream();
									BZip2.Decompress(memoryStream, memoryStream2, isStreamOwner: false);
									memoryStream.Dispose();
									byte[] array4 = memoryStream2.ToArray();
									memoryStream2.Dispose();
									MemoryStream memoryStream3 = new MemoryStream(array4, 0, array4.Length, writable: true, publiclyVisible: true);
									LWLuaFileUtil.EncodeLWLuaFileData(memoryStream3);
									FileStream fileStream = File.Open(text, FileMode.Create, FileAccess.Write);
									memoryStream3.WriteTo(fileStream);
									memoryStream3.Dispose();
									fileStream.Dispose();
								}
								else
								{
									MemoryStream inStream = new MemoryStream(array3);
									FileStream outStream = File.Open(text, FileMode.Create, FileAccess.Write);
									BZip2.Decompress(inStream, outStream, isStreamOwner: true);
								}
							}
							ulong size5 = 0uL;
							uint crc5 = 0u;
							if (File.Exists(text))
							{
								LWLuaFileUtil.GetSizeAndCrc(text, out size5, out crc5);
								initSucceed = true;
								if (num != 0 && num != size5)
								{
									Log.Error($"[LWLuaFileUpdate] unzip script file check size {num} != {size5}, [{num3}/{5}].");
									initSucceed = false;
								}
								if (num2 != 0 && num2 != crc5)
								{
									Log.Error($"[LWLuaFileUpdate] unzip script file check crc {num2} != {crc5}, [{num3}/{5}].");
									initSucceed = false;
								}
							}
							if (initSucceed)
							{
								Log.Info("[LWLuaFileUpdate] decompress script file to " + text + ", initSucceed.");
								LWLuaFileUtil.WriteSizeAndCrc(text2, size5, crc5);
								break;
							}
						}
						while (num3 < 5);
					}
					catch (Exception arg3)
					{
						Log.Error($"[LWLuaFileUpdate] {arg3}");
					}
					if (!initSucceed)
					{
						Log.Info("[LWLuaFileUpdate] decompress script file to memory stream");
						byte[] array5 = File.ReadAllBytes(text3);
						bool flag = false;
						if (EncryptUtils.IsChachaPackage(array5))
						{
							EncryptUtils.FromChachaToBZip(array5);
							flag = true;
						}
						MemoryStream memoryStream4 = new MemoryStream(array5);
						scriptFileMemory = new MemoryStream();
						BZip2.Decompress(memoryStream4, scriptFileMemory, isStreamOwner: false);
						if (flag)
						{
							byte[] array6 = scriptFileMemory.ToArray();
							MemoryStream stream = new MemoryStream(array6, 0, array6.Length, writable: true, publiclyVisible: true);
							LWLuaFileUtil.EncodeLWLuaFileData(stream);
							scriptFileMemory.Dispose();
							scriptFileMemory = stream;
						}
						memoryStream4.Dispose();
						File.Delete(text);
					}
					File.Delete(text3);
				}
				else
				{
					Log.Info("[LWLuaFileUpdate] skip uncompressed script file.");
					initSucceed = true;
				}
			}
			else
			{
				Log.Info("[LWLuaFileUpdate] script file exist, initSucceed.");
				initSucceed = true;
			}
		}
		catch (Exception arg4)
		{
			Log.Error($"[LWLuaFileUpdate] InitFileOnAppStart {arg4}");
		}
	}

	public static void SetFileVersionInfo(string versionInfoString)
	{
		try
		{
			_remoteVersionInfo = new VersionInfo();
			string[] array = versionInfoString.Split(new char[1] { ';' });
			string[] array2 = array[0].Split(new char[1] { '|' });
			_remoteVersionInfo.version = int.Parse(array2[0]);
			_remoteVersionInfo.bzSize = ulong.Parse(array2[1]);
			_remoteVersionInfo.bzCrc = uint.Parse(array2[2]);
			if (array2.Length >= 5)
			{
				_remoteSize = ulong.Parse(array2[3]);
				_remoteCrc = uint.Parse(array2[4]);
			}
			else
			{
				_remoteSize = 0uL;
				_remoteCrc = 0u;
			}
			int i = 1;
			for (int num = array.Length; i < num; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					string[] array3 = array[i].Split(new char[1] { '|' });
					int key = int.Parse(array3[0]);
					ulong item = ulong.Parse(array3[1]);
					uint item2 = uint.Parse(array3[2]);
					_remoteVersionInfo.patches.Add(key, (item, item2));
				}
			}
			Log.Info("[LWLuaFileUpdate] SetFileVersionInfo " + versionInfoString + ".");
		}
		catch (Exception message)
		{
			_remoteVersionInfo = null;
			Log.Error(message);
			if (string.IsNullOrEmpty(versionInfoString))
			{
				Log.Info("[LWLuaFileUpdate] versionInfoString is empty");
			}
			else
			{
				Log.Info("[LWLuaFileUpdate] versionInfoString " + versionInfoString);
			}
		}
	}

	public static void ResetFileVersionInfo()
	{
		_remoteVersionInfo = null;
	}

	public static ulong CheckUpdate(List<DownloadInfo> downloadInfos)
	{
		ulong result = 0uL;
		bool flag = XLuaManager.s_lwLuaFile != null && scriptFileMemory == null;
		int num = (flag ? XLuaManager.s_lwLuaFile.version : 0);
		if (_remoteVersionInfo == null)
		{
			Log.Error("[LWLuaFileUpdate] remote Version is null.");
			return result;
		}
		Log.Info($"[LWLuaFileUpdate] CheckUpdate hasLwLuaFile: {flag}, currentVer: {num}, remoteVer: {_remoteVersionInfo.version}.");
		if (flag)
		{
			if (ClientSwitch.IsOff(12))
			{
				if (num != _remoteVersionInfo.version)
				{
					result = DownloadLWFile(downloadInfos);
				}
				else
				{
					SkipDownloadLWFile();
				}
			}
			else if (num > _remoteVersionInfo.version)
			{
				result = DownloadLWFile(downloadInfos);
			}
			else if (num < _remoteVersionInfo.version)
			{
				if (_remoteVersionInfo.patches.TryGetValue(num, out (ulong, uint) value))
				{
					_updateFlag = UpdateFlag.Patch;
					string text = LWLuaFileUtil.FormatPatchFileName(_remoteVersionInfo.version, num, LWLuaFile.IsU440());
					_downloadFilePath = LWLuaFile.s_Workspace + "/" + text;
					result = Download(downloadInfos, text, value.Item1, value.Item2, _downloadFilePath);
					Log.Info("[LWLuaFileUpdate] DownloadLWFile Patch " + text + ".");
				}
				else
				{
					result = DownloadLWFile(downloadInfos);
				}
			}
			else
			{
				SkipDownloadLWFile();
			}
		}
		else
		{
			result = DownloadLWFile(downloadInfos);
		}
		return result;
	}

	private static void SkipDownloadLWFile()
	{
		_updateFlag = UpdateFlag.None;
		_downloadFilePath = string.Empty;
		Log.Info("[LWLuaFileUpdate] SkipDownloadLWFile.");
	}

	private static ulong DownloadLWFile(List<DownloadInfo> downloadInfos)
	{
		_updateFlag = UpdateFlag.Replace;
		string text = LWLuaFileUtil.FormatCompressedFileName(_remoteVersionInfo.version, LWLuaFile.IsU440());
		_downloadFilePath = LWLuaFile.s_Workspace + "/" + text;
		ulong result = Download(downloadInfos, text, _remoteVersionInfo.bzSize, _remoteVersionInfo.bzCrc, _downloadFilePath);
		Log.Info("[LWLuaFileUpdate] DownloadLWFile " + text + ".");
		return result;
	}

	private static ulong Download(List<DownloadInfo> downloadInfos, string fileName, ulong size, uint crc, string savePath)
	{
		DownloadInfo downloadInfo = new DownloadInfo();
		downloadInfo.crc = crc;
		downloadInfo.savePath = savePath;
		downloadInfo.size = size;
		downloadInfo.url = Versions.GetDownloadURL(fileName);
		downloadInfos.Add(downloadInfo);
		return size;
	}

	private static bool ApplyUpdateOnStartup(UpdateFlag updateFlag, string downloadFilePath, ulong size, uint crc)
	{
		Log.Info($"[LWLuaFileUpdate] ApplyUpdateOnStartup updateFlag: {updateFlag}, downloadFilePath: {downloadFilePath}, hasDownloadFile: {File.Exists(downloadFilePath)}, size: {size}, crc: {crc}.");
		_updateFlag = updateFlag;
		_downloadFilePath = downloadFilePath;
		_remoteSize = size;
		_remoteCrc = crc;
		bool result = ApplyUpdate();
		_updateFlag = UpdateFlag.None;
		_downloadFilePath = string.Empty;
		_remoteSize = 0uL;
		_remoteCrc = 0u;
		return result;
	}

	public static bool ApplyUpdate()
	{
		bool flag = false;
		Log.Info($"[LWLuaFileUpdate] ApplyUpdate updateFlag: {_updateFlag}, downloadFilePath: {_downloadFilePath}, hasDownloadFile: {File.Exists(_downloadFilePath)}.");
		if (_updateFlag != UpdateFlag.None && !string.IsNullOrEmpty(_downloadFilePath) && File.Exists(_downloadFilePath))
		{
			string text = LWLuaFile.s_Workspace + "/LWScripts.data";
			string text2 = LWLuaFile.s_Workspace + "/LWScripts.data.tmp";
			int num = 0;
			do
			{
				num++;
				if (ApplyUpdate(text, text2, num) && CheckUpdateResult(text2))
				{
					LWLuaFileUtil.WriteSizeAndCrc(LWLuaFile.s_Workspace + "/LWScripts.txt", _remoteSize, _remoteCrc);
					flag = true;
					break;
				}
			}
			while (num < 5);
			if (flag)
			{
				Log.Info("[LWLuaFileUpdate] update succeed with " + _downloadFilePath + ".");
				SwapFilePath(text, text2);
				File.Delete(text2);
				File.Delete(_downloadFilePath);
			}
			else
			{
				Log.Error("[LWLuaFileUpdate] update failed with " + _downloadFilePath + ".");
			}
		}
		return flag;
	}

	private static bool ApplyUpdate(string originalFile, string tmpOutputFile, int retryTime)
	{
		try
		{
			if (File.Exists(tmpOutputFile))
			{
				Log.Info($"[LWLuaFileUpdate] LWLuaFile delete tmp file {tmpOutputFile} [{retryTime}/{5}].");
				File.Delete(tmpOutputFile);
			}
		}
		catch (Exception arg)
		{
			Log.Error("[LWLuaFileUpdate] LWLuaFile delete tmp file Exception '{0}'.", arg);
		}
		bool result = false;
		if (_updateFlag == UpdateFlag.Replace)
		{
			Log.Info($"[LWLuaFileUpdate] ApplyUpdate replace script file with {_downloadFilePath} [{retryTime}/{5}].");
			try
			{
				byte[] array = File.ReadAllBytes(_downloadFilePath);
				if (EncryptUtils.IsChachaPackage(array))
				{
					EncryptUtils.FromChachaToBZip(array);
					MemoryStream memoryStream = new MemoryStream(array);
					MemoryStream memoryStream2 = new MemoryStream();
					BZip2.Decompress(memoryStream, memoryStream2, isStreamOwner: false);
					memoryStream.Dispose();
					byte[] array2 = memoryStream2.ToArray();
					memoryStream2.Dispose();
					MemoryStream memoryStream3 = new MemoryStream(array2, 0, array2.Length, writable: true, publiclyVisible: true);
					LWLuaFileUtil.EncodeLWLuaFileData(memoryStream3);
					FileStream fileStream = File.Open(tmpOutputFile, FileMode.Create, FileAccess.Write);
					memoryStream3.WriteTo(fileStream);
					memoryStream3.Dispose();
					fileStream.Dispose();
				}
				else
				{
					MemoryStream inStream = new MemoryStream(array);
					FileStream outStream = File.Open(tmpOutputFile, FileMode.Create, FileAccess.Write);
					BZip2.Decompress(inStream, outStream, isStreamOwner: true);
				}
				result = true;
			}
			catch (Exception arg2)
			{
				Log.Error("[LWLuaFileUpdate] LWLuaFile ApplyUpdate Replace Exception '{0}'.", arg2);
			}
		}
		else if (_updateFlag == UpdateFlag.Patch)
		{
			Log.Info($"[LWLuaFileUpdate] ApplyUpdate patch script file with {_downloadFilePath} [{retryTime}/{5}]");
			try
			{
				bool flag = false;
				bool flag2 = false;
				using (FileStream fileStream2 = File.OpenRead(_downloadFilePath))
				{
					byte[] array3 = new byte[8];
					fileStream2.Read(array3, 0, 8);
					flag2 = EncryptUtils.IsChachaPatch(array3);
				}
				using (FileStream fileStream3 = File.OpenRead(originalFile))
				{
					int num = 8;
					byte[] array4 = new byte[num];
					fileStream3.Read(array4, 0, num);
					MemoryStream memoryStream4 = new MemoryStream(array4, 0, array4.Length, writable: true, publiclyVisible: true);
					flag = LWLuaFileUtil.IsEncodedLWLuaFile(memoryStream4);
					memoryStream4.Dispose();
				}
				if (flag && flag2)
				{
					byte[] patchBytes = File.ReadAllBytes(_downloadFilePath);
					EncryptUtils.FromChachaToBsDiffPatch(patchBytes);
					byte[] array5 = File.ReadAllBytes(originalFile);
					MemoryStream memoryStream5 = new MemoryStream(array5, 0, array5.Length, writable: true, publiclyVisible: true);
					LWLuaFileUtil.DecodeLWLuaFileData(memoryStream5);
					MemoryStream memoryStream6 = new MemoryStream();
					BinaryPatch.Apply(memoryStream5, () => new MemoryStream(patchBytes), memoryStream6);
					memoryStream5.Dispose();
					byte[] array6 = memoryStream6.ToArray();
					memoryStream6.Dispose();
					MemoryStream memoryStream7 = new MemoryStream(array6, 0, array6.Length, writable: true, publiclyVisible: true);
					LWLuaFileUtil.EncodeLWLuaFileData(memoryStream7);
					byte[] bytes = memoryStream7.ToArray();
					memoryStream7.Dispose();
					File.WriteAllBytes(tmpOutputFile, bytes);
					result = true;
				}
				else if (flag && !flag2)
				{
					using FileStream output = new FileStream(tmpOutputFile, FileMode.Create);
					byte[] array7 = File.ReadAllBytes(originalFile);
					MemoryStream memoryStream8 = new MemoryStream(array7, 0, array7.Length, writable: true, publiclyVisible: true);
					LWLuaFileUtil.DecodeLWLuaFileData(memoryStream8);
					BinaryPatch.Apply(memoryStream8, () => new FileStream(_downloadFilePath, FileMode.Open, FileAccess.Read, FileShare.Read), output);
					memoryStream8.Dispose();
					result = true;
				}
				else if (!flag && flag2)
				{
					using FileStream input = new FileStream(originalFile, FileMode.Open, FileAccess.Read, FileShare.Read);
					byte[] patchBytes2 = File.ReadAllBytes(_downloadFilePath);
					EncryptUtils.FromChachaToBsDiffPatch(patchBytes2);
					MemoryStream memoryStream9 = new MemoryStream();
					BinaryPatch.Apply(input, () => new MemoryStream(patchBytes2), memoryStream9);
					byte[] array8 = memoryStream9.ToArray();
					memoryStream9.Dispose();
					MemoryStream memoryStream10 = new MemoryStream(array8, 0, array8.Length, writable: true, publiclyVisible: true);
					LWLuaFileUtil.EncodeLWLuaFileData(memoryStream10);
					byte[] bytes2 = memoryStream10.ToArray();
					memoryStream10.Dispose();
					File.WriteAllBytes(tmpOutputFile, bytes2);
					result = true;
				}
				else
				{
					using FileStream input2 = new FileStream(originalFile, FileMode.Open, FileAccess.Read, FileShare.Read);
					using FileStream output2 = new FileStream(tmpOutputFile, FileMode.Create);
					BinaryPatch.Apply(input2, () => new FileStream(_downloadFilePath, FileMode.Open, FileAccess.Read, FileShare.Read), output2);
					result = true;
				}
			}
			catch (Exception arg3)
			{
				Log.Error("[LWLuaFileUpdate] LWLuaFile ApplyUpdate Patch Exception '{0}'.", arg3);
			}
		}
		return result;
	}

	private static void SwapFilePath(string fileAPath, string fileBPath)
	{
		if (File.Exists(fileAPath))
		{
			string text = fileAPath + ".swap";
			File.Move(fileAPath, text);
			File.Move(fileBPath, fileAPath);
			File.Move(text, fileBPath);
		}
		else
		{
			File.Move(fileBPath, fileAPath);
		}
	}

	private static bool CheckUpdateResult(string file)
	{
		if (_remoteSize != 0 || _remoteCrc != 0)
		{
			LWLuaFileUtil.GetSizeAndCrc(file, out var size, out var crc);
			if (_remoteSize != 0 && size != _remoteSize)
			{
				Log.Error($"[LWLuaFileUpdate] update failed with file size: {_remoteSize} != {size}.");
				return false;
			}
			if (_remoteCrc != 0 && crc != _remoteCrc)
			{
				Log.Error($"[LWLuaFileUpdate] update failed with file crc: {_remoteCrc} != {crc}.");
				return false;
			}
			Log.Info("[LWLuaFileUpdate] pass update result check.");
		}
		else
		{
			Log.Info("[LWLuaFileUpdate] skip update result check.");
		}
		return true;
	}
}
