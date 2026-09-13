namespace BestHTTP.WebSocket;

public enum WebSocketStates : byte
{
	Connecting,
	Open,
	Closing,
	Closed,
	Unknown
}
