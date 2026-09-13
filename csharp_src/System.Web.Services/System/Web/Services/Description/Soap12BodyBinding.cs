using System.Web.Services.Configuration;

namespace System.Web.Services.Description;

[XmlFormatExtension("body", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(InputBinding), typeof(OutputBinding), typeof(MimePart))]
public sealed class Soap12BodyBinding : SoapBodyBinding
{
}
