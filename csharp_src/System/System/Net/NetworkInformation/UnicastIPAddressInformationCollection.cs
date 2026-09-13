using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation;

public class UnicastIPAddressInformationCollection : ICollection<UnicastIPAddressInformation>, IEnumerable<UnicastIPAddressInformation>, IEnumerable
{
	private Collection<UnicastIPAddressInformation> addresses = new Collection<UnicastIPAddressInformation>();

	public virtual int Count => addresses.Count;

	public virtual bool IsReadOnly => true;

	public virtual UnicastIPAddressInformation this[int index] => addresses[index];

	protected internal UnicastIPAddressInformationCollection()
	{
	}

	public virtual void CopyTo(UnicastIPAddressInformation[] array, int offset)
	{
		addresses.CopyTo(array, offset);
	}

	public virtual void Add(UnicastIPAddressInformation address)
	{
		throw new NotSupportedException(global::SR.GetString("The collection is read-only."));
	}

	internal void InternalAdd(UnicastIPAddressInformation address)
	{
		addresses.Add(address);
	}

	public virtual bool Contains(UnicastIPAddressInformation address)
	{
		return addresses.Contains(address);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public virtual IEnumerator<UnicastIPAddressInformation> GetEnumerator()
	{
		return addresses.GetEnumerator();
	}

	public virtual bool Remove(UnicastIPAddressInformation address)
	{
		throw new NotSupportedException(global::SR.GetString("The collection is read-only."));
	}

	public virtual void Clear()
	{
		throw new NotSupportedException(global::SR.GetString("The collection is read-only."));
	}
}
