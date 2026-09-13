using System.IO;

namespace System.Web.Services.Protocols;

public class UrlParameterWriter : UrlEncodedParameterWriter
{
	public override string GetRequestUrl(string url, object[] parameters)
	{
		StringWriter stringWriter = new StringWriter();
		Encode(stringWriter, parameters);
		return url + "?" + stringWriter.ToString();
	}
}
