using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using JPS.WorldPointObjects;
using Protobuf;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using UnityEngine;
using XLua;

public class WorldPointManager : WorldManagerBase
{
	private Dictionary<long, IWorldLodWatcher> lodWatcherDic;

	private const int EDGE = 4;

	private const int once_max_request_count = 160;

	private bool _splitLastAOIRequest;

	public static readonly float ShowMapIconDist = 78f;

	private static readonly int MaxBuildCountOneFrame = 10;

	private static readonly int MaxDelCountOneFrame = 50;

	private static readonly Vector2Int[] ViewLevelRange = new Vector2Int[4]
	{
		new Vector2Int(25, 40),
		new Vector2Int(70, 140),
		new Vector2Int(140, 280),
		new Vector2Int(100000, 100000)
	};

	public static readonly Vector2Int[] LodRequestRange = new Vector2Int[4]
	{
		new Vector2Int(2, 10),
		new Vector2Int(15, 35),
		new Vector2Int(35, 70),
		new Vector2Int(100000, 100000)
	};

	private static string _defaultCountryFlag;

	private static HashSet<string> _allCountryFlagSet;

	private Vector2Int reqPos;

	private Vector2Int lastDelPos;

	private int LOD = 1;

	protected int svLod;

	private int lastDelSvLod;

	private Dictionary<int, WorldTileInfo> allViewPoints = new Dictionary<int, WorldTileInfo>();

	private Dictionary<int, PointInfo> yellowLand = new Dictionary<int, PointInfo>();

	private Dictionary<long, WorldTileInfo> uuidInfoMap = new Dictionary<long, WorldTileInfo>();

	private Dictionary<long, WorldTileInfo> uuidInfoDesertMap = new Dictionary<long, WorldTileInfo>();

	private Dictionary<string, long> baseMainBuild = new Dictionary<string, long>();

	private Dictionary<long, long> baseDragonPoint = new Dictionary<long, long>();

	private Dictionary<int, WorldTileInfo> outOfViewPoints = new Dictionary<int, WorldTileInfo>();

	private Dictionary<int, WorldTileInfo> outOfViewPointsObj = new Dictionary<int, WorldTileInfo>();

	private Dictionary<int, int> specialBuildDic = new Dictionary<int, int>();

	private List<int> keysToRemove = new List<int>();

	private List<WorldTileInfo> timeOutPoints = new List<WorldTileInfo>();

	private WorldTileInfo myWorldTileInfo;

	protected bool startViewRequest;

	protected bool isRecvViewPoints;

	protected bool _isPointUpdate;

	protected bool _isCityPointUpdate;

	protected bool _myPointDirty;

	private HashSet<int> toBuildList = new HashSet<int>();

	private Dictionary<int, WorldPointObject> allObjs = new Dictionary<int, WorldPointObject>();

	private Dictionary<int, bool> needDestoryBuild = new Dictionary<int, bool>();

	private List<int> _needRefreshBoard = new List<int>();

	private Dictionary<int, bool> _needRefreshDeleteBoard = new Dictionary<int, bool>();

	private Dictionary<long, bool> _needPlayPlacedAnimBuild;

	private int collectResourceTile;

	private int collectResourceRange;

	private Dictionary<int, int> BuildTileSizeDic;

	private Dictionary<int, int> AllianceCitySizeDic;

	private Dictionary<int, int> TreasureSizeDic;

	private Dictionary<int, int> DragonBuildSizeDic;

	private Dictionary<int, int> BuildOffsetRangeDic;

	private Dictionary<int, int> AllianceCityTypeDic;

	private WorldGreen mWorldGreen;

	public WorldAoiAssistanceInfos aoiAssistanceInfos;

	public WorldALPointsInfos alliancePointsInfos;

	private int alliancePointLod = 5;

	private int SelfPointLod = 8;

	private int OtherPointLod = 4;

	private static NestedDictionary<int, int, long> battlePoints = new NestedDictionary<int, int, long>();

	private static Dictionary<int, int> _cacheBuildTileSizeDic;

	private static Dictionary<int, int> _cacheAllianceCitySizeDic;

	private static Dictionary<int, int> _cacheTreasureSizeDic;

	private static Dictionary<int, int> _cacheDragonBuildSizeDic;

	private static Dictionary<int, int> _cacheBuildOffsetRangeDic;

	private static Dictionary<int, int> _cacheAllianceCityTypeDic;

	private static int _cachedServerId = -1;

	private HashSet<int> _toCreateList = new HashSet<int>();

	protected float timeCount;

	protected List<PointInfo> _pointInfos = new List<PointInfo>();

	protected List<LandPointInfo> _landPointInfos = new List<LandPointInfo>();

	protected List<WorldDesertInfo> _desertInfos = new List<WorldDesertInfo>();

	protected static long needRefreshWerewolf = 0L;

	private List<int> needCheckShowTileList = new List<int>(256);

	private static readonly int[] World9BasePosX = new int[10] { 0, 0, 2000, 4000, 0, 2000, 4000, 0, 2000, 4000 };

	private static readonly int[] World9BasePosZ = new int[10] { 0, 0, 0, 0, 2000, 2000, 2000, 4000, 4000, 4000 };

	private List<IWorldDelayDestroyObject> delayDestroyList = new List<IWorldDelayDestroyObject>();

	private NestedDictionary<int, int, bool> dirtyAssistanceList = new NestedDictionary<int, int, bool>();

	private List<long> dirtyPointMarked = new List<long>();

	private HashSet<long> tempSet = new HashSet<long>();

	private static readonly Vector2Int[] ServerLodRange = new Vector2Int[4]
	{
		new Vector2Int(25, 40),
		new Vector2Int(70, 140),
		new Vector2Int(140, 280),
		new Vector2Int(100000, 100000)
	};

	private int _lwAoiBlockSize = 4;

	private int _lwAoiBlockCount;

	private Vector3 _leftBottomWorldPos = Vector3.zero;

	private Vector3 _rightTopWorldPos = Vector3.zero;

	private Vector2Int _lastViewLBBlock;

	private Vector2Int _lastViewLTBlock;

	private Vector2Int _lastViewRBBlock;

	private Vector2Int _lastViewRTBlock;

	private HashSet<int> _msgViewIndex = new HashSet<int>();

	private List<int> _addViewIndex = new List<int>();

	private HashSet<int> _curViewIndex = new HashSet<int>();

	private static int[] _lwAoiBlockSizeArray;

	private float _outEdgeScale = 1f;

	private bool firstTimeReqAoi = true;

	private bool battleFieldFirst;

	private const int kTileCountX = 1000;

	private const int kTileCountY = 1000;

	private Dictionary<int, Dictionary<int, Color>> labelSkinColorCfg = new Dictionary<int, Dictionary<int, Color>>();

	private Dictionary<int, float> labelSkinOffsetCfg = new Dictionary<int, float>();

	private Dictionary<int, float> labelSkinSizeAddCfg = new Dictionary<int, float>();

	private bool profileSwitch = true;

	private Dictionary<int, int> tileBlockIndex = new Dictionary<int, int>();

	private bool isRecordingTileBlock;

	private int recordingWorldId = int.MinValue;

	private int focusPoint;

	private bool focusPointIsChanged;

	public static double time { get; protected set; }

	public static bool busy => (double)Time.realtimeSinceStartup >= time;

	public static string defaultCountryFlag
	{
		get
		{
			if (string.IsNullOrEmpty(_defaultCountryFlag))
			{
				_defaultCountryFlag = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetDefaultNation");
			}
			return _defaultCountryFlag;
		}
	}

	public static HashSet<string> allCountryFlagSet
	{
		get
		{
			if (_allCountryFlagSet == null)
			{
				_allCountryFlagSet = new HashSet<string>();
				LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllNationFlags");
				int i = 1;
				for (int length = luaTable.Length; i <= length; i++)
				{
					_allCountryFlagSet.Add(luaTable.Get<string>(i));
				}
			}
			return _allCountryFlagSet;
		}
	}

	public int ObjsCount => allObjs.Count;

	public WorldGreen worldGreen
	{
		get
		{
			if (mWorldGreen == null)
			{
				mWorldGreen = new WorldGreen(world);
			}
			return mWorldGreen;
		}
	}

	public bool EnableWorldAssistanceOpt { get; private set; }

	public static int[] lwAoiBlockSizeArray
	{
		get
		{
			if (_lwAoiBlockSizeArray == null)
			{
				LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetLWAoiBlockSizeArray");
				List<int> list = new List<int>();
				for (int i = 0; i <= luaTable.Length; i++)
				{
					list.Add(luaTable.Get<int>(i));
				}
				_lwAoiBlockSizeArray = list.ToArray();
			}
			return _lwAoiBlockSizeArray;
		}
	}

	private float _outEdge => (float)_lwAoiBlockSize * _outEdgeScale;

	private bool _lockAoiBlocksUpdate => world.IsCameraInMoveToState();

	public Dictionary<int, int> TileBlockIndex => tileBlockIndex;

	private void OnUpdateLod(object obj)
	{
		int lod = (int)obj;
		UpdateLodWatcher(lod);
	}

	public void RegisterLodWatcher(IWorldLodWatcher watcher)
	{
		if (watcher != null)
		{
			if (lodWatcherDic == null)
			{
				lodWatcherDic = new Dictionary<long, IWorldLodWatcher>();
			}
			if (!lodWatcherDic.ContainsKey(watcher.Uid))
			{
				lodWatcherDic.Add(watcher.Uid, watcher);
			}
		}
	}

	public void UnregisterLodWatcher(IWorldLodWatcher watcher)
	{
		if (watcher != null && lodWatcherDic != null)
		{
			lodWatcherDic.Remove(watcher.Uid);
		}
	}

	private void UpdateLodWatcher(int lod)
	{
		if (lodWatcherDic == null || lodWatcherDic.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<long, IWorldLodWatcher> item in lodWatcherDic)
		{
			item.Value.UpdateLod(lod);
		}
	}

	public override string Description()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (allObjs.Count == 0)
		{
			stringBuilder.AppendLine("哈哈，一个地物都没有，好sui哦~");
		}
		else
		{
			stringBuilder.AppendLine($"当前地物(allObjs)数量为:{allObjs.Count}");
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			List<WorldBuildObjectNew> list = new List<WorldBuildObjectNew>();
			foreach (KeyValuePair<int, WorldPointObject> allObj in allObjs)
			{
				int pointType = allObj.Value.GetPointType();
				int value = (dictionary[pointType] = ((!dictionary.TryGetValue(pointType, out value)) ? 1 : (value + 1)));
				int worldId = allObj.Value.WorldId;
				int value2 = (dictionary2[worldId] = ((!dictionary2.TryGetValue(worldId, out value2)) ? 1 : (value2 + 1)));
				if (allObj.Value is WorldBuildObjectNew)
				{
					list.Add(allObj.Value as WorldBuildObjectNew);
				}
			}
			stringBuilder.AppendLine("按照PointType分组统计：");
			foreach (KeyValuePair<int, int> item in dictionary)
			{
				stringBuilder.AppendLine($"{(WorldPointType)item.Key} = {item.Value}");
			}
			stringBuilder.AppendLine("按照WorldId分组统计：");
			foreach (KeyValuePair<int, int> item2 in dictionary2)
			{
				stringBuilder.AppendLine($"{item2.Key} = {item2.Value}");
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine($"注册LodWatcher数量:{lodWatcherDic?.Count ?? 0}");
			if (list.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("你一定想看一下玩家主城的情况...");
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				foreach (WorldBuildObjectNew item3 in list)
				{
					if (item3.CanShowLittleSmartMode)
					{
						num3++;
					}
					if (item3.IsLittleSmartModeWorking)
					{
						num4++;
					}
					if (item3.InstanceRequested)
					{
						num5++;
					}
					if (item3.TitleRequested)
					{
						num6++;
					}
				}
				stringBuilder.AppendLine($"资产加载：主:{num5}, 标题:{num6}");
				stringBuilder.AppendLine($"小聪明：可用:{num3}, 在用:{num4}");
			}
			foreach (KeyValuePair<int, WorldPointObject> allObj2 in allObjs)
			{
				if (allObj2.Value.GetPointType() == 0)
				{
					stringBuilder.AppendLine($"其他类型: type={allObj2.Value.GetType().Name}, pos={allObj2.Value.WorldPosition}");
				}
			}
			stringBuilder.AppendLine($"是否正在收集地格占用情况:{isRecordingTileBlock}");
			if (isRecordingTileBlock)
			{
				stringBuilder.AppendLine($"已收集占用数:{tileBlockIndex.Count}");
			}
			stringBuilder.AppendLine($"延迟销毁数量:{delayDestroyList.Count}");
			stringBuilder.AppendLine($"大世界驻防显示开关状态:{EnableWorldAssistanceOpt}");
			if (aoiAssistanceInfos == null)
			{
				stringBuilder.AppendLine("大世界驻防信息为空！");
			}
			else
			{
				stringBuilder.Append(aoiAssistanceInfos.Description());
			}
			if (alliancePointsInfos == null)
			{
				stringBuilder.AppendLine("大世界联盟成员坐标信息为空！");
			}
			else
			{
				stringBuilder.AppendLine(alliancePointsInfos.Description());
			}
		}
		return stringBuilder.ToString();
	}

	private void UpdateLWAoi_Normal(bool isForce)
	{
		int lwAoiBlockSize = _lwAoiBlockSize;
		RecalculateBlockArg(1000, svLod, out _lwAoiBlockSize, out _lwAoiBlockCount);
		WorldCamera camera = world.Camera;
		Vector3 vector = camera.cameraAnchor[0];
		Vector3 vector2 = camera.cameraAnchor[2];
		Vector3 vector3 = camera.cameraAnchor[1];
		Vector3 vector4 = camera.cameraAnchor[3];
		Vector2Int vector2Int = PositionToAoiBlock(vector3.x - _outEdge, vector3.z + _outEdge);
		Vector2Int vector2Int2 = PositionToAoiBlock(vector.x - _outEdge, vector.z - _outEdge);
		Vector2Int vector2Int3 = PositionToAoiBlock(vector4.x + _outEdge, vector4.z - _outEdge);
		Vector2Int vector2Int4 = PositionToAoiBlock(vector2.x + _outEdge, vector2.z + _outEdge);
		if (!isForce && !_splitLastAOIRequest && lwAoiBlockSize == _lwAoiBlockSize && _lastViewLBBlock == vector2Int2 && _lastViewLTBlock == vector2Int && _lastViewRTBlock == vector2Int4 && _lastViewRBBlock == vector2Int3)
		{
			return;
		}
		if (isForce || lwAoiBlockSize != _lwAoiBlockSize)
		{
			_curViewIndex.Clear();
		}
		AoiBlockToTilePos(vector2Int2, out var x, out var y);
		AoiBlockToTilePos(vector2Int4, out var x2, out var y2);
		_lastViewLBBlock = vector2Int2;
		_lastViewRTBlock = vector2Int4;
		_lastViewLTBlock = vector2Int;
		_lastViewRBBlock = vector2Int3;
		int x3 = vector2Int2.x;
		int x4 = vector2Int.x;
		int x5 = vector2Int3.x;
		int x6 = vector2Int4.x;
		int y3 = vector2Int3.y;
		int y4 = vector2Int4.y;
		float num = y4 - y3;
		_msgViewIndex.Clear();
		_addViewIndex.Clear();
		for (int i = y3; i <= y4; i++)
		{
			float t = ((num == 0f) ? 0f : ((float)(i - y3) / num));
			int num2 = Mathf.FloorToInt(Mathf.Lerp(x3, x4, t));
			int num3 = Mathf.CeilToInt(Mathf.Lerp(x5, x6, t));
			for (int j = num2; j <= num3; j++)
			{
				if (j * _lwAoiBlockSize < 1000 && i * _lwAoiBlockSize < 1000)
				{
					int item = AoiBlockToIndex(j, i);
					if (!_curViewIndex.Contains(item))
					{
						_addViewIndex.Add(item);
					}
					_msgViewIndex.Add(item);
				}
			}
		}
		HashSet<int> msgViewIndex = _msgViewIndex;
		HashSet<int> curViewIndex = _curViewIndex;
		_curViewIndex = msgViewIndex;
		_msgViewIndex = curViewIndex;
		float num4 = 2f;
		Vector2Int tilePos = new Vector2Int((int)((float)x / num4), (int)((float)y / num4));
		Vector2Int tilePos2 = new Vector2Int((int)((float)(x2 + _lwAoiBlockSize * 2) / num4), (int)((float)(y2 + _lwAoiBlockSize * 2) / num4));
		TileCoord.ClampTilePos(ref tilePos);
		TileCoord.ClampTilePos(ref tilePos2);
		_leftBottomWorldPos = new Vector3((float)(tilePos.x - 4) * num4, 0f, (float)(tilePos.y - 4) * num4);
		_rightTopWorldPos = new Vector3((float)(tilePos2.x + 4) * num4, 0f, (float)(tilePos2.y + 4) * num4);
		int count = _addViewIndex.Count;
		if (count > 0)
		{
			int lbTileIndex = world.TilePosToIndex(tilePos);
			int rtTileIndex = world.TilePosToIndex(tilePos2);
			Vector3 curTarget = camera.CurTarget;
			Vector2Int vector2Int5 = new Vector2Int((int)(curTarget.x / num4), (int)(curTarget.z / num4));
			if (count <= 160)
			{
				_splitLastAOIRequest = false;
				SendAoiRequest(0, svLod, Mathf.Clamp(vector2Int5.x, 0, 1000), Mathf.Clamp(vector2Int5.y, 0, 1000), _addViewIndex, _lwAoiBlockSize, lbTileIndex, rtTileIndex);
			}
			else
			{
				_splitLastAOIRequest = true;
				List<int> range = _addViewIndex.GetRange(0, 160);
				SendAoiRequest(0, svLod, Mathf.Clamp(vector2Int5.x, 0, 1000), Mathf.Clamp(vector2Int5.y, 0, 1000), range, _lwAoiBlockSize, lbTileIndex, rtTileIndex);
				for (int k = 160; k < count; k++)
				{
					_curViewIndex.Remove(_addViewIndex[k]);
				}
			}
		}
		GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateViewRange", tilePos.x, tilePos.y, tilePos2.x, tilePos2.y);
	}

