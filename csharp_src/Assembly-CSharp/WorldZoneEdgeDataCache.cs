using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using VEngine;

public class WorldZoneEdgeDataCache
{
	public List<string> imgNameList = new List<string>();

	public Dictionary<int, int> imgIndexList = new Dictionary<int, int>();

	private Dictionary<int, Mesh> meshDict;

	private Dictionary<int, string> zoneDict;

	private List<WorldZoneEdgeData> edgeList;

	private Dictionary<int, Dictionary<int, Mesh>> cache;

	private static Dictionary<SeasonType, WorldZoneEdgeDataCache> _cacheEdge = new Dictionary<SeasonType, WorldZoneEdgeDataCache>();

	private static Dictionary<SeasonType, WorldZoneImageDetail> _cacheImageDetail = new Dictionary<SeasonType, WorldZoneImageDetail>();

	public WorldZoneEdgeDataCache(WorldZoneEdgeConfig data)
	{
		meshDict = new Dictionary<int, Mesh>();
		cache = new Dictionary<int, Dictionary<int, Mesh>>();
		zoneDict = new Dictionary<int, string>();
		edgeList = data.edgeList;
		foreach (WorldZoneEdgeDataKeyInfo zone in data.zoneList)
		{
			zoneDict.Add(zone.z, zone.i);
		}
	}

