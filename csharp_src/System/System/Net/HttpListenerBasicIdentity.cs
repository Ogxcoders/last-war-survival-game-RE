using System.Security.Principal;

namespace System.Net;

public class HttpListenerBasicIdentity : GenericIdentity
{
	private string password;

	public virtual string Password => password;

	public HttpListenerBasicIdentity(string username, string password)
		: base(username, "Basic")
	{
		this.password = password;
	}
}
