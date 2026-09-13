using Unity;

namespace System.Configuration;

[ConfigurationCollection(typeof(NameValueConfigurationElement))]
public sealed class NameValueConfigurationCollection : ConfigurationElementCollection
{
	public string[] AllKeys
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public new NameValueConfigurationElement this[string name]
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

	public NameValueConfigurationCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Add(NameValueConfigurationElement nameValue)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Clear()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected override ConfigurationElement CreateNewElement()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected override object GetElementKey(ConfigurationElement element)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void Remove(NameValueConfigurationElement nameValue)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Remove(string name)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
