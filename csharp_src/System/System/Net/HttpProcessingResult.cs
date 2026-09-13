namespace System.Net;

internal enum HttpProcessingResult
{
	Continue,
	ReadWait,
	WriteWait
}
