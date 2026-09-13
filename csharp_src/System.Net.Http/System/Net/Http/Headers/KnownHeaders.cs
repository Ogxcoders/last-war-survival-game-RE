using System.Runtime.InteropServices;

namespace System.Net.Http.Headers;

internal static class KnownHeaders
{
	private interface IHeaderNameAccessor
	{
		int Length { get; }

		char this[int index] { get; }
	}

	private readonly struct StringAccessor : IHeaderNameAccessor
	{
		private readonly string _string;

		public int Length => _string.Length;

		public char this[int index] => _string[index];

		public StringAccessor(string s)
		{
			_string = s;
		}
	}

	private readonly struct BytePtrAccessor : IHeaderNameAccessor
	{
		private unsafe readonly byte* _p;

		private readonly int _length;

		public int Length => _length;

		public unsafe char this[int index] => (char)_p[index];

		public unsafe BytePtrAccessor(byte* p, int length)
		{
			_p = p;
			_length = length;
		}
	}

	public static readonly KnownHeader Accept = new KnownHeader("Accept", HttpHeaderType.Request, MediaTypeHeaderParser.MultipleValuesParser);

	public static readonly KnownHeader AcceptCharset = new KnownHeader("Accept-Charset", HttpHeaderType.Request, GenericHeaderParser.MultipleValueStringWithQualityParser);

	public static readonly KnownHeader AcceptEncoding = new KnownHeader("Accept-Encoding", HttpHeaderType.Request, GenericHeaderParser.MultipleValueStringWithQualityParser);

	public static readonly KnownHeader AcceptLanguage = new KnownHeader("Accept-Language", HttpHeaderType.Request, GenericHeaderParser.MultipleValueStringWithQualityParser);

	public static readonly KnownHeader AcceptPatch = new KnownHeader("Accept-Patch");

	public static readonly KnownHeader AcceptRanges = new KnownHeader("Accept-Ranges", HttpHeaderType.Response, GenericHeaderParser.TokenListParser);

	public static readonly KnownHeader AccessControlAllowCredentials = new KnownHeader("Access-Control-Allow-Credentials");

	public static readonly KnownHeader AccessControlAllowHeaders = new KnownHeader("Access-Control-Allow-Headers");

	public static readonly KnownHeader AccessControlAllowMethods = new KnownHeader("Access-Control-Allow-Methods");

	public static readonly KnownHeader AccessControlAllowOrigin = new KnownHeader("Access-Control-Allow-Origin");

	public static readonly KnownHeader AccessControlExposeHeaders = new KnownHeader("Access-Control-Expose-Headers");

	public static readonly KnownHeader AccessControlMaxAge = new KnownHeader("Access-Control-Max-Age");

	public static readonly KnownHeader Age = new KnownHeader("Age", HttpHeaderType.Response, TimeSpanHeaderParser.Parser);

	public static readonly KnownHeader Allow = new KnownHeader("Allow", HttpHeaderType.Content, GenericHeaderParser.TokenListParser);

	public static readonly KnownHeader AltSvc = new KnownHeader("Alt-Svc");

	public static readonly KnownHeader Authorization = new KnownHeader("Authorization", HttpHeaderType.Request, GenericHeaderParser.SingleValueAuthenticationParser);

	public static readonly KnownHeader CacheControl = new KnownHeader("Cache-Control", HttpHeaderType.General, CacheControlHeaderParser.Parser);

	public static readonly KnownHeader Connection = new KnownHeader("Connection", HttpHeaderType.General, GenericHeaderParser.TokenListParser, new string[1] { "close" });

	public static readonly KnownHeader ContentDisposition = new KnownHeader("Content-Disposition", HttpHeaderType.Content, GenericHeaderParser.ContentDispositionParser);

	public static readonly KnownHeader ContentEncoding = new KnownHeader("Content-Encoding", HttpHeaderType.Content, GenericHeaderParser.TokenListParser, new string[2] { "gzip", "deflate" });

