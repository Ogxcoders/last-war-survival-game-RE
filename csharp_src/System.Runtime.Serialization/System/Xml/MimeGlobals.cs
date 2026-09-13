namespace System.Xml;

internal static class MimeGlobals
{
	internal static string MimeVersionHeader = "MIME-Version";

	internal static string DefaultVersion = "1.0";

	internal static string ContentIDScheme = "cid:";

	internal static string ContentIDHeader = "Content-ID";

	internal static string ContentTypeHeader = "Content-Type";

	internal static string ContentTransferEncodingHeader = "Content-Transfer-Encoding";

	internal static string EncodingBinary = "binary";

	internal static string Encoding8bit = "8bit";

	internal static byte[] COLONSPACE = new byte[2] { 58, 32 };

	internal static byte[] DASHDASH = new byte[2] { 45, 45 };

	internal static byte[] CRLF = new byte[2] { 13, 10 };

	internal static byte[] BoundaryPrefix = new byte[4] { 13, 10, 45, 45 };
}
