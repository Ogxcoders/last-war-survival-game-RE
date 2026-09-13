using System.Collections;

namespace System.Xml;

public class XmlNamedNodeMap : IEnumerable
{
	internal struct SmallXmlNodeList
	{
		private class SingleObjectEnumerator : IEnumerator
		{
			private object loneValue;

			private int position = -1;

			public object Current
			{
				get
				{
					if (position != 0)
					{
						throw new InvalidOperationException();
					}
					return loneValue;
				}
			}

			public SingleObjectEnumerator(object value)
			{
				loneValue = value;
			}

			public bool MoveNext()
			{
				if (position < 0)
				{
					position = 0;
					return true;
				}
				position = 1;
				return false;
			}

			public void Reset()
			{
				position = -1;
			}
		}

		private object field;

		public int Count
		{
			get
			{
				if (field == null)
				{
					return 0;
				}
				if (field is ArrayList arrayList)
				{
					return arrayList.Count;
				}
				return 1;
			}
		}

		public object this[int index]
		{
			get
			{
				if (field == null)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (field is ArrayList arrayList)
				{
					return arrayList[index];
				}
				if (index != 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return field;
			}
		}

		public void Add(object value)
		{
			if (field == null)
			{
				if (value == null)
				{
					ArrayList arrayList = new ArrayList();
					arrayList.Add(null);
					field = arrayList;
				}
				else
				{
					field = value;
				}
			}
			else if (field is ArrayList arrayList2)
			{
				arrayList2.Add(value);
			}
			else
			{
				ArrayList arrayList3 = new ArrayList();
				arrayList3.Add(field);
				arrayList3.Add(value);
				field = arrayList3;
			}
		}

		public void RemoveAt(int index)
		{
			if (field == null)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (field is ArrayList arrayList)
			{
				arrayList.RemoveAt(index);
				return;
			}
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			field = null;
		}

