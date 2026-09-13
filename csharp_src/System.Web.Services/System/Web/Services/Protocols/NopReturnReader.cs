using System.IO;
using System.Net;

namespace System.Web.Services.Protocols;

public class NopReturnReader : MimeReturnReader
{
	public override object GetInitializer(LogicalMethodInfo methodInfo)
	{
		return this;
	}

	public override void Initialize(object initializer)
	{
	}

	public override object Read(WebResponse response, Stream responseStream)
	{
		responseStream.Close();
		return null;
	}
}
