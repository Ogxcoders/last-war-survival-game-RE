using System.Xml.XPath;

namespace System.Xml;

internal sealed class DocumentXPathNodeIterator_Empty : XPathNodeIterator
{
	private XPathNavigator nav;

	public override XPathNavigator Current => nav;

	public override int CurrentPosition => 0;

	public override int Count => 0;

	internal DocumentXPathNodeIterator_Empty(DocumentXPathNavigator nav)
	{
		this.nav = nav.Clone();
	}

	internal DocumentXPathNodeIterator_Empty(DocumentXPathNodeIterator_Empty other)
	{
		nav = other.nav.Clone();
	}

	public override XPathNodeIterator Clone()
	{
		return new DocumentXPathNodeIterator_Empty(this);
	}

	public override bool MoveNext()
	{
		return false;
	}
}
