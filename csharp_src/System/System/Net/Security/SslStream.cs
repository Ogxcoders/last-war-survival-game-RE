using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Mono.Net.Security;
using Mono.Net.Security.Private;
using Mono.Security.Interface;

namespace System.Net.Security;

public class SslStream : AuthenticatedStream
{
	private MobileTlsProvider provider;

	private MonoTlsSettings settings;

	private RemoteCertificateValidationCallback validationCallback;

	private LocalCertificateSelectionCallback selectionCallback;

	private MobileAuthenticatedStream impl;

	private bool explicitSettings;

	internal MobileAuthenticatedStream Impl
	{
		get
		{
			CheckDisposed();
			return impl;
		}
	}

	internal MonoTlsProvider Provider
	{
		get
		{
			CheckDisposed();
			return provider;
		}
	}

	internal string InternalTargetHost
	{
		get
		{
			CheckDisposed();
			return impl.TargetHost;
		}
	}

	public TransportContext TransportContext => null;

	public override bool IsAuthenticated => Impl.IsAuthenticated;

	public override bool IsMutuallyAuthenticated => Impl.IsMutuallyAuthenticated;

	public override bool IsEncrypted => Impl.IsEncrypted;

	public override bool IsSigned => Impl.IsSigned;

	public override bool IsServer => Impl.IsServer;

	public virtual SslProtocols SslProtocol => Impl.SslProtocol;

	public virtual bool CheckCertRevocationStatus => Impl.CheckCertRevocationStatus;

	public virtual X509Certificate LocalCertificate => Impl.LocalCertificate;

	public virtual X509Certificate RemoteCertificate => Impl.RemoteCertificate;

	public virtual System.Security.Authentication.CipherAlgorithmType CipherAlgorithm => Impl.CipherAlgorithm;

	public virtual int CipherStrength => Impl.CipherStrength;

	public virtual System.Security.Authentication.HashAlgorithmType HashAlgorithm => Impl.HashAlgorithm;

	public virtual int HashStrength => Impl.HashStrength;

	public virtual System.Security.Authentication.ExchangeAlgorithmType KeyExchangeAlgorithm => Impl.KeyExchangeAlgorithm;

	public virtual int KeyExchangeStrength => Impl.KeyExchangeStrength;

	public SslApplicationProtocol NegotiatedApplicationProtocol
	{
		get
		{
			throw new PlatformNotSupportedException("https://github.com/mono/mono/issues/12880");
		}
	}

	public override bool CanSeek => false;

	public override bool CanRead
	{
		get
		{
			if (impl != null)
			{
				return impl.CanRead;
			}
			return false;
		}
	}

	public override bool CanTimeout => base.InnerStream.CanTimeout;

	public override bool CanWrite
	{
		get
		{
			if (impl != null)
			{
				return impl.CanWrite;
			}
			return false;
		}
	}

	public override int ReadTimeout
	{
		get
		{
			return Impl.ReadTimeout;
		}
		set
		{
			Impl.ReadTimeout = value;
		}
	}

	public override int WriteTimeout
	{
		get
		{
			return Impl.WriteTimeout;
		}
		set
		{
			Impl.WriteTimeout = value;
		}
	}

	public override long Length => Impl.Length;

	public override long Position
	{
		get
		{
			return Impl.Position;
		}
		set
		{
			throw new NotSupportedException(global::SR.GetString("This stream does not support seek operations."));
		}
	}

	private static MobileTlsProvider GetProvider()
	{
		return (MobileTlsProvider)Mono.Security.Interface.MonoTlsProviderFactory.GetProvider();
	}

	public SslStream(Stream innerStream)
		: this(innerStream, leaveInnerStreamOpen: false)
	{
	}

	public SslStream(Stream innerStream, bool leaveInnerStreamOpen)
		: base(innerStream, leaveInnerStreamOpen)
	{
		provider = GetProvider();
		settings = MonoTlsSettings.CopyDefaultSettings();
		impl = provider.CreateSslStream(this, innerStream, leaveInnerStreamOpen, settings);
	}

