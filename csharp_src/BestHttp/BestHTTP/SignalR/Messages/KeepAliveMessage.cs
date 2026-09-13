namespace BestHTTP.SignalR.Messages;

public sealed class KeepAliveMessage : IServerMessage
{
	MessageTypes IServerMessage.Type => MessageTypes.KeepAlive;

	void IServerMessage.Parse(object data)
	{
	}
}
