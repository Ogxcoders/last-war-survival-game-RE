using System.Collections;
using Unity;

namespace System.Configuration;

public class ConfigurationPropertyCollection : ICollection, IEnumerable
{
	public int Count
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(int);
		}
	}

	public bool IsSynchronized
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public ConfigurationProperty this[string name]
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public object SyncRoot
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationPropertyCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Add(ConfigurationProperty property)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Clear()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public bool Contains(string name)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public void CopyTo(ConfigurationProperty[] array, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public IEnumerator GetEnumerator()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public bool Remove(string name)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	void ICollection.CopyTo(Array array, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
