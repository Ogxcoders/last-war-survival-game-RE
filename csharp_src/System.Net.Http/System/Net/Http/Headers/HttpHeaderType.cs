namespace System.Net.Http.Headers;

[Flags]
internal enum HttpHeaderType : byte
{
	General = 1,
	Request = 2,
	Response = 4,
	Content = 8,
	Custom = 0x10,
	All = 0x1F,
	None = 0
}
