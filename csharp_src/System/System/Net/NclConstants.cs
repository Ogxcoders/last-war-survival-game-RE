namespace System.Net;

internal static class NclConstants
{
	internal static readonly object Sentinel = new object();

	internal static readonly object[] EmptyObjectArray = new object[0];

	internal static readonly Uri[] EmptyUriArray = new Uri[0];

	internal static readonly byte[] CRLF = new byte[2] { 13, 10 };

	internal static readonly byte[] ChunkTerminator = new byte[5] { 48, 13, 10, 13, 10 };
}
