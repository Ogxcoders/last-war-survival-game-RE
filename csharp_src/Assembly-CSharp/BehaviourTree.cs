using System;

public class BehaviourTree : IDisposable
{
	private BTNode _root;

	private BTNodeState _treeState;

	public BehaviourTree Init(BTNode root)
	{
		_treeState = BTNodeState.Running;
		_root = root;
		return this;
	}

	public BTNodeState Tick()
	{
		if (_treeState == BTNodeState.Running)
		{
			_treeState = _root.Tick();
		}
		return _treeState;
	}

	public void Recycle()
	{
		_root.Recycle();
		_root = null;
		BehaviourTreeManager.GetInstance().RecycleTree(this);
	}

	public void Dispose()
	{
	}
}
