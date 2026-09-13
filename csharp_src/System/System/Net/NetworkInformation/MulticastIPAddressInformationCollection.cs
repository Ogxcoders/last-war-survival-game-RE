using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation;

public class MulticastIPAddressInformationCollection : ICollection<MulticastIPAddressInformation>, IEnumerable<MulticastIPAddressInformation>, IEnumerable
{
	private Collection<MulticastIPAddressInformation> addresses = new Collection<MulticastIPAddressInformation>();

	public virtual int Count => addresses.Count;

	public virtual bool IsReadOnly => true;

	public virtual MulticastIPAddressInformation this[int index] => addresses[index];

	protected internal MulticastIPAddressInformationCollection()
	{
	}

	public virtual void CopyTo(MulticastIPAddressInformation[] array, int offset)
	{
		addresses.CopyTo(array, offset);
	}

	public virtual void Add(MulticastIPAddressInformation address)
	{
		throw new NotSupportedException(global::SR.GetString("The collection is read-only."));
	}

	internal void InternalAdd(MulticastIPAddressInformation address)
	{
		addresses.Add(address);
	}

	public virtual bool Contains(MulticastIPAddressInformation address)
	{
		return addresses.Contains(address);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public virtual IEnumerator<MulticastIPAddressInformation> GetEnumerator()
	{
		return addresses.GetEnumerator();
	}

	public virtual bool Remove(MulticastIPAddressInformation address)
	{
		throw new NotSupportedException(global::SR.GetString("The collection is read-only."));
	}

	public virtual void Clear()
	{
		throw new NotSupportedException(global::SR.GetString("The collection is read-only."));
	}
}
