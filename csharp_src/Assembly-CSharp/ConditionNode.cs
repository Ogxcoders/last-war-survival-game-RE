using System;

public class ConditionNode : BTNode
{
	private Func<bool> _conditionFunction;

	public ConditionNode Init(Func<bool> conditionFunction)
	{
		_conditionFunction = conditionFunction;
		return this;
	}

	protected override BTNodeState OnUpdate()
	{
		if (!_conditionFunction())
		{
			return BTNodeState.Failure;
		}
		return BTNodeState.Success;
	}

	public override void Recycle()
	{
		BehaviourTreeManager.GetInstance().RecycleConditionNode(this);
	}

	public override void Dispose()
	{
		_conditionFunction = null;
	}
}
