using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using GameKit.Base;
using ICSharpCode.SharpZipLib.Zip;
using LuaScriptInterface;
using UnityEngine;
using VEngine;
using XLua;
using XLua.LuaDLL;

public class XLuaManager
{
	[CSharpCallLua]
	public delegate void DataCenterInitDelegate();

	[CSharpCallLua]
	public delegate void UITimeForLuaDelegate(long timeStamp);

	[CSharpCallLua]
	public delegate void UIShowTipsDelegate(string msg, string img, string atlas);

	[CSharpCallLua]
	public delegate void UIShowFoldUpBuild(string posX, string posY, string posZ, string bUuid);

	[CSharpCallLua]
	public delegate void UIShowMessageDelegate(string tipText, int btnNum, string text1, string text2, Action action1, Action action2, Action closeAction, string titleText, bool isChangeImg);

	[CSharpCallLua]
	public delegate void WebSocketProtocalDelegate(string data);

	[CSharpCallLua]
	public delegate string GetTemplateDataFromLua(string xmlId, int id, string attrStr);

	[CSharpCallLua]
	public delegate void UpdateResourceItemMaxValueDelegate();

	[CSharpCallLua]
	public delegate void PreloadAssetsDelegate();

	[CSharpCallLua]
	public delegate void LuaLoadPbConfig(byte[] bytes);

	[CSharpCallLua]
	public delegate int ArmyFormationDataManager(long uuid);

	private enum LuaValueEnum
	{
		V_Bool,
		V_Int,
		V_UInt,
		V_Long,
		V_ULong,
		V_Double,
		V_String,
		V_Obj
	}

	private struct LuaValue
	{
		public LuaValueEnum type;

		public bool bv;

		public long lv;

		public ulong ulv;

		public double dv;

		public string sv;

		public object ov;
	}

	private struct CallContext
	{
		public LuaTable table;

		public LuaFunction function;

		public bool pushSelf;

		public List<LuaValue> paras;
	}

	public struct ByteString
	{
		public byte[] str;

		public int str_len;
	}

	private class LuaTableLookup
	{
		public string tablePath;

		public LuaTable table;

		public Dictionary<uint, LuaFunction> funcs;
	}

	private static readonly string[] LuaRootPath = new string[2] { "Assets/Main/LuaTxt/", "Assets/Main/DataTable/LuaTxt/" };

	private static readonly string[] LuaRootPathEditor = new string[2] { "/Main/LuaScripts/", "/Main/DataTable/Lua/" };

	private static readonly string LuaDataTableRootPath = "/ZipDocument/getnewlua/{0}/";

	private const string CommonMainScriptName = "Common.Main";

	private const string FrameworkMainScriptName = "Framework.FrameworkMain";

	private const string GameMainScriptName = "GameMain";

	private Dictionary<string, Asset> m_LuaScripts = new Dictionary<string, Asset>();

	private Action OnInitCallback;

	private LuaEnv m_LuaEnv;

	private LuaEnv m_LuaEnv_old;

	private LuaUpdater m_LuaUpdater;

	private GameObject updaterObject;

	private UIManager uiManager;

	private EventManager eventManager;

	private QueueDataManager queueDataManager;

	private BuildQueueManager buildQueueManager;

	private EarthOrderDataManager earthOrderDataManager;

	private MeteoriteHitPlaneEffectManager meteoriteHitPlaneEffectManager;

	private ItemData itemManager;

	private LuaFunction dispatchNetMessage;

	private DataCenterInitDelegate dataCenterInit;

	private UITimeForLuaDelegate updateUITimeStamp;

	private UIShowTipsDelegate showTips;

	private UIShowMessageDelegate showMessage;

	private UIShowFoldUpBuild showFoldUpBuild;

	private WebSocketProtocalDelegate webSocketProtocal;

	private GetTemplateDataFromLua getTemplateDataFromLua;

	private ArmyFormationDataManager armyFormationDataManager;

	private PreloadAssetsDelegate preloadAssets;

	private LuaLoadPbConfig luaLoadPbConfig;

	private UITimeForLuaDelegate syncServerTime;

	private static bool _delayLuaStartGame = false;

	public const bool UseLuaAsRawFile = false;

	private static MemoryStream s_tableMemStream = null;

	private static FileStream s_tableFileStream = null;

	private static ZipFile s_tableZipFile = null;

	private LWLuaFileUpdateParallel.LwScriptSwapPayload _swapPayload;

	private HashSet<string> __debug_report = new HashSet<string>();

	private Dictionary<uint, LuaTableLookup> lookupTables_ = new Dictionary<uint, LuaTableLookup>(8);

	private string lastFunctionName = "";

	private LuaTable lastTable;

	private LuaFunction lastFunction;

	private int lastCacheCount;

	private char[] call_sep_ = new char[2] { '.', ':' };

