using System.Net;
using System.Net.Security;

namespace System.Data.SqlClient.SNI;

internal class SspiClientContextStatus
{
	public SafeFreeCredentials CredentialsHandle { get; set; }

	public SafeDeleteContext SecurityContext { get; set; }

	public ContextFlagsPal ContextFlags { get; set; }
}
