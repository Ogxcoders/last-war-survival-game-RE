using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameFramework;
using GameKit.Base;
using Main.Scripts.Scene.LightAndDark;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using VEngine;

public class WorldStaticManager : WorldManagerBase
{
	public enum StaticObjectType
	{
		Decorate,
		City,
		MapOuter,
		LargeObject,
		GroundObject
	}

	private class Block
	{
		private WorldSceneDesc blockTpl;

		private WorldBlockDesc blockTileTpl;

		private Dictionary<Vector2Int, WorldDecorationChunkDesc> _chunks = new Dictionary<Vector2Int, WorldDecorationChunkDesc>();

		public void Init(byte[] bytes, WorldStaticManager mgr, int dataType)
		{
			blockTpl = GetWorldSceneDesc(bytes, dataType);
			_chunks = _cacheDecorationChunkDescMap;
		}

		public List<WorldSceneDesc.ObjectDesc> GetChunkObjList(Vector2Int chunkCoord)
		{
			if (_chunks != null && _chunks.TryGetValue(chunkCoord, out var value))
			{
				return value.GetChunkObjList(ObjectExtents, isS5: false);
			}
			return null;
		}
	}

	public enum DecorationInvisibleRectType
	{
		None,
		Circle
	}

	public struct CullData
	{
		public AABB bound;

		public uint lods;

		public int4 tileIndexes;
	}

	private struct CullingJob : IJobParallelFor
	{
		[DeallocateOnJobCompletion]
		[ReadOnly]
		public NativeArray<FrustumPlanes.PlanePacket4> Planes;

		[ReadOnly]
		public NativeArray<CullData> CullDatas;

		[ReadOnly]
		public NativeArray<int> TileDatas;

		[NativeDisableParallelForRestriction]
		public NativeArray<BatchVisibility> Batches;

		[NativeDisableParallelForRestriction]
		public NativeArray<int> IndexList;

		public uint lod;

		public int2 tileCount;

		public void Execute(int index)
		{
			BatchVisibility value = Batches[index];
			int num = 0;
			int instancesCount = value.instancesCount;
			int offset = value.offset;
			for (int i = 0; i < instancesCount; i++)
			{
				CullData cullData = CullDatas[offset + i];
				if ((lod & cullData.lods) == 0)
				{
					continue;
				}
				int x = cullData.tileIndexes.x;
				int y = cullData.tileIndexes.y;
				int z = cullData.tileIndexes.z;
				int w = cullData.tileIndexes.w;
				bool flag = false;
				for (int j = x; j <= y; j++)
				{
					for (int k = z; k <= w; k++)
					{
						int index2 = j + k * tileCount.x + 1;
						if (TileDatas[index2] > 0)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag && FrustumPlanes.Intersect2(Planes, cullData.bound) != FrustumPlanes.IntersectResult.Out)
				{
					IndexList[value.offset + num] = i;
					num++;
				}
			}
			value.visibleCount = num;
			Batches[index] = value;
		}
	}

	public class Region_S5
	{
		private struct S5_ChunksCache
		{
			public Vector2Int chunkCoord;

			public bool isLoaded;

			public bool isEmpty;

			public float loadTime;
		}

		private WorldSceneDesc blockTpl;

		private WorldBlockDesc blockTileTpl;

		public bool isEmptyRegion;

		public float usedTime = -1f;

		private Dictionary<Vector2Int, WorldDecorationChunkDesc> chunkMap;

		private Dictionary<Vector2Int, S5_ChunksCache> _S5ChunksCache = new Dictionary<Vector2Int, S5_ChunksCache>();

		private List<Asset> loadingAssets = new List<Asset>();

		public int ServerIndex = -1;

		public void Init(byte[] bytes, WorldStaticManager mgr)
		{
			if (bytes == null || bytes.Length == 0)
			{
				isEmptyRegion = true;
				return;
			}
			isEmptyRegion = false;
			chunkMap = LoadWorldDecoration(bytes);
			usedTime = Time.realtimeSinceStartup;
		}

		public Dictionary<Vector2Int, WorldDecorationChunkDesc> LoadWorldDecoration(byte[] data)
		{
			if (data == null || data.Length == 0)
			{
				return null;
			}
			using BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
			int num = binaryReader.ReadInt32();
			Dictionary<Vector2Int, WorldDecorationChunkDesc> dictionary = new Dictionary<Vector2Int, WorldDecorationChunkDesc>(num);
			int objectDescByteCount = binaryReader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				WorldDecorationChunkDesc worldDecorationChunkDesc = new WorldDecorationChunkDesc(2);
				worldDecorationChunkDesc.Load(binaryReader, objectDescByteCount);
				dictionary.Add(worldDecorationChunkDesc.chunkCoord, worldDecorationChunkDesc);
			}
			return dictionary;
		}

		public List<WorldSceneDesc.ObjectDesc> GetChunkObjList(Vector2Int chunkCoord)
		{
			if (chunkMap != null && chunkMap.TryGetValue(chunkCoord, out var value))
			{
				return value.GetChunkObjList(ObjectExtents, isS5: true);
			}
			return null;
		}
	}

	private static int CreateCountPerFrame = -1;

	private const int TileCountPerChunk = 25;

	private static int ObjectExtents = 1;

	private const int ObjLod = 5;

	private const int AllianceCityLod = 6;

	private const int TerrainLod = 8;

	private Transform parentNode;

	private static readonly int Prop_Control = Shader.PropertyToID("_Control");

	private static readonly int Prop_Splat0 = Shader.PropertyToID("_Splat0");

	private static readonly int Prop_Splat1 = Shader.PropertyToID("_Splat1");

	private static readonly int Prop_Control_ST = Shader.PropertyToID("_Control_ST");

	private static readonly int Prop_Splat0_ST = Shader.PropertyToID("_Splat0_ST");

	private static readonly int Prop_Splat1_ST = Shader.PropertyToID("_Splat1_ST");

	private static readonly int Prop_TerrainBounds = Shader.PropertyToID("_TerrainBounds");

	private const float CAMERA_ANCHOR_BIAS = 3f;

	private int chunkCountPerBlock;

	private Dictionary<string, Asset> terrainMatAssets_hight = new Dictionary<string, Asset>();

	private Dictionary<string, Asset> terrainMatAssets_low = new Dictionary<string, Asset>();

	private List<Asset> terrainAssets = new List<Asset>();

	private List<Asset> terrainAssetsNew = new List<Asset>();

	private Asset s2PreLoadTerrainAsset;

	private Dictionary<Vector2Int, GameObject> terrains = new Dictionary<Vector2Int, GameObject>();

	private bool _useNewTerrainQuad;

	private TerrainS2 _terrainS2;

	private TerrainQuadLayerRenderer _terrainS2Renderer;

	private float _timeSinceLevelLoad;

	private WorldFogRendererRenderer _fogRenderer;

	private ShakeDetection _shakeDetection;

	private Asset worldSceneDescAsset;

	private Asset WorldBlockDescAsset;

	private Asset DragonLandRangeDescAsset;

	private Asset DragonLandMapDataAsset;

	private List<Block> blockTemplates;

	private Block[] worldBlocks;

	private List<WorldSceneDesc.ObjectDesc> chunkObjList = new List<WorldSceneDesc.ObjectDesc>();

	private int lastViewLevel;

	private Vector2Int lastViewChunk;

	private Vector3 lastViewPos;

	private int lastViewRange;

	private float _lastZoom;

	private InstanceRequest blackBlockRequest;

	private InstanceRequest dragonBlockRequest;

	private Transform dragonDecorateRoot;

	private Dictionary<Vector2Int, int> occupyPoints = new Dictionary<Vector2Int, int>();

	private HashSet<int> obstacles = new HashSet<int>();

	private bool isLoadFinish;

	private Action loadTerrainCallback;

	private static WorldSceneDesc _worldSceneDesc;

	private static Dictionary<Vector2Int, WorldDecorationChunkDesc> _cacheDecorationChunkDescMap;

	private HashSet<int> _greenTileChangeList;

	public bool mIsNineNationMode;

	public bool mCurrentTerrain_HightQuality;

	private int lastSkinId = -1;

	private string lastWorldSceneDescPath;

	private string lastWorldBlockDescPath;

	private string lastWorldBlackBlockPath;

	private MapData dragonMapData;

	private Dictionary<DragonLandItem, int> dragonDelayList = new Dictionary<DragonLandItem, int>();

	private List<DragonLandItem> dragonBlockDataList;

	private Dictionary<int, List<GameObject>> dictDragonWarDesertData = new Dictionary<int, List<GameObject>>();

	private Vector2Int minXY;

	private Vector2Int maxXY;

	private Vector2Int lastMinXY;

	private Vector2Int lastMaxXY;

	private Vector3 cameraAnchorLeftBottom;

	private Vector3 cameraAnchorRightTop;

	private bool profileTerrainSwitch = true;

	private bool profileSwitch = true;

	private int _offset;

	private string _curPath;

	private static DecorationRenderData _decorationRenderData;

	private static Asset _decorationRenderDatAsset;

	private static readonly Dictionary<int, Dictionary<uint, DecorationRenderMesh[]>> _renderAssetMap = new Dictionary<int, Dictionary<uint, DecorationRenderMesh[]>>();

	private static readonly Dictionary<DecorationRenderMesh, Material> _renderMeshMaterialMap = new Dictionary<DecorationRenderMesh, Material>();

	private static readonly Dictionary<int, DecorationRenderState[]> _states = new Dictionary<int, DecorationRenderState[]>();

	private NativeArray<CullData> _cullDataArray;

	private NativeArray<int> _tileOccupyArray;

	private JobHandle _cullingDependency;

	private const int TILE_INDEX_COUNT = 1000000;

	private const int MAX_INST_PER_BATCH = 1023;

	private readonly Matrix4x4[] _mInstanceTransform = new Matrix4x4[1023];

	private readonly Dictionary<int, List<WorldSceneDesc.ObjectDesc>> _showObjMap = new Dictionary<int, List<WorldSceneDesc.ObjectDesc>>();

	private readonly List<int> _showLargeObjectIdList = new List<int>();

	private readonly List<int4> _tmpHideRect = new List<int4>(32);

	private int _tmpHideCount;

	private bool _enableInstancing;

	private SeasonType SeasonMapType;

	private SeasonType curSeasonType = SeasonType.Unknown;

	private DecorationInvisibleRectType decorationInvisibleRectType;

	private Vector3 decorationInvisibleRectCenter;

	private float decorationInvisibleRectRadius;

	private Stopwatch stopwatch = new Stopwatch();

	public const int TileCountPerRegion = 500;

	public static int RegionWidth = 6;

	public const int BlockBetweenServerWidth = 4;

	public const int BlockBetweenCenterWidth = 6;

	public const int BlockBetweenCenterWidth_Big = 5;

	public const int BlockBetweenCenterWidth_Small = 2;

	public Dictionary<Vector2Int, Region_S5> regionsMap_S5;

	private Dictionary<string, Asset> mLoadedAssets = new Dictionary<string, Asset>();

	private static readonly Dictionary<int, string> mRenderAssetPathMap = new Dictionary<int, string>();

	private Dictionary<int, string> mTerrainMatPathDic_Height = new Dictionary<int, string>();

	private Dictionary<int, string> mTerrainMatPathDic_Low = new Dictionary<int, string>();

	private Dictionary<int, Asset> mTerrainMatDic_Height = new Dictionary<int, Asset>();

	private Dictionary<int, Asset> mTerrainMatDic_Low = new Dictionary<int, Asset>();

	private Dictionary<Vector2Int, Vector4> mTerrainMatOffsetMap = new Dictionary<Vector2Int, Vector4>();

	private Dictionary<int, Asset> mWorldBlockDescAssetDic_S5 = new Dictionary<int, Asset>();

	private Dictionary<int, HashSet<int>> mBlockHashSet_S5 = new Dictionary<int, HashSet<int>>();

	private Dictionary<int, List<Rect>> mBlockBetweenServer_S5 = new Dictionary<int, List<Rect>>();

	private static readonly List<Rect> mGlobalRailRects = new List<Rect>();

	private InstanceRequest blackBlock_CenterRequest;

	private InstanceRequest worldFogRequest;

	private WorldMapGridRenderer.KingCity[] mKingCity;

	private bool mCurrentShowTerrain = true;

	private bool mJumpUpdateDecoration_S5;

	public Dictionary<Vector2Int, List<WorldDecorationChunkDesc>> mRegionChunksCache = new Dictionary<Vector2Int, List<WorldDecorationChunkDesc>>();

	private const int NorthDoorTileX = 1500;

	private const int NorthDoorTileY = 1993;

	private const int SouthDoorTileX = 1500;

	private const int SouthDoorTileY = 1002;

	private InstanceRequest northDoor;

	private InstanceRequest southDoor;

	private const string NorthDoorPath = "Assets/Main/SeasonRes/S5/Prefabs/World/beimen.prefab";

	private const string SouthDoorPath = "Assets/Main/SeasonRes/S5/Prefabs/World/nanmen.prefab";

	private const int DOOR_RADIUS = 20;

	private const int RailTrackWidth = 3;

	private const int RailTrackLength = 2000;

	private const int RailTrackOriginX = 499;

	private const int RailTrackOriginY = 499;

	private const int RailGridEndX_Inclusive = 2499;

	private const int RailGridEndY_Inclusive = 2499;

	private const int RailGridCenterX_Inclusive = 1499;

	private const int RailGridCenterY_Inclusive = 1499;

	public HashSet<int> ObstaclesSet => obstacles;

	public string TerrainPrefabName => "Assets/Main/Prefabs/World/Terrain_World.prefab";

	public SeasonType CurSeasonType
	{
		get
		{
			if (curSeasonType == SeasonType.Unknown)
			{
				curSeasonType = (SeasonType)GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetCurWorldSeasonType");
			}
			return curSeasonType;
		}
	}

	private static WorldSceneDesc GetWorldSceneDesc(byte[] bytes, int dataType)
	{
		if (_worldSceneDesc != null)
		{
			return _worldSceneDesc;
		}
		_worldSceneDesc = new WorldSceneDesc();
		_cacheDecorationChunkDescMap = _worldSceneDesc.LoadWorldDecoration(bytes, dataType);
		return _worldSceneDesc;
	}

