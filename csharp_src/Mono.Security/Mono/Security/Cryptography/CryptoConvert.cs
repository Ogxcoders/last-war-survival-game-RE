using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Security.Cryptography;

public sealed class CryptoConvert
{
	private CryptoConvert()
	{
	}

	private static int ToInt32LE(byte[] bytes, int offset)
	{
		return (bytes[offset + 3] << 24) | (bytes[offset + 2] << 16) | (bytes[offset + 1] << 8) | bytes[offset];
	}

	private static uint ToUInt32LE(byte[] bytes, int offset)
	{
		return (uint)((bytes[offset + 3] << 24) | (bytes[offset + 2] << 16) | (bytes[offset + 1] << 8) | bytes[offset]);
	}

	private static byte[] GetBytesLE(int val)
	{
		return new byte[4]
		{
			(byte)(val & 0xFF),
			(byte)((val >> 8) & 0xFF),
			(byte)((val >> 16) & 0xFF),
			(byte)((val >> 24) & 0xFF)
		};
	}

	private static byte[] Trim(byte[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != 0)
			{
				byte[] array2 = new byte[array.Length - i];
				Buffer.BlockCopy(array, i, array2, 0, array2.Length);
				return array2;
			}
		}
		return null;
	}

	public static RSA FromCapiPrivateKeyBlob(byte[] blob)
	{
		return FromCapiPrivateKeyBlob(blob, 0);
	}

	public static RSA FromCapiPrivateKeyBlob(byte[] blob, int offset)
	{
		RSAParameters parametersFromCapiPrivateKeyBlob = GetParametersFromCapiPrivateKeyBlob(blob, offset);
		RSA rSA = null;
		try
		{
			rSA = RSA.Create();
			rSA.ImportParameters(parametersFromCapiPrivateKeyBlob);
		}
		catch (CryptographicException ex)
		{
			try
			{
				rSA = new RSACryptoServiceProvider(new CspParameters
				{
					Flags = CspProviderFlags.UseMachineKeyStore
				});
				rSA.ImportParameters(parametersFromCapiPrivateKeyBlob);
			}
			catch
			{
				throw ex;
			}
		}
		return rSA;
	}

	private static RSAParameters GetParametersFromCapiPrivateKeyBlob(byte[] blob, int offset)
	{
		if (blob == null)
		{
			throw new ArgumentNullException("blob");
		}
		if (offset >= blob.Length)
		{
			throw new ArgumentException("blob is too small.");
		}
		RSAParameters result = default(RSAParameters);
		try
		{
			if (blob[offset] != 7 || blob[offset + 1] != 2 || blob[offset + 2] != 0 || blob[offset + 3] != 0 || ToUInt32LE(blob, offset + 8) != 843141970)
			{
				throw new CryptographicException("Invalid blob header");
			}
			int num = ToInt32LE(blob, offset + 12);
			byte[] array = new byte[4];
			Buffer.BlockCopy(blob, offset + 16, array, 0, 4);
			Array.Reverse(array);
			result.Exponent = Trim(array);
			int num2 = offset + 20;
			int num3 = num >> 3;
			result.Modulus = new byte[num3];
			Buffer.BlockCopy(blob, num2, result.Modulus, 0, num3);
			Array.Reverse(result.Modulus);
			num2 += num3;
			int num4 = num3 >> 1;
			result.P = new byte[num4];
			Buffer.BlockCopy(blob, num2, result.P, 0, num4);
			Array.Reverse(result.P);
			num2 += num4;
			result.Q = new byte[num4];
			Buffer.BlockCopy(blob, num2, result.Q, 0, num4);
			Array.Reverse(result.Q);
			num2 += num4;
			result.DP = new byte[num4];
			Buffer.BlockCopy(blob, num2, result.DP, 0, num4);
			Array.Reverse(result.DP);
			num2 += num4;
			result.DQ = new byte[num4];
			Buffer.BlockCopy(blob, num2, result.DQ, 0, num4);
			Array.Reverse(result.DQ);
			num2 += num4;
			result.InverseQ = new byte[num4];
			Buffer.BlockCopy(blob, num2, result.InverseQ, 0, num4);
			Array.Reverse(result.InverseQ);
			num2 += num4;
			result.D = new byte[num3];
			if (num2 + num3 + offset <= blob.Length)
			{
				Buffer.BlockCopy(blob, num2, result.D, 0, num3);
				Array.Reverse(result.D);
			}
			return result;
		}
		catch (Exception inner)
		{
			throw new CryptographicException("Invalid blob.", inner);
		}
	}

	public static DSA FromCapiPrivateKeyBlobDSA(byte[] blob)
	{
		return FromCapiPrivateKeyBlobDSA(blob, 0);
	}

	public static DSA FromCapiPrivateKeyBlobDSA(byte[] blob, int offset)
	{
		if (blob == null)
		{
			throw new ArgumentNullException("blob");
		}
		if (offset >= blob.Length)
		{
			throw new ArgumentException("blob is too small.");
		}
		DSAParameters parameters = default(DSAParameters);
		try
		{
			if (blob[offset] != 7 || blob[offset + 1] != 2 || blob[offset + 2] != 0 || blob[offset + 3] != 0 || ToUInt32LE(blob, offset + 8) != 844321604)
			{
				throw new CryptographicException("Invalid blob header");
			}
			int num = ToInt32LE(blob, offset + 12) >> 3;
			int num2 = offset + 16;
			parameters.P = new byte[num];
			Buffer.BlockCopy(blob, num2, parameters.P, 0, num);
			Array.Reverse(parameters.P);
			num2 += num;
			parameters.Q = new byte[20];
			Buffer.BlockCopy(blob, num2, parameters.Q, 0, 20);
			Array.Reverse(parameters.Q);
			num2 += 20;
			parameters.G = new byte[num];
			Buffer.BlockCopy(blob, num2, parameters.G, 0, num);
			Array.Reverse(parameters.G);
			num2 += num;
			parameters.X = new byte[20];
			Buffer.BlockCopy(blob, num2, parameters.X, 0, 20);
			Array.Reverse(parameters.X);
			num2 += 20;
			parameters.Counter = ToInt32LE(blob, num2);
			num2 += 4;
			parameters.Seed = new byte[20];
			Buffer.BlockCopy(blob, num2, parameters.Seed, 0, 20);
			Array.Reverse(parameters.Seed);
			num2 += 20;
		}
		catch (Exception inner)
		{
			throw new CryptographicException("Invalid blob.", inner);
		}
		DSA dSA = null;
		try
		{
			dSA = DSA.Create();
			dSA.ImportParameters(parameters);
		}
		catch (CryptographicException ex)
		{
			try
			{
				dSA = new DSACryptoServiceProvider(new CspParameters
				{
					Flags = CspProviderFlags.UseMachineKeyStore
				});
				dSA.ImportParameters(parameters);
			}
			catch
			{
				throw ex;
			}
		}
		return dSA;
	}

	public static byte[] ToCapiPrivateKeyBlob(RSA rsa)
	{
		RSAParameters rSAParameters = rsa.ExportParameters(includePrivateParameters: true);
		int num = rSAParameters.Modulus.Length;
		byte[] array = new byte[20 + (num << 2) + (num >> 1)];
		array[0] = 7;
		array[1] = 2;
		array[5] = 36;
		array[8] = 82;
		array[9] = 83;
		array[10] = 65;
		array[11] = 50;
		byte[] bytesLE = GetBytesLE(num << 3);
		array[12] = bytesLE[0];
		array[13] = bytesLE[1];
		array[14] = bytesLE[2];
		array[15] = bytesLE[3];
		int num2 = 16;
		int num3 = rSAParameters.Exponent.Length;
		while (num3 > 0)
		{
			array[num2++] = rSAParameters.Exponent[--num3];
		}
		num2 = 20;
		byte[] modulus = rSAParameters.Modulus;
		int num4 = modulus.Length;
		Array.Reverse(modulus, 0, num4);
		Buffer.BlockCopy(modulus, 0, array, num2, num4);
		num2 += num4;
		byte[] p = rSAParameters.P;
		num4 = p.Length;
		Array.Reverse(p, 0, num4);
		Buffer.BlockCopy(p, 0, array, num2, num4);
		num2 += num4;
		byte[] q = rSAParameters.Q;
		num4 = q.Length;
		Array.Reverse(q, 0, num4);
		Buffer.BlockCopy(q, 0, array, num2, num4);
		num2 += num4;
		byte[] dP = rSAParameters.DP;
		num4 = dP.Length;
		Array.Reverse(dP, 0, num4);
		Buffer.BlockCopy(dP, 0, array, num2, num4);
		num2 += num4;
		byte[] dQ = rSAParameters.DQ;
		num4 = dQ.Length;
		Array.Reverse(dQ, 0, num4);
		Buffer.BlockCopy(dQ, 0, array, num2, num4);
		num2 += num4;
		byte[] inverseQ = rSAParameters.InverseQ;
		num4 = inverseQ.Length;
		Array.Reverse(inverseQ, 0, num4);
		Buffer.BlockCopy(inverseQ, 0, array, num2, num4);
		num2 += num4;
		byte[] d = rSAParameters.D;
		num4 = d.Length;
		Array.Reverse(d, 0, num4);
		Buffer.BlockCopy(d, 0, array, num2, num4);
		return array;
	}

	public static byte[] ToCapiPrivateKeyBlob(DSA dsa)
	{
		DSAParameters dSAParameters = dsa.ExportParameters(includePrivateParameters: true);
		int num = dSAParameters.P.Length;
		byte[] array = new byte[16 + num + 20 + num + 20 + 4 + 20];
		array[0] = 7;
		array[1] = 2;
		array[5] = 34;
		array[8] = 68;
		array[9] = 83;
		array[10] = 83;
		array[11] = 50;
		byte[] bytesLE = GetBytesLE(num << 3);
		array[12] = bytesLE[0];
		array[13] = bytesLE[1];
		array[14] = bytesLE[2];
		array[15] = bytesLE[3];
		int num2 = 16;
		byte[] p = dSAParameters.P;
		Array.Reverse(p);
		Buffer.BlockCopy(p, 0, array, num2, num);
		num2 += num;
		byte[] q = dSAParameters.Q;
		Array.Reverse(q);
		Buffer.BlockCopy(q, 0, array, num2, 20);
		num2 += 20;
		byte[] g = dSAParameters.G;
		Array.Reverse(g);
		Buffer.BlockCopy(g, 0, array, num2, num);
		num2 += num;
		byte[] x = dSAParameters.X;
		Array.Reverse(x);
		Buffer.BlockCopy(x, 0, array, num2, 20);
		num2 += 20;
		Buffer.BlockCopy(GetBytesLE(dSAParameters.Counter), 0, array, num2, 4);
		num2 += 4;
		byte[] seed = dSAParameters.Seed;
		Array.Reverse(seed);
		Buffer.BlockCopy(seed, 0, array, num2, 20);
		return array;
	}

	public static RSA FromCapiPublicKeyBlob(byte[] blob)
	{
		return FromCapiPublicKeyBlob(blob, 0);
	}

	public static RSA FromCapiPublicKeyBlob(byte[] blob, int offset)
	{
		RSAParameters parametersFromCapiPublicKeyBlob = GetParametersFromCapiPublicKeyBlob(blob, offset);
		try
		{
			RSA rSA = null;
			try
			{
				rSA = RSA.Create();
				rSA.ImportParameters(parametersFromCapiPublicKeyBlob);
			}
			catch (CryptographicException)
			{
				rSA = new RSACryptoServiceProvider(new CspParameters
				{
					Flags = CspProviderFlags.UseMachineKeyStore
				});
				rSA.ImportParameters(parametersFromCapiPublicKeyBlob);
			}
			return rSA;
		}
		catch (Exception inner)
		{
			throw new CryptographicException("Invalid blob.", inner);
		}
	}

	private static RSAParameters GetParametersFromCapiPublicKeyBlob(byte[] blob, int offset)
	{
		if (blob == null)
		{
			throw new ArgumentNullException("blob");
		}
		if (offset >= blob.Length)
		{
			throw new ArgumentException("blob is too small.");
		}
		try
		{
			if (blob[offset] != 6 || blob[offset + 1] != 2 || blob[offset + 2] != 0 || blob[offset + 3] != 0 || ToUInt32LE(blob, offset + 8) != 826364754)
			{
				throw new CryptographicException("Invalid blob header");
			}
			int num = ToInt32LE(blob, offset + 12);
			RSAParameters result = new RSAParameters
			{
				Exponent = new byte[3]
			};
			result.Exponent[0] = blob[offset + 18];
			result.Exponent[1] = blob[offset + 17];
			result.Exponent[2] = blob[offset + 16];
			int srcOffset = offset + 20;
			int num2 = num >> 3;
			result.Modulus = new byte[num2];
			Buffer.BlockCopy(blob, srcOffset, result.Modulus, 0, num2);
			Array.Reverse(result.Modulus);
			return result;
		}
		catch (Exception inner)
		{
			throw new CryptographicException("Invalid blob.", inner);
		}
	}

	public static DSA FromCapiPublicKeyBlobDSA(byte[] blob)
	{
		return FromCapiPublicKeyBlobDSA(blob, 0);
	}

	public static DSA FromCapiPublicKeyBlobDSA(byte[] blob, int offset)
	{
		if (blob == null)
		{
			throw new ArgumentNullException("blob");
		}
		if (offset >= blob.Length)
		{
			throw new ArgumentException("blob is too small.");
		}
		try
		{
			if (blob[offset] != 6 || blob[offset + 1] != 2 || blob[offset + 2] != 0 || blob[offset + 3] != 0 || ToUInt32LE(blob, offset + 8) != 827544388)
			{
				throw new CryptographicException("Invalid blob header");
			}
			int num = ToInt32LE(blob, offset + 12);
			DSAParameters parameters = default(DSAParameters);
			int num2 = num >> 3;
			int num3 = offset + 16;
			parameters.P = new byte[num2];
			Buffer.BlockCopy(blob, num3, parameters.P, 0, num2);
			Array.Reverse(parameters.P);
			num3 += num2;
			parameters.Q = new byte[20];
			Buffer.BlockCopy(blob, num3, parameters.Q, 0, 20);
			Array.Reverse(parameters.Q);
			num3 += 20;
			parameters.G = new byte[num2];
			Buffer.BlockCopy(blob, num3, parameters.G, 0, num2);
			Array.Reverse(parameters.G);
			num3 += num2;
			parameters.Y = new byte[num2];
			Buffer.BlockCopy(blob, num3, parameters.Y, 0, num2);
			Array.Reverse(parameters.Y);
			num3 += num2;
			parameters.Counter = ToInt32LE(blob, num3);
			num3 += 4;
			parameters.Seed = new byte[20];
			Buffer.BlockCopy(blob, num3, parameters.Seed, 0, 20);
			Array.Reverse(parameters.Seed);
			num3 += 20;
			DSA dSA = DSA.Create();
			dSA.ImportParameters(parameters);
			return dSA;
		}
		catch (Exception inner)
		{
			throw new CryptographicException("Invalid blob.", inner);
		}
	}

	public static byte[] ToCapiPublicKeyBlob(RSA rsa)
	{
		RSAParameters rSAParameters = rsa.ExportParameters(includePrivateParameters: false);
		int num = rSAParameters.Modulus.Length;
		byte[] array = new byte[20 + num];
		array[0] = 6;
		array[1] = 2;
		array[5] = 36;
		array[8] = 82;
		array[9] = 83;
		array[10] = 65;
		array[11] = 49;
		byte[] bytesLE = GetBytesLE(num << 3);
		array[12] = bytesLE[0];
		array[13] = bytesLE[1];
		array[14] = bytesLE[2];
		array[15] = bytesLE[3];
		int num2 = 16;
		int num3 = rSAParameters.Exponent.Length;
		while (num3 > 0)
		{
			array[num2++] = rSAParameters.Exponent[--num3];
		}
		num2 = 20;
		byte[] modulus = rSAParameters.Modulus;
		int num4 = modulus.Length;
		Array.Reverse(modulus, 0, num4);
		Buffer.BlockCopy(modulus, 0, array, num2, num4);
		num2 += num4;
		return array;
	}

	public static byte[] ToCapiPublicKeyBlob(DSA dsa)
	{
		DSAParameters dSAParameters = dsa.ExportParameters(includePrivateParameters: false);
		int num = dSAParameters.P.Length;
		byte[] array = new byte[16 + num + 20 + num + num + 4 + 20];
		array[0] = 6;
		array[1] = 2;
		array[5] = 34;
		array[8] = 68;
		array[9] = 83;
		array[10] = 83;
		array[11] = 49;
		byte[] bytesLE = GetBytesLE(num << 3);
		array[12] = bytesLE[0];
		array[13] = bytesLE[1];
		array[14] = bytesLE[2];
		array[15] = bytesLE[3];
		int num2 = 16;
		byte[] p = dSAParameters.P;
		Array.Reverse(p);
		Buffer.BlockCopy(p, 0, array, num2, num);
		num2 += num;
		byte[] q = dSAParameters.Q;
		Array.Reverse(q);
		Buffer.BlockCopy(q, 0, array, num2, 20);
		num2 += 20;
		byte[] g = dSAParameters.G;
		Array.Reverse(g);
		Buffer.BlockCopy(g, 0, array, num2, num);
		num2 += num;
		byte[] y = dSAParameters.Y;
		Array.Reverse(y);
		Buffer.BlockCopy(y, 0, array, num2, num);
		num2 += num;
		Buffer.BlockCopy(GetBytesLE(dSAParameters.Counter), 0, array, num2, 4);
		num2 += 4;
		byte[] seed = dSAParameters.Seed;
		Array.Reverse(seed);
		Buffer.BlockCopy(seed, 0, array, num2, 20);
		return array;
	}

	public static RSA FromCapiKeyBlob(byte[] blob)
	{
		return FromCapiKeyBlob(blob, 0);
	}

	public static RSA FromCapiKeyBlob(byte[] blob, int offset)
	{
		if (blob == null)
		{
			throw new ArgumentNullException("blob");
		}
		if (offset >= blob.Length)
		{
			throw new ArgumentException("blob is too small.");
		}
		switch (blob[offset])
		{
		case 0:
			if (blob[offset + 12] == 6)
			{
				return FromCapiPublicKeyBlob(blob, offset + 12);
			}
			break;
		case 6:
			return FromCapiPublicKeyBlob(blob, offset);
		case 7:
			return FromCapiPrivateKeyBlob(blob, offset);
		}
		throw new CryptographicException("Unknown blob format.");
	}

	public static DSA FromCapiKeyBlobDSA(byte[] blob)
	{
		return FromCapiKeyBlobDSA(blob, 0);
	}

	public static DSA FromCapiKeyBlobDSA(byte[] blob, int offset)
	{
		if (blob == null)
		{
			throw new ArgumentNullException("blob");
		}
		if (offset >= blob.Length)
		{
			throw new ArgumentException("blob is too small.");
		}
		return blob[offset] switch
		{
			6 => FromCapiPublicKeyBlobDSA(blob, offset), 
			7 => FromCapiPrivateKeyBlobDSA(blob, offset), 
			_ => throw new CryptographicException("Unknown blob format."), 
		};
	}

	public static byte[] ToCapiKeyBlob(AsymmetricAlgorithm keypair, bool includePrivateKey)
	{
		if (keypair == null)
		{
			throw new ArgumentNullException("keypair");
		}
		if (keypair is RSA)
		{
			return ToCapiKeyBlob((RSA)keypair, includePrivateKey);
		}
		if (keypair is DSA)
		{
			return ToCapiKeyBlob((DSA)keypair, includePrivateKey);
		}
		return null;
	}

	public static byte[] ToCapiKeyBlob(RSA rsa, bool includePrivateKey)
	{
		if (rsa == null)
		{
			throw new ArgumentNullException("rsa");
		}
		if (includePrivateKey)
		{
			return ToCapiPrivateKeyBlob(rsa);
		}
		return ToCapiPublicKeyBlob(rsa);
	}

	public static byte[] ToCapiKeyBlob(DSA dsa, bool includePrivateKey)
	{
		if (dsa == null)
		{
			throw new ArgumentNullException("dsa");
		}
		if (includePrivateKey)
		{
			return ToCapiPrivateKeyBlob(dsa);
		}
		return ToCapiPublicKeyBlob(dsa);
	}

	public static string ToHex(byte[] input)
	{
		if (input == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(input.Length * 2);
		foreach (byte b in input)
		{
			stringBuilder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
		}
		return stringBuilder.ToString();
	}

	private static byte FromHexChar(char c)
	{
		if (c >= 'a' && c <= 'f')
		{
			return (byte)(c - 97 + 10);
		}
		if (c >= 'A' && c <= 'F')
		{
			return (byte)(c - 65 + 10);
		}
		if (c >= '0' && c <= '9')
		{
			return (byte)(c - 48);
		}
		throw new ArgumentException("invalid hex char");
	}

	public static byte[] FromHex(string hex)
	{
		if (hex == null)
		{
			return null;
		}
		if ((hex.Length & 1) == 1)
		{
			throw new ArgumentException("Length must be a multiple of 2");
		}
		byte[] array = new byte[hex.Length >> 1];
		int num = 0;
		int num2 = 0;
		while (num < array.Length)
		{
			array[num] = (byte)(FromHexChar(hex[num2++]) << 4);
			array[num++] += FromHexChar(hex[num2++]);
		}
		return array;
	}
}
