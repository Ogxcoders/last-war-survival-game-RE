using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameFramework;
using Protobuf;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Playables;
using VEngine;
using XLua;

public class WorldLandStaticManager : WorldManagerBase
{
	private class LandData
	{
		public int id;

		public int pointId;

		public int state;

		public List<int> nextList;

		public List<Vector2Int> tiles;

		public List<string> alters;

		public List<(string, string)> dynamicObj;

		public LandData()
		{
			id = 0;
			pointId = 0;
			state = 0;
			nextList = new List<int>();
			tiles = new List<Vector2Int>();
			alters = new List<string>();
			dynamicObj = new List<(string, string)>();
		}
	}

	private class Block
	{
		private WorldSceneDesc sceneDesc;

		private Dictionary<Vector2Int, List<WorldSceneDesc.ObjectDesc>> chunks = new Dictionary<Vector2Int, List<WorldSceneDesc.ObjectDesc>>();

		public void Init(byte[] bytes, WorldLandStaticManager mgr)
		{
			sceneDesc = new WorldSceneDesc();
			sceneDesc.Load(bytes);
			for (int i = 0; i < sceneDesc.objectDescs.Count; i++)
			{
				WorldSceneDesc.ObjectDesc objectDesc = sceneDesc.objectDescs[i];
				Vector2Int key = mgr.world.WorldToTile(objectDesc.localPos) / 15;
				key.Clamp(Vector2Int.zero, Vector2Int.one);
				if (chunks.TryGetValue(key, out var value))
				{
					value.Add(objectDesc);
					continue;
				}
				value = new List<WorldSceneDesc.ObjectDesc>();
				value.Add(objectDesc);
				chunks.Add(key, value);
			}
		}

		public void Uninit()
		{
		}

		public List<WorldSceneDesc.ObjectDesc> GetChunkObjList(Vector2Int chunkCoord)
		{
			if (chunks.TryGetValue(chunkCoord, out var value))
			{
				return value;
			}
			return null;
		}
	}

	private class StaticObject
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass3_0
		{
			public LandData landLockData;

			public StaticObject _003C_003E4__this;
		}

		public List<int> tiles;

		private int id;

		private int pointIndex;

		private InstanceRequest request;

		private WorldSceneDesc.ObjectDesc desc;

		private GameObject gameObject;

		private bool isVisible;

		private bool isUnlock;

		private Vector3 position;

		private TouchObjectEventTrigger touchEvent;

		private WorldLandStaticManager mgr;

		private InstanceRequest fadeOutInst;

		private ITimer fadeOutTimer;

		private List<InstanceRequest> dynamicObjReqs;

		public bool IsVisible => isVisible;

		public int Id => id;

		public int PointIndex => pointIndex;

		public StaticObject(WorldSceneDesc.ObjectDesc desc, int pointIndex, Vector3 pos, WorldLandStaticManager mgr)
		{
			id = desc.id;
			this.pointIndex = pointIndex;
			this.desc = desc;
			isVisible = true;
			position = pos;
			this.mgr = mgr;
			isUnlock = false;
			dynamicObjReqs = new List<InstanceRequest>();
		}

		public bool IsHasBuildInRange(Dictionary<int, int> buildRangeDic)
		{
			if (buildRangeDic.Count <= 0)
			{
				return false;
			}
			if (tiles == null || tiles.Count <= 0)
			{
				tiles = new List<int>();
				string templateData = GameEntry.ConfigCache.GetTemplateData("aps_landlock", id, "Tiles");
				string templateData2 = GameEntry.ConfigCache.GetTemplateData("aps_landlock", id, "Pos");
				if (!string.IsNullOrEmpty(templateData) && !string.IsNullOrEmpty(templateData2))
				{
					string[] array = templateData.Split(new char[1] { '|' });
					string[] array2 = templateData2.Split(new char[1] { ';' });
					if (array2.Length == 2)
					{
						Vector2Int vector2Int = SceneManager.World.IndexToTilePos(pointIndex);
						Vector2Int vector2Int2 = new Vector2Int(array2[0].ToInt(), array2[1].ToInt());
						string[] array3 = array;
						for (int i = 0; i < array3.Length; i++)
						{
							string[] array4 = array3[i].Split(new char[1] { ';' });
							if (array4.Length == 2)
							{
								Vector2Int vector2Int3 = new Vector2Int(array4[0].ToInt(), array4[1].ToInt());
								Vector2Int tilePos = vector2Int + vector2Int3 - vector2Int2;
								tiles.Add(SceneManager.World.TilePosToIndex(tilePos));
							}
						}
					}
				}
			}
			foreach (int tile in tiles)
			{
				if (buildRangeDic.ContainsKey(tile))
				{
					return true;
				}
			}
			return false;
		}