	private Dictionary<Type, Delegate> dictPushParam_ = new Dictionary<Type, Delegate>
	{
		{
			typeof(int),
			new Action<CallContext, int>(AddIntParam)
		},
		{
			typeof(double),
			new Action<CallContext, double>(AddDoubleParam)
		},
		{
			typeof(bool),
			new Action<CallContext, bool>(AddBoolParam)
		},
		{
			typeof(long),
			new Action<CallContext, long>(AddLongParam)
		},
		{
			typeof(ulong),
			new Action<CallContext, ulong>(AddULongParam)
		},
		{
			typeof(uint),
			new Action<CallContext, uint>(AddUIntParam)
		},
		{
			typeof(float),
			new Action<CallContext, float>(AddDoubleParam)
		},
		{
			typeof(string),
			new Action<CallContext, string>(AddStringParam)
		}
	};

	private List<LuaValue> global_list_cache_ = new List<LuaValue>(12);

	private byte[] global_byte_cache_ = new byte[256];

	private static List<string> __debugInfo = new List<string>();

	public LuaEnv Env => m_LuaEnv;

	public UIManager UIManager => uiManager;

	public EventManager EventManager => eventManager;

	public QueueDataManager QueueDataManager => queueDataManager;

	public BuildQueueManager BuildQueueManager => buildQueueManager;

	public EarthOrderDataManager EarthOrderDataManager => earthOrderDataManager;

	public MeteoriteHitPlaneEffectManager MeteoriteHitPlaneEffectManager => meteoriteHitPlaneEffectManager;

	public ItemData ItemManager => itemManager;

	public bool HasGameStart { get; protected set; }

	public static bool DelayLuaStartGame => _delayLuaStartGame;

	public static bool s_useLwLuaFile { get; private set; } = false;

	public static LWLuaFile s_lwLuaFile { get; private set; } = null;

	public void SetSwapLuaFile(LWLuaFileUpdateParallel.LwScriptSwapPayload payload)
	{
		_swapPayload = payload;
	}

	public void Initialize(Action callback = null)
	{
		ApplicationLaunch.StepLog("xLua Initialize");
		Log.Info("xLua open table file [" + ClientConfig.CURRENT_TABLE_FILE_PATH + "].");
		_delayLuaStartGame = true;
		if (!string.IsNullOrEmpty(ClientConfig.CURRENT_TABLE_FILE_PATH))
		{
			bool flag = false;
			using (FileStream fileStream = File.OpenRead(ClientConfig.CURRENT_TABLE_FILE_PATH))
			{
				byte[] array = new byte[8];
				fileStream.Read(array, 0, 8);
				flag = EncryptUtils.IsChachaTable(array);
			}
			if (flag)
			{
				byte[] array2 = File.ReadAllBytes(ClientConfig.CURRENT_TABLE_FILE_PATH);
				EncryptUtils.FromChachaToPKZip(array2);
				s_tableMemStream = new MemoryStream(array2);
				if (s_tableMemStream != null)
				{
					s_tableZipFile = new ZipFile(s_tableMemStream);
				}
			}
			else
			{
				s_tableFileStream = File.OpenRead(ClientConfig.CURRENT_TABLE_FILE_PATH);
				if (s_tableFileStream != null)
				{
					s_tableZipFile = new ZipFile(s_tableFileStream);
				}
			}
		}
		if (true)
		{
			if (_swapPayload != null)
			{
				s_lwLuaFile = _swapPayload.lwLuaFile;
			}
			else
			{
				s_lwLuaFile = LWLuaFile.Load(LWLuaFileUpdate.initSucceed, LWLuaFileUpdate.scriptFileMemory);
			}
		}
		else
		{
			s_lwLuaFile = null;
		}
		bool flag2 = true;
		s_useLwLuaFile = s_lwLuaFile != null && flag2;
		__debug_report.Clear();
		if (flag2 && s_lwLuaFile == null)
		{
			Log.Error("XLuaManager::Initialize lwLuaFile is null.");
		}
		ApplicationLaunch.UpdateTrackingResVersion();
		Log.Info("xLua Initialize begin.");
		OnInitCallback = callback;
		InitLuaEnv();
		OnInit();
	}

	public string GetStackInfo()
	{
		if (Env == null)
		{
			return string.Empty;
		}
		object[] array = Env.DoString("return debug.traceback()");
		if (array != null && array.Length != 0)
		{
			return array[0].ToString();
		}
		return string.Empty;
	}

	private void InitLuaEnv()
	{
		HasGameStart = false;
		if (m_LuaEnv_old != null && GameEntry.Setting.GetBool("ENABLE_DISPOSE_OLD_LUA_ENV_KEY", defaultValue: false))
		{
			try
			{
				Log.Info("m_LuaEnv_old Dispose");
				m_LuaEnv_old.translator.ForceClearAllDelegateBridge();
				m_LuaEnv_old.Dispose();
			}
			catch (Exception arg)
			{
				Log.Error($"m_LuaEnv_old Dispose Exception {arg}");
			}
			finally
			{
				m_LuaEnv_old = null;
				Log.Info("m_LuaEnv_old Dispose finally");
			}
		}
		m_LuaEnv = new LuaEnv();
		ClearCacheTableAll();
		if (m_LuaEnv != null)
		{
			m_LuaEnv.AddLoader(CustomLoader);
			m_LuaEnv.AddBuildin("rapidjson", Lua.LoadRapidJson);
			m_LuaEnv.AddBuildin("pb", Lua.LoadLuaProfobuf);
			initContext();
		}
		else
		{
			Log.Error("InitLuaEnv null!!!");
		}
		GameEntry.Event.Fire(EventId.ReInitLoadingLuaState);
	}

