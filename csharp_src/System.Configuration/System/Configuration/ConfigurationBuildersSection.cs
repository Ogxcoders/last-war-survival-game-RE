using Unity;

namespace System.Configuration;

public sealed class ConfigurationBuildersSection : ConfigurationSection
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

	public ConfigurationBuildersSection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationBuilder GetBuilderFromName(string builderName)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
