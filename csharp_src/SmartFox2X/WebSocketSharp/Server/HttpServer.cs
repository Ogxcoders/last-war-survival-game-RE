using System;
using System.IO;
using System.Security.Principal;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server;

public class HttpServer
{
	private HttpListener _listener;

	private Logger _logger;

	private int _port;

	private Thread _receiveThread;

	private string _rootPath;

	private bool _secure;

	private WebSocketServiceManager _services;

	private volatile ServerState _state;

	private object _sync;

	private bool _windows;

	public AuthenticationSchemes AuthenticationSchemes
	{
		get
		{
			return _listener.AuthenticationSchemes;
		}
		set
		{
			string text = _state.CheckIfStartable();
			if (text != null)
			{
				_logger.Error(text);
			}
			else
			{
				_listener.AuthenticationSchemes = value;
			}
		}
	}

	public bool IsListening => _state == ServerState.Start;

	public bool IsSecure => _secure;

	public bool KeepClean
	{
		get
		{
			return _services.KeepClean;
		}
		set
		{
			string text = _state.CheckIfStartable();
			if (text != null)
			{
				_logger.Error(text);
			}
			else
			{
				_services.KeepClean = value;
			}
		}
	}

	public Logger Log => _logger;

	public int Port => _port;

	public string Realm
	{
		get
		{
			return _listener.Realm;
		}
		set
		{
			string text = _state.CheckIfStartable();
			if (text != null)
			{
				_logger.Error(text);
			}
			else
			{
				_listener.Realm = value;
			}
		}
	}

	public bool ReuseAddress
	{
		get
		{
			return _listener.ReuseAddress;
		}
		set
		{
			string text = _state.CheckIfStartable();
			if (text != null)
			{
				_logger.Error(text);
			}
			else
			{
				_listener.ReuseAddress = value;
			}
		}
	}

	public string RootPath
	{
		get
		{
			if (_rootPath == null || _rootPath.Length <= 0)
			{
				return _rootPath = "./Public";
			}
			return _rootPath;
		}
		set
		{
			string text = _state.CheckIfStartable();
			if (text != null)
			{
				_logger.Error(text);
			}
			else
			{
				_rootPath = value;
			}
		}
	}

	public ServerSslConfiguration SslConfiguration
	{
		get
		{
			return _listener.SslConfiguration;
		}
		set
		{
			string text = _state.CheckIfStartable();
			if (text != null)
			{
				_logger.Error(text);
			}
			else
			{
				_listener.SslConfiguration = value;
			}
		}
	}

	public Func<IIdentity, NetworkCredential> UserCredentialsFinder
	{
		get
		{
			return _listener.UserCredentialsFinder;
		}
		set
		{
			string text = _state.CheckIfStartable();
			if (text != null)
			{
				_logger.Error(text);
			}
			else
			{
				_listener.UserCredentialsFinder = value;
			}
		}
	}

	public TimeSpan WaitTime
	{
		get
		{
			return _services.WaitTime;
		}
		set
		{
			string text = _state.CheckIfStartable() ?? value.CheckIfValidWaitTime();
			if (text != null)
			{
				_logger.Error(text);
			}
			else
			{
				_services.WaitTime = value;
			}
		}
	}

	public WebSocketServiceManager WebSocketServices => _services;

	public event EventHandler<HttpRequestEventArgs> OnConnect;

	public event EventHandler<HttpRequestEventArgs> OnDelete;

	public event EventHandler<HttpRequestEventArgs> OnGet;

	public event EventHandler<HttpRequestEventArgs> OnHead;

	public event EventHandler<HttpRequestEventArgs> OnOptions;

	public event EventHandler<HttpRequestEventArgs> OnPatch;

	public event EventHandler<HttpRequestEventArgs> OnPost;

	public event EventHandler<HttpRequestEventArgs> OnPut;

	public event EventHandler<HttpRequestEventArgs> OnTrace;

	public HttpServer()
		: this(80, secure: false)
	{
	}

	public HttpServer(int port)
		: this(port, port == 443)
	{
	}

	public HttpServer(int port, bool secure)
	{
		if (!port.IsPortNumber())
		{
			throw new ArgumentOutOfRangeException("port", "Not between 1 and 65535: " + port);
		}
		if ((port == 80 && secure) || (port == 443 && !secure))
		{
			throw new ArgumentException($"An invalid pair of 'port' and 'secure': {port}, {secure}");
		}
		_port = port;
		_secure = secure;
		_listener = new HttpListener();
		_logger = _listener.Log;
		_services = new WebSocketServiceManager(_logger);
		_state = ServerState.Ready;
		_sync = new object();
		OperatingSystem oSVersion = Environment.OSVersion;
		_windows = oSVersion.Platform != PlatformID.Unix && oSVersion.Platform != PlatformID.MacOSX;
		string uriPrefix = string.Format("http{0}://*:{1}/", _secure ? "s" : "", _port);
		_listener.Prefixes.Add(uriPrefix);
	}

	private void abort()
	{
		lock (_sync)
		{
			if (!IsListening)
			{
				return;
			}
			_state = ServerState.ShuttingDown;
		}
		_services.Stop(new CloseEventArgs(CloseStatusCode.ServerError), send: true, wait: false);
		_listener.Abort();
		_state = ServerState.Stop;
	}

	private string checkIfCertificateExists()
	{
		if (!_secure)
		{
			return null;
		}
		bool flag = _listener.SslConfiguration.ServerCertificate != null;
		bool flag2 = EndPointListener.CertificateExists(_port, _listener.CertificateFolderPath);
		if (flag && flag2)
		{
			_logger.Warn("The server certificate associated with the port number already exists.");
			return null;
		}
		if (flag || flag2)
		{
			return null;
		}
		return "The secure connection requires a server certificate.";
	}

