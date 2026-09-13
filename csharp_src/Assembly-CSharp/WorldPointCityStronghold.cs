using System;
using System.Collections.Generic;
using UnityEngine;
using VEngine;

public class WorldPointCityStronghold : MonoBehaviour
{
	private static readonly Vector2Int TileCount = new Vector2Int(1000, 1000);

	private static readonly int ExpandRadius = 7;

	private SeasonType theSeasonType;

	private int theServerId;

	private int theCityId;

	private int thePointIndex;

	private int theCitySize;

	private bool hasBoss = true;

	private Mesh mainMesh;

	private Matrix4x4 mainMatrix;

	private Vector2Int thePointTilePos = Vector2Int.zero;

	private Dictionary<int, int> theMonsterIds;

	private Dictionary<long, Vector2Int> MonsterDict = new Dictionary<long, Vector2Int>();

	private long[,] DirtyPoint;

	private List<Vector3> mesh_vertices = new List<Vector3>();

	private List<Vector2> mesh_uv1 = new List<Vector2>();

	private List<Vector2> mesh_uv2 = new List<Vector2>();

	private List<int> mesh_triangles = new List<int>();

	private const long mod_top = 16L;

	private const long mod_right_top = 256L;

	private const long mod_right = 4096L;

	private const long mod_right_bottom = 65536L;

	private const long mod_bottom = 1048576L;

	private const long mod_left_bottom = 16777216L;

	private const long mod_left = 268435456L;

	private const long mod_left_top = 4294967296L;

	private static Asset asset_main_default = null;

	private static Asset asset_mask_default = null;

	private static Asset asset_main_snow = null;

	private static Asset asset_mask_snow = null;

	private static Asset asset_main_mummy = null;

	private static Asset asset_mask_mummy = null;

	private static Asset asset_main_dark = null;

	private static Asset asset_mask_dark = null;

	private static Asset asset_main_9city = null;

	private static Asset asset_mask_9city = null;

	private static Material materialDefault = null;

	private static Material materialSnow = null;

	private static Material materialMummy = null;

	private static Material materialDark = null;

	private static Material material9City = null;

