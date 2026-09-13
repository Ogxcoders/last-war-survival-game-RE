using Unity;

namespace System.Configuration;

public class ProtectedProviderSettings : ConfigurationElement
{
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

	public ProtectedProviderSettings()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
