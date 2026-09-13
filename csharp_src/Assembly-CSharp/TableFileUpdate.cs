using System;
using System.Collections.Generic;
using System.IO;
using BsDiff;
using GameFramework;
using GameKit.Base;
using VEngine;

public class TableFileUpdate
{
	public static ulong GetDownloadSize(List<DownloadInfo> downloadInfos)
	{
		if (ClientConfig.SKIP_UPDATE_TABLE_AND_LOCALIZATION)
		{
			return 0uL;
		}
		Log.Info($"[TableFileUpdate] builtin: {ClientConfig.BUILTIN_TABLE_VERSION}, {ClientConfig.BUILTIN_TABLE_MD5}, current: {ClientConfig.CURRENT_TABLE_VERSION}, {ClientConfig.CURRENT_TABLE_MD5}, remote: {ClientConfig.Remote_Table_Version}, {ClientConfig.Remote_Table_MD5}");
		if (ClientConfig.Remote_Table_Version >= ClientConfig.BUILTIN_TABLE_VERSION && ClientConfig.Remote_Table_Version != ClientConfig.CURRENT_TABLE_VERSION)
		{
			if (ClientConfig.CURRENT_TABLE_MD5 == ClientConfig.Remote_Table_MD5 && !string.IsNullOrEmpty(ClientConfig.CURRENT_TABLE_FILE_PATH) && File.Exists(ClientConfig.CURRENT_TABLE_FILE_PATH))
			{
				return 0uL;
			}
			bool flag = ClientSwitch.IsOn(0);
			if (ClientConfig.HasTableFileReplaceLock())
			{
				Log.Info("[TableFileUpdate] force replace update.");
				flag = false;
			}
			if (flag && ClientConfig.Remote_Table_Patches.TryGetValue(ClientConfig.CURRENT_TABLE_VERSION, out (ulong, uint) value))
			{
				ClientConfig.Remote_Table_UpdateFlag = UpdateFlag.Patch;
				string text = ClientConfig.TableFilePatchName(ClientConfig.Remote_Table_Version, ClientConfig.CURRENT_TABLE_VERSION);
				DownloadInfo downloadInfo = new DownloadInfo();
				downloadInfo.crc = value.Item2;
				downloadInfo.savePath = ClientConfig.GetDownloadDataPath(text);
				downloadInfo.size = value.Item1;
				downloadInfo.url = ClientConfig.DownloadURL + "table/" + text;
				downloadInfos.Add(downloadInfo);
				Log.Info($"[TableFileUpdate] download patch {downloadInfo.url}, {ClientConfig.Remote_Table_Size}.");
				return value.Item1;
			}
			ClientConfig.Remote_Table_UpdateFlag = UpdateFlag.Replace;
			ClientConfig.RemoveTableFileReplaceLock();
			string text2 = ClientConfig.TableFileName(ClientConfig.Remote_Table_Version, ClientConfig.Remote_Table_MD5);
			ClientConfig.CURRENT_TABLE_VERSION = ClientConfig.Remote_Table_Version;
			ClientConfig.CURRENT_TABLE_MD5 = ClientConfig.Remote_Table_MD5;
			ClientConfig.CURRENT_TABLE_FILE_PATH = ClientConfig.GetDownloadDataPath(text2);
			DownloadInfo downloadInfo2 = new DownloadInfo();
			downloadInfo2.crc = ClientConfig.Remote_Table_CRC;
			downloadInfo2.savePath = ClientConfig.CURRENT_TABLE_FILE_PATH;
			downloadInfo2.size = ClientConfig.Remote_Table_Size;
			downloadInfo2.url = ClientConfig.DownloadURL + "table/" + text2;
			downloadInfos.Add(downloadInfo2);
			Log.Info($"[TableFileUpdate] download file {downloadInfo2.url}, {ClientConfig.Remote_Table_Size}.");
			return ClientConfig.Remote_Table_Size;
		}
		Log.Info("[TableFileUpdate] builtin is latest, return 0.");
		return 0uL;
	}

