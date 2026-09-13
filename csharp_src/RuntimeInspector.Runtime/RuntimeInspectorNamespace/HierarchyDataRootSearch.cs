using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeInspectorNamespace;

public class HierarchyDataRootSearch : HierarchyDataRoot
{
	private readonly List<Transform> searchResult = new List<Transform>();

	private readonly HierarchyDataRoot reference;

	private string searchTerm;

	public override string Name => reference.Name;

	public override int ChildCount => searchResult.Count;

	public HierarchyDataRootSearch(RuntimeHierarchy hierarchy, HierarchyDataRoot reference)
		: base(hierarchy)
	{
		this.reference = reference;
	}

	public override void RefreshContent()
	{
		if (!base.Hierarchy.IsInSearchMode)
		{
			return;
		}
		searchResult.Clear();
		searchTerm = base.Hierarchy.SearchTerm;
		int childCount = reference.ChildCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = reference.GetChild(i);
			if ((bool)child && !RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Contains(child.transform))
			{
				if (child.name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					searchResult.Add(child);
				}
				SearchTransformRecursively(child.transform);
			}
		}
	}

	public override bool Refresh()
	{
		m_depth = 0;
		bool result = base.Refresh();
		if (searchResult.Count == 0)
		{
			m_height = 0;
			m_depth = -1;
		}
		return result;
	}

	public override HierarchyDataTransform FindTransformInVisibleChildren(Transform target, int targetDepth = -1)
	{
		if (m_depth < 0 || targetDepth > 1 || !base.IsExpanded)
		{
			return null;
		}
		for (int num = children.Count - 1; num >= 0; num--)
		{
			if ((object)children[num].BoundTransform == target)
			{
				return children[num];
			}
		}
		return null;
	}

	private void SearchTransformRecursively(Transform obj)
	{
		for (int i = 0; i < obj.childCount; i++)
		{
			Transform child = obj.GetChild(i);
			if (!RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Contains(child))
			{
				if (child.name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					searchResult.Add(child);
				}
				SearchTransformRecursively(child);
			}
		}
	}

	public override Transform GetChild(int index)
	{
		return searchResult[index];
	}

	public override Transform GetNearestRootOf(Transform target)
	{
		if (!searchResult.Contains(target))
		{
			return null;
		}
		return target;
	}
}
