using System.Globalization;

namespace System.Security.Permissions;

[Serializable]
public sealed class TypeDescriptorPermission : CodeAccessPermission, IUnrestrictedPermission
{
	private TypeDescriptorPermissionFlags m_flags;

	public TypeDescriptorPermissionFlags Flags
	{
		get
		{
			return m_flags;
		}
		set
		{
			VerifyAccess(value);
			m_flags = value;
		}
	}

	public TypeDescriptorPermission(PermissionState state)
	{
		switch (state)
		{
		case PermissionState.Unrestricted:
			SetUnrestricted(unrestricted: true);
			break;
		case PermissionState.None:
			SetUnrestricted(unrestricted: false);
			break;
		default:
			throw new ArgumentException(global::SR.GetString("Invalid permission state."));
		}
	}

	public TypeDescriptorPermission(TypeDescriptorPermissionFlags flag)
	{
		VerifyAccess(flag);
		SetUnrestricted(unrestricted: false);
		m_flags = flag;
	}

	private void SetUnrestricted(bool unrestricted)
	{
		if (unrestricted)
		{
			m_flags = TypeDescriptorPermissionFlags.RestrictedRegistrationAccess;
		}
		else
		{
			Reset();
		}
	}

	private void Reset()
	{
		m_flags = TypeDescriptorPermissionFlags.NoFlags;
	}

	public bool IsUnrestricted()
	{
		return m_flags == TypeDescriptorPermissionFlags.RestrictedRegistrationAccess;
	}

	public override IPermission Union(IPermission target)
	{
		if (target == null)
		{
			return Copy();
		}
		try
		{
			TypeDescriptorPermission typeDescriptorPermission = (TypeDescriptorPermission)target;
			TypeDescriptorPermissionFlags typeDescriptorPermissionFlags = m_flags | typeDescriptorPermission.m_flags;
			if (typeDescriptorPermissionFlags == TypeDescriptorPermissionFlags.NoFlags)
			{
				return null;
			}
			return new TypeDescriptorPermission(typeDescriptorPermissionFlags);
		}
		catch (InvalidCastException)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, global::SR.GetString("Operation on type '{0}' attempted with target of incorrect type."), GetType().FullName));
		}
	}

	public override bool IsSubsetOf(IPermission target)
	{
		if (target == null)
		{
			return m_flags == TypeDescriptorPermissionFlags.NoFlags;
		}
		try
		{
			TypeDescriptorPermission obj = (TypeDescriptorPermission)target;
			TypeDescriptorPermissionFlags flags = m_flags;
			TypeDescriptorPermissionFlags flags2 = obj.m_flags;
			return (flags & flags2) == flags;
		}
		catch (InvalidCastException)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, global::SR.GetString("Operation on type '{0}' attempted with target of incorrect type."), GetType().FullName));
		}
	}

	public override IPermission Intersect(IPermission target)
	{
		if (target == null)
		{
			return null;
		}
		try
		{
			TypeDescriptorPermissionFlags typeDescriptorPermissionFlags = ((TypeDescriptorPermission)target).m_flags & m_flags;
			if (typeDescriptorPermissionFlags == TypeDescriptorPermissionFlags.NoFlags)
			{
				return null;
			}
			return new TypeDescriptorPermission(typeDescriptorPermissionFlags);
		}
		catch (InvalidCastException)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, global::SR.GetString("Operation on type '{0}' attempted with target of incorrect type."), GetType().FullName));
		}
	}

	public override IPermission Copy()
	{
		return new TypeDescriptorPermission(m_flags);
	}

	private void VerifyAccess(TypeDescriptorPermissionFlags type)
	{
		if ((type & ~TypeDescriptorPermissionFlags.RestrictedRegistrationAccess) != TypeDescriptorPermissionFlags.NoFlags)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, global::SR.GetString("Illegal enum value: {0}."), (int)type));
		}
	}

	public override SecurityElement ToXml()
	{
		SecurityElement securityElement = new SecurityElement("IPermission");
		securityElement.AddAttribute("class", GetType().FullName + ", " + GetType().Module.Assembly.FullName.Replace('"', '\''));
		securityElement.AddAttribute("version", "1");
		if (!IsUnrestricted())
		{
			securityElement.AddAttribute("Flags", m_flags.ToString());
		}
		else
		{
			securityElement.AddAttribute("Unrestricted", "true");
		}
		return securityElement;
	}

	public override void FromXml(SecurityElement securityElement)
	{
		if (securityElement == null)
		{
			throw new ArgumentNullException("securityElement");
		}
		string text = securityElement.Attribute("class");
		if (text == null || text.IndexOf(GetType().FullName, StringComparison.Ordinal) == -1)
		{
			throw new ArgumentException(global::SR.GetString("The value of \"class\" attribute is invalid."), "securityElement");
		}
		string text2 = securityElement.Attribute("Unrestricted");
		if (text2 != null && string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0)
		{
			m_flags = TypeDescriptorPermissionFlags.RestrictedRegistrationAccess;
			return;
		}
		m_flags = TypeDescriptorPermissionFlags.NoFlags;
		string text3 = securityElement.Attribute("Flags");
		if (text3 != null)
		{
			TypeDescriptorPermissionFlags flags = (TypeDescriptorPermissionFlags)Enum.Parse(typeof(TypeDescriptorPermissionFlags), text3);
			VerifyFlags(flags);
			m_flags = flags;
		}
	}

	internal static void VerifyFlags(TypeDescriptorPermissionFlags flags)
	{
		if ((flags & ~TypeDescriptorPermissionFlags.RestrictedRegistrationAccess) != TypeDescriptorPermissionFlags.NoFlags)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, global::SR.GetString("Illegal enum value: {0}."), (int)flags));
		}
	}
}
