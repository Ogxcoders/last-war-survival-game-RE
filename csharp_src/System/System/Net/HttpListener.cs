using System.Collections;
using System.IO;
using System.Net.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Mono.Net.Security.Private;
using Mono.Security.Authenticode;
using Mono.Security.Interface;

namespace System.Net;

public sealed class HttpListener : IDisposable
{
	public delegate ExtendedProtectionPolicy ExtendedProtectionSelector(HttpListenerRequest request);

	private MonoTlsProvider tlsProvider;

	private MonoTlsSettings tlsSettings;

	private X509Certificate certificate;

	private AuthenticationSchemes auth_schemes;

	private HttpListenerPrefixCollection prefixes;

	private AuthenticationSchemeSelector auth_selector;

	private string realm;

	private bool ignore_write_exceptions;

	private bool unsafe_ntlm_auth;

	private bool listening;

	private bool disposed;

	private readonly object _internalLock;

	private Hashtable registry;

	private ArrayList ctx_queue;

	private ArrayList wait_queue;

	private Hashtable connections;

	private ServiceNameStore defaultServiceNames;

	private ExtendedProtectionPolicy extendedProtectionPolicy;

	private ExtendedProtectionSelector extendedProtectionSelectorDelegate;

	public AuthenticationSchemes AuthenticationSchemes
	{
		get
		{
			return auth_schemes;
		}
		set
		{
			CheckDisposed();
			auth_schemes = value;
		}
	}

	public AuthenticationSchemeSelector AuthenticationSchemeSelectorDelegate
	{
		get
		{
			return auth_selector;
		}
		set
		{
			CheckDisposed();
			auth_selector = value;
		}
	}

	public ExtendedProtectionSelector ExtendedProtectionSelectorDelegate
	{
		get
		{
			return extendedProtectionSelectorDelegate;
		}
		set
		{
			CheckDisposed();
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			if (!AuthenticationManager.OSSupportsExtendedProtection)
			{
				throw new PlatformNotSupportedException(global::SR.GetString("This operation requires OS support for extended protection."));
			}
			extendedProtectionSelectorDelegate = value;
		}
	}

	public bool IgnoreWriteExceptions
	{
		get
		{
			return ignore_write_exceptions;
		}
		set
		{
			CheckDisposed();
			ignore_write_exceptions = value;
		}
	}

	public bool IsListening => listening;

	public static bool IsSupported => true;

	public HttpListenerPrefixCollection Prefixes
	{
		get
		{
			CheckDisposed();
			return prefixes;
		}
	}

