public class LoopNode : BTNode
{
	private BTNode _child;

	public LoopNode Init(BTNode child)
	{
		_child = child;
		return this;
	}

	protected override BTNodeState OnUpdate()
	{
		if (_child.Tick() != BTNodeState.Failure)
		{
			return BTNodeState.Running;
		}
		return BTNodeState.Failure;
	}

	public override void Recycle()
	{
		_child.Recycle();
		_child = null;
		BehaviourTreeManager.GetInstance().RecycleLoopNode(this);
	}

	public override void Dispose()
	{
	}
}
