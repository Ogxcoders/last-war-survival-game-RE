using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Util;
using System.Text;

namespace System.Security.Cryptography;

[ComVisible(true)]
public abstract class RSA : AsymmetricAlgorithm
{
	public override string KeyExchangeAlgorithm => "RSA";

	public override string SignatureAlgorithm => "RSA";

	public new static RSA Create()
	{
		return Create("System.Security.Cryptography.RSA");
	}

	public new static RSA Create(string algName)
	{
		return (RSA)CryptoConfig.CreateFromName(algName);
	}

	public virtual byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
	{
		throw DerivedClassMustOverride();
	}

	public virtual byte[] Decrypt(byte[] data, RSAEncryptionPadding padding)
	{
		throw DerivedClassMustOverride();
	}

	public virtual byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw DerivedClassMustOverride();
	}

	public virtual bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw DerivedClassMustOverride();
	}

	protected virtual byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
	{
		throw DerivedClassMustOverride();
	}

	protected virtual byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
	{
		throw DerivedClassMustOverride();
	}

	public byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		return SignData(data, 0, data.Length, hashAlgorithm, padding);
	}

	public virtual byte[] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
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
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		byte[] hash = HashData(data, offset, count, hashAlgorithm);
		return SignHash(hash, hashAlgorithm, padding);
	}

	public virtual byte[] SignData(Stream data, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw HashAlgorithmNameNullOrEmpty();
		}
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		byte[] hash = HashData(data, hashAlgorithm);
		return SignHash(hash, hashAlgorithm, padding);
	}

	public bool VerifyData(byte[] data, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		return VerifyData(data, 0, data.Length, signature, hashAlgorithm, padding);
	}

	public virtual bool VerifyData(byte[] data, int offset, int count, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
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
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		byte[] hash = HashData(data, offset, count, hashAlgorithm);
		return VerifyHash(hash, signature, hashAlgorithm, padding);
	}

	public bool VerifyData(Stream data, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
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
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		byte[] hash = HashData(data, hashAlgorithm);
		return VerifyHash(hash, signature, hashAlgorithm, padding);
	}

	private static Exception DerivedClassMustOverride()
	{
		return new NotImplementedException(Environment.GetResourceString("Derived classes must provide an implementation."));
	}

	internal static Exception HashAlgorithmNameNullOrEmpty()
	{
		return new ArgumentException(Environment.GetResourceString("The hash algorithm name cannot be null or empty."), "hashAlgorithm");
	}

	public virtual byte[] DecryptValue(byte[] rgb)
	{
		throw new NotSupportedException(Environment.GetResourceString("Method is not supported."));
	}

	public virtual byte[] EncryptValue(byte[] rgb)
	{
		throw new NotSupportedException(Environment.GetResourceString("Method is not supported."));
	}

	public override void FromXmlString(string xmlString)
	{
		if (xmlString == null)
		{
			throw new ArgumentNullException("xmlString");
		}
		RSAParameters parameters = default(RSAParameters);
		SecurityElement topElement = new Parser(xmlString).GetTopElement();
		string text = topElement.SearchForTextOfLocalName("Modulus");
		if (text == null)
		{
			throw new CryptographicException(Environment.GetResourceString("Input string does not contain a valid encoding of the '{0}' '{1}' parameter.", "RSA", "Modulus"));
		}
		parameters.Modulus = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text));
		string text2 = topElement.SearchForTextOfLocalName("Exponent");
		if (text2 == null)
		{
			throw new CryptographicException(Environment.GetResourceString("Input string does not contain a valid encoding of the '{0}' '{1}' parameter.", "RSA", "Exponent"));
		}
		parameters.Exponent = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text2));
		string text3 = topElement.SearchForTextOfLocalName("P");
		if (text3 != null)
		{
			parameters.P = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text3));
		}
		string text4 = topElement.SearchForTextOfLocalName("Q");
		if (text4 != null)
		{
			parameters.Q = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text4));
		}
		string text5 = topElement.SearchForTextOfLocalName("DP");
		if (text5 != null)
		{
			parameters.DP = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text5));
		}
		string text6 = topElement.SearchForTextOfLocalName("DQ");
		if (text6 != null)
		{
			parameters.DQ = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text6));
		}
		string text7 = topElement.SearchForTextOfLocalName("InverseQ");
		if (text7 != null)
		{
			parameters.InverseQ = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text7));
		}
		string text8 = topElement.SearchForTextOfLocalName("D");
		if (text8 != null)
		{
			parameters.D = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text8));
		}
		ImportParameters(parameters);
	}

	public override string ToXmlString(bool includePrivateParameters)
	{
		RSAParameters rSAParameters = ExportParameters(includePrivateParameters);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<RSAKeyValue>");
		stringBuilder.Append("<Modulus>" + Convert.ToBase64String(rSAParameters.Modulus) + "</Modulus>");
		stringBuilder.Append("<Exponent>" + Convert.ToBase64String(rSAParameters.Exponent) + "</Exponent>");
		if (includePrivateParameters)
		{
			stringBuilder.Append("<P>" + Convert.ToBase64String(rSAParameters.P) + "</P>");
			stringBuilder.Append("<Q>" + Convert.ToBase64String(rSAParameters.Q) + "</Q>");
			stringBuilder.Append("<DP>" + Convert.ToBase64String(rSAParameters.DP) + "</DP>");
			stringBuilder.Append("<DQ>" + Convert.ToBase64String(rSAParameters.DQ) + "</DQ>");
			stringBuilder.Append("<InverseQ>" + Convert.ToBase64String(rSAParameters.InverseQ) + "</InverseQ>");
			stringBuilder.Append("<D>" + Convert.ToBase64String(rSAParameters.D) + "</D>");
		}
		stringBuilder.Append("</RSAKeyValue>");
		return stringBuilder.ToString();
	}

	public abstract RSAParameters ExportParameters(bool includePrivateParameters);

	public abstract void ImportParameters(RSAParameters parameters);

	public static RSA Create(int keySizeInBits)
	{
		RSA rSA = Create();
		try
		{
			rSA.KeySize = keySizeInBits;
			return rSA;
		}
		catch
		{
			rSA.Dispose();
			throw;
		}
	}

	public static RSA Create(RSAParameters parameters)
	{
		RSA rSA = Create();
		try
		{
			rSA.ImportParameters(parameters);
			return rSA;
		}
		catch
		{
			rSA.Dispose();
			throw;
		}
	}

	public virtual bool TryDecrypt(ReadOnlySpan<byte> data, Span<byte> destination, RSAEncryptionPadding padding, out int bytesWritten)
	{
		byte[] array = Decrypt(data.ToArray(), padding);
		if (destination.Length >= array.Length)
		{
			new ReadOnlySpan<byte>(array).CopyTo(destination);
			bytesWritten = array.Length;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	public virtual bool TryEncrypt(ReadOnlySpan<byte> data, Span<byte> destination, RSAEncryptionPadding padding, out int bytesWritten)
	{
		byte[] array = Encrypt(data.ToArray(), padding);
		if (destination.Length >= array.Length)
		{
			new ReadOnlySpan<byte>(array).CopyTo(destination);
			bytesWritten = array.Length;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	protected virtual bool TryHashData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, out int bytesWritten)
	{
		byte[] array = ArrayPool<byte>.Shared.Rent(data.Length);
		byte[] array2;
		try
		{
			data.CopyTo(array);
			array2 = HashData(array, 0, data.Length, hashAlgorithm);
		}
		finally
		{
			Array.Clear(array, 0, data.Length);
			ArrayPool<byte>.Shared.Return(array);
		}
		if (destination.Length >= array2.Length)
		{
			new ReadOnlySpan<byte>(array2).CopyTo(destination);
			bytesWritten = array2.Length;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	public virtual bool TrySignHash(ReadOnlySpan<byte> hash, Span<byte> destination, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding, out int bytesWritten)
	{
		byte[] array = SignHash(hash.ToArray(), hashAlgorithm, padding);
		if (destination.Length >= array.Length)
		{
			new ReadOnlySpan<byte>(array).CopyTo(destination);
			bytesWritten = array.Length;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	public virtual bool TrySignData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding, out int bytesWritten)
	{
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw HashAlgorithmNameNullOrEmpty();
		}
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		if (TryHashData(data, destination, hashAlgorithm, out var bytesWritten2) && TrySignHash(destination.Slice(0, bytesWritten2), destination, hashAlgorithm, padding, out bytesWritten))
		{
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	public virtual bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw HashAlgorithmNameNullOrEmpty();
		}
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
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
					return VerifyHash(new ReadOnlySpan<byte>(array, 0, bytesWritten), signature, hashAlgorithm, padding);
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

	public virtual bool VerifyHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		return VerifyHash(hash.ToArray(), signature.ToArray(), hashAlgorithm, padding);
	}

	public virtual byte[] ExportRSAPrivateKey()
	{
		throw new PlatformNotSupportedException();
	}

	public virtual byte[] ExportRSAPublicKey()
	{
		throw new PlatformNotSupportedException();
	}

	public virtual void ImportRSAPrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual void ImportRSAPublicKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual bool TryExportRSAPrivateKey(Span<byte> destination, out int bytesWritten)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual bool TryExportRSAPublicKey(Span<byte> destination, out int bytesWritten)
	{
		throw new PlatformNotSupportedException();
	}
}
