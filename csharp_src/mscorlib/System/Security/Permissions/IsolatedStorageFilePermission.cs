using System.Runtime.InteropServices;

namespace System.Security.Permissions;

[Serializable]
[ComVisible(true)]
public sealed class IsolatedStorageFilePermission : IsolatedStoragePermission, IBuiltInPermission
{
	public IsolatedStorageFilePermission(PermissionState state)
		: base(state)
	{
	}

	public override IPermission Copy()
	{
		return new IsolatedStorageFilePermission(PermissionState.None)
		{
			m_userQuota = m_userQuota,
			m_machineQuota = m_machineQuota,
			m_expirationDays = m_expirationDays,
			m_permanentData = m_permanentData,
			m_allowed = m_allowed
		};
	}

	public override IPermission Intersect(IPermission target)
	{
		IsolatedStorageFilePermission isolatedStorageFilePermission = Cast(target);
		if (isolatedStorageFilePermission == null)
		{
			return null;
		}
		if (IsEmpty() && isolatedStorageFilePermission.IsEmpty())
		{
			return null;
		}
		return new IsolatedStorageFilePermission(PermissionState.None)
		{
			m_userQuota = ((m_userQuota < isolatedStorageFilePermission.m_userQuota) ? m_userQuota : isolatedStorageFilePermission.m_userQuota),
			m_machineQuota = ((m_machineQuota < isolatedStorageFilePermission.m_machineQuota) ? m_machineQuota : isolatedStorageFilePermission.m_machineQuota),
			m_expirationDays = ((m_expirationDays < isolatedStorageFilePermission.m_expirationDays) ? m_expirationDays : isolatedStorageFilePermission.m_expirationDays),
			m_permanentData = (m_permanentData && isolatedStorageFilePermission.m_permanentData),
			UsageAllowed = ((m_allowed < isolatedStorageFilePermission.m_allowed) ? m_allowed : isolatedStorageFilePermission.m_allowed)
		};
	}

	public override bool IsSubsetOf(IPermission target)
	{
		IsolatedStorageFilePermission isolatedStorageFilePermission = Cast(target);
		if (isolatedStorageFilePermission == null)
		{
			return IsEmpty();
		}
		if (isolatedStorageFilePermission.IsUnrestricted())
		{
			return true;
		}
		if (m_userQuota > isolatedStorageFilePermission.m_userQuota)
		{
			return false;
		}
		if (m_machineQuota > isolatedStorageFilePermission.m_machineQuota)
		{
			return false;
		}
		if (m_expirationDays > isolatedStorageFilePermission.m_expirationDays)
		{
			return false;
		}
		if (m_permanentData != isolatedStorageFilePermission.m_permanentData)
		{
			return false;
		}
		if (m_allowed > isolatedStorageFilePermission.m_allowed)
		{
			return false;
		}
		return true;
	}

	public override IPermission Union(IPermission target)
	{
		IsolatedStorageFilePermission isolatedStorageFilePermission = Cast(target);
		if (isolatedStorageFilePermission == null)
		{
			return Copy();
		}
		return new IsolatedStorageFilePermission(PermissionState.None)
		{
			m_userQuota = ((m_userQuota > isolatedStorageFilePermission.m_userQuota) ? m_userQuota : isolatedStorageFilePermission.m_userQuota),
			m_machineQuota = ((m_machineQuota > isolatedStorageFilePermission.m_machineQuota) ? m_machineQuota : isolatedStorageFilePermission.m_machineQuota),
			m_expirationDays = ((m_expirationDays > isolatedStorageFilePermission.m_expirationDays) ? m_expirationDays : isolatedStorageFilePermission.m_expirationDays),
			m_permanentData = (m_permanentData || isolatedStorageFilePermission.m_permanentData),
			UsageAllowed = ((m_allowed > isolatedStorageFilePermission.m_allowed) ? m_allowed : isolatedStorageFilePermission.m_allowed)
		};
	}

	[MonoTODO("(2.0) new override - something must have been added ???")]
	[ComVisible(false)]
	public override SecurityElement ToXml()
	{
		return base.ToXml();
	}

	int IBuiltInPermission.GetTokenIndex()
	{
		return 3;
	}

	private IsolatedStorageFilePermission Cast(IPermission target)
	{
		if (target == null)
		{
			return null;
		}
		IsolatedStorageFilePermission obj = target as IsolatedStorageFilePermission;
		if (obj == null)
		{
			CodeAccessPermission.ThrowInvalidPermission(target, typeof(IsolatedStorageFilePermission));
		}
		return obj;
	}
}
