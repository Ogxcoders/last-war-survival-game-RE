using System.Collections;

namespace System.Web.Services.Discovery;

public sealed class DiscoveryClientDocumentCollection : DictionaryBase
{
	public object this[string url]
	{
		get
		{
			return base.InnerHashtable[url];
		}
		set
		{
			if (url == null)
			{
				throw new ArgumentNullException();
			}
			base.InnerHashtable[url] = value;
		}
	}

	public ICollection Keys => base.InnerHashtable.Keys;

	public ICollection Values => base.InnerHashtable.Values;

	public void Add(string url, object value)
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
