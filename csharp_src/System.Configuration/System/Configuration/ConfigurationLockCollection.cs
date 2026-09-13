using System.Collections;
using Unity;

namespace System.Configuration;

public sealed class ConfigurationLockCollection : ICollection, IEnumerable
{
	public string AttributeList
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public int Count
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(int);
		}
	}

	public bool HasParentElements
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool IsModified
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
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

	public object SyncRoot
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	internal ConfigurationLockCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Add(string name)
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

	public void CopyTo(string[] array, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public IEnumerator GetEnumerator()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public bool IsReadOnly(string name)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public void Remove(string name)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void SetFromList(string attributeList)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	void ICollection.CopyTo(Array array, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
