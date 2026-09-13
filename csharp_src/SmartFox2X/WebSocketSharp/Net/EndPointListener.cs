using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace WebSocketSharp.Net;

internal sealed class EndPointListener
{
	private List<HttpListenerPrefix> _all;

	private static readonly string _defaultCertFolderPath;

	private IPEndPoint _endpoint;

	private Dictionary<HttpListenerPrefix, HttpListener> _prefixes;

	private bool _secure;

	private Socket _socket;

	private ServerSslConfiguration _sslConfig;

	private List<HttpListenerPrefix> _unhandled;

	private Dictionary<HttpConnection, HttpConnection> _unregistered;

	private object _unregisteredSync;

	public IPAddress Address => _endpoint.Address;

	public bool IsSecure => _secure;

	public int Port => _endpoint.Port;

	public ServerSslConfiguration SslConfiguration => _sslConfig;

	static EndPointListener()
	{
		_defaultCertFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
	}

	internal EndPointListener(IPAddress address, int port, bool secure, string certificateFolderPath, ServerSslConfiguration sslConfig, bool reuseAddress)
	{
		if (secure)
		{
			X509Certificate2 certificate = getCertificate(port, certificateFolderPath, sslConfig.ServerCertificate);
			if (certificate == null)
			{
				throw new ArgumentException("No server certificate could be found.");
			}
			_secure = secure;
			_sslConfig = sslConfig;
			_sslConfig.ServerCertificate = certificate;
		}
		_prefixes = new Dictionary<HttpListenerPrefix, HttpListener>();
		_unregistered = new Dictionary<HttpConnection, HttpConnection>();
		_unregisteredSync = ((ICollection)_unregistered).SyncRoot;
		_socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		if (reuseAddress)
		{
			_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
		}
		_endpoint = new IPEndPoint(address, port);
		_socket.Bind(_endpoint);
		_socket.Listen(500);
		SocketAsyncEventArgs e = new SocketAsyncEventArgs();
		e.UserToken = this;
		e.Completed += onAccept;
		if (!_socket.AcceptAsync(e))
		{
			onAccept(this, e);
		}
	}

	private static void addSpecial(List<HttpListenerPrefix> prefixes, HttpListenerPrefix prefix)
	{
		string path = prefix.Path;
		foreach (HttpListenerPrefix prefix2 in prefixes)
		{
			if (prefix2.Path == path)
			{
				throw new HttpListenerException(400, "The prefix is already in use.");
			}
		}
		prefixes.Add(prefix);
	}

	private void checkIfRemove()
	{
		if (_prefixes.Count > 0)
		{
			return;
		}
		List<HttpListenerPrefix> unhandled = _unhandled;
		if (unhandled == null || unhandled.Count <= 0)
		{
			unhandled = _all;
			if (unhandled == null || unhandled.Count <= 0)
			{
				EndPointManager.RemoveEndPoint(this);
			}
		}
	}

	private static RSACryptoServiceProvider createRSAFromFile(string filename)
	{
		byte[] array = null;
		using (FileStream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
		}
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.ImportCspBlob(array);
		return rSACryptoServiceProvider;
	}

	private static X509Certificate2 getCertificate(int port, string certificateFolderPath, X509Certificate2 defaultCertificate)
	{
		if (certificateFolderPath == null || certificateFolderPath.Length == 0)
		{
			certificateFolderPath = _defaultCertFolderPath;
		}
		try
		{
			string text = Path.Combine(certificateFolderPath, $"{port}.cer");
			string text2 = Path.Combine(certificateFolderPath, $"{port}.key");
			if (File.Exists(text) && File.Exists(text2))
			{
				return new X509Certificate2(text)
				{
					PrivateKey = createRSAFromFile(text2)
				};
			}
		}
		catch
		{
		}
		return defaultCertificate;
	}

