using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using VEngine;

public class WorldZone
{
	public int index;

	public int mapIndex = 1;

	public RectInt rect;

	public WorldZoneData data;

	public bool zone_loaded;

	public bool edge_loaded;

	public bool finded;

	public int findState;

	private SeasonType seasonType;

	public ZoneShowMode ShowMode;

	private Mesh baMesh;

	private Matrix4x4 baMatrix;

	private bool mIsInitPosition;

	private int m_ZoneField = -1;

	private int[] m_ZoneFieldArray;

	private Vector2Int m_ZoneFieldPos = Vector2Int.zero;

	private readonly WorldMapZoneManager mapManager;

	private Dictionary<int, Mesh> edgeEntities;

	private static readonly int SolidOutline = Shader.PropertyToID("_SolidOutline");

	private static readonly int SplashTex = Shader.PropertyToID("_SplashTex");

	private static readonly int OutLine_Color = Shader.PropertyToID("_OutlineColor");

	private static readonly int Glow_Color = Shader.PropertyToID("_GlowColor");

	private static readonly int MainTex = Shader.PropertyToID("_MainTex");

	public Material mMaterial;

	private int draw_layer;

	private Matrix4x4 edge_matrix;

	private Material edge_material;

	private Matrix4x4 zone_matrix;

	private Material zone_material;

	private Mesh zone_mesh;

	private Asset zone_asset;

	private static Dictionary<string, WorldCityColor> skinColorDict = new Dictionary<string, WorldCityColor>();

	public WorldZone(WorldMapZoneManager manager, WorldZoneData info, SeasonType theSeasonType)
	{
		seasonType = theSeasonType;
		mapManager = manager;
		index = info.ZoneId;
		rect = new RectInt(info.X, info.Y, info.W, info.H);
		data = info;
		draw_layer = LayerMask.NameToLayer("Default");
		Reset();
	}

	public void Reset()
	{
		m_ZoneField = -1;
		m_ZoneFieldArray = null;
		zone_loaded = false;
		edge_loaded = false;
		mIsInitPosition = false;
		finded = false;
		if (zone_asset != null)
		{
			zone_asset.Release();
			zone_asset = null;
		}
	}

	public void Destory()
	{
		data = null;
		Reset();
	}

