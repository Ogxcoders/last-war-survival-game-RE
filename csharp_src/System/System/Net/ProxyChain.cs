using System.Collections;
using System.Collections.Generic;

namespace System.Net;

internal abstract class ProxyChain : IEnumerable<Uri>, IEnumerable, IDisposable
{
	private class ProxyEnumerator : IEnumerator<Uri>, IDisposable, IEnumerator
	{
		private ProxyChain m_Chain;

		private bool m_Finished;

		private int m_CurrentIndex = -1;

		private bool m_TriedDirect;

		public Uri Current
		{
			get
			{
				if (m_Finished || m_CurrentIndex < 0)
				{
					throw new InvalidOperationException(global::SR.GetString("Enumeration has either not started or has already finished."));
				}
				return m_Chain.m_Cache[m_CurrentIndex];
			}
		}

		object IEnumerator.Current => Current;

		internal ProxyEnumerator(ProxyChain chain)
		{
			m_Chain = chain;
		}

		public bool MoveNext()
		{
			if (m_Finished)
			{
				return false;
			}
			checked
			{
				m_CurrentIndex++;
				if (m_Chain.m_Cache.Count > m_CurrentIndex)
				{
					return true;
				}
				if (m_Chain.m_CacheComplete)
				{
					m_Finished = true;
					return false;
				}
				lock (m_Chain.m_Cache)
				{
					if (m_Chain.m_Cache.Count > m_CurrentIndex)
					{
						return true;
					}
					if (m_Chain.m_CacheComplete)
					{
						m_Finished = true;
						return false;
					}
					Uri proxy;
					while (true)
					{
						if (!m_Chain.GetNextProxy(out proxy))
						{
							m_Finished = true;
							m_Chain.m_CacheComplete = true;
							return false;
						}
						if (!(proxy == null))
						{
							break;
						}
						if (!m_TriedDirect)
						{
							m_TriedDirect = true;
							break;
						}
					}
					m_Chain.m_Cache.Add(proxy);
					return true;
				}
			}
		}

		public void Reset()
		{
			m_Finished = false;
			m_CurrentIndex = -1;
		}

		public void Dispose()
		{
		}
	}

	private List<Uri> m_Cache = new List<Uri>();

	private bool m_CacheComplete;

	private ProxyEnumerator m_MainEnumerator;

	private Uri m_Destination;

	private HttpAbortDelegate m_HttpAbortDelegate;

	internal IEnumerator<Uri> Enumerator
	{
		get
		{
			if (m_MainEnumerator != null)
			{
				return m_MainEnumerator;
			}
			return GetEnumerator();
		}
	}

	internal Uri Destination => m_Destination;

	internal HttpAbortDelegate HttpAbortDelegate
	{
		get
		{
			if (m_HttpAbortDelegate == null)
			{
				m_HttpAbortDelegate = HttpAbort;
			}
			return m_HttpAbortDelegate;
		}
	}

	protected ProxyChain(Uri destination)
	{
		m_Destination = destination;
	}

	public IEnumerator<Uri> GetEnumerator()
	{
		ProxyEnumerator proxyEnumerator = new ProxyEnumerator(this);
		if (m_MainEnumerator == null)
		{
			m_MainEnumerator = proxyEnumerator;
		}
		return proxyEnumerator;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public virtual void Dispose()
	{
	}

	internal virtual void Abort()
	{
	}

	internal bool HttpAbort(HttpWebRequest request, WebException webException)
	{
		Abort();
		return true;
	}

	protected abstract bool GetNextProxy(out Uri proxy);
}
