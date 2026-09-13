using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using UnityEngine;
using VEngine;

public class PVEStaticManager : PVEDecorationManagerBase
{
	private class StaticObject
	{
		private InstanceRequest request;

		private WorldSceneDesc.ObjectDesc desc;

		private GameObject gameObject;

		private bool isVisible;

		private Vector2Int tilePos;

		private Vector3 position;

		private float renderOffsetZ;

		public int DataChunkY;

		public bool IsVisible => isVisible;

		public int Id => desc.id;

		public Vector2Int TilePos => tilePos;

		public StaticObject(WorldSceneDesc.ObjectDesc desc, int dataChunkY, float renderOffsetZ = 0f)
		{
			Init(desc, dataChunkY, renderOffsetZ);
		}

		public void Init(WorldSceneDesc.ObjectDesc desc, int dataChunkY, float renderOffsetZ = 0f)
		{
			this.desc = desc;
			this.renderOffsetZ = renderOffsetZ;
			Vector3 localPos = this.desc.localPos;
			position = localPos + Vector3.forward * renderOffsetZ;
			tilePos = TileCoord.WorldToTile(position);
			isVisible = true;
			DataChunkY = dataChunkY;
		}

		public void Load(Transform parent, bool useLightMap, Dictionary<int, PrefabLightmapData.RendererInfo> lightMapData)
		{
			request = GameEntry.Resource.InstantiateAsync(desc.assetPath, ObjectPoolTag.BattleScene);
			if (request == null)
			{
				return;
			}
			request.completed += delegate
			{
				if (request.gameObject == null)
				{
					Log.Error("gameObject null");
				}
				else if (desc == null)
				{
					Log.Warning("OnSpawn desc null");
				}
				else
				{
					gameObject = request.gameObject;
					gameObject.transform.SetParent(parent, worldPositionStays: false);
					gameObject.transform.localPosition = position;
					gameObject.transform.localScale = desc.scale;
					gameObject.transform.localRotation = Quaternion.Euler(desc.rotation);
					if (useLightMap)
					{
						ApplyLightMap(lightMapData, desc.id);
					}
					gameObject.SetActive(isVisible);
				}
			};
		}

		private void ApplyLightMap(Dictionary<int, PrefabLightmapData.RendererInfo> lightMapData, int id)
		{
			if (lightMapData.ContainsKey(id))
			{
				PrefabLightmapData.RendererInfo rendererInfo = lightMapData[id];
				MeshRenderer componentInChildren = gameObject.GetComponentInChildren<MeshRenderer>();
				if (componentInChildren != null)
				{
					componentInChildren.lightmapIndex = rendererInfo.lightmapIndex;
					componentInChildren.lightmapScaleOffset = rendererInfo.lightmapOffsetScale;
				}
			}
		}

		public void SetRenderOffsetZ(float renderOffsetZ)
		{
			this.renderOffsetZ = renderOffsetZ;
			Vector3 localPos = desc.localPos;
			position = localPos + Vector3.forward * renderOffsetZ;
			tilePos = TileCoord.WorldToTile(position);
			if (gameObject != null)
			{
				gameObject.transform.localPosition = position;
			}
		}

		public void Unload()
		{
			desc = null;
			request.Destroy();
			gameObject = null;
			isVisible = false;
			tilePos = Vector2Int.zero;
			position = Vector3.zero;
			renderOffsetZ = 0f;
		}

		public void SetVisible(bool v)
		{
			isVisible = v;
			if (gameObject != null)
			{
				gameObject.SetActive(v);
			}
		}
	}

	public const string PVEDecorationBytePath = "Assets/Main/Prefabs/PVELevel/{0}/decoration.bytes";

	private int _createCountPerFrame = 20;

	private List<Asset> _descAsset;

	private Dictionary<int, StaticObject> _objsDict = new Dictionary<int, StaticObject>();

	private Dictionary<int, WorldSceneDesc.ObjectDesc> _createList = new Dictionary<int, WorldSceneDesc.ObjectDesc>();

	private List<int> objsToRemove = new List<int>();

	private List<Vector2Int> chunksToRemove = new List<Vector2Int>();