	[System.MonoTODO]
	public HttpListenerTimeoutManager TimeoutManager
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO("not used anywhere in the implementation")]
	public ExtendedProtectionPolicy ExtendedProtectionPolicy
	{
		get
		{
			return extendedProtectionPolicy;
		}
		set
		{
			CheckDisposed();
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!AuthenticationManager.OSSupportsExtendedProtection && value.PolicyEnforcement == PolicyEnforcement.Always)
			{
				throw new PlatformNotSupportedException(global::SR.GetString("This operation requires OS support for extended protection."));
			}
			if (value.CustomChannelBinding != null)
			{
				throw new ArgumentException(global::SR.GetString("Custom channel bindings are not supported."), "CustomChannelBinding");
			}
			extendedProtectionPolicy = value;
		}
	}

	public ServiceNameCollection DefaultServiceNames => defaultServiceNames.ServiceNames;

	public string Realm
	{
		get
		{
			return realm;
		}
		set
		{
			CheckDisposed();
			realm = value;
		}
	}

	[System.MonoTODO("Support for NTLM needs some loving.")]
	public bool UnsafeConnectionNtlmAuthentication
	{
		get
		{
			return unsafe_ntlm_auth;
		}
		set
		{
			CheckDisposed();
			unsafe_ntlm_auth = value;
		}
	}

	internal HttpListener(X509Certificate certificate, MonoTlsProvider tlsProvider, MonoTlsSettings tlsSettings)
		: this()
	{
		this.certificate = certificate;
		this.tlsProvider = tlsProvider;
		this.tlsSettings = tlsSettings;
	}

	internal X509Certificate LoadCertificateAndKey(IPAddress addr, int port)
	{
		lock (_internalLock)
		{
			if (certificate != null)
			{
				return certificate;
			}
			try
			{
				string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mono");
				path = Path.Combine(path, "httplistener");
				string text = Path.Combine(path, $"{port}.cer");
				if (!File.Exists(text))
				{
					return null;
				}
				string text2 = Path.Combine(path, $"{port}.pvk");
				if (!File.Exists(text2))
				{
					return null;
				}
				X509Certificate2 x509Certificate = new X509Certificate2(text);
				RSA rSA = PrivateKey.CreateFromFile(text2).RSA;
				certificate = new X509Certificate2((X509Certificate2Impl)x509Certificate.Impl.CopyWithPrivateKey(rSA));
				return certificate;
			}
			catch
			{
				certificate = null;
				return null;
			}
		}
	}

	internal SslStream CreateSslStream(Stream innerStream, bool ownsStream, RemoteCertificateValidationCallback callback)
	{
		lock (_internalLock)
		{
			if (tlsProvider == null)
			{
				tlsProvider = MonoTlsProviderFactory.GetProvider();
			}
			MonoTlsSettings monoTlsSettings = (tlsSettings ?? MonoTlsSettings.DefaultSettings).Clone();
			monoTlsSettings.RemoteCertificateValidationCallback = CallbackHelpers.PublicToMono(callback);
			return new SslStream(innerStream, ownsStream, tlsProvider, monoTlsSettings);
		}
	}

	public HttpListener()
	{
		_internalLock = new object();
		prefixes = new HttpListenerPrefixCollection(this);
		registry = new Hashtable();
		connections = Hashtable.Synchronized(new Hashtable());
		ctx_queue = new ArrayList();
		wait_queue = new ArrayList();
		auth_schemes = AuthenticationSchemes.Anonymous;
		defaultServiceNames = new ServiceNameStore();
		extendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
	}

	public void Abort()
	{
		if (!disposed && listening)
		{
			Close(force: true);
		}
	}

	public void Close()
	{
		if (!disposed)
		{
			if (!listening)
			{
				disposed = true;
				return;
			}
			Close(force: true);
			disposed = true;
		}
	}

	private void Close(bool force)
	{
		CheckDisposed();
		EndPointManager.RemoveListener(this);
		Cleanup(force);
	}

	private void Cleanup(bool close_existing)
	{
		lock (_internalLock)
		{
			if (close_existing)
			{
				ICollection keys = registry.Keys;
				HttpListenerContext[] array = new HttpListenerContext[keys.Count];
				keys.CopyTo(array, 0);
				registry.Clear();
				for (int num = array.Length - 1; num >= 0; num--)
				{
					array[num].Connection.Close(force_close: true);
				}
			}
			lock (connections.SyncRoot)
			{
				ICollection keys2 = connections.Keys;
				HttpConnection[] array2 = new HttpConnection[keys2.Count];
				keys2.CopyTo(array2, 0);
				connections.Clear();
				for (int num2 = array2.Length - 1; num2 >= 0; num2--)
				{
					array2[num2].Close(force_close: true);
				}
			}
			lock (ctx_queue)
			{
				HttpListenerContext[] array3 = (HttpListenerContext[])ctx_queue.ToArray(typeof(HttpListenerContext));
				ctx_queue.Clear();
				for (int num3 = array3.Length - 1; num3 >= 0; num3--)
				{
					array3[num3].Connection.Close(force_close: true);
				}
			}
			lock (wait_queue)
			{
				Exception exc = new ObjectDisposedException("listener");
				foreach (ListenerAsyncResult item in wait_queue)
				{
					item.Complete(exc);
				}
				wait_queue.Clear();
			}
		}
	}

	public IAsyncResult BeginGetContext(AsyncCallback callback, object state)
	{
		CheckDisposed();
		if (!listening)
		{
			throw new InvalidOperationException("Please, call Start before using this method.");
		}
		ListenerAsyncResult listenerAsyncResult = new ListenerAsyncResult(callback, state);
		lock (wait_queue)
		{
			lock (ctx_queue)
			{
				HttpListenerContext contextFromQueue = GetContextFromQueue();
				if (contextFromQueue != null)
				{
					listenerAsyncResult.Complete(contextFromQueue, synch: true);
					return listenerAsyncResult;
				}
			}
			wait_queue.Add(listenerAsyncResult);
			return listenerAsyncResult;
		}
	}

	public HttpListenerContext EndGetContext(IAsyncResult asyncResult)
	{
		CheckDisposed();
		if (asyncResult == null)
		{
			throw new ArgumentNullException("asyncResult");
		}
		if (!(asyncResult is ListenerAsyncResult listenerAsyncResult))
		{
			throw new ArgumentException("Wrong IAsyncResult.", "asyncResult");
		}
		if (listenerAsyncResult.EndCalled)
		{
			throw new ArgumentException("Cannot reuse this IAsyncResult");
		}
		listenerAsyncResult.EndCalled = true;
		if (!listenerAsyncResult.IsCompleted)
		{
			listenerAsyncResult.AsyncWaitHandle.WaitOne();
		}
		lock (wait_queue)
		{
			int num = wait_queue.IndexOf(listenerAsyncResult);
			if (num >= 0)
			{
				wait_queue.RemoveAt(num);
			}
		}
		HttpListenerContext context = listenerAsyncResult.GetContext();
		context.ParseAuthentication(SelectAuthenticationScheme(context));
		return context;
	}

	internal AuthenticationSchemes SelectAuthenticationScheme(HttpListenerContext context)
	{
		if (AuthenticationSchemeSelectorDelegate != null)
		{
			return AuthenticationSchemeSelectorDelegate(context.Request);
		}
		return auth_schemes;
	}

	public HttpListenerContext GetContext()
	{
		if (prefixes.Count == 0)
		{
			throw new InvalidOperationException("Please, call AddPrefix before using this method.");
		}
		ListenerAsyncResult listenerAsyncResult = (ListenerAsyncResult)BeginGetContext(null, null);
		listenerAsyncResult.InGet = true;
		return EndGetContext(listenerAsyncResult);
	}

	public void Start()
	{
		CheckDisposed();
		if (!listening)
		{
			EndPointManager.AddListener(this);
			listening = true;
		}
	}

	public void Stop()
	{
		CheckDisposed();
		listening = false;
		Close(force: false);
	}

	void IDisposable.Dispose()
	{
		if (!disposed)
		{
			Close(force: true);
			disposed = true;
		}
	}

	public Task<HttpListenerContext> GetContextAsync()
	{
		return Task<HttpListenerContext>.Factory.FromAsync(BeginGetContext, EndGetContext, null);
	}

	internal void CheckDisposed()
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
	}

	private HttpListenerContext GetContextFromQueue()
	{
		if (ctx_queue.Count == 0)
		{
			return null;
		}
		HttpListenerContext result = (HttpListenerContext)ctx_queue[0];
		ctx_queue.RemoveAt(0);
		return result;
	}

	internal void RegisterContext(HttpListenerContext context)
	{
		lock (_internalLock)
		{
			registry[context] = context;
		}
		ListenerAsyncResult listenerAsyncResult = null;
		lock (wait_queue)
		{
			if (wait_queue.Count == 0)
			{
				lock (ctx_queue)
				{
					ctx_queue.Add(context);
				}
			}
			else
			{
				listenerAsyncResult = (ListenerAsyncResult)wait_queue[0];
				wait_queue.RemoveAt(0);
			}
		}
		listenerAsyncResult?.Complete(context);
	}

	internal void UnregisterContext(HttpListenerContext context)
	{
		lock (_internalLock)
		{
			registry.Remove(context);
		}
		lock (ctx_queue)
		{
			int num = ctx_queue.IndexOf(context);
			if (num >= 0)
			{
				ctx_queue.RemoveAt(num);
			}
		}
	}

	internal void AddConnection(HttpConnection cnc)
	{
		connections[cnc] = cnc;
	}

	internal void RemoveConnection(HttpConnection cnc)
	{
		connections.Remove(cnc);
	}
}
