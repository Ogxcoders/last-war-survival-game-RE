namespace WebSocketSharp.Net;

internal class HttpHeaderInfo
{
	private string _name;

	private HttpHeaderType _type;

	internal bool IsMultiValueInRequest => (_type & HttpHeaderType.MultiValueInRequest) == HttpHeaderType.MultiValueInRequest;

	internal bool IsMultiValueInResponse => (_type & HttpHeaderType.MultiValueInResponse) == HttpHeaderType.MultiValueInResponse;

	public bool IsRequest => (_type & HttpHeaderType.Request) == HttpHeaderType.Request;

	public bool IsResponse => (_type & HttpHeaderType.Response) == HttpHeaderType.Response;

	public string Name => _name;

	public HttpHeaderType Type => _type;

	internal HttpHeaderInfo(string name, HttpHeaderType type)
	{
		_name = name;
		_type = type;
	}

	public bool IsMultiValue(bool response)
	{
		if ((_type & HttpHeaderType.MultiValue) == HttpHeaderType.MultiValue)
		{
			if (!response)
			{
				return IsRequest;
			}
			return IsResponse;
		}
		if (!response)
		{
			return IsMultiValueInRequest;
		}
		return IsMultiValueInResponse;
	}

	public bool IsRestricted(bool response)
	{
		if ((_type & HttpHeaderType.Restricted) == HttpHeaderType.Restricted)
		{
			if (!response)
			{
				return IsRequest;
			}
			return IsResponse;
		}
		return false;
	}
}