	public static readonly KnownHeader ContentLanguage = new KnownHeader("Content-Language", HttpHeaderType.Content, GenericHeaderParser.TokenListParser);

	public static readonly KnownHeader ContentLength = new KnownHeader("Content-Length", HttpHeaderType.Content, Int64NumberHeaderParser.Parser);

	public static readonly KnownHeader ContentLocation = new KnownHeader("Content-Location", HttpHeaderType.Content, UriHeaderParser.RelativeOrAbsoluteUriParser);

	public static readonly KnownHeader ContentMD5 = new KnownHeader("Content-MD5", HttpHeaderType.Content, ByteArrayHeaderParser.Parser);

	public static readonly KnownHeader ContentRange = new KnownHeader("Content-Range", HttpHeaderType.Content, GenericHeaderParser.ContentRangeParser);

	public static readonly KnownHeader ContentSecurityPolicy = new KnownHeader("Content-Security-Policy");

	public static readonly KnownHeader ContentType = new KnownHeader("Content-Type", HttpHeaderType.Content, MediaTypeHeaderParser.SingleValueParser);

	public static readonly KnownHeader Cookie = new KnownHeader("Cookie");

	public static readonly KnownHeader Cookie2 = new KnownHeader("Cookie2");

	public static readonly KnownHeader Date = new KnownHeader("Date", HttpHeaderType.General, DateHeaderParser.Parser);

	public static readonly KnownHeader ETag = new KnownHeader("ETag", HttpHeaderType.Response, GenericHeaderParser.SingleValueEntityTagParser);

	public static readonly KnownHeader Expect = new KnownHeader("Expect", HttpHeaderType.Request, GenericHeaderParser.MultipleValueNameValueWithParametersParser, new string[1] { "100-continue" });

	public static readonly KnownHeader Expires = new KnownHeader("Expires", HttpHeaderType.Content, DateHeaderParser.Parser);

	public static readonly KnownHeader From = new KnownHeader("From", HttpHeaderType.Request, GenericHeaderParser.MailAddressParser);

	public static readonly KnownHeader Host = new KnownHeader("Host", HttpHeaderType.Request, GenericHeaderParser.HostParser);

	public static readonly KnownHeader IfMatch = new KnownHeader("If-Match", HttpHeaderType.Request, GenericHeaderParser.MultipleValueEntityTagParser);

	public static readonly KnownHeader IfModifiedSince = new KnownHeader("If-Modified-Since", HttpHeaderType.Request, DateHeaderParser.Parser);

	public static readonly KnownHeader IfNoneMatch = new KnownHeader("If-None-Match", HttpHeaderType.Request, GenericHeaderParser.MultipleValueEntityTagParser);

	public static readonly KnownHeader IfRange = new KnownHeader("If-Range", HttpHeaderType.Request, GenericHeaderParser.RangeConditionParser);

	public static readonly KnownHeader IfUnmodifiedSince = new KnownHeader("If-Unmodified-Since", HttpHeaderType.Request, DateHeaderParser.Parser);

	public static readonly KnownHeader KeepAlive = new KnownHeader("Keep-Alive");

	public static readonly KnownHeader LastModified = new KnownHeader("Last-Modified", HttpHeaderType.Content, DateHeaderParser.Parser);

	public static readonly KnownHeader Link = new KnownHeader("Link");

	public static readonly KnownHeader Location = new KnownHeader("Location", HttpHeaderType.Response, UriHeaderParser.RelativeOrAbsoluteUriParser);

	public static readonly KnownHeader MaxForwards = new KnownHeader("Max-Forwards", HttpHeaderType.Request, Int32NumberHeaderParser.Parser);

	public static readonly KnownHeader Origin = new KnownHeader("Origin");

	public static readonly KnownHeader P3P = new KnownHeader("P3P");

	public static readonly KnownHeader Pragma = new KnownHeader("Pragma", HttpHeaderType.General, GenericHeaderParser.MultipleValueNameValueParser);

