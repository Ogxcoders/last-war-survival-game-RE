using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm;

public sealed class Tables8kGcmMultiplier : IGcmMultiplier
{
	private byte[] H;

	private uint[][][] M;

	private uint[] z = new uint[4];

	public void Init(byte[] H)
	{
		if (M == null)
		{
			M = new uint[32][][];
		}
		else if (Arrays.AreEqual(this.H, H))
		{
			return;
		}
		this.H = Arrays.Clone(H);
		M[0] = new uint[16][];
		M[1] = new uint[16][];
		M[0][0] = new uint[4];
		M[1][0] = new uint[4];
		M[1][8] = GcmUtilities.AsUints(H);
		for (int num = 4; num >= 1; num >>= 1)
		{
			uint[] array = (uint[])M[1][num + num].Clone();
			GcmUtilities.MultiplyP(array);
			M[1][num] = array;
		}
		uint[] array2 = (uint[])M[1][1].Clone();
		GcmUtilities.MultiplyP(array2);
		M[0][8] = array2;
		for (int num2 = 4; num2 >= 1; num2 >>= 1)
		{
			uint[] array3 = (uint[])M[0][num2 + num2].Clone();
			GcmUtilities.MultiplyP(array3);
			M[0][num2] = array3;
		}
		int num3 = 0;
		while (true)
		{
			for (int i = 2; i < 16; i += i)
			{
				for (int j = 1; j < i; j++)
				{
					uint[] array4 = (uint[])M[num3][i].Clone();
					GcmUtilities.Xor(array4, M[num3][j]);
					M[num3][i + j] = array4;
				}
			}
			if (++num3 == 32)
			{
				break;
			}
			if (num3 > 1)
			{
				M[num3] = new uint[16][];
				M[num3][0] = new uint[4];
				for (int num4 = 8; num4 > 0; num4 >>= 1)
				{
					uint[] array5 = (uint[])M[num3 - 2][num4].Clone();
					GcmUtilities.MultiplyP8(array5);
					M[num3][num4] = array5;
				}
			}
		}
	}

	public void MultiplyH(byte[] x)
	{
		Array.Clear(z, 0, z.Length);
		for (int num = 15; num >= 0; num--)
		{
			uint[] array = M[num + num][x[num] & 0xF];
			z[0] ^= array[0];
			z[1] ^= array[1];
			z[2] ^= array[2];
			z[3] ^= array[3];
			array = M[num + num + 1][(x[num] & 0xF0) >> 4];
			z[0] ^= array[0];
			z[1] ^= array[1];
			z[2] ^= array[2];
			z[3] ^= array[3];
		}
		Pack.UInt32_To_BE(z, x, 0);
	}
}
