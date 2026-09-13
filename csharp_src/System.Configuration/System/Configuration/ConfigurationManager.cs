using System.Collections.Specialized;
using Unity;

namespace System.Configuration;

public static class ConfigurationManager
{
	public static NameValueCollection AppSettings
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public static ConnectionStringSettingsCollection ConnectionStrings
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public static object GetSection(string sectionName)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public static Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public static Configuration OpenExeConfiguration(string exePath)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public static Configuration OpenMachineConfiguration()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public static Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public static Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel, bool preLoad)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public static Configuration OpenMappedMachineConfiguration(ConfigurationFileMap fileMap)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public static void RefreshSection(string sectionName)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
