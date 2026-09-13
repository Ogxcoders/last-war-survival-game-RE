using System.Collections.Generic;

public class SelectorNode : BTNode
{
	private List<BTNode> _children = new List<BTNode>();

	private int _curIndex;

	public SelectorNode Init(List<BTNode> children)
	{
		_children = children;
		return this;
	}

	public void AddChild(BTNode child)
	{
		_children.Add(child);
	}

	protected override void OnEnter()
	{
		_curIndex = 0;
	}

	protected override BTNodeState OnUpdate()
	{
		while (_curIndex < _children.Count)
		{
			switch (_children[_curIndex].Tick())
			{
			case BTNodeState.Running:
				return BTNodeState.Running;
			case BTNodeState.Success:
				return BTNodeState.Success;
			case BTNodeState.Failure:
				_curIndex++;
				break;
			}
		}
		return BTNodeState.Failure;
	}

	public override void Recycle()
	{
		foreach (BTNode child in _children)
		{
			child.Recycle();
		}
		_children.Clear();
		BehaviourTreeManager.GetInstance().RecycleSelectorNode(this);
	}

	public override void Dispose()
	{
		_curIndex = 0;
	}
}
