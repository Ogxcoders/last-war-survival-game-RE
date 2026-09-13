using System.Collections;

namespace System.Web.Services.Discovery;

public sealed class DiscoveryExceptionDictionary : DictionaryBase
{
	public Exception this[string url]
	{
		get
		{
			return (Exception)base.InnerHashtable[url];
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

	public void Add(string url, Exception value)
	{
		base.InnerHashtable.Add(url, value);
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
