using System.Collections;

namespace System.Web.Services.Discovery;

public sealed class DiscoveryClientReferenceCollection : DictionaryBase
{
	public DiscoveryReference this[string url]
	{
		get
		{
			return (DiscoveryReference)base.InnerHashtable[url];
		}
		set
		{
			base.InnerHashtable[url] = value;
		}
	}

	public ICollection Keys => base.InnerHashtable.Keys;

	public ICollection Values => base.InnerHashtable.Values;

	public void Add(DiscoveryReference value)
	{
		Add(value.Url, value);
	}

	public void Add(string url, DiscoveryReference value)
	{
		base.InnerHashtable[url] = value;
	}

	public bool Contains(string url)
	{
		return base.InnerHashtable.Contains(url);
	}

	public void Remove(string url)
	{
		base.InnerHashtable.Remove(url);
	}
}
