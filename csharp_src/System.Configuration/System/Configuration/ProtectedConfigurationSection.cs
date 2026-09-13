using Unity;

namespace System.Configuration;

public sealed class ProtectedConfigurationSection : ConfigurationSection
{
	public string DefaultProvider
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

	protected internal override ConfigurationPropertyCollection Properties
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ProviderSettingsCollection Providers
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ProtectedConfigurationSection()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