	public WorldStaticManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		curSeasonType = SeasonType.Unknown;
		parentNode = new GameObject("Static").transform;
		parentNode.transform.SetParent(world.Transform, worldPositionStays: false);
		chunkCountPerBlock = world.BlockSize / 25;
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && curSkinMeta.IsNineNationMode())
		{
			mIsNineNationMode = true;
			ObjectExtents = 2;
		}
		else
		{
			mIsNineNationMode = false;
			ObjectExtents = 1;
		}
		InitViewChunk();
		InitWorldBlocks();
		LoadDecoration();
		InitBlackBlock();
		CreateDragonLandRange();
		if (CreateCountPerFrame < 0)
		{
			CreateCountPerFrame = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetWorldStaticObjectCreateCountPerFrame");
		}
		world.AfterUpdate += UpdateView;
		InitTerrainS2(hasPreloadMat: true);
		SetDecorationInvisibleRect(DecorationInvisibleRectType.None, Vector3.zero, 0f);
		LightDataManager.CloseEvColorInDarknessSeason();
		if (curSkinMeta != null)
		{
			SeasonType seasonType = (SeasonType)curSkinMeta.seasonType;
			world.WorldWeatherManager.OnSkinChange(seasonType);
			if (seasonType == SeasonType.Darkness && SystemInfo.supportsInstancing && !GameEntry.Data.Player.IsInBattleField())
			{
				CreateS4FogSystem();
			}
		}
	}

	public override void UnInit()
	{
		foreach (KeyValuePair<Vector2Int, GameObject> terrain in terrains)
		{
			SceneManager.WorldTerrainAssetHandler.Recycle(terrain.Value);
		}
		terrains.Clear();
		world.AfterUpdate -= UpdateView;
		UnloadTerrainAssets();
		UnityEngine.Object.Destroy(parentNode.gameObject);
		parentNode = null;
		worldBlocks = null;
		regionsMap_S5 = null;
		if (worldSceneDescAsset != null)
		{
			worldSceneDescAsset.Release();
			worldSceneDescAsset = null;
		}
		if (WorldBlockDescAsset != null)
		{
			WorldBlockDescAsset.Release();
			WorldBlockDescAsset = null;
		}
		if (DragonLandRangeDescAsset != null)
		{
			DragonLandRangeDescAsset.Release();
			DragonLandRangeDescAsset = null;
		}
		if (DragonLandMapDataAsset != null)
		{
			DragonLandMapDataAsset.Release();
			DragonLandMapDataAsset = null;
		}
		if (s2PreLoadTerrainAsset != null)
		{
			s2PreLoadTerrainAsset.Release();
		}
		foreach (KeyValuePair<string, Asset> item in terrainMatAssets_hight)
		{
			item.Value?.Release();
		}
		terrainMatAssets_hight.Clear();
		foreach (KeyValuePair<string, Asset> item2 in terrainMatAssets_low)
		{
			item2.Value?.Release();
		}
		terrainMatAssets_low.Clear();
		RemoveBlackDesert();
		RemoveDragonLandRange();
		UnloadWorldDecoration();
		UnloadTerrainS2();
		UnloadS4FogSystem();
		if (SceneSkinManager.Instance.GetCurSkinMeta() != null && mIsNineNationMode)
		{
			UnInit_S5();
		}
		mIsNineNationMode = false;
	}

	public override void OnUpdate(float deltaTime)
	{
		_timeSinceLevelLoad = Time.timeSinceLevelLoad;
		if (isLoadFinish)
		{
			UpdateDrawBatch();
		}
	}

	public void RemoveBlackDesert()
	{
		if (blackBlockRequest != null)
		{
			GameObject gameObject = blackBlockRequest.gameObject;
			if (gameObject != null)
			{
				gameObject.GameObjectRecycleAll();
			}
			blackBlockRequest.Destroy();
			blackBlockRequest = null;
		}
		if (blackBlock_CenterRequest != null)
		{
			blackBlock_CenterRequest.Destroy();
			blackBlock_CenterRequest = null;
		}
	}

	public bool IsTileWalkable(Vector2Int tilePos)
	{
		return !IsObstacle(tilePos);
	}

	public bool IsTileWalkable(Vector3 worldPos)
	{
		Vector2Int p = new Vector2Int((int)(worldPos.x / 2f), (int)(worldPos.z / 2f));
		return !IsObstacle(p);
	}

	public void AddOccupyPoints(Vector2Int tilePos, int serverId, Vector2Int size)
	{
		if (serverId > 0 && world.WorldSize > 1000)
		{
			Vector3 vector = TileCoord.TileToWorld(tilePos.x, tilePos.y, serverId);
			tilePos = new Vector2Int((int)(vector.x / 2f), (int)(vector.z / 2f));
		}
		for (int num = tilePos.y; num > tilePos.y - size.y; num--)
		{
			for (int num2 = tilePos.x; num2 > tilePos.x - size.x; num2--)
			{
				Vector2Int vector2Int = new Vector2Int(num2, num);
				if (world.IsInMap(vector2Int))
				{
					if (occupyPoints.TryGetValue(vector2Int, out var value))
					{
						occupyPoints[vector2Int] = value + 1;
					}
					else
					{
						occupyPoints.Add(vector2Int, 1);
					}
				}
			}
		}
		HideObjectInRect(tilePos, size);
		lastViewPos = new Vector3(float.MinValue, float.MinValue, float.MinValue);
	}

	public void RemoveOccupyPoints(Vector2Int tilePos, int serverId, Vector2Int size)
	{
		if (serverId > 0 && world.WorldSize > 1000)
		{
			Vector3 vector = TileCoord.TileToWorld(tilePos.x, tilePos.y, serverId);
			tilePos = new Vector2Int((int)(vector.x / 2f), (int)(vector.z / 2f));
		}
		for (int num = tilePos.y; num > tilePos.y - size.y; num--)
		{
			for (int num2 = tilePos.x; num2 > tilePos.x - size.x; num2--)
			{
				Vector2Int key = new Vector2Int(num2, num);
				if (occupyPoints.TryGetValue(key, out var value))
				{
					value--;
					if (value == 0)
					{
						occupyPoints.Remove(key);
					}
				}
			}
		}
	}

	public bool IsOccupied(Vector2Int p)
	{
		int num = p.x - ObjectExtents;
		int num2 = p.x + ObjectExtents;
		int num3 = p.y - ObjectExtents;
		int num4 = p.y + ObjectExtents;
		for (int i = num3; i <= num4; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				if (occupyPoints.ContainsKey(new Vector2Int(j, i)))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsObstacle(Vector2Int p)
	{
		if (mIsNineNationMode)
		{
			return IsObstacle_S5(p);
		}
		int item = world.TilePosToIndex(p);
		return obstacles.Contains(item);
	}

	public bool IsObstacle(Vector2Int p, int serverIndex)
	{
		int item = world.TilePosToIndex(p);
		return obstacles.Contains(item);
	}

	public void GreenAreaChange(WorldAreaGreenInfo.GreenType type, HashSet<int> changePoints)
	{
		if ((type != WorldAreaGreenInfo.GreenType.Green && type != WorldAreaGreenInfo.GreenType.Desert) || changePoints == null || changePoints.Count <= 0)
		{
			return;
		}
		if (_greenTileChangeList == null)
		{
			_greenTileChangeList = new HashSet<int>();
		}
		foreach (int changePoint in changePoints)
		{
			_greenTileChangeList.Add(changePoint);
		}
	}

	public void UpdateGreenArea(int xMin, int yMin, int xMax, int yMax)
	{
		if (_terrainS2 != null)
		{
			_terrainS2.UpdateBlocks(xMin, yMin, xMax, yMax);
			if (_greenTileChangeList != null && _greenTileChangeList.Count > 0)
			{
				_terrainS2.terrainQuadLayer.SetTransit();
				_greenTileChangeList.Clear();
			}
			SetViewDirty();
		}
	}

	private QuadCellS3 GetS3LayerStateValue(int x, int y)
	{
		int num = world.TilePosToIndex(new Vector2Int(x, y));
		int num2 = (world.IsGreen(num) ? 1 : 0);
		int num3 = (world.IsPointInBlackArea(num) ? 1 : 0);
		float v = ((_greenTileChangeList != null && _greenTileChangeList.Contains(num)) ? _timeSinceLevelLoad : 0f);
		return new QuadCellS3(num2, num3, v);
	}

	private void CheckLoadFinish()
	{
		foreach (Asset terrainAsset in terrainAssets)
		{
			if (!terrainAsset.isDone)
			{
				return;
			}
		}
		if (SceneManager.WorldTerrainAssetHandler.IsAssetLoadFinish() && (!NeedPreLoadS2Terrain() || s2PreLoadTerrainAsset == null || s2PreLoadTerrainAsset.isDone))
		{
			isLoadFinish = true;
		}
	}

	private void HideObjectInRect(Vector2Int p, Vector2Int size)
	{
		int num = p.x - (size.x - 1);
		int z = num + (size.x - 1);
		int num2 = p.y - (size.y - 1);
		int w = num2 + (size.y - 1);
		_tmpHideRect.Add(new int4(num, num2, z, w));
		_tmpHideCount++;
	}

	private void InitViewChunk()
	{
		lastViewChunk = new Vector2Int(int.MinValue, int.MinValue);
		lastViewPos = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		lastViewLevel = int.MinValue;
		lastViewRange = GetVisibleChunkRange();
	}

	public void SetViewDirty()
	{
		lastViewPos = new Vector3(float.MinValue, float.MinValue, float.MinValue);
	}

	public bool NeedPreLoadS2Terrain()
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta == null)
		{
			return false;
		}
		SeasonType seasonType = (SeasonType)curSkinMeta.seasonType;
		if (SystemInfo.supportsInstancing && curSkinMeta.world_terrain_mode == 1)
		{
			if (seasonType != SeasonType.Snow)
			{
				return seasonType == SeasonType.Mummy;
			}
			return true;
		}
		return false;
	}

	public void SetS2TerrainPreLoadAsset(string path, Asset matAsset)
	{
		if (_terrainS2 != null)
		{
			_terrainS2.SetPreLoadMaterial(path, matAsset);
		}
	}

	public void SetS2TerrainPreLoadAssetLoadFinish(string path, Asset matAsset)
	{
		if (_terrainS2 != null)
		{
			_terrainS2.SetPreLoadMaterialLoadFinish(matAsset);
		}
	}

	public void LoadTerrainAssets(Action callback)
	{
		loadTerrainCallback = callback;
		SceneSkinMeta meta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (NeedPreLoadS2Terrain())
		{
			Asset request = GameEntry.Resource.LoadAssetAsync(meta.world_terrain_mode_mat, typeof(Material));
			SetS2TerrainPreLoadAsset(meta.world_terrain_mode_mat, request);
			Asset asset = request;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				SetS2TerrainPreLoadAssetLoadFinish(meta.world_terrain_mode_mat, request);
				CheckLoadFinish();
				UpdateView();
				if (isLoadFinish && loadTerrainCallback != null)
				{
					loadTerrainCallback?.Invoke();
					loadTerrainCallback = null;
				}
			});
			s2PreLoadTerrainAsset = request;
		}
		SceneManager.WorldTerrainAssetHandler.LoadTerrain(TerrainPrefabName, delegate
		{
			CheckLoadFinish();
			UpdateView();
			if (isLoadFinish && loadTerrainCallback != null)
			{
				loadTerrainCallback?.Invoke();
				loadTerrainCallback = null;
			}
		});
	}

	private void UnloadTerrainAssets()
	{
		foreach (Asset terrainAsset in terrainAssets)
		{
			terrainAsset.Release();
		}
		terrainAssets.Clear();
	}

	public void OnSkinChange()
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta == null || curSkinMeta.id == lastSkinId)
		{
			return;
		}
		isLoadFinish = false;
		_showObjMap.Clear();
		if (mIsNineNationMode)
		{
			UnInit_S5();
		}
		if (curSkinMeta.IsNineNationMode())
		{
			mIsNineNationMode = true;
		}
		else
		{
			mIsNineNationMode = false;
		}
		SeasonType seasonType = (SeasonMapType = (SeasonType)curSkinMeta.seasonType);
		LoadDecoration();
		InitWorldBlocks();
		InitBlackBlock();
		isLoadFinish = true;
		lastSkinId = curSkinMeta.id;
		OnSkinChangeTerrainS2();
		world.WorldWeatherManager.OnSkinChange(seasonType);
		if (seasonType == SeasonType.Darkness && SystemInfo.supportsInstancing)
		{
			if (!GameEntry.Data.Player.IsInBattleField())
			{
				CreateS4FogSystem();
			}
			else
			{
				UnloadS4FogSystem();
			}
		}
		else
		{
			UnloadS4FogSystem();
		}
		__UpdateView(force: true);
	}

	private void InitWorldBlocks()
	{
		if (mIsNineNationMode)
		{
			InitWorldBlock_S5();
		}
		else
		{
			InitWorldBlock();
		}
	}

	private void InitWorldDecoration()
	{
		string assetPath = "Assets/Main/Scenes/NewWorldSceneDesc.bytes";
		SceneSkinMeta meta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (meta != null && !meta.world_deco_byte.IsNullOrEmpty())
		{
			assetPath = meta.world_deco_byte;
			lastSkinId = meta.id;
			if (!GameEntry.Resource.HasAsset(assetPath))
			{
				assetPath = assetPath.Replace("Assets/Main/Scenes/", "Assets/Main/Scenes/SeasonRes/");
			}
		}
		if (assetPath.Equals(lastWorldSceneDescPath))
		{
			return;
		}
		lastWorldSceneDescPath = assetPath;
		if (_worldSceneDesc != null)
		{
			_worldSceneDesc = null;
		}
		blockTemplates = new List<Block>();
		worldBlocks = new Block[1];
		if (worldSceneDescAsset != null)
		{
			worldSceneDescAsset.Release();
			worldSceneDescAsset = null;
		}
		worldSceneDescAsset = GameEntry.Resource.LoadAssetAsync(assetPath, typeof(TextAsset));
		Asset asset = worldSceneDescAsset;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (parentNode == null || SceneManager.CurrSceneID != 2 || worldSceneDescAsset == null)
			{
				worldSceneDescAsset?.Release();
				worldSceneDescAsset = null;
			}
			else if (world == null || world.gameObject == null)
			{
				worldSceneDescAsset?.Release();
				worldSceneDescAsset = null;
			}
			else
			{
				TextAsset textAsset = worldSceneDescAsset.Get<TextAsset>();
				if (textAsset == null)
				{
					worldSceneDescAsset?.Release();
					worldSceneDescAsset = null;
				}
				else
				{
					SceneSkinMeta curSkinMeta2 = SceneSkinManager.Instance.GetCurSkinMeta();
					if (curSkinMeta2 != null && curSkinMeta2.world_deco_byte != null && assetPath != curSkinMeta2.world_deco_byte)
					{
						worldSceneDescAsset?.Release();
						worldSceneDescAsset = null;
					}
					else
					{
						Block block = new Block();
						if (meta != null && meta.edge_performance == 1)
						{
							block.Init(textAsset.bytes, this, 2);
						}
						else
						{
							block.Init(textAsset.bytes, this, 1);
						}
						blockTemplates.Add(block);
						worldSceneDescAsset.Release();
						worldSceneDescAsset = null;
						GameEntry.Lua.CallWithReturn<int, string, string>("CSharpCallLuaInterface.GetConfigNum", "player_birth_rectangle", "k3");
						_ = 0;
						worldBlocks[0] = block;
						lastViewChunk = new Vector2Int(int.MinValue, int.MinValue);
					}
				}
			}
		});
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			SeasonMapType = (SeasonType)curSkinMeta.mapType;
		}
	}

	private void InitWorldBlock()
	{
		string text = "Assets/Main/Scenes/WorldBlockDesc.bytes";
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && !curSkinMeta.world_block.IsNullOrEmpty())
		{
			text = curSkinMeta.world_block;
			lastSkinId = curSkinMeta.id;
			if (!GameEntry.Resource.HasAsset(text))
			{
				text = text.Replace("Assets/Main/Scenes/", "Assets/Main/Scenes/SeasonRes/");
			}
		}
		if (text.Equals(lastWorldBlockDescPath))
		{
			return;
		}
		lastWorldBlockDescPath = text;
		if (WorldBlockDescAsset != null)
		{
			WorldBlockDescAsset.Release();
			WorldBlockDescAsset = null;
		}
		WorldBlockDescAsset = GameEntry.Resource.LoadAssetAsync(text, typeof(TextAsset));
		Asset worldBlockDescAsset = WorldBlockDescAsset;
		worldBlockDescAsset.completed = (Action<Asset>)Delegate.Combine(worldBlockDescAsset.completed, (Action<Asset>)delegate
		{
			if (WorldBlockDescAsset != null && !WorldBlockDescAsset.isError)
			{
				WorldBlockDesc worldBlockDesc = new WorldBlockDesc();
				TextAsset textAsset = WorldBlockDescAsset.Get<TextAsset>();
				if (textAsset != null)
				{
					worldBlockDesc.Load(textAsset.bytes);
				}
				obstacles.Clear();
				for (int i = 0; i < worldBlockDesc.Tiles.Count; i++)
				{
					obstacles.Add(world.TilePosToIndex(worldBlockDesc.Tiles[i]));
				}
				WorldBlockDescAsset.Release();
				WorldBlockDescAsset = null;
			}
		});
	}

	public void InitBlackBlock()
	{
		if (!GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.GetCanShowBlackLand"))
		{
			return;
		}
		string text = "Assets/Main/Prefabs/World/BlackBlock.prefab";
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			if (!curSkinMeta.world_terrain_black.IsNullOrEmpty())
			{
				text = curSkinMeta.world_terrain_black;
			}
			lastSkinId = curSkinMeta.id;
			if (curSkinMeta.IsNineNationMode())
			{
				InitBlackBlock_S5();
				return;
			}
		}
		if (text.Equals(lastWorldBlackBlockPath))
		{
			return;
		}
		lastWorldBlackBlockPath = text;
		if (blackBlockRequest != null)
		{
			GameObject gameObject = blackBlockRequest.gameObject;
			if (gameObject != null)
			{
				gameObject.GameObjectRecycleAll();
			}
			blackBlockRequest.Destroy();
			blackBlockRequest = null;
		}
		if (blackBlockRequest != null)
		{
			return;
		}
		blackBlockRequest = GameEntry.Resource.InstantiateAsync(text);
		blackBlockRequest.completed += delegate
		{
			GameObject gameObject2 = blackBlockRequest.gameObject;
			if (gameObject2 == null)
			{
				Log.Error("gameObject null");
			}
			else
			{
				Vector3 position = new Vector3(1000f, 0f, 1000f);
				gameObject2.transform.position = position;
			}
		};
	}

	public GameObject GetDragonLandRangeObj()
	{
		if (dragonBlockRequest == null)
		{
			return null;
		}
		return dragonBlockRequest.gameObject;
	}

	public void CreateDragonLandRange()
	{
		if (GameEntry.Data.Player.GetWorldId() <= 0 || dragonBlockRequest != null)
		{
			return;
		}
		string[] array = GameEntry.Lua.CallWithReturn<string[]>("CSharpCallLuaInterface.GetSpecialWorldCreateInfo");
		string text = array[0];
		string bytesStr = array[1];
		string mapDataStr = array[2];
		if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(bytesStr) || string.IsNullOrEmpty(mapDataStr))
		{
			return;
		}
		dragonBlockRequest = GameEntry.Resource.InstantiateAsync(text);
		dragonBlockRequest.completed += delegate
		{
			if (dragonBlockRequest.gameObject == null)
			{
				Log.Error("gameObject null");
			}
			else
			{
				dragonBlockRequest.gameObject.transform.position = new Vector3(1000f, 0f, 1000f);
				GameEntry.Lua.Call("CSharpCallLuaInterface.OnSpecialWorldCreate");
				dragonDecorateRoot = dragonBlockRequest.gameObject.transform.Find("DecorateRoot");
				if (DragonLandRangeDescAsset != null)
				{
					DragonLandRangeDescAsset.Release();
					DragonLandRangeDescAsset = null;
				}
				if (DragonLandMapDataAsset != null)
				{
					DragonLandMapDataAsset.Release();
					DragonLandMapDataAsset = null;
				}
				if (dragonDecorateRoot != null)
				{
					DragonLandRangeDescAsset = GameEntry.Resource.LoadAssetAsync(bytesStr, typeof(TextAsset));
					Asset dragonLandRangeDescAsset = DragonLandRangeDescAsset;
					dragonLandRangeDescAsset.completed = (Action<Asset>)Delegate.Combine(dragonLandRangeDescAsset.completed, (Action<Asset>)delegate
					{
						dragonBlockDataList = DragonLandRangeDesc.Load(DragonLandRangeDescAsset.Get<TextAsset>().bytes);
						DragonLandRangeDescAsset.Release();
						DragonLandRangeDescAsset = null;
						if (dragonMapData != null)
						{
							__UpdateView(force: true);
						}
					});
					DragonLandMapDataAsset = GameEntry.Resource.LoadAssetAsync(mapDataStr, typeof(MapData));
					Asset dragonLandMapDataAsset = DragonLandMapDataAsset;
					dragonLandMapDataAsset.completed = (Action<Asset>)Delegate.Combine(dragonLandMapDataAsset.completed, (Action<Asset>)delegate
					{
						dragonMapData = DragonLandMapDataAsset.Get<MapData>();
						if (dragonMapData != null)
						{
							if (dragonDelayList != null && dragonDelayList.Count > 0)
							{
								foreach (KeyValuePair<DragonLandItem, int> dragonDelay in dragonDelayList)
								{
									InitDragonWarDesertData(dragonDelay.Key, dragonDecorateRoot);
								}
								dragonDelayList.Clear();
							}
							else if (dragonBlockDataList != null)
							{
								__UpdateView(force: true);
							}
						}
					});
				}
			}
		};
	}

	private void InitDragonWarDesertData(DragonLandItem item, Transform DecorateRoot)
	{
		if (item == null || item.assetPath.IsNullOrEmpty() || item.go != null)
		{
			return;
		}
		if (dragonMapData != null)
		{
			GameObject gameObject = dragonMapData.TryGetGameObject(item.assetPath);
			if (!(gameObject != null))
			{
				return;
			}
			gameObject.transform.SetParent(DecorateRoot);
			gameObject.transform.localScale = item.localScale;
			gameObject.transform.localPosition = item.localPosition;
			gameObject.transform.localRotation = item.localRotation;
			gameObject.SetActive(value: true);
			if (item.rangeList != null && item.rangeList.Count > 0)
			{
				List<GameObject> list = null;
				foreach (int range in item.rangeList)
				{
					list = null;
					dictDragonWarDesertData.TryGetValue(range, out list);
					if (list == null)
					{
						list = new List<GameObject>();
						list.Add(gameObject);
						dictDragonWarDesertData.Add(range, list);
					}
					else
					{
						list.Add(gameObject);
					}
				}
			}
			item.go = gameObject;
		}
		else if (!dragonDelayList.ContainsKey(item))
		{
			dragonDelayList.Add(item, 1);
		}
	}

	public void RemoveDragonLandRange()
	{
		if (DragonLandRangeDescAsset != null)
		{
			DragonLandRangeDescAsset.Release();
			DragonLandRangeDescAsset = null;
		}
		if (DragonLandMapDataAsset != null)
		{
			DragonLandMapDataAsset.Release();
			DragonLandMapDataAsset = null;
		}
		if (dragonBlockDataList != null)
		{
			foreach (DragonLandItem dragonBlockData in dragonBlockDataList)
			{
				if (dragonBlockData != null && dragonBlockData.go != null)
				{
					dragonBlockData.go.Destroy();
					dragonBlockData.go = null;
				}
			}
			dragonBlockDataList.Clear();
			dragonBlockDataList = null;
		}
		if (dragonBlockRequest != null)
		{
			dictDragonWarDesertData.Clear();
			dragonBlockRequest.Destroy();
			dragonBlockRequest = null;
		}
		dragonMapData = null;
	}

	public void RemoveDragonLandPoint(int pointIndex)
	{
		if (dictDragonWarDesertData != null && dictDragonWarDesertData.Count > 0)
		{
			List<GameObject> value = null;
			Vector2Int vector2Int = world.IndexToTilePos(pointIndex);
			int num = 2;
			Vector2Int tileCount = new Vector2Int(1000, 1000);
			for (int num2 = vector2Int.y + num; num2 > vector2Int.y - num; num2--)
			{
				for (int num3 = vector2Int.x + num; num3 > vector2Int.x - num; num3--)
				{
					int key = TileCoord.TilePosToIndex(new Vector2Int(num3, num2), tileCount);
					dictDragonWarDesertData.TryGetValue(key, out value);
					if (value != null)
					{
						foreach (GameObject item in value)
						{
							if (item != null)
							{
								item.SetActive(value: false);
							}
						}
					}
				}
			}
		}
		else
		{
			if (!GameEntry.Data.Player.IsInBattleField(2) || dragonBlockDataList == null || dragonBlockDataList.Count <= 0)
			{
				return;
			}
			GameObject gameObject = null;
			Vector3 vector = world.TileIndexToWorld(pointIndex);
			int num4 = 4;
			float num5 = vector.x - (float)num4;
			float num6 = vector.x + (float)num4;
			float num7 = vector.z - (float)num4;
			float num8 = vector.z + (float)num4;
			foreach (DragonLandItem dragonBlockData in dragonBlockDataList)
			{
				gameObject = dragonBlockData.go;
				if (!(gameObject == null))
				{
					float x = dragonBlockData.position.x;
					float z = dragonBlockData.position.z;
					if (x >= num5 && x <= num6 && z >= num7 && z <= num8)
					{
						gameObject.SetActive(value: false);
					}
				}
			}
		}
	}

	private string GetTerrainMatPath()
	{
		bool flag = UseTerrainMatHeight();
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			if (!string.IsNullOrEmpty(curSkinMeta.world_terrain_low))
			{
				if (flag)
				{
					return curSkinMeta.world_terrain;
				}
				return curSkinMeta.world_terrain_low;
			}
			return curSkinMeta.world_terrain;
		}
		return string.Empty;
	}

	public void ChangeTerrainMat()
	{
		bool flag = false;
		bool flag2 = UseTerrainMatHeight();
		if (flag2 == mCurrentTerrain_HightQuality || 1 == 0)
		{
			return;
		}
		if (mIsNineNationMode)
		{
			if (terrains == null)
			{
				return;
			}
			{
				foreach (KeyValuePair<Vector2Int, GameObject> terrain in terrains)
				{
					Vector2Int key = terrain.Key;
					MeshRenderer[] componentsInChildren = terrain.Value.transform.GetComponentsInChildren<MeshRenderer>();
					if (componentsInChildren != null && componentsInChildren.Length != 0)
					{
						SetTerrainMaterial_S5(componentsInChildren, key);
					}
				}
				return;
			}
		}
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta == null || curSkinMeta.world_terrain.IsNullOrEmpty() || string.IsNullOrEmpty(curSkinMeta.world_terrain_low))
		{
			return;
		}
		Asset asset = null;
		string terrainMatPath = GetTerrainMatPath();
		if (flag2)
		{
			if (!terrainMatAssets_hight.ContainsKey(terrainMatPath))
			{
				Asset value = GameEntry.Resource.LoadAsset(terrainMatPath, typeof(Material));
				terrainMatAssets_hight[terrainMatPath] = value;
			}
			asset = terrainMatAssets_hight[terrainMatPath];
			mCurrentTerrain_HightQuality = true;
		}
		else
		{
			if (!terrainMatAssets_low.ContainsKey(terrainMatPath))
			{
				Asset value2 = GameEntry.Resource.LoadAsset(terrainMatPath, typeof(Material));
				terrainMatAssets_low[terrainMatPath] = value2;
			}
			asset = terrainMatAssets_low[terrainMatPath];
			mCurrentTerrain_HightQuality = false;
		}
		if (terrains == null)
		{
			return;
		}
		foreach (KeyValuePair<Vector2Int, GameObject> terrain2 in terrains)
		{
			Vector2Int key2 = terrain2.Key;
			MeshRenderer[] componentsInChildren2 = terrain2.Value.transform.GetComponentsInChildren<MeshRenderer>();
			if (componentsInChildren2 != null && componentsInChildren2.Length != 0 && asset != null && asset.asset != null)
			{
				Material sharedMaterial = (Material)asset.asset;
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].sharedMaterial = sharedMaterial;
					SetTerrainMaterial(componentsInChildren2[i], curSkinMeta, key2);
				}
			}
		}
	}

	public void ChangeTerrain()
	{
		ChangeTerrainMat();
		InitViewChunk();
		UpdateView();
		if (_useNewTerrainQuad || _fogRenderer != null)
		{
			YieldUtils.DoEndOfFrame(world, OnQuailtyChange);
		}
	}

	private void OnQuailtyChange()
	{
		if (_useNewTerrainQuad && _terrainS2Renderer != null)
		{
			_terrainS2Renderer.AttachToRenderer(world.Camera.__camera);
		}
		if (_fogRenderer != null)
		{
			_fogRenderer.AttachToRenderer(world.Camera.__camera);
		}
	}

	private Vector2Int TilePosToChunkCoord(Vector2Int tilePos)
	{
		return new Vector2Int(tilePos.x / 25, tilePos.y / 25);
	}

	private Vector2Int TilePos2ChunkCoord_Floor(Vector2 tilePos)
	{
		int x = Mathf.FloorToInt(tilePos.x / 25f);
		int y = Mathf.FloorToInt(tilePos.y / 25f);
		return new Vector2Int(x, y);
	}

	private Vector2Int TilePosToBlockCoord(Vector2Int tilePos)
	{
		return new Vector2Int(tilePos.x / world.BlockSize, tilePos.y / world.BlockSize);
	}

	private Vector2Int ChunkCoordToBlockCoord(Vector2Int chunkCoord)
	{
		if (chunkCoord.x < 0)
		{
			chunkCoord.x -= chunkCountPerBlock;
		}
		if (chunkCoord.y < 0)
		{
			chunkCoord.y -= chunkCountPerBlock;
		}
		return new Vector2Int(chunkCoord.x / chunkCountPerBlock, chunkCoord.y / chunkCountPerBlock);
	}

	public Vector2Int ChunkCoordToTilePos(Vector2Int chunkCoord)
	{
		return new Vector2Int(chunkCoord.x * 25, chunkCoord.y * 25);
	}

	private void GetChunkObjList(Vector2Int chunkCoord, List<WorldSceneDesc.ObjectDesc> list)
	{
		list.Clear();
		if (GameEntry.Data.Player.IsInBattleField())
		{
			return;
		}
		Vector2Int tilePos = ChunkCoordToTilePos(chunkCoord);
		Block block = GetBlock(tilePos);
		if (block != null)
		{
			List<WorldSceneDesc.ObjectDesc> list2 = block.GetChunkObjList(chunkCoord);
			if (list2 != null)
			{
				list.AddRange(list2);
			}
		}
	}

	private Block GetBlock(Vector2Int tilePos)
	{
		return worldBlocks[0];
	}

	private int GetVisibleChunkRange()
	{
		float lodDistance = world.GetLodDistance();
		int i;
		for (i = 1; lodDistance >= (float)(i * 350); i++)
		{
		}
		return i;
	}

	private void UpdateXYRange()
	{
		WorldCamera camera = world.Camera;
		if (camera == null)
		{
			minXY.Set(0, 0);
			maxXY.Set(0, 0);
			cameraAnchorLeftBottom.Set(0f, 0f, 0f);
			cameraAnchorRightTop.Set(0f, 0f, 0f);
		}
		else
		{
			cameraAnchorLeftBottom = camera.cameraAnchor[0] + new Vector3(-3f, 0f, -3f);
			cameraAnchorRightTop = camera.cameraAnchor[2] + new Vector3(3f, 0f, 3f);
			Vector2 tilePos = new Vector2(cameraAnchorLeftBottom.x / 2f, cameraAnchorLeftBottom.z / 2f);
			Vector2 tilePos2 = new Vector2(cameraAnchorRightTop.x / 2f, cameraAnchorRightTop.z / 2f);
			minXY = TilePos2ChunkCoord_Floor(tilePos);
			maxXY = TilePos2ChunkCoord_Floor(tilePos2);
		}
	}

	private void UpdateView()
	{
		__UpdateView();
	}

	private void __UpdateView(bool force = false)
	{
		if (!isLoadFinish)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		Vector3 curTarget = world.CurTarget;
		Vector2Int tilePos = new Vector2Int((int)(curTarget.x / 2f), (int)(curTarget.z / 2f));
		Vector2Int vector2Int = TilePosToChunkCoord(tilePos);
		int lodLevel = world.GetLodLevel();
		int visibleChunkRange = GetVisibleChunkRange();
		Vector3 cameraPos = world.GetCameraPos();
		UpdateXYRange();
		float lodDistance = world.GetLodDistance();
		if (Mathf.Abs(_lastZoom - lodDistance) > 50f)
		{
			_lastZoom = lodDistance;
			flag4 = true;
		}
		if (lastViewChunk != vector2Int)
		{
			lastViewChunk = vector2Int;
			flag = true;
		}
		if (lastViewPos != cameraPos)
		{
			lastViewPos = cameraPos;
			flag2 = true;
		}
		if (lastViewLevel != lodLevel || lastViewRange != visibleChunkRange)
		{
			lastViewLevel = lodLevel;
			lastViewRange = visibleChunkRange;
			flag3 = true;
		}
		if (lastMinXY != minXY || lastMaxXY != maxXY)
		{
			lastMinXY = minXY;
			lastMaxXY = maxXY;
			flag5 = true;
		}
		flag4 = flag4 || force;
		flag = flag || force;
		flag2 = flag2 || force;
		flag3 = flag3 || force;
		flag5 = flag5 || force;
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		SeasonType seasonType = curSkinMeta?.GetMapType() ?? SeasonType.Nothing;
		if (lodLevel < 6 && world.hasReceiveViewPointsReply && (flag2 || flag3 || flag4))
		{
			WorldCamera camera = world.Camera;
			Vector3 vector = camera.cameraAnchor[0] + new Vector3(-3f, 0f, -3f);
			Vector3 vector2 = camera.cameraAnchor[2] + new Vector3(3f, 0f, 3f);
			Vector2Int tilePos2 = new Vector2Int((int)(vector.x / 2f), (int)(vector.z / 2f));
			Vector2Int tilePos3 = new Vector2Int((int)(vector2.x / 2f), (int)(vector2.z / 2f));
			Vector2Int vector2Int2 = TilePosToChunkCoord(tilePos2);
			Vector2Int vector2Int3 = TilePosToChunkCoord(tilePos3);
			int x = vector2Int2.x;
			int x2 = vector2Int3.x;
			int y = vector2Int2.y;
			int y2 = vector2Int3.y;
			if (dragonBlockDataList != null && dragonDecorateRoot != null)
			{
				Rect rect = new Rect(vector.x, vector.z, vector2.x - vector.x, vector2.z - vector.z);
				Vector2 point = default(Vector2);
				foreach (DragonLandItem dragonBlockData in dragonBlockDataList)
				{
					if (dragonBlockData != null && dragonBlockData.go == null)
					{
						bool flag6 = false;
						point.x = dragonBlockData.position.x;
						point.y = dragonBlockData.position.z;
						if (rect.Contains(point))
						{
							flag6 = true;
						}
						else if (dragonBlockData.rect != Rect.zero)
						{
							flag6 = rect.Overlaps(dragonBlockData.rect);
						}
						if (flag6)
						{
							InitDragonWarDesertData(dragonBlockData, dragonDecorateRoot);
						}
					}
				}
			}
			ClearShowObjMap();
			_showLargeObjectIdList.Clear();
			for (int i = y; i <= y2; i++)
			{
				for (int j = x; j <= x2; j++)
				{
					Vector2Int chunkCoord = new Vector2Int(j, i);
					if (seasonType == SeasonType.NineNation)
					{
						GetChunkObjList_S5(chunkCoord, chunkObjList, flag3, flag4);
					}
					else if (worldBlocks != null)
					{
						GetChunkObjList(chunkCoord, chunkObjList);
					}
					if (chunkObjList.Count <= 0)
					{
						continue;
					}
					for (int k = 0; k < chunkObjList.Count; k++)
					{
						WorldSceneDesc.ObjectDesc objectDesc = chunkObjList[k];
						Vector2Int vector2Int4 = Vector2Int.zero;
						if (objectDesc.type == 1)
						{
							vector2Int4 = new Vector2Int((int)(objectDesc.localPos.x / 2f), (int)(objectDesc.localPos.z / 2f));
						}
						else if (objectDesc.type == 0 || objectDesc.type == 2)
						{
							vector2Int4 = objectDesc.worldDecorationTilePos;
						}
						if (objectDesc.type == 2 || objectDesc.type == 3)
						{
							if (!_showLargeObjectIdList.Contains(objectDesc.id))
							{
								AddShowObj(objectDesc);
								_showLargeObjectIdList.Add(objectDesc.id);
							}
						}
						else if (world.IsInMap(vector2Int4) && (objectDesc.type == 1 || !IsOccupied(vector2Int4) || IsObstacle(vector2Int4)))
						{
							AddShowObj(objectDesc);
						}
					}
				}
			}
		}
		if (_useNewTerrainQuad)
		{
			switch (lodLevel)
			{
			case 6:
			case 7:
			case 8:
				_terrainS2Renderer.SetActive(active: false);
				break;
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			{
				_terrainS2Renderer.SetActive(active: true);
				_terrainS2.SetLodLevel(lodLevel);
				WorldCamera camera2 = world.Camera;
				int num = _terrainS2.RectBias();
				Vector3 vector3 = camera2.cameraAnchor[0] + new Vector3(-num, 0f, -num);
				Vector3 vector4 = camera2.cameraAnchor[2] + new Vector3(num, 0f, num);
				Vector2Int lb = new Vector2Int((int)(vector3.x / 2f), (int)(vector3.z / 2f));
				Vector2Int rt = new Vector2Int((int)(vector4.x / 2f), (int)(vector4.z / 2f));
				if (_terrainS2.BlockChanged(lb, rt) || flag3 || flag4)
				{
					_terrainS2.UpdateView(lb, rt);
				}
				break;
			}
			}
		}
		if (lodLevel >= 1 && lodLevel <= 8 && (flag || flag3 || flag4 || flag5))
		{
			foreach (KeyValuePair<Vector2Int, GameObject> terrain in terrains)
			{
				SceneManager.WorldTerrainAssetHandler.Recycle(terrain.Value);
			}
			terrains.Clear();
			if (!_useNewTerrainQuad)
			{
				if (seasonType == SeasonType.NineNation)
				{
					UpdateTerrainObject_S5(world.Camera);
				}
				else
				{
					UpdateTerrainQuadObject(vector2Int, curSkinMeta);
				}
			}
		}
		UpdateWeather(flag2, lodLevel);
		if (!(seasonType == SeasonType.Darkness && flag2) || lodLevel > 8)
		{
			return;
		}
		if (lodLevel >= 6)
		{
			if (_fogRenderer != null)
			{
				_fogRenderer.SetActive(active: false);
			}
			world.mWorldFogManager.HideWorldFogObj(hide: true);
			return;
		}
		if (world.mWorldFogManager.NeedUpdateAllFog(world.Camera))
		{
			world.mWorldFogManager.UpdateAllFogOnViewChange(world.Camera);
		}
		if (_fogRenderer != null)
		{
			_fogRenderer.SetActive(active: true);
		}
		world.mWorldFogManager.HideWorldFogObj(hide: false);
	}

	private void UpdateTerrainQuadObject(Vector2Int viewChunkCoord, SceneSkinMeta skinMeta)
	{
		for (int i = minXY.y; i <= maxXY.y; i++)
		{
			for (int j = minXY.x; j <= maxXY.x; j++)
			{
				Vector2Int chunkCoord = new Vector2Int(j, i);
				Vector2Int vector2Int = ChunkCoordToBlockCoord(chunkCoord);
				if (terrains.ContainsKey(vector2Int))
				{
					continue;
				}
				GameObject gameObject = SceneManager.WorldTerrainAssetHandler.Spawn();
				if (gameObject == null)
				{
					continue;
				}
				if (skinMeta != null && !skinMeta.world_terrain.IsNullOrEmpty())
				{
					MeshRenderer[] componentsInChildren = gameObject.transform.GetComponentsInChildren<MeshRenderer>();
					if (componentsInChildren != null && componentsInChildren.Length != 0)
					{
						string terrainMatPath = GetTerrainMatPath();
						if (!string.IsNullOrEmpty(terrainMatPath))
						{
							bool num = UseTerrainMatHeight();
							Asset asset = null;
							if (num)
							{
								if (!terrainMatAssets_hight.ContainsKey(terrainMatPath))
								{
									Asset asset2 = GameEntry.Resource.LoadAsset(terrainMatPath, typeof(Material));
									terrainMatAssets_hight[terrainMatPath] = asset2;
									asset = asset2;
								}
								else
								{
									asset = terrainMatAssets_hight[terrainMatPath];
								}
								mCurrentTerrain_HightQuality = true;
							}
							else
							{
								if (!terrainMatAssets_low.ContainsKey(terrainMatPath))
								{
									Asset asset3 = GameEntry.Resource.LoadAsset(terrainMatPath, typeof(Material));
									terrainMatAssets_low[terrainMatPath] = asset3;
									asset = asset3;
								}
								else
								{
									asset = terrainMatAssets_low[terrainMatPath];
								}
								mCurrentTerrain_HightQuality = false;
							}
							if (asset != null && asset.asset != null)
							{
								Material sharedMaterial = (Material)asset.asset;
								for (int k = 0; k < componentsInChildren.Length; k++)
								{
									componentsInChildren[k].sharedMaterial = sharedMaterial;
									SetTerrainMaterial(componentsInChildren[k], skinMeta, vector2Int);
								}
							}
						}
					}
				}
				gameObject.transform.position = TileCoord.TileFloatToWorld(vector2Int.x * world.BlockSize, vector2Int.y * world.BlockSize, 0);
				terrains.Add(vector2Int, gameObject);
			}
		}
	}

	private void UpdateWeather(bool viewPosChanged = false, int currViewLevel = 0)
	{
		if (!(world == null) && world.WorldWeatherManager != null && viewPosChanged && currViewLevel <= 8)
		{
			world.WorldWeatherManager.UpdateWeather(currViewLevel, 6, 5);
		}
	}

	private void SetTerrainMaterial(Renderer renderer, SceneSkinMeta meta, Vector2Int blockCoord)
	{
		List<Vector4> worldTerrainOffset = meta.GetWorldTerrainOffset();
		if (worldTerrainOffset != null && worldTerrainOffset.Count != 0)
		{
			Vector4 vector = ((blockCoord.x < 0 || blockCoord.x > 1 || blockCoord.y < 0 || blockCoord.y > 1) ? worldTerrainOffset[0] : ((blockCoord.x > 0) ? ((blockCoord.y <= 0) ? worldTerrainOffset[2] : worldTerrainOffset[4]) : ((blockCoord.y <= 0) ? worldTerrainOffset[1] : worldTerrainOffset[3])));
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			renderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetVector("_ControlTex_ST", new Vector4(vector.x, vector.y, vector.z, vector.w));
			renderer.SetPropertyBlock(materialPropertyBlock);
		}
	}

	public void OnDrawGizmosTerrainS2()
	{
		_terrainS2?.DrawGizmos();
	}

	public void OnDrawGizmos()
	{
	}

	public bool GetProfileTerrainSwitch()
	{
		return profileTerrainSwitch;
	}

	public void ProfileToggleTerrain()
	{
		profileTerrainSwitch = !profileTerrainSwitch;
		foreach (GameObject value in terrains.Values)
		{
			value.SetLayerRecursively(LayerMask.NameToLayer(profileTerrainSwitch ? "Default" : "Hide"));
		}
	}

	public bool GetProfileSwitch()
	{
		return profileSwitch;
	}

	private void CreateTerrainS2(SeasonType seasonMapType)
	{
		_terrainS2 = new TerrainS2();
		switch (seasonMapType)
		{
		case SeasonType.Snow:
		{
			TerrainQuadS2 terrainQuadS2 = new TerrainQuadS2();
			_terrainS2.Init(terrainQuadS2);
			HeatSourceDataManager.GetInstance().SetOnHeatSourceChanged(_terrainS2.UpdateBlocks);
			terrainQuadS2.SetGetStateFunc(HeatSourceDataManager.GetInstance().GetTerrainStateByXY);
			break;
		}
		case SeasonType.Mummy:
		{
			TerrainQuadS3 terrainQuadS = new TerrainQuadS3();
			_terrainS2.Init(terrainQuadS);
			terrainQuadS.SetGetStateFunc(GetS3LayerStateValue);
			break;
		}
		}
		_terrainS2Renderer = ScriptableObject.CreateInstance<TerrainQuadLayerRenderer>();
		_terrainS2Renderer.AttachToRenderer(world.Camera.__camera);
		_terrainS2Renderer.SetActive(active: false);
		_terrainS2Renderer.SetTerrainQuadLayer(_terrainS2.terrainQuadLayer);
	}

	public void CreateS4FogSystem()
	{
		if (_fogRenderer != null)
		{
			_fogRenderer.SetActive(active: false);
			_fogRenderer.DetachFromRenderer(world.Camera.__camera);
			_fogRenderer = null;
		}
		_fogRenderer = ScriptableObject.CreateInstance<WorldFogRendererRenderer>();
		_fogRenderer.AttachToRenderer(world.Camera.__camera);
		_fogRenderer.SetActive(active: false);
		WorldFogInstanceRenderer worldFogInstanceRenderer = new WorldFogInstanceRenderer();
		_fogRenderer.SetFogInstanceRender(worldFogInstanceRenderer);
		world.mWorldFogManager.InitWorldFog(_fogRenderer, worldFogInstanceRenderer, world.Camera);
		if (_shakeDetection == null)
		{
			_shakeDetection = world.gameObject.AddComponent<ShakeDetection>();
		}
	}

	public void UnloadS4FogSystem()
	{
		if (_fogRenderer != null)
		{
			_fogRenderer.SetActive(active: false);
			_fogRenderer.DetachFromRenderer(world.Camera.__camera);
			_fogRenderer = null;
		}
		world.mWorldFogManager.UninitWorldFog();
		LightDataManager.GetInstance().ClearAllLightData();
		if (_shakeDetection != null)
		{
			_shakeDetection.StopDetection();
		}
	}

	public bool IsDawn(int targetServerId)
	{
		return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsDawn", targetServerId);
	}

	private void InitTerrainS2(bool hasPreloadMat)
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta == null)
		{
			return;
		}
		if (_terrainS2 != null)
		{
			UnloadTerrainS2();
		}
		SeasonType seasonType = (SeasonType)curSkinMeta.seasonType;
		_useNewTerrainQuad = SystemInfo.supportsInstancing && curSkinMeta.world_terrain_mode == 1 && (seasonType == SeasonType.Snow || seasonType == SeasonType.Mummy);
		if (_useNewTerrainQuad)
		{
			if (_terrainS2 == null)
			{
				CreateTerrainS2(seasonType);
			}
			if (!hasPreloadMat)
			{
				_terrainS2.LoadMaterial(curSkinMeta.world_terrain_mode_mat);
			}
		}
		if (_terrainS2Renderer != null)
		{
			_terrainS2Renderer.SetActive(_useNewTerrainQuad);
		}
	}

	private void UnloadTerrainS2()
	{
		_useNewTerrainQuad = false;
		if (_terrainS2 != null)
		{
			HeatSourceDataManager.GetInstance().SetOnHeatSourceChanged(null);
			_terrainS2.UnInit();
			_terrainS2 = null;
			_terrainS2Renderer.SetActive(active: false);
			_terrainS2Renderer.DetachFromRenderer(world.Camera.__camera);
			_terrainS2Renderer = null;
		}
	}

	private void OnSkinChangeTerrainS2()
	{
		InitTerrainS2(hasPreloadMat: false);
	}

	private bool UseTerrainMatHeight()
	{
		if (SceneQualitySetting.GetTerrainLevel() != 3)
		{
			return false;
		}
		return true;
	}

	private void ClearRenderAsset()
	{
		if (_renderAssetMap.Count <= 0)
		{
			return;
		}
		foreach (Dictionary<uint, DecorationRenderMesh[]> value in _renderAssetMap.Values)
		{
			value.Clear();
		}
		_renderAssetMap.Clear();
	}

	public void LoadDecoration()
	{
		_enableInstancing = SystemInfo.supportsInstancing;
		if (mIsNineNationMode)
		{
			InitDecoAsset_S5();
		}
		else
		{
			LoadDecoAsset();
		}
	}

	private void LoadDecoAsset()
	{
		string targetPath = "Assets/Main/Scenes/WorldDecoration/WorldDecoration.asset";
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && !curSkinMeta.world_deco_asset.IsNullOrEmpty())
		{
			targetPath = curSkinMeta.world_deco_asset;
		}
		if (_decorationRenderDatAsset != null)
		{
			if (_curPath == targetPath)
			{
				if (_decorationRenderData != null && _renderAssetMap.Count > 0)
				{
					InitWorldDecoration();
				}
				return;
			}
			_decorationRenderDatAsset.Release();
			_decorationRenderDatAsset = null;
			_decorationRenderData = null;
			worldBlocks = null;
			chunkObjList.Clear();
			ClearRenderAsset();
		}
		_curPath = targetPath;
		_decorationRenderDatAsset = GameEntry.Resource.LoadAssetAsync(_curPath, typeof(DecorationRenderData));
		Asset decorationRenderDatAsset = _decorationRenderDatAsset;
		decorationRenderDatAsset.completed = (Action<Asset>)Delegate.Combine(decorationRenderDatAsset.completed, (Action<Asset>)delegate(Asset asset)
		{
			if (parentNode == null)
			{
				_decorationRenderDatAsset?.Release();
				_decorationRenderDatAsset = null;
			}
			else if (world == null || world.gameObject == null)
			{
				_decorationRenderDatAsset?.Release();
				_decorationRenderDatAsset = null;
			}
			else if (asset.asset == null)
			{
				_decorationRenderDatAsset?.Release();
				_decorationRenderDatAsset = null;
			}
			else
			{
				SceneSkinMeta curSkinMeta2 = SceneSkinManager.Instance.GetCurSkinMeta();
				if (curSkinMeta2 != null && curSkinMeta2.world_deco_asset != null && targetPath != curSkinMeta2.world_deco_asset)
				{
					_decorationRenderDatAsset?.Release();
					_decorationRenderDatAsset = null;
				}
				else
				{
					_decorationRenderData = asset.asset as DecorationRenderData;
					if (!(_decorationRenderData == null))
					{
						DecorationRenderAsset[] renderAssets = _decorationRenderData.renderAssets;
						ClearRenderAsset();
						int i = 0;
						for (int num = renderAssets.Length; i < num; i++)
						{
							DecorationRenderAsset decorationRenderAsset = renderAssets[i];
							if (decorationRenderAsset == null)
							{
								UnityEngine.Debug.LogError("装饰物引用资源丢失！！！meta文件异常");
							}
							else
							{
								Dictionary<uint, DecorationRenderMesh[]> dictionary = new Dictionary<uint, DecorationRenderMesh[]>();
								_renderAssetMap[decorationRenderAsset.guid] = dictionary;
								if (decorationRenderAsset.states != null && decorationRenderAsset.states.Length != 0)
								{
									_states[decorationRenderAsset.guid] = decorationRenderAsset.states;
								}
								DecorationLodMesh[] lodMeshes = decorationRenderAsset.lodMeshes;
								int j = 0;
								for (int num2 = lodMeshes.Length; j < num2; j++)
								{
									DecorationLodMesh decorationLodMesh = lodMeshes[j];
									int[] lodRanges = decorationLodMesh.lodRanges;
									uint num3 = 0u;
									int[] array = lodRanges;
									if (array != null)
									{
										int num4 = array.Length;
										if (num4 == 0)
										{
											num3 = uint.MaxValue;
										}
										else
										{
											int k = 0;
											for (int num5 = num4; k < num5; k++)
											{
												num3 |= (uint)(1 << array[k]);
											}
										}
									}
									else
									{
										num3 = uint.MaxValue;
									}
									dictionary[num3] = decorationLodMesh.renderMeshes;
								}
							}
						}
						InitWorldDecoration();
					}
				}
			}
		});
	}

	private void ClearShowObjMap()
	{
		foreach (List<WorldSceneDesc.ObjectDesc> value in _showObjMap.Values)
		{
			value?.Clear();
		}
		_tmpHideRect.Clear();
		_tmpHideCount = 0;
	}

	private void AddShowObj(WorldSceneDesc.ObjectDesc objectDesc)
	{
		int showObjGuid = GetShowObjGuid(objectDesc);
		if (showObjGuid >= 0)
		{
			if (!_showObjMap.TryGetValue(showObjGuid, out var value))
			{
				value = new List<WorldSceneDesc.ObjectDesc>(32);
				_showObjMap.Add(showObjGuid, value);
			}
			value.Add(objectDesc);
			objectDesc.hide = false;
		}
	}

	private int GetShowObjGuid(WorldSceneDesc.ObjectDesc objectDesc)
	{
		if (objectDesc.type == 0)
		{
			switch (SeasonMapType)
			{
			case SeasonType.Mummy:
				if (!world.IsGreen(world.TilePosToIndex(objectDesc.worldDecorationTilePos)))
				{
					return GetGuidByState(objectDesc);
				}
				break;
			case SeasonType.Darkness:
				if (DescHasMultiple(objectDesc))
				{
					if (LightDataManager.GetInstance().IsLightUpInPointId(world.TilePosToIndex(objectDesc.worldDecorationTilePos)))
					{
						return GetGuidByState(objectDesc);
					}
					break;
				}
				return objectDesc.assetGuid;
			}
		}
		return objectDesc.assetGuid;
	}

	private bool DescHasMultiple(WorldSceneDesc.ObjectDesc objectDesc)
	{
		if (!_states.TryGetValue(objectDesc.assetGuid, out var value))
		{
			return false;
		}
		if (value == null || value.Length == 0)
		{
			return false;
		}
		return true;
	}

	private int GetGuidByState(WorldSceneDesc.ObjectDesc objectDesc, MapDecorateConfig.DecorateState target = MapDecorateConfig.DecorateState.Desert)
	{
		if (!_states.TryGetValue(objectDesc.assetGuid, out var value) || value == null || value.Length == 0)
		{
			return objectDesc.assetGuid;
		}
		Vector2Int worldDecorationTilePos = objectDesc.worldDecorationTilePos;
		UnityEngine.Random.InitState(worldDecorationTilePos.GetHashCode());
		int num = UnityEngine.Random.Range(0, 1000);
		DecorationRenderState[] array = value;
		foreach (DecorationRenderState decorationRenderState in array)
		{
			if (decorationRenderState.state == target && num <= decorationRenderState.percentAdd)
			{
				return decorationRenderState.guid;
			}
		}
		return -1;
	}

	private void UpdateDrawBatch()
	{
		if (_showObjMap.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<int, List<WorldSceneDesc.ObjectDesc>> item in _showObjMap)
		{
			int key = item.Key;
			List<WorldSceneDesc.ObjectDesc> value = item.Value;
			int count = value.Count;
			if (count <= 0)
			{
				continue;
			}
			int num = count / 1023;
			if (count % 1023 > 0)
			{
				num++;
			}
			int lodLevel = world.GetLodLevel();
			uint num2 = (uint)(1 << lodLevel);
			if (!_renderAssetMap.TryGetValue(key, out var value2))
			{
				if (mIsNineNationMode)
				{
					LoadRenderAsset(key);
				}
				continue;
			}
			DecorationRenderMesh[] array = null;
			foreach (KeyValuePair<uint, DecorationRenderMesh[]> item2 in value2)
			{
				if ((item2.Key & num2) != 0)
				{
					array = item2.Value;
					break;
				}
			}
			if (array == null)
			{
				continue;
			}
			int i = 0;
			for (int num3 = array.Length; i < num3; i++)
			{
				DecorationRenderMesh decorationRenderMesh = array[i];
				DecorationTransformInfo localTransformInfo = decorationRenderMesh.localTransformInfo;
				Vector3 pos = localTransformInfo.pos;
				Vector3 rotation = localTransformInfo.rotation;
				Vector3 scale = localTransformInfo.scale;
				Material renderMeshMaterial = GetRenderMeshMaterial(decorationRenderMesh);
				if (renderMeshMaterial == null)
				{
					continue;
				}
				for (int j = 0; j < num; j++)
				{
					int num4 = 1023 * j;
					int num5 = Mathf.Min(1023, count - num4);
					int num6 = num5;
					int num7 = 0;
					for (int k = 0; k < num5; k++)
					{
						int index = num4 + k;
						WorldSceneDesc.ObjectDesc objectDesc = value[index];
						if (objectDesc.hide)
						{
							num6--;
							continue;
						}
						if (_tmpHideCount > 0)
						{
							bool flag = false;
							int4 worldDecorationTileRect = objectDesc.worldDecorationTileRect;
							for (int l = 0; l < _tmpHideCount; l++)
							{
								if (_tmpHideRect[l].Overlaps(worldDecorationTileRect))
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								objectDesc.hide = true;
								num6--;
								continue;
							}
						}
						if (this.decorationInvisibleRectType != DecorationInvisibleRectType.None)
						{
							bool flag2 = false;
							Vector3 localPos = objectDesc.localPos;
							DecorationInvisibleRectType decorationInvisibleRectType = this.decorationInvisibleRectType;
							if (decorationInvisibleRectType == DecorationInvisibleRectType.Circle)
							{
								float num8 = localPos.x - decorationInvisibleRectCenter.x;
								float num9 = localPos.z - decorationInvisibleRectCenter.z;
								if (num8 * num8 + num9 * num9 < decorationInvisibleRectRadius * decorationInvisibleRectRadius)
								{
									flag2 = true;
								}
							}
							if (flag2)
							{
								Vector2Int worldDecorationTilePos = objectDesc.worldDecorationTilePos;
								if (!IsObstacle(worldDecorationTilePos))
								{
									num6--;
									continue;
								}
							}
						}
						Vector3 pos2 = objectDesc.localPos + pos;
						Vector3 euler = objectDesc.rotation + rotation;
						Vector3 scale2 = objectDesc.scale;
						Vector3 s = new Vector3(scale2.x * scale.x, scale2.y * scale.y, scale2.z * scale.z);
						_mInstanceTransform[num7] = Matrix4x4.TRS(pos2, Quaternion.Euler(euler), s);
						num7++;
					}
					if (num6 != 0)
					{
						ShowMesh(decorationRenderMesh, renderMeshMaterial, num6, _enableInstancing && renderMeshMaterial.enableInstancing);
					}
				}
			}
		}
		if (_tmpHideCount > 0)
		{
			_tmpHideRect.Clear();
			_tmpHideCount = 0;
		}
	}

	private void ShowMesh(DecorationRenderMesh renderMesh, Material material, int instancedCount, bool instanced)
	{
		Mesh mesh = renderMesh.mesh;
		if (instanced)
		{
			Graphics.DrawMeshInstanced(mesh, 0, material, _mInstanceTransform, instancedCount, null, renderMesh.shadowCastingMode, renderMesh.receiveShadows, renderMesh.layer);
			return;
		}
		for (int i = 0; i < instancedCount; i++)
		{
			Graphics.DrawMesh(mesh, _mInstanceTransform[i], material, renderMesh.layer, null, 0, null, renderMesh.shadowCastingMode, renderMesh.receiveShadows);
		}
	}

	private Material GetRenderMeshMaterial(DecorationRenderMesh decorationRenderMesh)
	{
		if (!_renderMeshMaterialMap.TryGetValue(decorationRenderMesh, out var value))
		{
			if (decorationRenderMesh.material == null)
			{
				_renderMeshMaterialMap.Add(decorationRenderMesh, null);
				return null;
			}
			Material material = UnityEngine.Object.Instantiate(decorationRenderMesh.material);
			_renderMeshMaterialMap.Add(decorationRenderMesh, material);
			return material;
		}
		return value;
	}

	public void SetDecorationInvisibleRect(DecorationInvisibleRectType rectType, Vector3 center, float radius)
	{
		decorationInvisibleRectType = rectType;
		decorationInvisibleRectCenter = center;
		decorationInvisibleRectRadius = radius;
	}

	private JobHandle OnPerformCulling(BatchRendererGroup rendererGroup, BatchCullingContext cullingContext)
	{
		if (cullingContext.cullingPlanes[2].distance == 0f)
		{
			return _cullingDependency;
		}
		int lodLevel = world.GetLodLevel();
		uint lod = (uint)(1 << lodLevel);
		NativeArray<FrustumPlanes.PlanePacket4> planes = FrustumPlanes.BuildSOAPlanePackets(cullingContext.cullingPlanes, Allocator.TempJob);
		return _cullingDependency = new CullingJob
		{
			Planes = planes,
			CullDatas = _cullDataArray,
			TileDatas = _tileOccupyArray,
			Batches = cullingContext.batchVisibility,
			IndexList = cullingContext.visibleIndices,
			lod = lod,
			tileCount = new int2(world.TileCount.x, world.TileCount.y)
		}.Schedule(cullingContext.batchVisibility.Length, 16);
	}

	private void UnloadWorldDecoration()
	{
		_showObjMap.Clear();
	}

	private void PlayObjAnimShow()
	{
	}

	public static Vector2Int GetGuidRange(int serverIndex)
	{
		if (serverIndex < 0 || serverIndex >= 9)
		{
			UnityEngine.Debug.LogError("服务器ID必须在0到 0-8之间");
		}
		int num = int.MaxValue / 9;
		int num2 = serverIndex * num + 1;
		int y = (serverIndex + 1) * num;
		if (serverIndex == 8)
		{
			y = int.MaxValue;
		}
		if (num2 < 1)
		{
			num2 = 1;
		}
		return new Vector2Int(num2, y);
	}

	public static int GetServerIdFromGuid(int guid)
	{
		if (guid <= 0)
		{
			UnityEngine.Debug.LogError("GUID必须是正整数");
			return -1;
		}
		long num = 2147483647L / 9L;
		int num2 = (int)(guid / num);
		if (num2 < 0 || num2 >= 9)
		{
			UnityEngine.Debug.LogError("GUID 超过范围" + guid);
			return -1;
		}
		return num2;
	}

	private void InitDecoAsset_S5()
	{
		ClearRenderAsset();
		_curPath = "";
		lastWorldSceneDescPath = "";
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			SeasonMapType = (SeasonType)curSkinMeta.mapType;
		}
		if (regionsMap_S5 == null)
		{
			regionsMap_S5 = new Dictionary<Vector2Int, Region_S5>();
		}
		InitS5TerrainMatAssetInfo();
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		SeasonDataManager.Instance.GetNinePalacesIndex(curServerId);
		string text = "Assets/Main/SeasonRes/S5/Scenes/WorldDecoration_S5/renderData.asset";
		if (mLoadedAssets.ContainsKey(text))
		{
			return;
		}
		if (!GameEntry.Resource.HasAsset(text))
		{
			Log.Error("decoration data not exist: {0}", text);
			return;
		}
		Asset decorationRenderDatAsset = GameEntry.Resource.LoadAssetAsync(text, typeof(DecorationRenderData_S5));
		if (decorationRenderDatAsset == null)
		{
			Log.Error("Failed to load decoration data: {0}", text);
			return;
		}
		mLoadedAssets.Add(text, decorationRenderDatAsset);
		Asset asset = decorationRenderDatAsset;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset asset2)
		{
			if (decorationRenderDatAsset != null)
			{
				if (SceneManager.CurrSceneID != 2)
				{
					decorationRenderDatAsset?.Release();
					decorationRenderDatAsset = null;
				}
				else
				{
					mRenderAssetPathMap.Clear();
					DecorationRenderData_S5 decorationRenderData_S = asset2.asset as DecorationRenderData_S5;
					if (!(decorationRenderData_S == null))
					{
						List<int> guidList = decorationRenderData_S.guidList;
						List<string> assetPathList = decorationRenderData_S.assetPathList;
						for (int i = 0; i < guidList.Count; i++)
						{
							mRenderAssetPathMap.Add(guidList[i], assetPathList[i]);
						}
					}
				}
			}
		});
		LoadWorldFog_S5();
		GameEntry.Event.Subscribe(EventId.OnMiniMapClickJump, CanJumpUpdateDecoration);
	}

	public void LoadAllRegionsAsset()
	{
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				LoadRegionAsset(new Vector2Int(i, j));
			}
		}
	}

	public void UnloadAllRegionsAsset()
	{
		foreach (KeyValuePair<Vector2Int, Region_S5> item in regionsMap_S5)
		{
			Vector2Int key = item.Key;
			string text = key.x + "-" + key.y;
			string key2 = $"Assets/Main/SeasonRes/S5/Scenes/NewWorldSceneDescS5_{text.ToString()}.bytes";
			if (mLoadedAssets.ContainsKey(key2) && mLoadedAssets[key2] != null)
			{
				mLoadedAssets[key2].Release();
				mLoadedAssets.Remove(key2);
			}
		}
		regionsMap_S5.Clear();
	}

	public void UnloadSomeRegionsData()
	{
		List<Vector2Int> currentCameraRegionCoord = GetCurrentCameraRegionCoord();
		Dictionary<Vector2Int, Region_S5> dictionary = new Dictionary<Vector2Int, Region_S5>();
		Vector2Int key = new Vector2Int(-1000, -1000);
		float num = 0f;
		foreach (KeyValuePair<Vector2Int, Region_S5> item in regionsMap_S5)
		{
			if (item.Value != null && !item.Value.isEmptyRegion && !currentCameraRegionCoord.Contains(item.Key))
			{
				dictionary.Add(item.Key, item.Value);
				if (num < item.Value.usedTime)
				{
					num = item.Value.usedTime;
					key = item.Key;
				}
			}
		}
		if (dictionary.ContainsKey(key))
		{
			dictionary.Remove(key);
		}
		foreach (KeyValuePair<Vector2Int, Region_S5> item2 in dictionary)
		{
			UnloadRegionAsset(item2.Key);
		}
	}

	public void UnloadRegionAsset(Vector2Int regionCoord)
	{
		string text = regionCoord.x + "-" + regionCoord.y;
		string key = $"Assets/Main/SeasonRes/S5/Scenes/NewWorldSceneDescS5_{text.ToString()}.bytes";
		if (mLoadedAssets.ContainsKey(key) && mLoadedAssets[key] != null)
		{
			mLoadedAssets[key].Release();
			mLoadedAssets.Remove(key);
		}
		if (regionsMap_S5.ContainsKey(regionCoord))
		{
			regionsMap_S5.Remove(regionCoord);
		}
	}

	public List<Vector2Int> GetCurrentCameraRegionCoord()
	{
		List<Vector2Int> list = new List<Vector2Int>();
		WorldCamera camera = world.Camera;
		Vector3 vector = camera.cameraAnchor[0] + new Vector3(-3f, 0f, -3f);
		Vector3 vector2 = camera.cameraAnchor[2] + new Vector3(3f, 0f, 3f);
		Vector2Int tilePos = new Vector2Int((int)(vector.x / 2f), (int)(vector.z / 2f));
		Vector2Int tilePos2 = new Vector2Int((int)(vector2.x / 2f), (int)(vector2.z / 2f));
		Vector2Int vector2Int = TilePosToChunkCoord(tilePos);
		Vector2Int vector2Int2 = TilePosToChunkCoord(tilePos2);
		int x = vector2Int.x;
		int x2 = vector2Int2.x;
		int y = vector2Int.y;
		int y2 = vector2Int2.y;
		for (int i = y; i <= y2; i++)
		{
			for (int j = x; j <= x2; j++)
			{
				Vector2Int chunkCoord = new Vector2Int(j, i);
				Vector2Int regionCoordFromTileCoord = GetRegionCoordFromTileCoord(ChunkCoordToTilePos(chunkCoord));
				if (!list.Contains(regionCoordFromTileCoord))
				{
					list.Add(regionCoordFromTileCoord);
				}
			}
		}
		return list;
	}

	public void LoadRegionAsset(Vector2Int regionCoord)
	{
		string text = regionCoord.x + "-" + regionCoord.y;
		string text2 = $"Assets/Main/SeasonRes/S5/Scenes/NewWorldSceneDescS5_{text.ToString()}.bytes";
		if (mLoadedAssets.ContainsKey(text2))
		{
			return;
		}
		if (!GameEntry.Resource.HasAsset(text2))
		{
			Region_S5 region_S = new Region_S5();
			region_S.Init(null, this);
			if (regionsMap_S5.ContainsKey(regionCoord))
			{
				Log.Error("regionsMap already contains key:" + regionCoord);
				regionsMap_S5[regionCoord] = region_S;
			}
			else
			{
				regionsMap_S5.Add(regionCoord, region_S);
			}
			return;
		}
		Asset worldDescAsset = GameEntry.Resource.LoadAssetAsync(text2, typeof(TextAsset));
		if (worldDescAsset == null)
		{
			Log.Error("LoadBlockAsset {0} is null", text2);
			return;
		}
		mLoadedAssets.Add(text2, worldDescAsset);
		Asset asset = worldDescAsset;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (worldDescAsset != null)
			{
				if (SceneManager.CurrSceneID != 2 || !mIsNineNationMode)
				{
					worldDescAsset?.Release();
					worldDescAsset = null;
				}
				else if (world == null || world.gameObject == null)
				{
					worldDescAsset?.Release();
					worldDescAsset = null;
				}
				else
				{
					TextAsset textAsset = worldDescAsset.Get<TextAsset>();
					if (textAsset == null)
					{
						worldDescAsset?.Release();
						worldDescAsset = null;
					}
					else
					{
						Region_S5 region_S2 = new Region_S5();
						region_S2.Init(textAsset.bytes, this);
						if (regionsMap_S5.ContainsKey(regionCoord))
						{
							Log.Error("regionsMap already contains key:" + regionCoord);
							regionsMap_S5[regionCoord] = region_S2;
						}
						else
						{
							regionsMap_S5.Add(regionCoord, region_S2);
						}
						worldDescAsset.Release();
						__UpdateView(force: true);
					}
				}
			}
		});
		if (GetCurrentRegionCount() > 2)
		{
			UnloadSomeRegionsData();
		}
	}

	public int GetCurrentRegionCount()
	{
		if (regionsMap_S5 == null)
		{
			return 0;
		}
		int num = 0;
		foreach (KeyValuePair<Vector2Int, Region_S5> item in regionsMap_S5)
		{
			if (!item.Value.isEmptyRegion)
			{
				num++;
			}
		}
		return num;
	}

	public void LoadRenderAsset(int guid)
	{
		if (!mRenderAssetPathMap.TryGetValue(guid, out var value))
		{
			Log.Error("WorldStaticManager_NineNation LoadRenderAsset {0} not found", guid);
		}
		else
		{
			if (mLoadedAssets.ContainsKey(value))
			{
				return;
			}
			Asset decorationRenderDatAsset = GameEntry.Resource.LoadAssetAsync(value, typeof(DecorationRenderAsset));
			if (decorationRenderDatAsset == null)
			{
				Log.Error("Failed to load decoration data: {0}", value);
				return;
			}
			mLoadedAssets.Add(value, decorationRenderDatAsset);
			Asset asset = decorationRenderDatAsset;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset asset2)
			{
				if (SceneManager.CurrSceneID != 2 || !mIsNineNationMode)
				{
					decorationRenderDatAsset?.Release();
					decorationRenderDatAsset = null;
				}
				else if (decorationRenderDatAsset != null && asset2 != null)
				{
					DecorationRenderAsset decorationRenderAsset = asset2.asset as DecorationRenderAsset;
					if (decorationRenderAsset == null)
					{
						Log.Error("装饰物引用资源丢失！！！meta文件异常");
					}
					else
					{
						Dictionary<uint, DecorationRenderMesh[]> dictionary = new Dictionary<uint, DecorationRenderMesh[]>();
						DecorationLodMesh[] lodMeshes = decorationRenderAsset.lodMeshes;
						int i = 0;
						for (int num = lodMeshes.Length; i < num; i++)
						{
							DecorationLodMesh decorationLodMesh = lodMeshes[i];
							int[] lodRanges = decorationLodMesh.lodRanges;
							uint num2 = 0u;
							int[] array = lodRanges;
							if (array != null)
							{
								int num3 = array.Length;
								if (num3 == 0)
								{
									num2 = uint.MaxValue;
								}
								else
								{
									int j = 0;
									for (int num4 = num3; j < num4; j++)
									{
										num2 |= (uint)(1 << array[j]);
									}
								}
							}
							else
							{
								num2 = uint.MaxValue;
							}
							dictionary[num2] = decorationLodMesh.renderMeshes;
						}
						_renderAssetMap[decorationRenderAsset.guid] = dictionary;
						if (decorationRenderAsset.states != null && decorationRenderAsset.states.Length != 0)
						{
							_states[decorationRenderAsset.guid] = decorationRenderAsset.states;
						}
					}
				}
			});
		}
	}

	private void GetChunkObjList_S5(Vector2Int chunkCoord, List<WorldSceneDesc.ObjectDesc> list, bool viewLevelChanged, bool zoomChange)
	{
		if (mJumpUpdateDecoration_S5)
		{
			if (!(viewLevelChanged || zoomChange))
			{
				list.Clear();
				return;
			}
			mJumpUpdateDecoration_S5 = false;
		}
		list.Clear();
		if (regionsMap_S5 == null)
		{
			return;
		}
		Vector2Int regionCoordFromTileCoord = GetRegionCoordFromTileCoord(ChunkCoordToTilePos(chunkCoord));
		Region_S5 region_S = GetRegion_S5(regionCoordFromTileCoord);
		if (region_S != null)
		{
			List<WorldSceneDesc.ObjectDesc> list2 = region_S.GetChunkObjList(chunkCoord);
			if (list2 != null)
			{
				list.AddRange(list2);
			}
		}
		else
		{
			LoadRegionAsset(regionCoordFromTileCoord);
		}
	}

	private Region_S5 GetRegion_S5(Vector2Int regionCoord)
	{
		if (regionsMap_S5.TryGetValue(regionCoord, out var value))
		{
			return value;
		}
		return null;
	}

	public void UnInit_S5()
	{
		_curPath = "";
		lastWorldSceneDescPath = "";
		foreach (KeyValuePair<string, Asset> mLoadedAsset in mLoadedAssets)
		{
			if (mLoadedAsset.Value != null)
			{
				mLoadedAsset.Value.Release();
			}
		}
		mLoadedAssets.Clear();
		regionsMap_S5 = null;
		foreach (KeyValuePair<int, Asset> item in mTerrainMatDic_Height)
		{
			if (item.Value != null)
			{
				item.Value.Release();
			}
		}
		mTerrainMatDic_Height.Clear();
		foreach (KeyValuePair<int, Asset> item2 in mTerrainMatDic_Low)
		{
			if (item2.Value != null)
			{
				item2.Value.Release();
			}
		}
		mTerrainMatDic_Low.Clear();
		foreach (KeyValuePair<int, Asset> item3 in mWorldBlockDescAssetDic_S5)
		{
			item3.Value?.Release();
		}
		if (worldFogRequest != null)
		{
			worldFogRequest.Destroy();
		}
		mWorldBlockDescAssetDic_S5.Clear();
		ClearRenderAsset();
		mGlobalRailRects.Clear();
		mBlockHashSet_S5.Clear();
		mBlockBetweenServer_S5.Clear();
		GameEntry.Event.Unsubscribe(EventId.OnMiniMapClickJump, CanJumpUpdateDecoration);
		RemoveNorthDoor();
		RemoveSouthDoor();
	}

	public static int GetTileCoordServerIndex(int width, int height, Vector2Int tileCoord)
	{
		if (tileCoord.x < 0 || tileCoord.x >= width || tileCoord.y < 0 || tileCoord.y >= height)
		{
			return -1;
		}
		int num = width / 3;
		int num2 = tileCoord.x / num;
		return tileCoord.y / num * 3 + num2;
	}

	public static Vector2Int GetRegionCoordFromTileCoord(Vector2Int tileCoord)
	{
		return new Vector2Int(tileCoord.x / 500, tileCoord.y / 500);
	}

	public static int GetServerIndexFromRegionCoord(Vector2Int regionCoord)
	{
		int x = regionCoord.x * 500;
		int y = regionCoord.y * 500;
		return GetTileCoordServerIndex(3000, 3000, new Vector2Int(x, y));
	}

	public static int GetServerIndexFromChunkCoord(Vector2Int chunkCoord)
	{
		int x = chunkCoord.x * 25;
		int y = chunkCoord.y * 25;
		return GetTileCoordServerIndex(3000, 3000, new Vector2Int(x, y));
	}

	public void LoadWorldFog_S5()
	{
		string text = "Assets/Main/SeasonRes/S5/Prefabs/World/s5_fog.prefab";
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			string edge_world_fog = curSkinMeta.edge_world_fog;
			if (!string.IsNullOrEmpty(edge_world_fog))
			{
				text = edge_world_fog;
			}
		}
		if (worldFogRequest != null)
		{
			worldFogRequest.Destroy();
			worldFogRequest = null;
		}
		if (!GameEntry.Resource.HasAsset(text))
		{
			Log.Error("LoadWorldFog_S5 not exist: {0}", text);
		}
		else
		{
			if (worldFogRequest != null)
			{
				return;
			}
			worldFogRequest = GameEntry.Resource.InstantiateAsync(text);
			worldFogRequest.completed += delegate
			{
				GameObject gameObject = worldFogRequest.gameObject;
				if (gameObject == null)
				{
					Log.Error("s5 worldFog gameObject null");
				}
				else if (SceneManager.CurrSceneID != 2)
				{
					worldFogRequest?.Destroy();
					worldFogRequest = null;
				}
				else
				{
					Vector3 position = new Vector3(0f, 5f, 0f);
					gameObject.transform.position = position;
				}
			};
		}
	}

	public void UpdateTerrainObject_S5(WorldCamera worldCamera)
	{
		Vector3 vector = worldCamera.cameraAnchor[0] + new Vector3(-3f, 0f, -3f);
		Vector3 vector2 = worldCamera.cameraAnchor[2] + new Vector3(3f, 0f, 3f);
		Vector2Int vector2Int = new Vector2Int((int)(vector.x / 2f), (int)(vector.z / 2f));
		Vector2Int vector2Int2 = new Vector2Int((int)(vector2.x / 2f), (int)(vector2.z / 2f));
		Vector2Int vector2Int3 = TilePosToChunkCoord(vector2Int);
		Vector2Int vector2Int4 = TilePosToChunkCoord(vector2Int2);
		int num = vector2Int3.x - 1;
		int num2 = vector2Int4.x + 1;
		int num3 = vector2Int3.y - 1;
		int num4 = vector2Int4.y + 1;
		for (int i = num3; i <= num4; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				Vector2Int chunkCoord = new Vector2Int(j, i);
				Vector2Int vector2Int5 = ChunkCoordToBlockCoord(chunkCoord);
				if (terrains.ContainsKey(vector2Int5))
				{
					continue;
				}
				GameObject gameObject = SceneManager.WorldTerrainAssetHandler.Spawn();
				if (!(gameObject == null))
				{
					MeshRenderer[] componentsInChildren = gameObject.transform.GetComponentsInChildren<MeshRenderer>();
					if (componentsInChildren != null && componentsInChildren.Length != 0)
					{
						SetTerrainMaterial_S5(componentsInChildren, vector2Int5);
					}
					gameObject.transform.position = TileFloatToWorld(vector2Int5.x * world.BlockSize, vector2Int5.y * world.BlockSize);
					terrains.Add(vector2Int5, gameObject);
				}
			}
		}
		CheckDoorInOutView(vector2Int, vector2Int2);
	}

	private void CheckDoorInOutView(Vector2Int lb, Vector2Int rt)
	{
		bool flag = lb.x - 20 <= 1500 && 1500 <= rt.x + 20 && lb.y - 20 <= 1993 && 1993 <= rt.y + 20;
		if (flag && northDoor == null)
		{
			CreateNorthDoor();
		}
		else if (!flag && northDoor != null)
		{
			RemoveNorthDoor();
		}
		flag = lb.x - 20 <= 1500 && 1500 <= rt.x + 20 && lb.y - 20 <= 1002 && 1002 <= rt.y + 20;
		if (flag && southDoor == null)
		{
			CreateSouthDoor();
		}
		else if (!flag && southDoor != null)
		{
			RemoveSouthDoor();
		}
	}

	private void RemoveNorthDoor()
	{
		if (northDoor != null)
		{
			northDoor.Destroy();
			northDoor = null;
		}
	}

	private void RemoveSouthDoor()
	{
		if (southDoor != null)
		{
			southDoor.Destroy();
			southDoor = null;
		}
	}

	private void CreateNorthDoor()
	{
		northDoor = GameEntry.Resource.InstantiateAsync("Assets/Main/SeasonRes/S5/Prefabs/World/beimen.prefab");
		if (northDoor == null)
		{
			return;
		}
		northDoor.completed += delegate
		{
			GameObject gameObject = northDoor.gameObject;
			if (!(gameObject == null))
			{
				gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				gameObject.transform.position = new Vector3(2998.86f, 0f, 4002.74f);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localRotation = Quaternion.identity;
			}
		};
	}

	private void CreateSouthDoor()
	{
		southDoor = GameEntry.Resource.InstantiateAsync("Assets/Main/SeasonRes/S5/Prefabs/World/nanmen.prefab");
		if (southDoor == null)
		{
			return;
		}
		southDoor.completed += delegate
		{
			GameObject gameObject = southDoor.gameObject;
			if (!(gameObject == null))
			{
				gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				gameObject.transform.position = new Vector3(2998.61f, 0f, 1995.92f);
				gameObject.transform.localScale = Vector3.one * 2f;
				gameObject.transform.localRotation = Quaternion.identity;
			}
		};
	}

	public void UpdateTerrainObject_S5(Vector2Int viewChunkCoord)
	{
		for (int i = minXY.y; i <= maxXY.y; i++)
		{
			for (int j = minXY.x; j <= maxXY.x; j++)
			{
				Vector2Int chunkCoord = new Vector2Int(j, i);
				Vector2Int vector2Int = ChunkCoordToBlockCoord(chunkCoord);
				if (terrains.ContainsKey(vector2Int))
				{
					continue;
				}
				GameObject gameObject = SceneManager.WorldTerrainAssetHandler.Spawn();
				if (!(gameObject == null))
				{
					gameObject.SetActive(mCurrentShowTerrain);
					MeshRenderer[] componentsInChildren = gameObject.transform.GetComponentsInChildren<MeshRenderer>();
					if (componentsInChildren != null && componentsInChildren.Length != 0)
					{
						SetTerrainMaterial_S5(componentsInChildren, vector2Int);
					}
					gameObject.transform.position = TileFloatToWorld(vector2Int.x * world.BlockSize, vector2Int.y * world.BlockSize);
					terrains.Add(vector2Int, gameObject);
				}
			}
		}
	}

	private Vector3 TileFloatToWorld(float x, float y)
	{
		return new Vector3(x * 2f, 0f, y * 2f);
	}

	private Vector4[] GetMatOffsetFromString(string offsetStr)
	{
		if (string.IsNullOrEmpty(offsetStr))
		{
			return null;
		}
		List<Vector4> list = new List<Vector4>();
		string[] array = offsetStr.Split(new char[1] { '|' });
		if (array.Length != 4)
		{
			Log.Error("GetMatOffsetFromString error, offsetStr is:" + offsetStr);
			return null;
		}
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string[] array3 = array2[i].Split(new char[1] { ',' });
			if (array3.Length == 4)
			{
				if (float.TryParse(array3[0], out var result) && float.TryParse(array3[1], out var result2) && float.TryParse(array3[2], out var result3) && float.TryParse(array3[3], out var result4))
				{
					Vector4 item = new Vector4(result, result2, result3, result4);
					list.Add(item);
				}
			}
			else
			{
				Log.Error("GetMatOffsetFromString error, offsetStr is:" + offsetStr);
			}
		}
		if (list.Count == 4)
		{
			return list.ToArray();
		}
		Log.Error("GetMatOffsetFromString error, offsetStr is:" + offsetStr);
		return null;
	}

	public void InitS5TerrainMatAssetInfo()
	{
		mTerrainMatPathDic_Height.Clear();
		mTerrainMatPathDic_Low.Clear();
		for (int i = 1; i < 10; i++)
		{
			SceneSkinMeta worldSkinByIndex = SeasonDataManager.Instance.GetWorldSkinByIndex(i);
			if (worldSkinByIndex != null)
			{
				mTerrainMatPathDic_Height.Add(i - 1, worldSkinByIndex.world_terrain);
				mTerrainMatPathDic_Low.Add(i - 1, worldSkinByIndex.world_terrain_low);
			}
		}
		mTerrainMatOffsetMap.Clear();
		Dictionary<int, Vector4[]> dictionary = new Dictionary<int, Vector4[]>();
		for (int j = 1; j < 10; j++)
		{
			SceneSkinMeta worldSkinByIndex2 = SeasonDataManager.Instance.GetWorldSkinByIndex(j);
			Vector4[] matOffsetFromString = GetMatOffsetFromString(worldSkinByIndex2.world_terrain_control);
			dictionary.Add(j - 1, matOffsetFromString);
		}
		for (int k = 0; k < 6; k++)
		{
			for (int l = 0; l < 6; l++)
			{
				InitTerrainMatOffsetMap(ref mTerrainMatOffsetMap, dictionary, new Vector2Int(k, l));
			}
		}
		List<Vector2Int> list = new List<Vector2Int>();
		int y = -1;
		int y2 = 6;
		for (int m = -1; m < 7; m++)
		{
			Vector2Int item = new Vector2Int(m, y);
			list.Add(item);
			Vector2Int item2 = new Vector2Int(m, y2);
			list.Add(item2);
		}
		int x = -1;
		int x2 = 6;
		for (int n = -1; n < 7; n++)
		{
			Vector2Int item3 = new Vector2Int(x, n);
			list.Add(item3);
			Vector2Int item4 = new Vector2Int(x2, n);
			list.Add(item4);
		}
		SetOuterBlockCoordOffset(list, ref mTerrainMatOffsetMap);
	}

	private void SetOuterBlockCoordOffset(List<Vector2Int> blockCoord_outerList, ref Dictionary<Vector2Int, Vector4> terrainMatOffsetMap)
	{
		foreach (Vector2Int blockCoord_outer in blockCoord_outerList)
		{
			Vector2Int key = blockCoord_outer;
			_ = Vector2.one;
			if (blockCoord_outer.y >= 0 && blockCoord_outer.y < 6 && blockCoord_outer.x < 0)
			{
				key = new Vector2Int(blockCoord_outer.x + 2, blockCoord_outer.y);
				new Vector2(0.025f, 0f);
			}
			else if (blockCoord_outer.y >= 0 && blockCoord_outer.y < 6 && blockCoord_outer.x >= 6)
			{
				key = new Vector2Int(blockCoord_outer.x - 2, blockCoord_outer.y);
				new Vector2(-0.025f, 0f);
			}
			else if (blockCoord_outer.x >= 0 && blockCoord_outer.x < 6 && blockCoord_outer.y < 0)
			{
				key = new Vector2Int(blockCoord_outer.x, blockCoord_outer.y + 2);
				new Vector2(0f, 0.025f);
			}
			else if (blockCoord_outer.x >= 0 && blockCoord_outer.x < 6 && blockCoord_outer.y >= 6)
			{
				key = new Vector2Int(blockCoord_outer.x, blockCoord_outer.y - 2);
				new Vector2(0f, -0.025f);
			}
			else if (blockCoord_outer.x < 0 && blockCoord_outer.y < 0)
			{
				key = new Vector2Int(blockCoord_outer.x + 2, blockCoord_outer.y + 2);
				new Vector2(0.025f, 0.025f);
			}
			else if (blockCoord_outer.x < 0 && blockCoord_outer.y >= 6)
			{
				key = new Vector2Int(blockCoord_outer.x + 2, blockCoord_outer.y - 2);
				new Vector2(0.025f, -0.025f);
			}
			else if (blockCoord_outer.x >= 6 && blockCoord_outer.y < 0)
			{
				key = new Vector2Int(blockCoord_outer.x - 2, blockCoord_outer.y + 2);
				new Vector2(-0.025f, 0.025f);
			}
			else if (blockCoord_outer.x >= 6 && blockCoord_outer.y >= 6)
			{
				key = new Vector2Int(blockCoord_outer.x - 2, blockCoord_outer.y - 2);
				new Vector2(-0.025f, -0.025f);
			}
			if (terrainMatOffsetMap.ContainsKey(key))
			{
				if (!terrainMatOffsetMap.ContainsKey(blockCoord_outer))
				{
					terrainMatOffsetMap.Add(blockCoord_outer, terrainMatOffsetMap[key]);
				}
			}
			else
			{
				Log.Error("cant find offsetCoord_outer" + key.ToString());
			}
		}
	}

	private void InitTerrainMatOffsetMap(ref Dictionary<Vector2Int, Vector4> map, Dictionary<int, Vector4[]> serverOffsetMap, Vector2Int blockCoord)
	{
		if (blockCoord.x < 0 || blockCoord.x > 5 || blockCoord.y < 0 || blockCoord.y > 5)
		{
			return;
		}
		int blockServerIndex = GetBlockServerIndex(blockCoord);
		serverOffsetMap.TryGetValue(blockServerIndex, out var value);
		if (value != null)
		{
			Vector4 value2 = Vector4.zero;
			Vector2Int vector2Int = new Vector2Int(blockCoord.x % 2, blockCoord.y % 2);
			if (vector2Int.x == 0 && vector2Int.y == 0)
			{
				value2 = value[0];
			}
			else if (vector2Int.x == 0 && vector2Int.y == 1)
			{
				value2 = value[2];
			}
			else if (vector2Int.x == 1 && vector2Int.y == 0)
			{
				value2 = value[1];
			}
			else if (vector2Int.x == 1 && vector2Int.y == 1)
			{
				value2 = value[3];
			}
			if (!map.ContainsKey(blockCoord))
			{
				map.Add(blockCoord, value2);
			}
		}
	}

	private void SetTerrainMaterial_S5(MeshRenderer[] meshRenders, Vector2Int blockCoord)
	{
		mCurrentTerrain_HightQuality = UseTerrainMatHeight();
		Material terrainMatAsset = GetTerrainMatAsset(blockCoord);
		for (int i = 0; i < meshRenders.Length; i++)
		{
			meshRenders[i].sharedMaterial = terrainMatAsset;
			Vector4 value = Vector4.zero;
			mTerrainMatOffsetMap.TryGetValue(blockCoord, out value);
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			meshRenders[i].GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetVector("_ControlTex_ST", new Vector4(value.x, value.y, value.z, value.w));
			switch (GetBlockServerIndex(blockCoord))
			{
			case 0:
				materialPropertyBlock.SetFloat("_X", 2000f);
				materialPropertyBlock.SetFloat("_Z", 2000f);
				break;
			case 2:
				materialPropertyBlock.SetFloat("_X", 4000f);
				materialPropertyBlock.SetFloat("_Z", 2000f);
				break;
			case 5:
				materialPropertyBlock.SetFloat("_X", 0f);
				materialPropertyBlock.SetFloat("_Z", 0f);
				break;
			case 6:
				materialPropertyBlock.SetFloat("_X", 2000f);
				materialPropertyBlock.SetFloat("_Z", 4000f);
				break;
			case 7:
				materialPropertyBlock.SetFloat("_X", 0f);
				materialPropertyBlock.SetFloat("_Z", 0f);
				break;
			case 8:
				materialPropertyBlock.SetFloat("_X", 4000f);
				materialPropertyBlock.SetFloat("_Z", 4000f);
				break;
			default:
				materialPropertyBlock.SetFloat("_X", 2000f);
				materialPropertyBlock.SetFloat("_Z", 2000f);
				break;
			}
			meshRenders[i].SetPropertyBlock(materialPropertyBlock);
		}
	}

	public Material GetTerrainMatAsset(Vector2Int blockCoord)
	{
		int blockServerIndex = GetBlockServerIndex(blockCoord);
		Asset terrainMatAsset = GetTerrainMatAsset(blockServerIndex);
		if (terrainMatAsset != null)
		{
			return terrainMatAsset.asset as Material;
		}
		return null;
	}

	public Asset GetTerrainMatAsset(int serverIndex)
	{
		if (UseTerrainMatHeight())
		{
			mTerrainMatDic_Height.TryGetValue(serverIndex, out var value);
			if (value != null && !(value.asset == null))
			{
				return mTerrainMatDic_Height[serverIndex];
			}
			mTerrainMatPathDic_Height.TryGetValue(serverIndex, out var value2);
			if (value2 == null)
			{
				Log.Error("S5 Failed to load terrain path is empty");
				return null;
			}
			Asset asset = GameEntry.Resource.LoadAsset(value2, typeof(Material));
			if (asset != null)
			{
				if (mTerrainMatDic_Height.ContainsKey(serverIndex))
				{
					mTerrainMatDic_Height[serverIndex] = asset;
				}
				else
				{
					mTerrainMatDic_Height.Add(serverIndex, asset);
				}
				return asset;
			}
			Log.Error("S5 Failed to load terrain mat: " + value2);
		}
		else
		{
			mTerrainMatDic_Low.TryGetValue(serverIndex, out var value3);
			if (value3 != null && !(value3.asset == null))
			{
				return mTerrainMatDic_Low[serverIndex];
			}
			mTerrainMatPathDic_Low.TryGetValue(serverIndex, out var value4);
			if (value4 == null)
			{
				Log.Error("S5 Failed to load terrain path is empty");
				return null;
			}
			Asset asset2 = GameEntry.Resource.LoadAsset(value4, typeof(Material));
			if (asset2 != null)
			{
				if (mTerrainMatDic_Low.ContainsKey(serverIndex))
				{
					mTerrainMatDic_Low[serverIndex] = asset2;
				}
				else
				{
					mTerrainMatDic_Low.Add(serverIndex, asset2);
				}
				return asset2;
			}
			Log.Error("S5 Failed to load terrain mat: " + value4);
		}
		return null;
	}

	public void TryHideTerrain_S5(object currentShowBg)
	{
		if (!(currentShowBg is bool flag) || terrains == null)
		{
			return;
		}
		foreach (KeyValuePair<Vector2Int, GameObject> terrain in terrains)
		{
			terrain.Value.SetActive(!flag);
			mCurrentShowTerrain = !flag;
		}
	}

	public void CanJumpUpdateDecoration(object jumpedState)
	{
		if (jumpedState is long)
		{
			if ((long)jumpedState == 1)
			{
				mJumpUpdateDecoration_S5 = true;
			}
			else
			{
				mJumpUpdateDecoration_S5 = false;
			}
		}
	}

	public bool IsObstacle_S5(Vector2Int tileCoord)
	{
		int tileCoordServerIndex = GetTileCoordServerIndex(3000, 3000, tileCoord);
		if (tileCoordServerIndex == -1)
		{
			return true;
		}
		if (mBlockBetweenServer_S5.TryGetValue(tileCoordServerIndex, out var value))
		{
			foreach (Rect item2 in value)
			{
				if (item2.Contains(tileCoord))
				{
					return true;
				}
			}
		}
		foreach (Rect mGlobalRailRect in mGlobalRailRects)
		{
			if (mGlobalRailRect.Contains(tileCoord))
			{
				return true;
			}
		}
		if (!mBlockHashSet_S5.ContainsKey(tileCoordServerIndex))
		{
			LoadWorldBlockData(tileCoordServerIndex);
			return false;
		}
		HashSet<int> hashSet = mBlockHashSet_S5[tileCoordServerIndex];
		int item = TileCoord.TilePosToIndex(tileCoord, new Vector2Int(3000, 3000));
		return hashSet.Contains(item);
	}

	public void SetKingCityObstacle_S5(WorldMapGridRenderer.KingCity kingCity)
	{
		WorldMapGridRenderer.KingCity[] array = new WorldMapGridRenderer.KingCity[9];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				int num = j * 3 + i;
				if (num == 0)
				{
					array[num] = kingCity;
					continue;
				}
				array[num] = new WorldMapGridRenderer.KingCity
				{
					minX = kingCity.minX + i * 1000,
					maxX = kingCity.maxX + i * 1000,
					minY = kingCity.minY + j * 1000,
					maxY = kingCity.maxY + j * 1000
				};
			}
		}
		mKingCity = array;
	}

	public bool IsInKingCityRange(int x, int y)
	{
		int tileCoordServerIndex = GetTileCoordServerIndex(3000, 3000, new Vector2Int(x, y));
		if (mKingCity != null && tileCoordServerIndex >= 0 && tileCoordServerIndex < mKingCity.Length)
		{
			WorldMapGridRenderer.KingCity kingCity = mKingCity[tileCoordServerIndex];
			if (x <= kingCity.maxX && y <= kingCity.maxY && x >= kingCity.minX && y >= kingCity.minY)
			{
				return true;
			}
		}
		return false;
	}

	private void LoadWorldBlockData(int serverIndex)
	{
		if (mWorldBlockDescAssetDic_S5.ContainsKey(serverIndex))
		{
			return;
		}
		string text = string.Format("Assets/Main/SeasonRes/{0}/Scenes/WorldBlockDesc{0}_{1}.bytes", "S5", serverIndex);
		Asset blockDescAsset = GameEntry.Resource.LoadAssetAsync(text, typeof(TextAsset));
		if (blockDescAsset == null)
		{
			Log.Error("S5 Failed to load world block desc: " + text);
			return;
		}
		Vector2Int tileCount = new Vector2Int(3000, 3000);
		Asset asset = blockDescAsset;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (blockDescAsset != null)
			{
				if (blockDescAsset.isError)
				{
					Log.Error("S5 Failed to load world block desc isError: " + blockDescAsset.error);
				}
				else if (SceneManager.CurrSceneID != 2)
				{
					blockDescAsset?.Release();
					blockDescAsset = null;
				}
				else
				{
					WorldBlockDesc worldBlockDesc = new WorldBlockDesc();
					TextAsset textAsset = blockDescAsset.Get<TextAsset>();
					if (textAsset != null)
					{
						worldBlockDesc.Load(textAsset.bytes);
					}
					HashSet<int> hashSet = new HashSet<int>();
					for (int i = 0; i < worldBlockDesc.Tiles.Count; i++)
					{
						hashSet.Add(TileCoord.TilePosToIndex(worldBlockDesc.Tiles[i], tileCount));
					}
					if (mBlockHashSet_S5.ContainsKey(serverIndex))
					{
						mBlockHashSet_S5[serverIndex] = hashSet;
					}
					else
					{
						mBlockHashSet_S5.Add(serverIndex, hashSet);
					}
					blockDescAsset.Release();
					blockDescAsset = null;
					GameEntry.Event.Fire(EventId.UpdateWorldBlockData);
				}
			}
		});
		if (!mWorldBlockDescAssetDic_S5.ContainsKey(serverIndex))
		{
			mWorldBlockDescAssetDic_S5.Add(serverIndex, blockDescAsset);
		}
	}

	private void InitWorldBlock_S5()
	{
		foreach (KeyValuePair<int, Asset> item in mWorldBlockDescAssetDic_S5)
		{
			_ = item.Key;
			item.Value?.Release();
		}
		mGlobalRailRects.Clear();
		mBlockBetweenServer_S5.Clear();
		for (int i = 0; i < 9; i++)
		{
			int serverIndex = i;
			InitBlockBetweenServer_S5(serverIndex);
		}
		InitializeGlobalRailRects();
	}

	private int GetBlockServerIndex(Vector2Int blockCoord)
	{
		int num = 1000 / world.BlockSize;
		if (num <= 0)
		{
			num = 1;
		}
		int num2 = blockCoord.x / num;
		int num3 = blockCoord.y / num;
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num2 > 2)
		{
			num2 = 2;
		}
		if (num3 > 2)
		{
			num3 = 2;
		}
		return num3 * 3 + num2;
	}

	private static void InitializeGlobalRailRects()
	{
		int num = 499;
		int num2 = 2499;
		int[] array = new int[3] { 499, 1499, 2499 };
		for (int i = 0; i < 3; i++)
		{
			int num3 = array[i];
			int num4 = num3 - 1;
			int num5 = num3 + 1;
			mGlobalRailRects.Add(Rect.MinMaxRect(num - 1, num4, num2 + 1 + 1, num5 + 1));
		}
		int num6 = 499;
		int num7 = 2499;
		int[] array2 = new int[3] { 499, 1499, 2499 };
		for (int j = 0; j < 3; j++)
		{
			int num8 = array2[j];
			int num9 = num8 - 1;
			int num10 = num8 + 1;
			mGlobalRailRects.Add(Rect.MinMaxRect(num9, num6 - 1, num10 + 1, num7 + 1 + 1));
		}
	}

	private void InitBlockBetweenServer_S5(int serverIndex)
	{
		Vector2Int vector2Int = new Vector2Int(-1, -1);
		Vector2Int vector2Int2 = new Vector2Int(-1, -1);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		List<Rect> list = new List<Rect>();
		mBlockBetweenServer_S5.Add(serverIndex, list);
		switch (serverIndex)
		{
		case 0:
		{
			num = 0;
			num3 = 1000;
			num2 = 0;
			num4 = 1000;
			vector2Int = new Vector2Int(num, num4 - 4);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item23 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item23);
			vector2Int = new Vector2Int(num3 - 4, num2);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item24 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item24);
			break;
		}
		case 1:
		{
			num = 1000;
			num3 = 2000;
			num2 = 0;
			num4 = 1000;
			vector2Int = new Vector2Int(num, num4 - 6);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item20 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item20);
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num + 4, num4);
			Rect item21 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item21);
			vector2Int = new Vector2Int(num3 - 4, num2);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item22 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item22);
			break;
		}
		case 2:
		{
			num = 2000;
			num3 = 3000;
			num2 = 0;
			num4 = 1000;
			vector2Int = new Vector2Int(num, num4 - 4);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item18 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item18);
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num + 4, num4);
			Rect item19 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item19);
			break;
		}
		case 3:
		{
			num = 0;
			num3 = 1000;
			num2 = 1000;
			num4 = 2000;
			vector2Int = new Vector2Int(num, num4 - 4);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item15 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item15);
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num3, num2 + 4);
			Rect item16 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item16);
			vector2Int = new Vector2Int(num3 - 6, num2);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item17 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item17);
			break;
		}
		case 4:
		{
			num = 1000;
			num3 = 2000;
			num2 = 1000;
			num4 = 2000;
			vector2Int = new Vector2Int(num, num4 - 2);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item11 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item11);
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num3, num2 + 5);
			Rect item12 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item12);
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num + 2, num4);
			Rect item13 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item13);
			vector2Int = new Vector2Int(num3 - 2, num2);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item14 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item14);
			break;
		}
		case 5:
		{
			num = 2000;
			num3 = 3000;
			num2 = 1000;
			num4 = 2000;
			vector2Int = new Vector2Int(num, num4 - 4);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item8 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item8);
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num3, num2 + 4);
			Rect item9 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item9);
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num + 6, num4);
			Rect item10 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item10);
			break;
		}
		case 6:
		{
			num = 0;
			num3 = 1000;
			num2 = 2000;
			num4 = 3000;
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num3, num2 + 4);
			Rect item6 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item6);
			vector2Int = new Vector2Int(num3 - 4, num2);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item7 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item7);
			break;
		}
		case 7:
		{
			num = 1000;
			num3 = 2000;
			num2 = 2000;
			num4 = 3000;
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num3, num2 + 6);
			Rect item3 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item3);
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num + 4, num4);
			Rect item4 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item4);
			vector2Int = new Vector2Int(num3 - 4, num2);
			vector2Int2 = new Vector2Int(num3, num4);
			Rect item5 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item5);
			break;
		}
		case 8:
		{
			num = 2000;
			num3 = 3000;
			num2 = 2000;
			num4 = 3000;
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num3, num2 + 4);
			Rect item = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item);
			vector2Int = new Vector2Int(num, num2);
			vector2Int2 = new Vector2Int(num + 4, num4);
			Rect item2 = Rect.MinMaxRect(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
			list.Add(item2);
			break;
		}
		}
	}

	public void InitBlackBlock_S5()
	{
		string prefabPath = "Assets/Main/SeasonRes/S5/Prefabs/World/BlackBlock_sj5_center.prefab";
		string text = "Assets/Main/SeasonRes/S5/Prefabs/World/BlackBlock_sj5.prefab";
		if (text.Equals(lastWorldBlackBlockPath))
		{
			return;
		}
		lastWorldBlackBlockPath = text;
		if (blackBlockRequest != null)
		{
			GameObject gameObject = blackBlockRequest.gameObject;
			if (gameObject != null)
			{
				gameObject.GameObjectRecycleAll();
			}
			blackBlockRequest.Destroy();
			blackBlockRequest = null;
		}
		if (blackBlockRequest == null)
		{
			blackBlockRequest = GameEntry.Resource.InstantiateAsync(text);
			blackBlockRequest.completed += delegate
			{
				GameObject gameObject2 = blackBlockRequest.gameObject;
				if (gameObject2 == null)
				{
					Log.Error("gameObject null");
				}
				else if (SceneManager.CurrSceneID != 2)
				{
					blackBlockRequest?.Destroy();
					blackBlockRequest = null;
				}
				else
				{
					Vector3 vector = new Vector3(1000f, 0f, 1000f);
					gameObject2.transform.position = vector;
					gameObject2.CreatePool();
					for (int i = 2; i < 10; i++)
					{
						if (i != 5)
						{
							GameObject gameObject3 = gameObject2.GameObjectSpawn();
							Vector3 worldBasePosByIndex = SeasonDataManager.Instance.GetWorldBasePosByIndex(i);
							gameObject3.transform.position = vector + worldBasePosByIndex;
						}
					}
				}
			};
		}
		if (blackBlock_CenterRequest != null)
		{
			blackBlock_CenterRequest.Destroy();
			blackBlock_CenterRequest = null;
		}
		if (blackBlock_CenterRequest != null)
		{
			return;
		}
		blackBlock_CenterRequest = GameEntry.Resource.InstantiateAsync(prefabPath);
		blackBlock_CenterRequest.completed += delegate
		{
			GameObject gameObject2 = blackBlock_CenterRequest.gameObject;
			if (gameObject2 == null)
			{
				Log.Error("gameObject null");
			}
			else if (SceneManager.CurrSceneID != 2)
			{
				blackBlock_CenterRequest?.Destroy();
				blackBlock_CenterRequest = null;
			}
			else
			{
				Vector3 position = new Vector3(3000f, 0f, 3000f);
				gameObject2.transform.position = position;
			}
		};
	}
}
