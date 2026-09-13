using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32;

[ComVisible(true)]
public static class Registry
{
	public static readonly RegistryKey ClassesRoot = new RegistryKey(RegistryHive.ClassesRoot);

	public static readonly RegistryKey CurrentConfig = new RegistryKey(RegistryHive.CurrentConfig);

	public static readonly RegistryKey CurrentUser = new RegistryKey(RegistryHive.CurrentUser);

	[Obsolete("Use PerformanceData instead")]
	public static readonly RegistryKey DynData = new RegistryKey(RegistryHive.DynData);

	public static readonly RegistryKey LocalMachine = new RegistryKey(RegistryHive.LocalMachine);

	public static readonly RegistryKey PerformanceData = new RegistryKey(RegistryHive.PerformanceData);

	public static readonly RegistryKey Users = new RegistryKey(RegistryHive.Users);

	private static RegistryKey ToKey(string keyName, bool setting)
	{
		if (keyName == null)
		{
			throw new ArgumentException("Not a valid registry key name", "keyName");
		}
		RegistryKey registryKey = null;
		string[] array = keyName.Split('\\');
		registryKey = array[0] switch
		{
			"HKEY_CLASSES_ROOT" => ClassesRoot, 
			"HKEY_CURRENT_CONFIG" => CurrentConfig, 
			"HKEY_CURRENT_USER" => CurrentUser, 
			"HKEY_DYN_DATA" => DynData, 
			"HKEY_LOCAL_MACHINE" => LocalMachine, 
			"HKEY_PERFORMANCE_DATA" => PerformanceData, 
			"HKEY_USERS" => Users, 
			_ => throw new ArgumentException("Keyname does not start with a valid registry root", "keyName"), 
		};
		for (int i = 1; i < array.Length; i++)
		{
			RegistryKey registryKey2 = registryKey.OpenSubKey(array[i], setting);
			if (registryKey2 == null)
			{
				if (!setting)
				{
					return null;
				}
				registryKey2 = registryKey.CreateSubKey(array[i]);
			}
			registryKey = registryKey2;
		}
		return registryKey;
	}

	public static void SetValue(string keyName, string valueName, object value)
	{
		RegistryKey registryKey = ToKey(keyName, setting: true);
		if (valueName.Length > 255)
		{
			throw new ArgumentException("valueName is larger than 255 characters", "valueName");
		}
		if (registryKey == null)
		{
			throw new ArgumentException("cant locate that keyName", "keyName");
		}
		registryKey.SetValue(valueName, value);
	}

	public static void SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind)
	{
		RegistryKey registryKey = ToKey(keyName, setting: true);
		if (valueName.Length > 255)
		{
			throw new ArgumentException("valueName is larger than 255 characters", "valueName");
		}
		if (registryKey == null)
		{
			throw new ArgumentException("cant locate that keyName", "keyName");
		}
		registryKey.SetValue(valueName, value, valueKind);
	}

	public static object GetValue(string keyName, string valueName, object defaultValue)
	{
		RegistryKey registryKey = ToKey(keyName, setting: false);
		if (registryKey == null)
		{
			return defaultValue;
		}
		return registryKey.GetValue(valueName, defaultValue);
	}
}
