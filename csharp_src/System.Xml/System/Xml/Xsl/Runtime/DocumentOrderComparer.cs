using System.Collections.Generic;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime;

internal class DocumentOrderComparer : IComparer<XPathNavigator>
{
	private List<XPathNavigator> roots;

	public int Compare(XPathNavigator navThis, XPathNavigator navThat)
	{
		switch (navThis.ComparePosition(navThat))
		{
		case XmlNodeOrder.Before:
			return -1;
		case XmlNodeOrder.Same:
			return 0;
		case XmlNodeOrder.After:
			return 1;
		default:
			if (roots == null)
			{
				roots = new List<XPathNavigator>();
			}
			if (GetDocumentIndex(navThis) >= GetDocumentIndex(navThat))
			{
				return 1;
			}
			return -1;
		}
	}

	public int GetDocumentIndex(XPathNavigator nav)
	{
		if (roots == null)
		{
			roots = new List<XPathNavigator>();
		}
		XPathNavigator xPathNavigator = nav.Clone();
		xPathNavigator.MoveToRoot();
		for (int i = 0; i < roots.Count; i++)
		{
			if (xPathNavigator.IsSamePosition(roots[i]))
			{
				return i;
			}
		}
		roots.Add(xPathNavigator);
		return roots.Count - 1;
	}
}
