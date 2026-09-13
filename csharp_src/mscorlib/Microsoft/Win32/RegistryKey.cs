using System;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32;

public sealed class RegistryKey : IDisposable
{
	public SafeRegistryHandle Handle
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public string Name
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int SubKeyCount
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int ValueCount
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public RegistryView View
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	internal RegistryKey(RegistryHive hiveId)
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
	}

	public void Close()
	{
	}

	public RegistryKey CreateSubKey(string subkey)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey CreateSubKey(string subkey, bool writable)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey CreateSubKey(string subkey, bool writable, RegistryOptions options)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions, RegistrySecurity registrySecurity)
	{
		throw new PlatformNotSupportedException();
	}

	public void DeleteSubKey(string subkey)
	{
	}

	public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
	{
	}

	public void DeleteSubKeyTree(string subkey)
	{
	}

	public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey)
	{
	}

	public void DeleteValue(string name)
	{
	}

	public void DeleteValue(string name, bool throwOnMissingValue)
	{
	}

	public void Flush()
	{
	}

	public static RegistryKey FromHandle(SafeRegistryHandle handle)
	{
		throw new PlatformNotSupportedException();
	}

	public static RegistryKey FromHandle(SafeRegistryHandle handle, RegistryView view)
	{
		throw new PlatformNotSupportedException();
	}

	public string[] GetSubKeyNames()
	{
		throw new PlatformNotSupportedException();
	}

	public object GetValue(string name)
	{
		throw new PlatformNotSupportedException();
	}

	public object GetValue(string name, object defaultValue)
	{
		throw new PlatformNotSupportedException();
	}

	public object GetValue(string name, object defaultValue, RegistryValueOptions options)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistrySecurity GetAccessControl()
	{
		throw new PlatformNotSupportedException();
	}

	public RegistrySecurity GetAccessControl(AccessControlSections includeSections)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryValueKind GetValueKind(string name)
	{
		throw new PlatformNotSupportedException();
	}

	public string[] GetValueNames()
	{
		throw new PlatformNotSupportedException();
	}

	public static RegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey OpenSubKey(string name)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey OpenSubKey(string name, bool writable)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey OpenSubKey(string name, RegistryRights rights)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
	{
		throw new PlatformNotSupportedException();
	}

	public RegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
	{
		throw new PlatformNotSupportedException();
	}

	public static RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName)
	{
		throw new PlatformNotSupportedException();
	}

	public static RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName, RegistryView view)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetAccessControl(RegistrySecurity registrySecurity)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetValue(string name, object value)
	{
	}

	public void SetValue(string name, object value, RegistryValueKind valueKind)
	{
	}
}
