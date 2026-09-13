using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Net;

[Serializable]
[ComVisible(true)]
public class WebHeaderCollection : NameValueCollection, ISerializable
{
	internal static class HeaderEncoding
	{
		internal unsafe static string GetString(byte[] bytes, int byteIndex, int byteCount)
		{
			fixed (byte* ptr = bytes)
			{
				return GetString(ptr + byteIndex, byteCount);
			}
		}

		internal unsafe static string GetString(byte* pBytes, int byteCount)
		{
			if (byteCount < 1)
			{
				return "";
			}
			string text = new string('\0', byteCount);
			fixed (char* ptr = text)
			{
				char* ptr2 = ptr;
				while (byteCount >= 8)
				{
					*ptr2 = (char)(*pBytes);
					ptr2[1] = (char)pBytes[1];
					ptr2[2] = (char)pBytes[2];
					ptr2[3] = (char)pBytes[3];
					ptr2[4] = (char)pBytes[4];
					ptr2[5] = (char)pBytes[5];
					ptr2[6] = (char)pBytes[6];
					ptr2[7] = (char)pBytes[7];
					ptr2 += 8;
					pBytes += 8;
					byteCount -= 8;
				}
				for (int i = 0; i < byteCount; i++)
				{
					ptr2[i] = (char)pBytes[i];
				}
			}
			return text;
		}

		internal static int GetByteCount(string myString)
		{
			return myString.Length;
		}

		internal unsafe static void GetBytes(string myString, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (myString.Length == 0)
			{
				return;
			}
			fixed (byte* ptr = bytes)
			{
				byte* ptr2 = ptr + byteIndex;
				int num = charIndex + charCount;
				while (charIndex < num)
				{
					*(ptr2++) = (byte)myString[charIndex++];
				}
			}
		}

		internal static byte[] GetBytes(string myString)
		{
			byte[] array = new byte[myString.Length];
			if (myString.Length != 0)
			{
				GetBytes(myString, 0, myString.Length, array, 0);
			}
			return array;
		}

