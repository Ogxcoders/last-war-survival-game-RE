using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using FibMatrix.BaseUtils;
using GameFramework;
using UnityEngine;
using UnityEngine.U2D;
using VEngine;
using XLua;

public class ResourceManager
{
	public class InstantiateInfo
	{
		public int loadCount;

		public long totalMsCost;

		public long maxMsCost;

		public int loadCountPool;

		public int loadCountNew;

		public long totalMsCostPool;

		public long totalMsCostNew;

		public int setupCount;

		public long totalSetupMsCost;

		public long maxSetupMsCost;

		public InstantiateInfo()
		{
			loadCount = 0;
			totalMsCost = 0L;
			maxMsCost = -1L;
			loadCountPool = 0;
			loadCountNew = 0;
			totalMsCostPool = 0L;
			totalMsCostNew = 0L;
			setupCount = 0;
			totalSetupMsCost = 0L;
			maxSetupMsCost = -1L;
		}

		public void RecordCreate(long msCost, bool fromPool)
		{
			loadCount++;
			totalMsCost += msCost;
			maxMsCost = ((msCost > maxMsCost) ? msCost : maxMsCost);
			if (fromPool)
			{
				loadCountPool++;
				totalMsCostPool += msCost;
			}
			else
			{
				loadCountNew++;
				totalMsCostNew += msCost;
			}
		}

		public void RecordSetup(long msCost)
		{
			setupCount++;
			totalSetupMsCost += msCost;
			maxSetupMsCost = ((msCost > maxSetupMsCost) ? msCost : maxSetupMsCost);
		}

		public float SafeDivide(long numerator, long denominator)
		{
			if (denominator == 0L)
			{
				return 0f;
			}
			return (float)Math.Round((float)numerator * 1f / (float)denominator, 2);
		}

