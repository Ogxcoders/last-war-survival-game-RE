using System.Security.Permissions;

namespace System.Security.Cryptography;

[Serializable]
[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public abstract class ECDiffieHellmanPublicKey : IDisposable
{
	private byte[] m_keyBlob;

	protected ECDiffieHellmanPublicKey()
	{
		m_keyBlob = new byte[0];
	}

	protected ECDiffieHellmanPublicKey(byte[] keyBlob)
	{
		if (keyBlob == null)
		{
			throw new ArgumentNullException("keyBlob");
		}
		m_keyBlob = keyBlob.Clone() as byte[];
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual byte[] ToByteArray()
	{
		return m_keyBlob.Clone() as byte[];
	}

	public virtual string ToXmlString()
	{
		throw new NotImplementedException(SR.GetString("Method not supported. Derived class must override."));
	}

	public virtual ECParameters ExportParameters()
	{
		throw new NotSupportedException(SR.GetString("Method not supported. Derived class must override."));
	}

	public virtual ECParameters ExportExplicitParameters()
	{
		throw new NotSupportedException(SR.GetString("Method not supported. Derived class must override."));
	}
}
