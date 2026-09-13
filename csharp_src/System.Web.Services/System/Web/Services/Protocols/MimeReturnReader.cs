using System.IO;
using System.Net;

namespace System.Web.Services.Protocols;

public abstract class MimeReturnReader : MimeFormatter
{
	public abstract object Read(WebResponse response, Stream responseStream);
}
