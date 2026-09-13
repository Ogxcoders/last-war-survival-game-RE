using System.Reflection;
using Mono.Security.Cryptography;

namespace System.Security.Cryptography;

internal static class Utils
{
	internal const int DefaultRsaProviderType = 1;

	private static volatile RNGCryptoServiceProvider _rng;

	internal static RNGCryptoServiceProvider StaticRandomNumberGenerator
	{
		get
		{
			if (_rng == null)
			{
				_rng = new RNGCryptoServiceProvider();
			}
			return _rng;
		}
	}

	static Utils()
	{
	}

	internal static byte[] GenerateRandom(int keySize)
	{
		byte[] array = new byte[keySize];
		StaticRandomNumberGenerator.GetBytes(array);
		return array;
	}

	[SecurityCritical]
	internal static bool HasAlgorithm(int dwCalg, int dwKeySize)
	{
		return true;
	}

	internal static string DiscardWhiteSpaces(string inputBuffer)
	{
		return DiscardWhiteSpaces(inputBuffer, 0, inputBuffer.Length);
	}

	internal static string DiscardWhiteSpaces(string inputBuffer, int inputOffset, int inputCount)
	{
		int num = 0;
		for (int i = 0; i < inputCount; i++)
		{
			if (char.IsWhiteSpace(inputBuffer[inputOffset + i]))
			{
				num++;
			}
		}
		char[] array = new char[inputCount - num];
		num = 0;
		for (int i = 0; i < inputCount; i++)
		{
			if (!char.IsWhiteSpace(inputBuffer[inputOffset + i]))
			{
				array[num++] = inputBuffer[inputOffset + i];
			}
		}
		return new string(array);
	}

	internal static int ConvertByteArrayToInt(byte[] input)
	{
		int num = 0;
		for (int i = 0; i < input.Length; i++)
		{
			num *= 256;
			num += input[i];
		}
		return num;
	}