	public void DoInit(int cityId, int pointIndex, int serverId, int citySize, Dictionary<int, int> monsterIds)
	{
		if (base.gameObject == null)
		{
			return;
		}
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			theSeasonType = curSkinMeta.GetMapType();
		}
		if (cityId > 0 && theSeasonType != SeasonType.Nothing && theSeasonType != SeasonType.Desert)
		{
			theServerId = serverId;
			theCityId = cityId;
			theCitySize = citySize;
			thePointIndex = pointIndex;
			theMonsterIds = monsterIds;
			thePointTilePos = TileCoord.IndexToTilePos(pointIndex, TileCount);
			MonsterDict.Clear();
			hasBoss = false;
			mainMesh = null;
			int num = theCitySize + ExpandRadius * 2;
			Vector3 pos = TileCoord.TileToWorld(thePointTilePos, serverId) - new Vector3(num, 0f, num);
			hasBoss = SceneManager.World.GetMonster(thePointIndex) != null;
			mainMatrix = Matrix4x4.TRS(pos, Quaternion.Euler(Vector3.right * 90f), Vector3.one);
			SceneManager.World.GetMonsterListInArea(thePointTilePos, theCitySize + ExpandRadius, theMonsterIds, MonsterDict);
			WorldMarchDataManager.OnMonsterAdd = (Action<long, int, int, int>)Delegate.Combine(WorldMarchDataManager.OnMonsterAdd, new Action<long, int, int, int>(OnMonsterAdd));
			WorldMarchDataManager.OnMonsterDelete = (Action<long, int, int, int>)Delegate.Combine(WorldMarchDataManager.OnMonsterDelete, new Action<long, int, int, int>(OnMonsterDelete));
			if (hasBoss)
			{
				ReCalcVirus();
			}
			if (theSeasonType == SeasonType.Mummy)
			{
				LoadMummyMaterial();
			}
			else if (theSeasonType == SeasonType.Darkness)
			{
				LoadDarknessMaterial();
			}
			else if (theSeasonType == SeasonType.NineNation)
			{
				LoadNineNationMaterial();
			}
			else if (theSeasonType == SeasonType.Snow)
			{
				LoadSnowMaterial();
			}
			else
			{
				LoadDefaultMaterial();
			}
		}
	}

	public void UnInit()
	{
		if (theCityId > 0)
		{
			WorldMarchDataManager.OnMonsterAdd = (Action<long, int, int, int>)Delegate.Remove(WorldMarchDataManager.OnMonsterAdd, new Action<long, int, int, int>(OnMonsterAdd));
			WorldMarchDataManager.OnMonsterDelete = (Action<long, int, int, int>)Delegate.Remove(WorldMarchDataManager.OnMonsterDelete, new Action<long, int, int, int>(OnMonsterDelete));
		}
		if (mainMesh != null)
		{
			UnityEngine.Object.Destroy(mainMesh);
			mainMesh = null;
		}
		hasBoss = false;
		theCityId = 0;
		theCitySize = 0;
		thePointIndex = 0;
		theMonsterIds = null;
	}

	private void OnMonsterAdd(long uuid, int monsterId, int serverId, int pointIndex)
	{
		if (theCityId <= 0 || theCitySize <= 0 || thePointIndex <= 0 || serverId != theServerId)
		{
			return;
		}
		if (pointIndex == thePointIndex)
		{
			hasBoss = true;
			ReCalcVirus();
		}
		else if (theMonsterIds.ContainsKey(monsterId))
		{
			int num = theCitySize + ExpandRadius;
			Vector2Int value = TileCoord.IndexToTilePos(pointIndex, TileCount);
			if (value.x >= thePointTilePos.x - num && value.x <= thePointTilePos.x + num && value.y >= thePointTilePos.y - num && value.y <= thePointTilePos.y + num)
			{
				MonsterDict[uuid] = value;
				ReCalcVirus();
			}
		}
	}

	private void OnMonsterDelete(long uuid, int monsterId, int serverId, int pointIndex)
	{
		if (theCityId <= 0 || theCitySize <= 0 || thePointIndex <= 0 || serverId != theServerId)
		{
			return;
		}
		if (pointIndex == thePointIndex)
		{
			hasBoss = false;
		}
		else if (MonsterDict.ContainsKey(uuid))
		{
			MonsterDict.Clear();
			SceneManager.World.GetMonsterListInArea(thePointTilePos, theCitySize + ExpandRadius, theMonsterIds, MonsterDict);
			MonsterDict.Remove(uuid);
			ReCalcVirus();
		}
		else if (theMonsterIds.ContainsKey(monsterId))
		{
			int num = theCitySize + ExpandRadius;
			Vector2Int vector2Int = TileCoord.IndexToTilePos(pointIndex, TileCount);
			if (vector2Int.x >= thePointTilePos.x - num && vector2Int.x <= thePointTilePos.x + num && vector2Int.y >= thePointTilePos.y - num && vector2Int.y <= thePointTilePos.y + num)
			{
				MonsterDict.Clear();
				SceneManager.World.GetMonsterListInArea(thePointTilePos, theCitySize + ExpandRadius, theMonsterIds, MonsterDict);
				MonsterDict.Remove(uuid);
				ReCalcVirus();
			}
		}
	}

	private void ReCalcVirus()
	{
		if (!hasBoss)
		{
			return;
		}
		int num = theCitySize + ExpandRadius * 2;
		if (DirtyPoint == null)
		{
			DirtyPoint = (long[,])Array.CreateInstance(typeof(long), num, num);
		}
		Array.Clear(DirtyPoint, 0, DirtyPoint.Length);
		int num2 = theCitySize / 2;
		for (int i = ExpandRadius; i < theCitySize + ExpandRadius; i++)
		{
			for (int j = ExpandRadius; j < theCitySize + ExpandRadius; j++)
			{
				DirtyPoint[i, j] = 1L;
			}
		}
		int num3 = thePointTilePos.x - num2 - ExpandRadius;
		int num4 = thePointTilePos.y - num2 - ExpandRadius;
		int num5 = thePointTilePos.x + num2 + ExpandRadius;
		int num6 = thePointTilePos.y + num2 + ExpandRadius;
		foreach (KeyValuePair<long, Vector2Int> item in MonsterDict)
		{
			if (item.Value.x > num3 && item.Value.y > num4 && item.Value.x < num5 && item.Value.y < num6)
			{
				DirtyPoint[item.Value.x - num3, item.Value.y - num4] = 1L;
			}
		}
		ReCalcEdge();
		ReCalcDrawData();
	}

	private void ReCalcEdge()
	{
		long num = 0L;
		int num2 = theCitySize + ExpandRadius * 2;
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				if (DirtyPoint[i, j] != 1)
				{
					num = 0L;
					if (j + 1 < num2 && DirtyPoint[i, j + 1] == 1)
					{
						num |= 0x10;
					}
					if (i + 1 < num2 && j + 1 < num2 && DirtyPoint[i + 1, j + 1] == 1)
					{
						num |= 0x100;
					}
					if (i + 1 < num2 && DirtyPoint[i + 1, j] == 1)
					{
						num |= 0x1000;
					}
					if (i + 1 < num2 && j > 0 && DirtyPoint[i + 1, j - 1] == 1)
					{
						num |= 0x10000;
					}
					if (j > 0 && DirtyPoint[i, j - 1] == 1)
					{
						num |= 0x100000;
					}
					if (i > 0 && j > 0 && DirtyPoint[i - 1, j - 1] == 1)
					{
						num |= 0x1000000;
					}
					if (i > 0 && DirtyPoint[i - 1, j] == 1)
					{
						num |= 0x10000000;
					}
					if (i > 0 && j + 1 < num2 && DirtyPoint[i - 1, j + 1] == 1)
					{
						num |= 0x100000000L;
					}
					DirtyPoint[i, j] = num;
				}
			}
		}
	}

	private void Update()
	{
		if (hasBoss && mainMesh != null && !(base.gameObject == null) && theCityId > 0 && thePointIndex > 0 && theCitySize > 0 && !(base.gameObject.transform.localPosition.x <= -9999f) && !(base.gameObject.transform.localPosition.y <= -9999f))
		{
			if (theSeasonType == SeasonType.Mummy && materialMummy != null)
			{
				Graphics.DrawMesh(mainMesh, mainMatrix, materialMummy, 0);
			}
			else if (theSeasonType == SeasonType.Snow && materialSnow != null)
			{
				Graphics.DrawMesh(mainMesh, mainMatrix, materialSnow, 0);
			}
			else if (theSeasonType == SeasonType.Darkness && materialDark != null)
			{
				Graphics.DrawMesh(mainMesh, mainMatrix, materialDark, 0);
			}
			else if (theSeasonType == SeasonType.NineNation && material9City != null)
			{
				Graphics.DrawMesh(mainMesh, mainMatrix, material9City, 0);
			}
			else if (materialDefault != null)
			{
				Graphics.DrawMesh(mainMesh, mainMatrix, materialDefault, 0);
			}
		}
	}

	private void ReCalcDrawData()
	{
		int num = theCitySize + ExpandRadius * 2;
		mesh_vertices.Clear();
		mesh_uv1.Clear();
		mesh_uv2.Clear();
		mesh_triangles.Clear();
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				long num2 = DirtyPoint[i, j];
				switch (num2)
				{
				case 1L:
					AppendMeshData(i, j, 1, 1);
					continue;
				case 0L:
					continue;
				}
				if ((num2 & 0x10101010) == 269488144)
				{
					AppendMeshData(i, j, 1, 4);
					continue;
				}
				if ((num2 & 0x10101000) == 269488128)
				{
					AppendMeshData(i, j, 0, 4);
					continue;
				}
				if ((num2 & 0x10100010) == 269484048)
				{
					AppendMeshData(i, j, 1, 5);
					continue;
				}
				if ((num2 & 0x10001010) == 268439568)
				{
					AppendMeshData(i, j, 2, 4);
					continue;
				}
				if ((num2 & 0x101010) == 1052688)
				{
					AppendMeshData(i, j, 1, 3);
					continue;
				}
				if ((num2 & 0x1010) == 4112)
				{
					AppendMeshData(i, j, 2, 3);
				}
				else if ((num2 & 0x10000010) == 268435472)
				{
					AppendMeshData(i, j, 2, 5);
				}
				else if ((num2 & 0x101000) == 1052672)
				{
					AppendMeshData(i, j, 0, 3);
				}
				else if ((num2 & 0x10100000) == 269484032)
				{
					AppendMeshData(i, j, 0, 5);
				}
				else
				{
					if ((num2 & 0x10) == 16)
					{
						AppendMeshData(i, j, 2, 1);
					}
					if ((num2 & 0x100000) == 1048576)
					{
						AppendMeshData(i, j, 0, 1);
					}
					if ((num2 & 0x10000000) == 268435456)
					{
						AppendMeshData(i, j, 1, 2);
					}
					if ((num2 & 0x1000) == 4096)
					{
						AppendMeshData(i, j, 1, 0);
					}
				}
				if ((num2 & 0x100) == 256)
				{
					AppendMeshData(i, j, 2, 0);
				}
				if ((num2 & 0x10000) == 65536)
				{
					AppendMeshData(i, j, 0, 0);
				}
				if ((num2 & 0x1000000) == 16777216)
				{
					AppendMeshData(i, j, 0, 2);
				}
				if ((num2 & 0x100000000L) == 4294967296L)
				{
					AppendMeshData(i, j, 2, 2);
				}
			}
		}
		if (mainMesh == null)
		{
			mainMesh = new Mesh();
		}
		mainMesh.Clear();
		mainMesh.vertices = mesh_vertices.ToArray();
		mainMesh.uv = mesh_uv1.ToArray();
		mainMesh.uv2 = mesh_uv2.ToArray();
		mainMesh.triangles = mesh_triangles.ToArray();
	}

	private void AppendMeshData(int x, int y, int row, int col)
	{
		int count = mesh_vertices.Count;
		mesh_vertices.Add(new Vector3(2 * x, 2 * y, 0f));
		mesh_vertices.Add(new Vector3(2 * x, 2 * y + 2, 0f));
		mesh_vertices.Add(new Vector3(2 * x + 2, 2 * y + 2, 0f));
		mesh_vertices.Add(new Vector3(2 * x + 2, 2 * y, 0f));
		mesh_uv1.Add(new Vector2(0f, 0f));
		mesh_uv1.Add(new Vector2(0f, 1f));
		mesh_uv1.Add(new Vector2(1f, 1f));
		mesh_uv1.Add(new Vector2(1f, 0f));
		mesh_uv2.Add(new Vector2((float)col * 0.125f, 0.75f - (float)row * 0.25f));
		mesh_uv2.Add(new Vector2((float)col * 0.125f, 1f - (float)row * 0.25f));
		mesh_uv2.Add(new Vector2((float)col * 0.125f + 0.125f, 1f - (float)row * 0.25f));
		mesh_uv2.Add(new Vector2((float)col * 0.125f + 0.125f, 0.75f - (float)row * 0.25f));
		mesh_triangles.Add(count);
		mesh_triangles.Add(1 + count);
		mesh_triangles.Add(2 + count);
		mesh_triangles.Add(2 + count);
		mesh_triangles.Add(3 + count);
		mesh_triangles.Add(count);
	}

	private static void LoadDefaultMaterial()
	{
		if (asset_main_default == null)
		{
			asset_main_default = GameEntry.Resource.LoadAssetAsync("Assets/Main/Scenes/Contaminate/GroundGrass_basecolor.png", typeof(Texture2D));
			asset_mask_default = GameEntry.Resource.LoadAssetAsync("Assets/Main/Scenes/Contaminate/Contaminate_default.tga", typeof(Texture2D));
			LoadAssetFinish(asset_main_default, asset_main_default, asset_mask_default);
			LoadAssetFinish(asset_mask_default, asset_main_default, asset_mask_default);
		}
	}

	private static void LoadSnowMaterial()
	{
		if (asset_main_snow == null)
		{
			asset_main_snow = GameEntry.Resource.LoadAssetAsync("Assets/Main/Scenes/Contaminate/Snow_basecolor.tga", typeof(Texture2D));
			asset_mask_snow = GameEntry.Resource.LoadAssetAsync("Assets/Main/Scenes/Contaminate/Contaminate_default.tga", typeof(Texture2D));
			LoadAssetFinish(asset_main_snow, asset_main_snow, asset_mask_snow);
			LoadAssetFinish(asset_mask_snow, asset_main_snow, asset_mask_snow);
		}
	}

	private static void LoadNineNationMaterial()
	{
		if (asset_main_9city == null)
		{
			asset_main_9city = GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S5/Scenes/Contaminate/GroundGrass_basecolor_s5.png", typeof(Texture2D));
			asset_mask_9city = GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S5/Scenes/Contaminate/Contaminate_s5.tga", typeof(Texture2D));
			LoadAssetFinish(asset_main_9city, asset_main_9city, asset_mask_9city);
			LoadAssetFinish(asset_mask_9city, asset_main_9city, asset_mask_9city);
		}
	}

	private static void LoadDarknessMaterial()
	{
		if (asset_main_dark == null)
		{
			asset_main_dark = GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S4/Scenes/Contaminate/GroundGrass_basecolor_s4.png", typeof(Texture2D));
			asset_mask_dark = GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S4/Scenes/Contaminate/Contaminate_default_s4.tga", typeof(Texture2D));
			LoadAssetFinish(asset_main_dark, asset_main_dark, asset_mask_dark);
			LoadAssetFinish(asset_mask_dark, asset_main_dark, asset_mask_dark);
		}
	}

	private static void LoadMummyMaterial()
	{
		if (asset_main_mummy == null)
		{
			asset_main_mummy = GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S3/Scenes/Contaminate/GroundGrass_basecolor_s3.png", typeof(Texture2D));
			asset_mask_mummy = GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S3/Scenes/Contaminate/Contaminate_default_s3.tga", typeof(Texture2D));
			LoadAssetFinish(asset_main_mummy, asset_main_mummy, asset_mask_mummy);
			LoadAssetFinish(asset_mask_mummy, asset_main_mummy, asset_mask_mummy);
		}
	}

	private static void LoadAssetFinish(Asset asset, Asset assetMain, Asset assetMask)
	{
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (assetMain.isDone && assetMask.isDone)
			{
				Asset asset2 = GameEntry.Resource.LoadAsset("Assets/Main/Shaders2024/StrongholdSurface.shader", typeof(Shader));
				if (asset2 != null)
				{
					Material material = new Material(asset2.asset as Shader);
					material.SetTexture("_BaseColor", assetMain.asset as Texture2D);
					material.SetTexture("_Mask", assetMask.asset as Texture2D);
					material.renderQueue = 2012;
					material.enableInstancing = true;
					if (assetMain == asset_main_default)
					{
						materialDefault = material;
					}
					if (assetMain == asset_main_snow)
					{
						materialSnow = material;
					}
					if (assetMain == asset_main_mummy)
					{
						materialMummy = material;
					}
					if (assetMain == asset_main_dark)
					{
						materialDark = material;
					}
					if (assetMain == asset_main_9city)
					{
						material9City = material;
					}
				}
			}
		});
	}

	public static void CleanAsset()
	{
		if (materialDefault != null)
		{
			UnityEngine.Object.Destroy(materialDefault);
			materialDefault = null;
		}
		if (materialSnow != null)
		{
			UnityEngine.Object.Destroy(materialSnow);
			materialSnow = null;
		}
		if (materialMummy != null)
		{
			UnityEngine.Object.Destroy(materialMummy);
			materialMummy = null;
		}
		if (materialDark != null)
		{
			UnityEngine.Object.Destroy(materialDark);
			materialDark = null;
		}
		if (material9City != null)
		{
			UnityEngine.Object.Destroy(material9City);
			material9City = null;
		}
		if (asset_main_default != null)
		{
			asset_main_default.Release();
			asset_main_default = null;
		}
		if (asset_mask_default != null)
		{
			asset_mask_default.Release();
			asset_mask_default = null;
		}
		if (asset_main_snow != null)
		{
			asset_main_snow.Release();
			asset_main_snow = null;
		}
		if (asset_mask_snow != null)
		{
			asset_mask_snow.Release();
			asset_mask_snow = null;
		}
		if (asset_main_mummy != null)
		{
			asset_main_mummy.Release();
			asset_main_mummy = null;
		}
		if (asset_mask_mummy != null)
		{
			asset_mask_mummy.Release();
			asset_mask_mummy = null;
		}
		if (asset_main_dark != null)
		{
			asset_main_dark.Release();
			asset_main_dark = null;
		}
		if (asset_mask_dark != null)
		{
			asset_mask_dark.Release();
			asset_mask_dark = null;
		}
		if (asset_main_9city != null)
		{
			asset_main_9city.Release();
			asset_main_9city = null;
		}
		if (asset_mask_9city != null)
		{
			asset_mask_9city.Release();
			asset_mask_9city = null;
		}
	}
}
