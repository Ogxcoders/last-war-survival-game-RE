using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using VEngine;
using XLua;

public class WorldMapZoneManager : WorldManagerBase
{
	private enum LoadState
	{
		None,
		StartLoad,
		Update,
		Loaded
	}

	private const string ZoneMapPrefab = "Assets/Main/Scenes/Zone/Prefab/ZoneMapRoot.prefab";

	public const string EdgeMeshPath = "Assets/Main/Scenes/Zone/Edge/edge_{0}_{1}.bytes";

	public const string WorldCityColorTable = "worldcity_color";

	private float _cityUiScaleUp = 2f;

	private const float ZonePosToTilePos = 1f;

	private LoadState _loadState;

	private WorldZoneTreeNode _findNode;

	private WorldZoneMapData _zoneMapData;

	private WorldZoneMapRoot _zoneMapRoot;

	private AutoAdjustLod adjuster;

	private readonly List<WorldZone> _lastClippedZones = new List<WorldZone>();

	private List<WorldZone> _clippedZones = new List<WorldZone>();

	public Dictionary<int, Vector4> zonePos = new Dictionary<int, Vector4>(128);

	private int _worldCityColorCount;

	private Dictionary<int, WorldCityColor> _worldCityColors;

	private LuaTable _allianceCityLuaTable;

	private List<OasisViewScale> _oasisViewScaleList;

	private bool _functionOn;

	private Material _blackArea;

	private Material _blackAreaSeasonSnow;

	private Material _blackAreaSeasonDark;

	private Material _blackAreaCenter_S5;

	private Material _blackAreaMaterialDark_S5;

	private Material _blackAreaS5MaterialGreen_S5;

	private Material _blackAreaS5MaterialLight_S5;

	private Material _edgeMaterial1;

	private Material _edgeMaterial2;

	private Material _zoneMaterialFill;

	private Material _zoneMaterialOccupy;

	private Material _zoneMaterialOccupyS1_full;

	private Material _zoneMaterialOccupyS1_single;

	private static readonly Dictionary<Color, Material> CacheEdgeMat1 = new Dictionary<Color, Material>();

	private static readonly Dictionary<Color, Material> CacheEdgeMat2 = new Dictionary<Color, Material>();

	private int allianceCityCenterOffset = -3;

	private int allianceCityRange = 20;

	private bool isCurActive = true;

	private bool isSetActive = true;

	private SeasonType SeasonMapType;

	private int SeasonMapMode = -1;

	private bool isEdgeMode = true;

	private bool currentShowBg;

	private int _curServerId;

	private Asset mBlackZoneMaterialAsset;

	private Asset mBlackZoneMaterialAsset_S5Green;

	private Asset mBlackZoneMaterialAsset_S5Light;

	private Asset mBlackZoneMaterialAsset_S5Dark;

	private InstanceRequest bigMapRailway_S5;

	private Mesh mBlackZoneQuadMesh;

	private Color ZoneOutlineColor = new Color(0.27f, 0.37f, 0.16f, 1f);

	private int lastSkinId = -1;

	private Vector3 _p0;

	private Vector3 _p1;

	private Vector3 _p2;

	private Vector3 _p3;

	private Vector3 _curPos;

	private readonly Vector2Int[] _camVerts = new Vector2Int[4];

	private Vector3 _lastCamPosition = Vector3.zero;

	private Rect _lastRect = Rect.zero;

	private HashSet<int> _BlackAreaPoints = new HashSet<int>();

	private HashSet<int> _BlackAreaZoneInit = new HashSet<int>();

	private Dictionary<int, int> _DynamicBlackArea;

	private HashSet<int> _greenTileChangeList;

	public Dictionary<int, WorldZone> WorldZones { get; private set; } = new Dictionary<int, WorldZone>();

	public Transform EdgeRoot { get; private set; }

	public Transform ZoneRoot { get; private set; }

	public Material GetEdgeMaterial(int matIdx, Color color)
	{
		Dictionary<Color, Material> dictionary = ((matIdx == 0) ? CacheEdgeMat1 : CacheEdgeMat2);
		if (dictionary.TryGetValue(color, out var value))
		{
			return value;
		}
		Material material = ((matIdx == 0) ? _edgeMaterial1 : _edgeMaterial2);
		if (material == null)
		{
			return null;
		}
		Material material2 = new Material(material)
		{
			name = Time.frameCount.ToString(),
			color = color
		};
		material2.renderQueue = 2011;
		dictionary.Add(color, material2);
		return material2;
	}

	public Material GetZoneMaterial(bool hasOwner)
	{
		if (hasOwner)
		{
			if (SeasonMapType == SeasonType.CityStronghold || SeasonMapType == SeasonType.Snow || SeasonMapType == SeasonType.Mummy || SeasonMapType == SeasonType.Darkness || SeasonMapType == SeasonType.NineNation)
			{
				Material material = ((SeasonMapMode == 2) ? _zoneMaterialOccupyS1_full : _zoneMaterialOccupyS1_single);
				if (material == null)
				{
					return null;
				}
				return new Material(material);
			}
			if (_zoneMaterialOccupy == null)
			{
				return null;
			}
			return new Material(_zoneMaterialOccupy);
		}
		if (_zoneMaterialFill == null)
		{
			return null;
		}
		Material material2 = new Material(_zoneMaterialFill);
		material2.SetColor("_OutlineColor", ZoneOutlineColor);
		return material2;
	}

	public WorldMapZoneManager(WorldScene scene)
		: base(scene)
	{
	}

	private static bool IsFuncOpen()
	{
		return GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.GetIsAllianceCityOpen");
	}

