using Unity;

namespace System.Configuration;

[ConfigurationCollection(typeof(KeyValueConfigurationElement))]
public class KeyValueConfigurationCollection : ConfigurationElementCollection
{
	public string[] AllKeys
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public new KeyValueConfigurationElement this[string key]
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

	protected override bool ThrowOnDuplicate
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public KeyValueConfigurationCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Add(KeyValueConfigurationElement keyValue)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Add(string key, string value)
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

	public void Remove(string key)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
