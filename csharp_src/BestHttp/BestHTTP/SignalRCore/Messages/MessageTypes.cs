namespace BestHTTP.SignalRCore.Messages;

public enum MessageTypes
{
	Handshake,
	Invocation,
	StreamItem,
	Completion,
	StreamInvocation,
	CancelInvocation,
	Ping,
	Close
}
