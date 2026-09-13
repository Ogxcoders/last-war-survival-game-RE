using System.Collections.Generic;
using UnityEngine;

namespace PVEBattleLogic.Unit;

public class UnitGameObjectUpdater : MonoBehaviour
{
	private Dictionary<int, (Transform, float)> _updateDic = new Dictionary<int, (Transform, float)>();

	public void Register(int handle, Transform trans, float updateZ)
	{
		if (_updateDic.ContainsKey(handle))
		{
			_updateDic[handle] = (trans, updateZ);
		}
		else
		{
			_updateDic.Add(handle, (trans, updateZ));
		}
	}

	public void Unregister(int handle)
	{
		if (_updateDic.ContainsKey(handle))
		{
			_updateDic.Remove(handle);
		}
	}

	public void Clear()
	{
		_updateDic.Clear();
	}

	private void Update()
	{
		foreach (KeyValuePair<int, (Transform, float)> item3 in _updateDic)
		{
			Transform item = item3.Value.Item1;
			float item2 = item3.Value.Item2;
			Vector3 position = item.position;
			position.z += item2 * Time.deltaTime;
			item.position = position;
		}
	}
}
