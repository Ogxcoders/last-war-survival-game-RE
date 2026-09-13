using System.Collections;
using Unity;

namespace System.Configuration.Provider;

public class ProviderCollection : ICollection, IEnumerable
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

	public ProviderBase this[string name]
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

	public ProviderCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual void Add(ProviderBase provider)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Clear()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void CopyTo(ProviderBase[] array, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public IEnumerator GetEnumerator()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void Remove(string name)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void SetReadOnly()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	void ICollection.CopyTo(Array array, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
