namespace System.Xml;

internal enum MimeWriterState
{
	Start,
	StartPreface,
	StartPart,
	Header,
	Content,
	Closed
}
