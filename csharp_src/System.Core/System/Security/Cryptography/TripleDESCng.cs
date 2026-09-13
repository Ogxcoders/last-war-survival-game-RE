namespace System.Security.Cryptography;

public sealed class TripleDESCng : TripleDES
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

	public TripleDESCng()
	{
		throw new NotImplementedException();
	}

	public TripleDESCng(string keyName)
	{
		throw new NotImplementedException();
	}

	public TripleDESCng(string keyName, CngProvider provider)
	{
		throw new NotImplementedException();
	}

	public TripleDESCng(string keyName, CngProvider provider, CngKeyOpenOptions openOptions)
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