	public static bool ApplyUpdate()
	{
		if (ClientConfig.Remote_Table_UpdateFlag == UpdateFlag.Patch && ClientConfig.Remote_Table_Patches.TryGetValue(ClientConfig.CURRENT_TABLE_VERSION, out (ulong, uint) _))
		{
			string file = ClientConfig.TableFileName(ClientConfig.Remote_Table_Version, ClientConfig.Remote_Table_MD5);
			string file2 = ClientConfig.TableFilePatchName(ClientConfig.Remote_Table_Version, ClientConfig.CURRENT_TABLE_VERSION);
			string cURRENT_TABLE_FILE_PATH = ClientConfig.CURRENT_TABLE_FILE_PATH;
			string downloadDataPath = ClientConfig.GetDownloadDataPath(file);
			string patchFile = ClientConfig.GetDownloadDataPath(file2);
			bool flag = false;
			try
			{
				bool flag2 = false;
				bool flag3 = false;
				using (FileStream fileStream = File.OpenRead(patchFile))
				{
					byte[] array = new byte[8];
					fileStream.Read(array, 0, 8);
					flag3 = EncryptUtils.IsChachaPatch(array);
				}
				using (FileStream fileStream2 = File.OpenRead(cURRENT_TABLE_FILE_PATH))
				{
					byte[] array2 = new byte[8];
					fileStream2.Read(array2, 0, 8);
					flag2 = EncryptUtils.IsChachaTable(array2);
				}
				if (flag2 && flag3)
				{
					byte[] patchBytes = File.ReadAllBytes(patchFile);
					EncryptUtils.FromChachaToBsDiffPatch(patchBytes);
					byte[] array3 = File.ReadAllBytes(cURRENT_TABLE_FILE_PATH);
					EncryptUtils.FromChachaToPKZip(array3);
					MemoryStream memoryStream = new MemoryStream(array3, 0, array3.Length, writable: true, publiclyVisible: true);
					MemoryStream memoryStream2 = new MemoryStream();
					BinaryPatch.Apply(memoryStream, () => new MemoryStream(patchBytes), memoryStream2);
					memoryStream.Dispose();
					byte[] array4 = memoryStream2.ToArray();
					memoryStream2.Dispose();
					EncryptUtils.FromPKZipToChacha(array4);
					File.WriteAllBytes(downloadDataPath, array4);
					flag = true;
				}
				else if (flag2 && !flag3)
				{
					byte[] array5 = File.ReadAllBytes(cURRENT_TABLE_FILE_PATH);
					EncryptUtils.FromChachaToPKZip(array5);
					MemoryStream memoryStream3 = new MemoryStream(array5, 0, array5.Length, writable: true, publiclyVisible: true);
					using FileStream output = new FileStream(downloadDataPath, FileMode.Create);
					BinaryPatch.Apply(memoryStream3, () => new FileStream(patchFile, FileMode.Open, FileAccess.Read, FileShare.Read), output);
					memoryStream3.Dispose();
					flag = true;
				}
				else if (!flag2 && flag3)
				{
					using FileStream input = new FileStream(cURRENT_TABLE_FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read);
					byte[] patchBytes2 = File.ReadAllBytes(patchFile);
					EncryptUtils.FromChachaToBsDiffPatch(patchBytes2);
					MemoryStream memoryStream4 = new MemoryStream();
					BinaryPatch.Apply(input, () => new MemoryStream(patchBytes2), memoryStream4);
					byte[] array6 = memoryStream4.ToArray();
					memoryStream4.Dispose();
					EncryptUtils.FromPKZipToChacha(array6);
					File.WriteAllBytes(downloadDataPath, array6);
					flag = true;
				}
				else
				{
					using FileStream input2 = new FileStream(cURRENT_TABLE_FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read);
					using FileStream output2 = new FileStream(downloadDataPath, FileMode.Create);
					BinaryPatch.Apply(input2, () => new FileStream(patchFile, FileMode.Open, FileAccess.Read, FileShare.Read), output2);
					flag = true;
				}
			}
			catch (Exception arg)
			{
				Log.Error("[TableFileUpdate] ApplyUpdate Patch Exception '{0}'.", arg);
			}
			finally
			{
				File.Delete(patchFile);
			}
			if (!flag)
			{
				OnUpdateFailed(downloadDataPath);
				return false;
			}
			LWLuaFileUtil.GetSizeAndCrc(downloadDataPath, out var size, out var crc);
			if (ClientConfig.Remote_Table_Size != 0 && size != ClientConfig.Remote_Table_Size)
			{
				Log.Error($"[TableFileUpdate] update failed with file size: {ClientConfig.Remote_Table_Size} != {size}.");
				OnUpdateFailed(downloadDataPath);
				return false;
			}
			if (ClientConfig.Remote_Table_CRC != 0 && crc != ClientConfig.Remote_Table_CRC)
			{
				Log.Error($"[TableFileUpdate] update failed with file crc: {ClientConfig.Remote_Table_CRC} != {crc}.");
				OnUpdateFailed(downloadDataPath);
				return false;
			}
			Log.Info("[TableFileUpdate] pass update result check.");
			ClientConfig.CURRENT_TABLE_VERSION = ClientConfig.Remote_Table_Version;
			ClientConfig.CURRENT_TABLE_MD5 = ClientConfig.Remote_Table_MD5;
			ClientConfig.CURRENT_TABLE_FILE_PATH = downloadDataPath;
		}
		return true;
	}

	private static void OnUpdateFailed(string outputFile)
	{
		if (File.Exists(outputFile))
		{
			File.Delete(outputFile);
		}
		ClientConfig.CreateTableFileReplaceLock();
	}
}
