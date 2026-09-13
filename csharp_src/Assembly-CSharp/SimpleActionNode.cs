using System;

public class SimpleActionNode : ActionNode
{
	private Action _action;

	public SimpleActionNode Init(Action action)
	{
		_action = action;
		return this;
	}

	protected override BTNodeState OnUpdate()
	{
		_action();
		return BTNodeState.Success;
	}

	public override void Recycle()
	{
		BehaviourTreeManager.GetInstance().RecycleSimpleActionNode(this);
	}

	public override void Dispose()
	{
		_action = null;
	}
}
