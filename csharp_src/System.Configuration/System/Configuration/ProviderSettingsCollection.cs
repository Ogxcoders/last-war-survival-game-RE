using System.Runtime.CompilerServices;
using Unity;

namespace System.Configuration;

[ConfigurationCollection(typeof(ProviderSettings))]
public sealed class ProviderSettingsCollection : ConfigurationElementCollection
{
	public new ProviderSettings this[string key]
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

	public ProviderSettingsCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	[SpecialName]
	public ProviderSettings get_Item(int index)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void Add(ProviderSettings provider)
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

	public void Remove(string name)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