		public string CsvInfo()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append($"{loadCount},{totalMsCost},{SafeDivide(totalMsCost, loadCount)},{maxMsCost},");
			stringBuilder.Append($"{loadCountPool},{totalMsCostPool},{SafeDivide(totalMsCostPool, loadCountPool)},");
			stringBuilder.Append($"{loadCountNew},{totalMsCostNew},{SafeDivide(totalMsCostNew, loadCountNew)},");
			stringBuilder.Append($"{setupCount},{totalSetupMsCost},{SafeDivide(totalSetupMsCost, setupCount)},{maxSetupMsCost}");
			return stringBuilder.ToString();
		}
	}

	private class LoadTraceRecordInfo
	{
		public string prefab;

		public string trace;

		public int loadNum;

		public int priority;

		public LoadTraceRecordInfo(string prefab, string trace, int priority)
		{
			this.prefab = prefab;
			this.trace = trace;
			this.priority = priority;
			loadNum = 1;
		}
	}

	public enum PreloadType
	{
		Cache,
		KeepAlive
	}

	public class PreloadCache
	{
		public Asset asset;

		public float expiredTime;
	}

	private class LoadQueue
	{
		private const int MAX_INSTANCE_PERFRAME = 20;

		private const float MAX_INSTANCE_TIME = 15f;

		private static int _remainNum;

		private static float _remainTime;

		private static LoadQueue[] _queues = new LoadQueue[4];

		private const long MAX_INSTANCE_TIME_SW = 15L;

		private static Stopwatch _timer = new Stopwatch();

		private int _priority;

		private Queue<InstanceRequest> _toInstanceList = new Queue<InstanceRequest>();

		private LinkedList<InstanceRequest> _instancingList = new LinkedList<InstanceRequest>();

		public LoadQueue(int priority)
		{
			_priority = priority;
		}

		private void Update()
		{
			int remainNum = _remainNum;
			if (_toInstanceList.Count > 0 && _instancingList.Count < remainNum)
			{
				int num = Math.Min(remainNum - _instancingList.Count, _toInstanceList.Count);
				_remainNum -= num;
				while (_toInstanceList.Count > 0)
				{
					InstanceRequest instanceRequest = _toInstanceList.Dequeue();
					if (instanceRequest.state != InstanceRequest.State.Destroy)
					{
						instanceRequest.Instantiate();
						_instancingList.AddLast(instanceRequest);
						if (--num <= 0)
						{
							break;
						}
					}
				}
			}
			_ = _instancingList.Count;
			_ = Time.realtimeSinceStartup;
			LinkedListNode<InstanceRequest> linkedListNode = _instancingList.First;
			while (linkedListNode != null)
			{
				LinkedListNode<InstanceRequest> next = linkedListNode.Next;
				if (!linkedListNode.Value.Update())
				{
					_instancingList.Remove(linkedListNode);
				}
				linkedListNode = next;
				if (_timer.ElapsedMilliseconds > 15)
				{
					break;
				}
			}
		}

		private void Add(InstanceRequest request)
		{
			_toInstanceList.Enqueue(request);
		}

		private void Clear()
		{
			foreach (InstanceRequest toInstance in _toInstanceList)
			{
				toInstance.Destroy();
			}
			foreach (InstanceRequest instancing in _instancingList)
			{
				instancing.Destroy();
			}
			_toInstanceList.Clear();
			_instancingList.Clear();
		}

		public static void Init()
		{
			_queues[2] = new LoadQueue(2);
			_queues[1] = new LoadQueue(1);
			_queues[0] = new LoadQueue(0);
			_queues[3] = new LoadQueue(3);
		}

		public static void UpdateLoadQueue()
		{
			UpdateLoadQueueBySW();
		}

		private static void UpdateLoadQueueBySW()
		{
			_timer.Restart();
			_remainNum = 20;
			try
			{
				_queues[3].Update();
			}
			catch (Exception ex)
			{
				if (CommonUtils.IsDebug())
				{
					Log.Error(ex.ToString() ?? "");
				}
			}
			if (CanContinue())
			{
				try
				{
					_queues[2].Update();
				}
				catch (Exception ex2)
				{
					if (CommonUtils.IsDebug())
					{
						Log.Error(ex2.ToString() ?? "");
					}
				}
			}
			if (CanContinue())
			{
				try
				{
					_queues[1].Update();
				}
				catch (Exception ex3)
				{
					if (CommonUtils.IsDebug())
					{
						Log.Error(ex3.ToString() ?? "");
					}
				}
			}
			if (CanContinue())
			{
				try
				{
					_queues[0].Update();
				}
				catch (Exception ex4)
				{
					if (CommonUtils.IsDebug())
					{
						Log.Error(ex4.ToString() ?? "");
					}
				}
			}
			_timer.Stop();
		}

		public static void AddRequest(InstanceRequest request, int priority = 2)
		{
			if (request != null)
			{
				if (priority < 0 || priority > 3)
				{
					Log.Error($"ErrorProperty {request.PrefabPath} -> {priority}");
					priority = Mathf.Clamp(priority, 0, 3);
				}
				_queues[priority].Add(request);
			}
		}

		public static void ClearAll()
		{
			_queues[3].Clear();
			_queues[2].Clear();
			_queues[1].Clear();
			_queues[0].Clear();
		}

		private static bool CanContinue()
		{
			if (_remainNum <= 0)
			{
				if (_timer.IsRunning)
				{
					return _timer.ElapsedMilliseconds < 15;
				}
				return false;
			}
			return true;
		}
	}

	private Dictionary<string, InstantiateInfo> instantiateInfos;

	private const int EDITOR_SAMPLE_GAP = 3;

	private float editorSampleTimer;

	private int editorSampleDataCount;

	private int[] secondStampArray;

	private int[] toInstanceCountArray;

	private int[] instancingCountArray;

	private Stopwatch setupStopWatch;

	private string currentPrefabPath;

	private Dictionary<string, Dictionary<string, LoadTraceRecordInfo>> _loadTraceRecordMap = new Dictionary<string, Dictionary<string, LoadTraceRecordInfo>>();

	private Stopwatch instStopWatch;

	private string currentInstName;

	public readonly string GameResManifestName = "gameres";

	public readonly string DataTableManifestName = "datatable";

	public readonly string LuaManifestName = "lua";

	public readonly string DllResManifestName = "dllres";

	private string[] manifests;

	private string bkgroundManifest;

	private string packageResManifest;

	private List<string> manifestsInUse;

	private float lastTimePerSecondUpdate;

	private float lastTimeCleanPoolUpdate;

	private DownloadUpdateBkground _updateBkground;

	public const string BUNDLE_OFFSET_TABLE_FILE = "BundleOffsetTable.bytes";

	public float asyncDelayTime;

	private string _asyncDelayPath;

	public HashSet<string> asyncDelayPathSet;

	private const float CacheTime = 300f;

	private Dictionary<string, PreloadCache> preloadCache = new Dictionary<string, PreloadCache>();

	private List<string> keysRemove = new List<string>();

	private static readonly HashSet<string> s_RequestPathStats = new HashSet<string>();

	private const string AtlasRootPath = "Assets/Main/Atlas/{0}.spriteatlas";

	private ObjectPoolMgr objectPoolMgr = new ObjectPoolMgr();

	public const string k_GenGPUAnimAssetPath = "Assets/_Art_LastWar/GenGPUAnim/";

	public bool EditorSampling { get; private set; }

	public string asyncDelayPath
	{
		get
		{
			return _asyncDelayPath;
		}
		set
		{
			_asyncDelayPath = value;
			if (string.IsNullOrWhiteSpace(_asyncDelayPath))
			{
				asyncDelayPathSet = null;
				return;
			}
			asyncDelayPathSet = new HashSet<string>(_asyncDelayPath.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries));
		}
	}

	public string AccCheckVersionURL => null;

	public string CheckVersionURL => null;

	public bool Loggable
	{
		get
		{
			return VEngine.Logger.Loggable;
		}
		set
		{
			VEngine.Logger.Loggable = value;
		}
	}

	public bool IsSimulation => Versions.IsSimulation;

	public bool SkipUpdateBundle => Versions.SkipUpdate;

	public Transform ObjectPoolRootTrans => objectPoolMgr.Root;

	public void EditorRecordCreate(string prefabName, long msCost, bool fromPool)
	{
		instantiateInfos = instantiateInfos ?? new Dictionary<string, InstantiateInfo>(1024, StringComparer.OrdinalIgnoreCase);
		if (!instantiateInfos.TryGetValue(prefabName, out var value))
		{
			value = new InstantiateInfo();
			instantiateInfos.Add(prefabName, value);
		}
		value.RecordCreate(msCost, fromPool);
	}

	public void EditorRecordSetupCost(string prefabName, long msCost)
	{
		instantiateInfos = instantiateInfos ?? new Dictionary<string, InstantiateInfo>(1024, StringComparer.OrdinalIgnoreCase);
		if (!instantiateInfos.TryGetValue(prefabName, out var value))
		{
			value = new InstantiateInfo();
			instantiateInfos.Add(prefabName, value);
		}
		value.RecordSetup(msCost);
	}

	public void EditorDescription(ref string desc)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("我是什么？");
		stringBuilder.AppendLine("是ResourceManager!");
		stringBuilder.AppendLine("采样状态：" + EditorSampling);
		if (EditorSampling)
		{
			stringBuilder.AppendLine($"统计数量:{instantiateInfos?.Count ?? 0}");
		}
		desc = stringBuilder.ToString();
	}

	public void EditorGetPoolInfo(ref string desc)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine($"缓存池长度：{objectPoolMgr.interface_poolList.Count}    t:{Time.realtimeSinceStartup}");
		Dictionary<string, ObjectPool>.Enumerator enumerator = objectPoolMgr.interface_poolList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			ObjectPool value = enumerator.Current.Value;
			stringBuilder.AppendLine($" out/pool: {value.GetObjCount()}/{value.GetPoolCount()}  {value.prefabPath}");
		}
		desc = stringBuilder.ToString();
	}

	public void EditorStartSample()
	{
		if (!EditorSampling)
		{
			UnityEngine.Debug.Log("[Res]StartSample");
			EditorSampling = true;
			editorSampleTimer = 0f;
			editorSampleDataCount = 0;
			secondStampArray = secondStampArray ?? new int[1024];
			toInstanceCountArray = toInstanceCountArray ?? new int[1024];
			instancingCountArray = instancingCountArray ?? new int[1024];
			Array.Clear(secondStampArray, 0, secondStampArray.Length);
			Array.Clear(toInstanceCountArray, 0, toInstanceCountArray.Length);
			Array.Clear(instancingCountArray, 0, instancingCountArray.Length);
			SampleInstancingInfo();
			EditorStartSampleLoadTrace();
		}
	}

	[Conditional("UNITY_EDITOR")]
	private void EditorSampleUpdate()
	{
	}

	private void SampleInstancingInfo()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		editorSampleTimer -= unscaledDeltaTime;
		if (editorSampleTimer <= 0f)
		{
			if (editorSampleDataCount >= secondStampArray.Length)
			{
				Array.Resize(ref secondStampArray, editorSampleDataCount + 500);
				Array.Resize(ref toInstanceCountArray, editorSampleDataCount + 500);
				Array.Resize(ref instancingCountArray, editorSampleDataCount + 500);
			}
			secondStampArray[editorSampleDataCount] = editorSampleDataCount * 3;
			toInstanceCountArray[editorSampleDataCount] = 0;
			instancingCountArray[editorSampleDataCount] = 0;
			editorSampleDataCount++;
			editorSampleTimer = 3f;
		}
	}

	public void EditorEndSample()
	{
		EditorSampling = false;
		EditorEndSampleLoadTrace();
		if (!EditorSampling || instantiateInfos.Count == 0)
		{
			UnityEngine.Debug.LogWarning("[Res]没有任何记录值得导出");
			return;
		}
		UnityEngine.Debug.Log($"[Res]EndSample:{instantiateInfos.Count}");
		DateTime now = DateTime.Now;
		string fullPath = Path.GetFullPath($"{Application.dataPath}/../EditorOutput/ResProfile{now.Year}-{now.Month}-{now.Day}_{now.Hour}_{now.Minute}_{now.Second}");
		if (!Directory.Exists(fullPath))
		{
			Directory.CreateDirectory(fullPath);
		}
		string text = fullPath + "/InstantiateInfo.csv";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("名称,加载次数,总耗时(ms),平均耗时,最大耗时,加载次数(池),总耗时(池),平均耗时(池),加载次数(新),总耗时(新),平均耗时(新),回调处理次数,回调总耗时,回调平均耗时,回调最大耗时");
		if (instantiateInfos.Count > 0)
		{
			foreach (KeyValuePair<string, InstantiateInfo> instantiateInfo in instantiateInfos)
			{
				stringBuilder.AppendLine(instantiateInfo.Key + "," + instantiateInfo.Value.CsvInfo());
			}
			instantiateInfos.Clear();
		}
		File.WriteAllText(text, stringBuilder.ToString(), Encoding.UTF8);
		UnityEngine.Debug.Log("[Res]资源实例化数据 保存到:" + text);
		text = fullPath + "/ResProfile.csv";
		stringBuilder.Length = 0;
		stringBuilder.AppendLine("秒,加载请求,加载中");
		if (editorSampleDataCount > 0)
		{
			for (int i = 0; i < editorSampleDataCount; i++)
			{
				stringBuilder.AppendLine($"{secondStampArray[i]},{toInstanceCountArray[i]},{instancingCountArray[i]}");
			}
		}
		File.WriteAllText(text, stringBuilder.ToString(), Encoding.UTF8);
		UnityEngine.Debug.Log("[Res]资源管理器请求数据 保存到:" + text);
		EditorOpenFolder(fullPath);
	}

	[Conditional("UNITY_EDITOR")]
	public void EditorStartSampleSetupCost(string prefabPath)
	{
	}

	[Conditional("UNITY_EDITOR")]
	public void EditorEndSampleSetupCost()
	{
	}

	[Conditional("UNITY_EDITOR")]
	public void RecordRequest(string path, int priority)
	{
		if (EditorSampling)
		{
			StackTrace stackTrace = new StackTrace(2, fNeedFileInfo: true);
			StringBuilder stringBuilder = new StringBuilder();
			StackFrame[] frames = stackTrace.GetFrames();
			foreach (StackFrame stackFrame in frames)
			{
				stringBuilder.AppendLine($"{stackFrame.GetFileName()}:{stackFrame.GetFileLineNumber()}  {stackFrame.GetMethod()}");
			}
			string text = stringBuilder.ToString();
			if (text.Contains("XLuaGen"))
			{
				text = GameEntry.Lua.CallWithReturn<string>("debug.traceback");
			}
			if (!_loadTraceRecordMap.TryGetValue(path, out var value))
			{
				value = new Dictionary<string, LoadTraceRecordInfo>();
				_loadTraceRecordMap.Add(path, value);
			}
			if (!value.TryGetValue(text, out var value2))
			{
				value2 = new LoadTraceRecordInfo(path, text, priority);
				value.Add(text, value2);
			}
			else
			{
				value2.loadNum++;
			}
		}
	}

	private void EditorStartSampleLoadTrace()
	{
		_loadTraceRecordMap.Clear();
	}

	private void EditorEndSampleLoadTrace()
	{
		DateTime now = DateTime.Now;
		string fullPath = Path.GetFullPath($"{Application.dataPath}/../EditorOutput/ResProfile_{now.Year}{now.Month}{now.Day}");
		if (!Directory.Exists(fullPath))
		{
			Directory.CreateDirectory(fullPath);
		}
		string text = $"{fullPath}/LoadTrace_{now.Hour}-{now.Minute}-{now.Second}.csv";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("名称,加载次数,加载优先级,堆栈");
		if (_loadTraceRecordMap.Count > 0)
		{
			foreach (KeyValuePair<string, Dictionary<string, LoadTraceRecordInfo>> item in _loadTraceRecordMap)
			{
				foreach (KeyValuePair<string, LoadTraceRecordInfo> item2 in item.Value)
				{
					LoadTraceRecordInfo value = item2.Value;
					string trace = value.trace;
					trace = trace.Replace("\"", "\"\"");
					trace = "\"" + trace + "\"";
					stringBuilder.AppendLine($"{value.prefab},{value.loadNum},{value.priority},{trace}");
				}
			}
		}
		File.WriteAllText(text, stringBuilder.ToString(), Encoding.UTF8);
		UnityEngine.Debug.Log("[Res]加载堆栈统计 保存到:" + text);
	}

	[Conditional("UNITY_EDITOR")]
	public void EditorStartSampleInstCost(string instName)
	{
	}

	[Conditional("UNITY_EDITOR")]
	public void EditorEndSampleInstCost(bool fromPool)
	{
	}

	private void EditorOpenFolder(string folder)
	{
		folder = $"\"{folder}\"";
		switch (Application.platform)
		{
		case RuntimePlatform.WindowsEditor:
			Process.Start("Explorer.exe", folder.Replace('/', '\\'));
			break;
		case RuntimePlatform.OSXEditor:
			Process.Start("open", folder);
			break;
		}
	}

	public void Initialize(Action<bool> onComplete)
	{
		bool syncLoadPackageManifest = StartupConfig.Inst.syncLoadPackageManifest;
		Log.Info($"ResourceManager::Initialize syncMode:{syncLoadPackageManifest}");
		Versions.syncLoadPackageManifest = syncLoadPackageManifest;
		InitializeVersions operation = Versions.InitializeAsync();
		InitializeVersions initializeVersions = operation;
		initializeVersions.completed = (Action<Operation>)Delegate.Combine(initializeVersions.completed, (Action<Operation>)delegate
		{
			if (operation.status == OperationStatus.Failed)
			{
				Log.Error(operation.error);
			}
			onComplete?.Invoke(operation.status == OperationStatus.Success);
		});
		manifests = operation.manifests.ToArray();
		bkgroundManifest = operation.bkgroundManifest;
		packageResManifest = operation.packageResManifest;
		manifestsInUse = new List<string>(2) { "GameRes", "DllRes" };
		Asset.EditorHookBeforeRequestAsset = RecordeRequestPathInEditor;
		Versions.DownloadURL = NetworkURLConfig.DownloadURL;
		Versions.getDownloadURL = NetworkURLConfig.GetDownloadURL;
		Versions.IsGMUser = GrayUtils.isGM;
		SpriteAtlasManager.atlasRegistered += OnAtlasRegistered;
		SpriteAtlasManager.atlasRequested += OnAtlasRequested;
		operation.Start();
		LoadQueue.Init();
	}

	private static void RecordeRequestPathInEditor(string path)
	{
		if (Application.isEditor)
		{
			string text = "(" + Path.GetExtension(path) + ")" + Path.GetDirectoryName(path);
			text = text.Replace("\\", "/");
			if (!text.Contains(")Assets/Main/") && !text.StartsWith("(.prefab)Assets/_Art_LastWar/") && !text.StartsWith("(.prefab)Assets/_Art/Effect/prefab/") && !text.StartsWith("(.asset)Assets/Art/Emoji/EmojiAssets/") && !s_RequestPathStats.Contains(text))
			{
				s_RequestPathStats.Add(text);
				FibEngineBIApi.SendEvent("RequestAssetTypeDir", new Dictionary<string, object> { { "typeAndDir", text } });
			}
		}
	}

	public string[] GetManifestNames()
	{
		return manifests;
	}

	public List<string> GetManifestNamesInUse()
	{
		return manifestsInUse;
	}

	public string GetBkgroundManifestName()
	{
		return bkgroundManifest;
	}

	public string GetPackageResManifestName()
	{
		return packageResManifest;
	}

	public string GetTempDownloadPath(string file)
	{
		string text = Application.temporaryCachePath + "/Download/" + file;
		string directoryName = Path.GetDirectoryName(text);
		if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		return text;
	}

	public void OverrideManifest(Manifest manifest, bool refreshMemory = true)
	{
		if (Versions.SkipUpdate)
		{
			return;
		}
		string tempDownloadPath = GetTempDownloadPath(manifest.name);
		string downloadDataPath = Versions.GetDownloadDataPath(manifest.name);
		if (File.Exists(tempDownloadPath))
		{
			File.Copy(tempDownloadPath, downloadDataPath, overwrite: true);
		}
		string versionFile = Manifest.GetVersionFile(manifest.name);
		tempDownloadPath = GetTempDownloadPath(versionFile);
		if (File.Exists(tempDownloadPath))
		{
			string downloadDataPath2 = Versions.GetDownloadDataPath(versionFile);
			string text = "null";
			try
			{
				ManifestVersionFile manifestVersionFile = ManifestVersionFile.Load(tempDownloadPath);
				text = $"[{manifestVersionFile.version}, {manifestVersionFile.crc}]";
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
			}
			Log.Info("OverrideManifest Copy " + tempDownloadPath + " to " + downloadDataPath2 + ", " + text + ".");
			File.Copy(tempDownloadPath, downloadDataPath2, overwrite: true);
		}
		else
		{
			Log.Info("OverrideManifest Copy " + manifest.name + " version file " + tempDownloadPath + " isn`t exists.");
		}
		if (!refreshMemory)
		{
			Log.Info($"Load manifest skip {downloadDataPath} {manifest.version} refresh memory");
		}
		else if (Versions.IsChanged(manifest.name))
		{
			if (File.Exists(downloadDataPath))
			{
				manifest.Load(downloadDataPath);
				Log.Info($"Load manifest {downloadDataPath} {manifest.version} to override");
				Versions.Override(manifest);
			}
			else
			{
				Log.Info("skip manifest override load, " + downloadDataPath + " is builtin");
			}
		}
	}

	public UpdateVersions UpdateManifests()
	{
		return Versions.UpdateAsync(manifests);
	}

	public ulong GetDownloadSize(List<Manifest> manifests, List<DownloadInfo> downloadInfos, string targetPath = null, DownloadChecker downloadChecker = null)
	{
		if (Versions.SkipUpdate || manifests == null || manifests.Count == 0)
		{
			return 0uL;
		}
		Func<BundleInfo, bool> func = null;
		func = ((downloadChecker == null) ? new Func<BundleInfo, bool>(Versions.IsDownloaded) : new Func<BundleInfo, bool>(downloadChecker.IsDownloaded));
		ulong num = 0uL;
		downloadInfos.Clear();
		foreach (BundleInfo bundlesWithGroup in Versions.GetBundlesWithGroups(manifests.ToArray(), null))
		{
			string text = (BundleInfo.UseBundleAlias ? bundlesWithGroup.alias : bundlesWithGroup.name);
			string savePath = (string.IsNullOrEmpty(targetPath) ? Versions.GetDownloadDataPath(text) : (targetPath + "/" + text));
			if (!func(bundlesWithGroup) && !downloadInfos.Exists((DownloadInfo downloadInfo) => downloadInfo.savePath == savePath))
			{
				num += bundlesWithGroup.size;
				downloadInfos.Add(new DownloadInfo
				{
					crc = bundlesWithGroup.crc,
					url = Versions.GetDownloadURL(bundlesWithGroup.name),
					size = bundlesWithGroup.size,
					savePath = savePath
				});
			}
		}
		return num;
	}

	[BlackList]
	public (ulong totalSize, ulong downloadSize) GetDownloadSizeByPackage(Manifest manifests, List<DownloadInfo> downloadInfos, int packageId, string targetPath = null, DownloadChecker downloadChecker = null, bool useWarmup = false)
	{
		if (Versions.SkipUpdate || manifests == null)
		{
			Log.Info($"[GetDownloadSizeByPackage] skip, (${Versions.SkipUpdate} || {manifests == null})");
			return (totalSize: 0uL, downloadSize: 0uL);
		}
		Func<BundleInfo, bool> func = null;
		func = ((downloadChecker == null) ? new Func<BundleInfo, bool>(Versions.IsDownloaded) : new Func<BundleInfo, bool>(downloadChecker.IsDownloaded));
		ulong num = 0uL;
		ulong num2 = 0uL;
		List<BundleInfo> bundlesWithPackages = Versions.GetBundlesWithPackages(new Manifest[1] { manifests }, packageId);
		HashSet<string> hashSet = new HashSet<string>();
		foreach (BundleInfo item in bundlesWithPackages)
		{
			string text = (BundleInfo.UseBundleAlias ? item.alias : item.name);
			string savePath = (string.IsNullOrEmpty(targetPath) ? Versions.GetDownloadDataPath(text) : (targetPath + "/" + text));
			if (!hashSet.Contains(text))
			{
				num += item.size;
				hashSet.Add(text);
			}
			if (!func(item) && !downloadInfos.Exists((DownloadInfo downloadInfo) => downloadInfo.savePath == savePath))
			{
				num2 += item.size;
				string warmupPath = (useWarmup ? (Versions.WarmupDataPath + "/" + text) : string.Empty);
				downloadInfos.Add(new DownloadInfo
				{
					crc = item.crc,
					url = Versions.GetDownloadURL(item.name),
					size = item.size,
					savePath = savePath,
					warmupPath = warmupPath
				});
			}
		}
		return (totalSize: num, downloadSize: num2);
	}

	[BlackList]
	public ulong GetTotalSizeByPackage(Manifest manifests, int packageId)
	{
		if (Versions.SkipUpdate || manifests == null)
		{
			Log.Info($"[GetTotalSizeByPackage] skip, (${Versions.SkipUpdate} || {manifests == null})");
			return 0uL;
		}
		ulong num = 0uL;
		List<BundleInfo> bundlesWithPackages = Versions.GetBundlesWithPackages(new Manifest[1] { manifests }, packageId);
		HashSet<string> hashSet = new HashSet<string>();
		foreach (BundleInfo item2 in bundlesWithPackages)
		{
			string item = (BundleInfo.UseBundleAlias ? item2.alias : item2.name);
			if (!hashSet.Contains(item))
			{
				num += item2.size;
				hashSet.Add(item);
			}
		}
		return num;
	}

	[BlackList]
	public (ulong totalSize, ulong downloadSize) GetDownloadSizeByPackage2(Manifest manifests, List<DownloadInfo> downloadInfos, int packageId)
	{
		if (Versions.SkipUpdate || manifests == null)
		{
			Log.Info($"[GetDownloadSizeByPackage] skip, (${Versions.SkipUpdate} || {manifests == null})");
			return (totalSize: 0uL, downloadSize: 0uL);
		}
		ulong num = 0uL;
		ulong num2 = 0uL;
		List<BundleInfo> bundlesWithPackages = Versions.GetBundlesWithPackages(new Manifest[1] { manifests }, packageId);
		HashSet<string> hashSet = new HashSet<string>();
		foreach (BundleInfo item in bundlesWithPackages)
		{
			string text = (BundleInfo.UseBundleAlias ? item.alias : item.name);
			string downloadDataPath = Versions.GetDownloadDataPath(text);
			if (!hashSet.Contains(text))
			{
				num += item.size;
				hashSet.Add(text);
				item.isDownloaded = Versions.IsDownloaded(item);
				if (!item.isDownloaded)
				{
					num2 += item.size;
					string empty = string.Empty;
					downloadInfos.Add(new DownloadInfo
					{
						crc = item.crc,
						url = Versions.GetDownloadURL(item.name),
						size = item.size,
						savePath = downloadDataPath,
						warmupPath = empty
					});
				}
			}
		}
		return (totalSize: num, downloadSize: num2);
	}

	[BlackList]
	public void GetDownloadList(Manifest manifests, List<DownloadInfo> downloadInfos, int packageId, string targetPath = null)
	{
		if (Versions.SkipUpdate || manifests == null)
		{
			Log.Info($"[GetDownloadList] skip, (${Versions.SkipUpdate} || {manifests == null})");
			return;
		}
		List<BundleInfo> bundlesWithPackages = Versions.GetBundlesWithPackages(new Manifest[1] { manifests }, packageId);
		HashSet<string> hashSet = new HashSet<string>();
		foreach (BundleInfo item in bundlesWithPackages)
		{
			string text = (BundleInfo.UseBundleAlias ? item.alias : item.name);
			string savePath = (string.IsNullOrEmpty(targetPath) ? Versions.GetDownloadDataPath(text) : (targetPath + "/" + text));
			if (!hashSet.Contains(text))
			{
				hashSet.Add(text);
				downloadInfos.Add(new DownloadInfo
				{
					crc = item.crc,
					url = Versions.GetDownloadURL(item.name),
					size = item.size,
					savePath = savePath
				});
			}
		}
	}

	public void DeleteBundle(BundleInfo bundle)
	{
		ResourcePackageManager.RemoveBundle(bundle);
		Versions.DeleteBundle(bundle);
	}

	public DownloadVersions DownloadUpdates(List<DownloadInfo> downloadInfos, int queueID = 0)
	{
		return Versions.DownloadAsync(downloadInfos.ToArray(), queueID);
	}

	public void StartBkgroundDownload(List<string> manifestNames)
	{
		List<Manifest> list = new List<Manifest>();
		foreach (string manifestName in manifestNames)
		{
			Manifest manifest = Versions.GetManifest(manifestName);
			list.Add(manifest);
		}
		List<DownloadInfo> downloadInfos = new List<DownloadInfo>();
		if (GetDownloadSize(list, downloadInfos) != 0)
		{
			_updateBkground = new DownloadUpdateBkground
			{
				manifests = new List<Manifest>(list)
			};
			_updateBkground.Start(downloadInfos);
		}
	}

	public void BeginWhiteListCheck()
	{
		Versions.CheckWhiteList = true;
	}

	public bool EndWhiteListCheck()
	{
		Versions.CheckWhiteList = false;
		if (Versions.WhiteListFailed.Count > 0)
		{
			foreach (string item in Versions.WhiteListFailed)
			{
				Log.Info("whitelist failed: {0}", item);
			}
		}
		return Versions.WhiteListFailed.Count == 0;
	}

	public void Clear()
	{
		try
		{
			DynamicAtlasManager.Instance.ClearAll();
		}
		catch (Exception ex)
		{
			Log.Error("DynamicAtlas clear error : " + ex.Message);
		}
		SoftReferencePrefabManager.Instance.ClearAll();
		foreach (KeyValuePair<string, PreloadCache> item in preloadCache)
		{
			item.Value.asset.Release();
		}
		preloadCache.Clear();
		ClearGameLogicInfo();
		ClearInstance();
		objectPoolMgr.ClearAllPool();
		Versions.ClearCache();
		Log.Info("ResourceManager::Clear Versions.ClearCache");
		try
		{
			DownloadResGroupCommonManager.Instance.Clear();
		}
		catch (Exception ex2)
		{
			Log.Error("DownloadResGroupCommonManager clear error : " + ex2.Message);
		}
		Log.Info("ResourceManager::Clear");
	}

	private static bool IsAssetPathValid(string assetPath)
	{
		if (assetPath.IsNullOrEmpty())
		{
			return false;
		}
		if (assetPath.StartsWith("Assets/_Art_LastWar") && !assetPath.EndsWith(".prefab"))
		{
			UnityEngine.Debug.LogError("请勿从_Art_LastWar下直接加载资源。" + assetPath);
			return false;
		}
		return true;
	}

	public byte[] ReadAllBytes(string assetPath)
	{
		return SyncReader.ReadAllBytesByAssetPath(assetPath);
	}

	public Asset LoadAsset(string path, Type type)
	{
		path = TryConvertGPUSkinPath(path);
		return LoadAssetStatic(path, type);
	}

	public static Asset LoadAssetStatic(string path, Type type)
	{
		return Asset.Load(path, type);
	}

	public Asset LoadAssetAsync(string path, Type type)
	{
		path = TryConvertGPUSkinPath(path);
		return LoadAssetAsyncStatic(path, type);
	}

	public static Asset LoadAssetAsyncStatic(string path, Type type)
	{
		return Asset.LoadAsync(path, type);
	}

	public void PreloadAsset(string path, Type type, PreloadType preloadType = PreloadType.Cache)
	{
		float expiredTime = ((preloadType == PreloadType.Cache) ? (Time.realtimeSinceStartup + 300f) : float.MaxValue);
		if (preloadCache.TryGetValue(path, out var value))
		{
			value.expiredTime = expiredTime;
			return;
		}
		Asset asset = Asset.LoadAsync(path, type);
		preloadCache.Add(path, new PreloadCache
		{
			asset = asset,
			expiredTime = expiredTime
		});
	}

	public void UnloadAsset(Asset asset)
	{
		asset?.Release();
	}

	public bool HasAsset(string path)
	{
		string path2 = path;
		return Versions.GetAsset(ref path2) != null;
	}

	public bool IsAssetDownloaded(string path)
	{
		return Versions.IsAssetDownloaded(path);
	}

	public void UnloadUnusedAssets()
	{
		objectPoolMgr.ClearUnusedPool();
		Asset.UnloadUnusedAssets();
		Asset.DebugOutputCache();
		Bundle.DebugOutputCache();
	}

	public void UnloadUnusedAssetsSceneChange()
	{
		Asset.UnloadUnusedAssets();
	}

	public void CollectGarbage()
	{
		Log.Info("CollectGarbage begin");
		objectPoolMgr.DebugOutput();
		Asset.DebugOutputCache();
		Bundle.DebugOutputCache();
		objectPoolMgr.ClearUnusedPool();
		Asset.UnloadUnusedAssets();
		Log.Info("CollectGarbage end");
		objectPoolMgr.DebugOutput();
		Asset.DebugOutputCache();
		Bundle.DebugOutputCache();
	}

	public void DebugOutput()
	{
		objectPoolMgr.DebugOutput();
		Asset.DebugOutputCache();
		Bundle.DebugOutputCache();
	}

	public void DebugLoadCount()
	{
		Asset.DebugLoadCount();
	}

	public void RemoveCachedUnusedAssets()
	{
		Asset.RemoveCachedUnusedAssets();
	}

	public string GetRawFilePath(string path)
	{
		if (!Versions.GetDependencies(path, out var bundle, out var _))
		{
			return "";
		}
		return Versions.GetBundlePathOrURL(bundle);
	}

	public string GetResVersion()
	{
		string text = (XLuaManager.s_useLwLuaFile ? "F" : "B");
		string text2 = (ClientConfig.USE_DEV_LOCALE ? "DevLocale" : $"{ClientConfig.LOCALE_VERSION}");
		string arg = $"{ClientConfig.LWLuaVersion}{text}_{ClientConfig.CURRENT_TABLE_VERSION}.{text2}";
		int num = Versions.GetManifest("gameres")?.version ?? 0;
		int num2 = Versions.GetManifest("dllres")?.version ?? 0;
		return $"{num}.{num2}_{arg}";
	}

	public void SyncGameLogicInfo(string paramStr)
	{
		Versions.SyncGameLogicInfo(paramStr);
	}

	public void ClearGameLogicInfo()
	{
		Versions.ClearGameLogicInfo();
	}

	public void Update()
	{
		UpdateInstance();
		if ((double)(Time.realtimeSinceStartup - lastTimeCleanPoolUpdate) >= 0.1)
		{
			UpdatePoolClean();
			lastTimeCleanPoolUpdate = Time.realtimeSinceStartup;
		}
		if (Time.realtimeSinceStartup - lastTimePerSecondUpdate >= 1f)
		{
			lastTimePerSecondUpdate = Time.realtimeSinceStartup;
			UnityUIExtension.Update();
			UpdatePreloadRelease();
		}
		_updateBkground?.Update();
	}

	private void UpdatePoolClean()
	{
		objectPoolMgr.TryCleanPool();
	}

	private void UpdatePreloadRelease()
	{
		foreach (KeyValuePair<string, PreloadCache> item in preloadCache)
		{
			if (item.Value.expiredTime < Time.realtimeSinceStartup)
			{
				item.Value.asset.Release();
				keysRemove.Add(item.Key);
			}
		}
		if (keysRemove.Count <= 0)
		{
			return;
		}
		foreach (string item2 in keysRemove)
		{
			preloadCache.Remove(item2);
		}
		keysRemove.Clear();
	}

	private void OnAtlasRegistered(SpriteAtlas sa)
	{
	}

	private void OnAtlasRequested(string atlasName, Action<SpriteAtlas> callback)
	{
		Asset asset = LoadAssetStatic($"Assets/Main/Atlas/{atlasName}.spriteatlas", typeof(SpriteAtlas));
		if (!asset.isError)
		{
			callback(asset.asset as SpriteAtlas);
		}
	}

	private void UpdateInstance()
	{
		LoadQueue.UpdateLoadQueue();
	}

	private void ClearInstance()
	{
		LoadQueue.ClearAll();
	}

	public ObjectPool GetObjectPool(string prefabPath, ObjectPoolTag poolTag = ObjectPoolTag.Normal)
	{
		return objectPoolMgr.GetPool(prefabPath, this, poolTag);
	}

	public bool PrefabHasCache(string prefabPath)
	{
		return objectPoolMgr.PrefabHasCache(prefabPath);
	}

	public bool PrefabPoolIsReady(string prefabPath)
	{
		return objectPoolMgr.PrefabPoolIsReady(prefabPath);
	}

	public bool PrefabAssetsDownloaded(string prefabPath)
	{
		if (Versions.UseBundleDownloadedCache)
		{
			return Versions.IsAssetDownloaded_InCache(prefabPath);
		}
		if (Versions.GetDependencies(prefabPath, out var bundle, out var bundles))
		{
			if (bundle != null && !Versions.IsDownloaded(bundle))
			{
				return false;
			}
			if (bundles != null && bundles.Length != 0)
			{
				int i = 0;
				for (int num = bundles.Length; i < num; i++)
				{
					if (!Versions.IsDownloaded(bundles[i]))
					{
						return false;
					}
				}
			}
			return true;
		}
		return false;
	}

	public void ClearPoolByTag(ObjectPoolTag poolTag)
	{
		objectPoolMgr.ClearPoolByTag(poolTag);
	}

	public void ClearPoolByTagGroup(ObjectPoolTagGroup group)
	{
		objectPoolMgr.ClearPoolByTagGroup(group);
	}

	public InstanceRequest InstantiateAsync(string prefabPath, Func<string, BasePool> poolFunc, int property = 2)
	{
		InstanceRequest req = new InstanceRequest(prefabPath, poolFunc, property);
		if (CommonUtils.IsDebug())
		{
			if (asyncDelayTime > 0f && (asyncDelayPathSet == null || asyncDelayPathSet.Count == 0 || asyncDelayPathSet.Contains(prefabPath)))
			{
				YieldUtils.DelayActionWithOutContext(delegate
				{
					LoadQueue.AddRequest(req, property);
				}, asyncDelayTime);
			}
			else
			{
				LoadQueue.AddRequest(req, property);
			}
		}
		else
		{
			LoadQueue.AddRequest(req, property);
		}
		return req;
	}

	public InstanceRequest InstantiateAsync(string prefabPath, ObjectPoolTag poolTag = ObjectPoolTag.Normal, int property = 2)
	{
		prefabPath = TryConvertGPUSkinPath(prefabPath);
		InstanceRequest req = new InstanceRequest(prefabPath, poolTag, property);
		if (CommonUtils.IsDebug())
		{
			if (asyncDelayTime > 0f && (asyncDelayPathSet == null || asyncDelayPathSet.Count == 0 || asyncDelayPathSet.Contains(prefabPath)))
			{
				YieldUtils.DelayActionWithOutContext(delegate
				{
					LoadQueue.AddRequest(req, property);
				}, asyncDelayTime);
			}
			else
			{
				LoadQueue.AddRequest(req, property);
			}
		}
		else
		{
			LoadQueue.AddRequest(req, property);
		}
		return req;
	}

	public InstanceRequest InstantiateAsyncImmediately(string prefabPath)
	{
		prefabPath = TryConvertGPUSkinPath(prefabPath);
		InstanceRequest instanceRequest = new InstanceRequest(prefabPath);
		instanceRequest.Instantiate();
		LoadQueue.AddRequest(instanceRequest);
		return instanceRequest;
	}

	public InstanceRequest InstantiateAsyncImmediately(string prefabPath, Action<InstanceRequest> cb)
	{
		prefabPath = TryConvertGPUSkinPath(prefabPath);
		InstanceRequest instanceRequest = new InstanceRequest(prefabPath);
		instanceRequest.Instantiate();
		instanceRequest.completed += cb;
		if (instanceRequest.poolIsReady)
		{
			instanceRequest.Update();
		}
		else
		{
			LoadQueue.AddRequest(instanceRequest);
		}
		return instanceRequest;
	}

	private string TryConvertGPUSkinPath(string origPath)
	{
		if (!origPath.EndsWith(".prefab"))
		{
			return origPath;
		}
		if (!UnityEngine.Debug.isDebugBuild && !ClientSwitch.IsOn(35))
		{
			return origPath;
		}
		if (!SystemInfo.supportsInstancing || SystemInfo.graphicsShaderLevel < 45 || origPath.StartsWith("Assets/_Art_LastWar/GenGPUAnim/"))
		{
			return origPath;
		}
		string text = origPath.Replace("Assets/", "Assets/_Art_LastWar/GenGPUAnim/");
		if (HasAsset(text))
		{
			return text;
		}
		return origPath;
	}
}
