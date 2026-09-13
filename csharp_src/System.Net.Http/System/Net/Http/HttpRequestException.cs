namespace System.Net.Http;

[Serializable]
public class HttpRequestException : Exception
{
	public HttpRequestException()
		: this(null, null)
	{
	}

	public HttpRequestException(string message)
		: this(message, null)
	{
	}

	public HttpRequestException(string message, Exception inner)
		: base(message, inner)
	{
		if (inner != null)
		{
			base.HResult = inner.HResult;
		}
	}
}
