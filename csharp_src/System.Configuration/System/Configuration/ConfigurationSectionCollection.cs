using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Unity;

namespace System.Configuration;

[Serializable]
public sealed class ConfigurationSectionCollection : NameObjectCollectionBase
{
	public ConfigurationSection this[string name]
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	internal ConfigurationSectionCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	[SpecialName]
	public ConfigurationSection get_Item(int index)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void Add(string name, ConfigurationSection section)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Clear()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void CopyTo(ConfigurationSection[] array, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationSection Get(int index)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public ConfigurationSection Get(string name)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public string GetKey(int index)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
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
