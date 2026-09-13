using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using XLua;

public class WorldMonsterLandStaticManager : WorldManagerBase
{
	private class LandData
	{
		public int Id;

		public int PointId;

		public int State;

		public LandData()
		{
			Id = 0;
			PointId = 0;
			State = 0;
		}
	}

	private class StaticObject
	{
		private int id;

		private int pointIndex;

		private InstanceRequest request;

		private WorldSceneDesc.ObjectDesc desc;

		private GameObject gameObject;

		private bool isVisible;

		private bool isUnlock;

		private Vector3 position;

		private TouchObjectEventTrigger touchEvent;

		private WorldMonsterLandStaticManager mgr;

		private InstanceRequest fadeOutInst;

		private ITimer fadeOutTimer;

		public bool IsVisible => isVisible;

		public int Id => id;

		public int PointIndex => pointIndex;

		public StaticObject(int id, int pointIndex, Vector3 pos, WorldMonsterLandStaticManager mgr)
		{
			this.id = id;
			this.pointIndex = pointIndex;
			isVisible = true;
			position = pos;
			this.mgr = mgr;
			isUnlock = false;
		}

		public void Load(Transform parent)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("aps_monsterlock", id, "model");
			request = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Monsters/" + templateData + ".prefab");
			request.completed += delegate
			{
				if (request.gameObject == null)
				{
					Log.Error("gameObject null");
				}
				else
				{
					gameObject = request.gameObject;
					gameObject.transform.SetParent(parent, worldPositionStays: false);
					gameObject.transform.localPosition = position + new Vector3(0f, 0f, 0f);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
					gameObject.SetActive(isVisible);
					Transform transform = gameObject.transform.Find("ModelLabel");
					if (transform != null)
					{
						transform.gameObject.SetActive(value: false);
					}
					touchEvent = gameObject.GetComponentInChildren<TouchObjectEventTrigger>();
					if (touchEvent != null)
					{
						touchEvent.onPointerClick = OnClick;
					}
					if (mgr.IsSelfLockedLand(pointIndex))
					{
						GameEntry.Event.Fire(EventId.MonsterLockInView, Id);
					}
				}
			};
		}

		public void Unload()
		{
			if (mgr.IsSelfLockedLand(pointIndex))
			{
				GameEntry.Event.Fire(EventId.MonsterLockOutView, Id);
			}
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
				GameEntry.Event.Fire(EventId.MonsterLockOutView, Id);
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
				GameEntry.Lua.Call("CSharpCallLuaInterface.ClickMonsterLockById", id);
			}
			else
			{
				GameEntry.Lua.Call("UIUtil.OnClickWorld", pointIndex, 0);
			}
		}
	}

	private Transform parentNode;

	private const int DecorateLod = 1;

	private Dictionary<int, StaticObject> objsDict = new Dictionary<int, StaticObject>();

	private List<int> createList = new List<int>();

	private Dictionary<int, LandData> _landDatas = new Dictionary<int, LandData>();

	public WorldMonsterLandStaticManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		parentNode = new GameObject("MonsterStatic").transform;
		parentNode.transform.SetParent(world.Transform, worldPositionStays: false);
		ResetLandData();
		world.AfterUpdate += UpdateView;
		GameEntry.Event.Subscribe(EventId.MonsterLockStateUpdate, OnMonsterLockStateUpdate);
	}

	public override void UnInit()
	{
		foreach (KeyValuePair<int, StaticObject> item in objsDict)
		{
			item.Value.Unload();
		}
		objsDict.Clear();
		world.AfterUpdate -= UpdateView;
		UnityEngine.Object.Destroy(parentNode.gameObject);
		parentNode = null;
		GameEntry.Event.Unsubscribe(EventId.MonsterLockStateUpdate, OnMonsterLockStateUpdate);
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

	public void ResetLandData()
	{
		_landDatas.Clear();
		_ = GameEntry.Data.Building.GetMainPos() + Vector2Int.one;
		GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetMonsterLockDataList").ForEach(delegate(int _, LuaTable data)
		{
			LandData landData = new LandData
			{
				Id = data.Get<int>("monsterId")
			};
			int num = data.Get<int>("pointId");
			landData.State = data.Get<int>("state");
			landData.PointId = num;
			if (!_landDatas.ContainsKey(landData.PointId))
			{
				_landDatas.Add(landData.PointId, landData);
				createList.Add(num);
			}
			else
			{
				Log.Error($"Land point already exist {landData.PointId}");
			}
		});
	}

	public bool IsSelfLockedLand(int pointIndex)
	{
		if (_landDatas.TryGetValue(pointIndex, out var value))
		{
			return value.State != 5;
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

	private void UpdateView()
	{
		int lodLevel = world.GetLodLevel();
		if (createList.Count > 0)
		{
			foreach (int create in createList)
			{
				int num = create;
				Vector2Int vector2Int = world.IndexToTilePos(create);
				StaticObject staticObject = null;
				if (lodLevel <= 1)
				{
					Vector3 pos = world.TileFloatToWorld(vector2Int);
					staticObject = new StaticObject(_landDatas[num].Id, num, pos, this);
				}
				if (staticObject != null && !objsDict.ContainsKey(num))
				{
					staticObject.Load(parentNode);
					objsDict.Add(num, staticObject);
				}
			}
		}
		createList.Clear();
	}

	public void OnMonsterLockStateUpdate(object o)
	{
		ResetLandData();
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, StaticObject> item in objsDict)
		{
			if (!_landDatas.ContainsKey(item.Value.Id))
			{
				list.Add(item.Key);
			}
		}
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				DestroyObj(list[i]);
			}
			list.Clear();
		}
		int num = Convert.ToInt32(o);
		if (objsDict.TryGetValue(num, out var value) && IsSelfLockedLand(num))
		{
			GameEntry.Event.Fire(EventId.MonsterLockInView, value.Id);
		}
	}
}
