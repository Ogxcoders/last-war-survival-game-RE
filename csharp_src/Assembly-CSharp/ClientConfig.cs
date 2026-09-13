using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using GameFramework.Localization;
using GameKit.Base;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using UnityEngine.Networking;

public class ClientConfig
{
	public class DataFileSelectPayload
	{
		public int maxVersion;

		public string maxVersionFileName = string.Empty;

		public string maxVersionFileMD5 = string.Empty;

		public bool hasDataFile;
	}

	public class LocaleFileSelectPayload
	{
		public int maxVersion;

		public bool hasLocaleFile;
	}

	public static readonly string VersionFileName = "version";

	public static int LWLuaVersion = 0;

	public static string TABLE_ENV_SAVE_KEY = "client_table_env";

	public static string LAST_ENV_ONLENE_KEY = "LAST_ENV_ONLENE_KEY";

	public const string LOG_APP_FIRST_INSTALLED_KEY = "log_app_first_installed";

	public static string TABLE_ENV_LOACL = "本地";

	public static string TABLE_ENV_DEV = "dev";

	public static string TABLE_ENV_TEST = "test";

	public static string TABLE_ENV_ONLINE = "online";

	public static string TABLE_ENV = "online";

	public static string CURRENT_TABLE_FILE_PATH = "";

	public static bool LocalMode = false;

	public static UpdateFlag Remote_Table_UpdateFlag = UpdateFlag.None;

	public static int Remote_Table_Version = 0;

	public static ulong Remote_Table_Size = 0uL;

	public static string Remote_Table_MD5 = "";

	public static uint Remote_Table_CRC = 0u;

	public static Dictionary<int, (ulong size, uint crc)> Remote_Table_Patches = new Dictionary<int, (ulong, uint)>();

	public static bool SKIP_UPDATE_TABLE_AND_LOCALIZATION = false;

	public const long STEP_UPDATE_ALL_TROOPS_TIME = 2L;

	public const int STEP_UPDATE_ALL_TROOPS_MIN_TASK = 4;

	public const long STEP_UPDATE_MARCH_TIME = 2L;

	public const int STEP_UPDATE_MARCH_MIN_TASK = 4;

	public const long STEP_UPDATE_MARCH_MESSAGE_TIME = 2L;

	public const int STEP_UPDATE_MARCH_MESSAGE_MIN_TASK = 1;

	public const int STEP_UPDATE_MARCH_MESSAGE_MAX_TASK = 500;

	public static readonly string FrontConfigHost = "https://lastwar-serverlist-us-cf-ali.lastwarapp.com/gameservice";

	public static readonly string BuildPath = "table";

	public static string PlayerDataPath = BuildPath ?? "";

	public static string DownloadPath = BuildPath ?? "";

	public const string BUILD_ID = "0";

	public static string USE_LOCAL_DIALOG_SAVE_KEY = "client_use_local_dialog";

	public static bool LOCALE_IN_PACKAGE = false;

	public static int LOCALE_VERSION = 0;

	public static string LOCALE_ABB;

	public static int LOCALE_VERSION_PACKAGE = 0;

	public static string LOCALE_ABB_PACKAGE;

	public static int REMOTE_LOCALE_VERSION = 0;

	public static HashSet<string> REMOTE_LOCALE_SUPPORT = new HashSet<string>();

	public static bool USE_DEV_LOCALE = false;

	public static readonly string DefaultLocaleAbb = "en";

	public static readonly string LocaleBuildPath = "locale";

	public static string LocalePlayerDataPath = LocaleBuildPath ?? "";

	public static string LocaleDownloadPath = LocaleBuildPath ?? "";

	public static int BUILTIN_TABLE_VERSION { get; set; }

	public static string BUILTIN_TABLE_MD5 { get; set; }

	public static int CURRENT_TABLE_VERSION { get; set; }

	public static string CURRENT_TABLE_MD5 { get; set; }

