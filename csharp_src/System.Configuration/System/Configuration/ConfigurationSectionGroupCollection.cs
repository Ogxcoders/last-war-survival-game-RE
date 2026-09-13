using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Unity;

namespace System.Configuration;

[Serializable]
public sealed class ConfigurationSectionGroupCollection : NameObjectCollectionBase
{
	public ConfigurationSectionGroup this[string name]
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	internal ConfigurationSectionGroupCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	[SpecialName]
	public ConfigurationSectionGroup get_Item(int index)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void Add(string name, ConfigurationSectionGroup sectionGroup)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Clear()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void CopyTo(ConfigurationSectionGroup[] array, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationSectionGroup Get(int index)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public ConfigurationSectionGroup Get(string name)
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
