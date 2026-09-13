using Unity;

namespace System.Configuration;

public sealed class ExeConfigurationFileMap : ConfigurationFileMap
{
	public string ExeConfigFilename
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public string LocalUserConfigFilename
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public string RoamingUserConfigFilename
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public ExeConfigurationFileMap()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ExeConfigurationFileMap(string machineConfigFileName)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public override object Clone()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
