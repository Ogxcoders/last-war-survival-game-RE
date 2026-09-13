using Unity;

namespace System.Configuration;

public sealed class ConnectionStringSettings : ConfigurationElement
{
	public string ConnectionString
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

	public string Name
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

	public string ProviderName
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

	public ConnectionStringSettings()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConnectionStringSettings(string name, string connectionString)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConnectionStringSettings(string name, string connectionString, string providerName)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
