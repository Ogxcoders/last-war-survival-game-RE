using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("operation", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(OperationBinding))]
public class SoapOperationBinding : ServiceDescriptionFormatExtension
{
	private string soapAction;

	private SoapBindingStyle style;

	[XmlAttribute("soapAction")]
	public string SoapAction
	{
		get
		{
			return soapAction;
		}
		set
		{
			soapAction = value;
		}
	}

	[DefaultValue(SoapBindingStyle.Default)]
	[XmlAttribute("style")]
	public SoapBindingStyle Style
	{
		get
		{
			return style;
		}
		set
		{
			style = value;
		}
	}

	public SoapOperationBinding()
	{
		soapAction = string.Empty;
		style = SoapBindingStyle.Default;
	}
}
