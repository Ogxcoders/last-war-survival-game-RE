namespace BestHTTP.SignalR.Messages;

public interface IHubMessage
{
	ulong InvocationId { get; }
}
