using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime;

internal sealed class RtfTreeNavigator : RtfNavigator
{
	private XmlEventCache events;

	private NavigatorConstructor constr;

	private XmlNameTable nameTable;

	public override string Value => events.EventsToString();

	public override string BaseURI => events.BaseUri;

	public RtfTreeNavigator(XmlEventCache events, XmlNameTable nameTable)
	{
		this.events = events;
		constr = new NavigatorConstructor();
		this.nameTable = nameTable;
	}

	public RtfTreeNavigator(RtfTreeNavigator that)
	{
		events = that.events;
		constr = that.constr;
		nameTable = that.nameTable;
	}

	public override void CopyToWriter(XmlWriter writer)
	{
		events.EventsToWriter(writer);
	}

	public override XPathNavigator ToNavigator()
	{
		return constr.GetNavigator(events, nameTable);
	}

	public override XPathNavigator Clone()
	{
		return new RtfTreeNavigator(this);
	}

	public override bool MoveTo(XPathNavigator other)
	{
		if (other is RtfTreeNavigator rtfTreeNavigator)
		{
			events = rtfTreeNavigator.events;
			constr = rtfTreeNavigator.constr;
			nameTable = rtfTreeNavigator.nameTable;
			return true;
		}
		return false;
	}
}