	private void OnInit()
	{
		if (m_LuaEnv != null)
		{
			try
			{
				LuaArrAccessAPI.RegisterPinFunc(m_LuaEnv.L);
			}
			catch (Exception ex)
			{
				Debug.LogError("LuaArrAccessAPI.RegisterPinFunc error : " + ex.Message);
			}
			LoadScript("Common.Main");
			Log.Info("lua load frameworkMain");
			LoadScript("Framework.FrameworkMain");
			updaterObject = new GameObject("LuaUpdater");
			UnityEngine.Object.DontDestroyOnLoad(updaterObject);
			m_LuaUpdater = updaterObject.AddComponent<LuaUpdater>();
			m_LuaUpdater.OnInit(m_LuaEnv);
		}
		OnInitCallback?.Invoke();
	}

	public void Update()
	{
		if (m_LuaEnv != null)
		{
			m_LuaEnv.Tick();
			syncServerTime?.Invoke(GameEntry.Timer.GetServerTime());
		}
	}

	public void StartGame()
	{
		Log.Info("xLua StartGame begin.");
		if (m_LuaEnv != null)
		{
			LoadScript("GameMain");
			SafeDoString("GameMain.Start()");
			HasGameStart = true;
			dispatchNetMessage = m_LuaEnv.Global.GetInPath<LuaFunction>("GameMain.Protocal");
			webSocketProtocal = m_LuaEnv.Global.GetInPath<WebSocketProtocalDelegate>("GameMain.WebSocketProtocal");
			dataCenterInit = m_LuaEnv.Global.GetInPath<DataCenterInitDelegate>("GameMain.DataCenterInit");
			preloadAssets = m_LuaEnv.Global.GetInPath<PreloadAssetsDelegate>("GameMain.PreloadAssets");
			updateUITimeStamp = m_LuaEnv.Global.GetInPath<UITimeForLuaDelegate>("GameMain.UpdateUITimeStamp");
			getTemplateDataFromLua = m_LuaEnv.Global.GetInPath<GetTemplateDataFromLua>("GameMain.GetTemplateData");
			showTips = m_LuaEnv.Global.GetInPath<UIShowTipsDelegate>("GameMain.ShowTips");
			showMessage = m_LuaEnv.Global.GetInPath<UIShowMessageDelegate>("GameMain.ShowMessage");
			showFoldUpBuild = m_LuaEnv.Global.GetInPath<UIShowFoldUpBuild>("GameMain.ShowFoldUpBuild");
			uiManager = m_LuaEnv.Global.GetInPath<UIManager>("UIManager.Instance");
			eventManager = m_LuaEnv.Global.GetInPath<EventManager>("EventManager.Instance");
			queueDataManager = m_LuaEnv.Global.GetInPath<QueueDataManager>("DataCenter.QueueDataManager");
			buildQueueManager = m_LuaEnv.Global.GetInPath<BuildQueueManager>("DataCenter.BuildQueueManager");
			earthOrderDataManager = m_LuaEnv.Global.GetInPath<EarthOrderDataManager>("DataCenter.EarthOrderDataManager");
			meteoriteHitPlaneEffectManager = m_LuaEnv.Global.GetInPath<MeteoriteHitPlaneEffectManager>("DataCenter.MeteoriteHitPlaneEffectManager");
			itemManager = m_LuaEnv.Global.GetInPath<ItemData>("DataCenter.ItemData");
			armyFormationDataManager = m_LuaEnv.Global.GetInPath<ArmyFormationDataManager>("GameMain.GetArmyFormationStamina");
			luaLoadPbConfig = m_LuaEnv.Global.GetInPath<LuaLoadPbConfig>("PBController.InitBytes");
			syncServerTime = m_LuaEnv.Global.GetInPath<UITimeForLuaDelegate>("GameMain.SyncServerTime");
		}
		Log.Info("xLua StartGame end.");
	}

	public void ExitGame()
	{
		if (m_LuaEnv != null && HasGameStart)
		{
			SafeDoString("GameMain.Exit()");
		}
		else
		{
			Debug.LogError("calling failed, lua start game not complete");
		}
	}