	private void UpdateLWAoi_Big3000(bool isForce)
	{
		int lwAoiBlockSize = _lwAoiBlockSize;
		RecalculateBlockArg(3000, svLod, out _lwAoiBlockSize, out _lwAoiBlockCount);
		WorldCamera camera = world.Camera;
		Vector3 vector = camera.cameraAnchor[0];
		Vector3 vector2 = camera.cameraAnchor[2];
		Vector3 vector3 = camera.cameraAnchor[1];
		Vector3 vector4 = camera.cameraAnchor[3];
		Vector2Int vector2Int = PositionToAoiBlock(vector3.x - _outEdge, vector3.z + _outEdge);
		Vector2Int vector2Int2 = PositionToAoiBlock(vector.x - _outEdge, vector.z - _outEdge);
		Vector2Int vector2Int3 = PositionToAoiBlock(vector4.x + _outEdge, vector4.z - _outEdge);
		Vector2Int vector2Int4 = PositionToAoiBlock(vector2.x + _outEdge, vector2.z + _outEdge);
		if (!isForce && !_splitLastAOIRequest && lwAoiBlockSize == _lwAoiBlockSize && _lastViewLBBlock == vector2Int2 && _lastViewLTBlock == vector2Int && _lastViewRTBlock == vector2Int4 && _lastViewRBBlock == vector2Int3)
		{
			return;
		}
		if (isForce || lwAoiBlockSize != _lwAoiBlockSize)
		{
			_curViewIndex.Clear();
		}
		_lastViewLBBlock = vector2Int2;
		_lastViewRTBlock = vector2Int4;
		_lastViewLTBlock = vector2Int;
		_lastViewRBBlock = vector2Int3;
		int x = vector2Int2.x;
		int x2 = vector2Int.x;
		int x3 = vector2Int3.x;
		int x4 = vector2Int4.x;
		int y = vector2Int3.y;
		int y2 = vector2Int4.y;
		float num = y2 - y;
		_msgViewIndex.Clear();
		_addViewIndex.Clear();
		for (int i = y; i <= y2; i++)
		{
			float t = ((num == 0f) ? 0f : ((float)(i - y) / num));
			int num2 = Mathf.FloorToInt(Mathf.Lerp(x, x2, t));
			int num3 = Mathf.CeilToInt(Mathf.Lerp(x3, x4, t));
			for (int j = num2; j <= num3; j++)
			{
				if (j * _lwAoiBlockSize < 3000 && i * _lwAoiBlockSize < 3000)
				{
					int item = AoiBlockToIndex(j, i);
					if (!_curViewIndex.Contains(item))
					{
						_addViewIndex.Add(item);
					}
					_msgViewIndex.Add(item);
				}
			}
		}
		HashSet<int> msgViewIndex = _msgViewIndex;
		HashSet<int> curViewIndex = _curViewIndex;
		_curViewIndex = msgViewIndex;
		_msgViewIndex = curViewIndex;
		AoiBlockToTilePos(vector2Int2, out var x5, out var y3);
		AoiBlockToTilePos(vector2Int4, out var x6, out var y4);
		float num4 = 2f;
		Vector2Int vector2Int5 = new Vector2Int((int)((float)x5 / num4), (int)((float)y3 / num4));
		Vector2Int vector2Int6 = new Vector2Int((int)((float)(x6 + _lwAoiBlockSize * 2) / num4), (int)((float)(y4 + _lwAoiBlockSize * 2) / num4));
		vector2Int5.x = Mathf.Clamp(vector2Int5.x, 0, 2999);
		vector2Int5.y = Mathf.Clamp(vector2Int5.y, 0, 2999);
		vector2Int6.x = Mathf.Clamp(vector2Int6.x, 0, 2999);
		vector2Int6.y = Mathf.Clamp(vector2Int6.y, 0, 2999);
		_leftBottomWorldPos = new Vector3((float)(vector2Int5.x - 4) * num4, 0f, (float)(vector2Int5.y - 4) * num4);
		_rightTopWorldPos = new Vector3((float)(vector2Int6.x + 4) * num4, 0f, (float)(vector2Int6.y + 4) * num4);
		Vector3 curTarget = camera.CurTarget;
		int serverIdFromWorldPos = SeasonDataManager.Instance.GetServerIdFromWorldPos(curTarget);
		if (serverIdFromWorldPos > 0 && serverIdFromWorldPos != GameEntry.Data.Player.GetCurServerId())
		{
			world.ChangeServer(serverIdFromWorldPos);
		}
		int count = _addViewIndex.Count;
		if (count > 0)
		{
			int lbTileIndex = vector2Int5.x + vector2Int5.y * 3000 + 1;
			int rtTileIndex = vector2Int6.x + vector2Int6.y * 3000 + 1;
			Vector3 curTarget2 = camera.CurTarget;
			Vector2Int vector2Int7 = new Vector2Int((int)(curTarget2.x / num4), (int)(curTarget2.z / num4));
			if (count <= 160)
			{
				_splitLastAOIRequest = false;
				SendAoiRequest(1, svLod, Mathf.Clamp(vector2Int7.x, 0, 2999), Mathf.Clamp(vector2Int7.y, 0, 2999), _addViewIndex, _lwAoiBlockSize, lbTileIndex, rtTileIndex);
			}
			else
			{
				_splitLastAOIRequest = true;
				List<int> range = _addViewIndex.GetRange(0, 160);
				SendAoiRequest(1, svLod, Mathf.Clamp(vector2Int7.x, 0, 2999), Mathf.Clamp(vector2Int7.y, 0, 2999), range, _lwAoiBlockSize, lbTileIndex, rtTileIndex);
				for (int k = 160; k < count; k++)
				{
					_curViewIndex.Remove(_addViewIndex[k]);
				}
			}
		}
		GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateViewRange", vector2Int5.x, vector2Int5.y, vector2Int6.x, vector2Int6.y);
	}

	private void SendAoiRequest(int bigMap, int serverLod, int x, int y, List<int> addIndex, int blockSize, int lbTileIndex, int rtTileIndex)
	{
		if (serverLod > 2)
		{
			serverLod = 2;
		}
		int worldId = GameEntry.Data.Player.GetWorldId();
		int num = GameEntry.Data.Player.GetSelfServerId();
		bool flag = GameEntry.Data.Player.IsInSelfServer();
		if (!flag)
		{
			num = GameEntry.Data.Player.GetCrossServerId();
		}
		WorldGetBlockMessage.Instance.Send(new WorldGetBlockMessage.Request
		{
			bigMap = bigMap,
			x = x,
			y = y,
			type = 0,
			lod = serverLod,
			serverId = num,
			worldId = worldId,
			index = addIndex.ToArray(),
			blockSize = blockSize,
			firstTime = firstTimeReqAoi,
			leftBottom = lbTileIndex,
			rightTop = rtTileIndex,
			battleFieldFirst = battleFieldFirst
		});
		firstTimeReqAoi = false;
		battleFieldFirst = false;
		if (!CrossServerComponent.OnlyMainLine)
		{
			bool flag2 = GameEntry.Data.Player.IsInBattleField();
			if (!flag || flag2)
			{
				WorldCrossServerMessage.Instance.SendReqest(new WorldCrossServerMessage.RequestParam
				{
					x = x,
					y = y,
					type = 0,
					lod = serverLod,
					forceServerId = num,
					worldId = worldId,
					leftBottom = lbTileIndex,
					rightTop = rtTileIndex
				});
			}
		}
	}

	public WorldPointManager(WorldScene scene)
		: base(scene)
	{
		_needPlayPlacedAnimBuild = new Dictionary<long, bool>();
	}

	public override void Init()
	{
		collectResourceRange = GameEntry.Lua.CallWithReturn<int, string, string>("CSharpCallLuaInterface.GetConfigNum", "collect_resource_birth", "k2");
		collectResourceTile = GameEntry.Lua.CallWithReturn<int, string, string>("CSharpCallLuaInterface.GetConfigNum", "collect_resource_birth", "k1");
		InitBuildTileDictionary();
		LOD = world.GetLodLevel();
		GameEntry.Event.Subscribe(EventId.ScoutDragonScore, OnScoutDragonScore);
		GameEntry.Event.Subscribe(EventId.ChangeCameraLod, OnUpdateLod);
		GameEntry.Event.Subscribe(EventId.PlayerSwitchStateInitCompleted, RefreshPlayerSwitchState);
		GameEntry.Event.Subscribe(EventId.RefreshAllAllianceMainBuildingsInView, OnRefreshAllAllianceBuildingsInView);
		RefreshPlayerSwitchState();
	}

