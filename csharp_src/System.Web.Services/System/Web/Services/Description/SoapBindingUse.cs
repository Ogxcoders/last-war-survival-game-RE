using System.Xml.Serialization;

namespace System.Web.Services.Description;

public enum SoapBindingUse
{
	[XmlIgnore]
	Default,
	[XmlEnum("encoded")]
	Encoded,
	[XmlEnum("literal")]
	Literal
}
