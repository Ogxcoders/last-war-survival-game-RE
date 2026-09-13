using System.Buffers;
using System.IO;
using System.Security.Permissions;

namespace System.Security.Cryptography;

[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public abstract class ECDsa : AsymmetricAlgorithm
{
	public override string KeyExchangeAlgorithm => null;

	public override string SignatureAlgorithm => "ECDsa";

	public new static ECDsa Create()
	{
		throw new NotImplementedException();
	}

	public new static ECDsa Create(string algorithm)
	{
		if (algorithm == null)
		{
			throw new ArgumentNullException("algorithm");
		}
		return CryptoConfig.CreateFromName(algorithm) as ECDsa;
	}

	public static ECDsa Create(ECCurve curve)
	{
		ECDsa eCDsa = Create();
		if (eCDsa != null)
		{
			try
			{
				eCDsa.GenerateKey(curve);
			}
			catch
			{
				eCDsa.Dispose();
				throw;
			}
		}
		return eCDsa;
	}

	public static ECDsa Create(ECParameters parameters)
	{
		ECDsa eCDsa = Create();
		if (eCDsa != null)
		{
			try
			{
				eCDsa.ImportParameters(parameters);
			}
			catch
			{
				eCDsa.Dispose();
				throw;
			}
		}
		return eCDsa;
	}

	public abstract byte[] SignHash(byte[] hash);

	public abstract bool VerifyHash(byte[] hash, byte[] signature);

	protected virtual byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
	{
		throw DerivedClassMustOverride();
	}

	protected virtual byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
	{
		throw DerivedClassMustOverride();
	}

	public virtual byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		return SignData(data, 0, data.Length, hashAlgorithm);
	}

	public virtual byte[] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (offset < 0 || offset > data.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || count > data.Length - offset)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw HashAlgorithmNameNullOrEmpty();
		}
		byte[] hash = HashData(data, offset, count, hashAlgorithm);
		return SignHash(hash);
	}

	public virtual byte[] SignData(Stream data, HashAlgorithmName hashAlgorithm)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw HashAlgorithmNameNullOrEmpty();
		}
		byte[] hash = HashData(data, hashAlgorithm);
		return SignHash(hash);
	}

	public bool VerifyData(byte[] data, byte[] signature, HashAlgorithmName hashAlgorithm)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		return VerifyData(data, 0, data.Length, signature, hashAlgorithm);
	}

	public virtual bool VerifyData(byte[] data, int offset, int count, byte[] signature, HashAlgorithmName hashAlgorithm)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (offset < 0 || offset > data.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || count > data.Length - offset)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (signature == null)
		{
			throw new ArgumentNullException("signature");
		}
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw HashAlgorithmNameNullOrEmpty();
		}
		byte[] hash = HashData(data, offset, count, hashAlgorithm);
		return VerifyHash(hash, signature);
	}

	public bool VerifyData(Stream data, byte[] signature, HashAlgorithmName hashAlgorithm)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (signature == null)
		{
			throw new ArgumentNullException("signature");
		}
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw HashAlgorithmNameNullOrEmpty();
		}
		byte[] hash = HashData(data, hashAlgorithm);
		return VerifyHash(hash, signature);
	}

	public virtual ECParameters ExportParameters(bool includePrivateParameters)
	{
		throw new NotSupportedException(SR.GetString("Method not supported. Derived class must override."));
	}

	public virtual ECParameters ExportExplicitParameters(bool includePrivateParameters)
	{
		throw new NotSupportedException(SR.GetString("Method not supported. Derived class must override."));
	}

	public virtual void ImportParameters(ECParameters parameters)
	{
		throw new NotSupportedException(SR.GetString("Method not supported. Derived class must override."));
	}

	public virtual void GenerateKey(ECCurve curve)
	{
		throw new NotSupportedException(SR.GetString("Method not supported. Derived class must override."));
	}

	private static Exception DerivedClassMustOverride()
	{
		return new NotImplementedException(SR.GetString("Method not supported. Derived class must override."));
	}

	internal static Exception HashAlgorithmNameNullOrEmpty()
	{
		return new ArgumentException(SR.GetString("The hash algorithm name cannot be null or empty."), "hashAlgorithm");
	}

	protected virtual bool TryHashData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, out int bytesWritten)
	{
		byte[] array = ArrayPool<byte>.Shared.Rent(data.Length);
		try
		{
			data.CopyTo(array);
			byte[] array2 = HashData(array, 0, data.Length, hashAlgorithm);
			if (array2.Length <= destination.Length)
			{
				new ReadOnlySpan<byte>(array2).CopyTo(destination);
				bytesWritten = array2.Length;
				return true;
			}
			bytesWritten = 0;
			return false;
		}
		finally
		{
			Array.Clear(array, 0, data.Length);
			ArrayPool<byte>.Shared.Return(array);
		}
	}

	public virtual bool TrySignHash(ReadOnlySpan<byte> hash, Span<byte> destination, out int bytesWritten)
	{
		byte[] array = SignHash(hash.ToArray());
		if (array.Length <= destination.Length)
		{
			new ReadOnlySpan<byte>(array).CopyTo(destination);
			bytesWritten = array.Length;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	public virtual bool VerifyHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature)
	{
		return VerifyHash(hash.ToArray(), signature.ToArray());
	}

	public virtual bool TrySignData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, out int bytesWritten)
	{
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw new ArgumentException("The hash algorithm name cannot be null or empty.", "hashAlgorithm");
		}
		if (TryHashData(data, destination, hashAlgorithm, out var bytesWritten2) && TrySignHash(destination.Slice(0, bytesWritten2), destination, out bytesWritten))
		{
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	public virtual bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, HashAlgorithmName hashAlgorithm)
	{
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw new ArgumentException("The hash algorithm name cannot be null or empty.", "hashAlgorithm");
		}
		int num = 256;
		while (true)
		{
			int bytesWritten = 0;
			byte[] array = ArrayPool<byte>.Shared.Rent(num);
			try
			{
				if (TryHashData(data, array, hashAlgorithm, out bytesWritten))
				{
					return VerifyHash(new ReadOnlySpan<byte>(array, 0, bytesWritten), signature);
				}
			}
			finally
			{
				Array.Clear(array, 0, bytesWritten);
				ArrayPool<byte>.Shared.Return(array);
			}
			num = checked(num * 2);
		}
	}

	public virtual byte[] ExportECPrivateKey()
	{
		throw new PlatformNotSupportedException();
	}

	public virtual bool TryExportECPrivateKey(Span<byte> destination, out int bytesWritten)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual void ImportECPrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw new PlatformNotSupportedException();
	}
}
