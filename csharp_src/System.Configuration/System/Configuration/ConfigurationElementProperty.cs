using Unity;

namespace System.Configuration;

public sealed class ConfigurationElementProperty
{
	public ConfigurationValidatorBase Validator
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationElementProperty(ConfigurationValidatorBase validator)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