	private void processRequest(HttpListenerContext context)
	{
		EventHandler<HttpRequestEventArgs> eventHandler = (EventHandler<HttpRequestEventArgs>)(context.Request.HttpMethod switch
		{
			"PATCH" => this.OnPatch, 
			"CONNECT" => this.OnConnect, 
			"TRACE" => this.OnTrace, 
			"OPTIONS" => this.OnOptions, 
			"DELETE" => this.OnDelete, 
			"PUT" => this.OnPut, 
			"POST" => this.OnPost, 
			"HEAD" => this.OnHead, 
			"GET" => this.OnGet, 
			_ => null, 
		});
		if (eventHandler != null)
		{
			eventHandler(this, new HttpRequestEventArgs(context));
		}
		else
		{
			context.Response.StatusCode = 501;
		}
		context.Response.Close();
	}

	private void processRequest(HttpListenerWebSocketContext context)
	{
		if (!_services.InternalTryGetServiceHost(context.RequestUri.AbsolutePath, out var host))
		{
			context.Close(HttpStatusCode.NotImplemented);
		}
		else
		{
			host.StartSession(context);
		}
	}

	private void receiveRequest()
	{
		while (true)
		{
			try
			{
				HttpListenerContext ctx = _listener.GetContext();
				ThreadPool.QueueUserWorkItem(delegate
				{
					try
					{
						if (ctx.Request.IsUpgradeTo("websocket"))
						{
							processRequest(ctx.AcceptWebSocket(null));
						}
						else
						{
							processRequest(ctx);
						}
					}
					catch (Exception ex3)
					{
						_logger.Fatal(ex3.ToString());
						ctx.Connection.Close(force: true);
					}
				});
			}
			catch (HttpListenerException ex)
			{
				_logger.Warn("Receiving has been stopped.\nreason: " + ex.Message);
				break;
			}
			catch (Exception ex2)
			{
				_logger.Fatal(ex2.ToString());
				break;
			}
		}
		if (IsListening)
		{
			abort();
		}
	}

	private void startReceiving()
	{
		_listener.Start();
		_receiveThread = new Thread(receiveRequest);
		_receiveThread.IsBackground = true;
		_receiveThread.Start();
	}

	private void stopReceiving(int millisecondsTimeout)
	{
		_listener.Close();
		_receiveThread.Join(millisecondsTimeout);
	}

	public void AddWebSocketService<TBehaviorWithNew>(string path) where TBehaviorWithNew : WebSocketBehavior, new()
	{
		AddWebSocketService(path, () => new TBehaviorWithNew());
	}

	public void AddWebSocketService<TBehavior>(string path, Func<TBehavior> initializer) where TBehavior : WebSocketBehavior
	{
		string text = path.CheckIfValidServicePath() ?? ((initializer == null) ? "'initializer' is null." : null);
		if (text != null)
		{
			_logger.Error(text);
		}
		else
		{
			_services.Add(path, initializer);
		}
	}

	public byte[] GetFile(string path)
	{
		path = RootPath + path;
		if (_windows)
		{
			path = path.Replace("/", "\\");
		}
		if (!File.Exists(path))
		{
			return null;
		}
		return File.ReadAllBytes(path);
	}

	public bool RemoveWebSocketService(string path)
	{
		string text = path.CheckIfValidServicePath();
		if (text != null)
		{
			_logger.Error(text);
			return false;
		}
		return _services.Remove(path);
	}

	public void Start()
	{
		lock (_sync)
		{
			string text = _state.CheckIfStartable() ?? checkIfCertificateExists();
			if (text != null)
			{
				_logger.Error(text);
				return;
			}
			_services.Start();
			startReceiving();
			_state = ServerState.Start;
		}
	}

	public void Stop()
	{
		lock (_sync)
		{
			string text = _state.CheckIfStart();
			if (text != null)
			{
				_logger.Error(text);
				return;
			}
			_state = ServerState.ShuttingDown;
		}
		_services.Stop(new CloseEventArgs(), send: true, wait: true);
		stopReceiving(5000);
		_state = ServerState.Stop;
	}

	public void Stop(ushort code, string reason)
	{
		lock (_sync)
		{
			string text = _state.CheckIfStart() ?? code.CheckIfValidCloseParameters(reason);
			if (text != null)
			{
				_logger.Error(text);
				return;
			}
			_state = ServerState.ShuttingDown;
		}
		if (code.IsNoStatusCode())
		{
			_services.Stop(new CloseEventArgs(), send: true, wait: true);
		}
		else
		{
			bool flag = !code.IsReserved();
			_services.Stop(new CloseEventArgs(code, reason), flag, flag);
		}
		stopReceiving(5000);
		_state = ServerState.Stop;
	}

	public void Stop(CloseStatusCode code, string reason)
	{
		lock (_sync)
		{
			string text = _state.CheckIfStart() ?? code.CheckIfValidCloseParameters(reason);
			if (text != null)
			{
				_logger.Error(text);
				return;
			}
			_state = ServerState.ShuttingDown;
		}
		if (code.IsNoStatusCode())
		{
			_services.Stop(new CloseEventArgs(), send: true, wait: true);
		}
		else
		{
			bool flag = !code.IsReserved();
			_services.Stop(new CloseEventArgs(code, reason), flag, flag);
		}
		stopReceiving(5000);
		_state = ServerState.Stop;
	}
}
