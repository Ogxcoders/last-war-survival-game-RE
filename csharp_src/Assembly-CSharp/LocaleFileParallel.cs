using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using ICSharpCode.SharpZipLib.GZip;

public class LocaleFileParallel
{
	public class LocaleFileLoadInPersistentPayload
	{
		public int locale_version;

		public string locale_abb;

		public bool isReload;
	}

	public class LocaleFileSwapPayload
	{
		public bool isInitSuccess;

		public Dictionary<string, string> dictionaryRaw;

		public Dictionary<string, LocalizationManager.DialogEntry> dictionary;

		public int locale_version;

		public string locale_abb;

		public bool isReload;
	}

	public static bool ParseTxtDictionary(Dictionary<string, LocalizationManager.DialogEntry> dictionary, string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		dictionary.Clear();
		try
		{
			StringExtensions.LineSplitEnumerator enumerator = text.SplitLines().GetEnumerator();
			while (enumerator.MoveNext())
			{
				ReadOnlySpan<char> span = enumerator.Current;
				if (span.Length > 2 && span[0] != '#' && span[0] != '/' && span.Split_to_spanspan('=', out var span2, out var span3))
				{
					span2 = span2.Trim();
					span3 = span3.Trim();
					if (!span2.IsEmpty && !span3.IsEmpty)
					{
						string key = span2.ToString();
						string originDlg = span3.ToString();
						LocalizationManager.DialogEntry dialogEntry = new LocalizationManager.DialogEntry();
						dialogEntry.originDlg = originDlg;
						dialogEntry.hasCRLF = true;
						dictionary[key] = dialogEntry;
					}
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error("Can not parse dictionary '{0}' with exception '{1}'.", text, $"{ex.Message}\n{ex.StackTrace}");
			return false;
		}
	}

	private static bool ParseBinDictionary(Dictionary<string, string> dictionary, MemoryStream memoryStream)
	{
		if (memoryStream.Length <= 0)
		{
			return false;
		}
		dictionary.Clear();
		try
		{
			memoryStream.Seek(0L, SeekOrigin.Begin);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			binaryReader.ReadUInt32();
			while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
			{
				string key = binaryReader.ReadString();
				string value = binaryReader.ReadString();
				dictionary[key] = value;
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error("Can not parse bin dictionary with exception '{0}'.", $"{ex.Message}\n{ex.StackTrace}");
			return false;
		}
	}

	private static bool LoadDictionaryFromStream(Stream inStream, Dictionary<string, LocalizationManager.DialogEntry> dictionary)
	{
		bool result = false;
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			GZip.Decompress(inStream, memoryStream, isStreamOwner: false);
			string text = Encoding.UTF8.GetString(memoryStream.ToArray());
			result = ParseTxtDictionary(dictionary, text);
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		memoryStream.Close();
		return result;
	}

	private static bool LoadDictionaryFromStreamBin(Stream inStream, Dictionary<string, string> dictionaryRaw)
	{
		bool result = false;
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			GZip.Decompress(inStream, memoryStream, isStreamOwner: false);
			result = ParseBinDictionary(dictionaryRaw, memoryStream);
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		memoryStream.Close();
		return result;
	}

	public static void th_LoadLocaleFileInPersistent(object state)
	{
		try
		{
			Log.Info("[ParallelInit] th_LoadLocaleFileInPersistent");
			LocaleFileLoadInPersistentPayload obj = state as LocaleFileLoadInPersistentPayload;
			int locale_version = obj.locale_version;
			string locale_abb = obj.locale_abb;
			bool isReload = obj.isReload;
			bool flag = false;
			Dictionary<string, string> dictionaryRaw = new Dictionary<string, string>();
			Dictionary<string, LocalizationManager.DialogEntry> dictionary = new Dictionary<string, LocalizationManager.DialogEntry>();
			string localeBinDownloadDataPath = ClientConfig.GetLocaleBinDownloadDataPath(locale_version, locale_abb);
			if (File.Exists(localeBinDownloadDataPath))
			{
				FileStream fileStream = File.Open(localeBinDownloadDataPath, FileMode.Open, FileAccess.Read);
				flag = LoadDictionaryFromStreamBin(fileStream, dictionaryRaw);
				if (flag)
				{
					Log.Info($"[ParallelInit] th_LoadLocaleFileInPersistent LoadDictionaryFromStreamBin succeed, {locale_version}, {locale_abb}");
				}
				else
				{
					Log.Info($"[ParallelInit] th_LoadLocaleFileInPersistent LoadDictionaryFromStreamBin failed, {locale_version}, {locale_abb}");
				}
				fileStream.Close();
			}
			else
			{
				string localeDownloadDataPath = ClientConfig.GetLocaleDownloadDataPath(locale_version, locale_abb);
				if (File.Exists(localeDownloadDataPath))
				{
					FileStream fileStream2 = File.Open(localeDownloadDataPath, FileMode.Open, FileAccess.Read);
					flag = LoadDictionaryFromStream(fileStream2, dictionary);
					if (flag)
					{
						Log.Info($"[ParallelInit] th_LoadLocaleFileInPersistent LoadDictionaryFromStream succeed from persistent {locale_version}, {locale_abb}");
					}
					else
					{
						Log.Info($"[ParallelInit] th_LoadLocaleFileInPersistent LoadDictionaryFromStream failed from persistent {locale_version}, {locale_abb}");
					}
					fileStream2.Close();
				}
				else
				{
					Log.Error($"[ParallelInit] th_LoadLocaleFileInPersistent can`t load locale file, {locale_version}, {locale_abb}");
				}
			}
			if (flag)
			{
				ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.LocaleFileSwap, new LocaleFileSwapPayload
				{
					isInitSuccess = true,
					dictionaryRaw = dictionaryRaw,
					dictionary = dictionary,
					locale_version = locale_version,
					locale_abb = locale_abb,
					isReload = isReload
				}));
			}
			else if (!isReload)
			{
				ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.LocaleFileLoadInPackage));
			}
		}
		catch (Exception arg)
		{
			Log.Error($"[ParallelInit] th_LoadLocaleFileInPersistent {arg}");
			ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.SetTaskDone, 2));
		}
	}

	public static void mt_LoadLocaleFileInPackage(object state)
	{
		Log.Info("[ParallelInit] mt_LoadLocaleFileInPackage");
		bool flag = false;
		Dictionary<string, string> dictionaryRaw = new Dictionary<string, string>();
		Dictionary<string, LocalizationManager.DialogEntry> dictionary = new Dictionary<string, LocalizationManager.DialogEntry>();
		string text = ClientConfig.LocaleBinFileName(ClientConfig.LOCALE_VERSION, ClientConfig.LOCALE_ABB);
		if (BuiltinFileReader.ReadyFileFromBuiltIn(ClientConfig.LocalePlayerDataPath + "/" + text, out var _, out var handler))
		{
			MemoryStream memoryStream = new MemoryStream(handler.data);
			flag = LoadDictionaryFromStreamBin(memoryStream, dictionaryRaw);
			if (flag)
			{
				Log.Info($"[ParallelInit] mt_LoadLocaleFileInPackage LoadDictionaryFromStreamBin succeed, {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
			}
			else
			{
				Log.Info($"[ParallelInit] mt_LoadLocaleFileInPackage LoadDictionaryFromStreamBin failed, {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
			}
			memoryStream.Close();
		}
		else
		{
			string text2 = ClientConfig.LocaleFileName(ClientConfig.LOCALE_VERSION, ClientConfig.LOCALE_ABB);
			if (BuiltinFileReader.ReadyFileFromBuiltIn(ClientConfig.LocalePlayerDataPath + "/" + text2, out var error2, out var handler2))
			{
				MemoryStream memoryStream2 = new MemoryStream(handler2.data);
				flag = LoadDictionaryFromStream(memoryStream2, dictionary);
				if (flag)
				{
					Log.Info($"[ParallelInit] mt_LoadLocaleFileInPackage LoadDictionaryFromStream succeed, {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
				}
				else
				{
					Log.Info($"[ParallelInit] mt_LoadLocaleFileInPackage LoadDictionaryFromStream failed, {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
				}
				memoryStream2.Close();
			}
			else
			{
				Log.Error($"[ParallelInit] mt_LoadLocaleFileInPackage can`t load locale file, {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}, {error2}");
			}
		}
		ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.LocaleFileSwap, new LocaleFileSwapPayload
		{
			isInitSuccess = flag,
			dictionaryRaw = dictionaryRaw,
			dictionary = dictionary,
			locale_version = ClientConfig.LOCALE_VERSION,
			locale_abb = ClientConfig.LOCALE_ABB
		}));
	}

	public static void mt_LocaleFileSwap(object state)
	{
		Log.Info("[ParallelInit] mt_LocaleFileSwap");
		LocaleFileSwapPayload localeFileSwapPayload = state as LocaleFileSwapPayload;
		if (localeFileSwapPayload.isReload)
		{
			GameEntry.Localization.SetSwapLocaleFile(localeFileSwapPayload);
			GameEntry.Localization.UseSwapLocaleFile();
			ClientConfig.LOCALE_VERSION = localeFileSwapPayload.locale_version;
			ClientConfig.LOCALE_ABB = localeFileSwapPayload.locale_abb;
			ClientConfig.LOCALE_IN_PACKAGE = false;
			ClientConfig.MustHaveLocaleFile = true;
		}
		else
		{
			GameEntry.Localization.SetSwapLocaleFile(localeFileSwapPayload);
		}
	}
}
