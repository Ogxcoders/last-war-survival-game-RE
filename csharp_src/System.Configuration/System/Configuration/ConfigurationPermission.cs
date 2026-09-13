using System.Security;
using System.Security.Permissions;
using Unity;

namespace System.Configuration;

[Serializable]
public sealed class ConfigurationPermission : CodeAccessPermission, IUnrestrictedPermission
{
	public ConfigurationPermission(PermissionState state)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public override IPermission Copy()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public override void FromXml(SecurityElement securityElement)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public override IPermission Intersect(IPermission target)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public override bool IsSubsetOf(IPermission target)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public bool IsUnrestricted()
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public override SecurityElement ToXml()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
