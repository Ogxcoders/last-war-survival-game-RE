namespace BestHTTP.SignalRCore.Messages;

public struct InvocationMessage
{
	public MessageTypes type;

	public string invocationId;

	public bool nonblocking;

	public string target;

	public object[] arguments;
}
