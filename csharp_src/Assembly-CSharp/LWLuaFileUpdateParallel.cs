using System;
using System.IO;
using BsDiff;
using GameFramework;
using GameKit.Base;
using ICSharpCode.SharpZipLib.BZip2;
using UnityEngine.Networking;

public static class LWLuaFileUpdateParallel
{
	private class PatchInfo
	{
		public UpdateFlag updateFlag;

		public string downloadFilePath = string.Empty;

		public ulong remoteSize;

		public uint remoteCrc;
	}

	private class LwScriptLoadPayload
	{
		public bool initSucceed;

		public MemoryStream scriptFileMemory;
	}

	public class LwScriptSwapPayload
	{
		public LWLuaFile lwLuaFile;
	}

	private static bool initSucceed;

	private static MemoryStream scriptFileMemory;

	public static void mt_CheckLwScript(object state)
	{
		Log.Info("[ParallelInit] mt_CheckLwScript");
		try
		{
			if (!Directory.Exists(LWLuaFile.s_Workspace))
			{
				Directory.CreateDirectory(LWLuaFile.s_Workspace);
			}
			string path = LWLuaFile.s_Workspace + "/LWScripts.data";
			if (LWLuaFileUpdate.HasClearFlag())
			{
				LWLuaFileUpdate.RemoveClearFlag();
				if (File.Exists(path))
				{
					Log.Info("[ParallelInit] [LWLuaFileUpdate] clear flag is set, delete scriptFile.");
					File.Delete(path);
				}
				else
				{
					Log.Warning("[ParallelInit] [LWLuaFileUpdate] clear flag is set, but scriptFile is not exist.");
				}
			}
		}
		catch (Exception arg)
		{
			Log.Error($"[ParallelInit] [LWLuaFileUpdate] mt_CheckLwScript {arg}");
			ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.SetTaskDone, 4));
		}
	}

	private static bool ApplyUpdateOnStartup(UpdateFlag updateFlag, string downloadFilePath, ulong size, uint crc)
	{
		Log.Info($"[ParallelInit] [LWLuaFileUpdate] ApplyUpdateOnStartup updateFlag: {updateFlag}, downloadFilePath: {downloadFilePath}, hasDownloadFile: {File.Exists(downloadFilePath)}, size: {size}, crc: {crc}.");
		return ApplyUpdate(new PatchInfo
		{
			updateFlag = updateFlag,
			downloadFilePath = downloadFilePath,
			remoteSize = size,
			remoteCrc = crc
		});
	}

	private static bool ApplyUpdate(PatchInfo patchInfo)
	{
		UpdateFlag updateFlag = patchInfo.updateFlag;
		string downloadFilePath = patchInfo.downloadFilePath;
		ulong remoteSize = patchInfo.remoteSize;
		uint remoteCrc = patchInfo.remoteCrc;
		bool flag = false;
		Log.Info($"[ParallelInit] [LWLuaFileUpdate] ApplyUpdate updateFlag: {updateFlag}, downloadFilePath: {downloadFilePath}, hasDownloadFile: {File.Exists(downloadFilePath)}.");
		if (updateFlag != UpdateFlag.None && !string.IsNullOrEmpty(downloadFilePath) && File.Exists(downloadFilePath))
		{
			string text = LWLuaFile.s_Workspace + "/LWScripts.data";
			string text2 = LWLuaFile.s_Workspace + "/LWScripts.data.tmp";
			int num = 0;
			do
			{
				num++;
				if (ApplyUpdate(patchInfo, text, text2, num) && CheckUpdateResult(patchInfo, text2))
				{
					LWLuaFileUtil.WriteSizeAndCrc(LWLuaFile.s_Workspace + "/LWScripts.txt", remoteSize, remoteCrc);
					flag = true;
					break;
				}
			}
			while (num < 5);
			if (flag)
			{
				Log.Info("[ParallelInit] [LWLuaFileUpdate] update succeed with " + downloadFilePath + ".");
				SwapFilePath(text, text2);
				File.Delete(text2);
				File.Delete(downloadFilePath);
			}
			else
			{
				Log.Error("[ParallelInit] [LWLuaFileUpdate] update failed with " + downloadFilePath + ".");
			}
		}
		return flag;
	}

	private static bool ApplyUpdate(PatchInfo patchInfo, string originalFile, string tmpOutputFile, int retryTime)
	{
		UpdateFlag updateFlag = patchInfo.updateFlag;
		string _downloadFilePath = patchInfo.downloadFilePath;
		_ = patchInfo.remoteSize;
		_ = patchInfo.remoteCrc;
		try
		{
			if (File.Exists(tmpOutputFile))
			{
				Log.Info($"[ParallelInit] [LWLuaFileUpdate] LWLuaFile delete tmp file {tmpOutputFile} [{retryTime}/{5}].");
				File.Delete(tmpOutputFile);
			}
		}
		catch (Exception arg)
		{
			Log.Error("[ParallelInit] [LWLuaFileUpdate] LWLuaFile delete tmp file Exception '{0}'.", arg);
		}
		bool result = false;
		switch (updateFlag)
		{
		case UpdateFlag.Replace:
			Log.Info($"[ParallelInit] [LWLuaFileUpdate] ApplyUpdate replace script file with {_downloadFilePath} [{retryTime}/{5}].");
			try
			{
				byte[] array7 = File.ReadAllBytes(_downloadFilePath);
				if (EncryptUtils.IsChachaPackage(array7))
				{
					EncryptUtils.FromChachaToBZip(array7);
					MemoryStream memoryStream8 = new MemoryStream(array7);
					MemoryStream memoryStream9 = new MemoryStream();
					BZip2.Decompress(memoryStream8, memoryStream9, isStreamOwner: false);
					memoryStream8.Dispose();
					byte[] array8 = memoryStream9.ToArray();
					memoryStream9.Dispose();
					MemoryStream memoryStream10 = new MemoryStream(array8, 0, array8.Length, writable: true, publiclyVisible: true);
					LWLuaFileUtil.EncodeLWLuaFileData(memoryStream10);
					FileStream fileStream3 = File.Open(tmpOutputFile, FileMode.Create, FileAccess.Write);
					memoryStream10.WriteTo(fileStream3);
					memoryStream10.Dispose();
					fileStream3.Dispose();
				}
				else
				{
					MemoryStream inStream = new MemoryStream(array7);
					FileStream outStream = File.Open(tmpOutputFile, FileMode.Create, FileAccess.Write);
					BZip2.Decompress(inStream, outStream, isStreamOwner: true);
				}
				result = true;
			}
			catch (Exception arg3)
			{
				Log.Error("[ParallelInit] [LWLuaFileUpdate] LWLuaFile ApplyUpdate Replace Exception '{0}'.", arg3);
			}
			break;
		case UpdateFlag.Patch:
			Log.Info($"[ParallelInit] [LWLuaFileUpdate] ApplyUpdate patch script file with {_downloadFilePath} [{retryTime}/{5}]");
			try
			{
				bool flag = false;
				bool flag2 = false;
				using (FileStream fileStream = File.OpenRead(_downloadFilePath))
				{
					byte[] array = new byte[8];
					fileStream.Read(array, 0, 8);
					flag2 = EncryptUtils.IsChachaPatch(array);
				}
				using (FileStream fileStream2 = File.OpenRead(originalFile))
				{
					int num = 8;
					byte[] array2 = new byte[num];
					fileStream2.Read(array2, 0, num);
					MemoryStream memoryStream = new MemoryStream(array2, 0, array2.Length, writable: true, publiclyVisible: true);
					flag = LWLuaFileUtil.IsEncodedLWLuaFile(memoryStream);
					memoryStream.Dispose();
				}
				if (flag && flag2)
				{
					byte[] patchBytes = File.ReadAllBytes(_downloadFilePath);
					EncryptUtils.FromChachaToBsDiffPatch(patchBytes);
					byte[] array3 = File.ReadAllBytes(originalFile);
					MemoryStream memoryStream2 = new MemoryStream(array3, 0, array3.Length, writable: true, publiclyVisible: true);
					LWLuaFileUtil.DecodeLWLuaFileData(memoryStream2);
					MemoryStream memoryStream3 = new MemoryStream();
					BinaryPatch.Apply(memoryStream2, () => new MemoryStream(patchBytes), memoryStream3);
					memoryStream2.Dispose();
					byte[] array4 = memoryStream3.ToArray();
					memoryStream3.Dispose();
					MemoryStream memoryStream4 = new MemoryStream(array4, 0, array4.Length, writable: true, publiclyVisible: true);
					LWLuaFileUtil.EncodeLWLuaFileData(memoryStream4);
					byte[] bytes = memoryStream4.ToArray();
					memoryStream4.Dispose();
					File.WriteAllBytes(tmpOutputFile, bytes);
					result = true;
					break;
				}
				if (flag && !flag2)
				{
					using (FileStream output = new FileStream(tmpOutputFile, FileMode.Create))
					{
						byte[] array5 = File.ReadAllBytes(originalFile);
						MemoryStream memoryStream5 = new MemoryStream(array5, 0, array5.Length, writable: true, publiclyVisible: true);
						LWLuaFileUtil.DecodeLWLuaFileData(memoryStream5);
						BinaryPatch.Apply(memoryStream5, () => new FileStream(_downloadFilePath, FileMode.Open, FileAccess.Read, FileShare.Read), output);
						memoryStream5.Dispose();
						result = true;
					}
					break;
				}
				if (!flag && flag2)
				{
					using (FileStream input = new FileStream(originalFile, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						byte[] patchBytes2 = File.ReadAllBytes(_downloadFilePath);
						EncryptUtils.FromChachaToBsDiffPatch(patchBytes2);
						MemoryStream memoryStream6 = new MemoryStream();
						BinaryPatch.Apply(input, () => new MemoryStream(patchBytes2), memoryStream6);
						byte[] array6 = memoryStream6.ToArray();
						memoryStream6.Dispose();
						MemoryStream memoryStream7 = new MemoryStream(array6, 0, array6.Length, writable: true, publiclyVisible: true);
						LWLuaFileUtil.EncodeLWLuaFileData(memoryStream7);
						byte[] bytes2 = memoryStream7.ToArray();
						memoryStream7.Dispose();
						File.WriteAllBytes(tmpOutputFile, bytes2);
						result = true;
					}
					break;
				}
				using FileStream input2 = new FileStream(originalFile, FileMode.Open, FileAccess.Read, FileShare.Read);
				using FileStream output2 = new FileStream(tmpOutputFile, FileMode.Create);
				BinaryPatch.Apply(input2, () => new FileStream(_downloadFilePath, FileMode.Open, FileAccess.Read, FileShare.Read), output2);
				result = true;
			}
			catch (Exception arg2)
			{
				Log.Error("[ParallelInit] [LWLuaFileUpdate] LWLuaFile ApplyUpdate Patch Exception '{0}'.", arg2);
			}
			break;
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

	private static bool CheckUpdateResult(PatchInfo patchInfo, string file)
	{
		_ = patchInfo.updateFlag;
		_ = patchInfo.downloadFilePath;
		ulong remoteSize = patchInfo.remoteSize;
		uint remoteCrc = patchInfo.remoteCrc;
		if (remoteSize != 0 || remoteCrc != 0)
		{
			LWLuaFileUtil.GetSizeAndCrc(file, out var size, out var crc);
			if (remoteSize != 0 && size != remoteSize)
			{
				Log.Error($"[ParallelInit] [LWLuaFileUpdate] update failed with file size: {remoteSize} != {size}.");
				return false;
			}
			if (remoteCrc != 0 && crc != remoteCrc)
			{
				Log.Error($"[ParallelInit] [LWLuaFileUpdate] update failed with file crc: {remoteCrc} != {crc}.");
				return false;
			}
			Log.Info("[ParallelInit] [LWLuaFileUpdate] pass update result check.");
		}
		else
		{
			Log.Info("[ParallelInit] [LWLuaFileUpdate] skip update result check.");
		}
		return true;
	}

	public static void th_LwScriptInit(object state)
	{
		Log.Info("[ParallelInit] th_LwScriptInit");
		try
		{
			string text = LWLuaFile.s_Workspace + "/LWScripts.data";
			string text2 = LWLuaFile.s_Workspace + "/LWScripts.txt";
			string fileName = string.Empty;
			ulong size = 0uL;
			uint crc = 0u;
			UpdateFlag updateFlag = LWLuaFileUpdate.HasUpdateFile(ref fileName, ref size, ref crc);
			if (updateFlag != UpdateFlag.None)
			{
				if (File.Exists(text))
				{
					ApplyUpdateOnStartup(updateFlag, fileName, size, crc);
				}
				LWLuaFileUpdate.ClearUpdateFile();
			}
			if (File.Exists(text))
			{
				if (File.Exists(text2))
				{
					LWLuaFileUtil.ReadSizeAndCrc(text2, out var size2, out var crc2);
					LWLuaFileUtil.GetSizeAndCrc(text, out var size3, out var crc3);
					if (size3 != size2 || crc3 != crc2)
					{
						Log.Info("[ParallelInit] [LWLuaFileUpdate] version file size or crc mismatch, delete scriptFile.");
						File.Delete(text);
					}
					else
					{
						Log.Info($"[ParallelInit] [LWLuaFileUpdate] version file size: {size2}, crc: {crc2}.");
					}
				}
				else
				{
					Log.Info("[ParallelInit] [LWLuaFileUpdate] version file not exist, delete scriptFile.");
					File.Delete(text);
				}
			}
			else
			{
				Log.Info("[ParallelInit] [LWLuaFileUpdate] scriptFile file is not exist.");
			}
		}
		catch (Exception arg)
		{
			Log.Error($"[ParallelInit] [LWLuaFileUpdate] th_LwScriptInit {arg}");
		}
		ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.LwScriptCopyFromPackage));
	}

	public static void mt_CopyLwScriptFileFromPackage(object state)
	{
		Log.Info("[ParallelInit] mt_CopyLwScriptFileFromPackage");
		_CopyLwScriptFileFromPackage(state);
		LWLuaFileUpdate.initSucceed = initSucceed;
		if (LWLuaFileUpdate.scriptFileMemory != null)
		{
			LWLuaFileUpdate.scriptFileMemory.Dispose();
		}
		LWLuaFileUpdate.scriptFileMemory = scriptFileMemory;
		ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.LwScriptLoad, new LwScriptLoadPayload
		{
			initSucceed = initSucceed,
			scriptFileMemory = scriptFileMemory
		}));
	}

	private static void _CopyLwScriptFileFromPackage(object state)
	{
		initSucceed = false;
		scriptFileMemory?.Dispose();
		scriptFileMemory = null;
		try
		{
			string text = LWLuaFile.s_Workspace + "/LWScripts.data";
			string filePath = LWLuaFile.s_Workspace + "/LWScripts.txt";
			string fileName = LWLuaFile.s_DataPath + "/LWScripts.bz2";
			string text2 = LWLuaFile.s_Workspace + "/LWScripts.bz2";
			if (!File.Exists(text))
			{
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				string fileName2 = LWLuaFile.s_DataPath + "/LWScripts.txt";
				ulong num = 0uL;
				uint num2 = 0u;
				if (StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.303") >= 0)
				{
					try
					{
						if (BuiltinFileReader.ReadyFileFromBuiltIn(fileName2, out var _, out var handler))
						{
							string[] array = handler.text.Split(new char[1] { '|' });
							num = ulong.Parse(array[3]);
							num2 = uint.Parse(array[4]);
							Log.Info($"[ParallelInit] [LWLuaFileUpdateV2] version file size: {num}, crc: {num2}.");
						}
						else
						{
							Log.Error("[ParallelInit] [LWLuaFileUpdateV2] read version file failed.");
						}
					}
					catch (Exception arg)
					{
						Log.Error($"[ParallelInit] [LWLuaFileUpdateV2] version file read has error, {arg}");
					}
					Log.Info("[ParallelInit] [LWLuaFileUpdateV2] copy uncompressed script file to " + text + ".");
					string fileName3 = LWLuaFile.s_DataPath + "/LWScripts.data";
					BuiltinFileReader.CopyFileFromBuiltIn(fileName3, text);
					ulong size = 0uL;
					uint crc = 0u;
					if (File.Exists(text))
					{
						LWLuaFileUtil.GetSizeAndCrc(text, out size, out crc);
						initSucceed = true;
						if (num != 0 && num != size)
						{
							Log.Error($"[ParallelInit] [LWLuaFileUpdateV2] script file check size {num} != {size}.");
							initSucceed = false;
						}
						if (num2 != 0 && num2 != crc)
						{
							Log.Error($"[ParallelInit] [LWLuaFileUpdateV2] script file check crc {num2} != {crc}.");
							initSucceed = false;
						}
					}
					string error2;
					DownloadHandler handler2;
					if (initSucceed)
					{
						Log.Info("[ParallelInit] [LWLuaFileUpdateV2] copy script file to " + text + ", initSucceed.");
						LWLuaFileUtil.WriteSizeAndCrc(filePath, size, crc);
					}
					else if (BuiltinFileReader.ReadyFileFromBuiltIn(fileName3, out error2, out handler2))
					{
						Log.Info("[ParallelInit] [LWLuaFileUpdateV2] copy script file to memory stream");
						scriptFileMemory = new MemoryStream(handler2.data);
					}
					else
					{
						Log.Error("[ParallelInit] [LWLuaFileUpdateV2] copy script file to memory stream");
					}
				}
				else if (StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.191") >= 0)
				{
					Log.Info("[ParallelInit] [LWLuaFileUpdate] copy compressed script file to " + text2 + ".");
					BuiltinFileReader.CopyFileFromBuiltIn(fileName, text2);
					if (!File.Exists(text2))
					{
						Log.Error("[ParallelInit] [LWLuaFileUpdate] copy builtin LWFile has error, isn`t exist.");
						return;
					}
					try
					{
						if (BuiltinFileReader.ReadyFileFromBuiltIn(fileName2, out var _, out var handler3))
						{
							string[] array2 = handler3.text.Split(new char[1] { '|' });
							num = ulong.Parse(array2[3]);
							num2 = uint.Parse(array2[4]);
							Log.Info($"[ParallelInit] [LWLuaFileUpdate] version file size: {num}, crc: {num2}.");
						}
						else
						{
							Log.Error("[ParallelInit] [LWLuaFileUpdate] read version file failed.");
						}
					}
					catch (Exception arg2)
					{
						Log.Error($"[ParallelInit] [LWLuaFileUpdate] version file read has error, {arg2}");
					}
					try
					{
						int num3 = 0;
						do
						{
							num3++;
							if (File.Exists(text))
							{
								Log.Info($"[ParallelInit] [LWLuaFileUpdate] delete decompress script file, [{num3}/{5}].");
								File.Delete(text);
							}
							if (num3 == 1)
							{
								Log.Info($"[ParallelInit] [LWLuaFileUpdate] copy uncompressed script file to {text}, [{num3}/{5}].");
								BuiltinFileReader.CopyFileFromBuiltIn(LWLuaFile.s_DataPath + "/LWScripts.data", text);
							}
							else
							{
								Log.Info($"[ParallelInit] [LWLuaFileUpdate] decompress script file to {text}, [{num3}/{5}].");
								byte[] array3 = File.ReadAllBytes(text2);
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
							ulong size2 = 0uL;
							uint crc2 = 0u;
							if (File.Exists(text))
							{
								LWLuaFileUtil.GetSizeAndCrc(text, out size2, out crc2);
								initSucceed = true;
								if (num != 0 && num != size2)
								{
									Log.Error($"[ParallelInit] [LWLuaFileUpdate] unzip script file check size {num} != {size2}, [{num3}/{5}].");
									initSucceed = false;
								}
								if (num2 != 0 && num2 != crc2)
								{
									Log.Error($"[ParallelInit] [LWLuaFileUpdate] unzip script file check crc {num2} != {crc2}, [{num3}/{5}].");
									initSucceed = false;
								}
							}
							if (initSucceed)
							{
								Log.Info("[ParallelInit] [LWLuaFileUpdate] decompress script file to " + text + ", initSucceed.");
								LWLuaFileUtil.WriteSizeAndCrc(filePath, size2, crc2);
								break;
							}
						}
						while (num3 < 5);
					}
					catch (Exception arg3)
					{
						Log.Error($"[ParallelInit] [LWLuaFileUpdate] {arg3}");
					}
					if (!initSucceed)
					{
						Log.Info("[ParallelInit] [LWLuaFileUpdate] decompress script file to memory stream");
						byte[] array5 = File.ReadAllBytes(text2);
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
					File.Delete(text2);
				}
				else
				{
					Log.Info("[ParallelInit] [LWLuaFileUpdate] skip uncompressed script file.");
					initSucceed = true;
				}
			}
			else
			{
				Log.Info("[ParallelInit] [LWLuaFileUpdate] script file exist, initSucceed.");
				initSucceed = true;
			}
		}
		catch (Exception arg4)
		{
			Log.Error($"[ParallelInit] [LWLuaFileUpdate] InitFileOnAppStart {arg4}");
		}
	}

	public static void th_LwScriptLoad(object state)
	{
		try
		{
			Log.Info("[ParallelInit] th_LwScriptLoad");
			LwScriptLoadPayload lwScriptLoadPayload = state as LwScriptLoadPayload;
			LWLuaFile lwLuaFile = LWLuaFile.Load(lwScriptLoadPayload.initSucceed, lwScriptLoadPayload.scriptFileMemory);
			ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.LwScriptSwap, new LwScriptSwapPayload
			{
				lwLuaFile = lwLuaFile
			}));
		}
		catch (Exception arg)
		{
			Log.Error($"[ParallelInit] [LWLuaFileUpdate] InitFileOnAppStart {arg}");
			ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.SetTaskDone, 4));
		}
	}

	public static void mt_LwScriptSwap(object state)
	{
		Log.Info("[ParallelInit] mt_LwScriptSwap");
		LwScriptSwapPayload swapLuaFile = state as LwScriptSwapPayload;
		GameEntry.Lua.SetSwapLuaFile(swapLuaFile);
	}
}