	private Dictionary<Vector2Int, List<WorldSceneDesc.ObjectDesc>> _chunks = new Dictionary<Vector2Int, List<WorldSceneDesc.ObjectDesc>>();

	private Dictionary<int, PrefabLightmapData.RendererInfo> lightMapData = new Dictionary<int, PrefabLightmapData.RendererInfo>();

	private readonly Stack<StaticObject> _staticObjectStack = new Stack<StaticObject>(64);

	private bool useLightMap;

	public void InitLightMapConfig(string jsonStr)
	{
		lightMapData.Clear();
		if (!GameEntry.Resource.HasAsset(jsonStr))
		{
			return;
		}
		Asset asset = GameEntry.Resource.LoadAsset(jsonStr, typeof(TextAsset));
		if (asset == null)
		{
			return;
		}
		string[] array = asset.asset.ToString().Split(new char[1] { ';' });
		for (int i = 0; i < array.Length - 1; i++)
		{
			if (array[i].IsNullOrEmpty())
			{
				continue;
			}
			string[] array2 = array[i].Split(new char[1] { ':' });
			if (!array2[0].IsNullOrEmpty() && !(array2[0] == string.Empty))
			{
				int key = Convert.ToInt32(array2[0]);
				int lightmapIndex = Convert.ToInt32(array2[1]);
				string[] array3 = array2[2].Split(new char[1] { ',' });
				Vector4 lightmapOffsetScale = new Vector4(Convert.ToSingle(array3[0]), Convert.ToSingle(array3[1]), Convert.ToSingle(array3[2]), Convert.ToSingle(array3[3]));
				PrefabLightmapData.RendererInfo value = new PrefabLightmapData.RendererInfo
				{
					lightmapIndex = lightmapIndex,
					lightmapOffsetScale = lightmapOffsetScale
				};
				if (!lightMapData.ContainsKey(key))
				{
					lightMapData.Add(key, value);
				}
			}
		}
		useLightMap = true;
	}

	public override void Init(string descPath, int tileCountPerChunk, int createCountPerFrame)
	{
		parentNode = new GameObject("PVEStatic").transform;
		lastViewChunk = new Vector2Int(int.MinValue, int.MinValue);
		base.tileCountPerChunk = tileCountPerChunk;
		_createCountPerFrame = createCountPerFrame;
		_descAsset = new List<Asset>();
		sceneDesc = new WorldSceneDesc();
		loadCount = 0;
		finishCount = 0;
		enableChunkUnload = false;
		if (!string.IsNullOrEmpty(descPath))
		{
			Append(descPath, 0f);
		}
	}

	public override void InitLW(int tileCountPerChunk, int createCountPerFrame)
	{
		parentNode = new GameObject("PVEStatic").transform;
		lastViewChunk = new Vector2Int(int.MinValue, int.MinValue);
		base.tileCountPerChunk = tileCountPerChunk;
		_createCountPerFrame = createCountPerFrame;
		_descAsset = new List<Asset>();
		sceneDesc = new WorldSceneDesc();
		loadCount = 0;
		finishCount = 0;
		_lwObjId = 10000;
	}

