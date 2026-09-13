using System.Runtime.InteropServices;

namespace System.Security.Cryptography;

[ComVisible(true)]
public abstract class AsymmetricAlgorithm : IDisposable
{
	protected int KeySizeValue;

	protected KeySizes[] LegalKeySizesValue;

	public virtual int KeySize
	{
		get
		{
			return KeySizeValue;
		}
		set
		{
			for (int i = 0; i < LegalKeySizesValue.Length; i++)
			{
				if (LegalKeySizesValue[i].SkipSize == 0)
				{
					if (LegalKeySizesValue[i].MinSize == value)
					{
						KeySizeValue = value;
						return;
					}
					continue;
				}
				for (int j = LegalKeySizesValue[i].MinSize; j <= LegalKeySizesValue[i].MaxSize; j += LegalKeySizesValue[i].SkipSize)
				{
					if (j == value)
					{
						KeySizeValue = value;
						return;
					}
				}
			}
			throw new CryptographicException(Environment.GetResourceString("Specified key is not a valid size for this algorithm."));
		}
	}

	public virtual KeySizes[] LegalKeySizes => (KeySizes[])LegalKeySizesValue.Clone();

	public virtual string SignatureAlgorithm
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public virtual string KeyExchangeAlgorithm
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public void Dispose()
	{
		Clear();
	}

	public void Clear()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public static AsymmetricAlgorithm Create()
	{
		return Create("System.Security.Cryptography.AsymmetricAlgorithm");
	}

	public static AsymmetricAlgorithm Create(string algName)
	{
		return (AsymmetricAlgorithm)CryptoConfig.CreateFromName(algName);
	}

	public virtual void FromXmlString(string xmlString)
	{
		throw new NotImplementedException();
	}

	public virtual string ToXmlString(bool includePrivateParameters)
	{
		throw new NotImplementedException();
	}

	public virtual byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual byte[] ExportPkcs8PrivateKey()
	{
		throw new PlatformNotSupportedException();
	}

	public virtual byte[] ExportSubjectPublicKeyInfo()
	{
		throw new PlatformNotSupportedException();
	}

	public virtual void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual void ImportPkcs8PrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual void ImportSubjectPublicKeyInfo(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual bool TryExportPkcs8PrivateKey(Span<byte> destination, out int bytesWritten)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten)
	{
		throw new PlatformNotSupportedException();
	}
}
