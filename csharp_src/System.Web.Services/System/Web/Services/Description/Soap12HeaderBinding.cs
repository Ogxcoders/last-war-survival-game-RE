using System.Web.Services.Configuration;

namespace System.Web.Services.Description;

[XmlFormatExtension("header", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(InputBinding), typeof(OutputBinding))]
public sealed class Soap12HeaderBinding : SoapHeaderBinding
{
}
