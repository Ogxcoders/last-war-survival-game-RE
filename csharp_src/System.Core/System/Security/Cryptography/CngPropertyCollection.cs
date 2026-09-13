using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace System.Security.Cryptography;

[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public sealed class CngPropertyCollection : Collection<CngProperty>
{
}
