using UnityEngine;

public class WaitNode : ActionNode
{
	private float _duration;

	private float _endTime;

	public WaitNode Init(float duration)
	{
		_duration = duration;
		return this;
	}

	protected override void OnEnter()
	{
		_endTime = Time.time + _duration;
	}

	protected override BTNodeState OnUpdate()
	{
		if (!(Time.time < _endTime))
		{
			return BTNodeState.Success;
		}
		return BTNodeState.Running;
	}

	public override void Recycle()
	{
		BehaviourTreeManager.GetInstance().RecycleWaitNode(this);
	}

	public override void Dispose()
	{
		_duration = 0f;
		_endTime = 0f;
	}
}
