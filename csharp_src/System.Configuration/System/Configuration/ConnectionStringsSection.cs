using Unity;

namespace System.Configuration;

public sealed class ConnectionStringsSection : ConfigurationSection
{
	public ConnectionStringSettingsCollection ConnectionStrings
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

	public ConnectionStringsSection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override object GetRuntimeObject()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