	public static string DownloadURL => ConstURLConfig.onlineDownloadURL_;

	public static string TableEnvName => "table_" + TABLE_ENV;

	public static string ReplaceLockFile => DownloadPath + "/replace.lock";

	public static bool MustHaveDataTable { get; private set; }

	public static bool MustHaveLocaleFile { get; set; }

	public static bool IsAppFirstInstalled
	{
		get
		{
			return PlayerPrefs.GetInt("log_app_first_installed", 1) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("log_app_first_installed", value ? 1 : 0);
		}
	}

	public static void th_CheckDataFile(object state)
	{
		try
		{
			Log.Info("[ParallelInit] th_CheckDataFile");
			if (!Directory.Exists(DownloadPath))
			{
				Directory.CreateDirectory(DownloadPath);
			}
			int num = 0;
			string text = string.Empty;
			string maxVersionFileMD = string.Empty;
			bool flag = false;
			string[] files = Directory.GetFiles(DownloadPath);
			int i = 0;
			for (int num2 = files.Length; i < num2; i++)
			{
				string fileName = Path.GetFileName(files[i]);
				if (fileName.StartsWith("table") && Path.GetExtension(fileName) == ".data")
				{
					string[] array = Path.GetFileNameWithoutExtension(fileName).Split(new char[1] { '_' });
					int num3 = int.Parse(array[1]);
					if (num3 > num)
					{
						text = fileName;
						maxVersionFileMD = array[2];
						num = num3;
					}
					flag = true;
				}
			}
			if (flag)
			{
				bool flag2 = false;
				try
				{
					string downloadDataPath = GetDownloadDataPath(text);
					using FileStream fileStream = File.OpenRead(downloadDataPath);
					byte[] array2 = new byte[8];
					fileStream.Read(array2, 0, 8);
					bool num4 = EncryptUtils.IsChachaTable(array2);
					fileStream.Seek(0L, SeekOrigin.Begin);
					if (num4)
					{
						byte[] array3 = File.ReadAllBytes(downloadDataPath);
						EncryptUtils.FromChachaToPKZip(array3);
						MemoryStream memoryStream = new MemoryStream(array3);
						if (memoryStream != null)
						{
							using (ZipFile zipFile = new ZipFile(memoryStream))
							{
								flag2 = zipFile.TestArchive(testData: true);
							}
							memoryStream.Dispose();
						}
					}
					else
					{
						using ZipFile zipFile2 = new ZipFile(fileStream);
						flag2 = zipFile2.TestArchive(testData: true);
					}
					if (flag2)
					{
						Log.Info("[ParallelInit] th_CheckDataFile TestArchive pass");
					}
					else
					{
						Log.Error("[ParallelInit] th_CheckDataFile TestArchive failed!");
					}
				}
				catch (Exception message)
				{
					Log.Error(message);
				}
				if (!flag2)
				{
					num = 0;
					text = string.Empty;
					maxVersionFileMD = string.Empty;
					flag = false;
					int j = 0;
					for (int num5 = files.Length; j < num5; j++)
					{
						try
						{
							File.Delete(files[j]);
						}
						catch (Exception ex)
						{
							Log.Error("[ParallelInit] th_CheckDataFile delete file error: " + files[j] + ", " + ex.Message);
						}
					}
				}
			}
			ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.DataFileSelect, new DataFileSelectPayload
			{
				maxVersion = num,
				maxVersionFileName = text,
				maxVersionFileMD5 = maxVersionFileMD,
				hasDataFile = flag
			}));
		}
		catch (Exception arg)
		{
			Log.Error($"[ParallelInit] th_CheckDataFile {arg}");
			ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.SetTaskDone, 1));
		}
	}

	public static void mt_SelectDataFile(object state)
	{
		Log.Info("[ParallelInit] mt_SelectDataFile");
		DataFileSelectPayload obj = state as DataFileSelectPayload;
		int maxVersion = obj.maxVersion;
		string maxVersionFileName = obj.maxVersionFileName;
		string maxVersionFileMD = obj.maxVersionFileMD5;
		bool hasDataFile = obj.hasDataFile;
		string error;
		DownloadHandler handler;
		bool flag = BuiltinFileReader.ReadyFileFromBuiltIn(PlayerDataPath + "/" + VersionFileName, out error, out handler);
		if (flag)
		{
			Log.Info("[ParallelInit] mt_SelectDataFile Builtin table data info " + handler.text);
			int version = 0;
			ulong size = 0uL;
			string md = string.Empty;
			SplitTableVersionInfo(handler.text, ref version, ref size, ref md);
			BUILTIN_TABLE_VERSION = version;
			BUILTIN_TABLE_MD5 = md;
		}
		else
		{
			Log.Error("[ParallelInit] mt_SelectDataFile Builtin table data error " + error);
		}
		if (!hasDataFile)
		{
			if (flag)
			{
				CURRENT_TABLE_VERSION = BUILTIN_TABLE_VERSION;
				CURRENT_TABLE_MD5 = BUILTIN_TABLE_MD5;
				string text = TableFileName(CURRENT_TABLE_VERSION, CURRENT_TABLE_MD5);
				CURRENT_TABLE_FILE_PATH = GetDownloadDataPath(text);
				if (!BuiltinFileReader.CopyFileFromBuiltIn(PlayerDataPath + "/" + text, CURRENT_TABLE_FILE_PATH))
				{
					CURRENT_TABLE_FILE_PATH = string.Empty;
				}
				MustHaveDataTable = !string.IsNullOrEmpty(CURRENT_TABLE_FILE_PATH) && File.Exists(CURRENT_TABLE_FILE_PATH);
				Log.Info("[ParallelInit] mt_SelectDataFile copy datatable from builtin " + PlayerDataPath + "/" + text);
			}
			else
			{
				CURRENT_TABLE_FILE_PATH = string.Empty;
				CURRENT_TABLE_VERSION = 0;
				CURRENT_TABLE_MD5 = string.Empty;
				MustHaveDataTable = false;
			}
			return;
		}
		CURRENT_TABLE_FILE_PATH = GetDownloadDataPath(maxVersionFileName);
		CURRENT_TABLE_VERSION = maxVersion;
		CURRENT_TABLE_MD5 = maxVersionFileMD;
		MustHaveDataTable = true;
		string[] files = Directory.GetFiles(DownloadPath);
		if (files.Length <= 1)
		{
			return;
		}
		int i = 0;
		for (int num = files.Length; i < num; i++)
		{
			string path = files[i];
			string fileName = Path.GetFileName(path);
			if (fileName.StartsWith("table") && Path.GetExtension(fileName) == ".data" && fileName != maxVersionFileName)
			{
				File.Delete(path);
			}
		}
	}

	public static void th_CheckLocaleFile(object state)
	{
		try
		{
			Language language = (Language)state;
			if (!Directory.Exists(LocaleDownloadPath))
			{
				Directory.CreateDirectory(LocaleDownloadPath);
			}
			LOCALE_ABB = LocalizationManager.GetLanguageName(language);
			int num = 0;
			bool flag = false;
			string[] directories = Directory.GetDirectories(LocaleDownloadPath);
			int i = 0;
			for (int num2 = directories.Length; i < num2; i++)
			{
				int num3 = int.Parse(Path.GetFileName(directories[i]));
				bool flag2 = File.Exists(GetLocaleDownloadDataPath(num3, LOCALE_ABB)) || File.Exists(GetLocaleBinDownloadDataPath(num3, LOCALE_ABB));
				if (num3 > num && flag2)
				{
					num = num3;
					flag = true;
				}
			}
			if (flag && File.Exists(GetLocaleBinDownloadDataPath(num, LOCALE_ABB)))
			{
				bool flag3 = false;
				FileStream fileStream = File.Open(GetLocaleBinDownloadDataPath(num, LOCALE_ABB), FileMode.Open, FileAccess.Read);
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					GZip.Decompress(fileStream, memoryStream, isStreamOwner: false);
					flag3 = true;
				}
				catch (Exception message)
				{
					Log.Error(message);
				}
				finally
				{
					fileStream.Close();
					memoryStream.Close();
				}
				if (!flag3)
				{
					num = 0;
					flag = false;
					int j = 0;
					for (int num4 = directories.Length; j < num4; j++)
					{
						Directory.Delete(directories[j], recursive: true);
					}
				}
			}
			ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.LocaleFileSelect, new LocaleFileSelectPayload
			{
				maxVersion = num,
				hasLocaleFile = flag
			}));
		}
		catch (Exception arg)
		{
			Log.Error($"[ParallelInit] th_CheckLocaleFile {arg}");
			ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.SetTaskDone, 2));
		}
	}

	public static void mt_SelectLocaleFile(object state)
	{
		Log.Info("[ParallelInit] mt_SelectLocaleFile");
		LocaleFileSelectPayload obj = state as LocaleFileSelectPayload;
		int maxVersion = obj.maxVersion;
		if (!obj.hasLocaleFile)
		{
			if (BuiltinFileReader.ReadyFileFromBuiltIn(LocalePlayerDataPath + "/" + VersionFileName, out var _, out var handler))
			{
				Log.Info("Builtin locale data info " + handler.text);
				HashSet<string> supported = new HashSet<string>();
				SplitLocaleVersionInfo(handler.text, ref LOCALE_VERSION, ref supported);
				LOCALE_IN_PACKAGE = true;
				if (!supported.Contains(LOCALE_ABB))
				{
					LOCALE_ABB = DefaultLocaleAbb;
				}
				MustHaveLocaleFile = true;
			}
			else
			{
				LOCALE_VERSION = 0;
				LOCALE_IN_PACKAGE = false;
				MustHaveLocaleFile = false;
			}
		}
		else
		{
			if (BuiltinFileReader.ReadyFileFromBuiltIn(LocalePlayerDataPath + "/" + VersionFileName, out var _, out var handler2))
			{
				Log.Info("in package data Builtin locale data info " + handler2.text);
				HashSet<string> supported2 = new HashSet<string>();
				SplitLocaleVersionInfo(handler2.text, ref LOCALE_VERSION_PACKAGE, ref supported2);
				LOCALE_ABB_PACKAGE = LOCALE_ABB;
				if (!supported2.Contains(LOCALE_ABB_PACKAGE))
				{
					LOCALE_ABB_PACKAGE = DefaultLocaleAbb;
				}
			}
			LOCALE_VERSION = maxVersion;
			LOCALE_IN_PACKAGE = false;
			MustHaveLocaleFile = true;
			string[] directories = Directory.GetDirectories(LocaleDownloadPath);
			if (directories.Length > 1)
			{
				int i = 0;
				for (int num = directories.Length; i < num; i++)
				{
					if (Path.GetFileName(directories[i]) != maxVersion.ToString())
					{
						Directory.Delete(directories[i], recursive: true);
					}
				}
			}
		}
		if (MustHaveLocaleFile)
		{
			if (LOCALE_IN_PACKAGE)
			{
				ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.LocaleFileLoadInPackage));
				return;
			}
			ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.LocaleFileLoadInPersistent, new LocaleFileParallel.LocaleFileLoadInPersistentPayload
			{
				locale_version = LOCALE_VERSION,
				locale_abb = LOCALE_ABB
			}));
		}
	}

	public static string GetDownloadDataPath(string file)
	{
		return DownloadPath + "/" + file;
	}

	public static string TableFileName(int version, string md5)
	{
		return $"table_{version}_{md5}.data";
	}

	public static string TableFilePatchName(int v1, int v2)
	{
		return $"table_{v1}_{v2}.patch";
	}

	public static void CreateTableFileReplaceLock()
	{
		if (!File.Exists(ReplaceLockFile))
		{
			File.Create(ReplaceLockFile).Close();
		}
	}

	public static void RemoveTableFileReplaceLock()
	{
		if (File.Exists(ReplaceLockFile))
		{
			File.Delete(ReplaceLockFile);
		}
	}

	public static bool HasTableFileReplaceLock()
	{
		return File.Exists(ReplaceLockFile);
	}

	public static string GetDataFileEnv()
	{
		return PlayerPrefs.GetString(TABLE_ENV_SAVE_KEY, TABLE_ENV);
	}

	public static void SetDataFileEnv(string env)
	{
		if (env == TABLE_ENV_LOACL)
		{
			string dataFileEnv = GetDataFileEnv();
			if (dataFileEnv != TABLE_ENV_LOACL)
			{
				PlayerPrefs.SetString(LAST_ENV_ONLENE_KEY, dataFileEnv);
			}
		}
		PlayerPrefs.SetString(TABLE_ENV_SAVE_KEY, env);
		PlayerPrefs.Save();
	}

	public static string GetLastOnlineDataFileEnv()
	{
		return PlayerPrefs.GetString(LAST_ENV_ONLENE_KEY, "dev");
	}

	public static void CheckDataFileEnv()
	{
		TABLE_ENV = PlayerPrefs.GetString(TABLE_ENV_SAVE_KEY, TABLE_ENV);
		LocalMode = string.IsNullOrEmpty(TABLE_ENV) || TABLE_ENV == TABLE_ENV_LOACL;
	}

	public static void SplitTableVersionInfo(string str, ref int version, ref ulong size, ref string md5)
	{
		string[] array = str.Split(new char[1] { ',' });
		version = int.Parse(array[0]);
		size = ulong.Parse(array[1]);
		md5 = array[2];
	}

	public static void ParseTableVersionInfo(string str)
	{
		Remote_Table_Patches.Clear();
		if (!str.Contains(";"))
		{
			string[] array = str.Split(new char[1] { ',' });
			int remote_Table_Version = int.Parse(array[0]);
			ulong remote_Table_Size = ulong.Parse(array[1]);
			string remote_Table_MD = array[2];
			Remote_Table_Version = remote_Table_Version;
			Remote_Table_Size = remote_Table_Size;
			Remote_Table_MD5 = remote_Table_MD;
			Remote_Table_CRC = 0u;
			return;
		}
		string[] array2 = str.Split(new char[1] { ';' });
		string[] array3 = array2[0].Split(new char[1] { ',' });
		int remote_Table_Version2 = int.Parse(array3[0]);
		ulong remote_Table_Size2 = ulong.Parse(array3[1]);
		string remote_Table_MD2 = array3[2];
		uint remote_Table_CRC = uint.Parse(array3[3]);
		Remote_Table_Version = remote_Table_Version2;
		Remote_Table_Size = remote_Table_Size2;
		Remote_Table_MD5 = remote_Table_MD2;
		Remote_Table_CRC = remote_Table_CRC;
		int num = array2.Length;
		if (num > 1)
		{
			for (int i = 1; i < num; i++)
			{
				string[] array4 = array2[i].Split(new char[1] { '|' });
				remote_Table_Version2 = int.Parse(array4[0]);
				remote_Table_Size2 = ulong.Parse(array4[1]);
				remote_Table_CRC = uint.Parse(array4[2]);
				Remote_Table_Patches.Add(remote_Table_Version2, (remote_Table_Size2, remote_Table_CRC));
			}
		}
	}

	public static void CheckDataFileOnAppStart()
	{
		if (LocalMode)
		{
			return;
		}
		if (!Directory.Exists(DownloadPath))
		{
			Directory.CreateDirectory(DownloadPath);
		}
		int num = 0;
		string text = string.Empty;
		string cURRENT_TABLE_MD = string.Empty;
		bool flag = false;
		string[] files = Directory.GetFiles(DownloadPath);
		int i = 0;
		for (int num2 = files.Length; i < num2; i++)
		{
			string fileName = Path.GetFileName(files[i]);
			if (fileName.StartsWith("table") && Path.GetExtension(fileName) == ".data")
			{
				string[] array = Path.GetFileNameWithoutExtension(fileName).Split(new char[1] { '_' });
				int num3 = int.Parse(array[1]);
				if (num3 > num)
				{
					text = fileName;
					cURRENT_TABLE_MD = array[2];
					num = num3;
				}
				flag = true;
			}
		}
		if (flag)
		{
			bool flag2 = false;
			try
			{
				string downloadDataPath = GetDownloadDataPath(text);
				using FileStream fileStream = File.OpenRead(downloadDataPath);
				byte[] array2 = new byte[8];
				fileStream.Read(array2, 0, 8);
				bool num4 = EncryptUtils.IsChachaTable(array2);
				fileStream.Seek(0L, SeekOrigin.Begin);
				if (num4)
				{
					byte[] array3 = File.ReadAllBytes(downloadDataPath);
					EncryptUtils.FromChachaToPKZip(array3);
					MemoryStream memoryStream = new MemoryStream(array3);
					if (memoryStream != null)
					{
						using (ZipFile zipFile = new ZipFile(memoryStream))
						{
							flag2 = zipFile.TestArchive(testData: true);
						}
						memoryStream.Dispose();
					}
				}
				else
				{
					using ZipFile zipFile2 = new ZipFile(fileStream);
					flag2 = zipFile2.TestArchive(testData: true);
				}
				if (flag2)
				{
					Log.Info("CheckDataFileOnAppStart TestArchive pass.");
					ApplicationLaunch.StepLog("CheckDataFileOnAppStart TestArchive pass");
				}
				else
				{
					Log.Error("CheckDataFileOnAppStart TestArchive failed!");
				}
			}
			catch (Exception message)
			{
				Log.Error(message);
			}
			if (!flag2)
			{
				num = 0;
				text = string.Empty;
				cURRENT_TABLE_MD = string.Empty;
				flag = false;
				int j = 0;
				for (int num5 = files.Length; j < num5; j++)
				{
					try
					{
						File.Delete(files[j]);
					}
					catch (Exception ex)
					{
						Log.Error("CheckDataFileOnAppStart delete file error: " + files[j] + ", " + ex.Message);
					}
				}
			}
		}
		string error;
		DownloadHandler handler;
		bool flag3 = BuiltinFileReader.ReadyFileFromBuiltIn(PlayerDataPath + "/" + VersionFileName, out error, out handler);
		if (flag3)
		{
			Log.Info("Builtin table data info " + handler.text);
			int version = 0;
			ulong size = 0uL;
			string md = string.Empty;
			SplitTableVersionInfo(handler.text, ref version, ref size, ref md);
			BUILTIN_TABLE_VERSION = version;
			BUILTIN_TABLE_MD5 = md;
		}
		else
		{
			Log.Error("Builtin table data error " + error);
		}
		if (!flag)
		{
			if (flag3)
			{
				CURRENT_TABLE_VERSION = BUILTIN_TABLE_VERSION;
				CURRENT_TABLE_MD5 = BUILTIN_TABLE_MD5;
				string text2 = TableFileName(CURRENT_TABLE_VERSION, CURRENT_TABLE_MD5);
				CURRENT_TABLE_FILE_PATH = GetDownloadDataPath(text2);
				if (!BuiltinFileReader.CopyFileFromBuiltIn(PlayerDataPath + "/" + text2, CURRENT_TABLE_FILE_PATH))
				{
					CURRENT_TABLE_FILE_PATH = string.Empty;
				}
				MustHaveDataTable = !string.IsNullOrEmpty(CURRENT_TABLE_FILE_PATH) && File.Exists(CURRENT_TABLE_FILE_PATH);
			}
			else
			{
				CURRENT_TABLE_FILE_PATH = string.Empty;
				CURRENT_TABLE_VERSION = 0;
				CURRENT_TABLE_MD5 = string.Empty;
				MustHaveDataTable = false;
			}
			return;
		}
		CURRENT_TABLE_FILE_PATH = GetDownloadDataPath(text);
		CURRENT_TABLE_VERSION = num;
		CURRENT_TABLE_MD5 = cURRENT_TABLE_MD;
		MustHaveDataTable = true;
		if (files.Length <= 1)
		{
			return;
		}
		int k = 0;
		for (int num6 = files.Length; k < num6; k++)
		{
			string path = files[k];
			string fileName2 = Path.GetFileName(path);
			if (fileName2.StartsWith("table") && Path.GetExtension(fileName2) == ".data" && fileName2 != text)
			{
				File.Delete(path);
			}
		}
	}

	public static void DeleteOldTableFile()
	{
		string[] files = Directory.GetFiles(DownloadPath);
		string text = TableFileName(CURRENT_TABLE_VERSION, CURRENT_TABLE_MD5);
		int i = 0;
		for (int num = files.Length; i < num; i++)
		{
			string path = files[i];
			string fileName = Path.GetFileName(path);
			if (fileName.StartsWith("table") && Path.GetExtension(fileName) == ".data" && fileName != text)
			{
				File.Delete(path);
			}
		}
	}

	public static string GetLocaleDownloadDataPath(int version, string localeAbb)
	{
		return $"{LocaleDownloadPath}/{version}/{localeAbb}.bytes";
	}

	public static string LocaleDownloadURL(int version, string localeAbb)
	{
		return $"{ConstURLConfig.TableDownloadURL}locale/{version}/{localeAbb}.bytes";
	}

	public static string LocaleFileName(int version, string localeAbb)
	{
		return $"{version}/{localeAbb}.bytes";
	}

	public static string GetLocaleBinDownloadDataPath(int version, string localeAbb)
	{
		return $"{LocaleDownloadPath}/{version}/{localeAbb}.bin";
	}

	public static string LocaleBinDownloadURL(int version, string localeAbb)
	{
		return $"{ConstURLConfig.TableDownloadURL}locale/{version}/{localeAbb}.bin";
	}

	public static string GetDevLocaleBinDownloadDataPath(string localeAbb)
	{
		return LocaleDownloadPath + "_debug/" + localeAbb + ".bin";
	}

	public static string LocaleBinFileName(int version, string localeAbb)
	{
		return $"{version}/{localeAbb}.bin";
	}

	public static void SplitLocaleVersionInfo(string str, ref int version, ref HashSet<string> supported)
	{
		string[] array = str.Split(new char[1] { ',' });
		version = int.Parse(array[0]);
		supported.Clear();
		int i = 1;
		for (int num = array.Length; i < num; i++)
		{
			supported.Add(array[i]);
		}
	}

	public static bool UseLocalDialog()
	{
		return PlayerPrefs.GetInt(USE_LOCAL_DIALOG_SAVE_KEY, 0) == 1;
	}

	public static bool UseLocalCrowdinDev()
	{
		return PlayerPrefs.GetInt(USE_LOCAL_DIALOG_SAVE_KEY, 0) == 2;
	}

	public static int GetUseLocalValue()
	{
		return PlayerPrefs.GetInt(USE_LOCAL_DIALOG_SAVE_KEY, 0);
	}

	public static void SetUseLocalDialog(int use)
	{
		PlayerPrefs.SetInt(USE_LOCAL_DIALOG_SAVE_KEY, use);
		PlayerPrefs.Save();
	}

	public static void CheckLocaleFileOnAppStart(Language language)
	{
		if (!Directory.Exists(LocaleDownloadPath))
		{
			Directory.CreateDirectory(LocaleDownloadPath);
		}
		LOCALE_ABB = LocalizationManager.GetLanguageName(language);
		int num = 0;
		bool flag = false;
		string[] directories = Directory.GetDirectories(LocaleDownloadPath);
		int i = 0;
		for (int num2 = directories.Length; i < num2; i++)
		{
			int num3 = int.Parse(Path.GetFileName(directories[i]));
			bool flag2 = File.Exists(GetLocaleDownloadDataPath(num3, LOCALE_ABB)) || File.Exists(GetLocaleBinDownloadDataPath(num3, LOCALE_ABB));
			if (num3 > num && flag2)
			{
				num = num3;
				flag = true;
			}
		}
		if (flag && File.Exists(GetLocaleBinDownloadDataPath(num, LOCALE_ABB)))
		{
			bool flag3 = false;
			FileStream fileStream = File.Open(GetLocaleBinDownloadDataPath(num, LOCALE_ABB), FileMode.Open, FileAccess.Read);
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				GZip.Decompress(fileStream, memoryStream, isStreamOwner: false);
				flag3 = true;
			}
			catch (Exception message)
			{
				Log.Error(message);
			}
			finally
			{
				fileStream.Close();
				memoryStream.Close();
			}
			if (!flag3)
			{
				num = 0;
				flag = false;
				int j = 0;
				for (int num4 = directories.Length; j < num4; j++)
				{
					Directory.Delete(directories[j], recursive: true);
				}
			}
		}
		if (!flag)
		{
			if (BuiltinFileReader.ReadyFileFromBuiltIn(LocalePlayerDataPath + "/" + VersionFileName, out var _, out var handler))
			{
				Log.Info("Builtin locale data info " + handler.text);
				HashSet<string> supported = new HashSet<string>();
				SplitLocaleVersionInfo(handler.text, ref LOCALE_VERSION, ref supported);
				LOCALE_IN_PACKAGE = true;
				if (!supported.Contains(LOCALE_ABB))
				{
					LOCALE_ABB = DefaultLocaleAbb;
				}
				MustHaveLocaleFile = true;
			}
			else
			{
				LOCALE_VERSION = 0;
				LOCALE_IN_PACKAGE = false;
				MustHaveLocaleFile = false;
			}
			return;
		}
		if (BuiltinFileReader.ReadyFileFromBuiltIn(LocalePlayerDataPath + "/" + VersionFileName, out var _, out var handler2))
		{
			Log.Info("in package data Builtin locale data info " + handler2.text);
			HashSet<string> supported2 = new HashSet<string>();
			SplitLocaleVersionInfo(handler2.text, ref LOCALE_VERSION_PACKAGE, ref supported2);
			LOCALE_ABB_PACKAGE = LOCALE_ABB;
			if (!supported2.Contains(LOCALE_ABB_PACKAGE))
			{
				LOCALE_ABB_PACKAGE = DefaultLocaleAbb;
			}
		}
		LOCALE_VERSION = num;
		LOCALE_IN_PACKAGE = false;
		MustHaveLocaleFile = true;
		if (directories.Length <= 1)
		{
			return;
		}
		int k = 0;
		for (int num5 = directories.Length; k < num5; k++)
		{
			if (Path.GetFileName(directories[k]) != num.ToString())
			{
				Directory.Delete(directories[k], recursive: true);
			}
		}
	}

	public static void ConfigPath()
	{
		PlayerDataPath = Application.streamingAssetsPath + "/" + BuildPath;
		DownloadPath = Application.persistentDataPath + "/" + BuildPath;
		LocalePlayerDataPath = Application.streamingAssetsPath + "/" + LocaleBuildPath;
		LocaleDownloadPath = Application.persistentDataPath + "/" + LocaleBuildPath;
	}
}
