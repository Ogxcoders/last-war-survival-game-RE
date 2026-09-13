using System.Collections;

namespace System.Web.Services.Discovery;

public sealed class DiscoveryClientResultCollection : CollectionBase
{
	public DiscoveryClientResult this[int i]
	{
		get
		{
			if (i < 0 || i >= base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (DiscoveryClientResult)base.InnerList[i];
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

	public int Add(DiscoveryClientResult value)
	{
		return base.InnerList.Add(value);
	}

	public bool Contains(DiscoveryClientResult value)
	{
		return base.InnerList.Contains(value);
	}

	public void Remove(DiscoveryClientResult value)
	{
		base.InnerList.Remove(value);
	}
}
