namespace System.Security.Cryptography;

public sealed class AesGcm : IDisposable
{
	public static KeySizes NonceByteSizes
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static KeySizes TagByteSizes
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public AesGcm(byte[] key)
	{
		throw new PlatformNotSupportedException();
	}

	public AesGcm(ReadOnlySpan<byte> key)
	{
		throw new PlatformNotSupportedException();
	}

	public void Decrypt(byte[] nonce, byte[] ciphertext, byte[] tag, byte[] plaintext, byte[] associatedData = null)
	{
		throw new PlatformNotSupportedException();
	}

	public void Decrypt(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> tag, Span<byte> plaintext, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
	}

	public void Encrypt(byte[] nonce, byte[] plaintext, byte[] ciphertext, byte[] tag, byte[] associatedData = null)
	{
		throw new PlatformNotSupportedException();
	}

	public void Encrypt(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> plaintext, Span<byte> ciphertext, Span<byte> tag, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw new PlatformNotSupportedException();
	}
}
