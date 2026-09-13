using System.Collections.Generic;
using FibMatrix;
using UnityEngine;
using XLua;

public class MultiKillPointManager : WorldManagerBase
{
	private ObjectPool<MultiKillPoint> pointPool;

	public ObjectPool<MultiKillTask> taskPool;

	private ObjectPool<MultiKillCreateNode> multiKillCreateNodePool;

	private Dictionary<int, MultiKillPoint> allPoints;

	private List<MultiKillPoint> toRecycle;

	public int CAPACITY = 3;

	private int DELAY_DELETE_TIME = 5;

	public bool isShow = true;

	private const int LOD_LEVEL = 4;

	private Dictionary<int, int> killNum2PinTime;

	private List<int> killNumList;

	private Transform multiKillUIContainer;

	public Transform MultiKillUIContainer
	{
		get
		{
			if (multiKillUIContainer == null)
			{
				GameObject gameObject = GameObject.Find("GameFramework/UI/MultiKillUIContainer");
				if (gameObject == null)
				{
					GameObject gameObject2 = GameObject.Find("GameFramework/UI/WorldUIContainer");
					gameObject = Object.Instantiate(gameObject2, gameObject2.transform.parent, worldPositionStays: true);
					gameObject.name = "MultiKillUIContainer";
					gameObject.GetComponent<Canvas>().sortingOrder = 220;
				}
				multiKillUIContainer = gameObject.transform;
			}
			return multiKillUIContainer;
		}
	}

	public MultiKillPointManager(WorldScene scene)
		: base(scene)
	{
	}

	public int GetDelayDeleteTime(bool isPin, int killNum)
	{
		if (!isPin)
		{
			return DELAY_DELETE_TIME;
		}
		return GetPinTime(killNum);
	}

	public override void Init()
	{
		allPoints = new Dictionary<int, MultiKillPoint>();
		pointPool = new ObjectPool<MultiKillPoint>();
		taskPool = new ObjectPool<MultiKillTask>();
		multiKillCreateNodePool = new ObjectPool<MultiKillCreateNode>();
		toRecycle = new List<MultiKillPoint>();
		CAPACITY = GameEntry.Lua.CallWithReturn<int, string, string>("CSharpCallLuaInterface.GetConfigNum", "killstreak_report", "k2");
		CAPACITY = ((CAPACITY == 0) ? 3 : CAPACITY);
		DELAY_DELETE_TIME = GameEntry.Lua.CallWithReturn<int, string, string>("CSharpCallLuaInterface.GetConfigNum", "killstreak_report", "k1");
		DELAY_DELETE_TIME = ((DELAY_DELETE_TIME == 0) ? 3 : DELAY_DELETE_TIME);
		GameEntry.Event.Subscribe(EventId.ChangeCameraLod, OnLodChanged);
		int lodLevel = world.GetLodLevel();
		isShow = lodLevel <= 4;
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllMultiKillPinConfig");
		killNum2PinTime = new Dictionary<int, int>(8);
		killNumList = new List<int>(8);
		if (luaTable != null)
		{
			luaTable.ForEach(delegate(int killNum, int time)
			{
				killNum2PinTime[killNum] = time;
				killNumList.Add(killNum);
			});
			killNumList.Sort();
		}
	}

	public int GetPinTime(int killNum)
	{
		if (killNumList == null || killNumList.Count == 0)
		{
			return 0;
		}
		for (int i = 1; i < killNumList.Count; i++)
		{
			if (killNum < killNumList[i])
			{
				return killNum2PinTime[killNumList[i - 1]];
			}
		}
		return killNum2PinTime[killNumList[killNumList.Count - 1]];
	}

	public override void UnInit()
	{
		foreach (MultiKillPoint item in toRecycle)
		{
			allPoints.Remove(item.pointId);
			pointPool.Recycle(item);
		}
		toRecycle.Clear();
		pointPool.RecycleNoClear(allPoints.Values);
		pointPool.Dispose();
		taskPool.Dispose();
		multiKillCreateNodePool.Dispose();
		allPoints.Clear();
		multiKillCreateNodePool = null;
		taskPool = null;
		toRecycle = null;
		pointPool = null;
		allPoints = null;
		BehaviourTreeManager.GetInstance().Clear();
		multiKillUIContainer = null;
		GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, OnLodChanged);
	}

	private void OnLodChanged(object userdata)
	{
		isShow = (int)userdata <= 4;
	}

	public void MultiKillPointAddTask(MultiKillBubbleData data)
	{
		if (!allPoints.TryGetValue(data.pointId, out var value))
		{
			value = pointPool.Allocate();
			value.Init(this, data.pointId);
			allPoints[data.pointId] = value;
		}
		value.AddOrRefreshAORTask(data);
	}

	public override void OnUpdate(float deltaTime)
	{
		if (allPoints == null)
		{
			return;
		}
		foreach (MultiKillPoint value in allPoints.Values)
		{
			value.OnUpdate();
		}
		if (toRecycle.Count <= 0)
		{
			return;
		}
		foreach (MultiKillPoint item in toRecycle)
		{
			allPoints.Remove(item.pointId);
			pointPool.Recycle(item);
		}
		toRecycle.Clear();
	}

	public void MultiKillDataRecycle(MultiKillBubbleData data)
	{
		world.MultiKillDataRecycle(data);
	}

	public void RecycleMultiKillCreateNode(MultiKillCreateNode node)
	{
		multiKillCreateNodePool.Recycle(node);
	}

	public MultiKillCreateNode CreateMultiKillCreateNode(MultiKillPoint point, int index)
	{
		return multiKillCreateNodePool.Allocate().Init(point, index);
	}

	public void Recycle(MultiKillPoint point)
	{
		toRecycle.Add(point);
	}
}
