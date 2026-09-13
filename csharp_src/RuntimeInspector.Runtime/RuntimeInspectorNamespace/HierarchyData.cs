using System.Collections.Generic;
using UnityEngine;

namespace RuntimeInspectorNamespace;

public abstract class HierarchyData
{
	private static readonly List<HierarchyDataTransform> transformDataPool = new List<HierarchyDataTransform>(32);

	private static readonly List<List<HierarchyDataTransform>> childrenListPool = new List<List<HierarchyDataTransform>>(32);

	protected List<HierarchyDataTransform> children;

	protected HierarchyData parent;

	protected int m_index;

	protected int m_height = 1;

	protected int m_depth;

	public abstract string Name { get; }

	public abstract bool IsActive { get; }

	public abstract int ChildCount { get; }

	public abstract Transform BoundTransform { get; }

	public HierarchyDataRoot Root
	{
		get
		{
			HierarchyData hierarchyData = this;
			while (hierarchyData.parent != null)
			{
				hierarchyData = hierarchyData.parent;
			}
			return (HierarchyDataRoot)hierarchyData;
		}
	}

	public int Index => m_index;

	public int AbsoluteIndex
	{
		get
		{
			int num = m_index;
			for (HierarchyData hierarchyData = parent; hierarchyData != null; hierarchyData = hierarchyData.parent)
			{
				num += hierarchyData.m_index + 1;
			}
			return num;
		}
	}

	public int Height => m_height;

	public int Depth => m_depth;

	public bool CanExpand => ChildCount > 0;

	public bool IsExpanded
	{
		get
		{
			return children != null;
		}
		set
		{
			if (IsExpanded == value)
			{
				return;
			}
			if (value)
			{
				if (ChildCount == 0)
				{
					return;
				}
				PopChildrenList();
			}
			else
			{
				PoolChildrenList();
			}
			int height = m_height;
			Refresh();
			int num = m_height - height;
			if (num == 0)
			{
				return;
			}
			if (parent != null)
			{
				HierarchyData hierarchyData = this;
				for (HierarchyData hierarchyData2 = parent; hierarchyData2 != null; hierarchyData2 = hierarchyData2.parent)
				{
					List<HierarchyDataTransform> list = hierarchyData2.children;
					int i = list.IndexOf((HierarchyDataTransform)hierarchyData) + 1;
					for (int count = list.Count; i < count; i++)
					{
						list[i].m_index += num;
					}
					hierarchyData2.m_height += num;
					hierarchyData = hierarchyData2;
				}
			}
			Root?.Hierarchy.SetListViewDirty();
		}
	}

