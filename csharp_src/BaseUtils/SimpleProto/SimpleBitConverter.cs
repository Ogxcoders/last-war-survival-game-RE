using System;

namespace SimpleProto;

public class SimpleBitConverter
{
	public static void GetBytes(bool value, byte[] dst, int startIndex)
	{
		dst[startIndex] = (byte)(value ? 1u : 0u);
	}

	public static void GetBytes(short value, byte[] dst, int startIndex)
	{
		dst[startIndex] = (byte)(value & 0xFF);
		dst[startIndex + 1] = (byte)((value >> 8) & 0xFF);
	}

	public static void GetBytes(int value, byte[] dst, int startIndex)
	{
		dst[startIndex] = (byte)(value & 0xFF);
		dst[startIndex + 1] = (byte)((value >> 8) & 0xFF);
		dst[startIndex + 2] = (byte)((value >> 16) & 0xFF);
		dst[startIndex + 3] = (byte)((value >> 24) & 0xFF);
	}

	public static void GetBytes(long value, byte[] dst, int startIndex)
	{
		dst[startIndex] = (byte)(value & 0xFF);
		dst[startIndex + 1] = (byte)((value >> 8) & 0xFF);
		dst[startIndex + 2] = (byte)((value >> 16) & 0xFF);
		dst[startIndex + 3] = (byte)((value >> 24) & 0xFF);
		dst[startIndex + 4] = (byte)((value >> 32) & 0xFF);
		dst[startIndex + 5] = (byte)((value >> 40) & 0xFF);
		dst[startIndex + 6] = (byte)((value >> 48) & 0xFF);
		dst[startIndex + 7] = (byte)((value >> 56) & 0xFF);
	}

	public static void GetBytes(float value, byte[] dst, int startIndex)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		byte[] array = ReverseOrder(bytes, 0, bytes.Length);
		System.Buffer.BlockCopy(array, 0, dst, startIndex, array.Length);
	}

	public static void GetBytes(double value, byte[] dst, int startIndex)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		byte[] array = ReverseOrder(bytes, 0, bytes.Length);
		System.Buffer.BlockCopy(array, 0, dst, startIndex, array.Length);
	}

	public static bool ToBool(byte[] value, int startIndex)
	{
		return value[startIndex] != 0;
	}

	public static short ToShort(byte[] value, int startIndex)
	{
		return (short)((value[startIndex] & 0xFF) | (value[startIndex + 1] << 8));
	}

	public static int ToInt(byte[] value, int startIndex)
	{
		return (value[startIndex] & 0xFF) | (value[startIndex + 1] << 8) | (value[startIndex + 2] << 16) | (value[startIndex + 3] << 24);
	}

	public static long ToLong(byte[] value, int startIndex)
	{
		int num = (value[startIndex] & 0xFF) | (value[startIndex + 1] << 8) | (value[startIndex + 2] << 16) | (value[startIndex + 3] << 24);
		int num2 = (value[4 + startIndex] & 0xFF) | (value[startIndex + 5] << 8) | (value[startIndex + 6] << 16) | (value[startIndex + 7] << 24);
		return num | ((long)num2 << 32);
	}

	public static float ToFloat(byte[] value, int startIndex)
	{
		return BitConverter.ToSingle(ReverseOrder(value, startIndex, 4), startIndex);
	}

	public static double ToDouble(byte[] value, int startIndex)
	{
		return BitConverter.ToDouble(ReverseOrder(value, startIndex, 8), startIndex);
	}

	private static byte[] ReverseOrder(byte[] buff, int startIndex, int length)
	{
		if (BitConverter.IsLittleEndian)
		{
			return buff;
		}
		if (length < 2)
		{
			return buff;
		}
		int num = length / 2;
		for (int i = 0; i < num; i++)
		{
			byte b = buff[i + startIndex];
			int num2 = length - i - 1 + startIndex;
			buff[i + startIndex] = buff[num2];
			buff[num2] = b;
		}
		return buff;
	}
}
