using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;

public class SCrypt
{
	public static byte[] Generate(byte[] P, byte[] S, int N, int r, int p, int dkLen)
	{
		if (P == null)
		{
			throw new ArgumentNullException("Passphrase P must be provided.");
		}
		if (S == null)
		{
			throw new ArgumentNullException("Salt S must be provided.");
		}
		if (N <= 1 || !IsPowerOf2(N))
		{
			throw new ArgumentException("Cost parameter N must be > 1 and a power of 2.");
		}
		if (r == 1 && N >= 65536)
		{
			throw new ArgumentException("Cost parameter N must be > 1 and < 65536.");
		}
		if (r < 1)
		{
			throw new ArgumentException("Block size r must be >= 1.");
		}
		int num = int.MaxValue / (128 * r * 8);
		if (p < 1 || p > num)
		{
			throw new ArgumentException("Parallelisation parameter p must be >= 1 and <= " + num + " (based on block size r of " + r + ")");
		}
		if (dkLen < 1)
		{
			throw new ArgumentException("Generated key length dkLen must be >= 1.");
		}
		return MFcrypt(P, S, N, r, p, dkLen);
	}

	private static byte[] MFcrypt(byte[] P, byte[] S, int N, int r, int p, int dkLen)
	{
		int num = r * 128;
		byte[] array = SingleIterationPBKDF2(P, S, p * num);
		uint[] array2 = null;
		try
		{
			int num2 = array.Length >> 2;
			array2 = new uint[num2];
			Pack.LE_To_UInt32(array, 0, array2);
			int num3 = num >> 2;
			for (int i = 0; i < num2; i += num3)
			{
				SMix(array2, i, N, r);
			}
			Pack.UInt32_To_LE(array2, array, 0);
			return SingleIterationPBKDF2(P, array, dkLen);
		}
		finally
		{
			ClearAll(array, array2);
		}
	}

	private static byte[] SingleIterationPBKDF2(byte[] P, byte[] S, int dkLen)
	{
		Pkcs5S2ParametersGenerator pkcs5S2ParametersGenerator = new Pkcs5S2ParametersGenerator(new Sha256Digest());
		pkcs5S2ParametersGenerator.Init(P, S, 1);
		return ((KeyParameter)pkcs5S2ParametersGenerator.GenerateDerivedMacParameters(dkLen * 8)).GetKey();
	}

	private static void SMix(uint[] B, int BOff, int N, int r)
	{
		int num = r * 32;
		uint[] array = new uint[16];
		uint[] array2 = new uint[16];
		uint[] array3 = new uint[num];
		uint[] array4 = new uint[num];
		uint[][] array5 = new uint[N][];
		try
		{
			Array.Copy(B, BOff, array4, 0, num);
			for (int i = 0; i < N; i++)
			{
				array5[i] = (uint[])array4.Clone();
				BlockMix(array4, array, array2, array3, r);
			}
			uint num2 = (uint)(N - 1);
			for (int j = 0; j < N; j++)
			{
				uint num3 = array4[num - 16] & num2;
				Xor(array4, array5[num3], 0, array4);
				BlockMix(array4, array, array2, array3, r);
			}
			Array.Copy(array4, 0, B, BOff, num);
		}
		finally
		{
			Array[] arrays = array5;
			ClearAll(arrays);
			ClearAll(array4, array, array2, array3);
		}
	}

	private static void BlockMix(uint[] B, uint[] X1, uint[] X2, uint[] Y, int r)
	{
		Array.Copy(B, B.Length - 16, X1, 0, 16);
		int num = 0;
		int num2 = 0;
		int num3 = B.Length >> 1;
		for (int num4 = 2 * r; num4 > 0; num4--)
		{
			Xor(X1, B, num, X2);
			Salsa20Engine.SalsaCore(8, X2, X1);
			Array.Copy(X1, 0, Y, num2, 16);
			num2 = num3 + num - num2;
			num += 16;
		}
		Array.Copy(Y, 0, B, 0, Y.Length);
	}

	private static void Xor(uint[] a, uint[] b, int bOff, uint[] output)
	{
		for (int num = output.Length - 1; num >= 0; num--)
		{
			output[num] = a[num] ^ b[bOff + num];
		}
	}

	private static void Clear(Array array)
	{
		if (array != null)
		{
			Array.Clear(array, 0, array.Length);
		}
	}

	private static void ClearAll(params Array[] arrays)
	{
		for (int i = 0; i < arrays.Length; i++)
		{
			Clear(arrays[i]);
		}
	}

	private static bool IsPowerOf2(int x)
	{
		return (x & (x - 1)) == 0;
	}
}
