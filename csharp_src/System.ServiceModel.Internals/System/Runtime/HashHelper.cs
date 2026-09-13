namespace System.Runtime;

internal static class HashHelper
{
	public static byte[] ComputeHash(byte[] buffer)
	{
		int[] array = new int[16]
		{
			7, 12, 17, 22, 5, 9, 14, 20, 4, 11,
			16, 23, 6, 10, 15, 21
		};
		uint[] array2 = new uint[64]
		{
			3614090360u, 3905402710u, 606105819u, 3250441966u, 4118548399u, 1200080426u, 2821735955u, 4249261313u, 1770035416u, 2336552879u,
			4294925233u, 2304563134u, 1804603682u, 4254626195u, 2792965006u, 1236535329u, 4129170786u, 3225465664u, 643717713u, 3921069994u,
			3593408605u, 38016083u, 3634488961u, 3889429448u, 568446438u, 3275163606u, 4107603335u, 1163531501u, 2850285829u, 4243563512u,
			1735328473u, 2368359562u, 4294588738u, 2272392833u, 1839030562u, 4259657740u, 2763975236u, 1272893353u, 4139469664u, 3200236656u,
			681279174u, 3936430074u, 3572445317u, 76029189u, 3654602809u, 3873151461u, 530742520u, 3299628645u, 4096336452u, 1126891415u,
			2878612391u, 4237533241u, 1700485571u, 2399980690u, 4293915773u, 2240044497u, 1873313359u, 4264355552u, 2734768916u, 1309151649u,
			4149444226u, 3174756917u, 718787259u, 3951481745u
		};
		int num = (buffer.Length + 8) / 64 + 1;
		uint num2 = 1732584193u;
		uint num3 = 4023233417u;
		uint num4 = 2562383102u;
		uint num5 = 271733878u;
		for (int i = 0; i < num; i++)
		{
			byte[] array3 = buffer;
			int num6 = i * 64;
			if (num6 + 64 > buffer.Length)
			{
				array3 = new byte[64];
				for (int j = num6; j < buffer.Length; j++)
				{
					array3[j - num6] = buffer[j];
				}
				if (num6 <= buffer.Length)
				{
					array3[buffer.Length - num6] = 128;
				}
				if (i == num - 1)
				{
					array3[56] = (byte)(buffer.Length << 3);
					array3[57] = (byte)(buffer.Length >> 5);
					array3[58] = (byte)(buffer.Length >> 13);
					array3[59] = (byte)(buffer.Length >> 21);
				}
				num6 = 0;
			}
			uint num7 = num2;
			uint num8 = num3;
			uint num9 = num4;
			uint num10 = num5;
			for (int k = 0; k < 64; k++)
			{
				uint num11;
				int num12;
				if (k < 16)
				{
					num11 = (num8 & num9) | (~num8 & num10);
					num12 = k;
				}
				else if (k < 32)
				{
					num11 = (num8 & num10) | (num9 & ~num10);
					num12 = 5 * k + 1;
				}
				else if (k < 48)
				{
					num11 = num8 ^ num9 ^ num10;
					num12 = 3 * k + 5;
				}
				else
				{
					num11 = num9 ^ (num8 | ~num10);
					num12 = 7 * k;
				}
				num12 = (num12 & 0xF) * 4 + num6;
				uint num13 = num10;
				num10 = num9;
				num9 = num8;
				num8 = num7 + num11 + array2[k] + (uint)(array3[num12] + (array3[num12 + 1] << 8) + (array3[num12 + 2] << 16) + (array3[num12 + 3] << 24));
				num8 = (num8 << array[(k & 3) | ((k >> 2) & -4)]) | (num8 >> 32 - array[(k & 3) | ((k >> 2) & -4)]);
				num8 += num9;
				num7 = num13;
			}
			num2 += num7;
			num3 += num8;
			num4 += num9;
			num5 += num10;
		}
		return new byte[16]
		{
			(byte)num2,
			(byte)(num2 >> 8),
			(byte)(num2 >> 16),
			(byte)(num2 >> 24),
			(byte)num3,
			(byte)(num3 >> 8),
			(byte)(num3 >> 16),
			(byte)(num3 >> 24),
			(byte)num4,
			(byte)(num4 >> 8),
			(byte)(num4 >> 16),
			(byte)(num4 >> 24),
			(byte)num5,
			(byte)(num5 >> 8),
			(byte)(num5 >> 16),
			(byte)(num5 >> 24)
		};
	}
}