	private Mesh GetMesh(int index)
	{
		if (meshDict.TryGetValue(index, out var value))
		{
			return value;
		}
		WorldZoneEdgeData worldZoneEdgeData = edgeList[index];
		string[] source = worldZoneEdgeData.t.Split(new char[1] { '|' });
		string[] array = worldZoneEdgeData.ux.Split(new char[1] { '|' });
		string[] array2 = worldZoneEdgeData.uy.Split(new char[1] { '|' });
		string[] array3 = worldZoneEdgeData.vx.Split(new char[1] { '|' });
		string[] array4 = worldZoneEdgeData.vz.Split(new char[1] { '|' });
		List<Vector2> list = new List<Vector2>();
		List<Vector3> list2 = new List<Vector3>();
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(new Vector2(array[i].ToInt(), array2[i].ToInt()));
		}
		for (int j = 0; j < array3.Length; j++)
		{
			list2.Add(new Vector3(array3[j].ToFloat(), 0f, array4[j].ToFloat()));
		}
		value = new Mesh
		{
			vertices = list2.ToArray(),
			uv = list.ToArray(),
			triangles = source.ToList().ConvertAll((string x) => x.ToInt()).ToArray()
		};
		meshDict.Add(index, value);
		return value;
	}

	public Dictionary<int, Mesh> GetEdgeList(int zoneId)
	{
		if (cache.TryGetValue(zoneId, out var value))
		{
			return cache[zoneId];
		}
		value = new Dictionary<int, Mesh>();
		if (zoneDict.TryGetValue(zoneId, out var value2))
		{
			string[] array = value2.Split(new char[1] { '|' });
			for (int i = 0; i < array.Length; i += 2)
			{
				value.Add(array[i].ToInt(), GetMesh(array[i + 1].ToInt()));
			}
			cache[zoneId] = value;
		}
		return value;
	}

	public static void PreLoadZoneEdgeData(SceneSkinMeta meta)
	{
		SeasonType seasonType = SeasonType.Nothing;
		string edgeFilePath = "Assets/Main/Scenes/Zone/Edge_S0.asset";
		string imageDetailFilePath = null;
		if (meta != null)
		{
			seasonType = meta.GetMapType();
			if (seasonType == SeasonType.CityStronghold)
			{
				edgeFilePath = "Assets/Main/Scenes/Zone/Edge_S1.asset";
				if (!GameEntry.Resource.HasAsset(edgeFilePath))
				{
					edgeFilePath = "Assets/Main/SeasonRes/S1/Scenes/Edge_S1.asset";
				}
			}
			else if (seasonType == SeasonType.Snow)
			{
				edgeFilePath = "Assets/Main/Scenes/Zone/Edge_S2.asset";
				if (!GameEntry.Resource.HasAsset(edgeFilePath))
				{
					edgeFilePath = "Assets/Main/SeasonRes/S2/Scenes/Edge_S2.asset";
				}
			}
			else if (seasonType == SeasonType.Mummy)
			{
				edgeFilePath = "Assets/Main/SeasonRes/S3/Scenes/Zone/Edge_S3.asset";
				imageDetailFilePath = "Assets/Main/SeasonRes/S3/Scenes/Zone/zone_image_s3.bytes";
			}
			else if (seasonType == SeasonType.Darkness)
			{
				edgeFilePath = "Assets/Main/SeasonRes/S4/Scenes/Zone/Edge_S4.asset";
				imageDetailFilePath = "Assets/Main/SeasonRes/S4/Scenes/Zone/zone_image_s4.bytes";
			}
			else if (seasonType == SeasonType.NineNation)
			{
				edgeFilePath = "Assets/Main/SeasonRes/S5/Scenes/Zone/Edge_S5.asset";
				imageDetailFilePath = "Assets/Main/SeasonRes/S5/Scenes/Zone/zone_image_s5.bytes";
			}
		}
		if (!_cacheEdge.ContainsKey(seasonType) && GameEntry.Resource.HasAsset(edgeFilePath))
		{
			Asset EdgeDataAsset = GameEntry.Resource.LoadAssetAsync(edgeFilePath, typeof(WorldZoneEdgeConfig));
			Asset asset = EdgeDataAsset;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (EdgeDataAsset.isError)
				{
					Debug.LogFormat("#WorldZone#, Load Zone Edge Error! Error={0}, Path={1}", EdgeDataAsset.error, edgeFilePath);
				}
				else
				{
					WorldZoneEdgeConfig worldZoneEdgeConfig = EdgeDataAsset.Get<WorldZoneEdgeConfig>();
					if (worldZoneEdgeConfig != null)
					{
						_cacheEdge[seasonType] = new WorldZoneEdgeDataCache(worldZoneEdgeConfig);
					}
				}
			});
		}
		if (imageDetailFilePath == null || _cacheImageDetail.ContainsKey(seasonType) || !GameEntry.Resource.HasAsset(imageDetailFilePath))
		{
			return;
		}
		Asset ImageInfoDataAsset = GameEntry.Resource.LoadAssetAsync(imageDetailFilePath, typeof(TextAsset));
		Asset asset2 = ImageInfoDataAsset;
		asset2.completed = (Action<Asset>)Delegate.Combine(asset2.completed, (Action<Asset>)delegate
		{
			if (ImageInfoDataAsset.isError)
			{
				Debug.LogFormat("#WorldZone#, Load Zone Image Detail Error! Error={0}, Path={1}", ImageInfoDataAsset.error, imageDetailFilePath);
			}
			else
			{
				TextAsset textAsset = ImageInfoDataAsset.asset as TextAsset;
				if (textAsset != null)
				{
					using MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
					BinaryReader binaryReader = new BinaryReader(memoryStream);
					memoryStream.Seek(0L, SeekOrigin.Begin);
					WorldZoneImageDetail worldZoneImageDetail = new WorldZoneImageDetail();
					worldZoneImageDetail.Load(binaryReader);
					binaryReader.Close();
					_cacheImageDetail[seasonType] = worldZoneImageDetail;
				}
				ImageInfoDataAsset.Release();
			}
		});
	}

	public static void CleanCache()
	{
		_cacheEdge.Clear();
		_cacheImageDetail.Clear();
	}

	public static Dictionary<int, Mesh> GetEdgeList(SeasonType seasonType, WorldZoneData zoneData)
	{
		if (_cacheEdge.TryGetValue(seasonType, out var value))
		{
			return value.GetEdgeList(zoneData.ZoneId);
		}
		if (seasonType != SeasonType.Nothing)
		{
			return null;
		}
		string format = "Assets/Main/Scenes/Zone/Edge/edge_{0}_{1}.bytes";
		Dictionary<int, Mesh> dictionary = new Dictionary<int, Mesh>(zoneData.edgeIdList.Count);
		foreach (int edgeId in zoneData.edgeIdList)
		{
			string path = string.Format(format, zoneData.ZoneId, edgeId);
			if (!GameEntry.Resource.HasAsset(path))
			{
				continue;
			}
			Asset asset = GameEntry.Resource.LoadAsset(path, typeof(TextAsset));
			if (asset == null || asset.isError)
			{
				continue;
			}
			TextAsset textAsset = asset.asset as TextAsset;
			if (textAsset == null)
			{
				asset.Release();
				continue;
			}
			Mesh mesh = MeshSerializer.DeserializeMesh(textAsset.bytes);
			if (mesh != null)
			{
				dictionary[edgeId] = mesh;
			}
			asset.Release();
		}
		return dictionary;
	}

	public static string GetZoneImageName(SeasonType seasonType, WorldZoneData zoneData)
	{
		if (_cacheImageDetail.TryGetValue(seasonType, out var value))
		{
			return value.GetZoneImageName(zoneData.ZoneId);
		}
		return null;
	}
}
