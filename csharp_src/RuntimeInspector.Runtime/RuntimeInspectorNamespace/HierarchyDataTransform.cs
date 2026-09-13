using UnityEngine;

namespace RuntimeInspectorNamespace;

public class HierarchyDataTransform : HierarchyData
{
	private string cachedName;

	private Transform transform;

	private bool isSearchEntry;

	public override string Name
	{
		get
		{
			if (cachedName == null)
			{
				cachedName = (transform ? transform.name : "<destroyed>");
			}
			return cachedName;
		}
	}

	public override int ChildCount
	{
		get
		{
			if (isSearchEntry || !transform)
			{
				return 0;
			}
			return transform.childCount;
		}
	}

	public override Transform BoundTransform => transform;

	public override bool IsActive
	{
		get
		{
			if (!transform)
			{
				return true;
			}
			return transform.gameObject.activeInHierarchy;
		}
	}

	public void Initialize(Transform transform, bool isSearchEntry)
	{
		this.transform = transform;
		this.isSearchEntry = isSearchEntry;
	}

	public override Transform GetChild(int index)
	{
		return transform.GetChild(index);
	}

	public void ResetCachedName()
	{
		cachedName = null;
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
		if ((object)transform == target)
		{
			cachedName = target.name;
		}
		else if (children != null)
		{
			for (int num = children.Count - 1; num >= 0; num--)
			{
				children[num].RefreshNameOf(target);
			}
		}
	}

	public void PoolData()
	{
		parent = null;
		cachedName = null;
		m_depth = 0;
		m_height = 0;
		PoolChildrenList();
	}
}
