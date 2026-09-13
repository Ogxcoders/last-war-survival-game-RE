using System.Xml.Serialization;

namespace System.Web.Services.Description;

public enum SoapBindingStyle
{
	[XmlIgnore]
	Default,
	[XmlEnum("document")]
	Document,
	[XmlEnum("rpc")]
	Rpc
}