	public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback)
		: this(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, null)
	{
	}

	public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback)
		: base(innerStream, leaveInnerStreamOpen)
	{
		provider = GetProvider();
		settings = MonoTlsSettings.CopyDefaultSettings();
		SetAndVerifyValidationCallback(userCertificateValidationCallback);
		SetAndVerifySelectionCallback(userCertificateSelectionCallback);
		impl = provider.CreateSslStream(this, innerStream, leaveInnerStreamOpen, settings);
	}

	[System.MonoLimitation("encryptionPolicy is ignored")]
	public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback, EncryptionPolicy encryptionPolicy)
		: this(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback)
	{
	}

	internal SslStream(Stream innerStream, bool leaveInnerStreamOpen, MonoTlsProvider provider, MonoTlsSettings settings)
		: base(innerStream, leaveInnerStreamOpen)
	{
		this.provider = (MobileTlsProvider)provider;
		this.settings = settings.Clone();
		explicitSettings = true;
		impl = this.provider.CreateSslStream(this, innerStream, leaveInnerStreamOpen, settings);
	}

	internal static IMonoSslStream CreateMonoSslStream(Stream innerStream, bool leaveInnerStreamOpen, MobileTlsProvider provider, MonoTlsSettings settings)
	{
		return new SslStream(innerStream, leaveInnerStreamOpen, provider, settings).Impl;
	}

	private void SetAndVerifyValidationCallback(RemoteCertificateValidationCallback callback)
	{
		if (validationCallback == null)
		{
			validationCallback = callback;
			settings.RemoteCertificateValidationCallback = CallbackHelpers.PublicToMono(callback);
		}
		else if ((callback != null && validationCallback != callback) || (explicitSettings & (settings.RemoteCertificateValidationCallback != null)))
		{
			throw new InvalidOperationException(global::SR.Format("The '{0}' option was already set in the SslStream constructor.", "RemoteCertificateValidationCallback"));
		}
	}

	private void SetAndVerifySelectionCallback(LocalCertificateSelectionCallback callback)
	{
		if (selectionCallback == null)
		{
			selectionCallback = callback;
			if (callback == null)
			{
				settings.ClientCertificateSelectionCallback = null;
				return;
			}
			settings.ClientCertificateSelectionCallback = (string t, X509CertificateCollection lc, X509Certificate rc, string[] ai) => callback(this, t, lc, rc, ai);
		}
		else if ((callback != null && selectionCallback != callback) || (explicitSettings && settings.ClientCertificateSelectionCallback != null))
		{
			throw new InvalidOperationException(global::SR.Format("The '{0}' option was already set in the SslStream constructor.", "LocalCertificateSelectionCallback"));
		}
	}

	private MonoSslServerAuthenticationOptions CreateAuthenticationOptions(SslServerAuthenticationOptions sslServerAuthenticationOptions)
	{
		if (sslServerAuthenticationOptions.ServerCertificate == null && sslServerAuthenticationOptions.ServerCertificateSelectionCallback == null && selectionCallback == null)
		{
			throw new ArgumentNullException("ServerCertificate");
		}
		if ((sslServerAuthenticationOptions.ServerCertificate != null || selectionCallback != null) && sslServerAuthenticationOptions.ServerCertificateSelectionCallback != null)
		{
			throw new InvalidOperationException(global::SR.Format("The '{0}' option was already set in the SslStream constructor.", "ServerCertificateSelectionCallback"));
		}
		MonoSslServerAuthenticationOptions monoSslServerAuthenticationOptions = new MonoSslServerAuthenticationOptions(sslServerAuthenticationOptions);
		ServerCertificateSelectionCallback serverSelectionCallback = sslServerAuthenticationOptions.ServerCertificateSelectionCallback;
		if (serverSelectionCallback != null)
		{
			monoSslServerAuthenticationOptions.ServerCertSelectionDelegate = (string x) => serverSelectionCallback(this, x);
		}
		return monoSslServerAuthenticationOptions;
	}

	public virtual void AuthenticateAsClient(string targetHost)
	{
		AuthenticateAsClient(targetHost, new X509CertificateCollection(), SslProtocols.None, checkCertificateRevocation: false);
	}

	public virtual void AuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, bool checkCertificateRevocation)
	{
		AuthenticateAsClient(targetHost, clientCertificates, SslProtocols.None, checkCertificateRevocation);
	}

	public virtual void AuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
	{
		Impl.AuthenticateAsClient(targetHost, clientCertificates, enabledSslProtocols, checkCertificateRevocation);
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, AsyncCallback asyncCallback, object asyncState)
	{
		return BeginAuthenticateAsClient(targetHost, new X509CertificateCollection(), SslProtocols.None, checkCertificateRevocation: false, asyncCallback, asyncState);
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
	{
		return BeginAuthenticateAsClient(targetHost, clientCertificates, SslProtocols.None, checkCertificateRevocation, asyncCallback, asyncState);
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
	{
		return TaskToApm.Begin(Impl.AuthenticateAsClientAsync(targetHost, clientCertificates, enabledSslProtocols, checkCertificateRevocation), asyncCallback, asyncState);
	}

	public virtual void EndAuthenticateAsClient(IAsyncResult asyncResult)
	{
		TaskToApm.End(asyncResult);
	}

	public virtual void AuthenticateAsServer(X509Certificate serverCertificate)
	{
		Impl.AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, SslProtocols.None, checkCertificateRevocation: false);
	}

	public virtual void AuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, bool checkCertificateRevocation)
	{
		Impl.AuthenticateAsServer(serverCertificate, clientCertificateRequired, SslProtocols.None, checkCertificateRevocation);
	}

	public virtual void AuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
	{
		Impl.AuthenticateAsServer(serverCertificate, clientCertificateRequired, enabledSslProtocols, checkCertificateRevocation);
	}

	public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, AsyncCallback asyncCallback, object asyncState)
	{
		return BeginAuthenticateAsServer(serverCertificate, clientCertificateRequired: false, SslProtocols.None, checkCertificateRevocation: false, asyncCallback, asyncState);
	}

	public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
	{
		return BeginAuthenticateAsServer(serverCertificate, clientCertificateRequired, SslProtocols.None, checkCertificateRevocation, asyncCallback, asyncState);
	}

	public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
	{
		return TaskToApm.Begin(Impl.AuthenticateAsServerAsync(serverCertificate, clientCertificateRequired, enabledSslProtocols, checkCertificateRevocation), asyncCallback, asyncState);
	}

	public virtual void EndAuthenticateAsServer(IAsyncResult asyncResult)
	{
		TaskToApm.End(asyncResult);
	}

	public virtual Task AuthenticateAsClientAsync(string targetHost)
	{
		return Impl.AuthenticateAsClientAsync(targetHost, new X509CertificateCollection(), SslProtocols.None, checkCertificateRevocation: false);
	}

	public virtual Task AuthenticateAsClientAsync(string targetHost, X509CertificateCollection clientCertificates, bool checkCertificateRevocation)
	{
		return Impl.AuthenticateAsClientAsync(targetHost, clientCertificates, SslProtocols.None, checkCertificateRevocation);
	}

	public virtual Task AuthenticateAsClientAsync(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
	{
		return Impl.AuthenticateAsClientAsync(targetHost, clientCertificates, enabledSslProtocols, checkCertificateRevocation);
	}

	public Task AuthenticateAsClientAsync(SslClientAuthenticationOptions sslClientAuthenticationOptions, CancellationToken cancellationToken)
	{
		SetAndVerifyValidationCallback(sslClientAuthenticationOptions.RemoteCertificateValidationCallback);
		SetAndVerifySelectionCallback(sslClientAuthenticationOptions.LocalCertificateSelectionCallback);
		return Impl.AuthenticateAsClientAsync(new MonoSslClientAuthenticationOptions(sslClientAuthenticationOptions), cancellationToken);
	}

	public virtual Task AuthenticateAsServerAsync(X509Certificate serverCertificate)
	{
		return Impl.AuthenticateAsServerAsync(serverCertificate, clientCertificateRequired: false, SslProtocols.None, checkCertificateRevocation: false);
	}

	public virtual Task AuthenticateAsServerAsync(X509Certificate serverCertificate, bool clientCertificateRequired, bool checkCertificateRevocation)
	{
		return Impl.AuthenticateAsServerAsync(serverCertificate, clientCertificateRequired, SslProtocols.None, checkCertificateRevocation);
	}

	public virtual Task AuthenticateAsServerAsync(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
	{
		return Impl.AuthenticateAsServerAsync(serverCertificate, clientCertificateRequired, enabledSslProtocols, checkCertificateRevocation);
	}

	public Task AuthenticateAsServerAsync(SslServerAuthenticationOptions sslServerAuthenticationOptions, CancellationToken cancellationToken)
	{
		return Impl.AuthenticateAsServerAsync(CreateAuthenticationOptions(sslServerAuthenticationOptions), cancellationToken);
	}

	public virtual Task ShutdownAsync()
	{
		return Impl.ShutdownAsync();
	}

	public override void SetLength(long value)
	{
		Impl.SetLength(value);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException(global::SR.GetString("This stream does not support seek operations."));
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		return base.InnerStream.FlushAsync(cancellationToken);
	}

	public override void Flush()
	{
		base.InnerStream.Flush();
	}

	private void CheckDisposed()
	{
		if (impl == null)
		{
			throw new ObjectDisposedException("SslStream");
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (impl != null && disposing)
			{
				impl.Dispose();
				impl = null;
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return Impl.Read(buffer, offset, count);
	}

	public void Write(byte[] buffer)
	{
		Impl.Write(buffer);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		Impl.Write(buffer, offset, count);
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		return Impl.ReadAsync(buffer, offset, count, cancellationToken);
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		return Impl.WriteAsync(buffer, offset, count, cancellationToken);
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return TaskToApm.Begin(Impl.ReadAsync(buffer, offset, count), callback, state);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		return TaskToApm.End<int>(asyncResult);
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return TaskToApm.Begin(Impl.WriteAsync(buffer, offset, count), callback, state);
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		TaskToApm.End(asyncResult);
	}
}
