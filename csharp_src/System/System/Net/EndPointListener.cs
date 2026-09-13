using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net;

internal sealed class EndPointListener
{
	private HttpListener listener;

	private IPEndPoint endpoint;

	private Socket sock;

	private Hashtable prefixes;

	private ArrayList unhandled;

	private ArrayList all;

	private X509Certificate cert;

	private bool secure;

	private Dictionary<HttpConnection, HttpConnection> unregistered;

	internal HttpListener Listener => listener;

	public EndPointListener(HttpListener listener, IPAddress addr, int port, bool secure)
	{
		this.listener = listener;
		if (secure)
		{
			this.secure = secure;
			cert = listener.LoadCertificateAndKey(addr, port);
		}
		endpoint = new IPEndPoint(addr, port);
		sock = new Socket(addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		sock.Bind(endpoint);
		sock.Listen(500);
		SocketAsyncEventArgs e = new SocketAsyncEventArgs();
		e.UserToken = this;
		e.Completed += OnAccept;
		Socket accepted = null;
		Accept(sock, e, ref accepted);
		prefixes = new Hashtable();
		unregistered = new Dictionary<HttpConnection, HttpConnection>();
	}

	private static void Accept(Socket socket, SocketAsyncEventArgs e, ref Socket accepted)
	{
		e.AcceptSocket = null;
		bool flag;
		try
		{
			flag = socket.AcceptAsync(e);
		}
		catch
		{
			if (accepted != null)
			{
				try
				{
					accepted.Close();
				}
				catch
				{
				}
				accepted = null;
			}
			return;
		}
		if (!flag)
		{
			ProcessAccept(e);
		}
	}

	private static void ProcessAccept(SocketAsyncEventArgs args)
	{
		Socket accepted = null;
		if (args.SocketError == SocketError.Success)
		{
			accepted = args.AcceptSocket;
		}
		EndPointListener endPointListener = (EndPointListener)args.UserToken;
		Accept(endPointListener.sock, args, ref accepted);
		if (accepted == null)
		{
			return;
		}
		if (endPointListener.secure && endPointListener.cert == null)
		{
			accepted.Close();
			return;
		}
		HttpConnection httpConnection;
		try
		{
			httpConnection = new HttpConnection(accepted, endPointListener, endPointListener.secure, endPointListener.cert);
		}
		catch
		{
			accepted.Close();
			return;
		}
		lock (endPointListener.unregistered)
		{
			endPointListener.unregistered[httpConnection] = httpConnection;
		}
		httpConnection.BeginReadRequest();
	}

	private static void OnAccept(object sender, SocketAsyncEventArgs e)
	{
		ProcessAccept(e);
	}

	internal void RemoveConnection(HttpConnection conn)
	{
		lock (unregistered)
		{
			unregistered.Remove(conn);
		}
	}

	public bool BindContext(HttpListenerContext context)
	{
		HttpListenerRequest request = context.Request;
		ListenerPrefix prefix;
		HttpListener httpListener = SearchListener(request.Url, out prefix);
		if (httpListener == null)
		{
			return false;
		}
		context.Listener = httpListener;
		context.Connection.Prefix = prefix;
		return true;
	}

	public void UnbindContext(HttpListenerContext context)
	{
		if (context != null && context.Request != null)
		{
			context.Listener.UnregisterContext(context);
		}
	}

	private HttpListener SearchListener(Uri uri, out ListenerPrefix prefix)
	{
		prefix = null;
		if (uri == null)
		{
			return null;
		}
		string host = uri.Host;
		int port = uri.Port;
		string text = WebUtility.UrlDecode(uri.AbsolutePath);
		string text2 = ((text[text.Length - 1] == '/') ? text : (text + "/"));
		HttpListener result = null;
		int num = -1;
		if (host != null && host != "")
		{
			Hashtable hashtable = prefixes;
			foreach (ListenerPrefix key in hashtable.Keys)
			{
				string path = key.Path;
				if (path.Length >= num && !(key.Host != host) && key.Port == port && (text.StartsWith(path) || text2.StartsWith(path)))
				{
					num = path.Length;
					result = (HttpListener)hashtable[key];
					prefix = key;
				}
			}
			if (num != -1)
			{
				return result;
			}
		}
		ArrayList list = unhandled;
		result = MatchFromList(host, text, list, out prefix);
		if (text != text2 && result == null)
		{
			result = MatchFromList(host, text2, list, out prefix);
		}
		if (result != null)
		{
			return result;
		}
		list = all;
		result = MatchFromList(host, text, list, out prefix);
		if (text != text2 && result == null)
		{
			result = MatchFromList(host, text2, list, out prefix);
		}
		if (result != null)
		{
			return result;
		}
		return null;
	}

	private HttpListener MatchFromList(string host, string path, ArrayList list, out ListenerPrefix prefix)
	{
		prefix = null;
		if (list == null)
		{
			return null;
		}
		HttpListener result = null;
		int num = -1;
		foreach (ListenerPrefix item in list)
		{
			string path2 = item.Path;
			if (path2.Length >= num && path.StartsWith(path2))
			{
				num = path2.Length;
				result = item.Listener;
				prefix = item;
			}
		}
		return result;
	}

	private void AddSpecial(ArrayList coll, ListenerPrefix prefix)
	{
		if (coll == null)
		{
			return;
		}
		foreach (ListenerPrefix item in coll)
		{
			if (item.Path == prefix.Path)
			{
				throw new HttpListenerException(400, "Prefix already in use.");
			}
		}
		coll.Add(prefix);
	}

	private bool RemoveSpecial(ArrayList coll, ListenerPrefix prefix)
	{
		if (coll == null)
		{
			return false;
		}
		int count = coll.Count;
		for (int i = 0; i < count; i++)
		{
			if (((ListenerPrefix)coll[i]).Path == prefix.Path)
			{
				coll.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	private void CheckIfRemove()
	{
		if (prefixes.Count > 0)
		{
			return;
		}
		ArrayList arrayList = unhandled;
		if (arrayList == null || arrayList.Count <= 0)
		{
			arrayList = all;
			if (arrayList == null || arrayList.Count <= 0)
			{
				EndPointManager.RemoveEndPoint(this, endpoint);
			}
		}
	}

	public void Close()
	{
		sock.Close();
		lock (unregistered)
		{
			foreach (HttpConnection item in new List<HttpConnection>(unregistered.Keys))
			{
				item.Close(force_close: true);
			}
			unregistered.Clear();
		}
	}

	public void AddPrefix(ListenerPrefix prefix, HttpListener listener)
	{
		if (prefix.Host == "*")
		{
			ArrayList arrayList;
			ArrayList arrayList2;
			do
			{
				arrayList = unhandled;
				arrayList2 = ((arrayList != null) ? ((ArrayList)arrayList.Clone()) : new ArrayList());
				prefix.Listener = listener;
				AddSpecial(arrayList2, prefix);
			}
			while (Interlocked.CompareExchange(ref unhandled, arrayList2, arrayList) != arrayList);
			return;
		}
		if (prefix.Host == "+")
		{
			ArrayList arrayList;
			ArrayList arrayList2;
			do
			{
				arrayList = all;
				arrayList2 = ((arrayList != null) ? ((ArrayList)arrayList.Clone()) : new ArrayList());
				prefix.Listener = listener;
				AddSpecial(arrayList2, prefix);
			}
			while (Interlocked.CompareExchange(ref all, arrayList2, arrayList) != arrayList);
			return;
		}
		Hashtable hashtable;
		Hashtable hashtable2;
		do
		{
			hashtable = prefixes;
			if (hashtable.ContainsKey(prefix))
			{
				if ((HttpListener)hashtable[prefix] != listener)
				{
					throw new HttpListenerException(400, "There's another listener for " + prefix);
				}
				break;
			}
			hashtable2 = (Hashtable)hashtable.Clone();
			hashtable2[prefix] = listener;
		}
		while (Interlocked.CompareExchange(ref prefixes, hashtable2, hashtable) != hashtable);
	}

	public void RemovePrefix(ListenerPrefix prefix, HttpListener listener)
	{
		if (prefix.Host == "*")
		{
			ArrayList arrayList;
			ArrayList arrayList2;
			do
			{
				arrayList = unhandled;
				arrayList2 = ((arrayList != null) ? ((ArrayList)arrayList.Clone()) : new ArrayList());
			}
			while (RemoveSpecial(arrayList2, prefix) && Interlocked.CompareExchange(ref unhandled, arrayList2, arrayList) != arrayList);
			CheckIfRemove();
			return;
		}
		if (prefix.Host == "+")
		{
			ArrayList arrayList;
			ArrayList arrayList2;
			do
			{
				arrayList = all;
				arrayList2 = ((arrayList != null) ? ((ArrayList)arrayList.Clone()) : new ArrayList());
			}
			while (RemoveSpecial(arrayList2, prefix) && Interlocked.CompareExchange(ref all, arrayList2, arrayList) != arrayList);
			CheckIfRemove();
			return;
		}
		Hashtable hashtable;
		Hashtable hashtable2;
		do
		{
			hashtable = prefixes;
			if (!hashtable.ContainsKey(prefix))
			{
				break;
			}
			hashtable2 = (Hashtable)hashtable.Clone();
			hashtable2.Remove(prefix);
		}
		while (Interlocked.CompareExchange(ref prefixes, hashtable2, hashtable) != hashtable);
		CheckIfRemove();
	}
}
