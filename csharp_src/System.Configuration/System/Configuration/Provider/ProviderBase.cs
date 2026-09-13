using System.Collections.Specialized;
using Unity;

namespace System.Configuration.Provider;

public abstract class ProviderBase
{
	public virtual string Description
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public virtual string Name
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected ProviderBase()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual void Initialize(string name, NameValueCollection config)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