		[System.Runtime.CompilerServices.FriendAccessAllowed]
		internal static string DecodeUtf8FromString(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}
			bool flag = false;
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] > 'ÿ')
				{
					return input;
				}
				if (input[i] > '\u007f')
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				byte[] array = new byte[input.Length];
				for (int j = 0; j < input.Length; j++)
				{
					if (input[j] > 'ÿ')
					{
						return input;
					}
					array[j] = (byte)input[j];
				}
				try
				{
					return Encoding.GetEncoding("utf-8", EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback).GetString(array);
				}
				catch (ArgumentException)
				{
				}
			}
			return input;
		}
	}

	private enum RfcChar : byte
	{
		High,
		Reg,
		Ctl,
		CR,
		LF,
		WS,
		Colon,
		Delim
	}

	private const int ApproxAveHeaderLineSize = 30;

	private const int ApproxHighAvgNumHeaders = 16;

	private static readonly HeaderInfoTable HInfo = new HeaderInfoTable();

	private string[] m_CommonHeaders;

	private int m_NumCommonHeaders;

	private static readonly string[] s_CommonHeaderNames = new string[19]
	{
		"Accept-Ranges", "Content-Length", "Cache-Control", "Content-Type", "Date", "Expires", "ETag", "Last-Modified", "Location", "Proxy-Authenticate",
		"P3P", "Set-Cookie2", "Set-Cookie", "Server", "Via", "WWW-Authenticate", "X-AspNet-Version", "X-Powered-By", "["
	};

	private static readonly sbyte[] s_CommonHeaderHints = new sbyte[32]
	{
		-1, 0, -1, 1, 4, 5, -1, -1, -1, -1,
		-1, -1, 7, -1, -1, -1, 9, -1, -1, 11,
		-1, -1, 14, 15, 16, -1, -1, -1, -1, -1,
		-1, -1
	};

	private const int c_AcceptRanges = 0;

	private const int c_ContentLength = 1;

	private const int c_CacheControl = 2;

	private const int c_ContentType = 3;

	private const int c_Date = 4;

	private const int c_Expires = 5;

	private const int c_ETag = 6;

	private const int c_LastModified = 7;

	private const int c_Location = 8;

	private const int c_ProxyAuthenticate = 9;

	private const int c_P3P = 10;

	private const int c_SetCookie2 = 11;

	private const int c_SetCookie = 12;

	private const int c_Server = 13;

	private const int c_Via = 14;

	private const int c_WwwAuthenticate = 15;

	private const int c_XAspNetVersion = 16;

	private const int c_XPoweredBy = 17;

	private NameValueCollection m_InnerCollection;

	private WebHeaderCollectionType m_Type;

	private static readonly char[] HttpTrimCharacters = new char[6] { '\t', '\n', '\v', '\f', '\r', ' ' };

	private static RfcChar[] RfcCharMap = new RfcChar[128]
	{
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.WS,
		RfcChar.LF,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.CR,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.Ctl,
		RfcChar.WS,
		RfcChar.Reg,
		RfcChar.Delim,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Delim,
		RfcChar.Delim,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Delim,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Delim,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Colon,
		RfcChar.Delim,
		RfcChar.Delim,
		RfcChar.Delim,
		RfcChar.Delim,
		RfcChar.Delim,
		RfcChar.Delim,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Delim,
		RfcChar.Delim,
		RfcChar.Delim,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Reg,
		RfcChar.Delim,
		RfcChar.Reg,
		RfcChar.Delim,
		RfcChar.Reg,
		RfcChar.Ctl
	};

	internal string ContentLength
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[1]);
			}
			return m_CommonHeaders[1];
		}
	}

	internal string CacheControl
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[2]);
			}
			return m_CommonHeaders[2];
		}
	}

	internal string ContentType
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[3]);
			}
			return m_CommonHeaders[3];
		}
	}

	internal string Date
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[4]);
			}
			return m_CommonHeaders[4];
		}
	}

	internal string Expires
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[5]);
			}
			return m_CommonHeaders[5];
		}
	}

	internal string ETag
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[6]);
			}
			return m_CommonHeaders[6];
		}
	}

	internal string LastModified
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[7]);
			}
			return m_CommonHeaders[7];
		}
	}

	internal string Location => HeaderEncoding.DecodeUtf8FromString((m_CommonHeaders != null) ? m_CommonHeaders[8] : Get(s_CommonHeaderNames[8]));

	internal string ProxyAuthenticate
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[9]);
			}
			return m_CommonHeaders[9];
		}
	}

	internal string SetCookie2
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[11]);
			}
			return m_CommonHeaders[11];
		}
	}

	internal string SetCookie
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[12]);
			}
			return m_CommonHeaders[12];
		}
	}

	internal string Server
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[13]);
			}
			return m_CommonHeaders[13];
		}
	}

	internal string Via
	{
		get
		{
			if (m_CommonHeaders == null)
			{
				return Get(s_CommonHeaderNames[14]);
			}
			return m_CommonHeaders[14];
		}
	}

	private NameValueCollection InnerCollection
	{
		get
		{
			if (m_InnerCollection == null)
			{
				m_InnerCollection = new NameValueCollection(16, CaseInsensitiveAscii.StaticInstance);
			}
			return m_InnerCollection;
		}
	}

	private bool AllowHttpRequestHeader
	{
		get
		{
			if (m_Type == WebHeaderCollectionType.Unknown)
			{
				m_Type = WebHeaderCollectionType.WebRequest;
			}
			if (m_Type != WebHeaderCollectionType.WebRequest && m_Type != WebHeaderCollectionType.HttpWebRequest)
			{
				return m_Type == WebHeaderCollectionType.HttpListenerRequest;
			}
			return true;
		}
	}

	internal bool AllowHttpResponseHeader
	{
		get
		{
			if (m_Type == WebHeaderCollectionType.Unknown)
			{
				m_Type = WebHeaderCollectionType.WebResponse;
			}
			if (m_Type != WebHeaderCollectionType.WebResponse && m_Type != WebHeaderCollectionType.HttpWebResponse)
			{
				return m_Type == WebHeaderCollectionType.HttpListenerResponse;
			}
			return true;
		}
	}

	public string this[HttpRequestHeader header]
	{
		get
		{
			if (!AllowHttpRequestHeader)
			{
				throw new InvalidOperationException(global::SR.GetString("This collection holds response headers and cannot contain the specified request header."));
			}
			return base[UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header)];
		}
		set
		{
			if (!AllowHttpRequestHeader)
			{
				throw new InvalidOperationException(global::SR.GetString("This collection holds response headers and cannot contain the specified request header."));
			}
			base[UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header)] = value;
		}
	}

	public string this[HttpResponseHeader header]
	{
		get
		{
			if (!AllowHttpResponseHeader)
			{
				throw new InvalidOperationException(global::SR.GetString("This collection holds request headers and cannot contain the specified response header."));
			}
			if (m_CommonHeaders != null)
			{
				switch (header)
				{
				case HttpResponseHeader.ProxyAuthenticate:
					return m_CommonHeaders[9];
				case HttpResponseHeader.WwwAuthenticate:
					return m_CommonHeaders[15];
				}
			}
			return base[UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header)];
		}
		set
		{
			if (!AllowHttpResponseHeader)
			{
				throw new InvalidOperationException(global::SR.GetString("This collection holds request headers and cannot contain the specified response header."));
			}
			if (m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException("value", value, global::SR.GetString("Header values cannot be longer than {0} characters.", ushort.MaxValue));
			}
			base[UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header)] = value;
		}
	}

	public override int Count => ((m_InnerCollection != null) ? m_InnerCollection.Count : 0) + m_NumCommonHeaders;

	public override KeysCollection Keys
	{
		get
		{
			NormalizeCommonHeaders();
			return InnerCollection.Keys;
		}
	}

	public override string[] AllKeys
	{
		get
		{
			NormalizeCommonHeaders();
			return InnerCollection.AllKeys;
		}
	}

	private void NormalizeCommonHeaders()
	{
		if (m_CommonHeaders == null)
		{
			return;
		}
		for (int i = 0; i < m_CommonHeaders.Length; i++)
		{
			if (m_CommonHeaders[i] != null)
			{
				InnerCollection.Add(s_CommonHeaderNames[i], m_CommonHeaders[i]);
			}
		}
		m_CommonHeaders = null;
		m_NumCommonHeaders = 0;
	}

	internal static bool AllowMultiValues(string name)
	{
		HeaderInfo headerInfo = HInfo[name];
		if (!headerInfo.AllowMultiValues)
		{
			return headerInfo.HeaderName == "";
		}
		return true;
	}

	public void Add(HttpRequestHeader header, string value)
	{
		if (!AllowHttpRequestHeader)
		{
			throw new InvalidOperationException(global::SR.GetString("This collection holds response headers and cannot contain the specified request header."));
		}
		Add(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header), value);
	}

	public void Add(HttpResponseHeader header, string value)
	{
		if (!AllowHttpResponseHeader)
		{
			throw new InvalidOperationException(global::SR.GetString("This collection holds request headers and cannot contain the specified response header."));
		}
		if (m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
		{
			throw new ArgumentOutOfRangeException("value", value, global::SR.GetString("Header values cannot be longer than {0} characters.", ushort.MaxValue));
		}
		Add(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header), value);
	}

	public void Set(HttpRequestHeader header, string value)
	{
		if (!AllowHttpRequestHeader)
		{
			throw new InvalidOperationException(global::SR.GetString("This collection holds response headers and cannot contain the specified request header."));
		}
		Set(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header), value);
	}

	public void Set(HttpResponseHeader header, string value)
	{
		if (!AllowHttpResponseHeader)
		{
			throw new InvalidOperationException(global::SR.GetString("This collection holds request headers and cannot contain the specified response header."));
		}
		if (m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
		{
			throw new ArgumentOutOfRangeException("value", value, global::SR.GetString("Header values cannot be longer than {0} characters.", ushort.MaxValue));
		}
		Set(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header), value);
	}

	internal void SetInternal(HttpResponseHeader header, string value)
	{
		if (!AllowHttpResponseHeader)
		{
			throw new InvalidOperationException(global::SR.GetString("This collection holds request headers and cannot contain the specified response header."));
		}
		if (m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
		{
			throw new ArgumentOutOfRangeException("value", value, global::SR.GetString("Header values cannot be longer than {0} characters.", ushort.MaxValue));
		}
		SetInternal(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header), value);
	}

	public void Remove(HttpRequestHeader header)
	{
		if (!AllowHttpRequestHeader)
		{
			throw new InvalidOperationException(global::SR.GetString("This collection holds response headers and cannot contain the specified request header."));
		}
		Remove(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header));
	}

	public void Remove(HttpResponseHeader header)
	{
		if (!AllowHttpResponseHeader)
		{
			throw new InvalidOperationException(global::SR.GetString("This collection holds request headers and cannot contain the specified response header."));
		}
		Remove(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header));
	}

	protected void AddWithoutValidate(string headerName, string headerValue)
	{
		headerName = CheckBadChars(headerName, isHeaderValue: false);
		headerValue = CheckBadChars(headerValue, isHeaderValue: true);
		if (m_Type == WebHeaderCollectionType.HttpListenerResponse && headerValue != null && headerValue.Length > 65535)
		{
			throw new ArgumentOutOfRangeException("headerValue", headerValue, global::SR.GetString("Header values cannot be longer than {0} characters.", ushort.MaxValue));
		}
		NormalizeCommonHeaders();
		InvalidateCachedArrays();
		InnerCollection.Add(headerName, headerValue);
	}

	internal void SetAddVerified(string name, string value)
	{
		if (HInfo[name].AllowMultiValues)
		{
			NormalizeCommonHeaders();
			InvalidateCachedArrays();
			InnerCollection.Add(name, value);
		}
		else
		{
			NormalizeCommonHeaders();
			InvalidateCachedArrays();
			InnerCollection.Set(name, value);
		}
	}

	internal void AddInternal(string name, string value)
	{
		NormalizeCommonHeaders();
		InvalidateCachedArrays();
		InnerCollection.Add(name, value);
	}

	internal void ChangeInternal(string name, string value)
	{
		NormalizeCommonHeaders();
		InvalidateCachedArrays();
		InnerCollection.Set(name, value);
	}

	internal void RemoveInternal(string name)
	{
		NormalizeCommonHeaders();
		if (m_InnerCollection != null)
		{
			InvalidateCachedArrays();
			m_InnerCollection.Remove(name);
		}
	}

	internal void CheckUpdate(string name, string value)
	{
		value = CheckBadChars(value, isHeaderValue: true);
		ChangeInternal(name, value);
	}

	private void AddInternalNotCommon(string name, string value)
	{
		InvalidateCachedArrays();
		InnerCollection.Add(name, value);
	}

	internal static string CheckBadChars(string name, bool isHeaderValue)
	{
		if (name == null || name.Length == 0)
		{
			if (!isHeaderValue)
			{
				throw (name == null) ? new ArgumentNullException("name") : new ArgumentException(global::SR.GetString("The parameter '{0}' cannot be an empty string.", "name"), "name");
			}
			return string.Empty;
		}
		if (isHeaderValue)
		{
			name = name.Trim(HttpTrimCharacters);
			int num = 0;
			for (int i = 0; i < name.Length; i++)
			{
				char c = (char)(0xFF & name[i]);
				switch (num)
				{
				case 0:
					if (c == '\r')
					{
						num = 1;
					}
					else if (c == '\n')
					{
						num = 2;
					}
					else if (c == '\u007f' || (c < ' ' && c != '\t'))
					{
						throw new ArgumentException(global::SR.GetString("Specified value has invalid Control characters."), "value");
					}
					break;
				case 1:
					if (c == '\n')
					{
						num = 2;
						break;
					}
					throw new ArgumentException(global::SR.GetString("Specified value has invalid CRLF characters."), "value");
				case 2:
					if (c == ' ' || c == '\t')
					{
						num = 0;
						break;
					}
					throw new ArgumentException(global::SR.GetString("Specified value has invalid CRLF characters."), "value");
				}
			}
			if (num != 0)
			{
				throw new ArgumentException(global::SR.GetString("Specified value has invalid CRLF characters."), "value");
			}
		}
		else
		{
			if (name.IndexOfAny(ValidationHelper.InvalidParamChars) != -1)
			{
				throw new ArgumentException(global::SR.GetString("Specified value has invalid HTTP Header characters."), "name");
			}
			if (ContainsNonAsciiChars(name))
			{
				throw new ArgumentException(global::SR.GetString("Specified value has invalid non-ASCII characters."), "name");
			}
		}
		return name;
	}

	internal static bool IsValidToken(string token)
	{
		if (token.Length > 0 && token.IndexOfAny(ValidationHelper.InvalidParamChars) == -1)
		{
			return !ContainsNonAsciiChars(token);
		}
		return false;
	}

	internal static bool ContainsNonAsciiChars(string token)
	{
		for (int i = 0; i < token.Length; i++)
		{
			if (token[i] < ' ' || token[i] > '~')
			{
				return true;
			}
		}
		return false;
	}

	internal void ThrowOnRestrictedHeader(string headerName)
	{
		if (m_Type == WebHeaderCollectionType.HttpWebRequest)
		{
			if (HInfo[headerName].IsRequestRestricted)
			{
				throw new ArgumentException(global::SR.GetString("The '{0}' header must be modified using the appropriate property or method.", headerName), "name");
			}
		}
		else if (m_Type == WebHeaderCollectionType.HttpListenerResponse && HInfo[headerName].IsResponseRestricted)
		{
			throw new ArgumentException(global::SR.GetString("The '{0}' header must be modified using the appropriate property or method.", headerName), "name");
		}
	}

	public override void Add(string name, string value)
	{
		name = CheckBadChars(name, isHeaderValue: false);
		ThrowOnRestrictedHeader(name);
		value = CheckBadChars(value, isHeaderValue: true);
		if (m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
		{
			throw new ArgumentOutOfRangeException("value", value, global::SR.GetString("Header values cannot be longer than {0} characters.", ushort.MaxValue));
		}
		NormalizeCommonHeaders();
		InvalidateCachedArrays();
		InnerCollection.Add(name, value);
	}

	public void Add(string header)
	{
		if (ValidationHelper.IsBlankString(header))
		{
			throw new ArgumentNullException("header");
		}
		int num = header.IndexOf(':');
		if (num < 0)
		{
			throw new ArgumentException(global::SR.GetString("Specified value does not have a ':' separator."), "header");
		}
		string name = header.Substring(0, num);
		string name2 = header.Substring(num + 1);
		name = CheckBadChars(name, isHeaderValue: false);
		ThrowOnRestrictedHeader(name);
		name2 = CheckBadChars(name2, isHeaderValue: true);
		if (m_Type == WebHeaderCollectionType.HttpListenerResponse && name2 != null && name2.Length > 65535)
		{
			throw new ArgumentOutOfRangeException("value", name2, global::SR.GetString("Header values cannot be longer than {0} characters.", ushort.MaxValue));
		}
		NormalizeCommonHeaders();
		InvalidateCachedArrays();
		InnerCollection.Add(name, name2);
	}

	public override void Set(string name, string value)
	{
		if (ValidationHelper.IsBlankString(name))
		{
			throw new ArgumentNullException("name");
		}
		name = CheckBadChars(name, isHeaderValue: false);
		ThrowOnRestrictedHeader(name);
		value = CheckBadChars(value, isHeaderValue: true);
		if (m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
		{
			throw new ArgumentOutOfRangeException("value", value, global::SR.GetString("Header values cannot be longer than {0} characters.", ushort.MaxValue));
		}
		NormalizeCommonHeaders();
		InvalidateCachedArrays();
		InnerCollection.Set(name, value);
	}

	internal void SetInternal(string name, string value)
	{
		if (ValidationHelper.IsBlankString(name))
		{
			throw new ArgumentNullException("name");
		}
		name = CheckBadChars(name, isHeaderValue: false);
		value = CheckBadChars(value, isHeaderValue: true);
		if (m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
		{
			throw new ArgumentOutOfRangeException("value", value, global::SR.GetString("Header values cannot be longer than {0} characters.", ushort.MaxValue));
		}
		NormalizeCommonHeaders();
		InvalidateCachedArrays();
		InnerCollection.Set(name, value);
	}

	public override void Remove(string name)
	{
		if (ValidationHelper.IsBlankString(name))
		{
			throw new ArgumentNullException("name");
		}
		ThrowOnRestrictedHeader(name);
		name = CheckBadChars(name, isHeaderValue: false);
		NormalizeCommonHeaders();
		if (m_InnerCollection != null)
		{
			InvalidateCachedArrays();
			m_InnerCollection.Remove(name);
		}
	}

	public override string[] GetValues(string header)
	{
		NormalizeCommonHeaders();
		HeaderInfo headerInfo = HInfo[header];
		string[] values = InnerCollection.GetValues(header);
		if (headerInfo == null || values == null || !headerInfo.AllowMultiValues)
		{
			return values;
		}
		ArrayList arrayList = null;
		for (int i = 0; i < values.Length; i++)
		{
			string[] array = headerInfo.Parser(values[i]);
			if (arrayList == null)
			{
				if (array.Length > 1)
				{
					arrayList = new ArrayList(values);
					arrayList.RemoveRange(i, values.Length - i);
					arrayList.AddRange(array);
				}
			}
			else
			{
				arrayList.AddRange(array);
			}
		}
		if (arrayList != null)
		{
			string[] array2 = new string[arrayList.Count];
			arrayList.CopyTo(array2);
			return array2;
		}
		return values;
	}

	public override string ToString()
	{
		return GetAsString(this, winInetCompat: false, forTrace: false);
	}

	internal string ToString(bool forTrace)
	{
		return GetAsString(this, winInetCompat: false, forTrace: true);
	}

	internal static string GetAsString(NameValueCollection cc, bool winInetCompat, bool forTrace)
	{
		if (winInetCompat)
		{
			throw new InvalidOperationException();
		}
		if (cc == null || cc.Count == 0)
		{
			return "\r\n";
		}
		StringBuilder stringBuilder = new StringBuilder(30 * cc.Count);
		string text = cc[string.Empty];
		if (text != null)
		{
			stringBuilder.Append(text).Append("\r\n");
		}
		for (int i = 0; i < cc.Count; i++)
		{
			string key = cc.GetKey(i);
			string value = cc.Get(i);
			if (!ValidationHelper.IsBlankString(key))
			{
				stringBuilder.Append(key);
				if (winInetCompat)
				{
					stringBuilder.Append(':');
				}
				else
				{
					stringBuilder.Append(": ");
				}
				stringBuilder.Append(value).Append("\r\n");
			}
		}
		if (!forTrace)
		{
			stringBuilder.Append("\r\n");
		}
		return stringBuilder.ToString();
	}

	public byte[] ToByteArray()
	{
		return HeaderEncoding.GetBytes(ToString());
	}

	public static bool IsRestricted(string headerName)
	{
		return IsRestricted(headerName, response: false);
	}

	public static bool IsRestricted(string headerName, bool response)
	{
		if (!response)
		{
			return HInfo[CheckBadChars(headerName, isHeaderValue: false)].IsRequestRestricted;
		}
		return HInfo[CheckBadChars(headerName, isHeaderValue: false)].IsResponseRestricted;
	}

	public WebHeaderCollection()
		: base(DBNull.Value)
	{
	}

	internal WebHeaderCollection(WebHeaderCollectionType type)
		: base(DBNull.Value)
	{
		m_Type = type;
		if (type == WebHeaderCollectionType.HttpWebResponse)
		{
			m_CommonHeaders = new string[s_CommonHeaderNames.Length - 1];
		}
	}

	internal WebHeaderCollection(NameValueCollection cc)
		: base(DBNull.Value)
	{
		m_InnerCollection = new NameValueCollection(cc.Count + 2, CaseInsensitiveAscii.StaticInstance);
		int count = cc.Count;
		for (int i = 0; i < count; i++)
		{
			string key = cc.GetKey(i);
			string[] values = cc.GetValues(i);
			if (values != null)
			{
				for (int j = 0; j < values.Length; j++)
				{
					InnerCollection.Add(key, values[j]);
				}
			}
			else
			{
				InnerCollection.Add(key, null);
			}
		}
	}

	protected WebHeaderCollection(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(DBNull.Value)
	{
		int @int = serializationInfo.GetInt32("Count");
		m_InnerCollection = new NameValueCollection(@int + 2, CaseInsensitiveAscii.StaticInstance);
		for (int i = 0; i < @int; i++)
		{
			string name = serializationInfo.GetString(i.ToString(NumberFormatInfo.InvariantInfo));
			string value = serializationInfo.GetString((i + @int).ToString(NumberFormatInfo.InvariantInfo));
			InnerCollection.Add(name, value);
		}
	}

	public override void OnDeserialization(object sender)
	{
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
		NormalizeCommonHeaders();
		serializationInfo.AddValue("Count", Count);
		for (int i = 0; i < Count; i++)
		{
			serializationInfo.AddValue(i.ToString(NumberFormatInfo.InvariantInfo), GetKey(i));
			serializationInfo.AddValue((i + Count).ToString(NumberFormatInfo.InvariantInfo), Get(i));
		}
	}

	internal unsafe DataParseStatus ParseHeaders(byte[] buffer, int size, ref int unparsed, ref int totalResponseHeadersLength, int maximumResponseHeadersLength, ref WebParseError parseError)
	{
		fixed (byte* ptr = buffer)
		{
			if (buffer.Length < size)
			{
				return DataParseStatus.NeedMoreData;
			}
			int num = -1;
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			int num5 = -1;
			int num6 = unparsed;
			int num7 = totalResponseHeadersLength;
			WebParseErrorCode code = WebParseErrorCode.Generic;
			DataParseStatus dataParseStatus = DataParseStatus.Invalid;
			while (true)
			{
				string text = string.Empty;
				string text2 = string.Empty;
				bool flag = false;
				string text3 = null;
				char c;
				if (Count == 0)
				{
					while (num6 < size)
					{
						c = (char)ptr[num6];
						if (c != ' ' && c != '\t')
						{
							break;
						}
						num6++;
						if (maximumResponseHeadersLength < 0 || ++num7 < maximumResponseHeadersLength)
						{
							continue;
						}
						goto IL_0081;
					}
					if (num6 == size)
					{
						dataParseStatus = DataParseStatus.NeedMoreData;
						break;
					}
				}
				num = num6;
				while (num6 < size)
				{
					c = (char)ptr[num6];
					if (c != ':' && c != '\n')
					{
						if (c > ' ')
						{
							num2 = num6;
						}
						num6++;
						if (maximumResponseHeadersLength < 0 || ++num7 < maximumResponseHeadersLength)
						{
							continue;
						}
						goto IL_00cf;
					}
					goto IL_00d7;
				}
				goto IL_00ff;
				IL_02c4:
				dataParseStatus = DataParseStatus.DataTooBig;
				break;
				IL_02c9:
				if (num >= 0 && num2 >= num)
				{
					text = HeaderEncoding.GetString(ptr + num, num2 - num + 1);
				}
				if (text.Length > 0)
				{
					AddInternal(text, text3);
				}
				totalResponseHeadersLength = num7;
				unparsed = num6;
				if (num5 == 2)
				{
					dataParseStatus = DataParseStatus.Done;
					break;
				}
				continue;
				IL_017c:
				dataParseStatus = DataParseStatus.DataTooBig;
				break;
				IL_0081:
				dataParseStatus = DataParseStatus.DataTooBig;
				break;
				IL_024b:
				dataParseStatus = DataParseStatus.NeedMoreData;
				break;
				IL_01a7:
				dataParseStatus = DataParseStatus.NeedMoreData;
				break;
				IL_00cf:
				dataParseStatus = DataParseStatus.DataTooBig;
				break;
				IL_00d7:
				if (c == ':')
				{
					num6++;
					if (maximumResponseHeadersLength >= 0 && ++num7 >= maximumResponseHeadersLength)
					{
						dataParseStatus = DataParseStatus.DataTooBig;
						break;
					}
				}
				goto IL_00ff;
				IL_01df:
				dataParseStatus = DataParseStatus.DataTooBig;
				break;
				IL_01f1:
				dataParseStatus = DataParseStatus.NeedMoreData;
				break;
				IL_022f:
				dataParseStatus = DataParseStatus.DataTooBig;
				break;
				IL_00ff:
				if (num6 == size)
				{
					dataParseStatus = DataParseStatus.NeedMoreData;
					break;
				}
				while (true)
				{
					num5 = ((Count == 0 && num2 < 0) ? 1 : 0);
					while (num6 < size && num5 < 2)
					{
						c = (char)ptr[num6];
						if (c > ' ')
						{
							break;
						}
						if (c == '\n')
						{
							num5++;
							if (num5 == 1)
							{
								if (num6 + 1 == size)
								{
									goto end_IL_010c;
								}
								flag = ptr[num6 + 1] == 32 || ptr[num6 + 1] == 9;
							}
						}
						num6++;
						if (maximumResponseHeadersLength < 0 || ++num7 < maximumResponseHeadersLength)
						{
							continue;
						}
						goto IL_017c;
					}
					if (num5 != 2 && (num5 != 1 || flag))
					{
						if (num6 == size)
						{
							goto IL_01a7;
						}
						num3 = num6;
						while (num6 < size)
						{
							c = (char)ptr[num6];
							if (c == '\n')
							{
								break;
							}
							if (c > ' ')
							{
								num4 = num6;
							}
							num6++;
							if (maximumResponseHeadersLength < 0 || ++num7 < maximumResponseHeadersLength)
							{
								continue;
							}
							goto IL_01df;
						}
						if (num6 == size)
						{
							goto IL_01f1;
						}
						num5 = 0;
						while (num6 < size && num5 < 2)
						{
							c = (char)ptr[num6];
							if (c != '\r' && c != '\n')
							{
								break;
							}
							if (c == '\n')
							{
								num5++;
							}
							num6++;
							if (maximumResponseHeadersLength < 0 || ++num7 < maximumResponseHeadersLength)
							{
								continue;
							}
							goto IL_022f;
						}
						if (num6 == size && num5 < 2)
						{
							goto IL_024b;
						}
					}
					if (num3 >= 0 && num3 > num2 && num4 >= num3)
					{
						text2 = HeaderEncoding.GetString(ptr + num3, num4 - num3 + 1);
					}
					text3 = ((text3 == null) ? text2 : (text3 + " " + text2));
					if (num6 < size && num5 == 1)
					{
						c = (char)ptr[num6];
						if (c == ' ' || c == '\t')
						{
							num6++;
							if (maximumResponseHeadersLength < 0 || ++num7 < maximumResponseHeadersLength)
							{
								continue;
							}
							goto IL_02c4;
						}
					}
					goto IL_02c9;
					continue;
					end_IL_010c:
					break;
				}
				dataParseStatus = DataParseStatus.NeedMoreData;
				break;
			}
			if (dataParseStatus == DataParseStatus.Invalid)
			{
				parseError.Section = WebParseErrorSection.ResponseHeader;
				parseError.Code = code;
			}
			return dataParseStatus;
		}
	}

	internal unsafe DataParseStatus ParseHeadersStrict(byte[] buffer, int size, ref int unparsed, ref int totalResponseHeadersLength, int maximumResponseHeadersLength, ref WebParseError parseError)
	{
		WebParseErrorCode code = WebParseErrorCode.Generic;
		DataParseStatus dataParseStatus = DataParseStatus.Invalid;
		int i = unparsed;
		int num = ((maximumResponseHeadersLength <= 0) ? int.MaxValue : (maximumResponseHeadersLength - totalResponseHeadersLength + i));
		DataParseStatus dataParseStatus2 = DataParseStatus.DataTooBig;
		if (size < num)
		{
			num = size;
			dataParseStatus2 = DataParseStatus.NeedMoreData;
		}
		if (i >= num)
		{
			dataParseStatus = dataParseStatus2;
		}
		else
		{
			fixed (byte* ptr = buffer)
			{
				while (true)
				{
					IL_0056:
					if (ptr[i] == 13)
					{
						if (++i == num)
						{
							dataParseStatus = dataParseStatus2;
						}
						else if (ptr[i++] == 10)
						{
							totalResponseHeadersLength += i - unparsed;
							unparsed = i;
							dataParseStatus = DataParseStatus.Done;
						}
						else
						{
							dataParseStatus = DataParseStatus.Invalid;
							code = WebParseErrorCode.CrLfError;
						}
						break;
					}
					int num2 = i;
					for (; i < num; i++)
					{
						RfcChar rfcChar;
						if ((rfcChar = ((ptr[i] <= 127) ? RfcCharMap[ptr[i]] : RfcChar.High)) != RfcChar.Reg)
						{
							break;
						}
					}
					if (i == num)
					{
						dataParseStatus = dataParseStatus2;
						break;
					}
					if (i == num2)
					{
						dataParseStatus = DataParseStatus.Invalid;
						code = WebParseErrorCode.InvalidHeaderName;
						break;
					}
					int num3 = i - 1;
					int num4 = 0;
					while (true)
					{
						RfcChar rfcChar;
						if (i < num && (rfcChar = ((ptr[i] <= 127) ? RfcCharMap[ptr[i]] : RfcChar.High)) != RfcChar.Colon)
						{
							switch (rfcChar)
							{
							case RfcChar.WS:
								if (num4 != 1)
								{
									num4 = 0;
									goto IL_0126;
								}
								break;
							case RfcChar.CR:
								if (num4 == 0)
								{
									num4 = 1;
									goto IL_0126;
								}
								break;
							case RfcChar.LF:
								if (num4 == 1)
								{
									num4 = 2;
									goto IL_0126;
								}
								break;
							}
							dataParseStatus = DataParseStatus.Invalid;
							code = WebParseErrorCode.CrLfError;
							break;
						}
						if (i == num)
						{
							dataParseStatus = dataParseStatus2;
							break;
						}
						if (num4 != 0)
						{
							dataParseStatus = DataParseStatus.Invalid;
							code = WebParseErrorCode.IncompleteHeaderLine;
							break;
						}
						if (++i == num)
						{
							dataParseStatus = dataParseStatus2;
							break;
						}
						int num5 = -1;
						int num6 = -1;
						StringBuilder stringBuilder = null;
						while (true)
						{
							if (i < num && ((rfcChar = ((ptr[i] <= 127) ? RfcCharMap[ptr[i]] : RfcChar.High)) == RfcChar.WS || num4 != 2))
							{
								switch (rfcChar)
								{
								case RfcChar.WS:
									switch (num4)
									{
									case 2:
										num4 = 3;
										goto IL_0247;
									case 1:
										break;
									default:
										goto IL_0247;
									}
									break;
								case RfcChar.CR:
									if (num4 == 0)
									{
										num4 = 1;
										goto IL_0247;
									}
									break;
								case RfcChar.LF:
									if (num4 == 1)
									{
										num4 = 2;
										goto IL_0247;
									}
									break;
								case RfcChar.High:
								case RfcChar.Reg:
								case RfcChar.Colon:
								case RfcChar.Delim:
									if (num4 == 1)
									{
										break;
									}
									if (num4 == 3)
									{
										num4 = 0;
										if (num5 != -1)
										{
											string text = HeaderEncoding.GetString(ptr + num5, num6 - num5 + 1);
											if (stringBuilder == null)
											{
												stringBuilder = new StringBuilder(text, text.Length * 5);
											}
											else
											{
												stringBuilder.Append(" ");
												stringBuilder.Append(text);
											}
										}
										num5 = -1;
									}
									if (num5 == -1)
									{
										num5 = i;
									}
									num6 = i;
									goto IL_0247;
								}
								dataParseStatus = DataParseStatus.Invalid;
								code = WebParseErrorCode.CrLfError;
								break;
							}
							if (i == num)
							{
								dataParseStatus = dataParseStatus2;
								break;
							}
							string text2 = ((num5 == -1) ? "" : HeaderEncoding.GetString(ptr + num5, num6 - num5 + 1));
							if (stringBuilder != null)
							{
								if (text2.Length != 0)
								{
									stringBuilder.Append(" ");
									stringBuilder.Append(text2);
								}
								text2 = stringBuilder.ToString();
							}
							string text3 = null;
							int num7 = num3 - num2 + 1;
							if (m_CommonHeaders != null)
							{
								int num8 = s_CommonHeaderHints[ptr[num2] & 0x1F];
								if (num8 >= 0)
								{
									while (true)
									{
										string text4 = s_CommonHeaderNames[num8++];
										if (text4.Length < num7 || CaseInsensitiveAscii.AsciiToLower[ptr[num2]] != CaseInsensitiveAscii.AsciiToLower[(uint)text4[0]])
										{
											break;
										}
										if (text4.Length > num7)
										{
											continue;
										}
										byte* ptr2 = ptr + num2 + 1;
										int j;
										for (j = 1; j < text4.Length; j++)
										{
											if (*(ptr2++) != text4[j] && CaseInsensitiveAscii.AsciiToLower[*(ptr2 - 1)] != CaseInsensitiveAscii.AsciiToLower[(uint)text4[j]])
											{
												break;
											}
										}
										if (j == text4.Length)
										{
											m_NumCommonHeaders++;
											num8--;
											if (m_CommonHeaders[num8] == null)
											{
												m_CommonHeaders[num8] = text2;
											}
											else
											{
												NormalizeCommonHeaders();
												AddInternalNotCommon(text4, text2);
											}
											text3 = text4;
											break;
										}
									}
								}
							}
							if (text3 == null)
							{
								text3 = HeaderEncoding.GetString(ptr + num2, num7);
								AddInternalNotCommon(text3, text2);
							}
							totalResponseHeadersLength += i - unparsed;
							unparsed = i;
							goto IL_0056;
							IL_0247:
							i++;
						}
						break;
						IL_0126:
						i++;
					}
					break;
				}
			}
		}
		if (dataParseStatus == DataParseStatus.Invalid)
		{
			parseError.Section = WebParseErrorSection.ResponseHeader;
			parseError.Code = code;
		}
		return dataParseStatus;
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
		GetObjectData(serializationInfo, streamingContext);
	}

	public override string Get(string name)
	{
		if (m_CommonHeaders != null && name != null && name.Length > 0 && name[0] < 'Ā')
		{
			int num = s_CommonHeaderHints[name[0] & 0x1F];
			if (num >= 0)
			{
				while (true)
				{
					string text = s_CommonHeaderNames[num++];
					if (text.Length < name.Length || CaseInsensitiveAscii.AsciiToLower[(uint)name[0]] != CaseInsensitiveAscii.AsciiToLower[(uint)text[0]])
					{
						break;
					}
					if (text.Length <= name.Length)
					{
						int i;
						for (i = 1; i < text.Length && (name[i] == text[i] || (name[i] <= 'ÿ' && CaseInsensitiveAscii.AsciiToLower[(uint)name[i]] == CaseInsensitiveAscii.AsciiToLower[(uint)text[i]])); i++)
						{
						}
						if (i == text.Length)
						{
							return m_CommonHeaders[num - 1];
						}
					}
				}
			}
		}
		if (m_InnerCollection == null)
		{
			return null;
		}
		return m_InnerCollection.Get(name);
	}

	public override IEnumerator GetEnumerator()
	{
		NormalizeCommonHeaders();
		return new NameObjectKeysEnumerator(InnerCollection);
	}

	internal override bool InternalHasKeys()
	{
		NormalizeCommonHeaders();
		if (m_InnerCollection == null)
		{
			return false;
		}
		return m_InnerCollection.HasKeys();
	}

	public override string Get(int index)
	{
		NormalizeCommonHeaders();
		return InnerCollection.Get(index);
	}

	public override string[] GetValues(int index)
	{
		NormalizeCommonHeaders();
		return InnerCollection.GetValues(index);
	}

	public override string GetKey(int index)
	{
		NormalizeCommonHeaders();
		return InnerCollection.GetKey(index);
	}

	public override void Clear()
	{
		m_CommonHeaders = null;
		m_NumCommonHeaders = 0;
		InvalidateCachedArrays();
		if (m_InnerCollection != null)
		{
			m_InnerCollection.Clear();
		}
	}
}
