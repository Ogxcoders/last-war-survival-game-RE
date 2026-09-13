using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using VEngine;

public class CityStaticManager : CityManagerBase
{
	private class StaticObject
	{
		private InstanceRequest request;

		public WorldSceneDesc.ObjectDesc desc;

		private GameObject gameObject;

		private bool isVisible;

		private Vector2Int tilePos;

		private Vector3 position;

		public bool IsVisible => isVisible;

		public int Id => desc.id;

		public Vector2Int TilePos => tilePos;

		public StaticObject(WorldSceneDesc.ObjectDesc desc, Vector3 pos)
		{
			this.desc = desc;
			position = pos;
			tilePos = TileCoord.WorldToTile(pos);
			isVisible = true;
		}

		public void Load(Transform parent)
		{
			int property = 0;
			request = GameEntry.Resource.InstantiateAsync(desc.assetPath, ObjectPoolTag.Normal, property);
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
					gameObject.SetActive(isVisible);
					if (desc.assetPath.Contains("ZombieArea"))
					{
						gameObject.name = "ZombieArea" + desc.id;
						GameEntry.Event.Fire(EventId.CityZombieLoad, desc.id);
					}
				}
			};
		}

		public void Unload()
		{
			if (desc.assetPath.Contains("ZombieArea"))
			{
				GameEntry.Event.Fire(EventId.CityZombieUnload, desc.id);
			}
			desc = null;
			request.Destroy();
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

	private const int ObjectExtents = 1;

	private const int CreateCountPerFrame = 20;

	private const int TileCountPerChunk = 20;

	private Transform _parentNode;

	private WorldSceneDesc _sceneDesc;

	private Asset _descAsset;

	private Dictionary<int, StaticObject> _objsDict = new Dictionary<int, StaticObject>();

	private Dictionary<int, WorldSceneDesc.ObjectDesc> _createList = new Dictionary<int, WorldSceneDesc.ObjectDesc>();

	private List<int> objsToRemove = new List<int>();

	private HashSet<Vector2Int> occupyPoints = new HashSet<Vector2Int>();

	private Dictionary<Vector2Int, List<WorldSceneDesc.ObjectDesc>> _chunks = new Dictionary<Vector2Int, List<WorldSceneDesc.ObjectDesc>>();

	public CitySceneType citySceneType = CitySceneType.World;

	private bool isLoadFinish;

	private Vector2Int _lastViewChunk;

	private int _visibleChunkRange = 1;

	private int _lastVisibleChunkRang = 1;

	public CityStaticManager(CityScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		if (citySceneType == CitySceneType.DigDig)
		{
			return;
		}
		_parentNode = new GameObject("Static").transform;
		_parentNode.transform.SetParent(scene.Transform, worldPositionStays: false);
		_lastViewChunk = new Vector2Int(int.MinValue, int.MinValue);
		_sceneDesc = new WorldSceneDesc();
		string assetPath = "Assets/Main/Scenes/CitySceneDesc.bytes";
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && !curSkinMeta.city_deco.IsNullOrEmpty())
		{
			assetPath = curSkinMeta.city_deco;
			if (!GameEntry.Resource.HasAsset(assetPath))
			{
				assetPath = assetPath.Replace("Assets/Main/Scenes/", "Assets/Main/Scenes/SeasonRes/");
			}
		}
		_descAsset = GameEntry.Resource.LoadAssetAsync(assetPath, typeof(TextAsset));
		Asset descAsset = _descAsset;
		descAsset.completed = (Action<Asset>)Delegate.Combine(descAsset.completed, (Action<Asset>)delegate
		{
			if (_descAsset != null)
			{
				TextAsset textAsset = _descAsset.asset as TextAsset;
				if (textAsset == null)
				{
					Log.Error("CityStaticManager::textAsset == null " + assetPath);
				}
				else
				{
					_sceneDesc.Load(textAsset.bytes);
					for (int i = 0; i < _sceneDesc.objectDescs.Count; i++)
					{
						WorldSceneDesc.ObjectDesc objectDesc = _sceneDesc.objectDescs[i];
						Vector2Int key = TilePosToChunkCoord(scene.WorldToTile(objectDesc.localPos));
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
					_descAsset.Release();
					_descAsset = null;
					isLoadFinish = true;
					GameEntry.Event.Fire(EventId.OnCityDescAssetLoaded);
				}
			}
		});
	}

	public override void UnInit()
	{
		if (_descAsset != null)
		{
			_descAsset.Release();
			_descAsset = null;
		}
		foreach (KeyValuePair<int, StaticObject> item in _objsDict)
		{
			item.Value.Unload();
		}
		_objsDict.Clear();
		UnityEngine.Object.Destroy(_parentNode.gameObject);
		_parentNode = null;
	}

	public override void OnUpdate(float deltaTime)
	{
		UpdateView();
	}

	public void AddOccupyPoints(Vector2Int p, Vector2Int size)
	{
		for (int num = p.y; num > p.y - size.y; num--)
		{
			for (int num2 = p.x; num2 > p.x - size.x; num2--)
			{
				Vector2Int vector2Int = new Vector2Int(num2, num);
				if (scene.IsInMap(vector2Int))
				{
					occupyPoints.Add(vector2Int);
				}
			}
		}
		HideObjectInRect(p, size);
	}

	public void RemoveOccupyPoints(Vector2Int p, Vector2Int size)
	{
		for (int num = p.y; num > p.y - size.y; num--)
		{
			for (int num2 = p.x; num2 > p.x - size.x; num2--)
			{
				occupyPoints.Remove(new Vector2Int(num2, num));
			}
		}
		ShowObjectsInRect(p, size);
	}

	private void HideObjectInRect(Vector2Int p, Vector2Int size)
	{
		int num = p.x - (size.x - 1);
		int num2 = num + (size.x - 1);
		int num3 = p.y - (size.y - 1);
		int num4 = num3 + (size.y - 1);
		foreach (StaticObject value in _objsDict.Values)
		{
			int num5 = value.TilePos.x - 1;
			int num6 = value.TilePos.x + 1;
			int num7 = value.TilePos.y - 1;
			int num8 = value.TilePos.y + 1;
			if ((value.desc == null || value.desc.type != 4) && num5 <= num2 && num6 >= num && num7 <= num4 && num8 >= num3)
			{
				value.SetVisible(v: false);
			}
		}
	}

	private void ShowObjectsInRect(Vector2Int p, Vector2Int size)
	{
		int num = p.x - (size.x - 1);
		int num2 = num + (size.x - 1);
		int num3 = p.y - (size.y - 1);
		int num4 = num3 + (size.y - 1);
		foreach (StaticObject value in _objsDict.Values)
		{
			int num5 = value.TilePos.x - 1;
			int num6 = value.TilePos.x + 1;
			int num7 = value.TilePos.y - 1;
			int num8 = value.TilePos.y + 1;
			bool flag = false;
			flag = (value.desc == null || value.desc.type != 4) && IsOccupied(value.TilePos);
			if (num5 <= num2 && num6 >= num && num7 <= num4 && num8 >= num3 && !value.IsVisible && !flag)
			{
				value.SetVisible(v: true);
			}
		}
	}

	private void UpdateView()
	{
		if (!isLoadFinish)
		{
			return;
		}
		Vector2Int curTilePos = scene.CurTilePos;
		Vector2Int vector2Int = TilePosToChunkCoord(curTilePos);
		if (_lastViewChunk != vector2Int || _lastVisibleChunkRang != _visibleChunkRange)
		{
			_lastViewChunk = vector2Int;
			_lastVisibleChunkRang = _visibleChunkRange;
			_ = vector2Int.x;
			_ = _visibleChunkRange;
			_ = vector2Int.x;
			_ = _visibleChunkRange;
			_ = vector2Int.y;
			_ = _visibleChunkRange;
			_ = vector2Int.y;
			_ = _visibleChunkRange;
			for (int i = -_visibleChunkRange; i <= _visibleChunkRange; i++)
			{
				for (int j = -_visibleChunkRange; j <= _visibleChunkRange; j++)
				{
					Vector2Int chunkCoord = vector2Int + new Vector2Int(j, i);
					List<WorldSceneDesc.ObjectDesc> chunkObjList = GetChunkObjList(chunkCoord);
					if (chunkObjList == null)
					{
						continue;
					}
					for (int k = 0; k < chunkObjList.Count; k++)
					{
						WorldSceneDesc.ObjectDesc objectDesc = chunkObjList[k];
						Vector2Int p = scene.WorldToTile(objectDesc.localPos);
						bool flag = false;
						flag = objectDesc.type != 4 && IsOccupied(p);
						if (!_objsDict.ContainsKey(objectDesc.id) && !_createList.ContainsKey(objectDesc.id) && !flag)
						{
							_createList.Add(objectDesc.id, objectDesc);
						}
					}
				}
			}
		}
		if (_createList.Count <= 0)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<int, WorldSceneDesc.ObjectDesc> create in _createList)
		{
			WorldSceneDesc.ObjectDesc value = create.Value;
			objsToRemove.Add(create.Key);
			bool flag2 = false;
			if (value.type != 4 && IsOccupied(scene.WorldToTile(value.localPos)))
			{
				continue;
			}
			StaticObject staticObject = new StaticObject(value, value.localPos);
			if (!_objsDict.ContainsKey(staticObject.Id))
			{
				staticObject.Load(_parentNode);
				_objsDict.Add(staticObject.Id, staticObject);
				num++;
				if (num > 20)
				{
					break;
				}
			}
			else
			{
				Log.Error("same key allready exist {0}, {1}", staticObject.Id, value.id);
			}
		}
		for (int l = 0; l < objsToRemove.Count; l++)
		{
			_createList.Remove(objsToRemove[l]);
		}
		objsToRemove.Clear();
	}

	private bool IsOccupied(Vector2Int p)
	{
		int num = p.x - 1;
		int num2 = p.x + 1;
		int num3 = p.y - 1;
		int num4 = p.y + 1;
		for (int i = num3; i <= num4; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				if (occupyPoints.Contains(new Vector2Int(j, i)))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsTileWalkable(Vector2Int tilePos)
	{
		return true;
	}

	public bool IsTileWalkable(Vector3 worldPos)
	{
		return true;
	}

	private Vector2Int TilePosToChunkCoord(Vector2Int tilePos)
	{
		return new Vector2Int(tilePos.x / 20, tilePos.y / 20);
	}

	public List<WorldSceneDesc.ObjectDesc> GetChunkObjList(Vector2Int chunkCoord)
	{
		if (_chunks.TryGetValue(chunkCoord, out var value))
		{
			return value;
		}
		return null;
	}

	public void SetVisibleChunk(int range)
	{
		_visibleChunkRange = range;
	}

	public void ToggleShow(bool t)
	{
		if (_parentNode != null)
		{
			_parentNode.gameObject.SetActive(t);
		}
	}
}