	public void Shutdown()
	{
		ClearLuaReference();
		foreach (Asset value in m_LuaScripts.Values)
		{
			value.Release();
		}
		m_LuaScripts.Clear();
		if (updaterObject != null)
		{
			m_LuaUpdater.Dispose();
			UnityEngine.Object.Destroy(updaterObject);
			m_LuaUpdater = null;
			updaterObject = null;
		}
		OnInitCallback = null;
		s_tableZipFile?.Close();
		s_tableZipFile = null;
		s_tableFileStream?.Dispose();
		s_tableFileStream = null;
		s_tableMemStream?.Dispose();
		s_tableMemStream = null;
		s_lwLuaFile?.Dispose();
		s_lwLuaFile = null;
		_swapPayload = null;
		if (m_LuaEnv == null)
		{
			return;
		}
		try
		{
			m_LuaEnv.Dispose();
			m_LuaEnv = null;
		}
		catch (Exception ex)
		{
			m_LuaEnv_old = m_LuaEnv;
			if (!ex.Message.Contains("try to dispose a LuaEnv with C# callback!"))
			{
				Debug.LogError($"xLua dispose exception : {ex.Message}\n {ex.StackTrace}");
			}
		}
		finally
		{
			m_LuaEnv = null;
		}
	}

	public void SafeDoString(string scriptContent)
	{
		if (m_LuaEnv != null)
		{
			try
			{
				m_LuaEnv.DoString(scriptContent);
			}
			catch (Exception ex)
			{
				Debug.LogError($"xLua exception : {ex.Message}\n {ex.StackTrace}");
			}
		}
	}

	public void LoadScript(string scriptName)
	{
		SafeDoString($"require('{scriptName}')");
	}

	public static byte[] CustomLoader(ref string filepath)
	{
		return CustomLoaderImpl(ref filepath);
	}

	private static byte[] CustomLoaderImpl(ref string filepath)
	{
		if (ApplicationLaunch.Instance.Loading.isZipModeFinish && filepath.Contains("LuaDatatable"))
		{
			string[] array = filepath.Split(new char[1] { '.' });
			if (array.Length != 2)
			{
				array = filepath.Split(new char[1] { '/' });
			}
			if (array.Length == 2)
			{
				string text = array[^1];
				string path = Application.persistentDataPath + string.Format(LuaDataTableRootPath, GameEntry.Sdk.Version) + text + ".lua";
				if (File.Exists(path))
				{
					return File.ReadAllBytes(path);
				}
			}
		}
		if (!ClientConfig.LocalMode && filepath.Contains("LuaDatatable"))
		{
			if (s_tableZipFile != null)
			{
				string name = filepath.Replace("LuaDatatable.", "");
				ZipEntry entry = s_tableZipFile.GetEntry(name);
				if (entry != null)
				{
					using (Stream stream = s_tableZipFile.GetInputStream(entry))
					{
						byte[] array2 = new byte[entry.Size];
						stream.Read(array2, 0, array2.Length);
						return array2;
					}
				}
			}
			if (ClientConfig.MustHaveDataTable)
			{
				Log.Error("can`t load LuaDatatable file in zip file {0}", filepath);
				PostEventLog.Record("DATATABLE_LOAD_FALLBACK");
			}
		}
		if (GameEntry.Resource.IsSimulation || (Application.isEditor && !s_useLwLuaFile))
		{
			filepath = filepath.Replace(".", "/") + ".lua";
			string[] luaRootPathEditor = LuaRootPathEditor;
			foreach (string text2 in luaRootPathEditor)
			{
				string path2 = Application.dataPath + text2 + filepath;
				if (File.Exists(path2))
				{
					return File.ReadAllBytes(path2);
				}
			}
		}
		else
		{
			if (s_useLwLuaFile)
			{
				byte[] array3 = s_lwLuaFile?.LoadFile(filepath);
				if (array3 != null)
				{
					return array3;
				}
				if (!LWLuaFile.InWhiteList(filepath))
				{
					int num = ((s_lwLuaFile != null) ? s_lwLuaFile.version : 0);
					Log.Error($"can`t load lua file from lw lua file {num}, {filepath}");
				}
			}
			filepath = filepath.Replace(".", "/") + ".bytes";
			string[] luaRootPathEditor = LuaRootPath;
			for (int i = 0; i < luaRootPathEditor.Length; i++)
			{
				string text3 = luaRootPathEditor[i] + filepath;
				if (GameEntry.Lua.m_LuaScripts.TryGetValue(text3, out var value))
				{
					return (value.asset as TextAsset).bytes;
				}
				if (!GameEntry.Resource.HasAsset(text3))
				{
					continue;
				}
				byte[] array4 = null;
				value = GameEntry.Resource.LoadAsset(text3, typeof(TextAsset));
				if (value != null && !value.isError)
				{
					array4 = ((TextAsset)value.asset).bytes;
					if (GameEntry.Lua.m_LuaScripts.Count == 0)
					{
						GameEntry.Lua.m_LuaScripts.Add(text3, value);
					}
					else
					{
						Resources.UnloadAsset((TextAsset)value.asset);
						value.Release();
						GameEntry.Resource.RemoveCachedUnusedAssets();
					}
					return array4;
				}
			}
		}
		return null;
	}

	public void DispatchResponse(string cmd, object table)
	{
		if (m_LuaEnv != null && dispatchNetMessage != null && HasGameStart)
		{
			dispatchNetMessage.CallForPushTable(cmd, table);
		}
		else
		{
			Debug.LogError($"calling failed, lua start game not complete, {m_LuaEnv != null}, {dispatchNetMessage != null}, {HasGameStart}");
		}
	}

