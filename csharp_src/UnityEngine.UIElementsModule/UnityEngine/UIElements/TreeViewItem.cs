using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal class TreeViewItem<T> : ITreeViewItem
{
	internal TreeViewItem<T> m_Parent;

	private List<ITreeViewItem> m_Children;

	public int id { get; private set; }

	public ITreeViewItem parent => m_Parent;

	public IEnumerable<ITreeViewItem> children => m_Children;

	public bool hasChildren => m_Children != null && m_Children.Count > 0;

	public T data { get; private set; }

	public TreeViewItem(int id, T data, List<TreeViewItem<T>> children = null)
	{
		this.id = id;
		this.data = data;
		if (children == null)
		{
			return;
		}
		foreach (TreeViewItem<T> child in children)
		{
			AddChild(child);
		}
	}

	public void AddChild(ITreeViewItem child)
	{
		if (child is TreeViewItem<T> treeViewItem)
		{
			if (m_Children == null)
			{
				m_Children = new List<ITreeViewItem>();
			}
			m_Children.Add(treeViewItem);
			treeViewItem.m_Parent = this;
		}
	}

	public void AddChildren(IList<ITreeViewItem> children)
	{
		foreach (ITreeViewItem child in children)
		{
			AddChild(child);
		}
	}

	public void RemoveChild(ITreeViewItem child)
	{
		if (m_Children != null && child is TreeViewItem<T> item)
		{
			m_Children.Remove(item);
		}
	}
}
