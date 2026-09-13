namespace System.Net;

internal enum DataParseStatus
{
	NeedMoreData,
	ContinueParsing,
	Done,
	Invalid,
	DataTooBig
}