	private void CreateEdge()
	{
		WorldZoneData worldZoneData = data;
		if (worldZoneData == null)
		{
			return;
		}
		Color color = Color.white;
		int matIdx = 0;
		WorldCityColor worldCityColor = mapManager.GetWorldCityColor(worldZoneData.color);
		if (worldCityColor != null)
		{
			matIdx = 1;
			color = worldCityColor.outlineColor;
		}
		if (seasonType == SeasonType.NineNation)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("season_city_s5", worldZoneData.ZoneId, "zoneId");
			if (!templateData.IsNullOrEmpty())
			{
				mapIndex = templateData.ToInt();
			}
		}
		edge_material = mapManager.GetEdgeMaterial(matIdx, color);
		if (edge_material == null)
		{
			return;
		}
		InitBlackAreaConfig();
		edgeEntities = WorldZoneEdgeDataCache.GetEdgeList(seasonType, worldZoneData);
		if (edgeEntities != null && edgeEntities.Count > 0)
		{
			if (seasonType == SeasonType.NineNation)
			{
				Vector2Int vector2Int = new Vector2Int(worldZoneData.CityPos % 3000, worldZoneData.CityPos / 3000);
				Vector3 pos = new Vector3(vector2Int.x * 2 - 2, 0f, vector2Int.y * 2);
				edge_matrix = Matrix4x4.TRS(pos, Quaternion.Euler(Vector3.zero), Vector3.one * 2f);
			}
			else if (seasonType == SeasonType.Darkness || seasonType == SeasonType.Mummy || seasonType == SeasonType.Snow || seasonType == SeasonType.CityStronghold)
			{
				Vector3 vector = TileCoord.TileIndexToWorld(worldZoneData.CityPos, ForceChangeScene.World, 0);
				edge_matrix = Matrix4x4.TRS(vector - new Vector3(1f, 0f, 1f), Quaternion.Euler(Vector3.zero), Vector3.one * 2f);
			}
			else
			{
				edge_matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(Vector3.zero), Vector3.one * 2f);
			}
			edge_loaded = true;
		}
	}

	private void CreateZone()
	{
		WorldZoneData worldZoneData = data;
		if (seasonType != SeasonType.Nothing)
		{
			string text = null;
			if (seasonType == SeasonType.CityStronghold)
			{
				text = "Assets/Main/SeasonRes/S1/Scenes/Image_S1/king.png";
				int[] source = new int[22]
				{
					10, 12, 14, 155, 16, 18, 2, 20, 222, 367,
					368, 369, 370, 371, 372, 373, 374, 375, 376, 4,
					6, 8
				};
				if (worldZoneData.ZoneId != 48)
				{
					text = ((!source.Contains(worldZoneData.ZoneId)) ? "Assets/Main/SeasonRes/S1/Scenes/Image_S1/216.png" : "Assets/Main/SeasonRes/S1/Scenes/Image_S1/316.png");
				}
				if (!GameEntry.Resource.HasAsset(text))
				{
					text = text.Replace("Assets/Main/SeasonRes/S1/Scenes/", "Assets/Main/Scenes/Zone/");
				}
			}
			else if (seasonType == SeasonType.Snow)
			{
				text = "Assets/Main/SeasonRes/S2/Scenes/Image_S2/king.png";
				if (worldZoneData.ZoneId != 48)
				{
					text = ((data.W == data.H) ? "Assets/Main/SeasonRes/S2/Scenes/Image_S2/316316.png" : ((data.W >= data.H) ? "Assets/Main/SeasonRes/S2/Scenes/Image_S2/416316.png" : "Assets/Main/SeasonRes/S2/Scenes/Image_S2/316416.png"));
				}
				if (!GameEntry.Resource.HasAsset(text))
				{
					text = text.Replace("Assets/Main/SeasonRes/S2/Scenes/", "Assets/Main/Scenes/Zone/");
				}
			}
			else if (seasonType == SeasonType.Darkness)
			{
				string zoneImageName = WorldZoneEdgeDataCache.GetZoneImageName(seasonType, worldZoneData);
				if (zoneImageName == null)
				{
					return;
				}
				text = "Assets/Main/SeasonRes/S4/Scenes/Zone/Image_S4/zone_" + zoneImageName + ".png";
			}
			else if (seasonType == SeasonType.Mummy)
			{
				string zoneImageName2 = WorldZoneEdgeDataCache.GetZoneImageName(seasonType, worldZoneData);
				if (zoneImageName2 == null)
				{
					return;
				}
				text = "Assets/Main/SeasonRes/S3/Scenes/Zone/Image_S3/zone_" + zoneImageName2 + ".png";
			}
			else if (seasonType == SeasonType.NineNation)
			{
				string templateData = GameEntry.ConfigCache.GetTemplateData("season_city_s5", worldZoneData.ZoneId, "zoneId");
				if (!templateData.IsNullOrEmpty())
				{
					mapIndex = templateData.ToInt();
				}
				string zoneImageName3 = WorldZoneEdgeDataCache.GetZoneImageName(seasonType, worldZoneData);
				if (zoneImageName3 == null)
				{
					return;
				}
				text = "Assets/Main/SeasonRes/S5/Scenes/Zone/Image_S5/zone_" + zoneImageName3 + ".png";
			}
			zone_asset = GameEntry.Resource.LoadAssetAsync(text, typeof(Texture));
		}
		else
		{
			string path = $"Assets/Main/Scenes/Zone/Image/zone_{worldZoneData.ZoneId}.png";
			zone_asset = GameEntry.Resource.LoadAssetAsync(path, typeof(Texture));
		}
		zone_matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(Vector3.zero), Vector3.one * 0.5128205f);
		Asset asset = zone_asset;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			UpdateTexture();
		});
		zone_loaded = true;
		InitBlackAreaConfig();
		SetZoneRenderParam(mapManager.GetWorldCityColor(worldZoneData.color), worldZoneData.ServerId, skipSkinCheck: false);
	}

	public void SetEdgeMaterial(int matIdx, Color color)
	{
		if (data != null)
		{
			edge_material = mapManager.GetEdgeMaterial(matIdx, color);
		}
	}

	private static void ParseSkinColor(string skinColorStr, Material material)
	{
		if (skinColorStr.IsNullOrEmpty())
		{
			return;
		}
		string[] array = skinColorStr.Split(new char[1] { ';' });
		if (array.Length == 3)
		{
			WorldCityColor worldCityColor = new WorldCityColor(array[0], array[1], array[2]);
			if (material != null)
			{
				material.SetColor(SolidOutline, worldCityColor.baseColor);
				material.SetColor(OutLine_Color, worldCityColor.outlineColor);
				material.SetColor(Glow_Color, worldCityColor.innerColor);
			}
			skinColorDict[skinColorStr] = worldCityColor;
		}
	}

	public void UpdateRenderParam(string k2, string k3, string k4, string k5)
	{
		if (data == null)
		{
			return;
		}
		int serverId = data.ServerId;
		if (serverId <= 0)
		{
			SwitchZoneMaterial(occupy: false);
			return;
		}
		SwitchZoneMaterial(occupy: true);
		if (data == null || zone_material == null)
		{
			return;
		}
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		int sourceServerId = GameEntry.Data.Player.GetSourceServerId();
		string allianceId2 = data.AllianceId;
		Material material = zone_material;
		material.EnableKeyword("PURE_COLOR");
		WorldCityColor value3;
		if (allianceId2 == allianceId)
		{
			WorldCityColor value;
			if (k2.IsNullOrEmpty())
			{
				SwitchZoneMaterial(occupy: false);
			}
			else if (skinColorDict.TryGetValue(k2, out value))
			{
				material.SetColor(SolidOutline, value.baseColor);
				material.SetColor(OutLine_Color, value.outlineColor);
				material.SetColor(Glow_Color, value.innerColor);
			}
			else
			{
				ParseSkinColor(k2, material);
			}
		}
		else if (serverId == sourceServerId)
		{
			WorldCityColor value2;
			if (k3.IsNullOrEmpty())
			{
				SwitchZoneMaterial(occupy: false);
			}
			else if (skinColorDict.TryGetValue(k3, out value2))
			{
				material.SetColor(SolidOutline, value2.baseColor);
				material.SetColor(OutLine_Color, value2.outlineColor);
				material.SetColor(Glow_Color, value2.innerColor);
			}
			else
			{
				ParseSkinColor(k3, material);
			}
		}
		else if (k4.IsNullOrEmpty())
		{
			SwitchZoneMaterial(occupy: false);
		}
		else if (skinColorDict.TryGetValue(k4, out value3))
		{
			material.SetColor(SolidOutline, value3.baseColor);
			material.SetColor(OutLine_Color, value3.outlineColor);
			material.SetColor(Glow_Color, value3.innerColor);
		}
		else
		{
			ParseSkinColor(k4, material);
		}
	}

	public void SetZoneRenderParam(WorldCityColor cfg, int occupyServerId, bool skipSkinCheck)
	{
		if (data == null)
		{
			return;
		}
		if (!skipSkinCheck)
		{
			string text = "season_map_zone_mode";
			if (GameEntry.Setting.GetPrivateBool(text, defaultValue: false))
			{
				SeasonDataManager instance = SeasonDataManager.Instance;
				string text2 = instance.GetData(text, "k1", string.Empty);
				if (!text2.IsNullOrEmpty() && text2.Contains(";" + GameEntry.Data.Player.GetCurServerId() + ";"))
				{
					string k = instance.GetData(text, "k2", string.Empty);
					string k2 = instance.GetData(text, "k3", string.Empty);
					string k3 = instance.GetData(text, "k4", string.Empty);
					string k4 = instance.GetData(text, "k5", string.Empty);
					ShowMode = ZoneShowMode.Normal;
					UpdateRenderParam(k, k2, k3, k4);
					return;
				}
			}
		}
		if (ShowMode != ZoneShowMode.Normal)
		{
			ShowZoneOasisMaterial();
			return;
		}
		if (cfg == null)
		{
			SwitchZoneMaterial(occupy: false);
			return;
		}
		bool flag = true;
		if (seasonType == SeasonType.Desert)
		{
			flag = true;
		}
		else if (seasonType == SeasonType.NineNation)
		{
			flag = cfg.splashPath.IsNullOrEmpty();
		}
		else if (seasonType != SeasonType.Nothing)
		{
			flag = cfg.splashPath.IsNullOrEmpty();
			if (!flag)
			{
				int curServerId = GameEntry.Data.Player.GetCurServerId();
				if (occupyServerId <= 0 || occupyServerId == curServerId)
				{
					flag = true;
				}
			}
		}
		else
		{
			flag = cfg.splashIndex == 0;
		}
		SwitchZoneMaterial(occupy: true);
		if (data == null || zone_material == null)
		{
			return;
		}
		Material material = zone_material;
		material.SetColor(SolidOutline, cfg.baseColor);
		material.SetColor(OutLine_Color, cfg.outlineColor);
		material.SetColor(Glow_Color, cfg.innerColor);
		if (flag)
		{
			material.EnableKeyword("PURE_COLOR");
			return;
		}
		Texture2D texture2D = cfg.GetTexture2D();
		if (texture2D != null)
		{
			material.DisableKeyword("PURE_COLOR");
			material.SetTexture(SplashTex, texture2D);
			return;
		}
		if (cfg.splashIndex <= 0)
		{
			material.EnableKeyword("PURE_COLOR");
			return;
		}
		Texture2D[] textures = mapManager.GetTextures();
		if (textures == null || textures.Length < cfg.splashIndex)
		{
			material.EnableKeyword("PURE_COLOR");
			return;
		}
		material.SetTexture(SplashTex, textures[cfg.splashIndex - 1]);
		material.DisableKeyword("PURE_COLOR");
	}

	public void SetZoneChangeColor(int beforeColorIndex, int afterColorIndex)
	{
		if (data == null)
		{
			return;
		}
		WorldCityColor worldCityColor = mapManager.GetWorldCityColor(beforeColorIndex);
		WorldCityColor worldCityColor2 = mapManager.GetWorldCityColor(afterColorIndex);
		if (worldCityColor2 != null)
		{
			SwitchZoneMaterial(occupy: true);
			Material material = zone_material;
			Color beforeBase = new Color(0.89f, 0.6f, 0.36f, 1f);
			Color beforeOutLine = new Color(0.89f, 0.6f, 0.36f, 1f);
			Color beforeInner = new Color(0.89f, 0.6f, 0.36f, 1f);
			Color afterBase = worldCityColor2.baseColor;
			Color afterOutLine = worldCityColor2.outlineColor;
			Color afterInner = worldCityColor2.innerColor;
			if (worldCityColor != null)
			{
				beforeBase = worldCityColor.baseColor;
				beforeOutLine = worldCityColor.outlineColor;
				beforeInner = worldCityColor.innerColor;
			}
			material.EnableKeyword("PURE_COLOR");
			float progress = 0f;
			DOTween.To(() => progress, delegate(float x)
			{
				progress = x;
			}, 1f, 2.5f).OnUpdate(delegate
			{
				Color value = Color.Lerp(beforeBase, afterBase, progress);
				Color value2 = Color.Lerp(beforeOutLine, afterOutLine, progress);
				Color value3 = Color.Lerp(beforeInner, afterInner, progress);
				material.SetColor(SolidOutline, value);
				material.SetColor(OutLine_Color, value2);
				material.SetColor(Glow_Color, value3);
			}).OnComplete(delegate
			{
				SetZoneRenderParam(mapManager.GetWorldCityColor(data.color), data.ServerId, skipSkinCheck: false);
				GameEntry.Event.Fire(EventId.UINoInput, 3);
				GameEntry.Event.Fire(EventId.WorldZoneChangeColorFinish, data.ZoneId.ToInt());
			});
		}
		else
		{
			GameEntry.Event.Fire(EventId.UINoInput, 3);
			GameEntry.Event.Fire(EventId.WorldZoneChangeColorFinish, data.ZoneId.ToInt());
		}
	}

	private void SwitchZoneMaterial(bool occupy)
	{
		if (zone_material == null || (occupy && "empty".Equals(zone_material.name)) || (!occupy && "occupy".Equals(zone_material.name)))
		{
			zone_material = mapManager.GetZoneMaterial(occupy);
			if (zone_material != null)
			{
				zone_material.name = (occupy ? "occupy" : "empty");
				UpdateTexture();
			}
		}
	}

	private void UpdateTexture()
	{
		if (!(zone_material != null) || zone_asset == null || !zone_asset.isDone)
		{
			return;
		}
		Texture value = zone_asset.asset as Texture;
		zone_material.SetTexture(MainTex, value);
		if (zone_mesh == null)
		{
			Vector4 zoneRect = mapManager.GetZoneRect(data.ZoneId);
			float num = zoneRect[0];
			float num2 = 0f - zoneRect[1];
			float num3 = zoneRect[2];
			float num4 = zoneRect[3];
			if (seasonType == SeasonType.CityStronghold || seasonType == SeasonType.Snow || seasonType == SeasonType.Mummy || seasonType == SeasonType.Darkness)
			{
				num -= 2.5f;
				num2 -= 2.5f;
				num3 += 5f;
				num4 += 5f;
			}
			zone_mesh = new Mesh
			{
				vertices = new Vector3[4]
				{
					new Vector3(num, 0f, num2),
					new Vector3(num, 0f, num2 + num4),
					new Vector3(num + num3, 0f, num2 + num4),
					new Vector3(num + num3, 0f, num2)
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

	public void OnUpdate(bool edge)
	{
		if (data == null || !mapManager.IsInited())
		{
			return;
		}
		if (edge)
		{
			if (edge_loaded && edgeEntities != null && edge_material != null)
			{
				foreach (Mesh value in edgeEntities.Values)
				{
					Graphics.DrawMesh(value, edge_matrix, edge_material, draw_layer);
				}
				return;
			}
			if (!edge_loaded && data != null)
			{
				CreateEdge();
			}
		}
		else if (zone_loaded && zone_mesh != null && zone_material != null)
		{
			Graphics.DrawMesh(zone_mesh, zone_matrix, zone_material, draw_layer);
		}
		else if (!zone_loaded && data != null)
		{
			CreateZone();
		}
	}

	public int GetCityField()
	{
		InitBlackAreaConfig();
		return m_ZoneField;
	}

	public void InitBlackAreaConfig()
	{
		if (m_ZoneField >= 0 || (m_ZoneFieldArray != null && m_ZoneFieldArray.Length == 4))
		{
			return;
		}
		string tabName = "lw_worldcity";
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && !string.IsNullOrEmpty(curSkinMeta.world_city_table_name))
		{
			tabName = curSkinMeta.world_city_table_name;
		}
		string templateData = GameEntry.ConfigCache.GetTemplateData(tabName, index, "city_field");
		string templateData2 = GameEntry.ConfigCache.GetTemplateData(tabName, index, "city_field_new");
		int.TryParse(templateData, out m_ZoneField);
		if (!string.IsNullOrWhiteSpace(templateData2))
		{
			string[] array = templateData2.Split(new char[1] { '|' });
			if (array.Length == 4)
			{
				m_ZoneFieldArray = new int[4] { m_ZoneField, m_ZoneField, m_ZoneField, m_ZoneField };
				int.TryParse(array[0], out m_ZoneFieldArray[0]);
				int.TryParse(array[1], out m_ZoneFieldArray[1]);
				int.TryParse(array[2], out m_ZoneFieldArray[2]);
				int.TryParse(array[3], out m_ZoneFieldArray[3]);
			}
		}
		string[] array2 = GameEntry.ConfigCache.GetTemplateData(tabName, index, "location").Split(new char[1] { '|' });
		if (array2.Length == 2)
		{
			m_ZoneFieldPos = new Vector2Int(int.Parse(array2[0]), int.Parse(array2[1]));
		}
	}

	public void DrawBlackArea(Mesh blackAreaMesh, Material baMaterial)
	{
		if (baMaterial != null)
		{
			if (!mIsInitPosition)
			{
				CreateBlackAreaMatrix();
				mIsInitPosition = true;
			}
			if (blackAreaMesh != null)
			{
				Graphics.DrawMesh(blackAreaMesh, baMatrix, baMaterial, 0);
			}
		}
	}

	private void CreateBlackAreaMatrix()
	{
		bool flag = false;
		Vector3 one = Vector3.one;
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			flag = !curSkinMeta.IsNotSeason();
		}
		int cityField = GetCityField();
		if (cityField < 0 && m_ZoneFieldArray == null)
		{
			return;
		}
		Vector3 pos = TileCoord.TileToWorld(m_ZoneFieldPos.x, m_ZoneFieldPos.y, 0);
		if (mapIndex > 1)
		{
			Vector3 worldBasePosByIndex = SeasonDataManager.Instance.GetWorldBasePosByIndex(mapIndex);
			pos += worldBasePosByIndex;
		}
		if (m_ZoneFieldArray != null)
		{
			one.x = 2f * (float)m_ZoneFieldArray[2] + 6f;
			one.y = 2f * (float)m_ZoneFieldArray[0] + 4f;
			one.z = 2f * (float)m_ZoneFieldArray[1] + 4f;
			pos += Vector3.one;
		}
		else
		{
			one *= (float)cityField * 2f;
			if (!flag)
			{
				one.x += 2f;
				one.y += 2f;
				one.z += 2f;
			}
		}
		one.z = 0f;
		pos.y = 0f;
		baMatrix = Matrix4x4.TRS(pos, Quaternion.Euler(Vector3.right * 90f), one);
	}

	private void ShowZoneOasisMaterial()
	{
		if (data != null)
		{
			OasisViewScale oasisViewScale = mapManager.GetOasisViewScale(data.Rate);
			if (oasisViewScale == null)
			{
				SwitchZoneMaterial(occupy: false);
				return;
			}
			SwitchZoneMaterial(occupy: true);
			Material material = zone_material;
			material.SetColor(SolidOutline, oasisViewScale.baseColor);
			material.SetColor(OutLine_Color, oasisViewScale.outlineColor);
			material.SetColor(Glow_Color, oasisViewScale.innerColor);
			material.EnableKeyword("PURE_COLOR");
		}
	}
}
