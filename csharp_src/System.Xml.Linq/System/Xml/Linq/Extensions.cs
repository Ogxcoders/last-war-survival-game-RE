using System.Collections.Generic;

namespace System.Xml.Linq;

public static class Extensions
{
	public static IEnumerable<XAttribute> Attributes(this IEnumerable<XElement> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return GetAttributes(source, null);
	}

	public static IEnumerable<XAttribute> Attributes(this IEnumerable<XElement> source, XName name)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (!(name != null))
		{
			return XAttribute.EmptySequence;
		}
		return GetAttributes(source, name);
	}

	public static IEnumerable<XElement> Ancestors<T>(this IEnumerable<T> source) where T : XNode
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return GetAncestors(source, null, self: false);
	}

	public static IEnumerable<XElement> Ancestors<T>(this IEnumerable<T> source, XName name) where T : XNode
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (!(name != null))
		{
			return XElement.EmptySequence;
		}
		return GetAncestors(source, name, self: false);
	}

	public static IEnumerable<XElement> AncestorsAndSelf(this IEnumerable<XElement> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return GetAncestors(source, null, self: true);
	}

	public static IEnumerable<XElement> AncestorsAndSelf(this IEnumerable<XElement> source, XName name)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (!(name != null))
		{
			return XElement.EmptySequence;
		}
		return GetAncestors(source, name, self: true);
	}

	public static IEnumerable<XNode> Nodes<T>(this IEnumerable<T> source) where T : XContainer
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return NodesIterator(source);
	}

	private static IEnumerable<XNode> NodesIterator<T>(IEnumerable<T> source) where T : XContainer
	{
		foreach (T root in source)
		{
			if (root == null)
			{
				continue;
			}
			XNode n = root.LastNode;
			if (n != null)
			{
				do
				{
					n = n.next;
					yield return n;
				}
				while (n.parent == root && n != root.content);
			}
		}
	}

	public static IEnumerable<XNode> DescendantNodes<T>(this IEnumerable<T> source) where T : XContainer
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return GetDescendantNodes(source, self: false);
	}

	public static IEnumerable<XElement> Descendants<T>(this IEnumerable<T> source) where T : XContainer
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return GetDescendants(source, null, self: false);
	}

	public static IEnumerable<XElement> Descendants<T>(this IEnumerable<T> source, XName name) where T : XContainer
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (!(name != null))
		{
			return XElement.EmptySequence;
		}
		return GetDescendants(source, name, self: false);
	}

	public static IEnumerable<XNode> DescendantNodesAndSelf(this IEnumerable<XElement> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return GetDescendantNodes(source, self: true);
	}

	public static IEnumerable<XElement> DescendantsAndSelf(this IEnumerable<XElement> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return GetDescendants(source, null, self: true);
	}

	public static IEnumerable<XElement> DescendantsAndSelf(this IEnumerable<XElement> source, XName name)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (!(name != null))
		{
			return XElement.EmptySequence;
		}
		return GetDescendants(source, name, self: true);
	}

	public static IEnumerable<XElement> Elements<T>(this IEnumerable<T> source) where T : XContainer
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return GetElements(source, null);
	}

	public static IEnumerable<XElement> Elements<T>(this IEnumerable<T> source, XName name) where T : XContainer
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (!(name != null))
		{
			return XElement.EmptySequence;
		}
		return GetElements(source, name);
	}

	public static IEnumerable<T> InDocumentOrder<T>(this IEnumerable<T> source) where T : XNode
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return DocumentOrderIterator(source);
	}

	private static IEnumerable<T> DocumentOrderIterator<T>(IEnumerable<T> source) where T : XNode
	{
		int count;
		T[] items = System.Collections.Generic.EnumerableHelpers.ToArray(source, out count);
		if (count > 0)
		{
			Array.Sort(items, 0, count, XNode.DocumentOrderComparer);
			int i = 0;
			while (i != count)
			{
				yield return items[i];
				int num = i + 1;
				i = num;
			}
		}
	}

	public static void Remove(this IEnumerable<XAttribute> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		int length;
		XAttribute[] array = System.Collections.Generic.EnumerableHelpers.ToArray(source, out length);
		for (int i = 0; i < length; i++)
		{
			array[i]?.Remove();
		}
	}

	public static void Remove<T>(this IEnumerable<T> source) where T : XNode
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		int length;
		T[] array = System.Collections.Generic.EnumerableHelpers.ToArray(source, out length);
		for (int i = 0; i < length; i++)
		{
			array[i]?.Remove();
		}
	}

	private static IEnumerable<XAttribute> GetAttributes(IEnumerable<XElement> source, XName name)
	{
		foreach (XElement e in source)
		{
			if (e == null)
			{
				continue;
			}
			XAttribute a = e.lastAttr;
			if (a == null)
			{
				continue;
			}
			do
			{
				a = a.next;
				if (name == null || a.name == name)
				{
					yield return a;
				}
			}
			while (a.parent == e && a != e.lastAttr);
		}
	}

	private static IEnumerable<XElement> GetAncestors<T>(IEnumerable<T> source, XName name, bool self) where T : XNode
	{
		foreach (T item in source)
		{
			if (item == null)
			{
				continue;
			}
			for (XElement e = (self ? ((XNode)item) : ((XNode)item.parent)) as XElement; e != null; e = e.parent as XElement)
			{
				if (name == null || e.name == name)
				{
					yield return e;
				}
			}
		}
	}

	private static IEnumerable<XNode> GetDescendantNodes<T>(IEnumerable<T> source, bool self) where T : XContainer
	{
		foreach (T root in source)
		{
			if (root == null)
			{
				continue;
			}
			if (self)
			{
				yield return root;
			}
			XNode n = root;
			while (true)
			{
				XNode firstNode;
				if (n is XContainer xContainer && (firstNode = xContainer.FirstNode) != null)
				{
					n = firstNode;
				}
				else
				{
					while (n != null && n != root && n == n.parent.content)
					{
						n = n.parent;
					}
					if (n == null || n == root)
					{
						break;
					}
					n = n.next;
				}
				yield return n;
			}
		}
	}

	private static IEnumerable<XElement> GetDescendants<T>(IEnumerable<T> source, XName name, bool self) where T : XContainer
	{
		foreach (T root in source)
		{
			if (root == null)
			{
				continue;
			}
			if (self)
			{
				XElement xElement = (XElement)(object)root;
				if (name == null || xElement.name == name)
				{
					yield return xElement;
				}
			}
			XNode n = root;
			XContainer xContainer = root;
			while (true)
			{
				if (xContainer != null && xContainer.content is XNode)
				{
					n = ((XNode)xContainer.content).next;
				}
				else
				{
					while (n != null && n != root && n == n.parent.content)
					{
						n = n.parent;
					}
					if (n == null || n == root)
					{
						break;
					}
					n = n.next;
				}
				XElement e = n as XElement;
				if (e != null && (name == null || e.name == name))
				{
					yield return e;
				}
				xContainer = e;
			}
		}
	}

	private static IEnumerable<XElement> GetElements<T>(IEnumerable<T> source, XName name) where T : XContainer
	{
		foreach (T root in source)
		{
			if (root == null)
			{
				continue;
			}
			XNode n = root.content as XNode;
			if (n == null)
			{
				continue;
			}
			do
			{
				n = n.next;
				if (n is XElement xElement && (name == null || xElement.name == name))
				{
					yield return xElement;
				}
			}
			while (n.parent == root && n != root.content);
		}
	}
}