	public override void UnInit()
	{
		GameEntry.Event.Unsubscribe(EventId.ScoutDragonScore, OnScoutDragonScore);
		GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, OnUpdateLod);
		GameEntry.Event.Unsubscribe(EventId.PlayerSwitchStateInitCompleted, RefreshPlayerSwitchState);
		GameEntry.Event.Unsubscribe(EventId.RefreshAllAllianceMainBuildingsInView, OnRefreshAllAllianceBuildingsInView);
		allViewPoints.Clear();
		uuidInfoMap.Clear();
		uuidInfoDesertMap.Clear();
		yellowLand.Clear();
		baseMainBuild.Clear();
		baseDragonPoint.Clear();
		outOfViewPointsObj.Clear();
		outOfViewPoints.Clear();
		toBuildList.Clear();
		dirtyAssistanceList.Clear();
		dirtyPointMarked.Clear();
		ClearALMemberPoints();
		aoiAssistanceInfos?.Clear();
		foreach (WorldPointObject value in allObjs.Values)
		{
			value.Destroy();
		}
		_needPlayPlacedAnimBuild.Clear();
		myWorldTileInfo = null;
		ClearConfigCache();
		focusPoint = -1;
		focusPointIsChanged = false;
		lodWatcherDic?.Clear();
		mWorldGreen?.Clear();
		ClearAllDelayDestroyList();
	}

	public void RemoveAllObject()
	{
		allViewPoints.Clear();
		uuidInfoMap.Clear();
		uuidInfoDesertMap.Clear();
		yellowLand.Clear();
		baseMainBuild.Clear();
		baseDragonPoint.Clear();
		outOfViewPoints.Clear();
		outOfViewPointsObj.Clear();
		toBuildList.Clear();
		dirtyAssistanceList.Clear();
		dirtyPointMarked.Clear();
		aoiAssistanceInfos?.Clear();
		focusPoint = -1;
		focusPointIsChanged = false;
		foreach (WorldPointObject value in allObjs.Values)
		{
			value.Destroy();
		}
		allObjs.Clear();
		_needPlayPlacedAnimBuild.Clear();
		myWorldTileInfo = null;
		StopRecordTileBlock();
		mWorldGreen?.Clear();
		ClearAllDelayDestroyList();
	}

	public bool IsBuildFinish()
	{
		if (isRecvViewPoints)
		{
			return toBuildList.Count < 100;
		}
		return false;
	}

	private void InitBuildTileDictionary()
	{
		if (_cacheBuildTileSizeDic != null)
		{
			int curServerId = GameEntry.Data.Player.GetCurServerId();
			if (!GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.GetNeedRefreshWorldPointSize") && curServerId == _cachedServerId)
			{
				BuildTileSizeDic = _cacheBuildTileSizeDic;
				AllianceCitySizeDic = _cacheAllianceCitySizeDic;
				TreasureSizeDic = _cacheTreasureSizeDic;
				DragonBuildSizeDic = _cacheDragonBuildSizeDic;
				BuildOffsetRangeDic = _cacheBuildOffsetRangeDic;
				AllianceCityTypeDic = _cacheAllianceCityTypeDic;
				return;
			}
		}
		BuildTileSizeDic = new Dictionary<int, int>();
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllBuildTileByItemId");
		if (luaTable != null)
		{
			for (int i = 1; i <= luaTable.Length; i++)
			{
				LuaTable luaTable2 = (LuaTable)luaTable[i];
				if (luaTable2.ContainsKey("itemId") && luaTable2.ContainsKey("tiles"))
				{
					int num = luaTable2.Get<int>("itemId");
					int value = luaTable2.Get<int>("tiles");
					if (num == 10100000)
					{
						value = GameEntry.Lua.CallWithReturn<int, string, string>("CSharpCallLuaInterface.GetConfigNum", "worldmap_city", "k12");
					}
					BuildTileSizeDic[num] = value;
				}
			}
		}
		_cacheBuildTileSizeDic = BuildTileSizeDic;
		AllianceCitySizeDic = new Dictionary<int, int>();
		AllianceCityTypeDic = new Dictionary<int, int>();
		luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllyCitySize");
		if (luaTable != null)
		{
			for (int j = 1; j <= luaTable.Length; j++)
			{
				LuaTable luaTable3 = (LuaTable)luaTable[j];
				if (luaTable3.ContainsKey("itemId") && luaTable3.ContainsKey("size"))
				{
					int key = luaTable3.Get<int>("itemId");
					int value2 = luaTable3.Get<int>("size");
					int value3 = luaTable3.Get<int>("cityType");
					AllianceCitySizeDic[key] = value2;
					AllianceCityTypeDic[key] = value3;
				}
			}
		}
		_cacheAllianceCitySizeDic = AllianceCitySizeDic;
		_cacheAllianceCityTypeDic = AllianceCityTypeDic;
		TreasureSizeDic = new Dictionary<int, int>();
		_cacheTreasureSizeDic = TreasureSizeDic;
		DragonBuildSizeDic = new Dictionary<int, int>();
		luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetALLDragonBuildSize");
		if (luaTable != null)
		{
			for (int k = 1; k <= luaTable.Length; k++)
			{
				LuaTable luaTable4 = (LuaTable)luaTable[k];
				if (luaTable4.ContainsKey("itemId") && luaTable4.ContainsKey("size"))
				{
					int key2 = luaTable4.Get<int>("itemId");
					int value4 = luaTable4.Get<int>("size");
					DragonBuildSizeDic[key2] = value4;
				}
			}
		}
		_cacheDragonBuildSizeDic = DragonBuildSizeDic;
		BuildOffsetRangeDic = new Dictionary<int, int>();
		luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllBuildOffset");
		if (luaTable != null)
		{
			for (int l = 1; l <= luaTable.Length; l++)
			{
				LuaTable luaTable5 = (LuaTable)luaTable[l];
				if (luaTable5.ContainsKey("id") && luaTable5.ContainsKey("offer_range"))
				{
					int key3 = luaTable5.Get<int>("id");
					int value5 = luaTable5.Get<int>("offer_range");
					BuildOffsetRangeDic[key3] = value5;
				}
			}
		}
		_cacheBuildOffsetRangeDic = BuildOffsetRangeDic;
		_cachedServerId = GameEntry.Data.Player.GetCurServerId();
	}

	public PointInfo GetPointInfo(int pointIndex, int serverId)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null && (world == null || world.WorldSize < 3000 || serverId <= 0 || pointInfo.serverId == serverId))
		{
			return pointInfo;
		}
		return null;
	}

	public PointInfo GetPointInfo(int pointIndex)
	{
		return GetWorldTileInfo(pointIndex)?.GetPointInfo();
	}

	public WorldDesertInfo GetWorldDesertInfo(int pointIndex)
	{
		return GetWorldTileInfo(pointIndex)?.GetWorldDesertInfo();
	}

	public WorldTileInfo GetWorldTileInfo(int pointIndex)
	{
		if (allViewPoints.TryGetValue(pointIndex, out var value))
		{
			return value;
		}
		return null;
	}

	public PointInfo GetYellowLand(int pointIndex)
	{
		if (yellowLand.TryGetValue(pointIndex, out var value))
		{
			return value;
		}
		return null;
	}

	public int GetBuildTileByItemId(int itemId)
	{
		int result = -1;
		if (BuildTileSizeDic.ContainsKey(itemId))
		{
			result = BuildTileSizeDic[itemId];
		}
		return result;
	}

	public int GetAllianceCitySizeByItemId(int itemId)
	{
		int result = 1;
		if (AllianceCitySizeDic.ContainsKey(itemId))
		{
			result = AllianceCitySizeDic[itemId];
		}
		return result;
	}

	public int GetTreasureSizeByItemId(int itemId)
	{
		int result = 1;
		if (TreasureSizeDic.ContainsKey(itemId))
		{
			result = TreasureSizeDic[itemId];
		}
		return result;
	}

	public int GetDragonBuildSizeByItemId(int itemId)
	{
		int result = 1;
		if (DragonBuildSizeDic.ContainsKey(itemId))
		{
			result = DragonBuildSizeDic[itemId];
		}
		return result;
	}

	public int GetAllianceCityTypeByItemId(int itemId)
	{
		if (AllianceCityTypeDic != null && AllianceCityTypeDic.TryGetValue(itemId, out var value))
		{
			return value;
		}
		return 0;
	}

	public int GetBuildOffsetRangeByBuildId(int buildId)
	{
		int result = 0;
		if (BuildOffsetRangeDic.ContainsKey(buildId))
		{
			result = BuildOffsetRangeDic[buildId];
		}
		return result;
	}

	public PointInfo GetPointInfoByUuid(long uuid)
	{
		if (uuidInfoMap.TryGetValue(uuid, out var value))
		{
			PointInfo pointInfo = value.GetPointInfo();
			if (pointInfo != null)
			{
				return pointInfo;
			}
		}
		return null;
	}

	public WorldDesertInfo GetDesertInfoByUuid(long uuid)
	{
		if (uuidInfoDesertMap.TryGetValue(uuid, out var value))
		{
			WorldDesertInfo worldDesertInfo = value.GetWorldDesertInfo();
			if (worldDesertInfo != null)
			{
				return worldDesertInfo;
			}
		}
		return null;
	}

	public PointInfo GetMyPointInfo()
	{
		if (myWorldTileInfo != null)
		{
			return myWorldTileInfo.GetPointInfo();
		}
		return null;
	}

	public bool HasPointInfo(int pointIndex)
	{
		WorldTileInfo worldTileInfo = GetWorldTileInfo(pointIndex);
		if (worldTileInfo != null && worldTileInfo.GetPointInfo() != null)
		{
			return true;
		}
		return false;
	}

	public bool IsSelfPoint(int pointIndex)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			return pointInfo.ownerUid == GameEntry.Data.Player.Uid;
		}
		return false;
	}

	public int GetDesertPoint(int lv, int type)
	{
		foreach (WorldTileInfo value in uuidInfoDesertMap.Values)
		{
			WorldDesertInfo worldDesertInfo = value.GetWorldDesertInfo();
			int num = GameEntry.ConfigCache.GetTemplateData("desert", worldDesertInfo.desertId, "desert_type").ToInt();
			int num2 = GameEntry.ConfigCache.GetTemplateData("desert", worldDesertInfo.desertId, "desert_level").ToInt();
			if (lv == num2 && type == num && worldDesertInfo.GetPlayerType() == PlayerType.PlayerNone)
			{
				return worldDesertInfo.pointIndex;
			}
		}
		return 0;
	}

	public Dictionary<long, WorldTileInfo> GetDesertPointList()
	{
		return uuidInfoDesertMap;
	}

	public bool IsCollectPoint(int pointIndex)
	{
		return GetCollectInfoByIndex(pointIndex) != null;
	}

	public bool IsCollectRangePoint(int pointIndex)
	{
		return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsCollectRangePoint", pointIndex);
	}

	public CollectPointInfo GetCollectRangePoint(int pointIndex)
	{
		if (GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsCollectRangePoint", pointIndex))
		{
			return GameEntry.Lua.CallWithReturn<CollectPointInfo, int>("CSharpCallLuaInterface.GetCollectRangePoint", pointIndex);
		}
		return null;
	}

	public CollectPointInfo GetCollectInfoByIndex(int pointIndex)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.WorldCollectResource)
		{
			return pointInfo as CollectPointInfo;
		}
		return null;
	}

	public ResPointInfo GetResourcePointInfoByIndex(int pointIndex)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.WorldResource)
		{
			return pointInfo as ResPointInfo;
		}
		return null;
	}

	public ExplorePointInfo GetExplorePointInfoByIndex(int pointIndex)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null && (pointInfo.pointType == WorldPointType.EXPLORE_POINT || pointInfo.pointType == WorldPointType.DETECT_EVENT_PVE))
		{
			return pointInfo as ExplorePointInfo;
		}
		return null;
	}

	public SamplePointInfo GetSamplePointInfoByIndex(int pointIndex)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null && (pointInfo.pointType == WorldPointType.SAMPLE_POINT || pointInfo.pointType == WorldPointType.SAMPLE_POINT_NEW || pointInfo.pointType == WorldPointType.RESCUE_POINT || pointInfo.pointType == WorldPointType.RADAR_DOMINATOR_CURE || pointInfo.pointType == WorldPointType.TreasureChest || pointInfo.pointType == WorldPointType.RADAR_DOMINATOR_COCKATRICE_UNLOCK_2 || pointInfo.pointType == WorldPointType.SkyBattle))
		{
			return pointInfo as SamplePointInfo;
		}
		return null;
	}

	public DetectRetryTaskPointInfo GetDetectRetryTaskPointInfo(int pointIndex)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.DETECT_RETRY_TASK)
		{
			return pointInfo as DetectRetryTaskPointInfo;
		}
		return null;
	}

	public DetectAttackCityS0TaskPointInfo GetDetectAttackCityS0TaskPointInfo(int pointIndex)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.DETECT_ALLIANCE_CITY_SCOUT_MONSTER)
		{
			return pointInfo as DetectAttackCityS0TaskPointInfo;
		}
		return null;
	}

	public HeroDispatchMissionPointInfo GetHeroDispatchTaskPointInfoByIndex(int pointIndex)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.HERO_DISPATCH)
		{
			return pointInfo as HeroDispatchMissionPointInfo;
		}
		return null;
	}

	public GhostreconPointInfo GetGhostreconPointInfoByIndex(int pointIndex)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.GHOSTRECON_POINT)
		{
			return pointInfo as GhostreconPointInfo;
		}
		return null;
	}

	public GarbagePointInfo GetGarbagePointInfoByIndex(int pointIndex)
	{
		PointInfo pointInfo = GetPointInfo(pointIndex);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.GARBAGE)
		{
			return pointInfo as GarbagePointInfo;
		}
		return null;
	}

	public bool IsRoadPoint(int index, string uid, int dir)
	{
		int indexByOffsetByDirection = world.GetIndexByOffsetByDirection(index, dir);
		PointInfo pointInfo = GetPointInfo(indexByOffsetByDirection);
		if (pointInfo != null && pointInfo.ownerUid == uid)
		{
			if (pointInfo.pointType == WorldPointType.PlayerRoad)
			{
				return true;
			}
			if (pointInfo.pointType == WorldPointType.PlayerBuilding && pointInfo is BuildPointInfo { itemId: 10100000 } buildPointInfo)
			{
				int index2 = GameEntry.Lua.CallWithReturn<int, int, int>("CSharpCallLuaInterface.GetBuildModelCenter", buildPointInfo.mainIndex, buildPointInfo.tileSize);
				int num = buildPointInfo.tileSize / 2;
				if (indexByOffsetByDirection == SceneManager.World.GetIndexByOffset(index2, num) || indexByOffsetByDirection == SceneManager.World.GetIndexByOffset(index2, -num) || indexByOffsetByDirection == SceneManager.World.GetIndexByOffset(index2, 0, num) || indexByOffsetByDirection == SceneManager.World.GetIndexByOffset(index2, 0, -num))
				{
					return true;
				}
			}
		}
		return false;
	}

	public override void OnUpdate(float deltaTime)
	{
		time = Time.realtimeSinceStartup + 0.015f;
		int lodLevel = world.GetLodLevel();
		svLod = GetServerLod(lodLevel);
		UpdateLWAoi();
		DeleteTimeOutPoints();
		ViewCulling();
		AsyncCreate();
		ObjectsOnUpdate(deltaTime);
		DeleteOutOfAoi();
		UpdateDelayDestroyList();
		UpdateAssistanceChanged();
		UpdateDirtyPoints();
		if (_isPointUpdate)
		{
			_isPointUpdate = false;
			GameEntry.Event.Fire(EventId.UPDATE_POINTS_DATA);
		}
		if (_isCityPointUpdate)
		{
			_isCityPointUpdate = false;
			GameEntry.Event.Fire(EventId.UPDATE_CITY_POINTS_DATA);
		}
		if (focusPointIsChanged)
		{
			focusPointIsChanged = false;
			GameEntry.Event.Fire(EventId.FocusPointChanged, focusPoint);
		}
		timeCount += deltaTime;
		if (timeCount > 1f)
		{
			timeCount -= 1f;
			Update1000MS();
		}
	}

	protected void Update1000MS()
	{
		if (_myPointDirty)
		{
			_myPointDirty = false;
			if (myWorldTileInfo != null)
			{
				PointInfo pointInfo = myWorldTileInfo.GetPointInfo();
				if (pointInfo != null && pointInfo is BuildPointInfo buildPointInfo)
				{
					long num = 0L;
					int param = 0;
					if (buildPointInfo.sandWorm != null)
					{
						num = buildPointInfo.sandWorm.expireTime;
						param = buildPointInfo.sandWorm.monsterId;
					}
					GameEntry.Lua.Call("CSharpCallLuaInterface.SetMyBaseWormWrap", num, param);
					if (num == 0L)
					{
						world.HandleSandWormUpdate(buildPointInfo, SandWormAnim.MoveCity);
					}
				}
			}
		}
		if (needRefreshWerewolf <= 0)
		{
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		if (serverTime < needRefreshWerewolf)
		{
			return;
		}
		needRefreshWerewolf = 0L;
		foreach (PointInfo pointInfo2 in _pointInfos)
		{
			if (pointInfo2 is BuildPointInfo { IsWerewolf: not false } buildPointInfo2)
			{
				buildPointInfo2.UncoverWerewolf(serverTime);
				MarkPointIsDirty(buildPointInfo2.uuid);
			}
		}
	}

	private void DeleteTimeOutPoints()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		foreach (WorldTileInfo value in allViewPoints.Values)
		{
			PointInfo pointInfo = value.GetPointInfo();
			if (pointInfo == null)
			{
				continue;
			}
			if (pointInfo is GarbagePointInfo garbagePointInfo)
			{
				if (garbagePointInfo.endTime < serverTime)
				{
					timeOutPoints.Add(value);
				}
				continue;
			}
			if (pointInfo is HeroDispatchMissionPointInfo heroDispatchMissionPointInfo && heroDispatchMissionPointInfo.actEndTime < serverTime && heroDispatchMissionPointInfo.expiredTime < serverTime)
			{
				timeOutPoints.Add(value);
			}
			if (pointInfo is GhostreconPointInfo ghostreconPointInfo && ghostreconPointInfo.actEndTime < serverTime)
			{
				timeOutPoints.Add(value);
			}
			if (pointInfo is WorldMeteoritePoint worldMeteoritePoint && worldMeteoritePoint.ExpiredTime > 0 && worldMeteoritePoint.ExpiredTime < serverTime)
			{
				timeOutPoints.Add(value);
			}
		}
		if (timeOutPoints.Count <= 0)
		{
			return;
		}
		foreach (WorldTileInfo timeOutPoint in timeOutPoints)
		{
			PointInfo pointInfo2 = timeOutPoint.GetPointInfo();
			if (pointInfo2 != null && pointInfo2.isMainPoint)
			{
				DestroyObject(timeOutPoint.pointIndex);
				RemovePointInfo(timeOutPoint.pointIndex, forceRemove: false);
			}
		}
		timeOutPoints.Clear();
	}

	public void HandleViewPointsReply(ISFSObject message)
	{
		if (message.ContainsKey("errorCode"))
		{
			Log.Error("HandleViewRep: {0}", message.GetUtfString("errorCode"));
			return;
		}
		ISFSArray sFSArray = message.GetSFSArray("serverPointArr");
		if (sFSArray != null)
		{
			int count = sFSArray.Count;
			for (int i = 0; i < count; i++)
			{
				ParseWorldGetBlock(sFSArray.GetSFSObject(i));
			}
		}
		else
		{
			ParseWorldGetBlock(message);
		}
	}

	private void HeatSourceDataManagerHandleWorldGetBlock(SFSObject msg, int lb, int rt)
	{
		if (msg != null)
		{
			HeatSourceDataManager.GetInstance().HandleWorldGetBlock(msg, lb, rt);
		}
	}

	private void ParseWorldGetBlock(ISFSObject message)
	{
		_pointInfos.Clear();
		_landPointInfos.Clear();
		_desertInfos.Clear();
		int num = message.GetInt("serverId");
		int num2 = message.GetInt("worldId");
		ISFSArray sFSArray = message.GetSFSArray("points");
		if (sFSArray != null)
		{
			for (int i = 0; i < sFSArray.Count; i++)
			{
				WorldPointInfo p = WorldPointInfo.Parser.ParseFrom(sFSArray.GetByteArray(i).Bytes);
				_pointInfos.Add(NewPointInfo(p, isCreate: false));
			}
		}
		ISFSArray sFSArray2 = message.GetSFSArray("lands");
		if (sFSArray2 != null)
		{
			for (int j = 0; j < sFSArray2.Count; j++)
			{
				LandPointInfo item = LandPointInfo.Parser.ParseFrom(sFSArray2.GetByteArray(j).Bytes);
				_landPointInfos.Add(item);
			}
		}
		if (EnableWorldAssistanceOpt)
		{
			ByteArray byteArray = message.GetByteArray("alAssistanceInfo");
			if (byteArray != null)
			{
				aoiAssistanceInfos = aoiAssistanceInfos ?? new WorldAoiAssistanceInfos();
				aoiAssistanceInfos.UpdateFromBlock(byteArray, num, num2);
			}
		}
		ISFSArray sFSArray3 = message.GetSFSArray("deserts");
		if (sFSArray3 != null)
		{
			for (int k = 0; k < sFSArray3.Count; k++)
			{
				DesertInfo di = DesertInfo.Parser.ParseFrom(sFSArray3.GetByteArray(k).Bytes);
				_desertInfos.Add(new WorldDesertInfo(di));
			}
		}
		ISFSArray sFSArray4 = message.GetSFSArray("alInfos");
		if (sFSArray4 != null)
		{
			for (int l = 0; l < sFSArray4.Count; l++)
			{
				WorldPointInfo p2 = WorldPointInfo.Parser.ParseFrom(sFSArray4.GetByteArray(l).Bytes);
				_pointInfos.Add(NewPointInfo(p2, isCreate: false));
			}
		}
		ISFSObject sFSObject = message.GetSFSObject("worldFlags");
		if (sFSObject != null && sFSObject is SFSObject sfsData)
		{
			LuaStackTable param = sfsData.ToLuaTable(GameEntry.Lua.Env);
			GameEntry.Lua.Call("CSharpCallLuaInterface.CreateWarFlags", param);
		}
		ISFSObject sFSObject2 = message.GetSFSObject("effectObj");
		if (sFSObject2 != null && sFSObject2 is SFSObject sfsData2)
		{
			LuaStackTable param2 = sfsData2.ToLuaTable(GameEntry.Lua.Env);
			GameEntry.Lua.Call("CSharpCallLuaInterface.CreateWarEffect", param2, num, num2);
		}
		ISFSArray sFSArray5 = message.GetSFSArray("protectedBuildingInfos");
		if (sFSArray5 != null && sFSArray5 is SFSArray array)
		{
			LuaStackTable param3 = array.ToLuaTable(GameEntry.Lua.Env);
			GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateProtectedCityInfos", param3, num, num2);
		}
		ISFSObject sFSObject3 = message.GetSFSObject("hotSpots");
		if (sFSObject3 != null)
		{
			int lb = message.GetInt("leftBottom");
			int rt = message.GetInt("rightTop");
			HeatSourceDataManager.GetInstance().HandleWorldGetBlock(sFSObject3, lb, rt);
		}
		ISFSObject sFSObject4 = message.GetSFSObject("triggers");
		world.HandleTriggerWorldGetBlock(sFSObject4, num, num2);
		ByteArray byteArray2 = message.GetByteArray("v2WolfPoints");
		if (byteArray2?.Bytes != null)
		{
			WorldView2WolfPointInfoMsg worldView2WolfPointInfoMsg = WorldView2WolfPointInfoMsg.Parser.ParseFrom(byteArray2.Bytes);
			if (worldView2WolfPointInfoMsg != null)
			{
				world.HandleWerewolfWorldGetBlock(worldView2WolfPointInfoMsg);
			}
		}
		ISFSArray sFSArray6 = message.GetSFSArray("greenPoints");
		if (sFSArray6 != null)
		{
			worldGreen.ParseWorldGreenArea(sFSArray6, message);
		}
		ByteArray byteArray3 = message.GetByteArray("cityGreen");
		if (byteArray3 != null)
		{
			worldGreen.ParseWorldCityGreens(WorldAllCityGreenInfo.Parser.ParseFrom(byteArray3.Bytes));
		}
		ByteArray byteArray4 = message.GetByteArray("lightRangeData");
		if (byteArray4?.Bytes != null)
		{
			RangeLightData rangeLightData = RangeLightData.Parser.ParseFrom(byteArray4.Bytes);
			if (rangeLightData != null)
			{
				LightDataManager.GetInstance().HandleWorldGetBlock(rangeLightData, world);
			}
		}
		isRecvViewPoints = true;
		bool flag = SeasonDataManager.Instance.InSeasonBigMapMode();
		string uid = GameEntry.Data.Player.Uid;
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		int worldId = GameEntry.Data.Player.GetWorldId();
		foreach (PointInfo pointInfo in _pointInfos)
		{
			if ((pointInfo is SamplePointInfo && (pointInfo as SamplePointInfo).ownerUid != uid && pointInfo.pointType != WorldPointType.TreasureChest && pointInfo.pointType != WorldPointType.SkyBattle) || (pointInfo is ExplorePointInfo && (pointInfo as ExplorePointInfo).ownerUid != uid) || (pointInfo is GarbagePointInfo && (pointInfo as GarbagePointInfo).ownerUid != uid) || pointInfo.worldId != worldId)
			{
				continue;
			}
			if (pointInfo.serverId > 0 && worldId == 0)
			{
				if (flag)
				{
					if (!SeasonDataManager.Instance.InNinePalacesList(pointInfo.serverId))
					{
						continue;
					}
				}
				else if (pointInfo.serverId != curServerId)
				{
					continue;
				}
			}
			if (pointInfo.pointType == WorldPointType.MONSTER_REWARD)
			{
				byte[] extraInfo = pointInfo.extraInfo;
				if (CollectRewardInfo.Parser.ParseFrom(extraInfo).OwnerUid != uid)
				{
					continue;
				}
			}
			AddPointInfo(pointInfo);
			if (!pointInfo.isMainPoint)
			{
				continue;
			}
			bool flag2 = true;
			if (pointInfo.pointType == WorldPointType.PlayerRoad && pointInfo is BoardPointInfo { inside: >0 })
			{
				flag2 = false;
			}
			if (flag2)
			{
				if (pointInfo.pointType == WorldPointType.TREASURE && pointInfo is TreasurePointInfo treasurePointInfo && treasurePointInfo.killerId == GameEntry.Data.Player.Uid)
				{
					Log.Info($"AddToBuildList with killerId {pointInfo.uuid} , {pointInfo.mainIndex}");
				}
				AddToBuildList(pointInfo.pointIndex);
			}
		}
		CheckNeedRefreshRoad();
		MarkPointUpdate();
		GameEntry.Event.Fire(EventId.WorldGetBlockMsg);
		world.SetLandPointInfos(_landPointInfos);
		foreach (WorldDesertInfo desertInfo in _desertInfos)
		{
			if ((desertInfo.serverId <= 0 || desertInfo.serverId == GameEntry.Data.Player.GetCurServerId()) && !GameEntry.Data.Player.IsInBattleField())
			{
				AddDesertInfo(desertInfo);
				AddToBuildList(desertInfo.pointIndex);
			}
		}
	}

	public void ParseWorldPointFoldUp(ISFSObject message, int serverId = 0, int worldId = 0)
	{
		keysToRemove.Clear();
		ISFSArray sFSArray = message.GetSFSArray("pointIds");
		for (int i = 0; i < sFSArray.Count; i++)
		{
			keysToRemove.Add(sFSArray.GetInt(i));
		}
		if (keysToRemove.Count > 0)
		{
			foreach (int item in keysToRemove)
			{
				RemoveFromBuildList(item);
				FoldUpBuildObject(item);
				RemovePointInfo(item, forceRemove: false);
			}
			CheckNeedRefreshRoad();
		}
		MarkPointUpdate();
	}

	public void ParseWorldPointRemove(ISFSObject message, int serverId = 0, int worldId = 0)
	{
		keysToRemove.Clear();
		ISFSArray sFSArray = message.GetSFSArray("pointIds");
		for (int i = 0; i < sFSArray.Count; i++)
		{
			keysToRemove.Add(sFSArray.GetInt(i));
		}
		if (keysToRemove.Count > 0)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			_needRefreshBoard.Clear();
			_needRefreshDeleteBoard.Clear();
			foreach (int item in keysToRemove)
			{
				RemoveFromBuildList(item);
				WorldPointObject objectByPoint = GetObjectByPoint(item);
				int num = -1;
				if (objectByPoint != null)
				{
					num = objectByPoint.GetPointType();
					if (objectByPoint is WorldMeteoritePointObject worldMeteoritePointObject)
					{
						worldMeteoritePointObject.TryDelayDestroy(serverTime);
					}
					else if ((!(objectByPoint is WorldDetectEventItemObject worldDetectEventItemObject) || !worldDetectEventItemObject.DoDisappear()) && (!(objectByPoint is WorldHeroDispatchTaskObject worldHeroDispatchTaskObject) || !worldHeroDispatchTaskObject.DoDisappear()) && (!(objectByPoint is WorldGhostreconTaskObject worldGhostreconTaskObject) || !worldGhostreconTaskObject.DoDisappear()))
					{
						DestroyObject(item);
					}
				}
				else
				{
					DestroyObject(item);
				}
				if (allViewPoints.ContainsKey(item))
				{
					RemovePointInfo(item, forceRemove: false);
				}
				else
				{
					if (num <= 0)
					{
						continue;
					}
					foreach (KeyValuePair<long, WorldTileInfo> item2 in uuidInfoMap)
					{
						if (item2.Value != null)
						{
							PointInfo pointInfo = item2.Value.GetPointInfo();
							if (pointInfo != null && item == pointInfo.mainIndex && num == pointInfo.PointType)
							{
								uuidInfoMap.Remove(pointInfo.uuid);
								break;
							}
						}
					}
				}
			}
			CheckNeedRefreshRoad();
		}
		MarkPointUpdate();
	}

	public void ParseWorldPointUpdate(ISFSObject message, int serverId = 0, int worldId = 0)
	{
		bool isCreate = message.GetUtfString("type") == "create";
		_pointInfos.Clear();
		ISFSArray sFSArray = message.GetSFSArray("points");
		for (int i = 0; i < sFSArray.Count; i++)
		{
			WorldPointInfo p = WorldPointInfo.Parser.ParseFrom(sFSArray.GetByteArray(i).Bytes);
			_pointInfos.Add(NewPointInfo(p, isCreate));
		}
		if (_pointInfos.Count > 0)
		{
			string uid = GameEntry.Data.Player.Uid;
			_needRefreshBoard.Clear();
			_needRefreshDeleteBoard.Clear();
			List<int> list = new List<int>();
			foreach (PointInfo pointInfo2 in _pointInfos)
			{
				if ((pointInfo2 is SamplePointInfo && (pointInfo2 as SamplePointInfo).ownerUid != uid) || (pointInfo2 is ExplorePointInfo && (pointInfo2 as ExplorePointInfo).ownerUid != uid) || (pointInfo2 is GarbagePointInfo && (pointInfo2 as GarbagePointInfo).ownerUid != uid) || (pointInfo2.serverId > 0 && ((serverId > 0 && pointInfo2.serverId != serverId) || pointInfo2.worldId != GameEntry.Data.Player.GetWorldId())))
				{
					continue;
				}
				if (pointInfo2.pointType == WorldPointType.MONSTER_REWARD)
				{
					byte[] extraInfo = pointInfo2.extraInfo;
					if (CollectRewardInfo.Parser.ParseFrom(extraInfo).OwnerUid != uid)
					{
						continue;
					}
				}
				if (pointInfo2.pointIndex == focusPoint)
				{
					focusPointIsChanged = true;
				}
				if (allViewPoints.ContainsKey(pointInfo2.pointIndex))
				{
					if (pointInfo2.isMainPoint)
					{
						WorldTileInfo worldTileInfo = allViewPoints[pointInfo2.pointIndex];
						PointInfo pointInfo = worldTileInfo.GetPointInfo();
						if ((pointInfo2.pointType != WorldPointType.PlayerBuilding && pointInfo2.pointType != WorldPointType.HERO_DISPATCH && pointInfo2.pointType != WorldPointType.WORLD_ALLIANCE_CITY && pointInfo2.pointType != WorldPointType.WORLD_ALLIANCE_BUILD && pointInfo2.pointType != WorldPointType.WORLD_CITY_STRONGHOLD && pointInfo2.pointType != WorldPointType.WORLD_CITY_TRADE && pointInfo2.pointType != WorldPointType.GOLD_TREE && pointInfo2.pointType != WorldPointType.DRAGON_BUILDING && pointInfo2.pointType != WorldPointType.WINTER_ENTITY && pointInfo2.pointType != WorldPointType.BATTLEFIELD_TYPE && pointInfo2.pointType != WorldPointType.TREASURE && pointInfo2.pointType != WorldPointType.CITY_ATTACHMENT_WALL && pointInfo2.pointType != WorldPointType.CITY_ATTACHMENT_BUILD && pointInfo2.pointType != WorldPointType.GHOSTRECON_POINT && pointInfo2.pointType != WorldPointType.WorldSuppliesPoint && pointInfo2.pointType != WorldPointType.RadarSeasonSnowSurvivor && pointInfo2.pointType != WorldPointType.ZONE_MOBILIZATION && pointInfo2.pointType != WorldPointType.MONSETER_CHALLENGE_NEW_TREASURE) || pointInfo == null || (pointInfo != null && IsNeedChangeBuildObj(pointInfo as BuildPointInfo, pointInfo2 as BuildPointInfo)))
						{
							AddPointInfo(pointInfo2);
							RemoveFromBuildList(pointInfo2.pointIndex);
							if (pointInfo2.pointType != WorldPointType.PlayerRoad)
							{
								DestroyObject(pointInfo2.pointIndex);
								AddToBuildList(pointInfo2.pointIndex);
							}
						}
						else if (pointInfo2.pointType == WorldPointType.PlayerBuilding && worldTileInfo.GetPointType() == 0)
						{
							DestroyObject(pointInfo2.pointIndex);
							AddPointInfo(pointInfo2);
							AddToCreateList(pointInfo2.pointIndex);
						}
						else
						{
							AddPointInfo(pointInfo2);
							if (pointInfo2.uuid > 0)
							{
								UpdateObjectByUuid(pointInfo2.uuid);
							}
							else
							{
								UpdateObject(pointInfo2.pointIndex);
							}
						}
					}
					else
					{
						AddPointInfo(pointInfo2);
					}
				}
				else
				{
					AddPointInfo(pointInfo2);
					if (pointInfo2.isMainPoint)
					{
						if (pointInfo2.pointType == WorldPointType.PlayerBuilding && pointInfo2 is BuildPointInfo { level: >0 } buildPointInfo && !_needPlayPlacedAnimBuild.ContainsKey(buildPointInfo.uuid))
						{
							_needPlayPlacedAnimBuild.Add(buildPointInfo.uuid, value: true);
						}
						bool flag = true;
						if (pointInfo2.pointType == WorldPointType.PlayerRoad && pointInfo2 is BoardPointInfo { inside: >0 })
						{
							flag = false;
						}
						if (flag)
						{
							AddToBuildList(pointInfo2.pointIndex);
						}
						if (pointInfo2.pointType == WorldPointType.PlayerRoad && pointInfo2.ownerUid != GameEntry.Data.Player.Uid && pointInfo2 is BoardPointInfo { state: BoardState.Updating } boardPointInfo2)
						{
							list.Add(boardPointInfo2.pointIndex);
						}
					}
				}
				if (pointInfo2.pointType == WorldPointType.WINTER_ENTITY || pointInfo2.pointType == WorldPointType.BATTLEFIELD_TYPE)
				{
					GameEntry.Event.Fire(EventId.WinterStormEntityUpdate, pointInfo2.pointIndex);
				}
			}
			if (list.Count > 0)
			{
				world.FakeModelManager.StartPrintRoad(list, isOther: true);
			}
			CheckNeedRefreshRoad();
		}
		MarkPointUpdate();
	}

	public void HandleViewUpdateNotify(ISFSObject message)
	{
		string utfString = message.GetUtfString("type");
		int num = message.GetInt("sid");
		DCPlayer player = GameEntry.Data.Player;
		if (num > 0 && !player.IsInBattleField())
		{
			int crossServerId = player.GetCrossServerId();
			if (crossServerId > 0 && crossServerId != num && !SeasonDataManager.Instance.InSeasonBigMapMode())
			{
				return;
			}
		}
		int worldId = message.GetInt("worldId");
		switch (utfString)
		{
		case "create":
		case "change":
		case "relocate":
			ParseWorldPointUpdate(message, num, worldId);
			break;
		case "remove":
			ParseWorldPointRemove(message, num, worldId);
			break;
		case "foldUp":
			ParseWorldPointFoldUp(message, num, worldId);
			break;
		}
	}

	public void HandleViewAssistanceInfoUpdateNotify(ISFSObject message)
	{
		if (message != null && EnableWorldAssistanceOpt)
		{
			int serverId = message.GetInt("serverId");
			int worldId = message.GetInt("worldId");
			aoiAssistanceInfos = aoiAssistanceInfos ?? new WorldAoiAssistanceInfos();
			aoiAssistanceInfos.UpdateSingleInfo(message.GetByteArray("pointAssistanceInfo"), serverId, worldId);
			MarkPointUpdate();
		}
	}

	public void HandleViewTileUpdateNotify(ISFSObject message)
	{
		if (message.GetUtfString("type") == "update")
		{
			ParseWorldTileUpdate(message);
		}
	}

	public void HandleALPoints(ISFSObject message)
	{
		if (alliancePointsInfos == null)
		{
			alliancePointsInfos = new WorldALPointsInfos();
		}
		alliancePointsInfos.UpdateFromMsg(message);
	}

	public void GetALMemberPoints(out long leaderPosition, out List<long> memberPositions)
	{
		if (alliancePointsInfos == null)
		{
			leaderPosition = 0L;
			memberPositions = null;
		}
		else
		{
			alliancePointsInfos.GetALMemberPoints(out leaderPosition, out memberPositions);
		}
	}

	public void ClearALMemberPoints()
	{
		alliancePointsInfos?.Clear();
	}

	private void ParseWorldTileUpdate(ISFSObject message)
	{
		_desertInfos.Clear();
		ISFSArray sFSArray = message.GetSFSArray("deserts");
		if (sFSArray != null)
		{
			for (int i = 0; i < sFSArray.Count; i++)
			{
				DesertInfo di = DesertInfo.Parser.ParseFrom(sFSArray.GetByteArray(i).Bytes);
				_desertInfos.Add(new WorldDesertInfo(di));
			}
		}
		foreach (WorldDesertInfo desertInfo in _desertInfos)
		{
			AddDesertInfo(desertInfo);
			AddToBuildList(desertInfo.pointIndex);
		}
	}

	private static void AddWerewolfPointInfo(BuildPointInfo bi)
	{
		if (bi.IsWerewolf && needRefreshWerewolf <= 0)
		{
			needRefreshWerewolf = bi.GetStatusExpireTimeById(704102) + 1000;
		}
	}

	public static PointInfo NewPointInfo(WorldPointInfo p, bool isCreate)
	{
		PointInfo result = null;
		try
		{
			switch ((WorldPointType)p.PointType)
			{
			case WorldPointType.PlayerBuilding:
			{
				BuildPointInfo buildPointInfo = new BuildPointInfo(p);
				AddWerewolfPointInfo(buildPointInfo);
				if (GMSwitch.IsGM)
				{
					int num = GMSwitch.GetInt("DebugBuildSkinID");
					if (GMSwitch.GetBool("DebugBuildSkinRandom") && GMSwitch.GetObject("RandomSkinIds") is List<int> { Count: >0 } list)
					{
						num = list[UnityEngine.Random.Range(0, list.Count)];
					}
					int num2 = GMSwitch.GetInt("DebugBuildSkinEffId");
					if (num > 0)
					{
						buildPointInfo.skinId = num;
					}
					if (num2 > 0)
					{
						buildPointInfo.effectId = num2;
					}
				}
				result = buildPointInfo;
				break;
			}
			case WorldPointType.WorldResource:
				result = new ResPointInfo(p);
				break;
			case WorldPointType.WorldCollectResource:
				result = new CollectPointInfo(p);
				break;
			case WorldPointType.PlayerRoad:
				result = new BoardPointInfo(p);
				break;
			case WorldPointType.SAMPLE_POINT:
			case WorldPointType.SAMPLE_POINT_NEW:
			case WorldPointType.RESCUE_POINT:
				result = new SamplePointInfo(p);
				break;
			case WorldPointType.EXPLORE_POINT:
			case WorldPointType.DETECT_EVENT_PVE:
				result = new ExplorePointInfo(p);
				break;
			case WorldPointType.GARBAGE:
				result = new GarbagePointInfo(p);
				break;
			case WorldPointType.WORLD_CITY_STRONGHOLD:
			case WorldPointType.WORLD_CITY_TRADE:
			case WorldPointType.GOLD_TREE:
				result = new AllyCityPointInfo(p);
				break;
			case WorldPointType.WORLD_CITY_OUTPOST:
			{
				WorldOutpostPoint worldOutpostPoint = new WorldOutpostPoint(p, isCreate);
				result = worldOutpostPoint;
				battlePoints.Add(worldOutpostPoint.serverId, worldOutpostPoint.CityId, worldOutpostPoint.uuid);
				break;
			}
			case WorldPointType.WORLD_CITY_OUTPOST_TOWER:
			{
				WorldOutpostTowerPoint worldOutpostTowerPoint = new WorldOutpostTowerPoint(p, isCreate);
				result = worldOutpostTowerPoint;
				battlePoints.Add(worldOutpostTowerPoint.serverId, worldOutpostTowerPoint.CityId, worldOutpostTowerPoint.uuid);
				break;
			}
			case WorldPointType.WORLD_ALLIANCE_CITY:
				result = new AllyCityPointInfo(p);
				break;
			case WorldPointType.HERO_DISPATCH:
				result = new HeroDispatchMissionPointInfo(p);
				break;
			case WorldPointType.DRAGON_BUILDING:
				result = new DragonPointInfo(p);
				break;
			case WorldPointType.DRAGON_SCORE_POINT:
				result = new DragonScorePointInfo(p);
				break;
			case WorldPointType.WINTER_ENTITY:
				result = new WinterStormPointInfo(p);
				break;
			case WorldPointType.BATTLEFIELD_TYPE:
				result = new BattlefieldBuildPointInfo(p);
				break;
			case WorldPointType.TREASURE:
				result = new TreasurePointInfo(p);
				break;
			case WorldPointType.WORLD_ALLIANCE_BUILD:
				result = new AllianceBuildPointInfo(p, isCreate);
				break;
			case WorldPointType.WorldAllianceCollectResource:
				result = new WorldAllianceCollectResource(p, isCreate);
				break;
			case WorldPointType.WorldSuppliesPoint:
				result = new WorldSuppliesPoint(p);
				break;
			case WorldPointType.RadarSeasonSnowSurvivor:
				result = new ExplorePointInfo(p);
				break;
			case WorldPointType.GHOSTRECON_POINT:
				result = new GhostreconPointInfo(p);
				break;
			case WorldPointType.CITY_ATTACHMENT_BUILD:
				result = new CityAttachmentBuildPointInfo(p);
				break;
			case WorldPointType.CITY_ATTACHMENT_WALL:
				result = new CityAttachmentWallPointInfo(p);
				break;
			case WorldPointType.RADAR_DOMINATOR_GUIDE:
			case WorldPointType.RADAR_DOMINATOR_CURE:
			case WorldPointType.RADAR_DOMINATOR_COCKATRICE_UNLOCK_1:
			case WorldPointType.RADAR_DOMINATOR_COCKATRICE_UNLOCK_2:
				result = new DominatorSamplePointInfo(p);
				break;
			case WorldPointType.SURPRISE_POINT:
				result = new SurprisePointInfo(p);
				break;
			case WorldPointType.CAVE_EXPLORATION:
				result = new SamplePointInfo(p);
				break;
			case WorldPointType.WORLD_RUIN_DESTROY_BUILDING:
				return new WorldRuinDestroyBuildingPointInfo(p);
			case WorldPointType.ZONE_MOBILIZATION:
				result = new WorldZoneMobilizationPointInfo(p);
				break;
			case WorldPointType.METEORITE_POINT:
				result = new WorldMeteoritePoint(p);
				break;
			case WorldPointType.ACTIVITY_WORLD_TREASURE:
				result = new WorldActivityTreasureInfo(p);
				break;
			case WorldPointType.MONSETER_CHALLENGE_NEW_TREASURE:
				result = new NewAlChallengeTreasurePointInfo(p);
				break;
			case WorldPointType.DETECT_RETRY_TASK:
				result = new DetectRetryTaskPointInfo(p);
				break;
			case WorldPointType.DETECT_ALLIANCE_CITY_SCOUT_MONSTER:
				result = new DetectAttackCityS0TaskPointInfo(p);
				break;
			case WorldPointType.DETECT_DIG_GAME:
			case WorldPointType.TreasureChest:
			case WorldPointType.SkyBattle:
			case WorldPointType.DETECT_LAST_STAND:
				result = new SamplePointInfo(p);
				break;
			case WorldPointType.DETECT_SUPPLIES_SEARCH:
				result = new SamplePointInfo(p);
				break;
			default:
				result = new PointInfo(p);
				break;
			}
		}
		catch (Exception)
		{
			Log.Error($"Error world point:{p.Uuid}");
		}
		return result;
	}

	public void ShowObject(int point)
	{
		GetObjectByPoint(point)?.SetVisible(v: true);
	}

	public void HideObject(int point)
	{
		GetObjectByPoint(point)?.SetVisible(v: false);
	}

	public void UpdateObject(int point)
	{
		GetObjectByPoint(point)?.UpdateGameObject();
	}

	public void UpdateObjectByUuid(long pointUuid)
	{
		GetObjectByUuid(pointUuid)?.UpdateGameObject();
	}

	private void RefreshPlayerSwitchState(object obj = null)
	{
		EnableWorldAssistanceOpt = GameEntry.Lua.CallWithReturn<bool>("WorldBattleUtil.EnableShowWorldAssistanceInfo");
	}

	public void UpdateViewRequest(bool isForce = false)
	{
		if (isForce)
		{
			UpdateLWAoi(isForce: true);
		}
	}

	private void OnScoutDragonScore(object userdata)
	{
		try
		{
			int num = (int)(long)userdata;
			if (num > 1)
			{
				DestroyObject(num);
				RemovePointInfo(num, forceRemove: false);
				MarkPointUpdate();
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	public int GetServerLod(int lod)
	{
		if (lod <= 4)
		{
			return 0;
		}
		if (lod <= 5)
		{
			return 1;
		}
		return 2;
	}

	public void StartViewRequest()
	{
		startViewRequest = true;
	}

	[Obsolete("已经不用了", false)]
	public void SendViewRequest(Vector2Int tilePos, int viewLevel, int serverId)
	{
		if (viewLevel > 2)
		{
			viewLevel = 2;
		}
		tilePos = world.ClampTilePos(tilePos);
		int num = serverId;
		bool flag = GameEntry.Data.Player.IsInSelfServer();
		if (!flag)
		{
			num = GameEntry.Data.Player.GetCrossServerId();
		}
		bool flag2 = GameEntry.Data.Player.IsInBattleField();
		int worldId = GameEntry.Data.Player.GetWorldId();
		if (worldId > 0 || flag2)
		{
			viewLevel = 0;
		}
		GetViewLevelWorldInfoMessage.Instance.Send(new GetViewLevelWorldInfoMessage.Request
		{
			x = tilePos.x,
			y = tilePos.y,
			type = 0,
			viewLvl = viewLevel,
			serverId = num,
			worldId = worldId
		});
		if ((!flag || flag2) && !CrossServerComponent.OnlyMainLine)
		{
			WorldCrossServerMessage.Instance.SendReqest(new WorldCrossServerMessage.RequestParam
			{
				x = tilePos.x,
				y = tilePos.y,
				type = 0,
				forceServerId = num,
				worldId = worldId
			});
		}
		reqPos = tilePos;
	}

	protected void MarkPointUpdate()
	{
		_isPointUpdate = true;
	}

	protected void AddToBuildList(int pointIndex)
	{
		toBuildList.Add(pointIndex);
		outOfViewPoints.Remove(pointIndex);
		outOfViewPointsObj.Remove(pointIndex);
	}

	private void RemoveFromBuildList(int pointIndex)
	{
		toBuildList.Remove(pointIndex);
	}

	protected void AddPointInfo(PointInfo pi)
	{
		PointInfo pointInfo = null;
		if (uuidInfoMap.TryGetValue(pi.uuid, out var value) && value.pointIndex != pi.pointIndex)
		{
			DestroyObject(value.pointIndex);
			RemovePointInfo(value.pointIndex, forceRemove: false);
		}
		if (allViewPoints.TryGetValue(pi.pointIndex, out var value2))
		{
			if (value2 != null)
			{
				pointInfo = value2.GetPointInfo();
				value2.AddPointInfo(pi);
			}
		}
		else
		{
			WorldTileInfo worldTileInfo = new WorldTileInfo();
			worldTileInfo.AddPointInfo(pi);
			allViewPoints.Add(worldTileInfo.pointIndex, worldTileInfo);
		}
		_ = pi.pointType;
		_ = 17;
		Vector2Int vector2Int = world.IndexToTilePos(pi.pointIndex);
		int num = pi.tileSize;
		int num2 = pi.tileSize;
		if (pi.pointType == WorldPointType.WorldCollectResource)
		{
			int circleRange = GetCircleRange(pi.tileSize);
			vector2Int.x += circleRange;
			vector2Int.y += circleRange;
		}
		else if ((pi.pointType == WorldPointType.CITY_ATTACHMENT_WALL || pi.pointType == WorldPointType.CITY_ATTACHMENT_BUILD) && pi is CityAttachmentPointInfo cityAttachmentPointInfo)
		{
			num = cityAttachmentPointInfo.tileSizeX;
			num2 = cityAttachmentPointInfo.tileSizeY;
		}
		if (num > 1 || num2 > 1)
		{
			int num3 = num / 2;
			int num4 = num2 / 2;
			for (int i = -num3; i <= num3; i++)
			{
				for (int j = -num4; j <= num4; j++)
				{
					Vector2Int vector2Int2 = vector2Int + new Vector2Int(i, j);
					if (!SceneManager.World.IsInMap(vector2Int2))
					{
						continue;
					}
					int num5 = world.TilePosToIndex(vector2Int2);
					if (num5 == pi.mainIndex)
					{
						continue;
					}
					PointInfo pointInfo2 = pi.Clone();
					pointInfo2.pointIndex = num5;
					pointInfo2.mainIndex = pi.mainIndex;
					if (allViewPoints.TryGetValue(num5, out var value3))
					{
						if (value3 != null && (value3.GetWorldPointType() != WorldPointType.WORLD_ALLIANCE_BUILD || value3.GetPointSize() > 5))
						{
							value3.AddPointInfo(pointInfo2);
						}
					}
					else
					{
						WorldTileInfo worldTileInfo2 = new WorldTileInfo();
						worldTileInfo2.AddPointInfo(pointInfo2);
						allViewPoints.Add(worldTileInfo2.pointIndex, worldTileInfo2);
					}
					if (pi.pointType == WorldPointType.PlayerBuilding && pi is BuildPointInfo buildPointInfo && (buildPointInfo.itemId == 735000 || buildPointInfo.itemId == 792000))
					{
						specialBuildDic[num5] = buildPointInfo.itemId;
						specialBuildDic[pi.mainIndex] = buildPointInfo.itemId;
					}
				}
			}
		}
		world.AddOccupyPoints(vector2Int, new Vector2Int(num, num2), pi.serverId);
		if (pi.pointType == WorldPointType.WORLD_ALLIANCE_CITY)
		{
			_isCityPointUpdate = true;
			int value4 = 0;
			if (!(pi is AllyCityPointInfo allyCityPointInfo) || !AllianceCityTypeDic.TryGetValue(allyCityPointInfo.CityId, out value4))
			{
				value4 = 3;
			}
			if (value4 != 2 && value4 != 6)
			{
				int num6 = 3;
				int num7 = pi.tileSize / 2;
				int num8 = num7 + num6;
				for (int k = -num8; k <= num8; k++)
				{
					for (int l = -num8; l <= num8; l++)
					{
						if (-num7 > k || k > num7 || -num7 > l || l > num7)
						{
							Vector2Int vector2Int3 = vector2Int + new Vector2Int(k, l);
							if (SceneManager.World.IsInMap(vector2Int3))
							{
								int key = world.TilePosToIndex(vector2Int3);
								yellowLand[key] = pi;
							}
						}
					}
				}
			}
		}
		if (pi.pointType == WorldPointType.PlayerRoad)
		{
			_needRefreshDeleteBoard[pi.pointIndex] = true;
			for (int m = 0; m < GameDefines.ConnectDirList.Count; m++)
			{
				int indexByOffsetByDirection = world.GetIndexByOffsetByDirection(pi.pointIndex, (int)GameDefines.ConnectDirList[m]);
				if (SceneManager.World.IsInMapByIndex(indexByOffsetByDirection))
				{
					_needRefreshBoard.Add(indexByOffsetByDirection);
				}
			}
			if (pi is BoardPointInfo { uuid: not 0L } boardPointInfo)
			{
				if (!uuidInfoMap.ContainsKey(boardPointInfo.uuid))
				{
					WorldTileInfo value5 = new WorldTileInfo();
					uuidInfoMap[boardPointInfo.uuid] = value5;
				}
				uuidInfoMap[boardPointInfo.uuid].AddPointInfo(boardPointInfo);
			}
		}
		else if (pi.pointType == WorldPointType.PlayerBuilding)
		{
			if (pi is BuildPointInfo { uuid: not 0L } buildPointInfo2)
			{
				if (!uuidInfoMap.ContainsKey(buildPointInfo2.uuid))
				{
					WorldTileInfo value6 = new WorldTileInfo();
					uuidInfoMap[buildPointInfo2.uuid] = value6;
				}
				uuidInfoMap[buildPointInfo2.uuid].AddPointInfo(buildPointInfo2);
				if (buildPointInfo2.itemId == 10100000 || buildPointInfo2.itemId == 735000)
				{
					baseMainBuild[buildPointInfo2.ownerUid] = buildPointInfo2.uuid;
					if (buildPointInfo2.ownerUid == GameEntry.Data.Player.Uid && buildPointInfo2.IsNormalType())
					{
						myWorldTileInfo = uuidInfoMap[buildPointInfo2.uuid];
						_myPointDirty = true;
					}
				}
				if (buildPointInfo2.roadDir != GameDefines.BuildConnectRoadDirection.None)
				{
					List<int> posIndexListByDir = buildPointInfo2.GetPosIndexListByDir(buildPointInfo2.roadDir);
					for (int n = 0; n < posIndexListByDir.Count; n++)
					{
						_needRefreshBoard.Add(posIndexListByDir[n]);
					}
				}
			}
			if (pointInfo != null && pointInfo.pointType == WorldPointType.PlayerBuilding && pi is BuildPointInfo { roadDir: not GameDefines.BuildConnectRoadDirection.None } buildPointInfo3)
			{
				List<int> posIndexListByDir2 = buildPointInfo3.GetPosIndexListByDir(buildPointInfo3.roadDir);
				for (int num9 = 0; num9 < posIndexListByDir2.Count; num9++)
				{
					_needRefreshBoard.Add(posIndexListByDir2[num9]);
				}
			}
		}
		else if (pi.pointType == WorldPointType.SAMPLE_POINT || pi.pointType == WorldPointType.SAMPLE_POINT_NEW || pi.pointType == WorldPointType.RESCUE_POINT)
		{
			if (pi is SamplePointInfo { uuid: not 0L } samplePointInfo)
			{
				if (!uuidInfoMap.ContainsKey(samplePointInfo.uuid))
				{
					WorldTileInfo value7 = new WorldTileInfo();
					uuidInfoMap[samplePointInfo.uuid] = value7;
				}
				uuidInfoMap[samplePointInfo.uuid].AddPointInfo(samplePointInfo);
			}
		}
		else if (pi.pointType == WorldPointType.EXPLORE_POINT || pi.pointType == WorldPointType.DETECT_EVENT_PVE)
		{
			if (pi is ExplorePointInfo { uuid: not 0L } explorePointInfo)
			{
				if (!uuidInfoMap.ContainsKey(explorePointInfo.uuid))
				{
					WorldTileInfo value8 = new WorldTileInfo();
					uuidInfoMap[explorePointInfo.uuid] = value8;
				}
				uuidInfoMap[explorePointInfo.uuid].AddPointInfo(explorePointInfo);
			}
		}
		else if (pi.pointType == WorldPointType.GARBAGE)
		{
			if (pi is GarbagePointInfo { uuid: not 0L } garbagePointInfo)
			{
				if (!uuidInfoMap.ContainsKey(garbagePointInfo.uuid))
				{
					WorldTileInfo value9 = new WorldTileInfo();
					uuidInfoMap[garbagePointInfo.uuid] = value9;
				}
				uuidInfoMap[garbagePointInfo.uuid].AddPointInfo(garbagePointInfo);
			}
		}
		else if (pi.pointType == WorldPointType.WORLD_CITY_STRONGHOLD || pi.pointType == WorldPointType.WORLD_ALLIANCE_CITY || pi.pointType == WorldPointType.WORLD_ALLIANCE_BUILD || pi.pointType == WorldPointType.WORLD_CITY_TRADE || pi.pointType == WorldPointType.WORLD_CITY_OUTPOST || pi.pointType == WorldPointType.WORLD_CITY_OUTPOST_TOWER || pi.pointType == WorldPointType.GOLD_TREE)
		{
			_isCityPointUpdate = true;
			if (pi.uuid != 0L)
			{
				if (!uuidInfoMap.ContainsKey(pi.uuid))
				{
					WorldTileInfo value10 = new WorldTileInfo();
					uuidInfoMap[pi.uuid] = value10;
				}
				uuidInfoMap[pi.uuid].AddPointInfo(pi);
			}
		}
		else if (pi.pointType == WorldPointType.DRAGON_BUILDING || pi.pointType == WorldPointType.DRAGON_SCORE_POINT || pi.pointType == WorldPointType.WINTER_ENTITY || pi.pointType == WorldPointType.BATTLEFIELD_TYPE)
		{
			if (pi.uuid != 0L)
			{
				baseDragonPoint[pi.uuid] = pi.uuid;
				if (!uuidInfoMap.ContainsKey(pi.uuid))
				{
					uuidInfoMap[pi.uuid] = new WorldTileInfo();
				}
				uuidInfoMap[pi.uuid].AddPointInfo(pi);
			}
		}
		else if (pi.uuid != 0L)
		{
			if (!uuidInfoMap.ContainsKey(pi.uuid))
			{
				WorldTileInfo value11 = new WorldTileInfo();
				uuidInfoMap[pi.uuid] = value11;
			}
			uuidInfoMap[pi.uuid].AddPointInfo(pi);
		}
		if (pi.pointType == WorldPointType.TREASURE && pi is TreasurePointInfo treasurePointInfo && treasurePointInfo.killerId == GameEntry.Data.Player.Uid)
		{
			Log.Info($"AddPointInfo with killerId {pi.uuid} , {pi.mainIndex} , {pi.isMainPoint}");
		}
	}

	private void AddDesertInfo(WorldDesertInfo di)
	{
		if (allViewPoints.ContainsKey(di.pointIndex))
		{
			allViewPoints[di.pointIndex].AddDesertInfo(di);
		}
		else
		{
			WorldTileInfo worldTileInfo = new WorldTileInfo();
			worldTileInfo.AddDesertInfo(di);
			allViewPoints.Add(worldTileInfo.pointIndex, worldTileInfo);
		}
		if (di != null && di.uuid != 0L)
		{
			if (!uuidInfoDesertMap.ContainsKey(di.uuid))
			{
				WorldTileInfo value = new WorldTileInfo();
				uuidInfoDesertMap[di.uuid] = value;
			}
			uuidInfoDesertMap[di.uuid].AddDesertInfo(di);
		}
	}

	private void RemoveDesertInfo(int pointIndex)
	{
		if (allViewPoints.TryGetValue(pointIndex, out var value))
		{
			WorldDesertInfo worldDesertInfo = value.GetWorldDesertInfo();
			if (worldDesertInfo != null && worldDesertInfo.uuid != 0L)
			{
				uuidInfoDesertMap.Remove(worldDesertInfo.uuid);
			}
			value.RemoveDesertInfo();
			if (value.GetIsDataEmpty())
			{
				allViewPoints.Remove(value.pointIndex);
			}
		}
	}

	private void RemovePointInfo(int pointIndex, bool forceRemove)
	{
		if (!allViewPoints.TryGetValue(pointIndex, out var value))
		{
			return;
		}
		aoiAssistanceInfos?.RemoveByPointIndex(value.serverId, pointIndex);
		PointInfo pointInfo = value.GetPointInfo();
		if (forceRemove && SceneManager.IsInWorld() && GameEntry.Data.Player.IsInBattleField() && (pointInfo == null || (!GameEntry.Data.Player.IsInBattleField(3) && !GameEntry.Data.Player.IsInBattleField(4)) || pointInfo.pointType != WorldPointType.PlayerBuilding || pointInfo.ownerUid == GameEntry.Data.Player.Uid))
		{
			return;
		}
		int num = 0;
		int num2 = 1;
		int num3 = 1;
		if (pointInfo == null)
		{
			num = value.pointIndex;
		}
		else
		{
			num2 = pointInfo.tileSize;
			num3 = pointInfo.tileSize;
			num = pointInfo.pointIndex;
		}
		Vector2Int vector2Int = world.IndexToTilePos(num);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.WorldCollectResource)
		{
			int circleRange = GetCircleRange(pointInfo.tileSize);
			vector2Int.x += circleRange;
			vector2Int.y += circleRange;
		}
		else if (pointInfo != null && (pointInfo.pointType == WorldPointType.CITY_ATTACHMENT_WALL || pointInfo.pointType == WorldPointType.CITY_ATTACHMENT_BUILD) && pointInfo is CityAttachmentPointInfo cityAttachmentPointInfo)
		{
			num2 = cityAttachmentPointInfo.tileSizeX;
			num3 = cityAttachmentPointInfo.tileSizeY;
		}
		needCheckShowTileList.Clear();
		if (num2 > 1 || num3 > 1)
		{
			int num4 = num2 / 2;
			int num5 = num3 / 2;
			for (int i = -num4; i <= num4; i++)
			{
				for (int j = -num5; j <= num5; j++)
				{
					Vector2Int vector2Int2 = vector2Int + new Vector2Int(i, j);
					if (!world.IsInMap(vector2Int2))
					{
						continue;
					}
					int num6 = world.TilePosToIndex(vector2Int2);
					if (num6 == pointIndex)
					{
						continue;
					}
					if (allViewPoints.ContainsKey(num6))
					{
						WorldTileInfo worldTileInfo = allViewPoints[num6];
						if (worldTileInfo != null)
						{
							worldTileInfo.RemovePointInfo();
							if (worldTileInfo.GetIsDataEmpty() || forceRemove)
							{
								allViewPoints.Remove(num6);
							}
						}
					}
					specialBuildDic.Remove(num6);
					needCheckShowTileList.Add(num6);
				}
			}
		}
		if (pointInfo != null)
		{
			world.RemoveOccupyPoints(vector2Int, new Vector2Int(num2, num3), pointInfo.serverId);
		}
		else
		{
			world.RemoveOccupyPoints(vector2Int, new Vector2Int(num2, num3), value.serverId);
		}
		value.RemovePointInfo();
		if (value.GetIsDataEmpty() || forceRemove)
		{
			allViewPoints.Remove(value.pointIndex);
		}
		needCheckShowTileList.Add(value.pointIndex);
		if (pointInfo != null)
		{
			if (pointInfo.pointType == WorldPointType.PlayerRoad)
			{
				if (pointInfo is BoardPointInfo boardPointInfo)
				{
					uuidInfoMap.Remove(boardPointInfo.uuid);
				}
				_needRefreshDeleteBoard[pointInfo.pointIndex] = true;
				for (int k = 0; k < GameDefines.ConnectDirList.Count; k++)
				{
					int dir = (int)GameDefines.ConnectDirList[k];
					int indexByOffsetByDirection = world.GetIndexByOffsetByDirection(pointInfo.pointIndex, dir);
					if (SceneManager.World.IsInMapByIndex(indexByOffsetByDirection))
					{
						_needRefreshBoard.Add(indexByOffsetByDirection);
					}
				}
			}
			else if (pointInfo.pointType == WorldPointType.PlayerBuilding)
			{
				if (pointInfo is BuildPointInfo buildPointInfo)
				{
					uuidInfoMap.Remove(buildPointInfo.uuid);
					if (buildPointInfo.itemId == 10100000)
					{
						baseMainBuild.Remove(buildPointInfo.ownerUid);
					}
					if (buildPointInfo.roadDir != GameDefines.BuildConnectRoadDirection.None)
					{
						List<int> posIndexListByDir = buildPointInfo.GetPosIndexListByDir(buildPointInfo.roadDir);
						for (int l = 0; l < posIndexListByDir.Count; l++)
						{
							_needRefreshBoard.Add(posIndexListByDir[l]);
						}
					}
				}
			}
			else if (pointInfo.pointType == WorldPointType.SAMPLE_POINT || pointInfo.pointType == WorldPointType.SAMPLE_POINT_NEW || pointInfo.pointType == WorldPointType.RESCUE_POINT)
			{
				SamplePointInfo samplePointInfo = pointInfo as SamplePointInfo;
				uuidInfoMap.Remove(samplePointInfo.uuid);
			}
			else if (!forceRemove && pointInfo.pointType == WorldPointType.DRAGON_SCORE_POINT)
			{
				baseDragonPoint.Remove(pointInfo.uuid);
			}
			else if (pointInfo.pointType == WorldPointType.EXPLORE_POINT || pointInfo.pointType == WorldPointType.DETECT_EVENT_PVE)
			{
				ExplorePointInfo explorePointInfo = pointInfo as ExplorePointInfo;
				uuidInfoMap.Remove(explorePointInfo.uuid);
			}
			else if (pointInfo.pointType == WorldPointType.GARBAGE)
			{
				GarbagePointInfo garbagePointInfo = pointInfo as GarbagePointInfo;
				uuidInfoMap.Remove(garbagePointInfo.uuid);
			}
			else if (pointInfo.pointType == WorldPointType.HERO_DISPATCH)
			{
				uuidInfoMap.Remove(pointInfo.uuid);
			}
			else if (!forceRemove && (pointInfo.pointType == WorldPointType.WINTER_ENTITY || pointInfo.pointType == WorldPointType.BATTLEFIELD_TYPE))
			{
				baseDragonPoint.Remove(pointInfo.uuid);
				GameEntry.Event.Fire(EventId.WinterStormEntityUpdate, pointInfo.pointIndex);
			}
			else if (pointInfo.pointType == WorldPointType.GHOSTRECON_POINT)
			{
				uuidInfoMap.Remove(pointInfo.uuid);
			}
			else if (pointInfo.pointType == WorldPointType.CITY_ATTACHMENT_WALL)
			{
				uuidInfoMap.Remove(pointInfo.uuid);
			}
			else if (pointInfo.pointType == WorldPointType.CITY_ATTACHMENT_BUILD)
			{
				uuidInfoMap.Remove(pointInfo.uuid);
			}
			else if (pointInfo.pointType == WorldPointType.DETECT_RETRY_TASK)
			{
				uuidInfoMap.Remove(pointInfo.uuid);
			}
			else if (pointInfo.pointType == WorldPointType.DETECT_ALLIANCE_CITY_SCOUT_MONSTER)
			{
				uuidInfoMap.Remove(pointInfo.uuid);
			}
			else if (pointInfo.pointType == WorldPointType.WORLD_ALLIANCE_BUILD)
			{
				uuidInfoMap.Remove(pointInfo.uuid);
			}
		}
		for (int m = 0; m < needCheckShowTileList.Count; m++)
		{
			WorldTileInfo worldTileInfo2 = GetWorldTileInfo(needCheckShowTileList[m]);
			if (worldTileInfo2 != null && !worldTileInfo2.GetIsDataEmpty())
			{
				AddToBuildList(worldTileInfo2.pointIndex);
			}
		}
	}

	protected void ViewCulling()
	{
		int lodLevel = world.GetLodLevel();
		int num = ((lodLevel < 0) ? 1 : (1 << lodLevel));
		Dictionary<int, WorldTileInfo>.Enumerator enumerator = allViewPoints.GetEnumerator();
		while (enumerator.MoveNext())
		{
			PointInfo pointInfo = enumerator.Current.Value.GetPointInfo();
			if (pointInfo != null && pointInfo.isMainPoint && (pointInfo.IsMine() || (num & pointInfo.LodLayerMask) == 0 || (pointInfo.pointType == WorldPointType.PlayerBuilding && pointInfo is BuildPointInfo { AOSType: not AllianceOfficialSkillType.None })) && !IsOutOfViewRange(pointInfo.pointIndex, pointInfo.serverId))
			{
				WorldPointObject objectByPoint = GetObjectByPoint(pointInfo.pointIndex);
				if (objectByPoint == null || objectByPoint.GetPointType() != pointInfo.PointType)
				{
					AddToBuildList(pointInfo.pointIndex);
				}
			}
		}
		if (myWorldTileInfo != null)
		{
			PointInfo pointInfo2 = myWorldTileInfo.GetPointInfo();
			if (pointInfo2 != null && pointInfo2.isMainPoint && !toBuildList.Contains(pointInfo2.pointIndex) && !IsOutOfViewRange(pointInfo2.pointIndex, pointInfo2.serverId) && (pointInfo2.serverId == GameEntry.Data.Player.GetCurServerId() || SeasonDataManager.Instance.InNinePalacesList(pointInfo2.serverId)))
			{
				WorldPointObject objectByPoint2 = GetObjectByPoint(pointInfo2.pointIndex);
				if (objectByPoint2 == null || objectByPoint2.GetPointType() != pointInfo2.PointType)
				{
					AddToBuildList(pointInfo2.pointIndex);
				}
			}
		}
		keysToRemove.Clear();
		foreach (int toBuild in toBuildList)
		{
			int num2 = toBuild;
			keysToRemove.Add(num2);
			PointInfo pointInfo3 = GetPointInfo(num2);
			int num3 = OtherPointLod;
			if (pointInfo3 != null)
			{
				int num4 = pointInfo3.mainIndex;
				PlayerType playerType = pointInfo3.GetPlayerType();
				if (num2 != num4 && pointInfo3.pointType == WorldPointType.PlayerBuilding)
				{
					WorldTileInfo worldTileInfo = GetWorldTileInfo(num2);
					if (worldTileInfo != null)
					{
						WorldDesertInfo worldDesertInfo = worldTileInfo.GetWorldDesertInfo();
						if (worldDesertInfo != null && worldDesertInfo.GetPlayerType() != PlayerType.PlayerNone)
						{
							playerType = worldDesertInfo.GetPlayerType();
							num4 = worldDesertInfo.pointIndex;
						}
					}
				}
				num2 = num4;
				if (playerType == PlayerType.PlayerSelf)
				{
					num3 = SelfPointLod;
				}
				else if (pointInfo3.pointType == WorldPointType.PlayerBuilding && pointInfo3 is BuildPointInfo buildPointInfo2)
				{
					if (buildPointInfo2.AOSType == AllianceOfficialSkillType.None)
					{
						if (playerType == PlayerType.PlayerAlliance || playerType == PlayerType.PlayerAllianceLeader)
						{
							num3 = alliancePointLod;
						}
					}
					else
					{
						num3 = SelfPointLod;
					}
				}
				else if (playerType == PlayerType.PlayerAlliance || playerType == PlayerType.PlayerAllianceLeader)
				{
					num3 = alliancePointLod;
				}
			}
			if (LOD > num3)
			{
				continue;
			}
			WorldPointObject objectByPoint3 = GetObjectByPoint(num2);
			if (objectByPoint3 == null)
			{
				if (pointInfo3 != null && (pointInfo3.pointType == WorldPointType.WINTER_ENTITY || pointInfo3.pointType == WorldPointType.BATTLEFIELD_TYPE || !IsOutOfViewRange(pointInfo3.pointIndex, pointInfo3.serverId)))
				{
					AddToCreateList(num2);
				}
				continue;
			}
			WorldTileInfo worldTileInfo2 = GetWorldTileInfo(num2);
			if (worldTileInfo2 == null)
			{
				continue;
			}
			int pointType = objectByPoint3.GetPointType();
			if (worldTileInfo2.GetPointType() != pointType)
			{
				DestroyObject(num2);
				if (!IsOutOfViewRange(worldTileInfo2.pointIndex, worldTileInfo2.serverId))
				{
					AddToCreateList(num2);
				}
			}
			else
			{
				objectByPoint3.UpdateGameObject();
			}
		}
		int count = keysToRemove.Count;
		for (int i = 0; i < count; i++)
		{
			toBuildList.Remove(keysToRemove[i]);
		}
	}

	protected void AsyncCreate()
	{
		if (_toCreateList.Count <= 0)
		{
			return;
		}
		int num = 0;
		int num2 = _toCreateList.Count / 10;
		keysToRemove.Clear();
		foreach (int toCreate in _toCreateList)
		{
			if (busy && num > num2)
			{
				break;
			}
			num++;
			CreateObject(toCreate);
			keysToRemove.Add(toCreate);
		}
		foreach (int item in keysToRemove)
		{
			_toCreateList.Remove(item);
		}
	}

	private void AddToCreateList(int pointIndex)
	{
		_toCreateList.Add(pointIndex);
	}

	private bool IsOutOfViewRange(int pointIndex, int serverId)
	{
		float num = 0f;
		float num2 = 0f;
		if (pointIndex > 0)
		{
			pointIndex--;
			num = (float)(pointIndex % 1000) * 2f;
			num2 = (float)(pointIndex / 1000) * 2f;
		}
		if (serverId > 0 && world.WorldSize > 1000)
		{
			int ninePalacesIndex = SeasonDataManager.Instance.GetNinePalacesIndex(serverId);
			if (ninePalacesIndex > 1)
			{
				num += (float)World9BasePosX[ninePalacesIndex];
				num2 += (float)World9BasePosZ[ninePalacesIndex];
			}
		}
		WorldCamera camera = world.Camera;
		Vector3 vector = camera.cameraAnchor[0];
		Vector3 vector2 = camera.cameraAnchor[2];
		if (!(num < vector.x - 4f) && !(num > vector2.x + 4f) && !(num2 < vector.z - 4f))
		{
			return num2 > vector2.z + 4f;
		}
		return true;
	}

	private void CreateObject(int pointIndex)
	{
		WorldTileInfo worldTileInfo = GetWorldTileInfo(pointIndex);
		if (worldTileInfo == null || worldTileInfo.GetIsDataEmpty())
		{
			return;
		}
		PointInfo pointInfo = GetPointInfo(pointIndex);
		WorldPointObject worldPointObject = null;
		int num = worldTileInfo.GetPointType();
		WorldPointType worldPointType = (WorldPointType)num;
		if (pointInfo != null && pointIndex != pointInfo.mainIndex)
		{
			WorldDesertInfo worldDesertInfo = worldTileInfo.GetWorldDesertInfo();
			if (worldDesertInfo != null && worldDesertInfo.GetPlayerType() != PlayerType.PlayerNone)
			{
				worldPointType = WorldPointType.SeasonDesert;
				num = (int)worldPointType;
			}
		}
		switch (worldPointType)
		{
		case WorldPointType.WORLD_ALLIANCE_CITY:
		case WorldPointType.WORLD_CITY_STRONGHOLD:
		case WorldPointType.WORLD_CITY_TRADE:
		case WorldPointType.GOLD_TREE:
		case WorldPointType.SeasonDesert:
			worldPointObject = new WorldBasePointObject(world, pointIndex, num);
			break;
		case WorldPointType.WORLD_CITY_OUTPOST:
			worldPointObject = new WorldOutpostPointObject(world, pointIndex, num);
			break;
		case WorldPointType.WORLD_CITY_OUTPOST_TOWER:
			worldPointObject = new WorldOutpostTowerPointObject(world, pointIndex, num);
			break;
		case WorldPointType.PlayerBuilding:
			worldPointObject = ((!(world.GetPointInfo(pointIndex) is BuildPointInfo { specialType: Protobuf.SpecialType.DetectEvent } buildPointInfo) || !(buildPointInfo.ownerUid != GameEntry.Data.Player.Uid)) ? ((WorldPointObject)new WorldBuildObjectNew(world, pointIndex, num)) : ((WorldPointObject)new WorldBarricadeObject(world, pointIndex, num)));
			break;
		case WorldPointType.WorldResource:
		{
			int num2 = -1;
			ResPointInfo resPointInfo = world.GetPointInfo(pointIndex) as ResPointInfo;
			if (resPointInfo != null)
			{
				num2 = resPointInfo.GetResPointType();
			}
			worldPointObject = ((num2 != 1) ? new WorldResObject(world, pointIndex, num) : ((!(resPointInfo.ownerUid != GameEntry.Data.Player.Uid)) ? ((WorldPointObject)new WorldDetectResObject(world, pointIndex, num)) : ((WorldPointObject)new WorldBarricadeObject(world, pointIndex, num))));
			break;
		}
		case WorldPointType.METEORITE_POINT:
			worldPointObject = new WorldMeteoritePointObject(world, pointIndex, num);
			break;
		case WorldPointType.WorldCollectResource:
			worldPointObject = new WorldCollectObject(world, pointIndex, num);
			break;
		case WorldPointType.PlayerRoad:
			worldPointObject = new WorldBoardObject(world, pointIndex, num);
			break;
		case WorldPointType.EXPLORE_POINT:
		case WorldPointType.DETECT_EVENT_PVE:
			worldPointObject = new WorldExploreObject(world, pointIndex, num);
			break;
		case WorldPointType.SAMPLE_POINT:
		case WorldPointType.SAMPLE_POINT_NEW:
			worldPointObject = new WorldSampleObject(world, pointIndex, num);
			break;
		case WorldPointType.RESCUE_POINT:
			worldPointObject = new WorldDetectRescueObject(world, pointIndex, num);
			break;
		case WorldPointType.GARBAGE:
		{
			worldPointObject = new WorldGarbageObject(world, pointIndex, num);
			bool visible = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsShowWorldCollectPoint");
			worldPointObject.SetVisible(visible);
			break;
		}
		case WorldPointType.DRAGON_BUILDING:
			worldPointObject = new WorldDragonPointObject(world, pointIndex, num);
			break;
		case WorldPointType.DRAGON_SCORE_POINT:
			worldPointObject = new WorldDragonPointObject(world, pointIndex, num);
			break;
		case WorldPointType.WINTER_ENTITY:
			worldPointObject = new WorldWinterStormPointObject(world, pointIndex, num);
			break;
		case WorldPointType.BATTLEFIELD_TYPE:
			worldPointObject = new WorldBattlefieldPointObject(world, pointIndex, num);
			break;
		case WorldPointType.TREASURE:
			if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
			{
				worldPointObject = ((treasurePointInfo.type != WorldTreasureType.RadarTreasure && treasurePointInfo.type != WorldTreasureType.ActivityRadarTreasure && treasurePointInfo.type != WorldTreasureType.OffSeasonDetect) ? ((treasurePointInfo.type == WorldTreasureType.FlowerTrainUpgradeTreasure) ? new SiegeTreasureObject4FlowerTrain(world, pointIndex, num) : ((treasurePointInfo.type == WorldTreasureType.WolfShadow) ? ((WorldPointObject)new WolfShadowObject(world, pointIndex, num)) : ((WorldPointObject)((treasurePointInfo.type == WorldTreasureType.S1PrefabSeasonBossTreasure || treasurePointInfo.type == WorldTreasureType.S1PrefabWorldBossTreasure) ? new SiegeTreasureObject4FullPath(world, pointIndex, num) : ((treasurePointInfo.type != WorldTreasureType.FlowerTrainCheerTreasure) ? ((WorldDetectEventItemObject)new SiegeTreasureObject(world, pointIndex, num)) : ((WorldDetectEventItemObject)new WorldFlowerTrainRewardObject(world, pointIndex, num))))))) : new WorldTreasureObject(world, pointIndex, num));
				if (treasurePointInfo.killerId == GameEntry.Data.Player.Uid)
				{
					Log.Info($"TryCreate with killerId {treasurePointInfo.uuid} , {treasurePointInfo.mainIndex}");
				}
			}
			else
			{
				worldPointObject = new WorldTreasureObject(world, pointIndex, num);
			}
			break;
		case WorldPointType.HERO_DISPATCH:
			worldPointObject = new WorldHeroDispatchTaskObject(world, pointIndex, num);
			break;
		case WorldPointType.WORLD_ALLIANCE_BUILD:
			if (pointInfo != null)
			{
				AllianceBuildingPointInfo allianceBuildingPointInfo = AllianceBuildingPointInfo.Parser.ParseFrom(pointInfo.extraInfo);
				if (allianceBuildingPointInfo != null)
				{
					worldPointObject = ((GameEntry.ConfigCache.GetTemplateData("alliance_res_build", allianceBuildingPointInfo.BuildId, "type").ToInt() != 3) ? ((WorldPointObject)new WorldAllianceBuildObject(world, pointIndex, num)) : ((WorldPointObject)new WorldZombieRushObject(world, pointIndex, num)));
				}
			}
			break;
		case WorldPointType.WorldAllianceCollectResource:
			worldPointObject = new WorldAllianceCollectResourceObject(world, pointIndex, num);
			break;
		case WorldPointType.WorldSuppliesPoint:
			worldPointObject = new WorldSuppliesPointObject(world, pointIndex, num);
			break;
		case WorldPointType.GHOSTRECON_POINT:
			worldPointObject = new WorldGhostreconTaskObject(world, pointIndex, num);
			break;
		case WorldPointType.RadarSeasonSnowSurvivor:
			worldPointObject = new DetectEventSurvivorObjects(world, pointIndex, num);
			break;
		case WorldPointType.CITY_ATTACHMENT_WALL:
			worldPointObject = new CityAttachmentWallPointObject(world, pointIndex, num);
			break;
		case WorldPointType.CITY_ATTACHMENT_BUILD:
			worldPointObject = new CityAttachmentBuildPointObject(world, pointIndex, num);
			break;
		case WorldPointType.RADAR_DOMINATOR_GUIDE:
		case WorldPointType.RADAR_DOMINATOR_CURE:
		case WorldPointType.RADAR_DOMINATOR_COCKATRICE_UNLOCK_1:
		case WorldPointType.RADAR_DOMINATOR_COCKATRICE_UNLOCK_2:
			worldPointObject = new DominatorGuideWorldPointObject(world, pointIndex, num);
			break;
		case WorldPointType.CAVE_EXPLORATION:
			worldPointObject = new DetectEventCaveExplorationObject(world, pointIndex, num);
			break;
		case WorldPointType.ZONE_MOBILIZATION:
			worldPointObject = new WorldZoneMobilizationObject(world, pointIndex, num);
			break;
		case WorldPointType.WORLD_RUIN_DESTROY_BUILDING:
			worldPointObject = new WorldRuinDestroyBuildingObject(world, pointIndex, num);
			break;
		case WorldPointType.ACTIVITY_WORLD_TREASURE:
			worldPointObject = new WorldActivityTreasureObjects(world, pointIndex, num);
			break;
		case WorldPointType.MONSETER_CHALLENGE_NEW_TREASURE:
			worldPointObject = new NewAlChallengeTreasureObject(world, pointIndex, num);
			break;
		case WorldPointType.DETECT_RETRY_TASK:
			worldPointObject = ((!(world.GetPointInfo(pointIndex) is DetectRetryTaskPointInfo detectRetryTaskPointInfo) || !(detectRetryTaskPointInfo.ownerUid != GameEntry.Data.Player.Uid)) ? ((WorldPointObject)new WorldDetectRetryTaskObject(world, pointIndex, num)) : ((WorldPointObject)new WorldBarricadeObject(world, pointIndex, num)));
			break;
		case WorldPointType.DETECT_ALLIANCE_CITY_SCOUT_MONSTER:
		{
			DetectAttackCityS0TaskPointInfo detectAttackCityS0TaskPointInfo = world.GetPointInfo(pointIndex) as DetectAttackCityS0TaskPointInfo;
			if (detectAttackCityS0TaskPointInfo != null && string.IsNullOrEmpty(detectAttackCityS0TaskPointInfo.eventId))
			{
				detectAttackCityS0TaskPointInfo.eventId = GameEntry.Lua.CallWithReturn<string, long>("CSharpCallLuaInterface.GetAttackCityS0DetectInfo", detectAttackCityS0TaskPointInfo.Id);
			}
			if (detectAttackCityS0TaskPointInfo != null && !string.IsNullOrEmpty(detectAttackCityS0TaskPointInfo.eventId))
			{
				worldPointObject = new WorldDetectAttackCityMonsterObject(world, pointIndex, num);
			}
			break;
		}
		case WorldPointType.DETECT_DIG_GAME:
			worldPointObject = new DetectEventDigGameObject(world, pointIndex, num);
			break;
		case WorldPointType.DETECT_LAST_STAND:
			worldPointObject = new DetectEventLastStandObject(world, pointIndex, num);
			break;
		case WorldPointType.TreasureChest:
			worldPointObject = ((!(world.GetPointInfo(pointIndex) is SamplePointInfo samplePointInfo2) || !(samplePointInfo2.ownerUid != GameEntry.Data.Player.Uid)) ? ((WorldPointObject)new WorldTreasureChestObject(world, pointIndex, num)) : ((WorldPointObject)new WorldBarricadeObject(world, pointIndex, num)));
			break;
		case WorldPointType.SkyBattle:
			worldPointObject = ((!(world.GetPointInfo(pointIndex) is SamplePointInfo samplePointInfo) || !(samplePointInfo.ownerUid != GameEntry.Data.Player.Uid)) ? ((WorldPointObject)new WorldSkyBattleObject(world, pointIndex, num)) : ((WorldPointObject)new WorldBarricadeObject(world, pointIndex, num)));
			break;
		case WorldPointType.DETECT_SUPPLIES_SEARCH:
			worldPointObject = new DetectEventSuppliesSearchObject(world, pointIndex, num);
			break;
		default:
			worldPointObject = new WorldBasePointObject(world, pointIndex, num);
			break;
		}
		if (worldPointObject != null)
		{
			if (pointInfo != null)
			{
				worldPointObject.InitByPointInfo(pointInfo);
			}
			worldPointObject.CreateGameObject();
			if (GameEntry.Data.Player.IsInBattleField())
			{
				world.RemoveDragonLandPoint(pointIndex);
			}
			allObjs.Add(pointIndex, worldPointObject);
			worldPointObject.UpdateTileSize(pointInfo?.tileSize ?? 0);
			if (isRecordingTileBlock && pointInfo != null && recordingWorldId == pointInfo.worldId && (pointInfo.pointType != WorldPointType.TREASURE || !(pointInfo is TreasurePointInfo treasurePointInfo2) || (treasurePointInfo2.killerId.IsNullOrEmpty() && (treasurePointInfo2.type != WorldTreasureType.WolfShadow || !(treasurePointInfo2.ownerUid == GameEntry.Data.Player.Uid)))))
			{
				worldPointObject.RecordBlockIndex(tileBlockIndex);
			}
		}
	}

	private void DestroyObject(int pointIndex)
	{
		if (allObjs.TryGetValue(pointIndex, out var value))
		{
			if (isRecordingTileBlock)
			{
				value.RemoveBlockIndex(tileBlockIndex);
			}
			value.Destroy();
			allObjs.Remove(pointIndex);
		}
	}

	public void AddToDeleteList(int index)
	{
		if (!needDestoryBuild.ContainsKey(index))
		{
			needDestoryBuild.Add(index, value: true);
		}
	}

	private void DestroyBuildList()
	{
		if (needDestoryBuild.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<int, bool> item in needDestoryBuild)
		{
			DestroyObject(item.Key);
		}
		needDestoryBuild.Clear();
	}

	public void AddToDelayDestroyList(IWorldDelayDestroyObject obj)
	{
		if (obj != null)
		{
			delayDestroyList.Add(obj);
		}
	}

	private void UpdateDelayDestroyList()
	{
		if (delayDestroyList.Count <= 0)
		{
			return;
		}
		for (int num = delayDestroyList.Count - 1; num >= 0; num--)
		{
			IWorldDelayDestroyObject worldDelayDestroyObject = delayDestroyList[num];
			if (worldDelayDestroyObject == null || worldDelayDestroyObject.CanDestroy)
			{
				worldDelayDestroyObject?.DestroyImmediate();
				delayDestroyList.RemoveAt(num);
			}
		}
	}

	private void UpdateAssistanceChanged()
	{
		if (aoiAssistanceInfos == null || !aoiAssistanceInfos.IsDirty)
		{
			return;
		}
		dirtyAssistanceList.Clear();
		if (aoiAssistanceInfos.FillDirtyPoints(dirtyAssistanceList) <= 0)
		{
			return;
		}
		int errorCount = 0;
		dirtyAssistanceList.ForEach(delegate(int serverId, int pointIndex, bool dirty)
		{
			PointInfo pointInfo = GetPointInfo(pointIndex);
			if (pointInfo != null)
			{
				switch (pointInfo.pointType)
				{
				case WorldPointType.WINTER_ENTITY:
					if (pointInfo is WinterStormPointInfo winterStormPointInfo)
					{
						winterStormPointInfo.UpdateAssistanceCount();
						MarkPointIsDirty(pointInfo.uuid);
					}
					break;
				case WorldPointType.DRAGON_BUILDING:
					if (pointInfo is DragonPointInfo dragonPointInfo)
					{
						dragonPointInfo.UpdateAssistanceCount();
						MarkPointIsDirty(pointInfo.uuid);
					}
					break;
				case WorldPointType.PlayerBuilding:
					if (pointInfo is BuildPointInfo buildPointInfo)
					{
						buildPointInfo.UpdateAssistanceCount();
						MarkPointIsDirty(pointInfo.uuid);
					}
					else
					{
						errorCount++;
					}
					break;
				case WorldPointType.WORLD_ALLIANCE_CITY:
				case WorldPointType.WORLD_CITY_STRONGHOLD:
				case WorldPointType.WORLD_CITY_TRADE:
					if (pointInfo is AllyCityPointInfo allyCityPointInfo)
					{
						allyCityPointInfo.UpdateAssistanceCount();
						MarkPointIsDirty(pointInfo.uuid);
						_isCityPointUpdate = true;
					}
					else
					{
						errorCount++;
					}
					break;
				case WorldPointType.WORLD_CITY_OUTPOST:
					if (pointInfo is WorldOutpostPoint worldOutpostPoint)
					{
						worldOutpostPoint.UpdateAssistanceCount();
						MarkPointIsDirty(pointInfo.uuid);
						_isCityPointUpdate = true;
					}
					else
					{
						errorCount++;
					}
					break;
				case WorldPointType.WORLD_CITY_OUTPOST_TOWER:
					if (pointInfo is WorldOutpostTowerPoint worldOutpostTowerPoint)
					{
						worldOutpostTowerPoint.UpdateAssistanceCount();
						MarkPointIsDirty(pointInfo.uuid);
						_isCityPointUpdate = true;
					}
					else
					{
						errorCount++;
					}
					break;
				case WorldPointType.WORLD_ALLIANCE_BUILD:
					if (pointInfo is AllianceBuildPointInfo allianceBuildPointInfo)
					{
						allianceBuildPointInfo.UpdateAssistanceCount();
						MarkPointIsDirty(pointInfo.uuid);
					}
					else
					{
						errorCount++;
					}
					break;
				default:
					errorCount++;
					break;
				}
			}
			else
			{
				errorCount++;
			}
		});
		dirtyAssistanceList.Clear();
	}

	public void MarkPointIsDirty(long dirtyPointUuid)
	{
		dirtyPointMarked.Add(dirtyPointUuid);
		MarkPointUpdate();
	}

	private void UpdateDirtyPoints()
	{
		if (dirtyPointMarked.Count <= 0)
		{
			return;
		}
		for (int num = dirtyPointMarked.Count - 1; num >= 0; num--)
		{
			long item = dirtyPointMarked[num];
			if (!tempSet.Add(item))
			{
				dirtyPointMarked.RemoveAt(num);
			}
		}
		for (int i = 0; i < dirtyPointMarked.Count; i++)
		{
			UpdateObjectByUuid(dirtyPointMarked[i]);
		}
		dirtyPointMarked.Clear();
		tempSet.Clear();
	}

	public bool TryGetAssistanceCountByPointIndex(int serverId, int pointIndex, out int count, out int max)
	{
		count = (max = 0);
		return aoiAssistanceInfos?.TryGetAssistanceCount(serverId, pointIndex, out count, out max) ?? false;
	}

	private void ClearAllDelayDestroyList()
	{
		if (delayDestroyList.Count > 0)
		{
			for (int num = delayDestroyList.Count - 1; num >= 0; num--)
			{
				delayDestroyList[num]?.DestroyImmediate();
			}
			delayDestroyList.Clear();
		}
	}

	private void FoldUpBuildObject(int pointIndex)
	{
		if (allObjs.TryGetValue(pointIndex, out var value))
		{
			if (value is WorldBuildObjectNew worldBuildObjectNew)
			{
				worldBuildObjectNew.FoldUpBuild();
			}
			else
			{
				DestroyObject(pointIndex);
			}
		}
	}

	public WorldPointObject GetObjectByPoint(int pointIndex)
	{
		WorldPointObject value = null;
		if (allObjs.TryGetValue(pointIndex, out value))
		{
			return value;
		}
		return null;
	}

	public WorldPointObject GetObjectByPoint(int serverId, int pointIndex)
	{
		WorldPointObject value = null;
		if (allObjs.TryGetValue(pointIndex, out value))
		{
			return value;
		}
		return null;
	}

	public WorldPointObject GetObjectByUuid(long uuid)
	{
		PointInfo pointInfoByUuid = GetPointInfoByUuid(uuid);
		if (pointInfoByUuid == null)
		{
			return null;
		}
		return GetObjectByPoint(pointInfoByUuid.mainIndex);
	}

	protected void ObjectsOnUpdate(float deltaTime)
	{
		foreach (WorldPointObject value in allObjs.Values)
		{
			value.OnUpdate(deltaTime);
		}
		DestroyBuildList();
	}

	public CityBuilding GetBuildingByPoint(int pointIndex)
	{
		return null;
	}

	public CityBuilding GetBuildingByUuid(long uuid)
	{
		PointInfo pointInfoByUuid = GetPointInfoByUuid(uuid);
		if (pointInfoByUuid != null)
		{
			return GetBuildingByPoint(pointInfoByUuid.mainIndex);
		}
		return null;
	}

	public WorldBuilding GetWorldBuildingByPoint(int pointIndex)
	{
		return (GetObjectByPoint(pointIndex) as WorldBuildObjectNew)?.GetCityBuilding();
	}

	public WorldBuilding GetWorldBuildingByUuid(long uuid)
	{
		PointInfo pointInfoByUuid = GetPointInfoByUuid(uuid);
		if (pointInfoByUuid != null)
		{
			return GetWorldBuildingByPoint(pointInfoByUuid.mainIndex);
		}
		return null;
	}

	public bool IsRoad(int index)
	{
		PointInfo pointInfo = GetPointInfo(index);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.PlayerRoad)
		{
			return true;
		}
		return false;
	}

	public bool IsSelfRoad(int index)
	{
		PointInfo pointInfo = GetPointInfo(index);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.PlayerRoad && pointInfo.ownerUid == GameEntry.Data.Player.Uid)
		{
			return true;
		}
		return false;
	}

	public bool IsSelfFreeBoard(int index)
	{
		PointInfo pointInfo = GetPointInfo(index);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.PlayerRoad && pointInfo.ownerUid == GameEntry.Data.Player.Uid)
		{
			return true;
		}
		return false;
	}

	public bool IsBoard(int index)
	{
		PointInfo pointInfo = GetPointInfo(index);
		if (pointInfo != null && pointInfo.pointType == WorldPointType.PlayerRoad)
		{
			return true;
		}
		return false;
	}

	private bool IsNeedChangeBuildObj(BuildPointInfo oldInfo, BuildPointInfo newInfo)
	{
		if (oldInfo != null && newInfo != null)
		{
			if (oldInfo.pointType == WorldPointType.PlayerBuilding && newInfo.pointType == WorldPointType.PlayerBuilding)
			{
				return oldInfo.GetSkinId() != newInfo.GetSkinId();
			}
			string modelPath = oldInfo.GetModelPath();
			string modelPath2 = newInfo.GetModelPath();
			if (!modelPath.IsNullOrEmpty() && !modelPath2.IsNullOrEmpty())
			{
				return modelPath != modelPath2;
			}
		}
		return false;
	}

	public int GetCollectResourceTile()
	{
		return collectResourceTile;
	}

	public int GetCollectResourceRange()
	{
		return collectResourceRange;
	}

	public int GetCollectResourceBuildRange()
	{
		return GetCircleRange(GetCollectResourceRange()) - GetCircleRange(GetCollectResourceTile()) + 1;
	}

	public static int GetCircleRange(int tile)
	{
		return (tile - 1) / 2;
	}

	public int GetCollectPoint(int resourceType)
	{
		foreach (WorldTileInfo value in allViewPoints.Values)
		{
			PointInfo pointInfo = value.GetPointInfo();
			if (pointInfo != null && pointInfo.pointType == WorldPointType.WorldCollectResource && pointInfo.isMainPoint && pointInfo is CollectPointInfo collectPointInfo && collectPointInfo.resourceType == (ResourceType)resourceType)
			{
				return pointInfo.mainIndex;
			}
		}
		return 0;
	}

	public List<int> GetAllCollectRangePoint(int resourceType)
	{
		List<int> list = new List<int>();
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		int circleRange = GetCircleRange(GetCollectResourceTile());
		int circleRange2 = GetCircleRange(GetCollectResourceRange());
		foreach (WorldTileInfo value in allViewPoints.Values)
		{
			PointInfo pointInfo = value.GetPointInfo();
			if (pointInfo == null || pointInfo.pointType != WorldPointType.WorldCollectResource || !pointInfo.isMainPoint || !(pointInfo is CollectPointInfo collectPointInfo) || collectPointInfo.resourceType != (ResourceType)resourceType)
			{
				continue;
			}
			int pointIndex = value.pointIndex;
			for (int num = circleRange2; num > circleRange; num--)
			{
				List<int> list2 = GameEntry.Lua.CallWithReturn<List<int>, int, int, int, int>("CSharpCallLuaInterface.GetOutermostIndexByIndex", pointIndex, num, circleRange2, circleRange2);
				for (int i = 0; i < list2.Count; i++)
				{
					if (!dictionary.ContainsKey(list2[i]))
					{
						if (GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsCanShowCollectGreenByPoint", list2[i]))
						{
							dictionary.Add(list2[i], value: true);
							list.Add(list2[i]);
						}
						else
						{
							dictionary.Add(list2[i], value: false);
						}
					}
				}
			}
		}
		dictionary = null;
		return list;
	}

	public List<int> GetAllCollectRangePointType(int resourceType, int mainIndex)
	{
		List<int> list = new List<int>();
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		int circleRange = GetCircleRange(GetCollectResourceTile());
		int circleRange2 = GetCircleRange(GetCollectResourceRange());
		foreach (WorldTileInfo value in allViewPoints.Values)
		{
			PointInfo pointInfo = value.GetPointInfo();
			if (pointInfo == null || pointInfo.pointType != WorldPointType.WorldCollectResource || !pointInfo.isMainPoint || pointInfo.mainIndex != mainIndex || !(pointInfo is CollectPointInfo collectPointInfo) || collectPointInfo.resourceType != (ResourceType)resourceType)
			{
				continue;
			}
			int pointIndex = pointInfo.pointIndex;
			for (int num = circleRange2; num > circleRange; num--)
			{
				List<int> list2 = GameEntry.Lua.CallWithReturn<List<int>, int, int, int, int>("CSharpCallLuaInterface.GetOutermostIndexByIndex", pointIndex, num, circleRange2, circleRange2);
				for (int i = 0; i < list2.Count; i++)
				{
					if (!dictionary.ContainsKey(list2[i]))
					{
						if (GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsCanShowCollectGreenByPoint", list2[i]))
						{
							dictionary.Add(list2[i], value: true);
							list.Add(list2[i]);
						}
						else
						{
							dictionary.Add(list2[i], value: false);
						}
					}
				}
			}
		}
		dictionary = null;
		return list;
	}

	public List<int> GetAllCollectRange(int resourceType, int mainIndex)
	{
		List<int> list = new List<int>();
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		int circleRange = GetCircleRange(GetCollectResourceTile());
		int circleRange2 = GetCircleRange(GetCollectResourceRange());
		foreach (WorldTileInfo value in allViewPoints.Values)
		{
			PointInfo pointInfo = value.GetPointInfo();
			if (pointInfo == null || pointInfo.pointType != WorldPointType.WorldCollectResource || !pointInfo.isMainPoint || pointInfo.mainIndex != mainIndex || !(pointInfo is CollectPointInfo collectPointInfo) || collectPointInfo.resourceType != (ResourceType)resourceType)
			{
				continue;
			}
			int pointIndex = pointInfo.pointIndex;
			for (int num = circleRange2; num > circleRange; num--)
			{
				List<int> list2 = GameEntry.Lua.CallWithReturn<List<int>, int, int, int, int>("CSharpCallLuaInterface.GetOutermostIndexByIndex", pointIndex, num, circleRange2, circleRange2);
				for (int i = 0; i < list2.Count; i++)
				{
					if (!dictionary.ContainsKey(list2[i]))
					{
						list.Add(list2[i]);
					}
				}
			}
		}
		dictionary = null;
		return list;
	}

	public List<int> GetGarbagePoint()
	{
		List<int> list = new List<int>();
		foreach (WorldTileInfo value in allViewPoints.Values)
		{
			PointInfo pointInfo = value.GetPointInfo();
			if (pointInfo != null && pointInfo.pointType == WorldPointType.GARBAGE && pointInfo.isMainPoint && pointInfo is GarbagePointInfo)
			{
				list.Add(pointInfo.mainIndex);
			}
		}
		if (list.Count > 1)
		{
			Vector2Int mainPos = SceneManager.World.IndexToTilePos(SceneManager.World.curIndex);
			list.Sort(delegate(int a, int b)
			{
				Vector2Int vector2Int = SceneManager.World.IndexToTilePos(a) - mainPos;
				Vector2Int vector2Int2 = SceneManager.World.IndexToTilePos(b) - mainPos;
				return vector2Int.x * vector2Int.x + vector2Int.y * vector2Int.y - vector2Int2.x * vector2Int2.x + vector2Int2.y * vector2Int2.y;
			});
		}
		return list;
	}

	public BuildPointInfo GetBaseMainInfoByOwnerUid(string ownerUid)
	{
		if (baseMainBuild.ContainsKey(ownerUid))
		{
			PointInfo pointInfoByUuid = GetPointInfoByUuid(baseMainBuild[ownerUid]);
			if (pointInfoByUuid != null)
			{
				return pointInfoByUuid as BuildPointInfo;
			}
		}
		return null;
	}

	public bool IsOutCityByPoint(int point)
	{
		Vector2Int vector2Int = world.IndexToTilePos(point);
		foreach (KeyValuePair<string, long> item in baseMainBuild)
		{
			PointInfo pointInfoByUuid = GetPointInfoByUuid(item.Value);
			if (pointInfoByUuid == null || !(pointInfoByUuid is BuildPointInfo buildPointInfo))
			{
				continue;
			}
			int buildId = 10100000 + buildPointInfo.level;
			int buildOffsetRangeByBuildId = SceneManager.World.GetBuildOffsetRangeByBuildId(buildId);
			if (buildOffsetRangeByBuildId > 0)
			{
				int circleRange = GetCircleRange(buildPointInfo.tileSize);
				Vector2Int vector2Int2 = world.IndexToTilePos(buildPointInfo.mainIndex) - new Vector2Int(circleRange, circleRange);
				int num = buildOffsetRangeByBuildId + circleRange;
				if ((vector2Int2.x - vector2Int.x) * (vector2Int2.x - vector2Int.x) + (vector2Int2.y - vector2Int.y) * (vector2Int2.y - vector2Int.y) <= num * num)
				{
					return false;
				}
			}
		}
		return true;
	}

	public BuildPointInfo GetBaseMainByScreen()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		foreach (KeyValuePair<string, long> item in baseMainBuild)
		{
			PointInfo pointInfoByUuid = GetPointInfoByUuid(item.Value);
			if (pointInfoByUuid != null)
			{
				Vector3 vector = world.WorldToScreenPoint(world.TileIndexToWorld(pointInfoByUuid.mainIndex));
				if (vector.x > 0f && vector.x < num && vector.y > 0f && vector.y < num2)
				{
					return pointInfoByUuid as BuildPointInfo;
				}
			}
		}
		return null;
	}

	public List<BuildPointInfo> GetMainByScreen()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		List<BuildPointInfo> list = new List<BuildPointInfo>();
		foreach (KeyValuePair<string, long> item in baseMainBuild)
		{
			PointInfo pointInfoByUuid = GetPointInfoByUuid(item.Value);
			if (pointInfoByUuid != null)
			{
				Vector3 vector = world.WorldToScreenPoint(world.TileIndexToWorld(pointInfoByUuid.mainIndex));
				if (vector.x > 0f && vector.x < num && vector.y > 0f && vector.y < num2)
				{
					list.Add(pointInfoByUuid as BuildPointInfo);
				}
			}
		}
		return list;
	}

	public List<BuildPointInfo> GetAllMainBaseList()
	{
		List<BuildPointInfo> list = new List<BuildPointInfo>();
		foreach (KeyValuePair<string, long> item2 in baseMainBuild)
		{
			PointInfo pointInfoByUuid = GetPointInfoByUuid(item2.Value);
			if (pointInfoByUuid != null && pointInfoByUuid.pointType == WorldPointType.PlayerBuilding && pointInfoByUuid is BuildPointInfo item)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0)
	{
		List<BuildPointInfo> list = new List<BuildPointInfo>();
		foreach (KeyValuePair<string, long> item2 in baseMainBuild)
		{
			PointInfo pointInfoByUuid = GetPointInfoByUuid(item2.Value);
			PlayerType playerType = pointInfoByUuid.GetPlayerType();
			if (pointInfoByUuid != null && pointInfoByUuid.pointType == WorldPointType.PlayerBuilding && pointInfoByUuid is BuildPointInfo item && playerType == t0)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1)
	{
		List<BuildPointInfo> list = new List<BuildPointInfo>();
		foreach (KeyValuePair<string, long> item2 in baseMainBuild)
		{
			PointInfo pointInfoByUuid = GetPointInfoByUuid(item2.Value);
			PlayerType playerType = pointInfoByUuid.GetPlayerType();
			if (pointInfoByUuid != null && pointInfoByUuid.pointType == WorldPointType.PlayerBuilding && pointInfoByUuid is BuildPointInfo item && (playerType == t0 || playerType == t1))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1, PlayerType t2)
	{
		List<BuildPointInfo> list = new List<BuildPointInfo>();
		foreach (KeyValuePair<string, long> item2 in baseMainBuild)
		{
			PointInfo pointInfoByUuid = GetPointInfoByUuid(item2.Value);
			PlayerType playerType = pointInfoByUuid.GetPlayerType();
			if (pointInfoByUuid != null && pointInfoByUuid.pointType == WorldPointType.PlayerBuilding && pointInfoByUuid is BuildPointInfo item && (playerType == t0 || playerType == t1 || playerType == t2))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1, PlayerType t2, PlayerType t3)
	{
		List<BuildPointInfo> list = new List<BuildPointInfo>();
		foreach (KeyValuePair<string, long> item2 in baseMainBuild)
		{
			PointInfo pointInfoByUuid = GetPointInfoByUuid(item2.Value);
			PlayerType playerType = pointInfoByUuid.GetPlayerType();
			if (pointInfoByUuid != null && pointInfoByUuid.pointType == WorldPointType.PlayerBuilding && pointInfoByUuid is BuildPointInfo item && (playerType == t0 || playerType == t1 || playerType == t2 || playerType == t3))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<PointInfo> GetAllDragonPointList()
	{
		List<PointInfo> list = new List<PointInfo>();
		foreach (KeyValuePair<long, long> item in baseDragonPoint)
		{
			PointInfo pointInfoByUuid = GetPointInfoByUuid(item.Value);
			if (pointInfoByUuid != null)
			{
				list.Add(pointInfoByUuid);
			}
		}
		return list;
	}

	public List<int> GetAllDragonResourceList()
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, WorldTileInfo> allViewPoint in allViewPoints)
		{
			WorldTileInfo value = allViewPoint.Value;
			if (value != null)
			{
				PointInfo pointInfo = value.GetPointInfo();
				if (pointInfo != null && pointInfo.worldId > 0 && pointInfo.pointType == WorldPointType.WorldResource)
				{
					list.Add(value.pointIndex);
				}
			}
		}
		return list;
	}

	public bool IsNeedPlayPlacedAnim(long uuid)
	{
		if (_needPlayPlacedAnimBuild.ContainsKey(uuid))
		{
			_needPlayPlacedAnimBuild.Remove(uuid);
			return true;
		}
		return false;
	}

	public void CheckNeedRefreshRoad()
	{
		foreach (int item in _needRefreshBoard)
		{
			if (!_needRefreshDeleteBoard.ContainsKey(item))
			{
				_needRefreshDeleteBoard[item] = true;
				PointInfo pointInfo = GetPointInfo(item);
				if (pointInfo != null && pointInfo.pointType == WorldPointType.PlayerRoad)
				{
					UpdateObject(item);
				}
			}
		}
		_needRefreshBoard.Clear();
		_needRefreshDeleteBoard.Clear();
	}

	public void RefreshRoads()
	{
		foreach (KeyValuePair<int, WorldPointObject> allObj in allObjs)
		{
			PointInfo pointInfo = world.GetPointInfo(allObj.Key);
			if (pointInfo != null && pointInfo.pointType == WorldPointType.PlayerRoad)
			{
				allObj.Value.UpdateGameObject();
			}
		}
	}

	public void OnDrawGizmos()
	{
		DrawAoiGizmos();
	}

	public void ChangeOutEdgeScale(float scale)
	{
		_outEdgeScale = scale;
		UpdateLWAoi(isForce: true);
	}

	private void UpdateLWAoi(bool isForce = false)
	{
		if (!startViewRequest)
		{
			return;
		}
		if (isForce)
		{
			world.Camera.OnUpdate(0f);
		}
		if (isForce || !_lockAoiBlocksUpdate)
		{
			if (world.WorldSize == 3000)
			{
				UpdateLWAoi_Big3000(isForce);
			}
			else
			{
				UpdateLWAoi_Normal(isForce);
			}
		}
	}

	public void SetFirstViewRequestFlag(bool flag)
	{
		firstTimeReqAoi = flag;
	}

	public void SetBattleFieldFirst(bool flag)
	{
		battleFieldFirst = flag;
	}

	public bool IsOutOfLWAoi(int pointIndex, int serverId = 0)
	{
		int num = 0;
		int num2 = 0;
		if (pointIndex >= 1 && pointIndex <= 1000000)
		{
			int num3 = pointIndex - 1;
			num = num3 % 1000;
			num2 = num3 / 1000;
		}
		int num4 = 0;
		if (serverId > 0 && world.WorldSize > 1000)
		{
			Vector3 worldBasePos = SeasonDataManager.Instance.GetWorldBasePos(serverId);
			int num5 = (num + (int)worldBasePos.x / 2) / _lwAoiBlockSize;
			num4 = (num2 + (int)worldBasePos.z / 2) / _lwAoiBlockSize * _lwAoiBlockCount + num5;
		}
		else
		{
			int num6 = num / _lwAoiBlockSize;
			num4 = num2 / _lwAoiBlockSize * _lwAoiBlockCount + num6;
		}
		return !_curViewIndex.Contains(num4);
	}

	private void DeleteOutOfAoi()
	{
		Vector3 curTarget = world.CurTarget;
		foreach (WorldTileInfo value3 in allViewPoints.Values)
		{
			PointInfo pointInfo = value3.GetPointInfo();
			if (pointInfo == null)
			{
				if (IsOutOfLWAoi(value3.pointIndex, value3.serverId) && !outOfViewPoints.ContainsKey(value3.pointIndex))
				{
					outOfViewPoints.Add(value3.pointIndex, value3);
				}
			}
			else
			{
				if (!pointInfo.isMainPoint)
				{
					continue;
				}
				if (pointInfo.pointType == WorldPointType.WORLD_ALLIANCE_BUILD)
				{
					if (pointInfo is AllianceBuildPointInfo { offter_range: >0 } allianceBuildPointInfo)
					{
						Vector3 vector = TileCoord.TileIndexToWorld(pointInfo.pointIndex, ForceChangeScene.World, pointInfo.serverId);
						int num = Math.Max(allianceBuildPointInfo.offter_range * 2, 30) * 2;
						if (Math.Abs(vector.x - curTarget.x) < (float)num && Math.Abs(vector.z - curTarget.z) < (float)num)
						{
							continue;
						}
					}
				}
				else if (pointInfo.pointType == WorldPointType.WORLD_ALLIANCE_CITY || pointInfo.pointType == WorldPointType.WORLD_CITY_STRONGHOLD || pointInfo.pointType == WorldPointType.WORLD_CITY_TRADE || pointInfo.pointType == WorldPointType.GOLD_TREE)
				{
					if (pointInfo is AllyCityPointInfo { CityField: >0 } allyCityPointInfo)
					{
						Vector3 vector2 = TileCoord.TileIndexToWorld(pointInfo.pointIndex, ForceChangeScene.World, pointInfo.serverId);
						int num2 = Math.Max(allyCityPointInfo.CityField * 2, 20) * 2;
						if (Math.Abs(vector2.x - curTarget.x) < (float)num2 && Math.Abs(vector2.z - curTarget.z) < (float)num2)
						{
							continue;
						}
					}
				}
				else if (pointInfo.pointType == WorldPointType.CITY_ATTACHMENT_WALL || pointInfo.pointType == WorldPointType.CITY_ATTACHMENT_BUILD)
				{
					Vector3 vector3 = TileCoord.TileIndexToWorld(pointInfo.pointIndex, ForceChangeScene.World, pointInfo.serverId);
					if (Math.Abs(vector3.x - curTarget.x) < 32f && Math.Abs(vector3.z - curTarget.z) < 32f)
					{
						continue;
					}
				}
				else if (pointInfo.pointType == WorldPointType.WINTER_ENTITY || pointInfo.pointType == WorldPointType.BATTLEFIELD_TYPE)
				{
					if (!IsOutOfLWAoi(value3.pointIndex))
					{
						continue;
					}
					int count = 0;
					aoiAssistanceInfos?.TryGetAssistanceCount(value3.serverId, value3.pointIndex, out count, out var _);
					if (count > 0)
					{
						aoiAssistanceInfos?.RemoveByPointIndex(value3.serverId, value3.pointIndex);
						if (pointInfo != null)
						{
							MarkPointIsDirty(pointInfo.uuid);
						}
					}
					continue;
				}
				if (IsOutOfLWAoi(value3.pointIndex, value3.serverId))
				{
					if (!outOfViewPoints.ContainsKey(value3.pointIndex))
					{
						outOfViewPoints.Add(value3.pointIndex, value3);
					}
				}
				else if (IsOutOfViewRange(value3.pointIndex, value3.serverId) && !outOfViewPointsObj.ContainsKey(value3.pointIndex))
				{
					outOfViewPointsObj.Add(value3.pointIndex, value3);
				}
			}
		}
		if (outOfViewPoints.Count > 0)
		{
			keysToRemove.Clear();
			foreach (KeyValuePair<int, WorldTileInfo> outOfViewPoint in outOfViewPoints)
			{
				WorldTileInfo value = outOfViewPoint.Value;
				DestroyObject(value.pointIndex);
				RemovePointInfo(value.pointIndex, forceRemove: true);
				keysToRemove.Add(value.pointIndex);
			}
			foreach (int item in keysToRemove)
			{
				outOfViewPoints.Remove(item);
			}
		}
		if (outOfViewPointsObj.Count <= 0)
		{
			return;
		}
		keysToRemove.Clear();
		foreach (KeyValuePair<int, WorldTileInfo> item2 in outOfViewPointsObj)
		{
			WorldTileInfo value2 = item2.Value;
			DestroyObject(value2.pointIndex);
			keysToRemove.Add(value2.pointIndex);
		}
		foreach (int item3 in keysToRemove)
		{
			outOfViewPointsObj.Remove(item3);
		}
	}

	private void RecalculateBlockArg(int tileCount, int serverLod, out int size, out int count)
	{
		serverLod = Mathf.Clamp(serverLod, 0, 2);
		size = lwAoiBlockSizeArray[serverLod];
		count = tileCount / size;
	}

	private Vector2Int PositionToAoiBlock(float x, float y)
	{
		x = Mathf.Max(0f, x);
		y = Mathf.Max(0f, y);
		return new Vector2Int(Mathf.FloorToInt(x / 2f / (float)_lwAoiBlockSize), Mathf.FloorToInt(y / 2f / (float)_lwAoiBlockSize));
	}

	private int AoiBlockToIndex(int x, int y)
	{
		return y * _lwAoiBlockCount + x;
	}

	private void IndexToAoiBlock(int index, out int x, out int y)
	{
		x = index % _lwAoiBlockCount;
		y = index / _lwAoiBlockCount;
	}

	private void AoiBlockToTilePos(Vector2Int block, out int x, out int y)
	{
		x = block.x * _lwAoiBlockSize * 2;
		y = block.y * _lwAoiBlockSize * 2;
	}

	private void DrawAoiGizmos()
	{
		float num = (float)(_lwAoiBlockSize * 2) * 0.9f;
		Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.6f);
		for (int i = 0; i < _addViewIndex.Count; i++)
		{
			Gizmos.DrawCube(GetAoiIndexCenter(_addViewIndex[i]), new Vector3(num, 1f, num));
		}
		Gizmos.color = new Color(0.2f, 0.2f, 0.8f, 0.6f);
		HashSet<int>.Enumerator enumerator = _curViewIndex.GetEnumerator();
		while (enumerator.MoveNext())
		{
			int current = enumerator.Current;
			if (!_addViewIndex.Contains(current))
			{
				Gizmos.DrawCube(GetAoiIndexCenter(current), new Vector3(num, 1f, num));
			}
		}
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		WorldCamera camera = world.Camera;
		Vector3 vector = camera.cameraAnchor[0];
		Vector3 vector2 = camera.cameraAnchor[2];
		Vector3 vector3 = camera.cameraAnchor[1];
		Vector3 vector4 = camera.cameraAnchor[3];
		Vector3 vector5 = new Vector3(vector3.x - _outEdge, 0f, vector3.z + _outEdge);
		Vector3 vector6 = new Vector3(vector.x - _outEdge, 0f, vector.z - _outEdge);
		Vector3 vector7 = new Vector3(vector4.x + _outEdge, 0f, vector4.z - _outEdge);
		Vector3 vector8 = new Vector3(vector2.x + _outEdge, 0f, vector2.z + _outEdge);
		Gizmos.DrawLine(vector6, vector7);
		Gizmos.DrawLine(vector7, vector8);
		Gizmos.DrawLine(vector8, vector5);
		Gizmos.DrawLine(vector5, vector6);
		Gizmos.color = Color.yellow;
		vector5 = new Vector3(_leftBottomWorldPos.x, 0f, _rightTopWorldPos.z);
		vector6 = new Vector3(_leftBottomWorldPos.x, 0f, _leftBottomWorldPos.z);
		vector7 = new Vector3(_rightTopWorldPos.x, 0f, _leftBottomWorldPos.z);
		vector8 = new Vector3(_rightTopWorldPos.x, 0f, _rightTopWorldPos.z);
		Gizmos.DrawLine(vector6, vector7);
		Gizmos.DrawLine(vector7, vector8);
		Gizmos.DrawLine(vector8, vector5);
		Gizmos.DrawLine(vector5, vector6);
	}

	private Vector3 GetAoiIndexCenter(int index)
	{
		IndexToAoiBlock(index, out var x, out var y);
		return new Vector3(_lwAoiBlockSize * x * 2 + _lwAoiBlockSize, 0f, _lwAoiBlockSize * y * 2 + _lwAoiBlockSize);
	}

	public Color GetLabelSkinColor(int skinId, int colorType)
	{
		if (labelSkinColorCfg.TryGetValue(skinId, out var value))
		{
			if (value.TryGetValue(colorType, out var value2))
			{
				return value2;
			}
			return value[colorType] = GameEntry.Lua.CallWithReturn<Color, int, int>("CSharpCallLuaInterface.GetLabelTextColor", skinId, colorType);
		}
		Color color2 = GameEntry.Lua.CallWithReturn<Color, int, int>("CSharpCallLuaInterface.GetLabelTextColor", skinId, colorType);
		value = new Dictionary<int, Color> { [colorType] = color2 };
		labelSkinColorCfg[skinId] = value;
		return color2;
	}

	public float GetLabelSkinOffset(int skinId)
	{
		if (labelSkinOffsetCfg.TryGetValue(skinId, out var value))
		{
			return value;
		}
		value = GameEntry.Lua.CallWithReturn<float, int>("CSharpCallLuaInterface.GetTitleNameDeltaX", skinId);
		labelSkinOffsetCfg[skinId] = value;
		return value;
	}

	public float GetLabelSkinSizeAdd(int skinId)
	{
		if (labelSkinSizeAddCfg.TryGetValue(skinId, out var value))
		{
			return value;
		}
		value = GameEntry.Lua.CallWithReturn<float, int>("CSharpCallLuaInterface.GetWorldNameBgSize", skinId);
		labelSkinSizeAddCfg[skinId] = value;
		return value;
	}

	private void ClearConfigCache()
	{
		foreach (Dictionary<int, Color> value in labelSkinColorCfg.Values)
		{
			value.Clear();
		}
		labelSkinColorCfg.Clear();
		labelSkinOffsetCfg.Clear();
		labelSkinSizeAddCfg.Clear();
	}

	public bool GetProfileSwitch()
	{
		return profileSwitch;
	}

	public void ProfileToggle()
	{
		profileSwitch = !profileSwitch;
		foreach (KeyValuePair<int, WorldPointObject> allObj in allObjs)
		{
			allObj.Value.SetVisible(profileSwitch);
		}
	}

	public Dictionary<int, int> GetSpecialPointDic()
	{
		return specialBuildDic;
	}

	public void HandleSandWormUpdate(BuildPointInfo bi, SandWormAnim anim)
	{
		if (allObjs.TryGetValue(bi.pointIndex, out var value) && value is WorldBuildObjectNew worldBuildObjectNew)
		{
			worldBuildObjectNew.CheckShowSandWorm(bi, anim);
		}
	}

	public void StartRecordTileBlock(int worldId)
	{
		if (isRecordingTileBlock)
		{
			return;
		}
		isRecordingTileBlock = true;
		recordingWorldId = worldId;
		tileBlockIndex.Clear();
		int num = 0;
		int num2 = -1;
		foreach (KeyValuePair<int, WorldPointObject> allObj in allObjs)
		{
			WorldPointObject value = allObj.Value;
			if (value == null)
			{
				continue;
			}
			if (value.WorldId != recordingWorldId)
			{
				num++;
				num2 = value.WorldId;
				continue;
			}
			PointInfo pointInfo = GetPointInfo(value.GetPointIndex(), value.GetServerId());
			if (pointInfo != null && pointInfo.pointType == WorldPointType.TREASURE && pointInfo is TreasurePointInfo treasurePointInfo)
			{
				if (!treasurePointInfo.killerId.IsNullOrEmpty())
				{
					num++;
					continue;
				}
				if (treasurePointInfo.type == WorldTreasureType.WolfShadow && treasurePointInfo.ownerUid == GameEntry.Data.Player.Uid)
				{
					num++;
					continue;
				}
			}
			allObj.Value?.RecordBlockIndex(tileBlockIndex);
		}
		if (num > 0)
		{
			Log.Info($"WorldMapGridRenderer.StartRecordTileBlock skip point count => {num}! target worldId:{recordingWorldId}, skip worldId:{num2}");
		}
	}

	public void StopRecordTileBlock()
	{
		tileBlockIndex.Clear();
		isRecordingTileBlock = false;
		recordingWorldId = int.MinValue;
	}

	public void SetFocusPoint(int focusPoint)
	{
		if (EnableWorldAssistanceOpt)
		{
			this.focusPoint = focusPoint;
		}
	}

	private void OnRefreshAllAllianceBuildingsInView(object obj = null)
	{
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		string uid = GameEntry.Data.Player.GetUid();
		foreach (KeyValuePair<int, WorldTileInfo> allViewPoint in allViewPoints)
		{
			PointInfo pointInfo = allViewPoint.Value.GetPointInfo();
			if (pointInfo != null && pointInfo.isMainPoint && pointInfo.pointType == WorldPointType.PlayerBuilding && pointInfo is BuildPointInfo { itemId: 10100000 } buildPointInfo && ((!string.IsNullOrEmpty(allianceId) && buildPointInfo.allianceId == allianceId) || buildPointInfo.ownerUid == uid) && !IsOutOfViewRange(allViewPoint.Value.pointIndex, allViewPoint.Value.serverId))
			{
				GetObjectByPoint(pointInfo.mainIndex)?.UpdateGameObject();
			}
		}
	}

	public void OnWorldColorDirty(string allianceId)
	{
		foreach (WorldPointObject value in allObjs.Values)
		{
			value.OnWorldColorDirty(allianceId);
		}
	}

	public void HandlePushWorldObjStateChange(ISFSObject msg)
	{
		long uuid = msg.TryGetLong("objUuid");
		GetPointInfoByUuid(uuid)?.SetStatus(msg);
	}

	public static void OnCityStatusChanged(int serverId, int cityId)
	{
	}

	public static long GetCityUuid(int serverId, int cityId)
	{
		if (battlePoints.TryGetValue(serverId, cityId, out var value))
		{
			return value;
		}
		return 0L;
	}

	public static void DoMissileFire(int serverId, int cityId, long uuid)
	{
		if (uuid <= 0 && battlePoints.TryGetValue(serverId, cityId, out var value))
		{
			uuid = value;
		}
		if (uuid <= 0)
		{
			return;
		}
		SceneInterface sceneInterface = SceneManager.World;
		if (sceneInterface != null)
		{
			WorldPointObject objectByUuid = sceneInterface.GetObjectByUuid(uuid);
			if (objectByUuid != null && objectByUuid is WorldOutpostTowerPointObject worldOutpostTowerPointObject && worldOutpostTowerPointObject.MissileFire != null)
			{
				worldOutpostTowerPointObject.MissileFire.DoFire();
			}
		}
	}
}
