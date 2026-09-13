using System.Runtime.InteropServices;

namespace System.Security.Permissions;

[Serializable]
[ComVisible(true)]
public sealed class ReflectionPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
{
	private const int version = 1;

	private ReflectionPermissionFlag flags;

	public ReflectionPermissionFlag Flags
	{
		get
		{
			return flags;
		}
		set
		{
			if ((value & (ReflectionPermissionFlag.AllFlags | ReflectionPermissionFlag.RestrictedMemberAccess)) != value)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Invalid flags {0}"), value), "ReflectionPermissionFlag");
			}
			flags = value;
		}
	}

	public ReflectionPermission(PermissionState state)
	{
		if (CodeAccessPermission.CheckPermissionState(state, allowUnrestricted: true) == PermissionState.Unrestricted)
		{
			flags = ReflectionPermissionFlag.AllFlags;
		}
		else
		{
			flags = ReflectionPermissionFlag.NoFlags;
		}
	}

	public ReflectionPermission(ReflectionPermissionFlag flag)
	{
		Flags = flag;
	}

	public override IPermission Copy()
	{
		return new ReflectionPermission(flags);
	}

	public override void FromXml(SecurityElement esd)
	{
		CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
		if (CodeAccessPermission.IsUnrestricted(esd))
		{
			flags = ReflectionPermissionFlag.AllFlags;
			return;
		}
		flags = ReflectionPermissionFlag.NoFlags;
		string obj = esd.Attributes["Flags"] as string;
		if (obj.IndexOf("MemberAccess") >= 0)
		{
			flags |= ReflectionPermissionFlag.MemberAccess;
		}
		if (obj.IndexOf("ReflectionEmit") >= 0)
		{
			flags |= ReflectionPermissionFlag.ReflectionEmit;
		}
		if (obj.IndexOf("TypeInformation") >= 0)
		{
			flags |= ReflectionPermissionFlag.TypeInformation;
		}
	}

	public override IPermission Intersect(IPermission target)
	{
		ReflectionPermission reflectionPermission = Cast(target);
		if (reflectionPermission == null)
		{
			return null;
		}
		if (IsUnrestricted())
		{
			if (reflectionPermission.Flags == ReflectionPermissionFlag.NoFlags)
			{
				return null;
			}
			return reflectionPermission.Copy();
		}
		if (reflectionPermission.IsUnrestricted())
		{
			if (flags == ReflectionPermissionFlag.NoFlags)
			{
				return null;
			}
			return Copy();
		}
		ReflectionPermission reflectionPermission2 = (ReflectionPermission)reflectionPermission.Copy();
		reflectionPermission2.Flags &= flags;
		if (reflectionPermission2.Flags != ReflectionPermissionFlag.NoFlags)
		{
			return reflectionPermission2;
		}
		return null;
	}

	public override bool IsSubsetOf(IPermission target)
	{
		ReflectionPermission reflectionPermission = Cast(target);
		if (reflectionPermission == null)
		{
			return flags == ReflectionPermissionFlag.NoFlags;
		}
		if (IsUnrestricted())
		{
			return reflectionPermission.IsUnrestricted();
		}
		if (reflectionPermission.IsUnrestricted())
		{
			return true;
		}
		return (flags & reflectionPermission.Flags) == flags;
	}

	public bool IsUnrestricted()
	{
		return flags == ReflectionPermissionFlag.AllFlags;
	}

	public override SecurityElement ToXml()
	{
		SecurityElement securityElement = Element(1);
		if (IsUnrestricted())
		{
			securityElement.AddAttribute("Unrestricted", "true");
		}
		else if (flags == ReflectionPermissionFlag.NoFlags)
		{
			securityElement.AddAttribute("Flags", "NoFlags");
		}
		else if ((flags & ReflectionPermissionFlag.AllFlags) == ReflectionPermissionFlag.AllFlags)
		{
			securityElement.AddAttribute("Flags", "AllFlags");
		}
		else
		{
			string text = "";
			if ((flags & ReflectionPermissionFlag.MemberAccess) == ReflectionPermissionFlag.MemberAccess)
			{
				text = "MemberAccess";
			}
			if ((flags & ReflectionPermissionFlag.ReflectionEmit) == ReflectionPermissionFlag.ReflectionEmit)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "ReflectionEmit";
			}
			if ((flags & ReflectionPermissionFlag.TypeInformation) == ReflectionPermissionFlag.TypeInformation)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "TypeInformation";
			}
			securityElement.AddAttribute("Flags", text);
		}
		return securityElement;
	}

	public override IPermission Union(IPermission other)
	{
		ReflectionPermission reflectionPermission = Cast(other);
		if (other == null)
		{
			return Copy();
		}
		if (IsUnrestricted() || reflectionPermission.IsUnrestricted())
		{
			return new ReflectionPermission(PermissionState.Unrestricted);
		}
		ReflectionPermission obj = (ReflectionPermission)reflectionPermission.Copy();
		obj.Flags |= flags;
		return obj;
	}

	int IBuiltInPermission.GetTokenIndex()
	{
		return 4;
	}

	private ReflectionPermission Cast(IPermission target)
	{
		if (target == null)
		{
			return null;
		}
		ReflectionPermission obj = target as ReflectionPermission;
		if (obj == null)
		{
			CodeAccessPermission.ThrowInvalidPermission(target, typeof(ReflectionPermission));
		}
		return obj;
	}
}
