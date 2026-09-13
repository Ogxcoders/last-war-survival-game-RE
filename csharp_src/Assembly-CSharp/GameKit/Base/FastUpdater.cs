namespace GameKit.Base;

public sealed class FastUpdater : SingletonBehaviour<FastUpdater>
{
	private FastUpdateSet<IFastUpdate> _updates = new FastUpdateSet<IFastUpdate>();

	private FastUpdateSet<IFastLateUpdate> _lateUpdates = new FastUpdateSet<IFastLateUpdate>();

	private void Update()
	{
		for (int num = _updates.Count - 1; num >= 0; num--)
		{
			_updates[num].DoUpdate();
		}
	}

	private void LateUpdate()
	{
		for (int num = _lateUpdates.Count - 1; num >= 0; num--)
		{
			_lateUpdates[num].DoLateUpdate();
		}
	}

	public void Add(object obj)
	{
		if (obj is IFastUpdate item)
		{
			_updates.Add(item);
		}
		if (obj is IFastLateUpdate item2)
		{
			_lateUpdates.Add(item2);
		}
	}

	public void Remove(object obj)
	{
		if (obj is IFastUpdate item)
		{
			_updates.Remove(item);
		}
		if (obj is IFastLateUpdate item2)
		{
			_lateUpdates.Remove(item2);
		}
	}
}
