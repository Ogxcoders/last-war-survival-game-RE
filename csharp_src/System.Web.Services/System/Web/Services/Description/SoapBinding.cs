using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPrefix("soap", "http://schemas.xmlsoap.org/wsdl/soap/")]
[XmlFormatExtensionPrefix("soapenc", "http://schemas.xmlsoap.org/soap/encoding/")]
[XmlFormatExtension("binding", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(Binding))]
public class SoapBinding : ServiceDescriptionFormatExtension
{
	public const string HttpTransport = "http://schemas.xmlsoap.org/soap/http";

	public const string Namespace = "http://schemas.xmlsoap.org/wsdl/soap/";

	private SoapBindingStyle style;

	private string transport;

	private static XmlSchema schema;

	public static XmlSchema Schema
	{
		get
		{
			if (schema == null)
			{
				schema = XmlSchema.Read(typeof(SoapBinding).Assembly.GetManifestResourceStream("wsdl-1.1-soap.xsd"), null);
			}
			return schema;
		}
	}

	[DefaultValue(SoapBindingStyle.Document)]
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

	[XmlAttribute("transport")]
	public string Transport
	{
		get
		{
			return transport;
		}
		set
		{
			transport = value;
		}
	}

	public SoapBinding()
	{
		style = SoapBindingStyle.Document;
		transport = string.Empty;
	}
}
