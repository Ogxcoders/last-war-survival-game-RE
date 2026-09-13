using System.Web.Services.Configuration;

namespace System.Web.Services.Description;

[XmlFormatExtensionPrefix("soap12", "http://schemas.xmlsoap.org/wsdl/soap12/")]
[XmlFormatExtension("binding", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(Binding))]
public sealed class Soap12Binding : SoapBinding
{
	public new const string HttpTransport = "http://schemas.xmlsoap.org/soap/http";

	public new const string Namespace = "http://schemas.xmlsoap.org/wsdl/soap12/";
}
