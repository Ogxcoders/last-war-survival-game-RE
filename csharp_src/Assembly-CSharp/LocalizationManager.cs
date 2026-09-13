using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FibMatrix;
using GameFramework;
using GameFramework.Localization;
using ICSharpCode.SharpZipLib.GZip;
using TMPro;
using ThinkingAnalytics;
using UnityEngine;
using VEngine;
using XLua;

public class LocalizationManager
{
	public class DialogEntry
	{
		public string originDlg;

		public bool hasCRLF;
	}

	private class CacheEntry
	{
		public string cacheDlg;

		public object[] cacheParams;
	}

	private static readonly string[] ColumnSplit = new string[1] { "=" };

	private const int ColumnCount = 2;

	private Dictionary<string, DialogEntry> m_Dictionary = new Dictionary<string, DialogEntry>();

	private Dictionary<string, string> m_DictionaryRaw = new Dictionary<string, string>();

	private readonly Dictionary<string, CacheEntry> m_CacheDict = new Dictionary<string, CacheEntry>();

	private Language m_Language;

	private LocaleFileParallel.LocaleFileSwapPayload _swapPayload;

	private List<Language> SuportedLanguages = new List<Language>
	{
		Language.English,
		Language.ChineseSimplified,
		Language.Arabic
	};

	private Dictionary<string, string> m_DictionarySkin;

