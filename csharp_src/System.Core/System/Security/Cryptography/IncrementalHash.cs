namespace System.Security.Cryptography;

public sealed class IncrementalHash : IDisposable
{
	private const int NTE_BAD_ALGID = -2146893816;

	private readonly HashAlgorithmName _algorithmName;

	private HashAlgorithm _hash;

	private bool _disposed;

	private bool _resetPending;

	public HashAlgorithmName AlgorithmName => _algorithmName;

	private IncrementalHash(HashAlgorithmName name, HashAlgorithm hash)
	{
		_algorithmName = name;
		_hash = hash;
	}

	public void AppendData(byte[] data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		AppendData(data, 0, data.Length);
	}

	public void AppendData(byte[] data, int offset, int count)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset", "Non negative number is required.");
		}
		if (count < 0 || count > data.Length)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (data.Length - count < offset)
		{
			throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
		}
		if (_disposed)
		{
			throw new ObjectDisposedException(typeof(IncrementalHash).Name);
		}
		if (_resetPending)
		{
			_hash.Initialize();
			_resetPending = false;
		}
		_hash.TransformBlock(data, offset, count, null, 0);
	}

	public byte[] GetHashAndReset()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(typeof(IncrementalHash).Name);
		}
		if (_resetPending)
		{
			_hash.Initialize();
		}
		_hash.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
		byte[] hash = _hash.Hash;
		_resetPending = true;
		return hash;
	}

	public void Dispose()
	{
		_disposed = true;
		if (_hash != null)
		{
			_hash.Dispose();
			_hash = null;
		}
	}

	public static IncrementalHash CreateHash(HashAlgorithmName hashAlgorithm)
	{
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw new ArgumentException("The hash algorithm name cannot be null or empty.", "hashAlgorithm");
		}
		return new IncrementalHash(hashAlgorithm, GetHashAlgorithm(hashAlgorithm));
	}

	public static IncrementalHash CreateHMAC(HashAlgorithmName hashAlgorithm, byte[] key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw new ArgumentException("The hash algorithm name cannot be null or empty.", "hashAlgorithm");
		}
		return new IncrementalHash(hashAlgorithm, GetHMAC(hashAlgorithm, key));
	}

	private static HashAlgorithm GetHashAlgorithm(HashAlgorithmName hashAlgorithm)
	{
		if (hashAlgorithm == HashAlgorithmName.MD5)
		{
			return new MD5CryptoServiceProvider();
		}
		if (hashAlgorithm == HashAlgorithmName.SHA1)
		{
			return new SHA1CryptoServiceProvider();
		}
		if (hashAlgorithm == HashAlgorithmName.SHA256)
		{
			return new SHA256CryptoServiceProvider();
		}
		if (hashAlgorithm == HashAlgorithmName.SHA384)
		{
			return new SHA384CryptoServiceProvider();
		}
		if (hashAlgorithm == HashAlgorithmName.SHA512)
		{
			return new SHA512CryptoServiceProvider();
		}
		throw new CryptographicException(-2146893816);
	}

	private static HashAlgorithm GetHMAC(HashAlgorithmName hashAlgorithm, byte[] key)
	{
		if (hashAlgorithm == HashAlgorithmName.MD5)
		{
			return new HMACMD5(key);
		}
		if (hashAlgorithm == HashAlgorithmName.SHA1)
		{
			return new HMACSHA1(key);
		}
		if (hashAlgorithm == HashAlgorithmName.SHA256)
		{
			return new HMACSHA256(key);
		}
		if (hashAlgorithm == HashAlgorithmName.SHA384)
		{
			return new HMACSHA384(key);
		}
		if (hashAlgorithm == HashAlgorithmName.SHA512)
		{
			return new HMACSHA512(key);
		}
		throw new CryptographicException(-2146893816);
	}

	public void AppendData(ReadOnlySpan<byte> data)
	{
		AppendData(data.ToArray());
	}

	public bool TryGetHashAndReset(Span<byte> destination, out int bytesWritten)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(typeof(IncrementalHash).Name);
		}
		byte[] hashAndReset = GetHashAndReset();
		if (hashAndReset.AsSpan().TryCopyTo(destination))
		{
			bytesWritten = hashAndReset.Length;
			return true;
		}
		bytesWritten = 0;
		return false;
	}
}
