using System.Collections;

namespace System.Net;

internal sealed class EndPointManager
{
	private static Hashtable ip_to_endpoints = new Hashtable();

	private EndPointManager()
	{
	}

	public static void AddListener(HttpListener listener)
	{
		ArrayList arrayList = new ArrayList();
		try
		{
			lock (ip_to_endpoints)
			{
				foreach (string prefix in listener.Prefixes)
				{
					AddPrefixInternal(prefix, listener);
					arrayList.Add(prefix);
				}
			}
		}
		catch
		{
			foreach (string item in arrayList)
			{
				RemovePrefix(item, listener);
			}
			throw;
		}
	}

	public static void AddPrefix(string prefix, HttpListener listener)
	{
		lock (ip_to_endpoints)
		{
			AddPrefixInternal(prefix, listener);
		}
	}

	private static void AddPrefixInternal(string p, HttpListener listener)
	{
		ListenerPrefix listenerPrefix = new ListenerPrefix(p);
		if (listenerPrefix.Path.IndexOf('%') != -1)
		{
			throw new HttpListenerException(400, "Invalid path.");
		}
		if (listenerPrefix.Path.IndexOf("//", StringComparison.Ordinal) != -1)
		{
			throw new HttpListenerException(400, "Invalid path.");
		}
		GetEPListener(listenerPrefix.Host, listenerPrefix.Port, listener, listenerPrefix.Secure).AddPrefix(listenerPrefix, listener);
	}

	private static EndPointListener GetEPListener(string host, int port, HttpListener listener, bool secure)
	{
		IPAddress address;
		if (host == "*")
		{
			address = IPAddress.Any;
		}
		else if (!IPAddress.TryParse(host, out address))
		{
			try
			{
				IPHostEntry hostByName = Dns.GetHostByName(host);
				address = ((hostByName == null) ? IPAddress.Any : hostByName.AddressList[0]);
			}
			catch
			{
				address = IPAddress.Any;
			}
		}
		Hashtable hashtable = null;
		if (ip_to_endpoints.ContainsKey(address))
		{
			hashtable = (Hashtable)ip_to_endpoints[address];
		}
		else
		{
			hashtable = new Hashtable();
			ip_to_endpoints[address] = hashtable;
		}
		EndPointListener endPointListener = null;
		if (hashtable.ContainsKey(port))
		{
			endPointListener = (EndPointListener)hashtable[port];
		}
		else
		{
			endPointListener = new EndPointListener(listener, address, port, secure);
			hashtable[port] = endPointListener;
		}
		return endPointListener;
	}

	public static void RemoveEndPoint(EndPointListener epl, IPEndPoint ep)
	{
		lock (ip_to_endpoints)
		{
			Hashtable obj = (Hashtable)ip_to_endpoints[ep.Address];
			obj.Remove(ep.Port);
			if (obj.Count == 0)
			{
				ip_to_endpoints.Remove(ep.Address);
			}
			epl.Close();
		}
	}

	public static void RemoveListener(HttpListener listener)
	{
		lock (ip_to_endpoints)
		{
			foreach (string prefix in listener.Prefixes)
			{
				RemovePrefixInternal(prefix, listener);
			}
		}
	}

	public static void RemovePrefix(string prefix, HttpListener listener)
	{
		lock (ip_to_endpoints)
		{
			RemovePrefixInternal(prefix, listener);
		}
	}

	private static void RemovePrefixInternal(string prefix, HttpListener listener)
	{
		ListenerPrefix listenerPrefix = new ListenerPrefix(prefix);
		if (listenerPrefix.Path.IndexOf('%') == -1 && listenerPrefix.Path.IndexOf("//", StringComparison.Ordinal) == -1)
		{
			GetEPListener(listenerPrefix.Host, listenerPrefix.Port, listener, listenerPrefix.Secure).RemovePrefix(listenerPrefix, listener);
		}
	}
}