	private Dictionary<Language, List<(string, string)>> _fontLanguages = new Dictionary<Language, List<(string, string)>>
	{
		{
			Language.Japanese,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "MPLUSRounded1c-Medium SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "MPLUSRounded1c-Medium SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "MPLUSRounded1c-Medium SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "MPLUSRounded1c-Medium SDF")
			}
		},
		{
			Language.Russian,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "Tilda-Sans_Extra-Bold SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "Tilda-Sans_Extra-Bold SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "Tilda-Sans_Extra-Bold SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "Tilda-Sans_Extra-Bold SDF")
			}
		},
		{
			Language.Korean,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "SDGothicNeoaUniTTF-Chat SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "SDGothicNeoaUniTTF-eMd SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "SDNemony2cBasicRg SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "SDGothicNeoaUniTTF-hEb SDF")
			}
		},
		{
			Language.ChineseSimplified,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "NotoSans_SC_TC-Medium SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "NotoSans_SC_TC-Medium SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "NotoSans_SC_TC-Medium SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "NotoSans_SC_TC-Medium SDF")
			}
		},
		{
			Language.ChineseTraditional,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "NotoSans_SC_TC-Medium SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "NotoSans_SC_TC-Medium SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "NotoSans_SC_TC-Medium SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "NotoSans_SC_TC-Medium SDF")
			}
		},
		{
			Language.Thai,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "MN Tangme SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "MN Tangme SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "MN ADVENTURE Light SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "MN Mahesuan SDF")
			}
		},
		{
			Language.Turkish,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "NotoSansSC-Bold SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "GemunuLibre-ExtraBold SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "Saira-Black SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "Saira-Black SDF")
			}
		},
		{
			Language.German,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "Blinker-Bold SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "Blinker-Bold SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "Blinker-Bold SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "Blinker-Bold SDF")
			}
		},
		{
			Language.PortuguesePortugal,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "Blinker-Bold SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "Blinker-Bold SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "Blinker-Bold SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "Blinker-Bold SDF")
			}
		},
		{
			Language.Vietnamese,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "Blinker-Bold SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "ProtestStrike-Regular SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "ProtestStrike-Regular SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "ProtestStrike-Regular SDF")
			}
		},
		{
			Language.Polish,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "Roboto-ExtraBold SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "Roboto-ExtraBold SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "Roboto-ExtraBold SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "Roboto-ExtraBold SDF")
			}
		},
		{
			Language.Arabic,
			new List<(string, string)>
			{
				("Assets/Main/TMPFont/Main/Chat_SDF.asset", "NotoKufiArabic-Regular SDF"),
				("Assets/Main/TMPFont/Main/Body.asset", "NotoKufiArabic-Regular SDF"),
				("Assets/Main/TMPFont/Main/Chapter.asset", "NotoKufiArabic-Medium SDF"),
				("Assets/Main/TMPFont/Main/Title.asset", "NotoKufiArabic-Medium SDF")
			}
		}
	};

	private List<(string, string)> _commonLanguage = new List<(string, string)>
	{
		("Assets/Main/TMPFont/Main/Chat_SDF.asset", "NotoSansSC-Bold SDF"),
		("Assets/Main/TMPFont/Main/Body.asset", "GemunuLibre-ExtraBold SDF"),
		("Assets/Main/TMPFont/Main/Chapter.asset", "Bangers-Regular SDF"),
		("Assets/Main/TMPFont/Main/Title.asset", "Saira-Black SDF")
	};

	private List<Asset> fontAssetsCache = new List<Asset>();

	private List<string> _needInitFonts = new List<string>
	{
		"Assets/Main/TMPFont/Sub/Chat/NotoSansSC-Bold SDF.asset", "Assets/Main/TMPFont/Sub/CK/NotoSans_SC_TC-Medium SDF.asset", "Assets/Main/TMPFont/Sub/JP/MPLUSRounded1c-Medium SDF.asset", "Assets/Main/TMPFont/Sub/KR/SDGothicNeoaUniTTF-eMd SDF.asset", "Assets/Main/TMPFont/Sub/KR/SDGothicNeoaUniTTF-hEb SDF.asset", "Assets/Main/TMPFont/Sub/KR/SDNemony2cBasicRg SDF.asset", "Assets/Main/TMPFont/Sub/Ru/Tilda-Sans_Extra-Bold SDF.asset", "Assets/Main/TMPFont/Sub/De/Blinker-Bold SDF.asset", "Assets/Main/TMPFont/Sub/VI/ProtestStrike-Regular SDF.asset", "Assets/Main/TMPFont/Sub/PO/Roboto-ExtraBold SDF.asset",
		"Assets/Main/TMPFont/Sub/Ar/NotoKufiArabic-Regular SDF.asset", "Assets/Main/TMPFont/Sub/Ar/NotoKufiArabic-Medium SDF.asset"
	};

	private Dictionary<string, Dictionary<uint, TMP_Character>> _removedCharactersPerFont = new Dictionary<string, Dictionary<uint, TMP_Character>>();

	private static readonly Dictionary<Language, bool> LanguageFontMap = new Dictionary<Language, bool>
	{
		{
			Language.German,
			true
		},
		{
			Language.PortuguesePortugal,
			true
		},
		{
			Language.Vietnamese,
			true
		},
		{
			Language.Polish,
			true
		}
	};

	public bool IsInitDone { get; private set; }

	public bool IsInitSuccess { get; private set; }

	public bool ShowKey { get; set; }

	public Language Language
	{
		get
		{
			return m_Language;
		}
		set
		{
			if (value == Language.Unspecified)
			{
				Log.Error("Language is invalid.");
			}
			m_Language = value;
			ThinkingAnalyticsAPI.UserSet(new Dictionary<string, object> { { "lwu_language", m_Language } });
		}
	}

	public static Language SystemLanguage
	{
		get
		{
			switch (Application.systemLanguage)
			{
			case UnityEngine.SystemLanguage.Afrikaans:
				return Language.Afrikaans;
			case UnityEngine.SystemLanguage.Arabic:
				return Language.Arabic;
			case UnityEngine.SystemLanguage.Basque:
				return Language.Basque;
			case UnityEngine.SystemLanguage.Belarusian:
				return Language.Belarusian;
			case UnityEngine.SystemLanguage.Bulgarian:
				return Language.Bulgarian;
			case UnityEngine.SystemLanguage.Catalan:
				return Language.Catalan;
			case UnityEngine.SystemLanguage.Chinese:
				return Language.ChineseSimplified;
			case UnityEngine.SystemLanguage.ChineseSimplified:
				return Language.ChineseTraditional;
			case UnityEngine.SystemLanguage.ChineseTraditional:
				return Language.ChineseTraditional;
			case UnityEngine.SystemLanguage.Czech:
				return Language.Czech;
			case UnityEngine.SystemLanguage.Danish:
				return Language.Danish;
			case UnityEngine.SystemLanguage.Dutch:
				return Language.Dutch;
			case UnityEngine.SystemLanguage.English:
				return Language.English;
			case UnityEngine.SystemLanguage.Estonian:
				return Language.Estonian;
			case UnityEngine.SystemLanguage.Faroese:
				return Language.Faroese;
			case UnityEngine.SystemLanguage.Finnish:
				return Language.Finnish;
			case UnityEngine.SystemLanguage.French:
				return Language.French;
			case UnityEngine.SystemLanguage.German:
				return Language.German;
			case UnityEngine.SystemLanguage.Greek:
				return Language.Greek;
			case UnityEngine.SystemLanguage.Hebrew:
				return Language.Hebrew;
			case UnityEngine.SystemLanguage.Hungarian:
				return Language.Hungarian;
			case UnityEngine.SystemLanguage.Icelandic:
				return Language.Icelandic;
			case UnityEngine.SystemLanguage.Indonesian:
				return Language.Indonesian;
			case UnityEngine.SystemLanguage.Italian:
				return Language.Italian;
			case UnityEngine.SystemLanguage.Japanese:
				return Language.Japanese;
			case UnityEngine.SystemLanguage.Korean:
				return Language.Korean;
			case UnityEngine.SystemLanguage.Latvian:
				return Language.Latvian;
			case UnityEngine.SystemLanguage.Lithuanian:
				return Language.Lithuanian;
			case UnityEngine.SystemLanguage.Norwegian:
				return Language.Norwegian;
			case UnityEngine.SystemLanguage.Polish:
				return Language.Polish;
			case UnityEngine.SystemLanguage.Portuguese:
				return Language.PortuguesePortugal;
			case UnityEngine.SystemLanguage.Romanian:
				return Language.Romanian;
			case UnityEngine.SystemLanguage.Russian:
				return Language.Russian;
			case UnityEngine.SystemLanguage.SerboCroatian:
				return Language.SerboCroatian;
			case UnityEngine.SystemLanguage.Slovak:
				return Language.Slovak;
			case UnityEngine.SystemLanguage.Slovenian:
				return Language.Slovenian;
			case UnityEngine.SystemLanguage.Spanish:
				return Language.Spanish;
			case UnityEngine.SystemLanguage.Swedish:
				return Language.Swedish;
			case UnityEngine.SystemLanguage.Thai:
				return Language.Thai;
			case UnityEngine.SystemLanguage.Turkish:
				return Language.Turkish;
			case UnityEngine.SystemLanguage.Ukrainian:
				return Language.Ukrainian;
			case UnityEngine.SystemLanguage.Unknown:
				return Language.Unspecified;
			case UnityEngine.SystemLanguage.Vietnamese:
				return Language.Vietnamese;
			default:
				Log.Error($"Not supported Language .{Application.systemLanguage}");
				return Language.Unspecified;
			}
		}
	}

	public int DictionaryCount => m_DictionaryRaw.Count + m_Dictionary.Count;

	public void Initialize(Language userLanguage)
	{
		if (userLanguage != Language.Unspecified)
		{
			m_Language = userLanguage;
		}
		else
		{
			m_Language = SystemLanguage;
		}
		InitFont();
		IsInitDone = false;
		IsInitSuccess = false;
		m_Dictionary.Clear();
		m_DictionaryRaw.Clear();
	}

	public void Shutdown()
	{
		IsInitDone = false;
		IsInitSuccess = false;
	}

	public void Uninitialize()
	{
	}

	public void SetSwapLocaleFile(LocaleFileParallel.LocaleFileSwapPayload payload)
	{
		_swapPayload = payload;
	}

	public void UseSwapLocaleFile(bool safeMode = false)
	{
		if (_swapPayload != null)
		{
			IsInitDone = !safeMode;
			IsInitSuccess = _swapPayload.isInitSuccess;
			if (IsInitSuccess)
			{
				m_DictionaryRaw = _swapPayload.dictionaryRaw;
				m_Dictionary = _swapPayload.dictionary;
				Log.Info($"LocalizationManager::UseSwapLocaleFile {_swapPayload.locale_abb}, {_swapPayload.locale_version}");
			}
			else
			{
				Log.Error("LocalizationManager::UseSwapLocaleFile can`t load locale file in package or persistent path.");
			}
		}
		else
		{
			Log.Error("LocalizationManager::UseSwapLocaleFile can`t use swap locale file.");
		}
	}

	public void LoadDictionary(string dictionaryName)
	{
		IsInitDone = false;
		IsInitSuccess = false;
		Log.Info($"LocalizationManager::LoadDictionary {ClientConfig.MustHaveLocaleFile}, {ClientConfig.LOCALE_IN_PACKAGE}, {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
		if (ClientConfig.MustHaveLocaleFile)
		{
			if (ClientConfig.LOCALE_IN_PACKAGE)
			{
				string text = ClientConfig.LocaleBinFileName(ClientConfig.LOCALE_VERSION, ClientConfig.LOCALE_ABB);
				if (BuiltinFileReader.ReadyFileFromBuiltIn(ClientConfig.LocalePlayerDataPath + "/" + text, out var _, out var handler))
				{
					MemoryStream memoryStream = new MemoryStream(handler.data);
					if (LoadDictionaryFromStreamBin(memoryStream))
					{
						Log.Info($"LocalizationManager::LoadDictionaryFromStreamBin succeed from package {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
					}
					else
					{
						Log.Info($"LocalizationManager::LoadDictionaryFromStreamBin failed from package {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
					}
					memoryStream.Close();
				}
				else
				{
					string text2 = ClientConfig.LocaleFileName(ClientConfig.LOCALE_VERSION, ClientConfig.LOCALE_ABB);
					if (BuiltinFileReader.ReadyFileFromBuiltIn(ClientConfig.LocalePlayerDataPath + "/" + text2, out var error2, out var handler2))
					{
						MemoryStream memoryStream2 = new MemoryStream(handler2.data);
						if (LoadDictionaryFromStream(memoryStream2))
						{
							Log.Info($"LocalizationManager::LoadDictionaryFromStream succeed from package {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
						}
						else
						{
							Log.Info($"LocalizationManager::LoadDictionaryFromStream failed from package {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
						}
						memoryStream2.Close();
					}
					else
					{
						Log.Error($"can`t load locale file in package {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}, {error2}");
					}
				}
			}
			else
			{
				string localeBinDownloadDataPath = ClientConfig.GetLocaleBinDownloadDataPath(ClientConfig.LOCALE_VERSION, ClientConfig.LOCALE_ABB);
				if (File.Exists(localeBinDownloadDataPath))
				{
					FileStream fileStream = File.Open(localeBinDownloadDataPath, FileMode.Open, FileAccess.Read);
					if (LoadDictionaryFromStreamBin(fileStream))
					{
						Log.Info($"LocalizationManager::LoadDictionaryFromStreamBin succeed from persistent {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
					}
					else
					{
						Log.Info($"LocalizationManager::LoadDictionaryFromStreamBin failed from persistent {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
					}
					fileStream.Close();
				}
				else
				{
					string localeDownloadDataPath = ClientConfig.GetLocaleDownloadDataPath(ClientConfig.LOCALE_VERSION, ClientConfig.LOCALE_ABB);
					if (File.Exists(localeDownloadDataPath))
					{
						FileStream fileStream2 = File.Open(localeDownloadDataPath, FileMode.Open, FileAccess.Read);
						if (LoadDictionaryFromStream(fileStream2))
						{
							Log.Info($"LocalizationManager::LoadDictionaryFromStream succeed from persistent {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
						}
						else
						{
							Log.Info($"LocalizationManager::LoadDictionaryFromStream failed from persistent {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
						}
						fileStream2.Close();
					}
					else
					{
						Log.Error($"can`t load locale file in persistentDataPath {ClientConfig.LOCALE_VERSION}, {ClientConfig.LOCALE_ABB}");
					}
				}
			}
		}
		if (IsInitDone && IsInitSuccess)
		{
			return;
		}
		if (ClientConfig.LOCALE_VERSION_PACKAGE != 0 && ClientConfig.LOCALE_ABB_PACKAGE != null)
		{
			Log.Warning($"backup, load locale in package {ClientConfig.LOCALE_VERSION_PACKAGE}, {ClientConfig.LOCALE_ABB_PACKAGE}");
			string text3 = ClientConfig.LocaleBinFileName(ClientConfig.LOCALE_VERSION_PACKAGE, ClientConfig.LOCALE_ABB_PACKAGE);
			if (BuiltinFileReader.ReadyFileFromBuiltIn(ClientConfig.LocalePlayerDataPath + "/" + text3, out var _, out var handler3))
			{
				MemoryStream memoryStream3 = new MemoryStream(handler3.data);
				if (LoadDictionaryFromStreamBin(memoryStream3))
				{
					Log.Info($"LocalizationManager::LoadDictionaryFromStreamBin succeed from package {ClientConfig.LOCALE_VERSION_PACKAGE}, {ClientConfig.LOCALE_ABB_PACKAGE}");
				}
				else
				{
					Log.Info($"LocalizationManager::LoadDictionaryFromStreamBin failed from package {ClientConfig.LOCALE_VERSION_PACKAGE}, {ClientConfig.LOCALE_ABB_PACKAGE}");
				}
				memoryStream3.Close();
			}
			else
			{
				string text4 = ClientConfig.LocaleFileName(ClientConfig.LOCALE_VERSION_PACKAGE, ClientConfig.LOCALE_ABB_PACKAGE);
				if (BuiltinFileReader.ReadyFileFromBuiltIn(ClientConfig.LocalePlayerDataPath + "/" + text4, out var error4, out var handler4))
				{
					MemoryStream memoryStream4 = new MemoryStream(handler4.data);
					if (LoadDictionaryFromStream(memoryStream4))
					{
						Log.Info($"LocalizationManager::LoadDictionaryFromStream succeed from package {ClientConfig.LOCALE_VERSION_PACKAGE}, {ClientConfig.LOCALE_ABB_PACKAGE}");
					}
					else
					{
						Log.Info($"LocalizationManager::LoadDictionaryFromStream failed from package {ClientConfig.LOCALE_VERSION_PACKAGE}, {ClientConfig.LOCALE_ABB_PACKAGE}");
					}
					memoryStream4.Close();
				}
				else
				{
					Log.Error($"can`t load locale file in package {ClientConfig.LOCALE_VERSION_PACKAGE}, {ClientConfig.LOCALE_ABB_PACKAGE}, {error4}");
				}
			}
		}
		if (!IsInitDone || !IsInitSuccess)
		{
			if (ClientConfig.MustHaveLocaleFile)
			{
				Log.Error("can`t load locale file in package or persistent path.");
				PostEventLog.Record("LOCALE_LOAD_FALLBACK");
			}
			string languageName = (IsSuported() ? Language.ToString() : "English");
			LoadDictionaryFallback(languageName, dictionaryName);
		}
	}

	public void ReloadDictionaryByUpdate(int version, string localeAbb)
	{
		try
		{
			string localeBinDownloadDataPath = ClientConfig.GetLocaleBinDownloadDataPath(version, localeAbb);
			bool flag = File.Exists(localeBinDownloadDataPath);
			string localeDownloadDataPath = ClientConfig.GetLocaleDownloadDataPath(version, localeAbb);
			bool flag2 = File.Exists(localeDownloadDataPath);
			if (flag || flag2)
			{
				bool flag3 = false;
				if (flag)
				{
					FileStream fileStream = File.Open(localeBinDownloadDataPath, FileMode.Open, FileAccess.Read);
					MemoryStream memoryStream = new MemoryStream();
					GZip.Decompress(fileStream, memoryStream, isStreamOwner: false);
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					flag3 = ParseBinDictionary(dictionary, memoryStream);
					if (flag3)
					{
						m_DictionaryRaw = dictionary;
					}
					fileStream.Close();
					memoryStream.Close();
					Log.Info("LocalizationManager::ReloadDictionaryByUpdate load from " + localeBinDownloadDataPath);
				}
				else if (flag2)
				{
					FileStream fileStream2 = File.Open(localeDownloadDataPath, FileMode.Open, FileAccess.Read);
					MemoryStream memoryStream2 = new MemoryStream();
					GZip.Decompress(fileStream2, memoryStream2, isStreamOwner: false);
					string text = Encoding.UTF8.GetString(memoryStream2.ToArray());
					Dictionary<string, DialogEntry> dictionary2 = new Dictionary<string, DialogEntry>();
					flag3 = ParseTxtDictionary(dictionary2, text);
					if (flag3)
					{
						m_Dictionary = dictionary2;
					}
					fileStream2.Close();
					memoryStream2.Close();
					Log.Info("LocalizationManager::ReloadDictionaryByUpdate load from " + localeDownloadDataPath);
				}
				if (flag3)
				{
					ClientConfig.LOCALE_VERSION = version;
					ClientConfig.LOCALE_ABB = localeAbb;
					ClientConfig.LOCALE_IN_PACKAGE = false;
					ClientConfig.MustHaveLocaleFile = true;
					Log.Info($"LocalizationManager::ReloadDictionaryByUpdate succeed from persistent {version}, {localeAbb}");
				}
				else
				{
					Log.Error($"LocalizationManager::ReloadDictionaryByUpdate failed from persistent {version}, {localeAbb}");
				}
			}
			else
			{
				Log.Error($"LocalizationManager::ReloadDictionaryByUpdate can`t load locale file in persistentDataPath {version}, {localeAbb}");
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
	}

	public void ReloadDictionaryByUpdateDevLocale(string localeAbb)
	{
		try
		{
			string devLocaleBinDownloadDataPath = ClientConfig.GetDevLocaleBinDownloadDataPath(localeAbb);
			if (File.Exists(devLocaleBinDownloadDataPath))
			{
				FileStream fileStream = File.Open(devLocaleBinDownloadDataPath, FileMode.Open, FileAccess.Read);
				MemoryStream memoryStream = new MemoryStream();
				GZip.Decompress(fileStream, memoryStream, isStreamOwner: false);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				bool num = ParseBinDictionary(dictionary, memoryStream);
				if (num)
				{
					m_DictionaryRaw = dictionary;
				}
				fileStream.Close();
				memoryStream.Close();
				Log.Info("LocalizationManager::ReloadDictionaryByUpdateDevLocale load from " + devLocaleBinDownloadDataPath);
				if (num)
				{
					ClientConfig.USE_DEV_LOCALE = true;
					Log.Info("LocalizationManager::ReloadDictionaryByUpdateDevLocale succeed from persistent " + localeAbb);
				}
				else
				{
					Log.Error("LocalizationManager::ReloadDictionaryByUpdateDevLocale failed from persistent " + localeAbb);
				}
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
	}

	private bool LoadDictionaryFromStream(Stream inStream)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			GZip.Decompress(inStream, memoryStream, isStreamOwner: false);
			IsInitDone = true;
			Dictionary<string, DialogEntry> dictionary = new Dictionary<string, DialogEntry>();
			string text = Encoding.UTF8.GetString(memoryStream.ToArray());
			IsInitSuccess = ParseTxtDictionary(dictionary, text);
			if (IsInitSuccess)
			{
				m_Dictionary = dictionary;
			}
			memoryStream.Close();
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		if (IsInitDone)
		{
			return IsInitSuccess;
		}
		return false;
	}

	private bool LoadDictionaryFromStreamBin(Stream inStream)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			GZip.Decompress(inStream, memoryStream, isStreamOwner: false);
			IsInitDone = true;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			IsInitSuccess = ParseBinDictionary(dictionary, memoryStream);
			if (IsInitSuccess)
			{
				m_DictionaryRaw = dictionary;
			}
			memoryStream.Close();
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		if (IsInitDone)
		{
			return IsInitSuccess;
		}
		return false;
	}

	private void LoadDictionaryFallback(string languageName, string dictionaryName)
	{
		string dictionaryAssetName = $"Assets/Main/Localization/{languageName}/Dictionaries/{dictionaryName}.txt";
		Log.Info("LoadDictionary start {0}", dictionaryAssetName);
		Asset asset = GameEntry.Resource.LoadAssetAsync(dictionaryAssetName, typeof(TextAsset));
		if (asset != null)
		{
			Asset asset2 = asset;
			asset2.completed = (Action<Asset>)Delegate.Combine(asset2.completed, (Action<Asset>)delegate(Asset request)
			{
				Log.Info("LoadDictionary completed {0}", dictionaryAssetName);
				LoadDictionaryCallback(m_Dictionary, request.asset as TextAsset);
				request.Release();
			});
			return;
		}
		asset = GameEntry.Resource.LoadAssetAsync("Assets/Main/Localization/English/Dictionaries/Dialog.txt", typeof(TextAsset));
		if (asset != null)
		{
			Asset asset3 = asset;
			asset3.completed = (Action<Asset>)Delegate.Combine(asset3.completed, (Action<Asset>)delegate(Asset request)
			{
				Log.Info("LoadDictionary completed {0}", dictionaryAssetName);
				LoadDictionaryCallback(m_Dictionary, request.asset as TextAsset);
				request.Release();
			});
		}
		else
		{
			IsInitDone = true;
			IsInitSuccess = false;
		}
	}

	private void LoadDictionaryCallback(Dictionary<string, DialogEntry> dictionary, TextAsset asset)
	{
		IsInitDone = true;
		IsInitSuccess = ParseTxtDictionary(dictionary, asset.text);
	}

	public static bool ParseTxtDictionary(Dictionary<string, DialogEntry> dictionary, string text)
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
						string value = span3.ToString();
						AddRawString(dictionary, key, value);
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

	public static bool ParseBinDictionary(Dictionary<string, string> dictionary, MemoryStream memoryStream)
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

	public bool IsSuported()
	{
		return SuportedLanguages.Contains(Language);
	}

	public void SetSuportedLanguages(List<int> langList)
	{
		SuportedLanguages.Clear();
		foreach (int lang in langList)
		{
			SuportedLanguages.Add((Language)lang);
		}
	}

	public void UpdateSkinData(LuaTable rowData)
	{
		if (rowData != null)
		{
			m_DictionarySkin = new Dictionary<string, string>();
			rowData.ForEach(delegate(string keyOld, string keyNew)
			{
				m_DictionarySkin[keyOld] = keyNew;
			});
		}
	}

	public string GetString(string key, params object[] args)
	{
		if (string.IsNullOrEmpty(key))
		{
			if (CommonUtils.IsDebug())
			{
				return "Key is invalid";
			}
			return string.Empty;
		}
		if (m_DictionarySkin != null)
		{
			string value = string.Empty;
			if (m_DictionarySkin.TryGetValue(key, out value) && !value.IsNullOrEmpty())
			{
				key = value;
			}
		}
		if (ShowKey)
		{
			return "<<" + key + ">>";
		}
		string value2 = string.Empty;
		if (!m_DictionaryRaw.TryGetValue(key, out value2))
		{
			if (!m_Dictionary.TryGetValue(key, out var value3))
			{
				return "<" + key + ">";
			}
			if (value3.hasCRLF)
			{
				if (value3.originDlg.Contains("\\n"))
				{
					value3.originDlg = value3.originDlg.Replace("\\n", "\n");
				}
				value3.hasCRLF = false;
			}
			value2 = value3.originDlg;
		}
		try
		{
			if (args != null && args.Length != 0)
			{
				string empty = string.Empty;
				if (args[0] is string[] array)
				{
					string format = value2;
					object[] args2 = array;
					return string.Format(format, args2);
				}
				return string.Format(value2, args);
			}
		}
		catch (Exception)
		{
		}
		return value2;
	}

	private static bool HasRawString(Dictionary<string, DialogEntry> dictionary, string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			throw new Exception("Key is invalid.");
		}
		return dictionary.ContainsKey(key);
	}

	private string GetRawString(string key)
	{
		return string.Empty;
	}

	private static bool AddRawString(Dictionary<string, DialogEntry> dictionary, string key, string value)
	{
		DialogEntry dialogEntry = new DialogEntry();
		dialogEntry.originDlg = value;
		dialogEntry.hasCRLF = true;
		dictionary[key] = dialogEntry;
		return true;
	}

	private bool RemoveRawString(string key)
	{
		if (!HasRawString(m_Dictionary, key))
		{
			return false;
		}
		return m_Dictionary.Remove(key);
	}

	public string GetLanguageName()
	{
		return GetLanguageName(Language);
	}

	public string GetLanguageNameToLoading(Language languageName)
	{
		return GetLanguageName(languageName);
	}

	public string GetLanguageNameByInt(int language)
	{
		return GetLanguageName((Language)language);
	}

	public static string GetLanguageName(Language language)
	{
		return language switch
		{
			Language.ChineseSimplified => "zh_CN", 
			Language.ChineseTraditional => "zh_TW", 
			Language.English => "en", 
			Language.PortuguesePortugal => "pt", 
			Language.Turkish => "tr", 
			Language.French => "fr", 
			Language.Norwegian => "no", 
			Language.Korean => "ko", 
			Language.Japanese => "ja", 
			Language.Dutch => "nl", 
			Language.Italian => "it", 
			Language.German => "de", 
			Language.Spanish => "es", 
			Language.Russian => "ru", 
			Language.Arabic => "ar", 
			Language.Persian => "pr", 
			Language.Thai => "th", 
			Language.Vietnamese => "vi", 
			Language.Indonesian => "id", 
			Language.Polish => "pl", 
			_ => "en", 
		};
	}

	private static string[] SplitToLines(string text)
	{
		List<string> list = new List<string>();
		int position = 0;
		string text2 = null;
		while ((text2 = ReadLine(text, ref position)) != null)
		{
			list.Add(text2);
		}
		return list.ToArray();
	}

	private static string ReadLine(string text, ref int position)
	{
		if (text == null)
		{
			return null;
		}
		int length = text.Length;
		int i;
		for (i = position; i < length; i++)
		{
			char c = text[i];
			if (c == '\n' || c == '\r')
			{
				string result = text.Substring(position, i - position);
				position = i + 1;
				if (c == '\r' && position < length && text[position] == '\n')
				{
					position++;
				}
				return result;
			}
		}
		if (i > position)
		{
			string result2 = text.Substring(position, i - position);
			position = i;
			return result2;
		}
		return null;
	}

	public int GetLanguage()
	{
		return (int)Language;
	}

	public void SetLanguage(int language)
	{
		GameEntry.Setting.UserLanguage = (Language)language;
		UIRunTimeConfig.IsArabic = GameEntry.Setting.UserLanguage == Language.Arabic;
		SuperTextMesh.IsArabicLanguage = GameEntry.Setting.UserLanguage == Language.Arabic;
	}

	public Font GetFontByLanguage()
	{
		return null;
	}

	public bool HasKey(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return false;
		}
		if (m_Dictionary == null)
		{
			return false;
		}
		if (!m_DictionaryRaw.ContainsKey(key))
		{
			return m_Dictionary.ContainsKey(key);
		}
		return true;
	}

	private Dictionary<uint, TMP_Character> GetOrCreateRemovedDict(string fontKey)
	{
		if (!_removedCharactersPerFont.TryGetValue(fontKey, out var value))
		{
			value = new Dictionary<uint, TMP_Character>();
			_removedCharactersPerFont[fontKey] = value;
		}
		return value;
	}

	public void InitFont()
	{
		if (_fontLanguages.TryGetValue(m_Language, out var value))
		{
			try
			{
				foreach (var fontInfo in value)
				{
					if (GameEntry.Resource.LoadAsset(fontInfo.Item1, typeof(TMP_FontAsset)).asset is TMP_FontAsset { fallbackFontAssetTable: var fallbackFontAssetTable } tMP_FontAsset)
					{
						TMP_FontAsset tMP_FontAsset2 = fallbackFontAssetTable.Find((TMP_FontAsset font) => font.name == fontInfo.Item2);
						if (tMP_FontAsset2 != null)
						{
							fallbackFontAssetTable.Remove(tMP_FontAsset2);
							fallbackFontAssetTable.Insert(0, tMP_FontAsset2);
							tMP_FontAsset.fallbackFontAssetTable = fallbackFontAssetTable;
						}
						string item = fontInfo.Item1;
						bool flag = fontInfo.Item1.EndsWith("Title.asset") || fontInfo.Item1.EndsWith("Body.asset");
						if (flag)
						{
							Dictionary<uint, TMP_Character> characterLookupTable = tMP_FontAsset.characterLookupTable;
							if (_removedCharactersPerFont.TryGetValue(item, out var value2))
							{
								foreach (KeyValuePair<uint, TMP_Character> item2 in value2)
								{
									if (!characterLookupTable.ContainsKey(item2.Key))
									{
										characterLookupTable[item2.Key] = item2.Value;
									}
								}
							}
							tMP_FontAsset.characterLookupTable = characterLookupTable;
						}
						if (flag)
						{
							if (LanguageFontMap.TryGetValue(m_Language, out var value3))
							{
								Dictionary<uint, TMP_Character> characterLookupTable2 = tMP_FontAsset.characterLookupTable;
								Dictionary<uint, TMP_Character> orCreateRemovedDict = GetOrCreateRemovedDict(item);
								for (uint num = 65u; num <= 90; num++)
								{
									if (characterLookupTable2.TryGetValue(num, out var value4))
									{
										orCreateRemovedDict[num] = value4;
										characterLookupTable2.Remove(num);
									}
									if (characterLookupTable2.TryGetValue(num + 32, out value4))
									{
										orCreateRemovedDict[num + 32] = value4;
										characterLookupTable2.Remove(num + 32);
									}
								}
								if (value3)
								{
									for (uint num2 = 48u; num2 <= 57; num2++)
									{
										if (characterLookupTable2.TryGetValue(num2, out var value5))
										{
											orCreateRemovedDict[num2] = value5;
											characterLookupTable2.Remove(num2);
										}
									}
								}
								tMP_FontAsset.characterLookupTable = characterLookupTable2;
							}
							else if (m_Language == Language.Japanese)
							{
								Dictionary<uint, TMP_Character> characterLookupTable3 = tMP_FontAsset.characterLookupTable;
								Dictionary<uint, TMP_Character> orCreateRemovedDict2 = GetOrCreateRemovedDict(item);
								for (uint num3 = 40u; num3 <= 41; num3++)
								{
									if (characterLookupTable3.TryGetValue(num3, out var value6))
									{
										orCreateRemovedDict2[num3] = value6;
										characterLookupTable3.Remove(num3);
									}
								}
								tMP_FontAsset.characterLookupTable = characterLookupTable3;
							}
							else if (m_Language == Language.Arabic)
							{
								Dictionary<uint, TMP_Character> characterLookupTable4 = tMP_FontAsset.characterLookupTable;
								Dictionary<uint, TMP_Character> orCreateRemovedDict3 = GetOrCreateRemovedDict(item);
								uint[] array = new uint[4] { 33u, 38u, 47u, 8230u };
								foreach (uint key in array)
								{
									if (characterLookupTable4.TryGetValue(key, out var value7))
									{
										orCreateRemovedDict3[key] = value7;
										characterLookupTable4.Remove(key);
									}
								}
								tMP_FontAsset.characterLookupTable = characterLookupTable4;
							}
						}
					}
				}
				return;
			}
			catch (Exception ex)
			{
				FibMatrix.Logger.Error("InitFont Exception." + ex.ToString());
				return;
			}
		}
		try
		{
			foreach (var fontInfo2 in _commonLanguage)
			{
				if (GameEntry.Resource.LoadAsset(fontInfo2.Item1, typeof(TMP_FontAsset)).asset is TMP_FontAsset { fallbackFontAssetTable: var fallbackFontAssetTable2 } tMP_FontAsset3)
				{
					TMP_FontAsset tMP_FontAsset4 = fallbackFontAssetTable2.Find((TMP_FontAsset font) => font.name == fontInfo2.Item2);
					if (tMP_FontAsset4 != null)
					{
						fallbackFontAssetTable2.Remove(tMP_FontAsset4);
						fallbackFontAssetTable2.Insert(0, tMP_FontAsset4);
						tMP_FontAsset3.fallbackFontAssetTable = fallbackFontAssetTable2;
					}
				}
			}
		}
		catch (Exception ex2)
		{
			FibMatrix.Logger.Error("InitFont Exception." + ex2.ToString());
		}
	}

	public void ClearFontDynamicData()
	{
		foreach (Asset item in fontAssetsCache)
		{
			if (item != null)
			{
				GameEntry.Resource.UnloadAsset(item);
			}
		}
		fontAssetsCache.Clear();
		foreach (string needInitFont in _needInitFonts)
		{
			try
			{
				Asset asset = GameEntry.Resource.LoadAsset(needInitFont, typeof(TMP_FontAsset));
				if (asset != null)
				{
					fontAssetsCache.Add(asset);
				}
				if (asset.asset is TMP_FontAsset { atlasPopulationMode: AtlasPopulationMode.Dynamic, isMultiAtlasTexturesEnabled: not false } tMP_FontAsset)
				{
					tMP_FontAsset.ClearFontAssetData();
					_ = tMP_FontAsset.atlasTexture;
				}
			}
			catch (Exception ex)
			{
				FibMatrix.Logger.Error("InitFont Exception." + ex.ToString());
			}
		}
	}
}
