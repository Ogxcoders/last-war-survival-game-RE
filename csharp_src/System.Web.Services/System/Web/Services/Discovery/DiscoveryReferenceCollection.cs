using System.Collections;

namespace System.Web.Services.Discovery;

public sealed class DiscoveryReferenceCollection : CollectionBase
{
	public DiscoveryReference this[int i]
	{
		get
		{
			if (i < 0 || i >= base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (DiscoveryReference)base.InnerList[i];
		}
		set
		{
			if (i < 0 || i >= base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			base.InnerList[i] = value;
		}
	}

	public int Add(DiscoveryReference value)
	{
		return base.InnerList.Add(value);
	}

	public bool Contains(DiscoveryReference value)
	{
		return base.InnerList.Contains(value);
	}

	public void Remove(DiscoveryReference value)
	{
		base.InnerList.Remove(value);
	}
}
