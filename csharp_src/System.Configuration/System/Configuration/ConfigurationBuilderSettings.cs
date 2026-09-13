using Unity;

namespace System.Configuration;

public class ConfigurationBuilderSettings : ConfigurationElement
{
	public ProviderSettingsCollection Builders
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected internal override ConfigurationPropertyCollection Properties
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationBuilderSettings()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
