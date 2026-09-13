using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Hubs;

public interface IHub
{
	Connection Connection { get; set; }

	bool Call(ClientMessage msg);

	bool HasSentMessageId(ulong id);

	void Close();

	void OnMethod(MethodCallMessage msg);

	void OnMessage(IServerMessage msg);
}
