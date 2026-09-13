using System.Security.Cryptography.X509Certificates;

namespace System.Security.Policy;

[Serializable]
public sealed class PublisherMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
{
	public X509Certificate Certificate { get; set; }

	public PublisherMembershipCondition(X509Certificate certificate)
	{
	}

	public bool Check(Evidence evidence)
	{
		return false;
	}

	public IMembershipCondition Copy()
	{
		return this;
	}

	public override bool Equals(object o)
	{
		return base.Equals(o);
	}

	public void FromXml(SecurityElement e)
	{
	}

	public void FromXml(SecurityElement e, PolicyLevel level)
	{
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return base.ToString();
	}

	public SecurityElement ToXml()
	{
		return null;
	}

	public SecurityElement ToXml(PolicyLevel level)
	{
		return null;
	}
}