		public void Insert(int index, object value)
		{
			if (field == null)
			{
				if (index != 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				Add(value);
				return;
			}
			if (field is ArrayList arrayList)
			{
				arrayList.Insert(index, value);
				return;
			}
			switch (index)
			{
			case 0:
			{
				ArrayList arrayList2 = new ArrayList();
				arrayList2.Add(value);
				arrayList2.Add(field);
				field = arrayList2;
				break;
			}
			case 1:
			{
				ArrayList arrayList2 = new ArrayList();
				arrayList2.Add(field);
				arrayList2.Add(value);
				field = arrayList2;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public IEnumerator GetEnumerator()
		{
			if (field == null)
			{
				return XmlDocument.EmptyEnumerator;
			}
			if (field is ArrayList arrayList)
			{
				return arrayList.GetEnumerator();
			}
			return new SingleObjectEnumerator(field);
		}
	}

	internal XmlNode parent;

	internal SmallXmlNodeList nodes;

	public virtual int Count => nodes.Count;

	internal XmlNamedNodeMap(XmlNode parent)
	{
		this.parent = parent;
	}

	public virtual XmlNode GetNamedItem(string name)
	{
		int num = FindNodeOffset(name);
		if (num >= 0)
		{
			return (XmlNode)nodes[num];
		}
		return null;
	}

	public virtual XmlNode SetNamedItem(XmlNode node)
	{
		if (node == null)
		{
			return null;
		}
		int num = FindNodeOffset(node.LocalName, node.NamespaceURI);
		if (num == -1)
		{
			AddNode(node);
			return null;
		}
		return ReplaceNodeAt(num, node);
	}

	public virtual XmlNode RemoveNamedItem(string name)
	{
		int num = FindNodeOffset(name);
		if (num >= 0)
		{
			return RemoveNodeAt(num);
		}
		return null;
	}

	public virtual XmlNode Item(int index)
	{
		if (index < 0 || index >= nodes.Count)
		{
			return null;
		}
		try
		{
			return (XmlNode)nodes[index];
		}
		catch (ArgumentOutOfRangeException)
		{
			throw new IndexOutOfRangeException(Res.GetString("The index being passed in is out of range."));
		}
	}

	public virtual XmlNode GetNamedItem(string localName, string namespaceURI)
	{
		int num = FindNodeOffset(localName, namespaceURI);
		if (num >= 0)
		{
			return (XmlNode)nodes[num];
		}
		return null;
	}

	public virtual XmlNode RemoveNamedItem(string localName, string namespaceURI)
	{
		int num = FindNodeOffset(localName, namespaceURI);
		if (num >= 0)
		{
			return RemoveNodeAt(num);
		}
		return null;
	}

	public virtual IEnumerator GetEnumerator()
	{
		return nodes.GetEnumerator();
	}

	internal int FindNodeOffset(string name)
	{
		int count = Count;
		for (int i = 0; i < count; i++)
		{
			XmlNode xmlNode = (XmlNode)nodes[i];
			if (name == xmlNode.Name)
			{
				return i;
			}
		}
		return -1;
	}

	internal int FindNodeOffset(string localName, string namespaceURI)
	{
		int count = Count;
		for (int i = 0; i < count; i++)
		{
			XmlNode xmlNode = (XmlNode)nodes[i];
			if (xmlNode.LocalName == localName && xmlNode.NamespaceURI == namespaceURI)
			{
				return i;
			}
		}
		return -1;
	}

	internal virtual XmlNode AddNode(XmlNode node)
	{
		XmlNode oldParent = ((node.NodeType != XmlNodeType.Attribute) ? node.ParentNode : ((XmlAttribute)node).OwnerElement);
		string value = node.Value;
		XmlNodeChangedEventArgs eventArgs = parent.GetEventArgs(node, oldParent, parent, value, value, XmlNodeChangedAction.Insert);
		if (eventArgs != null)
		{
			parent.BeforeEvent(eventArgs);
		}
		nodes.Add(node);
		node.SetParent(parent);
		if (eventArgs != null)
		{
			parent.AfterEvent(eventArgs);
		}
		return node;
	}

	internal virtual XmlNode AddNodeForLoad(XmlNode node, XmlDocument doc)
	{
		XmlNodeChangedEventArgs insertEventArgsForLoad = doc.GetInsertEventArgsForLoad(node, parent);
		if (insertEventArgsForLoad != null)
		{
			doc.BeforeEvent(insertEventArgsForLoad);
		}
		nodes.Add(node);
		node.SetParent(parent);
		if (insertEventArgsForLoad != null)
		{
			doc.AfterEvent(insertEventArgsForLoad);
		}
		return node;
	}

	internal virtual XmlNode RemoveNodeAt(int i)
	{
		XmlNode xmlNode = (XmlNode)nodes[i];
		string value = xmlNode.Value;
		XmlNodeChangedEventArgs eventArgs = parent.GetEventArgs(xmlNode, parent, null, value, value, XmlNodeChangedAction.Remove);
		if (eventArgs != null)
		{
			parent.BeforeEvent(eventArgs);
		}
		nodes.RemoveAt(i);
		xmlNode.SetParent(null);
		if (eventArgs != null)
		{
			parent.AfterEvent(eventArgs);
		}
		return xmlNode;
	}

	internal XmlNode ReplaceNodeAt(int i, XmlNode node)
	{
		XmlNode result = RemoveNodeAt(i);
		InsertNodeAt(i, node);
		return result;
	}

	internal virtual XmlNode InsertNodeAt(int i, XmlNode node)
	{
		XmlNode oldParent = ((node.NodeType != XmlNodeType.Attribute) ? node.ParentNode : ((XmlAttribute)node).OwnerElement);
		string value = node.Value;
		XmlNodeChangedEventArgs eventArgs = parent.GetEventArgs(node, oldParent, parent, value, value, XmlNodeChangedAction.Insert);
		if (eventArgs != null)
		{
			parent.BeforeEvent(eventArgs);
		}
		nodes.Insert(i, node);
		node.SetParent(parent);
		if (eventArgs != null)
		{
			parent.AfterEvent(eventArgs);
		}
		return node;
	}
}
