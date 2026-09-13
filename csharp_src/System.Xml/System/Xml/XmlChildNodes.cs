using System.Collections;

namespace System.Xml;

internal class XmlChildNodes : XmlNodeList
{
	private XmlNode container;

	public override int Count
	{
		get
		{
			int num = 0;
			for (XmlNode xmlNode = container.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				num++;
			}
			return num;
		}
	}

	public XmlChildNodes(XmlNode container)
	{
		this.container = container;
	}

	public override XmlNode Item(int i)
	{
		if (i < 0)
		{
			return null;
		}
		XmlNode xmlNode = container.FirstChild;
		while (xmlNode != null)
		{
			if (i == 0)
			{
				return xmlNode;
			}
			xmlNode = xmlNode.NextSibling;
			i--;
		}
		return null;
	}

	public override IEnumerator GetEnumerator()
	{
		if (container.FirstChild == null)
		{
			return XmlDocument.EmptyEnumerator;
		}
		return new XmlChildEnumerator(container);
	}
}
