namespace BestHTTP.SignalR;

public enum ConnectionStates
{
	Initial,
	Authenticating,
	Negotiating,
	Connecting,
	Connected,
	Reconnecting,
	Closed
}
