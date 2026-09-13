using Unity;

namespace System.Configuration;

public class KeyValueConfigurationElement : ConfigurationElement
{
	public string Key
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

	public KeyValueConfigurationElement(string key, string value)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override void Init()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
