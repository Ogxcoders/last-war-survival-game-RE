using System.Runtime.InteropServices;

namespace System.Security.Permissions;

[Serializable]
[ComVisible(true)]
public sealed class KeyContainerPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
{
	private KeyContainerPermissionAccessEntryCollection _accessEntries;

	private KeyContainerPermissionFlags _flags;

	private const int version = 1;

	public KeyContainerPermissionAccessEntryCollection AccessEntries => _accessEntries;

	public KeyContainerPermissionFlags Flags => _flags;

	public KeyContainerPermission(PermissionState state)
	{
		if (CodeAccessPermission.CheckPermissionState(state, allowUnrestricted: true) == PermissionState.Unrestricted)
		{
			_flags = KeyContainerPermissionFlags.AllFlags;
		}
	}

	public KeyContainerPermission(KeyContainerPermissionFlags flags)
	{
		SetFlags(flags);
	}

	public KeyContainerPermission(KeyContainerPermissionFlags flags, KeyContainerPermissionAccessEntry[] accessList)
	{
		SetFlags(flags);
		if (accessList != null)
		{
			_accessEntries = new KeyContainerPermissionAccessEntryCollection();
			foreach (KeyContainerPermissionAccessEntry accessEntry in accessList)
			{
				_accessEntries.Add(accessEntry);
			}
		}
	}

	public override IPermission Copy()
	{
		if (_accessEntries.Count == 0)
		{
			return new KeyContainerPermission(_flags);
		}
		KeyContainerPermissionAccessEntry[] array = new KeyContainerPermissionAccessEntry[_accessEntries.Count];
		_accessEntries.CopyTo(array, 0);
		return new KeyContainerPermission(_flags, array);
	}

	[MonoTODO("(2.0) missing support for AccessEntries")]
	public override void FromXml(SecurityElement securityElement)
	{
		CodeAccessPermission.CheckSecurityElement(securityElement, "securityElement", 1, 1);
		if (CodeAccessPermission.IsUnrestricted(securityElement))
		{
			_flags = KeyContainerPermissionFlags.AllFlags;
		}
		else
		{
			_flags = (KeyContainerPermissionFlags)Enum.Parse(typeof(KeyContainerPermissionFlags), securityElement.Attribute("Flags"));
		}
	}

	[MonoTODO("(2.0)")]
	public override IPermission Intersect(IPermission target)
	{
		return null;
	}

	[MonoTODO("(2.0)")]
	public override bool IsSubsetOf(IPermission target)
	{
		return false;
	}

	public bool IsUnrestricted()
	{
		return _flags == KeyContainerPermissionFlags.AllFlags;
	}

	[MonoTODO("(2.0) missing support for AccessEntries")]
	public override SecurityElement ToXml()
	{
		SecurityElement securityElement = Element(1);
		if (IsUnrestricted())
		{
			securityElement.AddAttribute("Unrestricted", "true");
		}
		return securityElement;
	}

	public override IPermission Union(IPermission target)
	{
		KeyContainerPermission keyContainerPermission = Cast(target);
		if (keyContainerPermission == null)
		{
			return Copy();
		}
		KeyContainerPermissionAccessEntryCollection keyContainerPermissionAccessEntryCollection = new KeyContainerPermissionAccessEntryCollection();
		KeyContainerPermissionAccessEntryEnumerator enumerator = _accessEntries.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyContainerPermissionAccessEntry current = enumerator.Current;
			keyContainerPermissionAccessEntryCollection.Add(current);
		}
		enumerator = keyContainerPermission._accessEntries.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyContainerPermissionAccessEntry current2 = enumerator.Current;
			if (_accessEntries.IndexOf(current2) == -1)
			{
				keyContainerPermissionAccessEntryCollection.Add(current2);
			}
		}
		if (keyContainerPermissionAccessEntryCollection.Count == 0)
		{
			return new KeyContainerPermission(_flags | keyContainerPermission._flags);
		}
		KeyContainerPermissionAccessEntry[] array = new KeyContainerPermissionAccessEntry[keyContainerPermissionAccessEntryCollection.Count];
		keyContainerPermissionAccessEntryCollection.CopyTo(array, 0);
		return new KeyContainerPermission(_flags | keyContainerPermission._flags, array);
	}

	int IBuiltInPermission.GetTokenIndex()
	{
		return 16;
	}

	private void SetFlags(KeyContainerPermissionFlags flags)
	{
		if ((flags & KeyContainerPermissionFlags.AllFlags) == 0)
		{
			throw new ArgumentException(string.Format(Locale.GetText("Invalid enum {0}"), flags), "KeyContainerPermissionFlags");
		}
		_flags = flags;
	}

	private KeyContainerPermission Cast(IPermission target)
	{
		if (target == null)
		{
			return null;
		}
		KeyContainerPermission obj = target as KeyContainerPermission;
		if (obj == null)
		{
			CodeAccessPermission.ThrowInvalidPermission(target, typeof(KeyContainerPermission));
		}
		return obj;
	}
}
