namespace System.Net;

internal enum WriteBufferState
{
	Disabled,
	Headers,
	Buffer,
	Playback
}
