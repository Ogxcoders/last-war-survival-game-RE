using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class AutoDisposePoolManager
{
	private Dictionary<string, AutoDisposePool> _pools = new Dictionary<string, AutoDisposePool>();

	private DoublyLinkedList _linkedList = new DoublyLinkedList();

	private float _lastTickTime;

	private const float TICK_INTERVAL = 2f;

	private const float DISPOSE_INTERVAL = 10f;

	public void OnPoolSpawn(AutoDisposePool pool)
	{
		_linkedList.MoveToHead(pool);
	}

	public void Tick()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (realtimeSinceStartup - _lastTickTime > 2f)
		{
			_lastTickTime = realtimeSinceStartup;
			_linkedList.TraverseBackward(TickUnusePoolNode);
		}
	}

	public bool TryGetPool(string poolName, out AutoDisposePool pool)
	{
		return _pools.TryGetValue(poolName, out pool);
	}

	public void RegisterPool(AutoDisposePool pool)
	{
		_pools.Add(pool.prefabPath, pool);
		_linkedList.AddFirst(pool);
	}

	private void RemovePool(AutoDisposePool pool)
	{
		_linkedList.Remove(pool);
		_pools.Remove(pool.prefabPath);
	}

	public void Clear()
	{
		_linkedList.TraverseBackward(ForceClear);
		_linkedList = null;
		_pools.Clear();
		_pools = null;
	}

	private bool ForceClear(AutoDisposePool node)
	{
		if (!node.Clear())
		{
			Log.Error($"[WorldDynamicObjPool] clean 失败 pool/obj {node.GetPoolCount()}/{node.GetObjCount()}  {node.prefabPath}");
		}
		return true;
	}

	private bool TickUnusePoolNode(AutoDisposePool node)
	{
		if (_lastTickTime - node.lastActiveTime > 10f)
		{
			node.GetObjCount();
			_ = node.prefabPath;
			if (node.Clear())
			{
				RemovePool(node);
			}
			int objCount = node.GetObjCount();
			return true;
		}
		return false;
	}

	public int GetPoolCount()
	{
		return _pools.Count;
	}

	public int GetObjectsInPoolCount()
	{
		int num = 0;
		AutoDisposePool autoDisposePool = _linkedList.Head;
		while (autoDisposePool != null)
		{
			AutoDisposePool next = autoDisposePool.next;
			num += autoDisposePool.GetPoolCount();
			autoDisposePool = next;
		}
		return num;
	}
}
