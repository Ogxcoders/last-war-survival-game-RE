using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime;

internal struct XmlNavigatorStack
{
	private XPathNavigator[] stkNav;

	private int sp;

	private const int InitialStackSize = 8;

	public bool IsEmpty => sp == 0;

	public void Push(XPathNavigator nav)
	{
		if (stkNav == null)
		{
			stkNav = new XPathNavigator[8];
		}
		else if (sp >= stkNav.Length)
		{
			XPathNavigator[] sourceArray = stkNav;
			stkNav = new XPathNavigator[2 * sp];
			Array.Copy(sourceArray, stkNav, sp);
		}
		stkNav[sp++] = nav;
	}

	public XPathNavigator Pop()
	{
		return stkNav[--sp];
	}

	public XPathNavigator Peek()
	{
		return stkNav[sp - 1];
	}

	public void Reset()
	{
		sp = 0;
	}
}