	public override void Init()
	{
		GameEntry.Event.Subscribe(EventId.WorldCityOwnerInfoReceived, OnCityOwnerInfoReceived);
		GameEntry.Event.Subscribe(EventId.WorldCityOwnerInfoChanged, OnCityOwnerInfoChanged);
		GameEntry.Event.Subscribe(EventId.ShowWorldZoneChangeColor, DoCityColorChange);
		GameEntry.Event.Subscribe(EventId.OnModeMenuShow, OnModeMenuShow);
		GameEntry.Event.Subscribe(EventId.OnModeMenuHide, OnModeMenuHide);
		GameEntry.Event.Subscribe(EventId.ZoneSkinColorSettingChanged, OnZoneSkinColorSettingChanged);
		world.AfterUpdate += OnCameraAfterUpdate;
		int num = GameEntry.Lua.CallWithReturnInt("CSharpCallLuaInterface.GetConfigNum", "NPC_city_range", "k2");
		if (num > 0)
		{
			allianceCityRange = num / 2;
		}
		_functionOn = IsFuncOpen();
		if (!_functionOn)
		{
			return;
		}
		_curServerId = GameEntry.Data.Player.GetCurServerId();
		_loadState = LoadState.StartLoad;
		LoadWorldCityColor();
		CreateBlackAreaQuad();
		LoadMapDataFromFile(delegate
		{
			if (_zoneMapData == null)
			{
				Debug.LogError("#WorldZone# _zoneMapData is null!");
			}
			else
			{
				Asset request = GameEntry.Resource.LoadAssetAsync("Assets/Main/Scenes/Zone/Prefab/ZoneMapRoot.prefab", typeof(GameObject));
				if (request == null)
				{
					Debug.LogError("#WorldZone# request is null!");
				}
				else
				{
					Asset asset = request;
					asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
					{
						if (request.isError)
						{
							Debug.LogFormat("#WorldZone#, Load ZoneMapPrefab Error! Error={0}", request.error);
						}
						else if (_loadState == LoadState.None)
						{
							request.Release();
						}
						else if (world == null || world.gameObject == null)
						{
							Log.Warning("WorldMapZoneManager world is null");
						}
						else
						{
							GameObject gameObject = UnityEngine.Object.Instantiate(request.asset as GameObject, world.Transform);
							_zoneMapRoot = gameObject.GetComponent<WorldZoneMapRoot>();
							_zoneMapRoot.transform.localScale = Vector3.one;
							ZoneRoot = _zoneMapRoot.transform.Find("ZoneRoot");
							EdgeRoot = _zoneMapRoot.transform.Find("EdgeRoot");
							SetZoneLineColorAndMap();
							adjuster = _zoneMapRoot.GetComponent<AutoAdjustLod>();
							if (adjuster != null)
							{
								adjuster.SetBeforeLodFadeCallback(OnBgFade);
							}
							Material[] materials = EdgeRoot.Find("MaterialHolder").GetComponent<MeshRenderer>().materials;
							_edgeMaterial1 = materials[0];
							_edgeMaterial2 = materials[1];
							if (materials.Length >= 6)
							{
								_zoneMaterialFill = materials[2];
								_zoneMaterialOccupy = materials[3];
								_zoneMaterialOccupyS1_full = materials[4];
								_zoneMaterialOccupyS1_single = materials[5];
							}
							Transform transform = ZoneRoot.Find("Collider");
							if (transform != null)
							{
								transform.gameObject.SetActive(value: true);
								transform.GetComponent<TouchObjectEventTrigger>().onPointerClick = OnBgClick;
							}
							Transform transform2 = ZoneRoot.Find("ColliderBig");
							if (transform2 != null)
							{
								SceneSkinMeta curSkinMeta2 = SceneSkinManager.Instance.GetCurSkinMeta();
								if (curSkinMeta2 != null && curSkinMeta2.IsNineNationMode())
								{
									transform2.gameObject.SetActive(value: true);
									transform2.GetComponent<TouchObjectEventTrigger>().onPointerClick = OnBgClick;
								}
								else
								{
									transform2.gameObject.SetActive(value: false);
								}
							}
							LoadZonePosFile(delegate
							{
								UpdateAllZoneOwner();
								OnCameraAfterUpdate();
							});
						}
					});
				}
			}
		});
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			LoadBlockAreaMaterials((SeasonType)curSkinMeta.seasonType);
			if (curSkinMeta.seasonType == 6)
			{
				LoadBlockAreaMaterials_S5();
			}
		}
	}

	public void LoadBlockAreaMaterials(SeasonType _seasonMapType)
	{
		string text = "Assets/Main/Material/blackearth/huitu_nsj.mat";
		if (_seasonMapType == SeasonType.Snow)
		{
			text = "Assets/Main/SeasonRes/Shared/Material/huitu_sj_1_snsw.mat";
		}
		else if (_seasonMapType == SeasonType.Mummy)
		{
			text = string.Empty;
		}
		else if (_seasonMapType == SeasonType.Darkness)
		{
			text = "Assets/Main/SeasonRes/Shared/Material/huitu_sj_4.mat";
		}
		else if (_seasonMapType == SeasonType.NineNation)
		{
			text = "Assets/Main/SeasonRes/Shared/Material/huitu_s5.mat";
		}
		if (text.IsNullOrEmpty())
		{
			return;
		}
		if (mBlackZoneMaterialAsset != null)
		{
			mBlackZoneMaterialAsset.Release();
		}
		mBlackZoneMaterialAsset = GameEntry.Resource.LoadAssetAsync(text, typeof(Material));
		if (mBlackZoneMaterialAsset == null)
		{
			Log.Info("MapZone Material request is null!");
			return;
		}
		Asset asset = mBlackZoneMaterialAsset;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (mBlackZoneMaterialAsset != null)
			{
				if (mBlackZoneMaterialAsset.isError)
				{
					Log.Info("MapZone Material request.isError! error:" + mBlackZoneMaterialAsset.error);
				}
				else if (mBlackZoneMaterialAsset.isDone && mBlackZoneMaterialAsset.asset is Material)
				{
					if (_seasonMapType == SeasonType.Snow)
					{
						_blackAreaSeasonSnow = mBlackZoneMaterialAsset.asset as Material;
					}
					else if (_seasonMapType != SeasonType.Mummy)
					{
						if (_seasonMapType == SeasonType.Darkness)
						{
							_blackAreaSeasonDark = mBlackZoneMaterialAsset.asset as Material;
						}
						else if (_seasonMapType == SeasonType.NineNation)
						{
							_blackAreaCenter_S5 = mBlackZoneMaterialAsset.asset as Material;
						}
						else
						{
							_blackArea = mBlackZoneMaterialAsset.asset as Material;
						}
					}
				}
			}
		});
	}

	public void LoadBlockAreaMaterials_S5()
	{
		string path = "Assets/Main/SeasonRes/Shared/Material/huitu_sj_5_hong.mat";
		string path2 = "Assets/Main/SeasonRes/Shared/Material/huitu_sj_5_lv.mat";
		string path3 = "Assets/Main/SeasonRes/Shared/Material/huitu_sj_5_huang.mat";
		if (mBlackZoneMaterialAsset_S5Dark != null)
		{
			mBlackZoneMaterialAsset_S5Dark.Release();
		}
		mBlackZoneMaterialAsset_S5Dark = GameEntry.Resource.LoadAssetAsync(path, typeof(Material));
		if (mBlackZoneMaterialAsset_S5Dark == null)
		{
			Log.Info("MapZone Material request is null!");
			return;
		}
		Asset asset = mBlackZoneMaterialAsset_S5Dark;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (mBlackZoneMaterialAsset_S5Dark != null)
			{
				if (mBlackZoneMaterialAsset_S5Dark.isError)
				{
					Log.Info("MapZone Material request.isError! error:" + mBlackZoneMaterialAsset_S5Dark.error);
				}
				else if (mBlackZoneMaterialAsset_S5Dark.isDone && mBlackZoneMaterialAsset_S5Dark.asset is Material)
				{
					_blackAreaMaterialDark_S5 = mBlackZoneMaterialAsset_S5Dark.asset as Material;
				}
			}
		});
		if (mBlackZoneMaterialAsset_S5Green != null)
		{
			mBlackZoneMaterialAsset_S5Green.Release();
		}
		mBlackZoneMaterialAsset_S5Green = GameEntry.Resource.LoadAssetAsync(path2, typeof(Material));
		if (mBlackZoneMaterialAsset_S5Green == null)
		{
			Log.Info("MapZone Material request is null!");
			return;
		}
		Asset asset2 = mBlackZoneMaterialAsset_S5Green;
		asset2.completed = (Action<Asset>)Delegate.Combine(asset2.completed, (Action<Asset>)delegate
		{
			if (mBlackZoneMaterialAsset_S5Green != null)
			{
				if (mBlackZoneMaterialAsset_S5Green.isError)
				{
					Log.Info("MapZone Material request.isError! error:" + mBlackZoneMaterialAsset_S5Green.error);
				}
				else if (mBlackZoneMaterialAsset_S5Green.isDone && mBlackZoneMaterialAsset_S5Green.asset is Material)
				{
					_blackAreaS5MaterialGreen_S5 = mBlackZoneMaterialAsset_S5Green.asset as Material;
				}
			}
		});
		if (mBlackZoneMaterialAsset_S5Light != null)
		{
			mBlackZoneMaterialAsset_S5Light.Release();
		}
		mBlackZoneMaterialAsset_S5Light = GameEntry.Resource.LoadAssetAsync(path3, typeof(Material));
		if (mBlackZoneMaterialAsset_S5Light == null)
		{
			Log.Info("MapZone Material request is null!");
			return;
		}
		Asset asset3 = mBlackZoneMaterialAsset_S5Light;
		asset3.completed = (Action<Asset>)Delegate.Combine(asset3.completed, (Action<Asset>)delegate
		{
			if (mBlackZoneMaterialAsset_S5Light != null)
			{
				if (mBlackZoneMaterialAsset_S5Light.isError)
				{
					Log.Info("MapZone Material request.isError! error:" + mBlackZoneMaterialAsset_S5Light.error);
				}
				else if (mBlackZoneMaterialAsset_S5Light.isDone && mBlackZoneMaterialAsset_S5Light.asset is Material)
				{
					_blackAreaS5MaterialLight_S5 = mBlackZoneMaterialAsset_S5Light.asset as Material;
				}
			}
		});
	}

	public override void UnInit()
	{
		_allianceCityLuaTable?.Dispose();
		GameEntry.Event.Unsubscribe(EventId.WorldCityOwnerInfoReceived, OnCityOwnerInfoReceived);
		GameEntry.Event.Unsubscribe(EventId.WorldCityOwnerInfoChanged, OnCityOwnerInfoChanged);
		GameEntry.Event.Unsubscribe(EventId.ShowWorldZoneChangeColor, DoCityColorChange);
		GameEntry.Event.Unsubscribe(EventId.OnModeMenuShow, OnModeMenuShow);
		GameEntry.Event.Unsubscribe(EventId.OnModeMenuHide, OnModeMenuHide);
		GameEntry.Event.Unsubscribe(EventId.ZoneSkinColorSettingChanged, OnZoneSkinColorSettingChanged);
		world.AfterUpdate -= OnCameraAfterUpdate;
		foreach (KeyValuePair<int, WorldZone> worldZone in WorldZones)
		{
			worldZone.Value.Destory();
		}
		WorldZones.Clear();
		if (mBlackZoneMaterialAsset != null)
		{
			mBlackZoneMaterialAsset.Release();
			mBlackZoneMaterialAsset = null;
		}
		if (mBlackZoneMaterialAsset_S5Light != null)
		{
			mBlackZoneMaterialAsset_S5Light.Release();
			mBlackZoneMaterialAsset_S5Light = null;
		}
		if (mBlackZoneMaterialAsset_S5Green != null)
		{
			mBlackZoneMaterialAsset_S5Green.Release();
			mBlackZoneMaterialAsset_S5Green = null;
		}
		if (mBlackZoneMaterialAsset_S5Dark != null)
		{
			mBlackZoneMaterialAsset_S5Dark.Release();
			mBlackZoneMaterialAsset_S5Dark = null;
		}
		if (bigMapRailway_S5 != null)
		{
			bigMapRailway_S5.Destroy();
			bigMapRailway_S5 = null;
		}
		_loadState = LoadState.None;
	}

	public bool IsInited()
	{
		return _loadState == LoadState.Loaded;
	}

	public void OnSkinChange()
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && curSkinMeta.id != lastSkinId)
		{
			lastSkinId = curSkinMeta.id;
			_loadState = LoadState.StartLoad;
			_lastClippedZones.Clear();
			_clippedZones.Clear();
			foreach (KeyValuePair<int, WorldZone> worldZone in WorldZones)
			{
				worldZone.Value.Reset();
			}
			WorldZones.Clear();
			LoadWorldCityColor();
			LoadMapDataFromFile(delegate
			{
				SetZoneLineColorAndMap();
				LoadZonePosFile(delegate
				{
					UpdateAllZoneOwner();
				});
			});
			LoadBlockAreaMaterials((SeasonType)curSkinMeta.seasonType);
			if (curSkinMeta.seasonType == 6)
			{
				LoadBlockAreaMaterials_S5();
			}
		}
		else
		{
			_loadState = LoadState.StartLoad;
			UpdateAllZoneOwner();
			_loadState = LoadState.Loaded;
		}
	}

	public void SetZoneLineColorAndMap()
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta == null)
		{
			return;
		}
		lastSkinId = curSkinMeta.id;
		SeasonMapMode = curSkinMeta.splash_fill_mode;
		SeasonMapType = (SeasonType)curSkinMeta.mapType;
		if (!curSkinMeta.world_zone_line.IsNullOrEmpty())
		{
			string[] array = curSkinMeta.world_zone_line.Split(new char[1] { ',' });
			if (array.Length == 4)
			{
				ZoneOutlineColor = new Color(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
			}
		}
		if (bigMapRailway_S5 != null)
		{
			bigMapRailway_S5.Destroy();
			bigMapRailway_S5 = null;
		}
		string world_map = curSkinMeta.world_map;
		if (!string.IsNullOrEmpty(world_map) && ZoneRoot != null)
		{
			SpriteRenderer spriteRenderer = ZoneRoot?.Find("Bg1")?.GetComponent<SpriteRenderer>();
			if (spriteRenderer != null)
			{
				if (curSkinMeta.IsNineNationMode())
				{
					spriteRenderer.transform.localPosition = new Vector3(58.5f, -58.5f, 0f);
					spriteRenderer.size = new Vector2(143f, 143f);
					spriteRenderer.LoadSprite(world_map);
					LoadBigMapRailway_S5(ZoneRoot);
				}
				else
				{
					spriteRenderer.transform.localPosition = new Vector3(19.5f, -19.5f, 0f);
					spriteRenderer.size = new Vector2(65f, 65f);
					spriteRenderer.LoadSprite(world_map);
				}
			}
		}
		_oasisViewScaleList = null;
	}

	public void LoadBigMapRailway_S5(Transform zoneRoot)
	{
		string text = "Assets/Main/SeasonRes/S5/Prefabs/World/RailwayWorld.prefab";
		if (bigMapRailway_S5 != null)
		{
			bigMapRailway_S5.Destroy();
			bigMapRailway_S5 = null;
		}
		if (!GameEntry.Resource.HasAsset(text))
		{
			Log.Error("RailwayWorld.prefab not exist: {0}", text);
		}
		else
		{
			if (bigMapRailway_S5 != null)
			{
				return;
			}
			bigMapRailway_S5 = GameEntry.Resource.InstantiateAsync(text);
			if (bigMapRailway_S5 == null)
			{
				return;
			}
			bigMapRailway_S5.completed += delegate
			{
				GameObject gameObject = bigMapRailway_S5.gameObject;
				if (gameObject == null)
				{
					Log.Error("s5 LoadBigMapRailway_S5 gameObject null");
				}
				else if (SceneManager.CurrSceneID != 2)
				{
					bigMapRailway_S5?.Destroy();
					bigMapRailway_S5 = null;
				}
				else if (zoneRoot != null)
				{
					gameObject.transform.parent = zoneRoot;
					gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
					gameObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
					gameObject.transform.localScale = new Vector3(0.0195f, -1f, 0.0195f);
				}
				else
				{
					bigMapRailway_S5?.Destroy();
					bigMapRailway_S5 = null;
				}
			};
		}
	}

	private void LoadWorldCityColor()
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		_worldCityColors = new Dictionary<int, WorldCityColor>();
		if (curSkinMeta != null && curSkinMeta.IsDesertMode())
		{
			_worldCityColorCount = 1;
			return;
		}
		string text = "worldcity_color";
		if (curSkinMeta != null && !curSkinMeta.world_city_color.IsNullOrEmpty())
		{
			text = curSkinMeta.world_city_color;
		}
		_worldCityColorCount = GameEntry.Lua.CallWithReturnInt("CSharpCallLuaInterface.GetDataTableCount", text);
		for (int i = 1; i <= _worldCityColorCount; i++)
		{
			WorldCityColor value = new WorldCityColor(i, text);
			_worldCityColors.Add(i, value);
		}
	}

	public void SetMapZoneActive(bool active)
	{
		isSetActive = active;
	}

	public int GetZoneIdByPosId(int pointId)
	{
		if (_zoneMapData != null)
		{
			WorldZoneData zoneByPosId = _zoneMapData.GetZoneByPosId(pointId);
			if (zoneByPosId != null)
			{
				return zoneByPosId.ZoneId;
			}
		}
		return 0;
	}

	public int GetZoneIdByWorldPos(Vector3 worldPos)
	{
		if (_zoneMapData != null)
		{
			WorldZoneData zoneIdByWorldPos = _zoneMapData.GetZoneIdByWorldPos(worldPos);
			if (zoneIdByWorldPos != null)
			{
				return zoneIdByWorldPos.ZoneId;
			}
		}
		return 0;
	}

	public bool IsPointInAllianceCity(int pointId, int serverId)
	{
		if (_zoneMapData == null)
		{
			return false;
		}
		Vector3 worldPos = TileCoord.TileIndexToWorld(pointId, ForceChangeScene.World, serverId);
		Vector2Int a = TileCoord.IndexToTilePos(_zoneMapData.GetZoneIdByWorldPos(worldPos).CityPos, new Vector2Int(((float)_zoneMapData.width * 1f).ToInt(), ((float)_zoneMapData.height * 1f).ToInt()));
		a.x %= 1000;
		a.y %= 1000;
		a += new Vector2Int(allianceCityCenterOffset, allianceCityCenterOffset);
		Vector2Int b = world.IndexToTilePos(pointId);
		return Vector2Int.Distance(a, b) <= (float)allianceCityRange;
	}

	public WorldZoneData GetZoneData(int zoneId)
	{
		return _zoneMapData?.GetZoneData(zoneId);
	}

	public WorldCityColor GetWorldCityColor(int index)
	{
		if (!_worldCityColors.TryGetValue(index, out var value))
		{
			return null;
		}
		return value;
	}

	private void LoadZonePosFile(Action callback)
	{
		zonePos.Clear();
		string path = "Assets/Main/Scenes/Zone/pos.bytes";
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			lastSkinId = curSkinMeta.id;
			if (curSkinMeta.IsDesertMode())
			{
				path = "Assets/Main/Scenes/Zone/pos.bytes";
			}
			else if (curSkinMeta.IsCityStrongholdMode())
			{
				path = "Assets/Main/SeasonRes/S1/Scenes/pos_S1.bytes";
				if (!GameEntry.Resource.HasAsset(path))
				{
					path = "Assets/Main/Scenes/Zone/pos_S1.bytes";
				}
			}
			else if (curSkinMeta.IsSnowMode())
			{
				path = "Assets/Main/SeasonRes/S2/Scenes/pos_S2.bytes";
				if (!GameEntry.Resource.HasAsset(path))
				{
					path = "Assets/Main/Scenes/Zone/pos_S2.bytes";
				}
			}
			else if (curSkinMeta.IsMummyMode())
			{
				path = "Assets/Main/SeasonRes/S3/Scenes/Zone/pos_S3.bytes";
			}
			else if (curSkinMeta.IsDarknessMode())
			{
				path = "Assets/Main/SeasonRes/S4/Scenes/Zone/pos_S4.bytes";
			}
			else if (curSkinMeta.IsNineNationMode())
			{
				path = "Assets/Main/SeasonRes/S5/Scenes/Zone/pos_S5.bytes";
			}
		}
		else
		{
			path = "Assets/Main/Scenes/Zone/pos.bytes";
			_loadState = LoadState.Loaded;
			lastSkinId = 1;
		}
		if (!GameEntry.Resource.HasAsset(path))
		{
			return;
		}
		Asset request = GameEntry.Resource.LoadAssetAsync(path, typeof(TextAsset));
		if (request == null)
		{
			Log.Info("LoadZonePosFile request is null!");
			return;
		}
		Asset asset = request;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (request.isError)
			{
				Log.Info("LoadZonePosFile request.isError! error:" + request.error);
			}
			else if (request.isDone)
			{
				if (request.asset is TextAsset asset2)
				{
					ParseWorldMapZonePos(asset2);
				}
				_loadState = LoadState.Loaded;
				request.Release();
				callback?.Invoke();
			}
		});
	}

	public void ParseWorldMapZonePos(TextAsset asset)
	{
		using MemoryStream memoryStream = new MemoryStream(asset.bytes);
		BinaryReader binaryReader = new BinaryReader(memoryStream);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		ushort num = binaryReader.ReadUInt16();
		zonePos.Clear();
		for (int i = 0; i < num; i++)
		{
			ushort key = binaryReader.ReadUInt16();
			short num2 = binaryReader.ReadInt16();
			short num3 = binaryReader.ReadInt16();
			short num4 = binaryReader.ReadInt16();
			short num5 = binaryReader.ReadInt16();
			zonePos.Add(key, new Vector4(num2, num3, num4, num5));
		}
		binaryReader.Close();
	}

	public Vector4 GetZoneRect(int zoneIndex)
	{
		if (!zonePos.TryGetValue(zoneIndex, out var value))
		{
			return Vector4.zero;
		}
		return value;
	}

	private void LoadMapDataFromFile(Action callback)
	{
		SeasonType type = SeasonType.Nothing;
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			type = curSkinMeta.GetMapType();
		}
		InitBlackArea();
		WorldZoneEdgeDataCache.PreLoadZoneEdgeData(curSkinMeta);
		WorldZoneMapData.TryLoadWorldZoneMapData(type, async: true, delegate(WorldZoneMapData data)
		{
			SceneSkinMeta curSkinMeta2 = SceneSkinManager.Instance.GetCurSkinMeta();
			if ((curSkinMeta2 == null || curSkinMeta2.GetMapType() == type) && data != null)
			{
				_zoneMapData = data;
				_findNode = new WorldZoneTreeNode(new RectInt(0, 0, _zoneMapData.width, _zoneMapData.height));
				_findNode.CrateSubNodes(1);
				foreach (WorldZoneData value in _zoneMapData.zones.Values)
				{
					WorldZone worldZone = new WorldZone(this, value, type);
					WorldZones[worldZone.index] = worldZone;
					_findNode.AddZone(worldZone);
				}
				callback?.Invoke();
			}
		});
	}

	private void OnCameraAfterUpdate()
	{
		if (!IsInited())
		{
			return;
		}
		if (_zoneMapRoot != null && isCurActive != isSetActive)
		{
			isCurActive = isSetActive;
			_zoneMapRoot.gameObject.SetActive(isCurActive);
		}
		if (isCurActive)
		{
			if (adjuster != null)
			{
				isEdgeMode = adjuster.IsMainShow();
			}
			OnClip();
		}
	}

	private void OnBgFade(bool isFadeIn, bool isMain)
	{
		if (isMain)
		{
			bool flag = adjuster.IsMainShow();
			if (currentShowBg != !flag)
			{
				currentShowBg = !flag;
				GameEntry.Event.Fire(EventId.OnWorldSceneShowBg, currentShowBg);
			}
		}
	}

	private void OnBgClick()
	{
		Vector3 touchPoint = world.GetTouchPoint();
		Vector2Int tilePos = new Vector2Int((int)(touchPoint.x / 2f), (int)(touchPoint.z / 2f));
		TileCoord.ClampTilePos(ref tilePos, world.WorldSize);
		world.AutoLookat(TileCoord.TileFloatToWorld(tilePos, 0), world.InitZoom);
	}

	private void OnClip(object sender = null, GameEventArgs e = null)
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		WorldCamera camera = world.Camera;
		float num = camera.GetRotation().eulerAngles.x % 360f;
		if (num < 30f || num > 95f || curSkinMeta == null)
		{
			return;
		}
		_curPos = camera.GetPosition();
		if (curServerId != _curServerId && !curSkinMeta.IsNineNationMode())
		{
			_curServerId = curServerId;
			if (_clippedZones.Count > 0)
			{
				foreach (WorldZone clippedZone in _clippedZones)
				{
					clippedZone.findState = 0;
				}
				_clippedZones.Clear();
			}
		}
		else if (_lastCamPosition == _curPos)
		{
			return;
		}
		_lastCamPosition = _curPos;
		_p0 = camera.GetRaycastGroundPoint(new Vector3(0f, Screen.height));
		_p1 = new Vector3(_curPos.x + _curPos.x - _p0.x, 0f, _p0.z);
		_p2 = camera.GetRaycastGroundPoint(new Vector3(0f, 0f));
		_p3 = new Vector3(_curPos.x + (_curPos.x - _p2.x), 0f, _p2.z);
		_lastRect = new Rect(_p0.x, _p0.z, _p1.x - _p0.x, _p2.z - _p0.z);
		_camVerts[0] = new Vector2Int((int)(_p0.x / 2f), (int)(_p0.z / 2f));
		_camVerts[1] = new Vector2Int((int)(_p1.x / 2f), (int)(_p1.z / 2f));
		_camVerts[2] = new Vector2Int((int)(_p3.x / 2f), (int)(_p3.z / 2f));
		_camVerts[3] = new Vector2Int((int)(_p2.x / 2f), (int)(_p2.z / 2f));
		RectInt rect = new RectInt(Mathf.CeilToInt((float)_camVerts[0].x / 1f), Mathf.CeilToInt((float)_camVerts[0].y / 1f), Mathf.CeilToInt((float)(_camVerts[1].x - _camVerts[0].x) / 1f), Mathf.CeilToInt((float)(_camVerts[3].y - _camVerts[0].y) / 1f));
		DoClipZones(rect);
	}

	private void CalcClipData(RectInt rect, ref List<WorldZone> zones)
	{
		if (_loadState == LoadState.Loaded && _findNode != null)
		{
			int count = 0;
			zones.Clear();
			_findNode.FindZone(rect, ref zones, ref count);
		}
	}

	private void DoClipZones(RectInt rect)
	{
		_lastClippedZones.Clear();
		if (_clippedZones.Count > 0)
		{
			_lastClippedZones.AddRange(_clippedZones);
			foreach (WorldZone lastClippedZone in _lastClippedZones)
			{
				lastClippedZone.findState = -1;
			}
		}
		CalcClipData(rect, ref _clippedZones);
		foreach (WorldZone lastClippedZone2 in _lastClippedZones)
		{
			if (lastClippedZone2.findState < 0)
			{
				lastClippedZone2.findState = 0;
				GameEntry.Event.Fire(EventId.AllianceCityOutView, (int)lastClippedZone2.data.ZoneId);
			}
		}
		foreach (WorldZone clippedZone in _clippedZones)
		{
			if (clippedZone.findState > 0)
			{
				clippedZone.findState = 0;
				GameEntry.Event.Fire(EventId.AllianceCityInView, (int)clippedZone.data.ZoneId);
			}
			clippedZone.finded = false;
		}
	}

	private List<int> UpdateAllZoneOwner()
	{
		List<int> list = new List<int>();
		if (!IsInited())
		{
			_loadState = LoadState.Update;
			return list;
		}
		_allianceCityLuaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllianceCityList");
		foreach (WorldZoneData value in _zoneMapData.zones.Values)
		{
			string allianceId = value.AllianceId;
			if (!GetZoneOwnerAllianceInfo(value.ZoneId, out var serverId, out var allianceId2, out var color))
			{
				value.ServerId = 0;
				value.AllianceId = string.Empty;
				value.color = -1;
				if (!allianceId.Equals(value.AllianceId))
				{
					list.Add(value.ZoneId);
				}
				continue;
			}
			value.AllianceId = allianceId2;
			value.ServerId = serverId;
			if (_worldCityColorCount == 0)
			{
				value.color = 1;
			}
			else
			{
				value.color = color % _worldCityColorCount + 1;
			}
			if (!allianceId.Equals(value.AllianceId))
			{
				list.Add(value.ZoneId);
			}
		}
		return list;
	}

	private WorldZone GetWorldZoneInfoById(int zoneId)
	{
		if (WorldZones.TryGetValue(zoneId, out var value))
		{
			return value;
		}
		return null;
	}

	private bool GetZoneOwnerAllianceInfo(int zoneId, out int serverId, out string allianceId, out int color)
	{
		allianceId = string.Empty;
		color = 0;
		serverId = 0;
		if (_allianceCityLuaTable == null || !_allianceCityLuaTable.ContainsKey(zoneId))
		{
			return false;
		}
		LuaTable luaTable = _allianceCityLuaTable.Get<LuaTable>(zoneId);
		if (luaTable == null)
		{
			Debug.LogError("GetZoneOwnerAllianceInfo Lua func return invalid format !!!");
			return false;
		}
		allianceId = luaTable.Get<string>("allianceId");
		color = luaTable.Get<int>("color");
		serverId = luaTable.Get<int>("occupyServerId");
		return true;
	}

	private void OnCityOwnerInfoReceived(object o)
	{
		OnCityOwnerInfoChanged();
	}

	private void OnCityOwnerInfoChanged(object o = null)
	{
		if (!_functionOn)
		{
			return;
		}
		List<int> list = UpdateAllZoneOwner();
		for (int i = 0; i < list.Count; i++)
		{
			WorldZone worldZoneInfoById = GetWorldZoneInfoById(list[i]);
			if (worldZoneInfoById != null)
			{
				WorldCityColor worldCityColor = GetWorldCityColor(worldZoneInfoById.data.color);
				worldZoneInfoById?.SetZoneRenderParam(worldCityColor, worldZoneInfoById.data.ServerId, skipSkinCheck: false);
				worldZoneInfoById.SetEdgeMaterial((worldCityColor != null) ? 1 : 0, worldCityColor?.outlineColor ?? Color.white);
			}
		}
	}

	private void DoCityColorChange(object o)
	{
		if (_functionOn && o != null && o is string text)
		{
			string[] array = text.Split(new char[1] { ';' });
			if (array.Length >= 3)
			{
				int zoneId = array[0].ToInt();
				int beforeColorIndex = array[1].ToInt() % _worldCityColorCount + 1;
				int afterColorIndex = array[2].ToInt() % _worldCityColorCount + 1;
				WorldZone worldZoneInfoById = GetWorldZoneInfoById(zoneId);
				if (worldZoneInfoById != null)
				{
					worldZoneInfoById.SetZoneChangeColor(beforeColorIndex, afterColorIndex);
					return;
				}
			}
		}
		GameEntry.Event.Fire(EventId.UINoInput, 3);
	}

	private void OnZoneSkinColorSettingChanged(object o)
	{
		string text = "season_map_zone_mode";
		if (GameEntry.Setting.GetPrivateBool(text, defaultValue: false))
		{
			SeasonDataManager instance = SeasonDataManager.Instance;
			string data = instance.GetData(text, "k2", string.Empty);
			string data2 = instance.GetData(text, "k3", string.Empty);
			string data3 = instance.GetData(text, "k4", string.Empty);
			string data4 = instance.GetData(text, "k5", string.Empty);
			{
				foreach (WorldZoneData value in _zoneMapData.zones.Values)
				{
					WorldZone worldZoneInfoById = GetWorldZoneInfoById(value.ZoneId);
					if (worldZoneInfoById != null)
					{
						worldZoneInfoById.ShowMode = ZoneShowMode.Normal;
						worldZoneInfoById.UpdateRenderParam(data, data2, data3, data4);
					}
				}
				return;
			}
		}
		foreach (WorldZoneData value2 in _zoneMapData.zones.Values)
		{
			WorldZone worldZoneInfoById2 = GetWorldZoneInfoById(value2.ZoneId);
			if (worldZoneInfoById2 != null)
			{
				worldZoneInfoById2.ShowMode = ZoneShowMode.Normal;
				int serverId = worldZoneInfoById2.data.ServerId;
				WorldCityColor worldCityColor = GetWorldCityColor(worldZoneInfoById2.data.color);
				worldZoneInfoById2.SetZoneRenderParam(worldCityColor, serverId, skipSkinCheck: true);
				worldZoneInfoById2.SetEdgeMaterial((worldCityColor != null) ? 1 : 0, worldCityColor?.outlineColor ?? Color.white);
			}
		}
	}

	private void OnModeMenuShow(object o)
	{
		if (!_functionOn)
		{
			return;
		}
		if (!IsInited())
		{
			_loadState = LoadState.Update;
			return;
		}
		foreach (WorldZoneData value in _zoneMapData.zones.Values)
		{
			WorldZone worldZoneInfoById = GetWorldZoneInfoById(value.ZoneId);
			if (worldZoneInfoById != null)
			{
				worldZoneInfoById.ShowMode = ZoneShowMode.Green;
				WorldCityColor worldCityColor = GetWorldCityColor(worldZoneInfoById.data.color);
				worldZoneInfoById.SetZoneRenderParam(worldCityColor, worldZoneInfoById.data.ServerId, skipSkinCheck: true);
				worldZoneInfoById.SetEdgeMaterial((worldCityColor != null) ? 1 : 0, worldCityColor?.outlineColor ?? Color.white);
			}
		}
	}

	private void OnModeMenuHide(object o)
	{
		foreach (WorldZoneData value in _zoneMapData.zones.Values)
		{
			WorldZone worldZoneInfoById = GetWorldZoneInfoById(value.ZoneId);
			if (worldZoneInfoById != null)
			{
				worldZoneInfoById.ShowMode = ZoneShowMode.Normal;
				WorldCityColor worldCityColor = GetWorldCityColor(worldZoneInfoById.data.color);
				worldZoneInfoById.SetZoneRenderParam(worldCityColor, worldZoneInfoById.data.ServerId, skipSkinCheck: true);
				worldZoneInfoById.SetEdgeMaterial((worldCityColor != null) ? 1 : 0, worldCityColor?.outlineColor ?? Color.white);
			}
		}
	}

	public OasisViewScale GetOasisViewScale(int rate)
	{
		if (_oasisViewScaleList == null)
		{
			_oasisViewScaleList = new List<OasisViewScale>();
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetOasisViewScaleList");
			if (luaTable != null)
			{
				for (int i = 1; i <= luaTable.Length; i++)
				{
					_oasisViewScaleList.Add(new OasisViewScale(luaTable.Get<LuaTable>(i)));
				}
				luaTable.Dispose();
			}
		}
		foreach (OasisViewScale oasisViewScale in _oasisViewScaleList)
		{
			if (oasisViewScale.InRange(rate))
			{
				return oasisViewScale;
			}
		}
		return null;
	}

	public Texture2D[] GetTextures()
	{
		return _zoneMapRoot.splashTextures;
	}

	private void CreateBlackAreaQuad()
	{
		if (!(mBlackZoneQuadMesh != null))
		{
			mBlackZoneQuadMesh = new Mesh
			{
				vertices = new Vector3[4]
				{
					new Vector3(-1f, -1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(1f, 1f, 0f),
					new Vector3(1f, -1f, 0f)
				},
				triangles = new int[6] { 0, 1, 2, 2, 3, 0 },
				uv = new Vector2[4]
				{
					new Vector2(0f, 1f),
					new Vector2(0f, 0f),
					new Vector2(1f, 0f),
					new Vector2(1f, 1f)
				}
			};
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		if (!isCurActive || !isSetActive || _loadState != LoadState.Loaded)
		{
			return;
		}
		foreach (WorldZone clippedZone in _clippedZones)
		{
			clippedZone.OnUpdate(isEdgeMode);
		}
		if (!isEdgeMode || mBlackZoneQuadMesh == null)
		{
			return;
		}
		if (SeasonMapType == SeasonType.Snow && _blackAreaSeasonSnow != null)
		{
			foreach (WorldZone clippedZone2 in _clippedZones)
			{
				clippedZone2.DrawBlackArea(mBlackZoneQuadMesh, _blackAreaSeasonSnow);
			}
			return;
		}
		if (SeasonMapType == SeasonType.Mummy)
		{
			return;
		}
		if (SeasonMapType == SeasonType.Darkness && _blackAreaSeasonDark != null)
		{
			foreach (WorldZone clippedZone3 in _clippedZones)
			{
				clippedZone3.DrawBlackArea(mBlackZoneQuadMesh, _blackAreaSeasonDark);
			}
			return;
		}
		if (SeasonMapType == SeasonType.NineNation)
		{
			foreach (WorldZone clippedZone4 in _clippedZones)
			{
				int index = clippedZone4.index;
				if (index != 321 && index != 332 && index != 343 && index != 984 && index != 995 && index != 1006 && index != 1647 && index != 1658 && index != 1669)
				{
					if (clippedZone4.mapIndex == 5)
					{
						if (clippedZone4.mMaterial == null)
						{
							clippedZone4.mMaterial = _blackAreaCenter_S5;
						}
						clippedZone4.DrawBlackArea(mBlackZoneQuadMesh, _blackAreaCenter_S5);
					}
					else
					{
						if (clippedZone4.mMaterial == null)
						{
							Material blackZoneMat_S = GetBlackZoneMat_S5(clippedZone4.mapIndex, clippedZone4.data);
							clippedZone4.mMaterial = blackZoneMat_S;
						}
						if (clippedZone4.mMaterial != null)
						{
							clippedZone4.DrawBlackArea(mBlackZoneQuadMesh, clippedZone4.mMaterial);
						}
					}
				}
			}
			return;
		}
		if (!(_blackArea != null))
		{
			return;
		}
		foreach (WorldZone clippedZone5 in _clippedZones)
		{
			clippedZone5.DrawBlackArea(mBlackZoneQuadMesh, _blackArea);
		}
	}

	public Material GetBlackZoneMat_S5(int serverIndex, WorldZoneData zondMapData)
	{
		Vector2Int cityPosTile = TileCoord.IndexToTilePos(zondMapData.CityPos, new Vector2Int(3000, 3000));
		Material material = null;
		if (IsInGreenArea(serverIndex, cityPosTile))
		{
			return _blackAreaS5MaterialGreen_S5;
		}
		if (serverIndex == 1 || serverIndex == 3 || serverIndex == 7 || serverIndex == 9)
		{
			return _blackAreaMaterialDark_S5;
		}
		return _blackAreaS5MaterialLight_S5;
	}

	public bool IsInGreenArea(int serverIndex, Vector2Int cityPosTile)
	{
		int num = cityPosTile.x % 1000;
		int num2 = cityPosTile.y % 1000;
		switch (serverIndex)
		{
		case 1:
			if (num < 656 && num2 < 656 && num > 0 && num2 > 0)
			{
				return true;
			}
			return false;
		case 2:
			if (num < 825 && num2 < 625 && num > 172 && num2 > 0)
			{
				return true;
			}
			return false;
		case 3:
			if (num < 656 && num2 < 999 && num > 342 && num2 > 0)
			{
				return true;
			}
			return false;
		case 4:
			if (num < 625 && num2 < 825 && num > 0 && num2 > 175)
			{
				return true;
			}
			return false;
		case 6:
			if (num < 999 && num2 < 825 && num > 338 && num2 > 175)
			{
				return true;
			}
			return false;
		case 7:
			if (num < 665 && num2 < 999 && num > 0 && num2 > 330)
			{
				return true;
			}
			return false;
		case 8:
			if (num < 824 && num2 < 999 && num > 175 && num2 > 335)
			{
				return true;
			}
			return false;
		case 9:
			if (num < 999 && num2 < 999 && num > 336 && num2 > 336)
			{
				return true;
			}
			return false;
		default:
			return false;
		}
	}

	private void InitBlackArea()
	{
		_BlackAreaPoints.Clear();
		_BlackAreaZoneInit.Clear();
		_DynamicBlackArea?.Clear();
		List<int> list = GameEntry.Lua.CallWithReturn<List<int>>("CSharpCallLuaInterface.GetBlackLandRange");
		if (list == null || list.Count < 4)
		{
			return;
		}
		for (int i = list[0]; i <= list[1]; i++)
		{
			for (int j = list[2]; j <= list[3]; j++)
			{
				_BlackAreaPoints.Add(world.TilePosToIndex(new Vector2Int(i, j)));
			}
		}
	}

	public bool IsInBlackArea(int pointId)
	{
		if (_BlackAreaPoints.Contains(pointId))
		{
			return true;
		}
		if (_DynamicBlackArea != null && _DynamicBlackArea.TryGetValue(pointId, out var value) && value > 0)
		{
			return true;
		}
		return IsInAllianceCityBlackArea(pointId);
	}

	public bool IsInAllianceCityBlackArea(int pointId)
	{
		if (_zoneMapData == null)
		{
			return false;
		}
		int zoneIdByPosId = GetZoneIdByPosId(pointId);
		if (!_BlackAreaZoneInit.Contains(zoneIdByPosId))
		{
			WorldZone worldZoneInfoById = GetWorldZoneInfoById(zoneIdByPosId);
			if (worldZoneInfoById == null)
			{
				return false;
			}
			int cityField = worldZoneInfoById.GetCityField();
			Vector2Int vector2Int = TileCoord.IndexToTilePos(_zoneMapData.GetZoneData(zoneIdByPosId).CityPos, new Vector2Int(((float)_zoneMapData.width * 1f).ToInt(), ((float)_zoneMapData.height * 1f).ToInt()));
			for (int i = vector2Int.x - cityField + 1; i < vector2Int.x + cityField; i++)
			{
				for (int j = vector2Int.y - cityField + 1; j < vector2Int.y + cityField; j++)
				{
					_BlackAreaPoints.Add(world.TilePosToIndex(new Vector2Int(i, j)));
				}
			}
			_BlackAreaZoneInit.Add(zoneIdByPosId);
		}
		return _BlackAreaPoints.Contains(pointId);
	}

	public void ShowBlackArea(int point, int tileWidth, int tileHeight)
	{
		if (tileWidth <= 0 || tileHeight <= 0)
		{
			return;
		}
		if (_DynamicBlackArea == null)
		{
			_DynamicBlackArea = new Dictionary<int, int>();
		}
		if (_greenTileChangeList == null)
		{
			_greenTileChangeList = new HashSet<int>();
		}
		else
		{
			_greenTileChangeList.Clear();
		}
		Vector2Int vector2Int = world.IndexToTilePos(point);
		int num = vector2Int.x - tileWidth;
		int num2 = vector2Int.x + tileWidth;
		int num3 = vector2Int.y - tileHeight;
		int num4 = vector2Int.y + tileHeight;
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				int num5 = world.TilePosToIndex(new Vector2Int(i, j));
				if (_DynamicBlackArea.TryGetValue(num5, out var value))
				{
					_DynamicBlackArea[num5] = value + 1;
				}
				else
				{
					_DynamicBlackArea[num5] = 1;
				}
				_greenTileChangeList.Add(num5);
			}
		}
		world.GreenAreaChange(WorldAreaGreenInfo.GreenType.Green, _greenTileChangeList);
		world.UpdateGreenArea(num, num3, num2, num4);
	}

	public void HideBlackArea(int point, int tileWidth, int tileHeight)
	{
		if (_DynamicBlackArea == null || tileWidth <= 0 || tileHeight <= 0)
		{
			return;
		}
		if (_greenTileChangeList == null)
		{
			_greenTileChangeList = new HashSet<int>();
		}
		else
		{
			_greenTileChangeList.Clear();
		}
		Vector2Int vector2Int = world.IndexToTilePos(point);
		int num = vector2Int.x - tileWidth;
		int num2 = vector2Int.x + tileWidth;
		int num3 = vector2Int.y - tileHeight;
		int num4 = vector2Int.y + tileHeight;
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				int num5 = world.TilePosToIndex(new Vector2Int(i, j));
				if (_DynamicBlackArea.TryGetValue(num5, out var value))
				{
					if (value > 1)
					{
						_DynamicBlackArea[num5] = value - 1;
					}
					else
					{
						_DynamicBlackArea.Remove(num5);
					}
					_greenTileChangeList.Add(num5);
				}
			}
		}
		world.GreenAreaChange(WorldAreaGreenInfo.GreenType.Green, _greenTileChangeList);
		world.UpdateGreenArea(num, num3, num2, num4);
	}
}