	public virtual bool Refresh()
	{
		if (m_depth < 0)
		{
			return false;
		}
		m_height = 1;
		bool flag = false;
		int childCount = ChildCount;
		if (IsExpanded)
		{
			if (childCount != children.Count)
			{
				flag = true;
			}
			RuntimeHierarchy runtimeHierarchy = null;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = GetChild(i);
				if (children.Count <= i)
				{
					if (runtimeHierarchy == null)
					{
						runtimeHierarchy = Root.Hierarchy;
					}
					GenerateChildItem(child, i, runtimeHierarchy);
				}
				else if (children[i].BoundTransform != child)
				{
					int j;
					for (j = 0; j < children.Count && !(children[j].BoundTransform == child); j++)
					{
					}
					if (j == children.Count)
					{
						if (runtimeHierarchy == null)
						{
							runtimeHierarchy = Root.Hierarchy;
						}
						GenerateChildItem(child, i, runtimeHierarchy);
					}
					else
					{
						HierarchyDataTransform item = children[j];
						children.RemoveAt(j);
						children.Insert(i, item);
					}
					flag = true;
				}
				flag |= children[i].Refresh();
				children[i].m_index = m_height - 1;
				m_height += children[i].m_height;
			}
			for (int num = children.Count - 1; num >= childCount; num--)
			{
				RemoveChildItem(num);
			}
		}
		return flag;
	}

	public HierarchyData FindDataAtIndex(int index)
	{
		int num = children.Count - 1;
		if (index <= num && children[index].m_index == index)
		{
			int i;
			for (i = index; i < num && index == children[i + 1].m_index; i++)
			{
			}
			return children[i];
		}
		int num2 = 0;
		int j = num;
		while (num2 <= j)
		{
			int k = (num2 + j) / 2;
			int index2 = children[k].m_index;
			if (index == index2)
			{
				for (; k < num && index == children[k + 1].m_index; k++)
				{
				}
				return children[k];
			}
			if (index < index2)
			{
				j = k - 1;
			}
			else
			{
				num2 = k + 1;
			}
		}
		if (j < 0)
		{
			j = 0;
		}
		for (; j < num && index >= children[j + 1].m_index; j++)
		{
		}
		return children[j].FindDataAtIndex(index - 1 - children[j].m_index);
	}

	public HierarchyDataTransform FindTransform(Transform target, Transform nextInPath = null)
	{
		if (m_depth < 0)
		{
			return null;
		}
		bool flag = nextInPath == null;
		if (flag)
		{
			nextInPath = ((this is HierarchyDataRootSearch) ? target : target.root);
		}
		int num = IndexOf(nextInPath);
		if (num < 0)
		{
			if (!flag || !(this is HierarchyDataRootPseudoScene))
			{
				return null;
			}
			nextInPath = target;
			num = IndexOf(nextInPath);
			while (num < 0 && nextInPath != null)
			{
				nextInPath = nextInPath.parent;
				num = IndexOf(nextInPath);
			}
			if (num < 0)
			{
				return null;
			}
		}
		if (!CanExpand)
		{
			return null;
		}
		bool isExpanded = IsExpanded;
		if (!isExpanded)
		{
			IsExpanded = true;
		}
		HierarchyDataTransform hierarchyDataTransform = children[num];
		if (hierarchyDataTransform.BoundTransform == target)
		{
			return hierarchyDataTransform;
		}
		HierarchyDataTransform hierarchyDataTransform2 = null;
		if (hierarchyDataTransform.BoundTransform == nextInPath)
		{
			Transform transform = target;
			Transform transform2 = transform.parent;
			while (transform2 != null && transform2 != nextInPath)
			{
				transform = transform2;
				transform2 = transform.parent;
			}
			if (transform2 != null)
			{
				hierarchyDataTransform2 = hierarchyDataTransform.FindTransform(target, transform);
			}
		}
		if (hierarchyDataTransform2 != null && hierarchyDataTransform2.m_depth < 0)
		{
			hierarchyDataTransform2 = null;
		}
		if (hierarchyDataTransform2 == null && !isExpanded)
		{
			IsExpanded = false;
		}
		return hierarchyDataTransform2;
	}

	public virtual HierarchyDataTransform FindTransformInVisibleChildren(Transform target, int targetDepth = -1)
	{
		for (int i = 0; i < children.Count; i++)
		{
			HierarchyDataTransform hierarchyDataTransform = children[i];
			if (hierarchyDataTransform.m_depth < 0)
			{
				continue;
			}
			if ((object)hierarchyDataTransform.BoundTransform == target)
			{
				if (targetDepth <= 0 || hierarchyDataTransform.m_depth == targetDepth)
				{
					return hierarchyDataTransform;
				}
			}
			else if ((targetDepth <= 0 || hierarchyDataTransform.m_depth < targetDepth) && hierarchyDataTransform.IsExpanded && (bool)hierarchyDataTransform.BoundTransform && target.IsChildOf(hierarchyDataTransform.BoundTransform))
			{
				hierarchyDataTransform = hierarchyDataTransform.FindTransformInVisibleChildren(target, targetDepth);
				if (hierarchyDataTransform != null)
				{
					return hierarchyDataTransform;
				}
			}
		}
		return null;
	}

	public abstract Transform GetChild(int index);

	public int IndexOf(Transform transform)
	{
		for (int num = ChildCount - 1; num >= 0; num--)
		{
			if ((object)GetChild(num) == transform)
			{
				return num;
			}
		}
		return -1;
	}

	public void GetSiblingIndexTraversalList(List<int> traversalList)
	{
		traversalList.Clear();
		HierarchyData hierarchyData = this;
		while (hierarchyData.parent != null)
		{
			traversalList.Add(hierarchyData.parent.children.IndexOf((HierarchyDataTransform)hierarchyData));
			hierarchyData = hierarchyData.parent;
		}
	}

	public HierarchyData TraverseSiblingIndexList(List<int> traversalList)
	{
		HierarchyData hierarchyData = this;
		for (int num = traversalList.Count - 1; num >= 0; num--)
		{
			int num2 = traversalList[num];
			if (hierarchyData.children == null || num2 >= hierarchyData.children.Count)
			{
				return null;
			}
			hierarchyData = hierarchyData.children[num2];
		}
		return hierarchyData;
	}

	private void GenerateChildItem(Transform child, int index, RuntimeHierarchy hierarchy)
	{
		bool flag = !RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Contains(child);
		if (flag && hierarchy.GameObjectFilter != null)
		{
			flag = hierarchy.GameObjectFilter(child);
		}
		int num = transformDataPool.Count - 1;
		HierarchyDataTransform hierarchyDataTransform;
		if (num >= 0)
		{
			hierarchyDataTransform = transformDataPool[num];
			transformDataPool.RemoveAt(num);
		}
		else
		{
			hierarchyDataTransform = new HierarchyDataTransform();
		}
		hierarchyDataTransform.Initialize(child, this is HierarchyDataRootSearch);
		hierarchyDataTransform.parent = this;
		if (flag)
		{
			hierarchyDataTransform.m_depth = m_depth + 1;
			hierarchyDataTransform.m_height = 1;
		}
		else
		{
			hierarchyDataTransform.m_depth = -1;
			hierarchyDataTransform.m_height = 0;
		}
		children.Insert(index, hierarchyDataTransform);
	}

	private void RemoveChildItem(int index)
	{
		children[index].PoolData();
		transformDataPool.Add(children[index]);
		children.RemoveAt(index);
	}

	protected void PoolChildrenList()
	{
		if (children != null)
		{
			for (int num = children.Count - 1; num >= 0; num--)
			{
				children[num].PoolData();
				transformDataPool.Add(children[num]);
			}
			children.Clear();
			childrenListPool.Add(children);
			children = null;
		}
	}

	protected void PopChildrenList()
	{
		int childCount = ChildCount;
		int num = -1;
		int num2 = int.MaxValue;
		for (int num3 = childrenListPool.Count - 1; num3 >= 0; num3--)
		{
			int num4 = childrenListPool[num3].Capacity - childCount;
			if (num4 < 0)
			{
				num4 = -num4;
			}
			if (num4 < num2)
			{
				num2 = num4;
				num = num3;
			}
		}
		if (num >= 0)
		{
			children = childrenListPool[num];
			childrenListPool.RemoveAt(num);
		}
		else
		{
			children = new List<HierarchyDataTransform>(ChildCount);
		}
	}

	public static void ClearPool()
	{
		childrenListPool.Clear();
		transformDataPool.Clear();
		if (childrenListPool.Capacity > 128)
		{
			childrenListPool.Capacity = 128;
		}
		if (transformDataPool.Capacity > 128)
		{
			transformDataPool.Capacity = 128;
		}
	}
}
