namespace BestHTTP.SignalR.Messages;

public sealed class DataMessage : IServerMessage
{
	MessageTypes IServerMessage.Type => MessageTypes.Data;

	public object Data { get; private set; }

	void IServerMessage.Parse(object data)
	{
		Data = data;
	}
}
