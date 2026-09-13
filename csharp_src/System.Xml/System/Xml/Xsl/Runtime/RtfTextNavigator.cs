using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime;

internal sealed class RtfTextNavigator : RtfNavigator
{
	private string text;

	private string baseUri;

	private NavigatorConstructor constr;

	public override string Value => text;

	public override string BaseURI => baseUri;

	public RtfTextNavigator(string text, string baseUri)
	{
		this.text = text;
		this.baseUri = baseUri;
		constr = new NavigatorConstructor();
	}

	public RtfTextNavigator(RtfTextNavigator that)
	{
		text = that.text;
		baseUri = that.baseUri;
		constr = that.constr;
	}

	public override void CopyToWriter(XmlWriter writer)
	{
		writer.WriteString(Value);
	}

	public override XPathNavigator ToNavigator()
	{
		return constr.GetNavigator(text, baseUri, new NameTable());
	}

	public override XPathNavigator Clone()
	{
		return new RtfTextNavigator(this);
	}

	public override bool MoveTo(XPathNavigator other)
	{
		if (other is RtfTextNavigator rtfTextNavigator)
		{
			text = rtfTextNavigator.text;
			baseUri = rtfTextNavigator.baseUri;
			constr = rtfTextNavigator.constr;
			return true;
		}
		return false;
	}
}
