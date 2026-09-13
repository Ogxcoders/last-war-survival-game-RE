using System.Web.Services.Configuration;

namespace System.Web.Services.Description;

[XmlFormatExtension("fault", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(FaultBinding))]
public sealed class Soap12FaultBinding : SoapFaultBinding
{
}
