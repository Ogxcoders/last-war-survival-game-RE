using System.Collections.Generic;
using FibMatrix;

public class WorldTriggerObjManager : WorldManagerBase
{
	private ObjectPool<WorldTriggerObj> pool;

	private Dictionary<long, WorldTriggerObj> allTriggerObjs;

	public WorldTriggerObjManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		allTriggerObjs = new Dictionary<long, WorldTriggerObj>(32);
		pool = new ObjectPool<WorldTriggerObj>(32);
	}

	public override void UnInit()
	{
		pool.RecycleNoClear(allTriggerObjs.Values);
		pool.Dispose();
		allTriggerObjs.Clear();
	}

	public void CreateOrRefreshOneTrigger(WorldTriggerData data)
	{
		if (allTriggerObjs.TryGetValue(data.uuid, out var value))
		{
			value.RefreshData(data);
			return;
		}
		value = pool.Allocate();
		value.RefreshData(data);
		allTriggerObjs[data.uuid] = value;
	}

	public void RemoveOneTrigger(long uuid)
	{
		if (allTriggerObjs.TryGetValue(uuid, out var value))
		{
			pool.Recycle(value);
			allTriggerObjs.Remove(uuid);
		}
	}
}
