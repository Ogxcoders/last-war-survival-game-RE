using System;

public abstract class BTNode : IDisposable
{
	private bool _entered;

	public BTNodeState Tick()
	{
		if (!_entered)
		{
			OnEnter();
			_entered = true;
		}
		BTNodeState num = OnUpdate();
		if (num != BTNodeState.Running)
		{
			_entered = false;
		}
		return num;
	}

	protected virtual void OnEnter()
	{
	}

	protected abstract BTNodeState OnUpdate();

	public virtual void Recycle()
	{
	}

	public virtual void Dispose()
	{
	}
}
