using UnityEngine;

public class MultiKillCreateNode : ActionNode
{
	private MultiKillPoint point;

	private bool isFinish;

	private int index;

	private const string DefaultPrefabPath = "Assets/Main/Prefabs/March/WorldMultiKill.prefab";

	private const string PinPrefabPath = "Assets/Main/Prefabs/March/WorldMultiKillPin.prefab";

	public MultiKillCreateNode Init(MultiKillPoint point, int index)
	{
		this.point = point;
		this.index = index;
		return this;
	}

	protected override void OnEnter()
	{
		isFinish = false;
		string text;
		if (point.IsCurTaskNeedPin())
		{
			text = "Assets/Main/Prefabs/March/WorldMultiKillPin.prefab";
		}
		else
		{
			text = GameEntry.ConfigCache.GetTemplateData("lw_decoration", point.curTask.data.skinId, "model_world");
			if (string.IsNullOrEmpty(text))
			{
				text = "Assets/Main/Prefabs/March/WorldMultiKill.prefab";
			}
		}
		InstanceRequest instanceRequest = GameEntry.Resource.InstantiateAsync(text);
		instanceRequest.completed += OnGameObjectCreate;
		MultiKillBubbleData data = point.curTask.data;
		point.requestInsts[data.marchUuid] = instanceRequest;
	}

	private void OnGameObjectCreate(InstanceRequest req)
	{
		isFinish = true;
		if (point.curTask != null)
		{
			MultiKillBubbleData data = point.curTask.data;
			MultiKillMonoBehaviour component = req.gameObject.GetComponent<MultiKillMonoBehaviour>();
			Transform transform = req.gameObject.transform;
			point.monos[data.marchUuid] = component;
			point.index2uuid[index] = data.marchUuid;
			transform.SetParent(SceneManager.World.DynamicObjNode);
			component.SetBubbleLocalPos(point.GetBubbleLocalPosByIndex(index).y);
			component.SetData(point, data, point.IsCurTaskNeedPin());
		}
	}

	protected override BTNodeState OnUpdate()
	{
		if (!isFinish)
		{
			return BTNodeState.Running;
		}
		return BTNodeState.Success;
	}

	public override void Recycle()
	{
		point.mgr.RecycleMultiKillCreateNode(this);
	}

	public override void Dispose()
	{
		point = null;
	}
}
