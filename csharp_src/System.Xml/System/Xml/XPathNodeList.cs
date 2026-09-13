using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;

namespace System.Xml;

internal class XPathNodeList : XmlNodeList
{
	private List<XmlNode> list;

	private XPathNodeIterator nodeIterator;

	private bool done;

	private static readonly object[] nullparams = new object[0];

	public override int Count
	{
		get
		{
			if (!done)
			{
				ReadUntil(int.MaxValue);
			}
			return list.Count;
		}
	}

	public XPathNodeList(XPathNodeIterator nodeIterator)
	{
		this.nodeIterator = nodeIterator;
		list = new List<XmlNode>();
		done = false;
	}

	private XmlNode GetNode(XPathNavigator n)
	{
		return ((IHasXmlNode)n).GetNode();
	}

	internal int ReadUntil(int index)
	{
		int num = list.Count;
		while (!done && num <= index)
		{
			if (nodeIterator.MoveNext())
			{
				XmlNode node = GetNode(nodeIterator.Current);
				if (node != null)
				{
					list.Add(node);
					num++;
				}
				continue;
			}
			done = true;
			break;
		}
		return num;
	}

	public override XmlNode Item(int index)
	{
		if (list.Count <= index)
		{
			ReadUntil(index);
		}
		if (index < 0 || list.Count <= index)
		{
			return null;
		}
		return list[index];
	}

	public override IEnumerator GetEnumerator()
	{
		return new XmlNodeListEnumerator(this);
	}
}
