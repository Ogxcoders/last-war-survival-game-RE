namespace System.Security.Cryptography;

public sealed class AesCng : Aes
{
	public override byte[] Key
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public override int KeySize
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public AesCng()
	{
		throw new NotImplementedException();
	}

	public AesCng(string keyName)
	{
		throw new NotImplementedException();
	}

	public AesCng(string keyName, CngProvider provider)
	{
		throw new NotImplementedException();
	}

	public AesCng(string keyName, CngProvider provider, CngKeyOpenOptions openOptions)
	{
		throw new NotImplementedException();
	}

	public override ICryptoTransform CreateDecryptor()
	{
		throw new NotImplementedException();
	}

	public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
	{
		throw new NotImplementedException();
	}

	public override ICryptoTransform CreateEncryptor()
	{
		throw new NotImplementedException();
	}

	public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
	{
		return null;
	}

	protected override void Dispose(bool disposing)
	{
		throw new NotImplementedException();
	}

	public override void GenerateIV()
	{
		throw new NotImplementedException();
	}

	public override void GenerateKey()
	{
		throw new NotImplementedException();
	}
}
