using System.Collections;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Net;

internal static class UnsafeNclNativeMethods
{
	internal static class HttpApi
	{
		internal static class HTTP_REQUEST_HEADER_ID
		{
			private static string[] m_Strings = new string[41]
			{
				"Cache-Control", "Connection", "Date", "Keep-Alive", "Pragma", "Trailer", "Transfer-Encoding", "Upgrade", "Via", "Warning",
				"Allow", "Content-Length", "Content-Type", "Content-Encoding", "Content-Language", "Content-Location", "Content-MD5", "Content-Range", "Expires", "Last-Modified",
				"Accept", "Accept-Charset", "Accept-Encoding", "Accept-Language", "Authorization", "Cookie", "Expect", "From", "Host", "If-Match",
				"If-Modified-Since", "If-None-Match", "If-Range", "If-Unmodified-Since", "Max-Forwards", "Proxy-Authorization", "Referer", "Range", "Te", "Translate",
				"User-Agent"
			};

			internal static string ToString(int position)
			{
				return m_Strings[position];
			}
		}

		internal static class HTTP_RESPONSE_HEADER_ID
		{
			private static Hashtable m_Hashtable;

			static HTTP_RESPONSE_HEADER_ID()
			{
				m_Hashtable = new Hashtable(30);
				for (int i = 0; i < 30; i++)
				{
					m_Hashtable.Add(m_Strings[i], i);
				}
			}

			internal static int IndexOfKnownHeader(string HeaderName)
			{
				object obj = m_Hashtable[HeaderName];
				if (obj != null)
				{
					return (int)obj;
				}
				return -1;
			}

			internal static string ToString(int position)
			{
				return m_Strings[position];
			}
		}

		internal enum Enum
		{
			HttpHeaderCacheControl = 0,
			HttpHeaderConnection = 1,
			HttpHeaderDate = 2,
			HttpHeaderKeepAlive = 3,
			HttpHeaderPragma = 4,
			HttpHeaderTrailer = 5,
			HttpHeaderTransferEncoding = 6,
			HttpHeaderUpgrade = 7,
			HttpHeaderVia = 8,
			HttpHeaderWarning = 9,
			HttpHeaderAllow = 10,
			HttpHeaderContentLength = 11,
			HttpHeaderContentType = 12,
			HttpHeaderContentEncoding = 13,
			HttpHeaderContentLanguage = 14,
			HttpHeaderContentLocation = 15,
			HttpHeaderContentMd5 = 16,
			HttpHeaderContentRange = 17,
			HttpHeaderExpires = 18,
			HttpHeaderLastModified = 19,
			HttpHeaderAcceptRanges = 20,
			HttpHeaderAge = 21,
			HttpHeaderEtag = 22,
			HttpHeaderLocation = 23,
			HttpHeaderProxyAuthenticate = 24,
			HttpHeaderRetryAfter = 25,
			HttpHeaderServer = 26,
			HttpHeaderSetCookie = 27,
			HttpHeaderVary = 28,
			HttpHeaderWwwAuthenticate = 29,
			HttpHeaderResponseMaximum = 30,
			HttpHeaderMaximum = 41
		}

		private const int HttpHeaderRequestMaximum = 41;

		private const int HttpHeaderResponseMaximum = 30;

		private static string[] m_Strings = new string[30]
		{
			"Cache-Control", "Connection", "Date", "Keep-Alive", "Pragma", "Trailer", "Transfer-Encoding", "Upgrade", "Via", "Warning",
			"Allow", "Content-Length", "Content-Type", "Content-Encoding", "Content-Language", "Content-Location", "Content-MD5", "Content-Range", "Expires", "Last-Modified",
			"Accept-Ranges", "Age", "ETag", "Location", "Proxy-Authenticate", "Retry-After", "Server", "Set-Cookie", "Vary", "WWW-Authenticate"
		};
	}

	internal static class SecureStringHelper
	{
		internal static string CreateString(SecureString secureString)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (secureString == null || secureString.Length == 0)
			{
				return string.Empty;
			}
			try
			{
				intPtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
				return Marshal.PtrToStringUni(intPtr);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.ZeroFreeGlobalAllocUnicode(intPtr);
				}
			}
		}

		internal unsafe static SecureString CreateSecureString(string plainString)
		{
			if (plainString == null || plainString.Length == 0)
			{
				return new SecureString();
			}
			SecureString result;
			fixed (char* value = plainString)
			{
				result = new SecureString(value, plainString.Length);
			}
			return result;
		}
	}
}
