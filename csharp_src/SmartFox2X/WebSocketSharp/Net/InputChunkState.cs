namespace WebSocketSharp.Net;

internal enum InputChunkState
{
	None,
	Body,
	BodyFinished,
	Trailer
}
