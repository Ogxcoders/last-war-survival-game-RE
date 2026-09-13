namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;

internal abstract class Interleave
{
	private const ulong M32 = 1431655765uL;

	private const ulong M64 = 6148914691236517205uL;

	private const ulong M64R = 12297829382473034410uL;

	internal static uint Expand8to16(uint x)
	{
		x &= 0xFF;
		x = (x | (x << 4)) & 0xF0F;
		x = (x | (x << 2)) & 0x3333;
		x = (x | (x << 1)) & 0x5555;
		return x;
	}

	internal static uint Expand16to32(uint x)
	{
		x &= 0xFFFF;
		x = (x | (x << 8)) & 0xFF00FF;
		x = (x | (x << 4)) & 0xF0F0F0F;
		x = (x | (x << 2)) & 0x33333333;
		x = (x | (x << 1)) & 0x55555555;
		return x;
	}

	internal static ulong Expand32to64(uint x)
	{
		uint num = (x ^ (x >> 8)) & 0xFF00;
		x ^= num ^ (num << 8);
		num = (x ^ (x >> 4)) & 0xF000F0;
		x ^= num ^ (num << 4);
		num = (x ^ (x >> 2)) & 0xC0C0C0C;
		x ^= num ^ (num << 2);
		num = (x ^ (x >> 1)) & 0x22222222;
		x ^= num ^ (num << 1);
		return (((ulong)(x >> 1) & 0x55555555uL) << 32) | ((ulong)x & 0x55555555uL);
	}

	internal static void Expand64To128(ulong x, ulong[] z, int zOff)
	{
		ulong num = (x ^ (x >> 16)) & 0xFFFF0000u;
		x ^= num ^ (num << 16);
		num = (x ^ (x >> 8)) & 0xFF000000FF00L;
		x ^= num ^ (num << 8);
		num = (x ^ (x >> 4)) & 0xF000F000F000F0L;
		x ^= num ^ (num << 4);
		num = (x ^ (x >> 2)) & 0xC0C0C0C0C0C0C0CL;
		x ^= num ^ (num << 2);
		num = (x ^ (x >> 1)) & 0x2222222222222222L;
		x ^= num ^ (num << 1);
		z[zOff] = x & 0x5555555555555555L;
		z[zOff + 1] = (x >> 1) & 0x5555555555555555L;
	}

	internal static void Expand64To128Rev(ulong x, ulong[] z, int zOff)
	{
		ulong num = (x ^ (x >> 16)) & 0xFFFF0000u;
		x ^= num ^ (num << 16);
		num = (x ^ (x >> 8)) & 0xFF000000FF00L;
		x ^= num ^ (num << 8);
		num = (x ^ (x >> 4)) & 0xF000F000F000F0L;
		x ^= num ^ (num << 4);
		num = (x ^ (x >> 2)) & 0xC0C0C0C0C0C0C0CL;
		x ^= num ^ (num << 2);
		num = (x ^ (x >> 1)) & 0x2222222222222222L;
		x ^= num ^ (num << 1);
		z[zOff] = x & 0xAAAAAAAAAAAAAAAAuL;
		z[zOff + 1] = (x << 1) & 0xAAAAAAAAAAAAAAAAuL;
	}

	internal static uint Shuffle(uint x)
	{
		uint num = (x ^ (x >> 8)) & 0xFF00;
		x ^= num ^ (num << 8);
		num = (x ^ (x >> 4)) & 0xF000F0;
		x ^= num ^ (num << 4);
		num = (x ^ (x >> 2)) & 0xC0C0C0C;
		x ^= num ^ (num << 2);
		num = (x ^ (x >> 1)) & 0x22222222;
		x ^= num ^ (num << 1);
		return x;
	}

	internal static ulong Shuffle(ulong x)
	{
		ulong num = (x ^ (x >> 16)) & 0xFFFF0000u;
		x ^= num ^ (num << 16);
		num = (x ^ (x >> 8)) & 0xFF000000FF00L;
		x ^= num ^ (num << 8);
		num = (x ^ (x >> 4)) & 0xF000F000F000F0L;
		x ^= num ^ (num << 4);
		num = (x ^ (x >> 2)) & 0xC0C0C0C0C0C0C0CL;
		x ^= num ^ (num << 2);
		num = (x ^ (x >> 1)) & 0x2222222222222222L;
		x ^= num ^ (num << 1);
		return x;
	}

	internal static uint Shuffle2(uint x)
	{
		uint num = (x ^ (x >> 7)) & 0xAA00AA;
		x ^= num ^ (num << 7);
		num = (x ^ (x >> 14)) & 0xCCCC;
		x ^= num ^ (num << 14);
		num = (x ^ (x >> 4)) & 0xF000F0;
		x ^= num ^ (num << 4);
		num = (x ^ (x >> 8)) & 0xFF00;
		x ^= num ^ (num << 8);
		return x;
	}

	internal static uint Unshuffle(uint x)
	{
		uint num = (x ^ (x >> 1)) & 0x22222222;
		x ^= num ^ (num << 1);
		num = (x ^ (x >> 2)) & 0xC0C0C0C;
		x ^= num ^ (num << 2);
		num = (x ^ (x >> 4)) & 0xF000F0;
		x ^= num ^ (num << 4);
		num = (x ^ (x >> 8)) & 0xFF00;
		x ^= num ^ (num << 8);
		return x;
	}

	internal static ulong Unshuffle(ulong x)
	{
		ulong num = (x ^ (x >> 1)) & 0x2222222222222222L;
		x ^= num ^ (num << 1);
		num = (x ^ (x >> 2)) & 0xC0C0C0C0C0C0C0CL;
		x ^= num ^ (num << 2);
		num = (x ^ (x >> 4)) & 0xF000F000F000F0L;
		x ^= num ^ (num << 4);
		num = (x ^ (x >> 8)) & 0xFF000000FF00L;
		x ^= num ^ (num << 8);
		num = (x ^ (x >> 16)) & 0xFFFF0000u;
		x ^= num ^ (num << 16);
		return x;
	}

	internal static uint Unshuffle2(uint x)
	{
		uint num = (x ^ (x >> 8)) & 0xFF00;
		x ^= num ^ (num << 8);
		num = (x ^ (x >> 4)) & 0xF000F0;
		x ^= num ^ (num << 4);
		num = (x ^ (x >> 14)) & 0xCCCC;
		x ^= num ^ (num << 14);
		num = (x ^ (x >> 7)) & 0xAA00AA;
		x ^= num ^ (num << 7);
		return x;
	}
}
