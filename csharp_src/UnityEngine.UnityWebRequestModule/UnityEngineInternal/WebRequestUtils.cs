using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEngineInternal;

internal static class WebRequestUtils
{
	private static Regex domainRegex = new Regex("^\\s*\\w+(?:\\.\\w+)+(\\/.*)?$");

	[RequiredByNativeCode]
	internal static string RedirectTo(string baseUri, string redirectUri)
	{
		Uri uri = ((redirectUri[0] != '/') ? new Uri(redirectUri, UriKind.RelativeOrAbsolute) : new Uri(redirectUri, UriKind.Relative));
		if (uri.IsAbsoluteUri)
		{
			return uri.AbsoluteUri;
		}
		Uri baseUri2 = new Uri(baseUri, UriKind.Absolute);
		Uri uri2 = new Uri(baseUri2, uri);
		return uri2.AbsoluteUri;
	}

	internal static string MakeInitialUrl(string targetUrl, string localUrl)
	{
		if (string.IsNullOrEmpty(targetUrl))
		{
			return "";
		}
		bool prependProtocol = false;
		Uri uri = new Uri(localUrl);
		Uri uri2 = null;
		if (targetUrl[0] == '/')
		{
			uri2 = new Uri(uri, targetUrl);
			prependProtocol = true;
		}
		if (uri2 == null && domainRegex.IsMatch(targetUrl))
		{
			targetUrl = uri.Scheme + "://" + targetUrl;
			prependProtocol = true;
		}
		FormatException ex = null;
		try
		{
			if (uri2 == null && targetUrl[0] != '.')
			{
				uri2 = new Uri(targetUrl);
			}
		}
		catch (FormatException ex2)
		{
			ex = ex2;
		}
		if (uri2 == null)
		{
			try
			{
				uri2 = new Uri(uri, targetUrl);
				prependProtocol = true;
			}
			catch (FormatException)
			{
				throw ex;
			}
		}
		return MakeUriString(uri2, targetUrl, prependProtocol);
	}

	internal static string MakeUriString(Uri targetUri, string targetUrl, bool prependProtocol)
	{
		if (targetUri.IsFile)
		{
			if (!targetUri.IsLoopback)
			{
				return targetUri.OriginalString;
			}
			string text = targetUri.AbsolutePath;
			if (text.Contains("%"))
			{
				text = URLDecode(text);
			}
			if (text.Length > 0 && text[0] != '/')
			{
				text = "/" + text;
			}
			return "file://" + text;
		}
		string scheme = targetUri.Scheme;
		if (!prependProtocol && targetUrl.Length >= scheme.Length + 2 && targetUrl[scheme.Length + 1] != '/')
		{
			StringBuilder stringBuilder = new StringBuilder(scheme, targetUrl.Length);
			stringBuilder.Append(':');
			if (scheme == "jar")
			{
				string text2 = targetUri.AbsolutePath;
				if (text2.Contains("%"))
				{
					text2 = URLDecode(text2);
				}
				if (text2.StartsWith("file:/") && text2.Length > 6 && text2[6] != '/')
				{
					stringBuilder.Append("file://");
					stringBuilder.Append(text2.Substring(5));
				}
				else
				{
					stringBuilder.Append(text2);
				}
				return stringBuilder.ToString();
			}
			stringBuilder.Append(targetUri.PathAndQuery);
			stringBuilder.Append(targetUri.Fragment);
			return stringBuilder.ToString();
		}
		if (targetUrl.Contains("%"))
		{
			return targetUri.OriginalString;
		}
		return targetUri.AbsoluteUri;
	}

	private static string URLDecode(string encoded)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(encoded);
		byte[] bytes2 = WWWTranscoder.URLDecode(bytes);
		return Encoding.UTF8.GetString(bytes2);
	}
}
