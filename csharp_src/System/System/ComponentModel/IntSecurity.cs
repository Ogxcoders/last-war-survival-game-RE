using System.IO;
using System.Security.Permissions;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
internal static class IntSecurity
{
	public static string UnsafeGetFullPath(string fileName)
	{
		return Path.GetFullPath(fileName);
	}
}