	internal static byte[] ConvertIntToByteArray(int dwInput)
	{
		byte[] array = new byte[8];
		int num = 0;
		if (dwInput == 0)
		{
			return new byte[1];
		}
		int num2 = dwInput;
		while (num2 > 0)
		{
			int num3 = num2 % 256;
			array[num] = (byte)num3;
			num2 = (num2 - num3) / 256;
			num++;
		}
		byte[] array2 = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array2[i] = array[num - i - 1];
		}
		return array2;
	}

	internal static void ConvertIntToByteArray(uint dwInput, ref byte[] counter)
	{
		uint num = dwInput;
		int num2 = 0;
		Array.Clear(counter, 0, counter.Length);
		if (dwInput != 0)
		{
			while (num != 0)
			{
				uint num3 = num % 256;
				counter[3 - num2] = (byte)num3;
				num = (num - num3) / 256;
				num2++;
			}
		}
	}

	internal static byte[] FixupKeyParity(byte[] key)
	{
		byte[] array = new byte[key.Length];
		for (int i = 0; i < key.Length; i++)
		{
			array[i] = (byte)(key[i] & 0xFE);
			byte b = (byte)((array[i] & 0xF) ^ (array[i] >> 4));
			byte b2 = (byte)((b & 3) ^ (b >> 2));
			if ((byte)((b2 & 1) ^ (b2 >> 1)) == 0)
			{
				array[i] |= 1;
			}
		}
		return array;
	}

	[SecurityCritical]
	internal unsafe static void DWORDFromLittleEndian(uint* x, int digits, byte* block)
	{
		int num = 0;
		int num2 = 0;
		while (num < digits)
		{
			x[num] = (uint)(block[num2] | (block[num2 + 1] << 8) | (block[num2 + 2] << 16) | (block[num2 + 3] << 24));
			num++;
			num2 += 4;
		}
	}

	internal static void DWORDToLittleEndian(byte[] block, uint[] x, int digits)
	{
		int num = 0;
		int num2 = 0;
		while (num < digits)
		{
			block[num2] = (byte)(x[num] & 0xFF);
			block[num2 + 1] = (byte)((x[num] >> 8) & 0xFF);
			block[num2 + 2] = (byte)((x[num] >> 16) & 0xFF);
			block[num2 + 3] = (byte)((x[num] >> 24) & 0xFF);
			num++;
			num2 += 4;
		}
	}

	[SecurityCritical]
	internal unsafe static void DWORDFromBigEndian(uint* x, int digits, byte* block)
	{
		int num = 0;
		int num2 = 0;
		while (num < digits)
		{
			x[num] = (uint)((block[num2] << 24) | (block[num2 + 1] << 16) | (block[num2 + 2] << 8) | block[num2 + 3]);
			num++;
			num2 += 4;
		}
	}

	internal static void DWORDToBigEndian(byte[] block, uint[] x, int digits)
	{
		int num = 0;
		int num2 = 0;
		while (num < digits)
		{
			block[num2] = (byte)((x[num] >> 24) & 0xFF);
			block[num2 + 1] = (byte)((x[num] >> 16) & 0xFF);
			block[num2 + 2] = (byte)((x[num] >> 8) & 0xFF);
			block[num2 + 3] = (byte)(x[num] & 0xFF);
			num++;
			num2 += 4;
		}
	}

	[SecurityCritical]
	internal unsafe static void QuadWordFromBigEndian(ulong* x, int digits, byte* block)
	{
		int num = 0;
		int num2 = 0;
		while (num < digits)
		{
			x[num] = ((ulong)block[num2] << 56) | ((ulong)block[num2 + 1] << 48) | ((ulong)block[num2 + 2] << 40) | ((ulong)block[num2 + 3] << 32) | ((ulong)block[num2 + 4] << 24) | ((ulong)block[num2 + 5] << 16) | ((ulong)block[num2 + 6] << 8) | block[num2 + 7];
			num++;
			num2 += 8;
		}
	}

	internal static void QuadWordToBigEndian(byte[] block, ulong[] x, int digits)
	{
		int num = 0;
		int num2 = 0;
		while (num < digits)
		{
			block[num2] = (byte)((x[num] >> 56) & 0xFF);
			block[num2 + 1] = (byte)((x[num] >> 48) & 0xFF);
			block[num2 + 2] = (byte)((x[num] >> 40) & 0xFF);
			block[num2 + 3] = (byte)((x[num] >> 32) & 0xFF);
			block[num2 + 4] = (byte)((x[num] >> 24) & 0xFF);
			block[num2 + 5] = (byte)((x[num] >> 16) & 0xFF);
			block[num2 + 6] = (byte)((x[num] >> 8) & 0xFF);
			block[num2 + 7] = (byte)(x[num] & 0xFF);
			num++;
			num2 += 8;
		}
	}

	internal static byte[] Int(uint i)
	{
		return new byte[4]
		{
			(byte)(i >> 24),
			(byte)(i >> 16),
			(byte)(i >> 8),
			(byte)i
		};
	}

	[SecurityCritical]
	internal static byte[] RsaOaepEncrypt(RSA rsa, HashAlgorithm hash, PKCS1MaskGenerationMethod mgf, RandomNumberGenerator rng, byte[] data)
	{
		return PKCS1.Encrypt_OAEP(rsa, hash, rng, data);
	}

	[SecurityCritical]
	internal static byte[] RsaOaepDecrypt(RSA rsa, HashAlgorithm hash, PKCS1MaskGenerationMethod mgf, byte[] encryptedData)
	{
		return PKCS1.Decrypt_OAEP(rsa, hash, encryptedData) ?? throw new CryptographicException(Environment.GetResourceString("Error occurred while decoding OAEP padding."));
	}

	[SecurityCritical]
	internal static byte[] RsaPkcs1Padding(RSA rsa, byte[] oid, byte[] hash)
	{
		int num = rsa.KeySize / 8;
		byte[] array = new byte[num];
		byte[] array2 = new byte[oid.Length + 8 + hash.Length];
		array2[0] = 48;
		int num2 = array2.Length - 2;
		array2[1] = (byte)num2;
		array2[2] = 48;
		num2 = oid.Length + 2;
		array2[3] = (byte)num2;
		Buffer.InternalBlockCopy(oid, 0, array2, 4, oid.Length);
		array2[4 + oid.Length] = 5;
		array2[4 + oid.Length + 1] = 0;
		array2[4 + oid.Length + 2] = 4;
		array2[4 + oid.Length + 3] = (byte)hash.Length;
		Buffer.InternalBlockCopy(hash, 0, array2, oid.Length + 8, hash.Length);
		int num3 = num - array2.Length;
		if (num3 <= 2)
		{
			throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Object identifier (OID) is unknown."));
		}
		array[0] = 0;
		array[1] = 1;
		for (int i = 2; i < num3 - 1; i++)
		{
			array[i] = byte.MaxValue;
		}
		array[num3 - 1] = 0;
		Buffer.InternalBlockCopy(array2, 0, array, num3, array2.Length);
		return array;
	}

	internal static bool CompareBigIntArrays(byte[] lhs, byte[] rhs)
	{
		if (lhs == null)
		{
			return rhs == null;
		}
		int i = 0;
		int j = 0;
		for (; i < lhs.Length && lhs[i] == 0; i++)
		{
		}
		for (; j < rhs.Length && rhs[j] == 0; j++)
		{
		}
		int num = lhs.Length - i;
		if (rhs.Length - j != num)
		{
			return false;
		}
		for (int k = 0; k < num; k++)
		{
			if (lhs[i + k] != rhs[j + k])
			{
				return false;
			}
		}
		return true;
	}

	internal static HashAlgorithmName OidToHashAlgorithmName(string oid)
	{
		return oid switch
		{
			"1.3.14.3.2.26" => HashAlgorithmName.SHA1, 
			"2.16.840.1.101.3.4.2.1" => HashAlgorithmName.SHA256, 
			"2.16.840.1.101.3.4.2.2" => HashAlgorithmName.SHA384, 
			"2.16.840.1.101.3.4.2.3" => HashAlgorithmName.SHA512, 
			_ => throw new NotSupportedException(), 
		};
	}

	internal static bool DoesRsaKeyOverride(RSA rsaKey, string methodName, Type[] parameterTypes)
	{
		Type type = rsaKey.GetType();
		if (rsaKey is RSACryptoServiceProvider)
		{
			return true;
		}
		if (type.FullName == "System.Security.Cryptography.RSACng")
		{
			return true;
		}
		return DoesRsaKeyOverrideSlowPath(type, methodName, parameterTypes);
	}

	private static bool DoesRsaKeyOverrideSlowPath(Type t, string methodName, Type[] parameterTypes)
	{
		if (t.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public, null, parameterTypes, null).DeclaringType == typeof(RSA))
		{
			return false;
		}
		return true;
	}

	internal static bool _ProduceLegacyHmacValues()
	{
		return Environment.GetEnvironmentVariable("legacyHMACMode") == "1";
	}
}