	public void DataCenterInit()
	{
		if (m_LuaEnv != null && dataCenterInit != null && HasGameStart)
		{
			dataCenterInit();
		}
		else
		{
			Debug.LogError("calling failed, lua start game not complete");
		}
	}

	public void PreloadAssets()
	{
		if (m_LuaEnv != null && preloadAssets != null && HasGameStart)
		{
			preloadAssets();
		}
		else
		{
			Debug.LogError("calling failed, lua start game not complete");
		}
	}

	public void UpdateUITimeStamp(long timeStamp)
	{
		if (m_LuaEnv != null && updateUITimeStamp != null && HasGameStart)
		{
			updateUITimeStamp(timeStamp);
		}
		else
		{
			Debug.LogError("calling failed, lua start game not complete");
		}
	}

	public void ShowTips(string msg, string img, string atlas)
	{
		if (m_LuaEnv != null && showTips != null && HasGameStart)
		{
			showTips(msg, img, atlas);
		}
		else
		{
			Debug.LogError("calling failed, lua start game not complete");
		}
	}

	public void ShowMessage(string tipText, int btnNum, string text1, string text2, Action action1, Action action2, Action closeAction, string titleText, bool isChangeImg)
	{
		if (m_LuaEnv != null && showMessage != null && HasGameStart)
		{
			showMessage(tipText, btnNum, text1, text2, action1, action2, closeAction, titleText, isChangeImg);
		}
		else
		{
			Debug.LogError("calling failed, lua start game not complete");
		}
	}

	public void ShowFoldUpBuild(string posX, string posY, string posZ, string bUuid)
	{
		if (m_LuaEnv != null && showFoldUpBuild != null && HasGameStart)
		{
			showFoldUpBuild(posX, posY, posZ, bUuid);
		}
		else
		{
			Debug.LogError("calling failed, lua start game not complete");
		}
	}

	public QueueData GetQueueDataByType(int type)
	{
		return QueueDataManager.GetQueueByType(type);
	}

	public QueueData GetQueueByBuildUuidForFarm(long bUuid)
	{
		return QueueDataManager.GetQueueByBuildUuidForFarm(bUuid);
	}

	public bool GetCanPlantForPastureByBuildUuid(long bUuid)
	{
		return QueueDataManager.GetCanPlantForPastureByBuildUuid(bUuid);
	}

	public bool IsShowEarthOrder()
	{
		return EarthOrderDataManager.IsShowEarthOrder();
	}

	public bool IsCanShowEffectByPoint(int point)
	{
		return MeteoriteHitPlaneEffectManager.IsCanShowEffectByPoint(point);
	}

	public void WebSocketResponse(string data)
	{
		if (m_LuaEnv != null && webSocketProtocal != null && HasGameStart)
		{
			webSocketProtocal(data);
		}
		else
		{
			Debug.LogError("calling failed, lua start game not complete");
		}
	}

	public string GetTemplateData(string xmlId, int id, string attrStr)
	{
		if (m_LuaEnv != null && getTemplateDataFromLua != null && HasGameStart)
		{
			return getTemplateDataFromLua(xmlId, id, attrStr);
		}
		Debug.LogError("calling failed, lua start game not complete");
		return "";
	}

	public int GetArmyInfoByStamina(long uuid)
	{
		if (m_LuaEnv != null && getTemplateDataFromLua != null && HasGameStart)
		{
			return armyFormationDataManager(uuid);
		}
		Debug.LogError("calling failed, lua start game not complete");
		return 0;
	}

	public void LuaLoadPb(byte[] bytes)
	{
		if (luaLoadPbConfig != null)
		{
			luaLoadPbConfig(bytes);
		}
	}

	private void ClearLuaReference()
	{
		dispatchNetMessage = null;
		webSocketProtocal = null;
		dataCenterInit = null;
		preloadAssets = null;
		updateUITimeStamp = null;
		getTemplateDataFromLua = null;
		showTips = null;
		showMessage = null;
		showFoldUpBuild = null;
		uiManager = null;
		eventManager = null;
		queueDataManager = null;
		buildQueueManager = null;
		earthOrderDataManager = null;
		meteoriteHitPlaneEffectManager = null;
		itemManager = null;
		armyFormationDataManager = null;
		luaLoadPbConfig = null;
		syncServerTime = null;
	}