	public static readonly KnownHeader ProxyAuthenticate = new KnownHeader("Proxy-Authenticate", HttpHeaderType.Response, GenericHeaderParser.MultipleValueAuthenticationParser);

	public static readonly KnownHeader ProxyAuthorization = new KnownHeader("Proxy-Authorization", HttpHeaderType.Request, GenericHeaderParser.SingleValueAuthenticationParser);

	public static readonly KnownHeader ProxyConnection = new KnownHeader("Proxy-Connection");

	public static readonly KnownHeader ProxySupport = new KnownHeader("Proxy-Support");

	public static readonly KnownHeader PublicKeyPins = new KnownHeader("Public-Key-Pins");

	public static readonly KnownHeader Range = new KnownHeader("Range", HttpHeaderType.Request, GenericHeaderParser.RangeParser);

	public static readonly KnownHeader Referer = new KnownHeader("Referer", HttpHeaderType.Request, UriHeaderParser.RelativeOrAbsoluteUriParser);

	public static readonly KnownHeader RetryAfter = new KnownHeader("Retry-After", HttpHeaderType.Response, GenericHeaderParser.RetryConditionParser);

	public static readonly KnownHeader SecWebSocketAccept = new KnownHeader("Sec-WebSocket-Accept");

	public static readonly KnownHeader SecWebSocketExtensions = new KnownHeader("Sec-WebSocket-Extensions");

	public static readonly KnownHeader SecWebSocketKey = new KnownHeader("Sec-WebSocket-Key");

	public static readonly KnownHeader SecWebSocketProtocol = new KnownHeader("Sec-WebSocket-Protocol");

	public static readonly KnownHeader SecWebSocketVersion = new KnownHeader("Sec-WebSocket-Version");

	public static readonly KnownHeader Server = new KnownHeader("Server", HttpHeaderType.Response, ProductInfoHeaderParser.MultipleValueParser);

	public static readonly KnownHeader SetCookie = new KnownHeader("Set-Cookie");

	public static readonly KnownHeader SetCookie2 = new KnownHeader("Set-Cookie2");

	public static readonly KnownHeader StrictTransportSecurity = new KnownHeader("Strict-Transport-Security");

	public static readonly KnownHeader TE = new KnownHeader("TE", HttpHeaderType.Request, TransferCodingHeaderParser.MultipleValueWithQualityParser);

	public static readonly KnownHeader TSV = new KnownHeader("TSV");

	public static readonly KnownHeader Trailer = new KnownHeader("Trailer", HttpHeaderType.General, GenericHeaderParser.TokenListParser);

	public static readonly KnownHeader TransferEncoding = new KnownHeader("Transfer-Encoding", HttpHeaderType.General, TransferCodingHeaderParser.MultipleValueParser, new string[1] { "chunked" });

	public static readonly KnownHeader Upgrade = new KnownHeader("Upgrade", HttpHeaderType.General, GenericHeaderParser.MultipleValueProductParser);

	public static readonly KnownHeader UpgradeInsecureRequests = new KnownHeader("Upgrade-Insecure-Requests");

	public static readonly KnownHeader UserAgent = new KnownHeader("User-Agent", HttpHeaderType.Request, ProductInfoHeaderParser.MultipleValueParser);

	public static readonly KnownHeader Vary = new KnownHeader("Vary", HttpHeaderType.Response, GenericHeaderParser.TokenListParser);

	public static readonly KnownHeader Via = new KnownHeader("Via", HttpHeaderType.General, GenericHeaderParser.MultipleValueViaParser);

	public static readonly KnownHeader WWWAuthenticate = new KnownHeader("WWW-Authenticate", HttpHeaderType.Response, GenericHeaderParser.MultipleValueAuthenticationParser);

	public static readonly KnownHeader Warning = new KnownHeader("Warning", HttpHeaderType.General, GenericHeaderParser.MultipleValueWarningParser);