	private static HttpListener matchFromList(string host, string path, List<HttpListenerPrefix> list, out HttpListenerPrefix prefix)
	{
		prefix = null;
		if (list == null)
		{
			return null;
		}
		HttpListener result = null;
		int num = -1;
		foreach (HttpListenerPrefix item in list)
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

	private static void onAccept(object sender, EventArgs e)
	{
		SocketAsyncEventArgs e2 = (SocketAsyncEventArgs)e;
		EndPointListener endPointListener = (EndPointListener)e2.UserToken;
		Socket socket = null;
		if (e2.SocketError == SocketError.Success)
		{
			socket = e2.AcceptSocket;
			e2.AcceptSocket = null;
		}
		bool flag = true;
		try
		{
			flag = endPointListener._socket.AcceptAsync(e2);
		}
		catch
		{
			socket?.Close();
			return;
		}
		if (socket != null)
		{
			processAccepted(socket, endPointListener);
		}
		if (!flag)
		{
			onAccept(sender, e);
		}
	}

	private static void processAccepted(Socket socket, EndPointListener listener)
	{
		HttpConnection httpConnection = null;
		try
		{
			httpConnection = new HttpConnection(socket, listener);
			lock (listener._unregisteredSync)
			{
				listener._unregistered[httpConnection] = httpConnection;
			}
			httpConnection.BeginReadRequest();
		}
		catch
		{
			if (httpConnection != null)
			{
				httpConnection.Close(force: true);
			}
			else
			{
				socket.Close();
			}
		}
	}

	private static bool removeSpecial(List<HttpListenerPrefix> prefixes, HttpListenerPrefix prefix)
	{
		string path = prefix.Path;
		int count = prefixes.Count;
		for (int i = 0; i < count; i++)
		{
			if (prefixes[i].Path == path)
			{
				prefixes.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	private HttpListener searchListener(Uri uri, out HttpListenerPrefix prefix)
	{
		prefix = null;
		if (uri == null)
		{
			return null;
		}
		string host = uri.Host;
		int port = uri.Port;
		string text = HttpUtility.UrlDecode(uri.AbsolutePath);
		string text2 = ((text[text.Length - 1] == '/') ? text : (text + "/"));
		HttpListener result = null;
		int num = -1;
		if (host != null && host.Length > 0)
		{
			foreach (HttpListenerPrefix key in _prefixes.Keys)
			{
				string path = key.Path;
				if (path.Length >= num && !(key.Host != host) && key.Port == port && (text.StartsWith(path) || text2.StartsWith(path)))
				{
					num = path.Length;
					result = _prefixes[key];
					prefix = key;
				}
			}
			if (num != -1)
			{
				return result;
			}
		}
		List<HttpListenerPrefix> unhandled = _unhandled;
		result = matchFromList(host, text, unhandled, out prefix);
		if (text != text2 && result == null)
		{
			result = matchFromList(host, text2, unhandled, out prefix);
		}
		if (result != null)
		{
			return result;
		}
		unhandled = _all;
		result = matchFromList(host, text, unhandled, out prefix);
		if (text != text2 && result == null)
		{
			result = matchFromList(host, text2, unhandled, out prefix);
		}
		if (result != null)
		{
			return result;
		}
		return null;
	}

	internal static bool CertificateExists(int port, string certificateFolderPath)
	{
		if (certificateFolderPath == null || certificateFolderPath.Length == 0)
		{
			certificateFolderPath = _defaultCertFolderPath;
		}
		string path = Path.Combine(certificateFolderPath, $"{port}.cer");
		string path2 = Path.Combine(certificateFolderPath, $"{port}.key");
		if (File.Exists(path))
		{
			return File.Exists(path2);
		}
		return false;
	}

	internal void RemoveConnection(HttpConnection connection)
	{
		lock (_unregisteredSync)
		{
			_unregistered.Remove(connection);
		}
	}

	public void AddPrefix(HttpListenerPrefix prefix, HttpListener listener)
	{
		if (prefix.Host == "*")
		{
			List<HttpListenerPrefix> unhandled;
			List<HttpListenerPrefix> list;
			do
			{
				unhandled = _unhandled;
				list = ((unhandled != null) ? new List<HttpListenerPrefix>(unhandled) : new List<HttpListenerPrefix>());
				prefix.Listener = listener;
				addSpecial(list, prefix);
			}
			while (Interlocked.CompareExchange(ref _unhandled, list, unhandled) != unhandled);
			return;
		}
		if (prefix.Host == "+")
		{
			List<HttpListenerPrefix> all;
			List<HttpListenerPrefix> list2;
			do
			{
				all = _all;
				list2 = ((all != null) ? new List<HttpListenerPrefix>(all) : new List<HttpListenerPrefix>());
				prefix.Listener = listener;
				addSpecial(list2, prefix);
			}
			while (Interlocked.CompareExchange(ref _all, list2, all) != all);
			return;
		}
		Dictionary<HttpListenerPrefix, HttpListener> prefixes;
		Dictionary<HttpListenerPrefix, HttpListener> dictionary;
		do
		{
			prefixes = _prefixes;
			if (prefixes.ContainsKey(prefix))
			{
				if (prefixes[prefix] != listener)
				{
					throw new HttpListenerException(400, $"There's another listener for {prefix}.");
				}
				break;
			}
			dictionary = new Dictionary<HttpListenerPrefix, HttpListener>(prefixes);
			dictionary[prefix] = listener;
		}
		while (Interlocked.CompareExchange(ref _prefixes, dictionary, prefixes) != prefixes);
	}

	public bool BindContext(HttpListenerContext context)
	{
		HttpListenerPrefix prefix;
		HttpListener httpListener = searchListener(context.Request.Url, out prefix);
		if (httpListener == null)
		{
			return false;
		}
		context.Listener = httpListener;
		context.Connection.Prefix = prefix;
		return true;
	}

	public void Close()
	{
		_socket.Close();
		lock (_unregisteredSync)
		{
			List<HttpConnection> list = new List<HttpConnection>(_unregistered.Keys);
			_unregistered.Clear();
			foreach (HttpConnection item in list)
			{
				item.Close(force: true);
			}
			list.Clear();
		}
	}

	public void RemovePrefix(HttpListenerPrefix prefix, HttpListener listener)
	{
		if (prefix.Host == "*")
		{
			List<HttpListenerPrefix> unhandled;
			List<HttpListenerPrefix> list;
			do
			{
				unhandled = _unhandled;
				if (unhandled == null)
				{
					break;
				}
				list = new List<HttpListenerPrefix>(unhandled);
			}
			while (removeSpecial(list, prefix) && Interlocked.CompareExchange(ref _unhandled, list, unhandled) != unhandled);
			checkIfRemove();
			return;
		}
		if (prefix.Host == "+")
		{
			List<HttpListenerPrefix> all;
			List<HttpListenerPrefix> list2;
			do
			{
				all = _all;
				if (all == null)
				{
					break;
				}
				list2 = new List<HttpListenerPrefix>(all);
			}
			while (removeSpecial(list2, prefix) && Interlocked.CompareExchange(ref _all, list2, all) != all);
			checkIfRemove();
			return;
		}
		Dictionary<HttpListenerPrefix, HttpListener> prefixes;
		Dictionary<HttpListenerPrefix, HttpListener> dictionary;
		do
		{
			prefixes = _prefixes;
			if (!prefixes.ContainsKey(prefix))
			{
				break;
			}
			dictionary = new Dictionary<HttpListenerPrefix, HttpListener>(prefixes);
			dictionary.Remove(prefix);
		}
		while (Interlocked.CompareExchange(ref _prefixes, dictionary, prefixes) != prefixes);
		checkIfRemove();
	}

	public void UnbindContext(HttpListenerContext context)
	{
		if (context != null && context.Listener != null)
		{
			context.Listener.UnregisterContext(context);
		}
	}
}
