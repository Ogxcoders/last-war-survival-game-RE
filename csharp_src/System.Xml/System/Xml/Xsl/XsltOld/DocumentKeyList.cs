using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal struct DocumentKeyList
{
	private XPathNavigator rootNav;

	private Hashtable keyTable;

	public XPathNavigator RootNav => rootNav;

	public Hashtable KeyTable => keyTable;

	public DocumentKeyList(XPathNavigator rootNav, Hashtable keyTable)
	{
		this.rootNav = rootNav;
		this.keyTable = keyTable;
	}
}
