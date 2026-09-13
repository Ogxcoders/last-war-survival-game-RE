using System.Configuration.Provider;
using Unity;

namespace System.Configuration;

public class ProtectedConfigurationProviderCollection : ProviderCollection
{
	public new ProtectedConfigurationProvider this[string name]
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ProtectedConfigurationProviderCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public override void Add(ProviderBase provider)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