	public override void Append(string descPath, float offset)
	{
		loadCount++;
		string path = descPath;
		if (!descPath.StartsWith("Assets/"))
		{
			path = $"Assets/Main/Prefabs/PVELevel/{descPath}/decoration.bytes";
		}
		Asset req = GameEntry.Resource.LoadAssetAsync(path, typeof(TextAsset));
		_descAsset.Add(req);
		Asset asset = req;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			finishCount++;
			TextAsset textAsset = req.asset as TextAsset;
			if (sceneDesc != null && textAsset != null)
			{
				List<WorldSceneDesc.ObjectDesc> list = new List<WorldSceneDesc.ObjectDesc>();
				sceneDesc.LoadWithPool(textAsset.bytes, _stack, list);
				for (int i = 0; i < list.Count; i++)
				{
					WorldSceneDesc.ObjectDesc objectDesc = list[i];
					objectDesc.id = ++_lwObjId;
					objectDesc.localPos += new Vector3(0f, 0f, offset);
					Vector2Int key = TilePosToChunkCoord(TileCoord.WorldToTile(objectDesc.localPos));
					if (_chunks.TryGetValue(key, out var value))
					{
						value.Add(objectDesc);
					}
					else
					{
						value = new List<WorldSceneDesc.ObjectDesc> { objectDesc };
						_chunks.Add(key, value);
					}
				}
			}
			req.Release();
			if (_descAsset != null)
			{
				_descAsset.Remove(req);
			}
		});
	}

	public override void UnInit()
	{
		if (_descAsset != null)
		{
			int i = 0;
			for (int count = _descAsset.Count; i < count; i++)
			{
				_descAsset[i].Release();
			}
			_descAsset = null;
		}
		foreach (StaticObject value in _objsDict.Values)
		{
			value.Unload();
		}
		_objsDict.Clear();
		UnityEngine.Object.Destroy(parentNode.gameObject);
		parentNode = null;
		_stack.Clear();
		_staticObjectStack.Clear();
		_createList.Clear();
		_chunks.Clear();
		sceneDesc = null;
		chunksToRemove.Clear();
		enableChunkUnload = false;
		base.UnInit();
	}

	public override void OnUpdate(int viewX, int viewY)
	{
		dataViewX = viewX;
		dataViewY = viewY;
		UpdateView(viewX, viewY);
	}

	public override void OnUpdateData(int viewX, int viewY, int dataViewX, int dataViewY)
	{
		base.dataViewX = dataViewX;
		base.dataViewY = dataViewY;
		try
		{
			UpdateView(viewX, viewY);
		}
		catch (Exception arg)
		{
			Log.Error($"OnUpdateData error : {arg}");
		}
	}

	private void UpdateView(int viewX, int viewY)
	{
		if (loadCount > finishCount)
		{
			return;
		}
		Vector2Int tilePos = new Vector2Int(viewX, viewY);
		Vector2Int vector2Int = TilePosToChunkCoord(tilePos);
		Vector2Int tilePos2 = new Vector2Int(dataViewX, dataViewY);
		Vector2Int vector2Int2 = TilePosToChunkCoord(tilePos2);
		if (lastViewChunk != vector2Int2 || lastVisibleChunkRange != visibleChunkRange)
		{
			lastViewChunk = vector2Int2;
			lastVisibleChunkRange = visibleChunkRange;
			int num = vector2Int.x - visibleChunkRange;
			int num2 = vector2Int.x + visibleChunkRange;
			int num3 = vector2Int.y - ((!enableChunkUnload) ? visibleChunkRange : 0);
			int num4 = vector2Int.y + visibleChunkRange;
			int num5 = vector2Int2.y - ((!enableChunkUnload) ? visibleChunkRange : 0);
			foreach (KeyValuePair<int, StaticObject> item in _objsDict)
			{
				StaticObject value = item.Value;
				Vector2Int vector2Int3 = TilePosToChunkCoord(value.TilePos);
				if (vector2Int3.x < num || vector2Int3.x > num2 || vector2Int3.y < num3 || vector2Int3.y > num4)
				{
					objsToRemove.Add(value.Id);
					value.Unload();
					_staticObjectStack.Push(value);
					continue;
				}
				int dataChunkY = value.DataChunkY;
				if (dataChunkY < num5)
				{
					num5 = dataChunkY;
				}
			}
			if (objsToRemove.Count > 0)
			{
				for (int i = 0; i < objsToRemove.Count; i++)
				{
					_objsDict.Remove(objsToRemove[i]);
				}
				objsToRemove.Clear();
			}
			foreach (KeyValuePair<int, WorldSceneDesc.ObjectDesc> create in _createList)
			{
				WorldSceneDesc.ObjectDesc value2 = create.Value;
				Vector2Int vector2Int4 = TilePosToChunkCoord(TileCoord.WorldToTile(value2.localPos + Vector3.forward * renderOffsetZ));
				if (vector2Int4.x < num || vector2Int4.x > num2 || vector2Int4.y < num3 || vector2Int4.y > num4)
				{
					objsToRemove.Add(create.Key);
				}
			}
			if (objsToRemove.Count > 0)
			{
				for (int j = 0; j < objsToRemove.Count; j++)
				{
					_createList.Remove(objsToRemove[j]);
				}
				objsToRemove.Clear();
			}
			Vector3 a = TileCoord.TileToWorld(tilePos2, 0);
			for (int k = ((!enableChunkUnload) ? (-visibleChunkRange) : 0); k <= visibleChunkRange; k++)
			{
				for (int l = -visibleChunkRange; l <= visibleChunkRange; l++)
				{
					Vector2Int chunkCoord = vector2Int2 + new Vector2Int(l, k);
					List<WorldSceneDesc.ObjectDesc> chunkObjList = GetChunkObjList(chunkCoord);
					if (chunkObjList == null)
					{
						continue;
					}
					for (int m = 0; m < chunkObjList.Count; m++)
					{
						WorldSceneDesc.ObjectDesc objectDesc = chunkObjList[m];
						if (!_objsDict.ContainsKey(objectDesc.id) && !_createList.ContainsKey(objectDesc.id))
						{
							objectDesc.distance = Vector3.Distance(a, objectDesc.localPos);
							_createList.Add(objectDesc.id, objectDesc);
							Vector2Int vector2Int5 = TilePosToChunkCoord(TileCoord.WorldToTile(objectDesc.localPos));
							if (vector2Int5.y < num5)
							{
								num5 = vector2Int5.y;
							}
						}
					}
				}
			}
			if (enableChunkUnload)
			{
				foreach (Vector2Int key2 in _chunks.Keys)
				{
					if (key2.y < num5 - 1)
					{
						chunksToRemove.Add(key2);
					}
				}
				if (chunksToRemove.Count > 0)
				{
					for (int n = 0; n < chunksToRemove.Count; n++)
					{
						Vector2Int key = chunksToRemove[n];
						if (_chunks.TryGetValue(key, out var value3))
						{
							ClearChunkToMove(value3);
							_chunks.Remove(key);
						}
					}
					chunksToRemove.Clear();
				}
			}
		}
		if (_createList.Count > 0)
		{
			List<WorldSceneDesc.ObjectDesc> list = _createList.Values.ToList();
			list.Sort(delegate(WorldSceneDesc.ObjectDesc objectDesc3, WorldSceneDesc.ObjectDesc b)
			{
				if (objectDesc3.distance > b.distance)
				{
					return 1;
				}
				return (objectDesc3.distance < b.distance) ? (-1) : 0;
			});
			int num6 = 0;
			int num7 = 0;
			for (int count = list.Count; num7 < count; num7++)
			{
				WorldSceneDesc.ObjectDesc objectDesc2 = list[num7];
				objsToRemove.Add(objectDesc2.id);
				Vector2Int vector2Int6 = TilePosToChunkCoord(TileCoord.WorldToTile(objectDesc2.localPos));
				StaticObject staticObject = null;
				if (_staticObjectStack != null && _staticObjectStack.Count > 0)
				{
					staticObject = _staticObjectStack.Pop();
					staticObject.Init(objectDesc2, vector2Int6.y, renderOffsetZ);
				}
				else
				{
					staticObject = new StaticObject(objectDesc2, vector2Int6.y, renderOffsetZ);
				}
				if (!_objsDict.ContainsKey(staticObject.Id))
				{
					staticObject.Load(parentNode, useLightMap, lightMapData);
					_objsDict.Add(staticObject.Id, staticObject);
					num6++;
					if (num6 > _createCountPerFrame)
					{
						break;
					}
				}
				else
				{
					Log.Error("same key allready exist {0}, {1}", staticObject.Id, objectDesc2.id);
					staticObject.Unload();
					_staticObjectStack?.Push(staticObject);
				}
			}
		}
		for (int num8 = 0; num8 < objsToRemove.Count; num8++)
		{
			_createList.Remove(objsToRemove[num8]);
		}
		objsToRemove.Clear();
	}

	private List<WorldSceneDesc.ObjectDesc> GetChunkObjList(Vector2Int chunkCoord)
	{
		if (_chunks.TryGetValue(chunkCoord, out var value))
		{
			return value;
		}
		return null;
	}

	public override void SetRenderOffsetZ(float renderOffsetZ)
	{
		base.SetRenderOffsetZ(renderOffsetZ);
		foreach (StaticObject value in _objsDict.Values)
		{
			value.SetRenderOffsetZ(renderOffsetZ);
		}
	}
}
