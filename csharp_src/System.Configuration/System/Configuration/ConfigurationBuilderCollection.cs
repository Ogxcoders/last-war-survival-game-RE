using System.Configuration.Provider;
using Unity;

namespace System.Configuration;

public class ConfigurationBuilderCollection : ProviderCollection
{
	public new ConfigurationBuilder this[string name]
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationBuilderCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public override void Add(ProviderBase builder)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
