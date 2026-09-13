using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Util;
using System.Text;

namespace System.Security.Cryptography;

[ComVisible(true)]
public abstract class DSA : AsymmetricAlgorithm
{
	public new static DSA Create()
	{
		return Create("System.Security.Cryptography.DSA");
	}

	public new static DSA Create(string algName)
	{
		return (DSA)CryptoConfig.CreateFromName(algName);
	}

	public abstract byte[] CreateSignature(byte[] rgbHash);

	public abstract bool VerifySignature(byte[] rgbHash, byte[] rgbSignature);

	protected virtual byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
	{
		throw DerivedClassMustOverride();
	}

	protected virtual byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
	{
		throw DerivedClassMustOverride();
	}

	public byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm)
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
		byte[] rgbHash = HashData(data, offset, count, hashAlgorithm);
		return CreateSignature(rgbHash);
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
		byte[] rgbHash = HashData(data, hashAlgorithm);
		return CreateSignature(rgbHash);
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
		byte[] rgbHash = HashData(data, offset, count, hashAlgorithm);
		return VerifySignature(rgbHash, signature);
	}

	public virtual bool VerifyData(Stream data, byte[] signature, HashAlgorithmName hashAlgorithm)
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
		byte[] rgbHash = HashData(data, hashAlgorithm);
		return VerifySignature(rgbHash, signature);
	}

	public override void FromXmlString(string xmlString)
	{
		if (xmlString == null)
		{
			throw new ArgumentNullException("xmlString");
		}
		DSAParameters parameters = default(DSAParameters);
		SecurityElement topElement = new Parser(xmlString).GetTopElement();
		string text = topElement.SearchForTextOfLocalName("P");
		if (text == null)
		{
			throw new CryptographicException(Environment.GetResourceString("Input string does not contain a valid encoding of the '{0}' '{1}' parameter.", "DSA", "P"));
		}
		parameters.P = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text));
		string text2 = topElement.SearchForTextOfLocalName("Q");
		if (text2 == null)
		{
			throw new CryptographicException(Environment.GetResourceString("Input string does not contain a valid encoding of the '{0}' '{1}' parameter.", "DSA", "Q"));
		}
		parameters.Q = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text2));
		string text3 = topElement.SearchForTextOfLocalName("G");
		if (text3 == null)
		{
			throw new CryptographicException(Environment.GetResourceString("Input string does not contain a valid encoding of the '{0}' '{1}' parameter.", "DSA", "G"));
		}
		parameters.G = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text3));
		string text4 = topElement.SearchForTextOfLocalName("Y");
		if (text4 == null)
		{
			throw new CryptographicException(Environment.GetResourceString("Input string does not contain a valid encoding of the '{0}' '{1}' parameter.", "DSA", "Y"));
		}
		parameters.Y = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text4));
		string text5 = topElement.SearchForTextOfLocalName("J");
		if (text5 != null)
		{
			parameters.J = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text5));
		}
		string text6 = topElement.SearchForTextOfLocalName("X");
		if (text6 != null)
		{
			parameters.X = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text6));
		}
		string text7 = topElement.SearchForTextOfLocalName("Seed");
		string text8 = topElement.SearchForTextOfLocalName("PgenCounter");
		if (text7 != null && text8 != null)
		{
			parameters.Seed = Convert.FromBase64String(Utils.DiscardWhiteSpaces(text7));
			parameters.Counter = Utils.ConvertByteArrayToInt(Convert.FromBase64String(Utils.DiscardWhiteSpaces(text8)));
		}
		else if (text7 != null || text8 != null)
		{
			if (text7 == null)
			{
				throw new CryptographicException(Environment.GetResourceString("Input string does not contain a valid encoding of the '{0}' '{1}' parameter.", "DSA", "Seed"));
			}
			throw new CryptographicException(Environment.GetResourceString("Input string does not contain a valid encoding of the '{0}' '{1}' parameter.", "DSA", "PgenCounter"));
		}
		ImportParameters(parameters);
	}

	public override string ToXmlString(bool includePrivateParameters)
	{
		DSAParameters dSAParameters = ExportParameters(includePrivateParameters);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<DSAKeyValue>");
		stringBuilder.Append("<P>" + Convert.ToBase64String(dSAParameters.P) + "</P>");
		stringBuilder.Append("<Q>" + Convert.ToBase64String(dSAParameters.Q) + "</Q>");
		stringBuilder.Append("<G>" + Convert.ToBase64String(dSAParameters.G) + "</G>");
		stringBuilder.Append("<Y>" + Convert.ToBase64String(dSAParameters.Y) + "</Y>");
		if (dSAParameters.J != null)
		{
			stringBuilder.Append("<J>" + Convert.ToBase64String(dSAParameters.J) + "</J>");
		}
		if (dSAParameters.Seed != null)
		{
			stringBuilder.Append("<Seed>" + Convert.ToBase64String(dSAParameters.Seed) + "</Seed>");
			stringBuilder.Append("<PgenCounter>" + Convert.ToBase64String(Utils.ConvertIntToByteArray(dSAParameters.Counter)) + "</PgenCounter>");
		}
		if (includePrivateParameters)
		{
			stringBuilder.Append("<X>" + Convert.ToBase64String(dSAParameters.X) + "</X>");
		}
		stringBuilder.Append("</DSAKeyValue>");
		return stringBuilder.ToString();
	}

	public abstract DSAParameters ExportParameters(bool includePrivateParameters);

	public abstract void ImportParameters(DSAParameters parameters);

	private static Exception DerivedClassMustOverride()
	{
		return new NotImplementedException(Environment.GetResourceString("Derived classes must provide an implementation."));
	}

	internal static Exception HashAlgorithmNameNullOrEmpty()
	{
		return new ArgumentException(Environment.GetResourceString("The hash algorithm name cannot be null or empty."), "hashAlgorithm");
	}

	public static DSA Create(int keySizeInBits)
	{
		DSA dSA = Create();
		try
		{
			dSA.KeySize = keySizeInBits;
			return dSA;
		}
		catch
		{
			dSA.Dispose();
			throw;
		}
	}

	public static DSA Create(DSAParameters parameters)
	{
		DSA dSA = Create();
		try
		{
			dSA.ImportParameters(parameters);
			return dSA;
		}
		catch
		{
			dSA.Dispose();
			throw;
		}
	}

	public virtual bool TryCreateSignature(ReadOnlySpan<byte> hash, Span<byte> destination, out int bytesWritten)
	{
		byte[] array = CreateSignature(hash.ToArray());
		if (array.Length <= destination.Length)
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
		try
		{
			data.CopyTo(array);
			byte[] array2 = HashData(array, 0, data.Length, hashAlgorithm);
			if (destination.Length >= array2.Length)
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

	public virtual bool TrySignData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, out int bytesWritten)
	{
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw HashAlgorithmNameNullOrEmpty();
		}
		if (TryHashData(data, destination, hashAlgorithm, out var bytesWritten2) && TryCreateSignature(destination.Slice(0, bytesWritten2), destination, out bytesWritten))
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
			throw HashAlgorithmNameNullOrEmpty();
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
					return VerifySignature(new ReadOnlySpan<byte>(array, 0, bytesWritten), signature);
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

	public virtual bool VerifySignature(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature)
	{
		return VerifySignature(hash.ToArray(), signature.ToArray());
	}
}
