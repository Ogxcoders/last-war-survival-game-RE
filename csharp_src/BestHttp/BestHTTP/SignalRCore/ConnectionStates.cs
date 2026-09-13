namespace BestHTTP.SignalRCore;

public enum ConnectionStates
{
	Initial,
	Authenticating,
	Negotiating,
	Redirected,
	Connected,
	CloseInitiated,
	Closed
}
