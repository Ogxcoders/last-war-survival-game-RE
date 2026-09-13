namespace BestHTTP.SignalR.Authentication;

public interface IAuthenticationProvider
{
	bool IsPreAuthRequired { get; }

	event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

	event OnAuthenticationFailedDelegate OnAuthenticationFailed;

	void StartAuthentication();

	void PrepareRequest(HTTPRequest request, RequestTypes type);
}
