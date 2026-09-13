using System.Web.Services.Configuration;

namespace System.Web.Services.Description;

[XmlFormatExtension("urlReplacement", "http://schemas.xmlsoap.org/wsdl/http/", typeof(InputBinding))]
public sealed class HttpUrlReplacementBinding : ServiceDescriptionFormatExtension
{
}