		public void Load(Transform parent)
		{
			request = GameEntry.Resource.InstantiateAsync(desc.assetPath);
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
					DoAlter();
					touchEvent = gameObject.GetComponent<TouchObjectEventTrigger>();
					if (touchEvent != null)
					{
						touchEvent.onPointerClick = OnClick;
					}
					if (mgr.IsSelfLockedLand(pointIndex))
					{
						GameEntry.Event.Fire(EventId.LandLockInView, Id);
					}
					string userData = Id + ";" + pointIndex;
					GameEntry.Event.Fire(EventId.LandLockCreate, userData);
				}
			};
		}

		private void DoAlter()
		{
			Transform transform = gameObject.transform.Find("Alter");
			if (transform == null)
			{
				return;
			}
			if (!mgr.IsSelfLockedLand(pointIndex))
			{
				transform.gameObject.SetActive(value: false);
				return;
			}
			transform.gameObject.SetActive(value: true);
			LandData landLockData = mgr._landDatas[pointIndex];
			Transform transform2 = gameObject.transform.Find("Alter");
			if (!(transform2 != null))
			{
				return;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			char[] separator = new char[1] { '_' };
			foreach (string alter2 in landLockData.alters)
			{
				string[] array = alter2.Split(separator, 2);
				if (array.Length >= 2)
				{
					string key = array[0];
					string value = array[1];
					dictionary[key] = value;
				}
			}
			_003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals0;
			foreach (Transform firstTf in transform2)
			{
				string name = firstTf.gameObject.name;
				bool flag = dictionary.ContainsKey(name);
				firstTf.gameObject.SetActive(flag);
				if (!flag)
				{
					continue;
				}
				foreach (Transform secondTf in firstTf)
				{
					string name2 = secondTf.gameObject.name;
					bool active = name2 == dictionary[name];
					secondTf.gameObject.SetActive(active);
					string alter = name + "_" + name2;
					bool flag2 = false;
					foreach (var item in landLockData.dynamicObj)
					{
						if (!(alter == item.Item1))
						{
							continue;
						}
						string prefabPath = item.Item2;
						string prefabName = prefabPath.Split(new char[1] { '/' })[^1].Replace(".prefab", "");
						if (!(secondTf.Find(prefabName) == null) || dynamicObjReqs.Exists((InstanceRequest r) => r.PrefabPath == prefabPath))
						{
							continue;
						}
						flag2 = true;
						InstanceRequest req = GameEntry.Resource.InstantiateAsync(prefabPath);
						req.completed += delegate
						{
							if (gameObject == null || firstTf == null || secondTf == null)
							{
								req.Destroy();
							}
							else
							{
								GameObject obj = req.gameObject;
								obj.name = prefabName;
								obj.transform.SetParent(secondTf);
								obj.transform.localPosition = Vector3.zero;
								obj.transform.localRotation = Quaternion.identity;
								CheckLastTimeLine(secondTf, alter);
							}
						};
						dynamicObjReqs.Add(req);
					}
					if (!flag2)
					{
						CheckLastTimeLine(secondTf, alter);
					}
				}
			}
			void CheckLastTimeLine(Transform transform3, string text)
			{
				if (text == landLockData.alters[landLockData.alters.Count - 1])
				{
					PlayableDirector componentInChildren = transform3.GetComponentInChildren<PlayableDirector>();
					if (componentInChildren != null)
					{
						string tempAlter = text;
						YieldUtils.DelayActionWithOutContext(delegate
						{
							GameEntry.Lua.Call("CSharpCallLuaInterface.LandLockTimeLineFinish", Unsafe.As<_003C_003Ec__DisplayClass3_0, int>(ref CS_0024_003C_003E8__locals0), tempAlter);
						}, (float)componentInChildren.duration);
					}
				}
			}
		}

		public void Update()
		{
			if (mgr.IsSelfLockedLand(pointIndex))
			{
				DoAlter();
			}
		}

		public void Unload()
		{
			if (mgr.IsSelfLockedLand(pointIndex))
			{
				GameEntry.Event.Fire(EventId.LandLockOutView, Id);
			}
			string userData = Id + ";" + pointIndex;
			GameEntry.Event.Fire(EventId.LandLockDestroy, userData);
			desc = null;
			if (touchEvent != null)
			{
				touchEvent.onPointerClick = null;
			}
			request.Destroy();
			if (fadeOutInst != null)
			{
				fadeOutInst.Destroy();
				fadeOutInst = null;
			}
			if (fadeOutTimer != null)
			{
				GameEntry.Timer.CancelTimer(fadeOutTimer);
				fadeOutTimer = null;
			}
		}

		public void FadeOut()
		{
			fadeOutInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/LandLockFadeOut.prefab");
			fadeOutInst.completed += delegate
			{
				fadeOutInst.gameObject.transform.position = mgr.world.TileIndexToWorld(pointIndex);
				fadeOutTimer = GameEntry.Timer.RegisterTimer(2f, delegate
				{
					fadeOutInst.Destroy();
					fadeOutInst = null;
					GameEntry.Timer.CancelTimer(fadeOutTimer);
					fadeOutTimer = null;
					mgr.DestroyObj(pointIndex);
				});
			};
			SetVisible(v: false);
			if (mgr.IsSelfLockedLand(pointIndex))
			{
				GameEntry.Event.Fire(EventId.LandLockOutView, Id);
			}
		}

		public void SetVisible(bool v)
		{
			isVisible = v;
			if (gameObject != null)
			{
				gameObject.SetActive(v);
			}
		}

		private void OnClick()
		{
			if (mgr.IsSelfLockedLand(pointIndex))
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.ClickSelfLandLock", id);
			}
			else
			{
				GameEntry.Lua.Call("UIUtil.OnClickWorld", pointIndex, 0);
			}
		}
	}

	private const int BlockSize = 30;

	private const int CreateCountPerFrame = 20;

	private const int TileCountPerChunk = 15;

	private const int DecorateLod = 1;

	private static Vector2Int StartOffset = new Vector2Int(9, 9);

	private Transform parentNode;

	private int chunkCountPerBlock;

	private Asset descAsset;

	private Block block;

	private Dictionary<int, StaticObject> objsDict = new Dictionary<int, StaticObject>();

	private Dictionary<int, WorldSceneDesc.ObjectDesc> recordCreateList = new Dictionary<int, WorldSceneDesc.ObjectDesc>();

	private Dictionary<int, WorldSceneDesc.ObjectDesc> createList = new Dictionary<int, WorldSceneDesc.ObjectDesc>();

	private List<int> objsToRemove = new List<int>();

	private List<WorldSceneDesc.ObjectDesc> chunkObjList = new List<WorldSceneDesc.ObjectDesc>();

	private Dictionary<int, LandPointInfo> unlockLandDict = new Dictionary<int, LandPointInfo>();

	private Dictionary<int, LandData> _landDatas = new Dictionary<int, LandData>();

	private int lastViewLevel;

	private Vector2Int lastViewChunk;

	private int lastViewRange;

	public WorldLandStaticManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		parentNode = new GameObject("LandStatic").transform;
		parentNode.transform.SetParent(world.Transform, worldPositionStays: false);
		chunkCountPerBlock = 2;
		InitViewChunk();
		InitBlock();
		ResetLandData();
		world.AfterUpdate += UpdateView;
		GameEntry.Event.Subscribe(EventId.LandLockStateUpdate, OnLandLockStateUpdate);
	}

	public override void UnInit()
	{
		if (block != null)
		{
			block.Uninit();
		}
		foreach (KeyValuePair<int, StaticObject> item in objsDict)
		{
			item.Value.Unload();
		}
		objsDict.Clear();
		world.AfterUpdate -= UpdateView;
		if (parentNode != null)
		{
			UnityEngine.Object.Destroy(parentNode.gameObject);
			parentNode = null;
		}
		if (descAsset != null)
		{
			descAsset.Release();
			descAsset = null;
		}
		GameEntry.Event.Unsubscribe(EventId.LandLockStateUpdate, OnLandLockStateUpdate);
	}

	public override void OnUpdate(float deltaTime)
	{
	}

	public void RemoveAllLandPoint()
	{
		foreach (KeyValuePair<int, StaticObject> item in objsDict)
		{
			item.Value.Unload();
		}
		objsDict.Clear();
		ResetLandData();
	}

	public void SetLandPointInfos(List<LandPointInfo> landPointInfos)
	{
		if (landPointInfos == null)
		{
			return;
		}
		bool flag = false;
		unlockLandDict.Clear();
		foreach (LandPointInfo landPointInfo in landPointInfos)
		{
			if (!unlockLandDict.ContainsKey(landPointInfo.Id))
			{
				unlockLandDict.Add(landPointInfo.Id, landPointInfo);
			}
			if (objsDict.TryGetValue(landPointInfo.Id, out var value))
			{
				value.Unload();
				objsDict.Remove(landPointInfo.Id);
			}
			if (_landDatas.ContainsKey(landPointInfo.Id))
			{
				flag = true;
			}
		}
		createList.Clear();
		foreach (KeyValuePair<int, WorldSceneDesc.ObjectDesc> recordCreate in recordCreateList)
		{
			int key = recordCreate.Key;
			_ = recordCreate.Value.id;
			if (!unlockLandDict.ContainsKey(key))
			{
				createList.Add(recordCreate.Key, recordCreate.Value);
			}
		}
		if (flag)
		{
			ResetLandData();
		}
		UpdateView();
	}

	public void HandleLandUpdate(ISFSObject message)
	{
		string utfString = message.GetUtfString("type");
		if (utfString == "create")
		{
			List<LandPointInfo> list = new List<LandPointInfo>();
			ISFSArray sFSArray = message.GetSFSArray("points");
			if (sFSArray != null)
			{
				for (int i = 0; i < sFSArray.Count; i++)
				{
					LandPointInfo item = LandPointInfo.Parser.ParseFrom(sFSArray.GetByteArray(i).Bytes);
					list.Add(item);
				}
			}
			bool flag = false;
			foreach (LandPointInfo item2 in list)
			{
				if (!unlockLandDict.ContainsKey(item2.Id))
				{
					unlockLandDict.Add(item2.Id, item2);
				}
				if (objsDict.TryGetValue(item2.Id, out var value))
				{
					value.FadeOut();
				}
				if (_landDatas.ContainsKey(item2.Id))
				{
					flag = true;
				}
			}
			if (flag)
			{
				ResetLandData();
			}
		}
		else
		{
			if (!(utfString == "remove"))
			{
				return;
			}
			bool flag2 = false;
			ISFSArray sFSArray2 = message.GetSFSArray("pointIds");
			for (int j = 0; j < sFSArray2.Count; j++)
			{
				int key = sFSArray2.GetInt(j);
				if (objsDict.TryGetValue(key, out var value2))
				{
					value2.Unload();
					objsDict.Remove(key);
				}
				if (_landDatas.ContainsKey(key))
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				ResetLandData();
			}
		}
	}

	private void InitViewChunk()
	{
		lastViewChunk = new Vector2Int(int.MinValue, int.MinValue);
		lastViewLevel = int.MinValue;
		lastViewRange = GetVisibleChunkRange();
	}

	private void InitBlock()
	{
		descAsset = GameEntry.Resource.LoadAssetAsync("Assets/Main/Scenes/LandLockDesc.bytes", typeof(TextAsset));
		Asset asset = descAsset;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			block = new Block();
			block.Init(descAsset.Get<TextAsset>().bytes, this);
			descAsset.Release();
			descAsset = null;
		});
	}

	public void ResetLandData()
	{
		_landDatas.Clear();
		Vector2Int basePos = GameEntry.Data.Building.GetMainPos() + Vector2Int.one;
		GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetLandLockDataListByState", -1).ForEach(delegate(int _, LuaTable data)
		{
			LandData landData = new LandData
			{
				id = data.Get<int>("id"),
				state = data.Get<int>("state")
			};
			LuaTable luaTable = data.Get<LuaTable>("pos");
			int x = luaTable.Get<int>("x");
			int y = luaTable.Get<int>("y");
			landData.nextList.Clear();
			LuaTable luaTable2 = data.Get<LuaTable>("nextList");
			for (int i = 1; i <= luaTable2.Length; i++)
			{
				landData.nextList.Add(luaTable2.Get<int>(i));
			}
			landData.alters.Clear();
			LuaTable luaTable3 = data.Get<LuaTable>("alters");
			for (int j = 1; j <= luaTable3.Length; j++)
			{
				landData.alters.Add(luaTable3.Get<string>(j));
			}
			landData.dynamicObj.Clear();
			LuaTable luaTable4 = data.Get<LuaTable>("dynamicObj");
			for (int k = 1; k <= luaTable4.Length; k++)
			{
				LuaTable luaTable5 = luaTable4.Get<LuaTable>(k);
				string item = luaTable5.Get<string>(1);
				string item2 = luaTable5.Get<string>(2);
				landData.dynamicObj.Add((item, item2));
			}
			string templateData = GameEntry.ConfigCache.GetTemplateData("aps_landlock", landData.id, "Tiles");
			if (!string.IsNullOrEmpty(templateData))
			{
				string[] array = templateData.Split(new char[1] { '|' });
				for (int l = 0; l < array.Length; l++)
				{
					string[] array2 = array[l].Split(new char[1] { ';' });
					if (array2.Length == 2)
					{
						landData.tiles.Add(basePos + new Vector2Int(array2[0].ToInt(), array2[1].ToInt()));
					}
				}
			}
			landData.pointId = world.TilePosToIndex(basePos + new Vector2Int(x, y));
			if (!_landDatas.ContainsKey(landData.pointId))
			{
				_landDatas.Add(landData.pointId, landData);
			}
			else
			{
				Log.Error($"Land point already exist {landData.pointId}");
			}
		});
	}

	private Vector2Int TilePosToChunkCoord(Vector2Int tilePos)
	{
		Vector2Int vector2Int = tilePos - StartOffset;
		vector2Int.x = Math.Max(0, vector2Int.x);
		vector2Int.y = Math.Max(0, vector2Int.y);
		return vector2Int / 15;
	}

	private Vector2Int TilePosToBlockCoord(Vector2Int tilePos)
	{
		Vector2Int vector2Int = tilePos - StartOffset;
		vector2Int.x = Math.Max(0, vector2Int.x);
		vector2Int.y = Math.Max(0, vector2Int.y);
		return vector2Int / 30;
	}

	private Vector2Int ChunkCoordToBlockCoord(Vector2Int chunkCoord)
	{
		chunkCoord.x = Math.Max(0, chunkCoord.x);
		chunkCoord.y = Math.Max(0, chunkCoord.y);
		return new Vector2Int(chunkCoord.x / chunkCountPerBlock, chunkCoord.y / chunkCountPerBlock);
	}

	private void GetChunkObjList(Vector2Int chunkCoord, List<WorldSceneDesc.ObjectDesc> list)
	{
		list.Clear();
		Vector2Int vector2Int = ChunkCoordToBlockCoord(chunkCoord);
		if (block != null)
		{
			Vector2Int vector2Int2 = new Vector2Int(vector2Int.x * chunkCountPerBlock, vector2Int.y * chunkCountPerBlock);
			List<WorldSceneDesc.ObjectDesc> list2 = block.GetChunkObjList(chunkCoord - vector2Int2);
			if (list2 != null)
			{
				list.AddRange(list2);
			}
		}
	}

	public bool IsSelfLockedLand(int pointIndex)
	{
		if (_landDatas.TryGetValue(pointIndex, out var value))
		{
			return value.state != 5;
		}
		return false;
	}

	public bool IsInSelfLandBlock(int pointIndex)
	{
		Vector2Int item = world.IndexToTilePos(pointIndex);
		foreach (LandData value in _landDatas.Values)
		{
			if (value.tiles.Contains(item) && value.state != 5)
			{
				return true;
			}
		}
		return false;
	}

	public void DestroyObj(int pointIndex)
	{
		if (objsDict.TryGetValue(pointIndex, out var value) && value != null)
		{
			value.Unload();
			objsDict.Remove(pointIndex);
		}
	}

	private int GetVisibleChunkRange()
	{
		return 1;
	}

	private void UpdateView()
	{
		bool flag = false;
		bool flag2 = false;
		Vector2Int curTilePos = world.CurTilePos;
		Vector2Int vector2Int = TilePosToChunkCoord(curTilePos);
		int lodLevel = world.GetLodLevel();
		int visibleChunkRange = GetVisibleChunkRange();
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		if (lastViewChunk != vector2Int)
		{
			lastViewChunk = vector2Int;
			flag = true;
		}
		if (lastViewLevel != lodLevel || lastViewRange != visibleChunkRange)
		{
			lastViewLevel = lodLevel;
			lastViewRange = visibleChunkRange;
			flag2 = true;
		}
		if (lodLevel <= 1)
		{
			if (flag || flag2)
			{
				int visibleChunkRange2 = GetVisibleChunkRange();
				int num = vector2Int.x - visibleChunkRange2;
				int num2 = vector2Int.x + visibleChunkRange2;
				int num3 = vector2Int.y - visibleChunkRange2;
				int num4 = vector2Int.y + visibleChunkRange2;
				foreach (KeyValuePair<int, StaticObject> item in objsDict)
				{
					int key = item.Key;
					Vector2Int tilePos = world.IndexToTilePos(key);
					Vector2Int vector2Int2 = TilePosToChunkCoord(tilePos);
					bool flag3 = lodLevel <= 1;
					if (vector2Int2.x < num || vector2Int2.x > num2 || vector2Int2.y < num3 || vector2Int2.y > num4 || !flag3)
					{
						objsToRemove.Add(key);
						item.Value.Unload();
					}
				}
				if (objsToRemove.Count > 0)
				{
					for (int i = 0; i < objsToRemove.Count; i++)
					{
						objsDict.Remove(objsToRemove[i]);
					}
					objsToRemove.Clear();
				}
				foreach (KeyValuePair<int, WorldSceneDesc.ObjectDesc> recordCreate in recordCreateList)
				{
					int key2 = recordCreate.Key;
					Vector2Int tilePos2 = world.IndexToTilePos(key2);
					Vector2Int vector2Int3 = TilePosToChunkCoord(tilePos2);
					bool flag4 = lodLevel <= 1;
					if (vector2Int3.x < num || vector2Int3.x > num2 || vector2Int3.y < num3 || vector2Int3.y > num4 || !flag4)
					{
						objsToRemove.Add(key2);
					}
				}
				if (objsToRemove.Count > 0)
				{
					for (int j = 0; j < objsToRemove.Count; j++)
					{
						recordCreateList.Remove(objsToRemove[j]);
					}
					objsToRemove.Clear();
				}
				foreach (KeyValuePair<int, WorldSceneDesc.ObjectDesc> create in createList)
				{
					int key3 = create.Key;
					Vector2Int tilePos3 = world.IndexToTilePos(key3);
					Vector2Int vector2Int4 = TilePosToChunkCoord(tilePos3);
					bool flag5 = lodLevel <= 1;
					if (vector2Int4.x < num || vector2Int4.x > num2 || vector2Int4.y < num3 || vector2Int4.y > num4 || !flag5)
					{
						objsToRemove.Add(key3);
					}
				}
				if (objsToRemove.Count > 0)
				{
					for (int k = 0; k < objsToRemove.Count; k++)
					{
						createList.Remove(objsToRemove[k]);
					}
					objsToRemove.Clear();
				}
				for (int l = -visibleChunkRange2; l <= visibleChunkRange2; l++)
				{
					for (int m = -visibleChunkRange2; m <= visibleChunkRange2; m++)
					{
						Vector2Int chunkCoord = vector2Int + new Vector2Int(m, l);
						GetChunkObjList(chunkCoord, chunkObjList);
						if (chunkObjList.Count <= 0)
						{
							continue;
						}
						Vector2Int vector2Int5 = ChunkCoordToBlockCoord(chunkCoord);
						for (int n = 0; n < chunkObjList.Count; n++)
						{
							WorldSceneDesc.ObjectDesc objectDesc = chunkObjList[n];
							_ = objectDesc.id;
							int type = objectDesc.type;
							Vector2Int vector2Int6 = StartOffset + vector2Int5 * 30 + new Vector2Int(16, 16) + (TileCoord.IndexToTilePos(type, new Vector2Int(100, 100)) - new Vector2Int(50, 50));
							int num5 = world.TilePosToIndex(vector2Int6);
							bool flag6 = lodLevel <= 1;
							if (!objsDict.ContainsKey(num5) && !recordCreateList.ContainsKey(num5) && world.IsInMap(vector2Int6) && !world.IsPointInAllianceCity(num5, curServerId) && flag6 && !unlockLandDict.ContainsKey(num5))
							{
								recordCreateList.Add(num5, objectDesc);
							}
						}
					}
				}
			}
		}
		else
		{
			if (objsDict.Count > 0)
			{
				foreach (KeyValuePair<int, StaticObject> item2 in objsDict)
				{
					item2.Value.Unload();
				}
				objsDict.Clear();
			}
			if (recordCreateList.Count > 0)
			{
				recordCreateList.Clear();
			}
			if (createList.Count > 0)
			{
				createList.Clear();
			}
		}
		if (createList.Count <= 0)
		{
			return;
		}
		int num6 = 0;
		Dictionary<int, int> specialPointDic = SceneManager.World.GetSpecialPointDic();
		foreach (KeyValuePair<int, WorldSceneDesc.ObjectDesc> create2 in createList)
		{
			int key4 = create2.Key;
			Vector2Int tilePos4 = world.IndexToTilePos(key4);
			WorldSceneDesc.ObjectDesc value = create2.Value;
			objsToRemove.Add(create2.Key);
			StaticObject staticObject = null;
			if (lodLevel <= 1)
			{
				Vector2Int vector2Int7 = TilePosToBlockCoord(tilePos4);
				Vector3 vector = world.TileFloatToWorld(StartOffset + vector2Int7 * 30);
				staticObject = new StaticObject(value, key4, vector + value.localPos, this);
			}
			if (staticObject != null && !objsDict.ContainsKey(key4) && !unlockLandDict.ContainsKey(key4) && !world.IsPointInAllianceCity(key4, curServerId) && (!staticObject.IsHasBuildInRange(specialPointDic) || _landDatas.ContainsKey(key4)))
			{
				staticObject.Load(parentNode);
				objsDict.Add(create2.Key, staticObject);
				if (num6 > 20)
				{
					break;
				}
				num6++;
			}
		}
		for (int num7 = 0; num7 < objsToRemove.Count; num7++)
		{
			createList.Remove(objsToRemove[num7]);
		}
		objsToRemove.Clear();
	}

	public void OnLandLockStateUpdate(object o)
	{
		ResetLandData();
		int num = Convert.ToInt32(o);
		if (objsDict.TryGetValue(num, out var value) && IsSelfLockedLand(num))
		{
			value.Update();
			GameEntry.Event.Fire(EventId.LandLockInView, value.Id);
		}
	}
}
