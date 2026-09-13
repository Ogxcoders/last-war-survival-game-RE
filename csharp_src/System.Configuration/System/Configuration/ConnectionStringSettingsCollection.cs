using System.Runtime.CompilerServices;
using Unity;

namespace System.Configuration;

[ConfigurationCollection(typeof(ConnectionStringSettings))]
public sealed class ConnectionStringSettingsCollection : ConfigurationElementCollection
{
	public new ConnectionStringSettings this[string name]
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

	public ConnectionStringSettingsCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	[SpecialName]
	public ConnectionStringSettings get_Item(int index)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void Add(ConnectionStringSettings settings)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected override void BaseAdd(int index, ConfigurationElement element)
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

	public int IndexOf(ConnectionStringSettings settings)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(int);
	}

	public void Remove(ConnectionStringSettings settings)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Remove(string name)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void RemoveAt(int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
