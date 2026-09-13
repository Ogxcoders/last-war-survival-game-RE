namespace System.Net;

internal enum WebParseErrorCode
{
	Generic,
	InvalidHeaderName,
	InvalidContentLength,
	IncompleteHeaderLine,
	CrLfError,
	InvalidChunkFormat,
	UnexpectedServerResponse
}
