using System;

namespace BestHTTP.SignalRCore;

public interface IAuthenticationProvider
{
	bool IsPreAuthRequired { get; }

	event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

	event OnAuthenticationFailedDelegate OnAuthenticationFailed;

	void StartAuthentication();

	void PrepareRequest(HTTPRequest request);

	Uri PrepareUri(Uri uri);
}
