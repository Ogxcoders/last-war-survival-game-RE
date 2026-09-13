using System.Web.Services.Configuration;

namespace System.Web.Services.Description;

[XmlFormatExtension("address", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(Port))]
public sealed class Soap12AddressBinding : SoapAddressBinding
{
}
