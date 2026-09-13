namespace System.Net;

internal enum HttpWriteMode
{
	Unknown,
	ContentLength,
	Chunked,
	Buffer,
	None
}
