using Unity;

namespace System.Configuration;

public sealed class NameValueConfigurationElement : ConfigurationElement
{
	public string Name
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

	public string Value
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

	public NameValueConfigurationElement(string name, string value)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
