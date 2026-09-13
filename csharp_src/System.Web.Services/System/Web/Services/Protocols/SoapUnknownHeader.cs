using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

public sealed class SoapUnknownHeader : SoapHeader
{
	private XmlElement element;

	[XmlIgnore]
	public XmlElement Element
	{
		get
		{
			return element;
		}
		set
		{
			element = value;
		}
	}

	public SoapUnknownHeader()
	{
		element = null;
	}

	internal SoapUnknownHeader(XmlElement elem)
		: base(elem)
	{
		element = elem;
	}
}
