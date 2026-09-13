using System.IO;
using System.Net;
using System.Text;

namespace System.Web.Services.Protocols;

public abstract class MimeParameterWriter : MimeFormatter
{
	public virtual Encoding RequestEncoding
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual bool UsesWriteRequest => false;

	public virtual string GetRequestUrl(string url, object[] parameters)
	{
		return url;
	}

	public virtual void InitializeRequest(WebRequest request, object[] values)
	{
	}

	public virtual void WriteRequest(Stream requestStream, object[] values)
	{
	}
}
