using System.Collections;
using System.Collections.Generic;

namespace System.Net;

public class HttpListenerPrefixCollection : ICollection<string>, IEnumerable<string>, IEnumerable
{
	private List<string> prefixes = new List<string>();

	private HttpListener listener;

	public int Count => prefixes.Count;

	public bool IsReadOnly => false;

	public bool IsSynchronized => false;

	internal HttpListenerPrefixCollection(HttpListener listener)
	{
		this.listener = listener;
	}

	public void Add(string uriPrefix)
	{
		listener.CheckDisposed();
		ListenerPrefix.CheckUri(uriPrefix);
		if (!prefixes.Contains(uriPrefix))
		{
			prefixes.Add(uriPrefix);
			if (listener.IsListening)
			{
				EndPointManager.AddPrefix(uriPrefix, listener);
			}
		}
	}

	public void Clear()
	{
		listener.CheckDisposed();
		prefixes.Clear();
		if (listener.IsListening)
		{
			EndPointManager.RemoveListener(listener);
		}
	}

	public bool Contains(string uriPrefix)
	{
		listener.CheckDisposed();
		return prefixes.Contains(uriPrefix);
	}

	public void CopyTo(string[] array, int offset)
	{
		listener.CheckDisposed();
		prefixes.CopyTo(array, offset);
	}

	public void CopyTo(Array array, int offset)
	{
		listener.CheckDisposed();
		((ICollection)prefixes).CopyTo(array, offset);
	}

	public IEnumerator<string> GetEnumerator()
	{
		return prefixes.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return prefixes.GetEnumerator();
	}

	public bool Remove(string uriPrefix)
	{
		listener.CheckDisposed();
		if (uriPrefix == null)
		{
			throw new ArgumentNullException("uriPrefix");
		}
		bool num = prefixes.Remove(uriPrefix);
		if (num && listener.IsListening)
		{
			EndPointManager.RemovePrefix(uriPrefix, listener);
		}
		return num;
	}
}
