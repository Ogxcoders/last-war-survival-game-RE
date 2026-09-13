using System.Security.Permissions;

namespace System.Security.Policy;

[Serializable]
[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
public abstract class EvidenceBase
{
	[SecurityPermission(SecurityAction.Assert, SerializationFormatter = true)]
	public virtual EvidenceBase Clone()
	{
		throw new NotImplementedException();
	}
}