	public static readonly KnownHeader XAspNetVersion = new KnownHeader("X-AspNet-Version");

	public static readonly KnownHeader XContentDuration = new KnownHeader("X-Content-Duration");

	public static readonly KnownHeader XContentTypeOptions = new KnownHeader("X-Content-Type-Options");

	public static readonly KnownHeader XFrameOptions = new KnownHeader("X-Frame-Options");

	public static readonly KnownHeader XMSEdgeRef = new KnownHeader("X-MSEdge-Ref");

	public static readonly KnownHeader XPoweredBy = new KnownHeader("X-Powered-By");

	public static readonly KnownHeader XRequestID = new KnownHeader("X-Request-ID");

	public static readonly KnownHeader XUACompatible = new KnownHeader("X-UA-Compatible");

	private static KnownHeader GetCandidate<T>(T key) where T : struct, IHeaderNameAccessor
	{
		switch (key.Length)
		{
		case 2:
			return TE;
		case 3:
			switch (key[0])
			{
			case 'A':
			case 'a':
				return Age;
			case 'P':
			case 'p':
				return P3P;
			case 'T':
			case 't':
				return TSV;
			case 'V':
			case 'v':
				return Via;
			}
			break;
		case 4:
			switch (key[0])
			{
			case 'D':
			case 'd':
				return Date;
			case 'E':
			case 'e':
				return ETag;
			case 'F':
			case 'f':
				return From;
			case 'H':
			case 'h':
				return Host;
			case 'L':
			case 'l':
				return Link;
			case 'V':
			case 'v':
				return Vary;
			}
			break;
		case 5:
			switch (key[0])
			{
			case 'A':
			case 'a':
				return Allow;
			case 'R':
			case 'r':
				return Range;
			}
			break;
		case 6:
			switch (key[0])
			{
			case 'A':
			case 'a':
				return Accept;
			case 'C':
			case 'c':
				return Cookie;
			case 'E':
			case 'e':
				return Expect;
			case 'O':
			case 'o':
				return Origin;
			case 'P':
			case 'p':
				return Pragma;
			case 'S':
			case 's':
				return Server;
			}
			break;
		case 7:
			switch (key[0])
			{
			case 'A':
			case 'a':
				return AltSvc;
			case 'C':
			case 'c':
				return Cookie2;
			case 'E':
			case 'e':
				return Expires;
			case 'R':
			case 'r':
				return Referer;
			case 'T':
			case 't':
				return Trailer;
			case 'U':
			case 'u':
				return Upgrade;
			case 'W':
			case 'w':
				return Warning;
			}
			break;
		case 8:
			switch (key[3])
			{
			case 'M':
			case 'm':
				return IfMatch;
			case 'R':
			case 'r':
				return IfRange;
			case 'A':
			case 'a':
				return Location;
			}
			break;
		case 10:
			switch (key[0])
			{
			case 'C':
			case 'c':
				return Connection;
			case 'K':
			case 'k':
				return KeepAlive;
			case 'S':
			case 's':
				return SetCookie;
			case 'U':
			case 'u':
				return UserAgent;
			}
			break;
		case 11:
			switch (key[0])
			{
			case 'C':
			case 'c':
				return ContentMD5;
			case 'R':
			case 'r':
				return RetryAfter;
			case 'S':
			case 's':
				return SetCookie2;
			}
			break;
		case 12:
			switch (key[2])
			{
			case 'C':
			case 'c':
				return AcceptPatch;
			case 'N':
			case 'n':
				return ContentType;
			case 'X':
			case 'x':
				return MaxForwards;
			case 'M':
			case 'm':
				return XMSEdgeRef;
			case 'P':
			case 'p':
				return XPoweredBy;
			case 'R':
			case 'r':
				return XRequestID;
			}
			break;
		case 13:
			switch (key[6])
			{
			case '-':
				return AcceptRanges;
			case 'I':
			case 'i':
				return Authorization;
			case 'C':
			case 'c':
				return CacheControl;
			case 'T':
			case 't':
				return ContentRange;
			case 'E':
			case 'e':
				return IfNoneMatch;
			case 'O':
			case 'o':
				return LastModified;
			case 'S':
			case 's':
				return ProxySupport;
			}
			break;
		case 14:
			switch (key[0])
			{
			case 'A':
			case 'a':
				return AcceptCharset;
			case 'C':
			case 'c':
				return ContentLength;
			}
			break;
		case 15:
			switch (key[7])
			{
			case '-':
				return XFrameOptions;
			case 'M':
			case 'm':
				return XUACompatible;
			case 'E':
			case 'e':
				return AcceptEncoding;
			case 'K':
			case 'k':
				return PublicKeyPins;
			case 'L':
			case 'l':
				return AcceptLanguage;
			}
			break;
		case 16:
			switch (key[11])
			{
			case 'O':
			case 'o':
				return ContentEncoding;
			case 'G':
			case 'g':
				return ContentLanguage;
			case 'A':
			case 'a':
				return ContentLocation;
			case 'C':
			case 'c':
				return ProxyConnection;
			case 'I':
			case 'i':
				return WWWAuthenticate;
			case 'R':
			case 'r':
				return XAspNetVersion;
			}
			break;
		case 17:
			switch (key[0])
			{
			case 'I':
			case 'i':
				return IfModifiedSince;
			case 'S':
			case 's':
				return SecWebSocketKey;
			case 'T':
			case 't':
				return TransferEncoding;
			}
			break;
		case 18:
			switch (key[0])
			{
			case 'P':
			case 'p':
				return ProxyAuthenticate;
			case 'X':
			case 'x':
				return XContentDuration;
			}
			break;
		case 19:
			switch (key[0])
			{
			case 'C':
			case 'c':
				return ContentDisposition;
			case 'I':
			case 'i':
				return IfUnmodifiedSince;
			case 'P':
			case 'p':
				return ProxyAuthorization;
			}
			break;
		case 20:
			return SecWebSocketAccept;
		case 21:
			return SecWebSocketVersion;
		case 22:
			switch (key[0])
			{
			case 'A':
			case 'a':
				return AccessControlMaxAge;
			case 'S':
			case 's':
				return SecWebSocketProtocol;
			case 'X':
			case 'x':
				return XContentTypeOptions;
			}
			break;
		case 23:
			return ContentSecurityPolicy;
		case 24:
			return SecWebSocketExtensions;
		case 25:
			switch (key[0])
			{
			case 'S':
			case 's':
				return StrictTransportSecurity;
			case 'U':
			case 'u':
				return UpgradeInsecureRequests;
			}
			break;
		case 27:
			return AccessControlAllowOrigin;
		case 28:
			switch (key[21])
			{
			case 'H':
			case 'h':
				return AccessControlAllowHeaders;
			case 'M':
			case 'm':
				return AccessControlAllowMethods;
			}
			break;
		case 29:
			return AccessControlExposeHeaders;
		case 32:
			return AccessControlAllowCredentials;
		}
		return null;
	}

	internal static KnownHeader TryGetKnownHeader(string name)
	{
		KnownHeader candidate = GetCandidate(new StringAccessor(name));
		if (candidate != null && StringComparer.OrdinalIgnoreCase.Equals(name, candidate.Name))
		{
			return candidate;
		}
		return null;
	}

	internal unsafe static KnownHeader TryGetKnownHeader(ReadOnlySpan<byte> name)
	{
		fixed (byte* reference = &MemoryMarshal.GetReference(name))
		{
			KnownHeader candidate = GetCandidate(new BytePtrAccessor(reference, name.Length));
			if (candidate != null && ByteArrayHelpers.EqualsOrdinalAsciiIgnoreCase(candidate.Name, name))
			{
				return candidate;
			}
		}
		return null;
	}
}
