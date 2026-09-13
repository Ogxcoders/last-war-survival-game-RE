using Unity;

namespace System.Configuration;

public class ConfigurationFileMap : ICloneable
{
	public string MachineConfigFilename
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

	public ConfigurationFileMap()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationFileMap(string machineConfigFilename)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual object Clone()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
