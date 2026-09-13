using System.Collections.Generic;

namespace System.Net.Http;

public class HttpMethod : IEquatable<HttpMethod>
{
	private readonly string _method;

	private int _hashcode;

	private static readonly HttpMethod s_getMethod = new HttpMethod("GET");

	private static readonly HttpMethod s_putMethod = new HttpMethod("PUT");

	private static readonly HttpMethod s_postMethod = new HttpMethod("POST");

	private static readonly HttpMethod s_deleteMethod = new HttpMethod("DELETE");

	private static readonly HttpMethod s_headMethod = new HttpMethod("HEAD");

	private static readonly HttpMethod s_optionsMethod = new HttpMethod("OPTIONS");

	private static readonly HttpMethod s_traceMethod = new HttpMethod("TRACE");

	private static readonly HttpMethod s_patchMethod = new HttpMethod("PATCH");

	private static readonly HttpMethod s_connectMethod = new HttpMethod("CONNECT");

	private static readonly Dictionary<HttpMethod, HttpMethod> s_knownMethods = new Dictionary<HttpMethod, HttpMethod>(9)
	{
		{ s_getMethod, s_getMethod },
		{ s_putMethod, s_putMethod },
		{ s_postMethod, s_postMethod },
		{ s_deleteMethod, s_deleteMethod },
		{ s_headMethod, s_headMethod },
		{ s_optionsMethod, s_optionsMethod },
		{ s_traceMethod, s_traceMethod },
		{ s_patchMethod, s_patchMethod },
		{ s_connectMethod, s_connectMethod }
	};

	public static HttpMethod Get => s_getMethod;

	public static HttpMethod Put => s_putMethod;

	public static HttpMethod Post => s_postMethod;

	public static HttpMethod Delete => s_deleteMethod;

	public static HttpMethod Head => s_headMethod;

	public static HttpMethod Options => s_optionsMethod;

	public static HttpMethod Trace => s_traceMethod;

	public static HttpMethod Patch => s_patchMethod;

	internal static HttpMethod Connect => s_connectMethod;

	public string Method => _method;

	public HttpMethod(string method)
	{
		if (string.IsNullOrEmpty(method))
		{
			throw new ArgumentException("The value cannot be null or empty.", "method");
		}
		if (HttpRuleParser.GetTokenLength(method, 0) != method.Length)
		{
			throw new FormatException("The format of the HTTP method is invalid.");
		}
		_method = method;
	}

	public bool Equals(HttpMethod other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)_method == other._method)
		{
			return true;
		}
		return string.Equals(_method, other._method, StringComparison.OrdinalIgnoreCase);
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as HttpMethod);
	}

	public override int GetHashCode()
	{
		if (_hashcode == 0)
		{
			_hashcode = StringComparer.OrdinalIgnoreCase.GetHashCode(_method);
		}
		return _hashcode;
	}

	public override string ToString()
	{
		return _method;
	}

	public static bool operator ==(HttpMethod left, HttpMethod right)
	{
		if ((object)left != null && (object)right != null)
		{
			return left.Equals(right);
		}
		return (object)left == right;
	}

	public static bool operator !=(HttpMethod left, HttpMethod right)
	{
		return !(left == right);
	}

	internal static HttpMethod Normalize(HttpMethod method)
	{
		if (!s_knownMethods.TryGetValue(method, out var value))
		{
			return method;
		}
		return value;
	}
}
