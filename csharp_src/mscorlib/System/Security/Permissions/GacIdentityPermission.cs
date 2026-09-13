using System.Runtime.InteropServices;

namespace System.Security.Permissions;

[Serializable]
[ComVisible(true)]
public sealed class GacIdentityPermission : CodeAccessPermission, IBuiltInPermission
{
	private const int version = 1;

	public GacIdentityPermission()
	{
	}

	public GacIdentityPermission(PermissionState state)
	{
		CodeAccessPermission.CheckPermissionState(state, allowUnrestricted: false);
	}

	public override IPermission Copy()
	{
		return new GacIdentityPermission();
	}

	public override IPermission Intersect(IPermission target)
	{
		if (Cast(target) == null)
		{
			return null;
		}
		return Copy();
	}

	public override bool IsSubsetOf(IPermission target)
	{
		return Cast(target) != null;
	}

	public override IPermission Union(IPermission target)
	{
		Cast(target);
		return Copy();
	}

	public override void FromXml(SecurityElement securityElement)
	{
		CodeAccessPermission.CheckSecurityElement(securityElement, "securityElement", 1, 1);
	}

	public override SecurityElement ToXml()
	{
		return Element(1);
	}

	int IBuiltInPermission.GetTokenIndex()
	{
		return 15;
	}

	private GacIdentityPermission Cast(IPermission target)
	{
		if (target == null)
		{
			return null;
		}
		GacIdentityPermission obj = target as GacIdentityPermission;
		if (obj == null)
		{
			CodeAccessPermission.ThrowInvalidPermission(target, typeof(GacIdentityPermission));
		}
		return obj;
	}
}