	public void DebugLuaFile(string filename)
	{
		try
		{
			if (!s_useLwLuaFile)
			{
				return;
			}
			if (filename == "CSharpCallLuaInterface")
			{
				filename = "Util.CSharpCallLuaInterface";
			}
			if (!__debug_report.Contains(filename))
			{
				byte[] array = s_lwLuaFile.LoadFile(filename);
				if (array != null)
				{
					string text = Convert.ToBase64String(array);
					Log.Info($"[LwLuaFile] Debug {filename}_{array.Length}_{text.Length}: {text}]");
				}
				else
				{
					Log.Info("[LwLuaFile] Debug " + filename + " not found");
				}
				__debug_report.Add(filename);
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
	}

	private void initContext()
	{
		Env.translator.AddPushByteFunc(typeof(ByteString), delegate(IntPtr L, ByteString v)
		{
			Lua.xlua_pushlstring(L, v.str, v.str_len);
		});
		Env.translator.AddCustomFunc(typeof(LuaStackTable), delegate(IntPtr L, object obj)
		{
			((LuaStackTable)obj).push();
		});
	}

	private void fillByteArray(byte[] dest, ReadOnlySpan<char> src)
	{
		int num = 0;
		ReadOnlySpan<char> readOnlySpan = src;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			char c = readOnlySpan[i];
			dest[num++] = (byte)c;
		}
	}

	public void ClearCacheTable(string luaTableName)
	{
		uint key = hash_function(luaTableName.AsSpan());
		lookupTables_.Remove(key);
	}

	public void ClearCacheTableAll()
	{
		lookupTables_.Clear();
		lastFunctionName = "";
		lastTable = null;
		lastFunction = null;
		lastCacheCount = 0;
	}

	private LuaTableLookup cacheTable(ReadOnlySpan<char> tableName, uint tableHashCode, LuaTable tbl)
	{
		LuaTableLookup luaTableLookup = new LuaTableLookup();
		luaTableLookup.tablePath = tableName.ToString();
		luaTableLookup.table = tbl;
		luaTableLookup.funcs = new Dictionary<uint, LuaFunction>(4);
		lookupTables_[tableHashCode] = luaTableLookup;
		return luaTableLookup;
	}

	private T getLuaChild<T>(LuaTable parent, ReadOnlySpan<char> childName)
	{
		byte[] array = byte_Rent(childName.Length);
		fillByteArray(array, childName);
		ByteString key = default(ByteString);
		key.str = array;
		key.str_len = childName.Length;
		T result = parent.Get<ByteString, T>(key);
		byte_Return(array);
		return result;
	}

	private bool getAndCacheTable(ReadOnlySpan<char> tableName, out LuaTableLookup lt)
	{
		uint num = hash_function(tableName);
		if (!lookupTables_.TryGetValue(num, out lt))
		{
			LuaTable luaTable = Env.Global;
			StringExtensions.SegmentSplitEnumerator enumerator = tableName.SplitSegments('.').GetEnumerator();
			while (enumerator.MoveNext())
			{
				ReadOnlySpan<char> childName = enumerator.Current;
				luaTable = getLuaChild<LuaTable>(luaTable, childName);
				if (luaTable == null)
				{
					break;
				}
			}
			if (luaTable == null)
			{
				Log.Error("getAndCacheTable {0} not found!", tableName.ToString());
				return false;
			}
			lt = cacheTable(tableName, num, luaTable);
		}
		return true;
	}

	private bool getAndCacheFunction(ReadOnlySpan<char> tableName, ReadOnlySpan<char> funcName, out LuaTable t, out LuaFunction f)
	{
		if (!getAndCacheTable(tableName, out var lt))
		{
			t = null;
			f = null;
			return false;
		}
		t = lt.table;
		uint key = hash_function(funcName);
		if (lt.funcs.TryGetValue(key, out f))
		{
			return true;
		}
		f = getLuaChild<LuaFunction>(lt.table, funcName);
		if (f == null)
		{
			Log.Error("getAndCacheFunction {0}.{1} not found!", tableName.ToString(), funcName.ToString());
			try
			{
				__debugInfo.Clear();
				lt.table.ForEach(delegate(object obj, object value)
				{
					__debugInfo.Add($"{obj} ({obj?.GetType().Name}), {value} ({value?.GetType().Name})");
				});
				Log.Error("getAndCacheFunction " + tableName.ToString() + ", " + string.Join(",", __debugInfo));
				DebugLuaFile(tableName.ToString());
			}
			catch (Exception message)
			{
				Log.Error(message);
			}
			return false;
		}
		uint key2 = hash_function(funcName);
		lt.funcs[key2] = f;
		return true;
	}

	private uint hash_function(ReadOnlySpan<char> str)
	{
		uint num = 5381u;
		for (int i = 0; i < str.Length; i++)
		{
			byte b = (byte)str[i];
			num = (num << 5) + num + b;
		}
		return num;
	}

	private byte[] byte_Rent(int len)
	{
		return global_byte_cache_;
	}

	private void byte_Return(byte[] b)
	{
	}

	private bool makeCallContext(string fullPathName, ref CallContext cc)
	{
		int num = fullPathName.LastIndexOfAny(call_sep_);
		if (num == -1)
		{
			Log.Error(" {0} path error?", fullPathName);
			return false;
		}
		LuaTable t = null;
		LuaFunction f = null;
		if (fullPathName == lastFunctionName)
		{
			t = lastTable;
			f = lastFunction;
			lastCacheCount++;
		}
		else
		{
			ReadOnlySpan<char> readOnlySpan = fullPathName.AsSpan();
			ReadOnlySpan<char> tableName = readOnlySpan.Slice(0, num);
			ReadOnlySpan<char> funcName = readOnlySpan.Slice(num + 1);
			if (!getAndCacheFunction(tableName, funcName, out t, out f))
			{
				Log.Error(" {0} not found!", fullPathName);
				return false;
			}
			lastFunctionName = fullPathName;
			lastTable = t;
			lastFunction = f;
		}
		cc.table = t;
		cc.function = f;
		cc.pushSelf = fullPathName[num] == ':';
		cc.paras = global_list_cache_;
		global_list_cache_.Clear();
		return true;
	}

	private T endCallContext<T>(CallContext cc)
	{
		LuaEnv env = Env;
		IntPtr l = env.L;
		ObjectTranslator translator = env.translator;
		int num = Lua.lua_gettop(l);
		int num2 = Lua.load_error_func(l, env.errorFuncRef);
		cc.function.push(l);
		int num3 = cc.paras.Count;
		if (cc.pushSelf)
		{
			num3++;
			cc.table.push(l);
		}
		for (int i = 0; i < cc.paras.Count; i++)
		{
			switch (cc.paras[i].type)
			{
			case LuaValueEnum.V_Bool:
				translator.PushByType(l, cc.paras[i].bv);
				break;
			case LuaValueEnum.V_Double:
				translator.PushByType(l, cc.paras[i].dv);
				break;
			case LuaValueEnum.V_Int:
				translator.PushByType(l, (int)cc.paras[i].lv);
				break;
			case LuaValueEnum.V_Long:
				translator.PushByType(l, cc.paras[i].lv);
				break;
			case LuaValueEnum.V_UInt:
				translator.PushByType(l, (uint)cc.paras[i].ulv);
				break;
			case LuaValueEnum.V_ULong:
				translator.PushByType(l, cc.paras[i].ulv);
				break;
			case LuaValueEnum.V_String:
				translator.PushByType(l, cc.paras[i].sv);
				break;
			case LuaValueEnum.V_Obj:
				translator.PushByType(l, cc.paras[i].ov);
				break;
			}
		}
		if (Lua.lua_pcall(l, num3, -1, num2) != 0)
		{
			try
			{
				env.ThrowExceptionFromError(num);
			}
			catch (LuaException ex)
			{
				Log.Error("CallLuaFunc but exception : {0}", ex.Message);
				return default(T);
			}
		}
		Lua.lua_remove(l, num2);
		int num4 = Lua.lua_gettop(l);
		if (num == num4)
		{
			return default(T);
		}
		T v = default(T);
		int num5 = num + 1;
		if (num5 <= num4)
		{
			translator.Get(l, num5, out v);
		}
		Lua.lua_settop(l, num);
		return v;
	}

	private static void AddIntParam(CallContext cc, int param)
	{
		LuaValue item = new LuaValue
		{
			type = LuaValueEnum.V_Int,
			lv = param
		};
		cc.paras.Add(item);
	}

	private static void AddUIntParam(CallContext cc, uint param)
	{
		LuaValue item = new LuaValue
		{
			type = LuaValueEnum.V_UInt,
			ulv = param
		};
		cc.paras.Add(item);
	}

	private static void AddLongParam(CallContext cc, long param)
	{
		LuaValue item = new LuaValue
		{
			type = LuaValueEnum.V_Long,
			lv = param
		};
		cc.paras.Add(item);
	}

	private static void AddULongParam(CallContext cc, ulong param)
	{
		LuaValue item = new LuaValue
		{
			type = LuaValueEnum.V_ULong,
			ulv = param
		};
		cc.paras.Add(item);
	}

	private static void AddDoubleParam(CallContext cc, double param)
	{
		LuaValue item = new LuaValue
		{
			type = LuaValueEnum.V_Double,
			dv = param
		};
		cc.paras.Add(item);
	}

	private static void AddDoubleParam(CallContext cc, float param)
	{
		LuaValue item = new LuaValue
		{
			type = LuaValueEnum.V_Double,
			dv = param
		};
		cc.paras.Add(item);
	}

	private static void AddBoolParam(CallContext cc, bool param)
	{
		LuaValue item = new LuaValue
		{
			type = LuaValueEnum.V_Bool,
			bv = param
		};
		cc.paras.Add(item);
	}

	private static void AddStringParam(CallContext cc, string param)
	{
		LuaValue item = new LuaValue
		{
			type = LuaValueEnum.V_String,
			sv = param
		};
		cc.paras.Add(item);
	}

	private static void AddObjectParam(CallContext cc, object obj)
	{
		LuaValue item = new LuaValue
		{
			type = LuaValueEnum.V_Obj,
			ov = obj
		};
		cc.paras.Add(item);
	}

	private void AddParam<T>(CallContext cc, T param)
	{
		Type typeFromHandle = typeof(T);
		if (dictPushParam_.TryGetValue(typeFromHandle, out var value))
		{
			((Action<CallContext, T>)value)(cc, param);
		}
		else
		{
			AddObjectParam(cc, param);
		}
	}

	public void Call(string luaFunctionPath)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			endCallContext<object>(cc);
		}
	}

	public void Call<T1>(string luaFunctionPath, T1 param1)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			endCallContext<object>(cc);
		}
	}

	public void Call<T1, T2>(string luaFunctionPath, T1 param1, T2 param2)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			AddParam(cc, param2);
			endCallContext<object>(cc);
		}
	}

	public void Call<T1, T2, T3>(string luaFunctionPath, T1 param1, T2 param2, T3 param3)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			AddParam(cc, param2);
			AddParam(cc, param3);
			endCallContext<object>(cc);
		}
	}

	public void Call<T1, T2, T3, T4>(string luaFunctionPath, T1 param1, T2 param2, T3 param3, T4 param4)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			AddParam(cc, param2);
			AddParam(cc, param3);
			AddParam(cc, param4);
			endCallContext<object>(cc);
		}
	}

	public void Call<T1, T2, T3, T4, T5>(string luaFunctionPath, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			AddParam(cc, param2);
			AddParam(cc, param3);
			AddParam(cc, param4);
			AddParam(cc, param5);
			endCallContext<object>(cc);
		}
	}

	public T CallWithReturn<T>(string luaFunctionPath)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			return endCallContext<T>(cc);
		}
		return default(T);
	}

	public T CallWithReturn<T, T1>(string luaFunctionPath, T1 param1)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			return endCallContext<T>(cc);
		}
		return default(T);
	}

	public T CallWithReturn<T, T1, T2>(string luaFunctionPath, T1 param1, T2 param2)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			AddParam(cc, param2);
			return endCallContext<T>(cc);
		}
		return default(T);
	}

	public T CallWithReturn<T, T1, T2, T3>(string luaFunctionPath, T1 param1, T2 param2, T3 param3)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			AddParam(cc, param2);
			AddParam(cc, param3);
			return endCallContext<T>(cc);
		}
		return default(T);
	}

	public T CallWithReturn<T, T1, T2, T3, T4>(string luaFunctionPath, T1 param1, T2 param2, T3 param3, T4 param4)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			AddParam(cc, param2);
			AddParam(cc, param3);
			AddParam(cc, param4);
			return endCallContext<T>(cc);
		}
		return default(T);
	}

	public T CallWithReturn<T, T1, T2, T3, T4, T5>(string luaFunctionPath, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			AddParam(cc, param2);
			AddParam(cc, param3);
			AddParam(cc, param4);
			AddParam(cc, param5);
			return endCallContext<T>(cc);
		}
		return default(T);
	}

	public T CallWithReturn<T, T1, T2, T3, T4, T5, T6>(string luaFunctionPath, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
	{
		CallContext cc = default(CallContext);
		if (makeCallContext(luaFunctionPath, ref cc))
		{
			AddParam(cc, param1);
			AddParam(cc, param2);
			AddParam(cc, param3);
			AddParam(cc, param4);
			AddParam(cc, param5);
			AddParam(cc, param6);
			return endCallContext<T>(cc);
		}
		return default(T);
	}

	public int CallWithReturnInt(string luaFunctionPath)
	{
		return CallWithReturn<int>(luaFunctionPath);
	}

	public int CallWithReturnInt<T1>(string luaFunctionPath, T1 param1)
	{
		return CallWithReturn<int, T1>(luaFunctionPath, param1);
	}

	public int CallWithReturnInt<T1, T2>(string luaFunctionPath, T1 param1, T2 param2)
	{
		return CallWithReturn<int, T1, T2>(luaFunctionPath, param1, param2);
	}

	public string CallWithReturnString(string luaFunctionPath)
	{
		return CallWithReturn<string>(luaFunctionPath);
	}

	public string CallWithReturnString<T1>(string luaFunctionPath, T1 param1)
	{
		return CallWithReturn<string, T1>(luaFunctionPath, param1);
	}

	public string CallWithReturnString<T1, T2>(string luaFunctionPath, T1 param1, T2 param2)
	{
		return CallWithReturn<string, T1, T2>(luaFunctionPath, param1, param2);
	}

	public void SetValue<T>(string luaTable, string attr, T value)
	{
		if (getAndCacheTable(luaTable.AsSpan(), out var lt))
		{
			try
			{
				lt.table.SetInPath(attr, value);
			}
			catch (Exception)
			{
				Log.Error("SetValue error!");
			}
		}
	}

	public T GetValue<T>(string luaTable, string attr)
	{
		if (getAndCacheTable(luaTable.AsSpan(), out var lt))
		{
			try
			{
				return lt.table.GetInPath<T>(attr);
			}
			catch (Exception)
			{
				Log.Error("SetValue error!");
			}
		}
		if (typeof(T) == typeof(string))
		{
			return (T)(object)"";
		}
		return default(T);
	}

	public int GetValue_Int(string luaTable, string attr)
	{
		return GetValue<int>(luaTable, attr);
	}

	public string GetValue_String(string luaTable, string attr)
	{
		return GetValue<string>(luaTable, attr);
	}
}
