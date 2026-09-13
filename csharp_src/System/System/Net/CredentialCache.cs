using System.Collections;

namespace System.Net;

public class CredentialCache : ICredentials, ICredentialsByHost, IEnumerable
{
	private class CredentialEnumerator : IEnumerator
	{
		private CredentialCache m_cache;

		private ICredentials[] m_array;

		private int m_index = -1;

		private int m_version;

		object IEnumerator.Current
		{
			get
			{
				if (m_index < 0 || m_index >= m_array.Length)
				{
					throw new InvalidOperationException(global::SR.GetString("Enumeration has either not started or has already finished."));
				}
				if (m_version != m_cache.m_version)
				{
					throw new InvalidOperationException(global::SR.GetString("Collection was modified; enumeration operation may not execute."));
				}
				return m_array[m_index];
			}
		}

		internal CredentialEnumerator(CredentialCache cache, Hashtable table, Hashtable hostTable, int version)
		{
			m_cache = cache;
			m_array = new ICredentials[table.Count + hostTable.Count];
			table.Values.CopyTo(m_array, 0);
			hostTable.Values.CopyTo(m_array, table.Count);
			m_version = version;
		}

		bool IEnumerator.MoveNext()
		{
			if (m_version != m_cache.m_version)
			{
				throw new InvalidOperationException(global::SR.GetString("Collection was modified; enumeration operation may not execute."));
			}
			if (++m_index < m_array.Length)
			{
				return true;
			}
			m_index = m_array.Length;
			return false;
		}

		void IEnumerator.Reset()
		{
			m_index = -1;
		}
	}

	private Hashtable cache = new Hashtable();

	private Hashtable cacheForHosts = new Hashtable();

	internal int m_version;

	private int m_NumbDefaultCredInCache;

	internal bool IsDefaultInCache => m_NumbDefaultCredInCache != 0;

	public static ICredentials DefaultCredentials => SystemNetworkCredential.defaultCredential;

	public static NetworkCredential DefaultNetworkCredentials => SystemNetworkCredential.defaultCredential;

	public void Add(Uri uriPrefix, string authType, NetworkCredential cred)
	{
		if (uriPrefix == null)
		{
			throw new ArgumentNullException("uriPrefix");
		}
		if (authType == null)
		{
			throw new ArgumentNullException("authType");
		}
		if (cred is SystemNetworkCredential)
		{
			throw new ArgumentException(global::SR.GetString("Default credentials cannot be supplied for the {0} authentication scheme.", authType), "authType");
		}
		m_version++;
		CredentialKey key = new CredentialKey(uriPrefix, authType);
		cache.Add(key, cred);
		if (cred is SystemNetworkCredential)
		{
			m_NumbDefaultCredInCache++;
		}
	}

	public void Add(string host, int port, string authenticationType, NetworkCredential credential)
	{
		if (host == null)
		{
			throw new ArgumentNullException("host");
		}
		if (authenticationType == null)
		{
			throw new ArgumentNullException("authenticationType");
		}
		if (host.Length == 0)
		{
			throw new ArgumentException(global::SR.GetString("The parameter '{0}' cannot be an empty string.", "host"));
		}
		if (port < 0)
		{
			throw new ArgumentOutOfRangeException("port");
		}
		if (credential is SystemNetworkCredential)
		{
			throw new ArgumentException(global::SR.GetString("Default credentials cannot be supplied for the {0} authentication scheme.", authenticationType), "authenticationType");
		}
		m_version++;
		CredentialHostKey key = new CredentialHostKey(host, port, authenticationType);
		cacheForHosts.Add(key, credential);
		if (credential is SystemNetworkCredential)
		{
			m_NumbDefaultCredInCache++;
		}
	}

	public void Remove(Uri uriPrefix, string authType)
	{
		if (!(uriPrefix == null) && authType != null)
		{
			m_version++;
			CredentialKey key = new CredentialKey(uriPrefix, authType);
			if (cache[key] is SystemNetworkCredential)
			{
				m_NumbDefaultCredInCache--;
			}
			cache.Remove(key);
		}
	}

	public void Remove(string host, int port, string authenticationType)
	{
		if (host != null && authenticationType != null && port >= 0)
		{
			m_version++;
			CredentialHostKey key = new CredentialHostKey(host, port, authenticationType);
			if (cacheForHosts[key] is SystemNetworkCredential)
			{
				m_NumbDefaultCredInCache--;
			}
			cacheForHosts.Remove(key);
		}
	}

	public NetworkCredential GetCredential(Uri uriPrefix, string authType)
	{
		if (uriPrefix == null)
		{
			throw new ArgumentNullException("uriPrefix");
		}
		if (authType == null)
		{
			throw new ArgumentNullException("authType");
		}
		int num = -1;
		NetworkCredential result = null;
		IDictionaryEnumerator enumerator = cache.GetEnumerator();
		while (enumerator.MoveNext())
		{
			CredentialKey credentialKey = (CredentialKey)enumerator.Key;
			if (credentialKey.Match(uriPrefix, authType))
			{
				int uriPrefixLength = credentialKey.UriPrefixLength;
				if (uriPrefixLength > num)
				{
					num = uriPrefixLength;
					result = (NetworkCredential)enumerator.Value;
				}
			}
		}
		return result;
	}

	public NetworkCredential GetCredential(string host, int port, string authenticationType)
	{
		if (host == null)
		{
			throw new ArgumentNullException("host");
		}
		if (authenticationType == null)
		{
			throw new ArgumentNullException("authenticationType");
		}
		if (host.Length == 0)
		{
			throw new ArgumentException(global::SR.GetString("The parameter '{0}' cannot be an empty string.", "host"));
		}
		if (port < 0)
		{
			throw new ArgumentOutOfRangeException("port");
		}
		NetworkCredential result = null;
		IDictionaryEnumerator enumerator = cacheForHosts.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (((CredentialHostKey)enumerator.Key).Match(host, port, authenticationType))
			{
				result = (NetworkCredential)enumerator.Value;
			}
		}
		return result;
	}

	public IEnumerator GetEnumerator()
	{
		return new CredentialEnumerator(this, cache, cacheForHosts, m_version);
	}
}
