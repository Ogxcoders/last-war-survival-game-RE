using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.Rendering;

public class GameObjectsSetting : QualitySettingGroup
{
	public List<GameObject> objects;

	public override void Switch(EnQualityLevel level)
	{
		if (objects == null)
		{
			return;
		}
		bool flag = base[level];
		for (int i = 0; i < objects.Count; i++)
		{
			if (objects[i] != null && objects[i].activeSelf != flag)
			{
				objects[i].SetActive(flag);
			}
		}
		base.Switch(level);
	}
}
