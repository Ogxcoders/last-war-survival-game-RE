using System.Collections.Generic;

namespace System.Net.Http;

internal sealed class HttpEnvironmentProxy : IWebProxy
{
	private const string EnvAllProxyUC = "ALL_PROXY";

	private const string EnvAllProxyLC = "all_proxy";

	private const string EnvHttpProxyLC = "http_proxy";

	private const string EnvHttpsProxyLC = "https_proxy";

	private const string EnvHttpsProxyUC = "HTTPS_PROXY";

	private const string EnvNoProxyLC = "no_proxy";

	private Uri _httpProxyUri;

	private Uri _httpsProxyUri;

	private string[] _bypass;

	private ICredentials _credentials;

	public ICredentials Credentials
	{
		get
		{
			return _credentials;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public static bool TryCreate(out IWebProxy proxy)
	{
		Uri uri = GetUriFromString(Environment.GetEnvironmentVariable("http_proxy"));
		Uri uri2 = GetUriFromString(Environment.GetEnvironmentVariable("https_proxy")) ?? GetUriFromString(Environment.GetEnvironmentVariable("HTTPS_PROXY"));
		if (uri == null || uri2 == null)
		{
			Uri uri3 = GetUriFromString(Environment.GetEnvironmentVariable("all_proxy")) ?? GetUriFromString(Environment.GetEnvironmentVariable("ALL_PROXY"));
			if (uri == null)
			{
				uri = uri3;
			}
			if (uri2 == null)
			{
				uri2 = uri3;
			}
		}
		if (uri == null && uri2 == null)
		{
			proxy = null;
			return false;
		}
		proxy = new HttpEnvironmentProxy(uri, uri2, Environment.GetEnvironmentVariable("no_proxy"));
		return true;
	}

	private HttpEnvironmentProxy(Uri httpProxy, Uri httpsProxy, string bypassList)
	{
		_httpProxyUri = httpProxy;
		_httpsProxyUri = httpsProxy;
		_credentials = HttpEnvironmentProxyCredentials.TryCreate(httpProxy, httpsProxy);
		if (string.IsNullOrWhiteSpace(bypassList))
		{
			return;
		}
		string[] array = bypassList.Split(',');
		List<string> list = new List<string>(array.Length);
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i].Trim();
			if (text.Length > 0)
			{
				list.Add(text);
			}
		}
		if (list.Count > 0)
		{
			_bypass = list.ToArray();
		}
	}

	private static Uri GetUriFromString(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return null;
		}
		if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
		{
			value = value.Substring(7);
		}
		string text = null;
		string text2 = null;
		ushort result = 80;
		string text3 = null;
		int num = value.LastIndexOf('@');
		if (num != -1)
		{
			string text4 = value.Substring(0, num);
			try
			{
				text4 = Uri.UnescapeDataString(text4);
			}
			catch
			{
			}
			value = value.Substring(num + 1);
			num = text4.IndexOf(':');
			if (num == -1)
			{
				text = text4;
			}
			else
			{
				text = text4.Substring(0, num);
				text2 = text4.Substring(num + 1);
			}
		}
		int num2 = value.IndexOf(']');
		num = value.LastIndexOf(':');
		if (num == -1 || (num2 != -1 && num < num2))
		{
			text3 = value;
		}
		else
		{
			text3 = value.Substring(0, num);
			int i;
			for (i = num + 1; i < value.Length && char.IsDigit(value[i]); i++)
			{
			}
			if (!ushort.TryParse(value.AsSpan(num + 1, i - num - 1), out result))
			{
				return null;
			}
		}
		try
		{
			UriBuilder uriBuilder = new UriBuilder("http", text3, result);
			if (text != null)
			{
				uriBuilder.UserName = Uri.EscapeDataString(text);
			}
			if (text2 != null)
			{
				uriBuilder.Password = Uri.EscapeDataString(text2);
			}
			return uriBuilder.Uri;
		}
		catch
		{
		}
		return null;
	}

	private bool IsMatchInBypassList(Uri input)
	{
		if (_bypass != null)
		{
			string[] bypass = _bypass;
			foreach (string text in bypass)
			{
				if (text[0] == '.')
				{
					if (text.Length - 1 == input.Host.Length && string.Compare(text, 1, input.Host, 0, input.Host.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return true;
					}
					if (input.Host.EndsWith(text, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
				else if (string.Equals(text, input.Host, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		return false;
	}

	public Uri GetProxy(Uri uri)
	{
		if (!(uri.Scheme == Uri.UriSchemeHttp))
		{
			return _httpsProxyUri;
		}
		return _httpProxyUri;
	}

	public bool IsBypassed(Uri uri)
	{
		if (!(GetProxy(uri) == null))
		{
			return IsMatchInBypassList(uri);
		}
		return true;
	}
}
