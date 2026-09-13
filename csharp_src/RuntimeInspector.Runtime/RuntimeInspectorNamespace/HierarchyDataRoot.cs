using UnityEngine;

namespace RuntimeInspectorNamespace;

public abstract class HierarchyDataRoot : HierarchyData
{
	public override Transform BoundTransform => null;

	public override bool IsActive => true;

	public RuntimeHierarchy Hierarchy { get; private set; }

	protected HierarchyDataRoot(RuntimeHierarchy hierarchy)
	{
		Hierarchy = hierarchy;
		PopChildrenList();
	}

	public abstract Transform GetNearestRootOf(Transform target);

	public abstract void RefreshContent();

	public override bool Refresh()
	{
		RefreshContent();
		return base.Refresh();
	}

	public override HierarchyDataTransform FindTransformInVisibleChildren(Transform target, int targetDepth = -1)
	{
		if (m_depth < 0 || !base.IsExpanded)
		{
			return null;
		}
		return base.FindTransformInVisibleChildren(target, targetDepth);
	}

	public void ResetCachedNames()
	{
		if (children != null)
		{
			for (int num = children.Count - 1; num >= 0; num--)
			{
				children[num].ResetCachedName();
			}
		}
	}

	public void RefreshNameOf(Transform target)
	{
		if (children != null)
		{
			for (int num = children.Count - 1; num >= 0; num--)
			{
				children[num].RefreshNameOf(target);
			}
		}
	}
}
