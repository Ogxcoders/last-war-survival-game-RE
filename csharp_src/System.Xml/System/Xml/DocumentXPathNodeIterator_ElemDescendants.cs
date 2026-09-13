using System.Xml.XPath;

namespace System.Xml;

internal abstract class DocumentXPathNodeIterator_ElemDescendants : XPathNodeIterator
{
	private DocumentXPathNavigator nav;

	private int level;

	private int position;

	public override XPathNavigator Current => nav;

	public override int CurrentPosition => position;

	internal DocumentXPathNodeIterator_ElemDescendants(DocumentXPathNavigator nav)
	{
		this.nav = (DocumentXPathNavigator)nav.Clone();
		level = 0;
		position = 0;
	}

	internal DocumentXPathNodeIterator_ElemDescendants(DocumentXPathNodeIterator_ElemDescendants other)
	{
		nav = (DocumentXPathNavigator)other.nav.Clone();
		level = other.level;
		position = other.position;
	}

	protected abstract bool Match(XmlNode node);

	protected void SetPosition(int pos)
	{
		position = pos;
	}

	public override bool MoveNext()
	{
		XmlNode xmlNode;
		do
		{
			if (nav.MoveToFirstChild())
			{
				level++;
			}
			else
			{
				if (level == 0)
				{
					return false;
				}
				while (!nav.MoveToNext())
				{
					level--;
					if (level == 0)
					{
						return false;
					}
					if (!nav.MoveToParent())
					{
						return false;
					}
				}
			}
			xmlNode = (XmlNode)nav.UnderlyingObject;
		}
		while (xmlNode.NodeType != XmlNodeType.Element || !Match(xmlNode));
		position++;
		return true;
	}
}
