using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("operation", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(OperationBinding))]
public sealed class Soap12OperationBinding : SoapOperationBinding
{
	private bool soapActionRequired;

	[DefaultValue(false)]
	[XmlAttribute("soapActionRequired")]
	public bool SoapActionRequired
	{
		get
		{
			return soapActionRequired;
		}
		set
		{
			soapActionRequired = value;
		}
	}
}
