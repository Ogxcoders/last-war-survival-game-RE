using System.Collections.Generic;
using UnityEngine;

namespace RuntimeInspectorNamespace;

public class HierarchyDataRootPseudoScene : HierarchyDataRoot
{
	private readonly string name;

	private readonly List<Transform> rootObjects = new List<Transform>();

	public override string Name => name;

	public override int ChildCount => rootObjects.Count;

	public HierarchyDataRootPseudoScene(RuntimeHierarchy hierarchy, string name)
		: base(hierarchy)
	{
		this.name = name;
	}

	public void AddChild(Transform child)
	{
		if (!rootObjects.Contains(child))
		{
			rootObjects.Add(child);
		}
	}

	public void InsertChild(int index, Transform child)
	{
		index = Mathf.Clamp(index, 0, rootObjects.Count);
		rootObjects.Insert(index, child);
		for (int num = rootObjects.Count - 1; num >= 0; num--)
		{
			if (num != index && rootObjects[num] == child)
			{
				rootObjects.RemoveAt(num);
				break;
			}
		}
	}

	public void RemoveChild(Transform child)
	{
		rootObjects.Remove(child);
	}

	public override void RefreshContent()
	{
		for (int num = rootObjects.Count - 1; num >= 0; num--)
		{
			if (!rootObjects[num])
			{
				rootObjects.RemoveAt(num);
			}
		}
	}

	public override Transform GetChild(int index)
	{
		return rootObjects[index];
	}

	public override Transform GetNearestRootOf(Transform target)
	{
		Transform transform = null;
		for (int num = rootObjects.Count - 1; num >= 0; num--)
		{
			Transform transform2 = rootObjects[num];
			if ((bool)transform2 && target.IsChildOf(transform2) && (!transform || transform2.IsChildOf(transform)))
			{
				transform = transform2;
			}
		}
		return transform;
	}
}
