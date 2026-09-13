using System.IO;
using System.Security.Permissions;

namespace System.Security.Cryptography;

[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public sealed class ECDsaCng : ECDsa
{
	public CngAlgorithm HashAlgorithm { get; set; }

	public CngKey Key
	{
		get
		{
			throw new NotImplementedException();
		}
		private set
		{
			throw new NotImplementedException();
		}
	}

	public ECDsaCng()
		: this(521)
	{
	}

	public ECDsaCng(int keySize)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	public ECDsaCng(CngKey key)
	{
		throw new NotImplementedException();
	}

	public ECDsaCng(ECCurve curve)
	{
		throw new NotImplementedException();
	}

	public override byte[] SignHash(byte[] hash)
	{
		throw new NotImplementedException();
	}

	public override bool VerifyHash(byte[] hash, byte[] signature)
	{
		throw new NotImplementedException();
	}

	public void FromXmlString(string xml, ECKeyXmlFormat format)
	{
		throw new NotImplementedException();
	}

	public byte[] SignData(byte[] data)
	{
		throw new NotImplementedException();
	}

	public byte[] SignData(Stream data)
	{
		throw new NotImplementedException();
	}

	public byte[] SignData(byte[] data, int offset, int count)
	{
		throw new NotImplementedException();
	}

	public string ToXmlString(ECKeyXmlFormat format)
	{
		throw new NotImplementedException();
	}

	public bool VerifyData(byte[] data, byte[] signature)
	{
		throw new NotImplementedException();
	}

	public bool VerifyData(Stream data, byte[] signature)
	{
		throw new NotImplementedException();
	}

	public bool VerifyData(byte[] data, int offset, int count, byte[] signature)
	{
		throw new NotImplementedException();
	}
}
