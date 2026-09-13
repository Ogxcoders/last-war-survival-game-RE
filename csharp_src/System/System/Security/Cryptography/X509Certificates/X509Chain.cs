using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography.X509Certificates;

public class X509Chain : IDisposable
{
	private X509ChainImpl impl;

	internal X509ChainImpl Impl
	{
		get
		{
			X509Helper2.ThrowIfContextInvalid(impl);
			return impl;
		}
	}

	internal bool IsValid => X509Helper2.IsValid(impl);

	[System.MonoTODO("Mono's X509Chain is fully managed. Always returns IntPtr.Zero.")]
	public IntPtr ChainContext
	{
		get
		{
			if (impl != null && impl.IsValid)
			{
				return impl.Handle;
			}
			return IntPtr.Zero;
		}
	}

	public X509ChainElementCollection ChainElements => Impl.ChainElements;

	public X509ChainPolicy ChainPolicy
	{
		get
		{
			return Impl.ChainPolicy;
		}
		set
		{
			Impl.ChainPolicy = value;
		}
	}

	public X509ChainStatus[] ChainStatus => Impl.ChainStatus;

	public SafeX509ChainHandle SafeHandle
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal void ThrowIfContextInvalid()
	{
		X509Helper2.ThrowIfContextInvalid(impl);
	}

	public X509Chain()
		: this(useMachineContext: false)
	{
	}

	public X509Chain(bool useMachineContext)
	{
		impl = X509Helper2.CreateChainImpl(useMachineContext);
	}

	internal X509Chain(X509ChainImpl impl)
	{
		X509Helper2.ThrowIfContextInvalid(impl);
		this.impl = impl;
	}

	[System.MonoTODO("Mono's X509Chain is fully managed. All handles are invalid.")]
	public X509Chain(IntPtr chainContext)
	{
		throw new NotSupportedException();
	}

	[System.MonoTODO("Not totally RFC3280 compliant, but neither is MS implementation...")]
	public bool Build(X509Certificate2 certificate)
	{
		return Impl.Build(certificate);
	}

	public void Reset()
	{
		Impl.Reset();
	}

	public static X509Chain Create()
	{
		return (X509Chain)CryptoConfig.CreateFromName("X509Chain");
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (impl != null)
		{
			impl.Dispose();
			impl = null;
		}
	}

	~X509Chain()
	{
		Dispose(disposing: false);
	}
}
