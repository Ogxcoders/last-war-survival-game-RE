using System.Security.Cryptography.X509Certificates;

namespace System.Security.Policy;

[Serializable]
public sealed class Publisher : EvidenceBase, IIdentityPermissionFactory
{
	public X509Certificate Certificate => null;

	public Publisher(X509Certificate cert)
	{
	}

	public object Copy()
	{
		return null;
	}

	public IPermission CreateIdentityPermission(Evidence evidence)
	{
		return null;
	}

	public override bool Equals(object o)
	{
		return base.Equals(o);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return base.ToString();
	}
}
