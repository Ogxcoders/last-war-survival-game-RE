using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class MultiKillPoint : IDisposable
{
	public const float SCALE = 0.5f;

	private int CAPACITY = 4;

	public int pointId;

	private Vector3 worldPos;

	public MultiKillPointManager mgr;

	public Dictionary<long, InstanceRequest> requestInsts = new Dictionary<long, InstanceRequest>();

	public Dictionary<long, MultiKillMonoBehaviour> monos = new Dictionary<long, MultiKillMonoBehaviour>();

	public List<long> index2uuid = new List<long>(4);

	private List<MultiKillTask> taskQueue = new List<MultiKillTask>();

	public MultiKillTask curTask;

	private BehaviourTree btree;

	private MultiKillMonoBehaviour toDelete;

	private Vector3 zeroLocalPos;

	private bool? PIN_FUNCTION_ON;

	public Vector3 WorldPos => worldPos;

	public void Init(MultiKillPointManager mgr, int pointId)
	{
		this.mgr = mgr;
		CAPACITY = mgr.CAPACITY;
		this.pointId = pointId;
		index2uuid.Clear();
		for (int i = 0; i < CAPACITY; i++)
		{
			index2uuid.Add(0L);
		}
		worldPos = SceneManager.World.TileIndexToWorld(pointId);
		InitZeroBubbleLocalPos();
		InitBehaviourTree();
	}

	public void Dispose()
	{
		foreach (MultiKillTask item in taskQueue)
		{
			mgr.taskPool.Recycle(item);
		}
		taskQueue.Clear();
		mgr.taskPool.Recycle(curTask);
		curTask = null;
		btree.Recycle();
		btree = null;
		foreach (InstanceRequest value in requestInsts.Values)
		{
			value.Destroy();
		}
		requestInsts.Clear();
		foreach (MultiKillMonoBehaviour value2 in monos.Values)
		{
			mgr.MultiKillDataRecycle(value2.Data);
			value2.Dispose();
		}
		monos.Clear();
		mgr = null;
		toDelete = null;
		index2uuid.Clear();
	}

	public void OnUpdate()
	{
		if (btree != null && btree.Tick() == BTNodeState.Failure)
		{
			mgr.Recycle(this);
		}
	}

	private void DeleteOneBubble(long uuid)
	{
		if (requestInsts.TryGetValue(uuid, out var value))
		{
			value.Destroy();
		}
		requestInsts.Remove(uuid);
		if (monos.TryGetValue(uuid, out var value2))
		{
			mgr.MultiKillDataRecycle(value2.Data);
			value2.Dispose();
		}
		monos.Remove(uuid);
		for (int i = 0; i < CAPACITY; i++)
		{
			if (index2uuid[i] == uuid)
			{
				index2uuid[i] = 0L;
				break;
			}
		}
	}

	private void InitZeroBubbleLocalPos()
	{
		if (GameEntry.Data.Player.GetWorldId() != 0)
		{
			zeroLocalPos = new Vector3(0f, 2f, 0f);
			return;
		}
		PointInfo pointInfo = SceneManager.World.GetPointInfo(pointId);
		if (pointInfo != null)
		{
			if (pointInfo.pointType == WorldPointType.WORLD_ALLIANCE_CITY)
			{
				zeroLocalPos = new Vector3(0f, 1.7f, 0f);
				return;
			}
			if (pointInfo.pointType == WorldPointType.WORLD_CITY_STRONGHOLD)
			{
				zeroLocalPos = new Vector3(0f, 2.4f, 0f);
				return;
			}
		}
		zeroLocalPos = new Vector3(0f, 1f, 0f);
	}

	public Vector3 GetBubbleLocalPosByIndex(int index)
	{
		return zeroLocalPos + new Vector3(0f, (float)index * 0.5f * 1.4f, 0f);
	}

	public bool IsCurTaskNeedPin()
	{
		if (!PIN_FUNCTION_ON.HasValue)
		{
			PIN_FUNCTION_ON = GameEntry.Data?.Player?.CheckSwitch("killstreak_report_world_top", defaultVal: true) ?? true;
		}
		if (PIN_FUNCTION_ON == false)
		{
			return false;
		}
		if (curTask == null)
		{
			return false;
		}
		if (mgr.GetPinTime(curTask.data.killNum) <= 0)
		{
			return false;
		}
		foreach (MultiKillMonoBehaviour value in monos.Values)
		{
			if (value.Data != null && value.Data.killNum > curTask.data.killNum)
			{
				return false;
			}
		}
		return true;
	}

	private bool HavePinAndCurTaskNotPin()
	{
		if (monos.TryGetValue(index2uuid[0], out var value))
		{
			if (value.isPin)
			{
				return !IsCurTaskNeedPin();
			}
			return false;
		}
		return false;
	}

	private bool IsCurTaskAlreadyPin()
	{
		if (monos.TryGetValue(index2uuid[0], out var value))
		{
			if (value.isPin)
			{
				return curTask.data.marchUuid == index2uuid[0];
			}
			return false;
		}
		return false;
	}

	public void AddOrRefreshAORTask(MultiKillBubbleData data)
	{
		foreach (MultiKillTask item in taskQueue)
		{
			if (item.type == MultiKillTaskType.AddOrRefresh && item.data.marchUuid == data.marchUuid)
			{
				mgr.MultiKillDataRecycle(item.data);
				item.data = data;
				return;
			}
		}
		taskQueue.Add(mgr.taskPool.Allocate().Init(MultiKillTaskType.AddOrRefresh, data));
	}

	public void AddDELTask(MultiKillBubbleData data)
	{
		taskQueue.Insert(0, mgr.taskPool.Allocate().Init(MultiKillTaskType.Delete, data));
	}

	private SelectorNode CreateDoDelNode()
	{
		BehaviourTreeManager instance = BehaviourTreeManager.GetInstance();
		SelectorNode selectorNode = instance.CreateSelectorNode();
		selectorNode.AddChild(instance.CreateConditionNode(delegate
		{
			if (curTask == null)
			{
				Log.Error("[CreateDoDelNode]curTaskIsNull");
				return true;
			}
			if (curTask.data == null)
			{
				Log.Error("[CreateDoDelNode]curTaskDataIsNull");
				return true;
			}
			if (monos.TryGetValue(curTask.data.marchUuid, out var value))
			{
				toDelete = value;
				return false;
			}
			return true;
		}));
		selectorNode.AddChild(instance.CreateSequenceNode(new List<BTNode>
		{
			instance.CreateSimpleActionNode(delegate
			{
				if (toDelete == null)
				{
					Log.Error("[CreateDoDelNode]toDeleteIsNull");
				}
				else
				{
					toDelete.FlyOut();
				}
			}),
			instance.CreateWaitNode(0.25f),
			instance.CreateSimpleActionNode(delegate
			{
				if (curTask == null)
				{
					Log.Error("[CreateDoDelNode]curTaskIsNull222");
				}
				else if (curTask.data == null)
				{
					Log.Error("[CreateDoDelNode]curTaskDataIsNull222");
				}
				else
				{
					DeleteOneBubble(curTask.data.marchUuid);
					toDelete = null;
				}
			})
		}));
		return selectorNode;
	}

	private void InitBehaviourTree()
	{
		BehaviourTreeManager instance = BehaviourTreeManager.GetInstance();
		SequenceNode sequenceNode = instance.CreateSequenceNode();
		sequenceNode.AddChild(instance.CreateSelectorNode(new List<BTNode>
		{
			instance.CreateConditionNode(delegate
			{
				for (int i = 1; i < CAPACITY; i++)
				{
					if (index2uuid[i] == 0L)
					{
						return true;
					}
				}
				return false;
			}),
			instance.CreateSequenceNode(new List<BTNode>
			{
				instance.CreateSimpleActionNode(delegate
				{
					monos[index2uuid[CAPACITY - 1]].FlyOut();
				}),
				instance.CreateWaitNode(0.25f),
				instance.CreateSimpleActionNode(delegate
				{
					DeleteOneBubble(index2uuid[CAPACITY - 1]);
				})
			})
		}));
		sequenceNode.AddChild(instance.CreateSimpleActionNode(delegate
		{
			int num = 1;
			for (int i = 1; i < CAPACITY; i++)
			{
				if (index2uuid[i] == 0L)
				{
					num = i;
					break;
				}
			}
			for (int num2 = num - 1; num2 > 0; num2--)
			{
				Vector3 bubbleLocalPosByIndex = GetBubbleLocalPosByIndex(num2 + 1);
				monos[index2uuid[num2]].SetBubbleLocalPos(bubbleLocalPosByIndex.y, 0.6f);
				index2uuid[num2 + 1] = index2uuid[num2];
			}
			index2uuid[1] = 0L;
		}));
		sequenceNode.AddChild(instance.CreateWaitNode(0.5f));
		SelectorNode selectorNode = instance.CreateSelectorNode();
		selectorNode.AddChild(instance.CreateConditionNode(() => index2uuid[1] == 0));
		selectorNode.AddChild(sequenceNode);
		SequenceNode sequenceNode2 = instance.CreateSequenceNode();
		sequenceNode2.AddChild(instance.CreateSelectorNode(new List<BTNode>
		{
			instance.CreateConditionNode(delegate
			{
				foreach (long item in index2uuid)
				{
					if (item == 0L)
					{
						return true;
					}
				}
				return false;
			}),
			instance.CreateSequenceNode(new List<BTNode>
			{
				instance.CreateSimpleActionNode(delegate
				{
					monos[index2uuid[CAPACITY - 1]].FlyOut();
				}),
				instance.CreateWaitNode(0.25f),
				instance.CreateSimpleActionNode(delegate
				{
					DeleteOneBubble(index2uuid[CAPACITY - 1]);
				})
			})
		}));
		sequenceNode2.AddChild(instance.CreateSimpleActionNode(delegate
		{
			int num = 0;
			for (int i = 0; i < CAPACITY; i++)
			{
				if (index2uuid[i] == 0L)
				{
					num = i;
					break;
				}
			}
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				Vector3 bubbleLocalPosByIndex = GetBubbleLocalPosByIndex(num2 + 1);
				long key = index2uuid[num2];
				if (monos.TryGetValue(key, out var value))
				{
					value.SetBubbleLocalPos(bubbleLocalPosByIndex.y, 0.6f);
					if (num2 == 0)
					{
						value.Degenerate();
					}
				}
				index2uuid[num2 + 1] = index2uuid[num2];
			}
			index2uuid[0] = 0L;
		}));
		sequenceNode2.AddChild(instance.CreateWaitNode(0.5f));
		SelectorNode selectorNode2 = instance.CreateSelectorNode();
		selectorNode2.AddChild(instance.CreateConditionNode(() => index2uuid[0] == 0));
		selectorNode2.AddChild(sequenceNode2);
		SequenceNode sequenceNode3 = instance.CreateSequenceNode();
		sequenceNode3.AddChild(instance.CreateSequenceNode(new List<BTNode>
		{
			CreateDoDelNode(),
			instance.CreateSimpleActionNode(delegate
			{
				mgr.taskPool.Recycle(curTask);
				curTask = null;
			})
		}));
		SelectorNode selectorNode3 = instance.CreateSelectorNode();
		selectorNode3.AddChild(instance.CreateSequenceNode(new List<BTNode>
		{
			instance.CreateConditionNode(IsCurTaskAlreadyPin),
			instance.CreateSimpleActionNode(delegate
			{
				MultiKillMonoBehaviour multiKillMonoBehaviour = monos[index2uuid[0]];
				mgr.MultiKillDataRecycle(multiKillMonoBehaviour.Data);
				multiKillMonoBehaviour.ResetData(curTask.data);
			}),
			instance.CreateWaitNode(1.5f),
			instance.CreateSimpleActionNode(delegate
			{
				mgr.taskPool.Recycle(curTask);
				curTask = null;
			})
		}));
		selectorNode3.AddChild(instance.CreateSequenceNode(new List<BTNode>
		{
			instance.CreateConditionNode(HavePinAndCurTaskNotPin),
			CreateDoDelNode(),
			selectorNode,
			mgr.CreateMultiKillCreateNode(this, 1),
			instance.CreateWaitNode(1.5f),
			instance.CreateSimpleActionNode(delegate
			{
				mgr.taskPool.Recycle(curTask);
				curTask = null;
			})
		}));
		selectorNode3.AddChild(instance.CreateSequenceNode(new List<BTNode>
		{
			CreateDoDelNode(),
			selectorNode2,
			mgr.CreateMultiKillCreateNode(this, 0),
			instance.CreateWaitNode(1.5f),
			instance.CreateSimpleActionNode(delegate
			{
				mgr.taskPool.Recycle(curTask);
				curTask = null;
			})
		}));
		SelectorNode selectorNode4 = instance.CreateSelectorNode();
		selectorNode4.AddChild(instance.CreateSequenceNode(new List<BTNode>
		{
			instance.CreateConditionNode(() => (curTask != null && curTask.type == MultiKillTaskType.Delete) ? true : false),
			sequenceNode3
		}));
		selectorNode4.AddChild(instance.CreateSequenceNode(new List<BTNode>
		{
			instance.CreateConditionNode(() => (curTask != null && curTask.type == MultiKillTaskType.AddOrRefresh) ? true : false),
			selectorNode3
		}));
		selectorNode4.AddChild(instance.CreateConditionNode(() => !mgr.isShow));
		selectorNode4.AddChild(instance.CreateSequenceNode(new List<BTNode>
		{
			instance.CreateConditionNode(() => taskQueue.Count > 0),
			instance.CreateSimpleActionNode(delegate
			{
				curTask = taskQueue[0];
				taskQueue.RemoveAt(0);
			})
		}));
		selectorNode4.AddChild(instance.CreateConditionNode(() => requestInsts.Count > 0));
		LoopNode root = instance.CreateLoopNode(selectorNode4);
		btree = instance.CreateTree(root);
	}
}
