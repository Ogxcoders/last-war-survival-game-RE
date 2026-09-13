using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Principal;
using System.Threading.Tasks;

namespace System.Net.Security;

public class NegotiateStream : AuthenticatedStream
{
	private int readTimeout;

	private int writeTimeout;

	public override bool CanRead => base.InnerStream.CanRead;

	public override bool CanSeek => base.InnerStream.CanSeek;

	[System.MonoTODO]
	public override bool CanTimeout
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool CanWrite => base.InnerStream.CanWrite;

	[System.MonoTODO]
	public virtual TokenImpersonationLevel ImpersonationLevel
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO]
	public override bool IsAuthenticated
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO]
	public override bool IsEncrypted
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO]
	public override bool IsMutuallyAuthenticated
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO]
	public override bool IsServer
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO]
	public override bool IsSigned
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override long Length => base.InnerStream.Length;

	public override long Position
	{
		get
		{
			return base.InnerStream.Position;
		}
		set
		{
			base.InnerStream.Position = value;
		}
	}

	public override int ReadTimeout
	{
		get
		{
			return readTimeout;
		}
		set
		{
			readTimeout = value;
		}
	}

	[System.MonoTODO]
	public virtual IIdentity RemoteIdentity
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override int WriteTimeout
	{
		get
		{
			return writeTimeout;
		}
		set
		{
			writeTimeout = value;
		}
	}

	[System.MonoTODO]
	public NegotiateStream(Stream innerStream)
		: base(innerStream, leaveInnerStreamOpen: false)
	{
	}

	[System.MonoTODO]
	public NegotiateStream(Stream innerStream, bool leaveInnerStreamOpen)
		: base(innerStream, leaveInnerStreamOpen)
	{
	}

	[System.MonoTODO]
	public virtual IAsyncResult BeginAuthenticateAsClient(AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName, AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, string targetName, AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual IAsyncResult BeginAuthenticateAsServer(AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual IAsyncResult BeginAuthenticateAsServer(NetworkCredential credential, ExtendedProtectionPolicy policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual IAsyncResult BeginAuthenticateAsServer(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual IAsyncResult BeginAuthenticateAsServer(ExtendedProtectionPolicy policy, AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual void AuthenticateAsClient()
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual void AuthenticateAsClient(NetworkCredential credential, string targetName)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual void AuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual void AuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual void AuthenticateAsClient(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual void AuthenticateAsServer()
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual void AuthenticateAsServer(ExtendedProtectionPolicy policy)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual void AuthenticateAsServer(NetworkCredential credential, ExtendedProtectionPolicy policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual void AuthenticateAsServer(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	protected override void Dispose(bool disposing)
	{
	}

	[System.MonoTODO]
	public virtual void EndAuthenticateAsClient(IAsyncResult asyncResult)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public override int EndRead(IAsyncResult asyncResult)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public virtual void EndAuthenticateAsServer(IAsyncResult asyncResult)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public override void EndWrite(IAsyncResult asyncResult)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public override void Flush()
	{
		base.InnerStream.Flush();
	}

	[System.MonoTODO]
	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotImplementedException();
	}

	public virtual Task AuthenticateAsClientAsync()
	{
		return Task.Factory.FromAsync(BeginAuthenticateAsClient, EndAuthenticateAsClient, null);
	}

	public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, string targetName)
	{
		return Task.Factory.FromAsync(BeginAuthenticateAsClient, EndAuthenticateAsClient, credential, targetName, null);
	}

	public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
	{
		return Task.Factory.FromAsync((AsyncCallback callback, object state) => BeginAuthenticateAsClient(credential, targetName, requiredProtectionLevel, allowedImpersonationLevel, callback, state), EndAuthenticateAsClient, null);
	}

	public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, ChannelBinding binding, string targetName)
	{
		throw new NotImplementedException();
	}

	public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, ChannelBinding binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
	{
		throw new NotImplementedException();
	}

	public virtual Task AuthenticateAsServerAsync()
	{
		return Task.Factory.FromAsync(BeginAuthenticateAsServer, EndAuthenticateAsServer, null);
	}

	public virtual Task AuthenticateAsServerAsync(ExtendedProtectionPolicy policy)
	{
		throw new NotImplementedException();
	}

	public virtual Task AuthenticateAsServerAsync(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
	{
		throw new NotImplementedException();
	}

	public virtual Task AuthenticateAsServerAsync(NetworkCredential credential, ExtendedProtectionPolicy policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
	{
		throw new NotImplementedException();
	}
}
