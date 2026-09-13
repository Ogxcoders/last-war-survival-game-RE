using System.Web.Services.Configuration;

namespace System.Web.Services.Description;

[XmlFormatExtension("urlEncoded", "http://schemas.xmlsoap.org/wsdl/http/", typeof(InputBinding))]
public sealed class HttpUrlEncodedBinding : ServiceDescriptionFormatExtension
{
}
