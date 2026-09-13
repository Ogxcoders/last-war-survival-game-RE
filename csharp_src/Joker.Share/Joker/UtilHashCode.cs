using System;
using System.Text;

namespace Joker;

public static class UtilHashCode
{
	public static int HashCodeFNV1A32(this string str)
	{
		return Encoding.UTF8.GetBytes(str).HashCodeFNV1A32();
	}

	public static int HashCodeFNV1A32(this byte[] obj)
	{
		uint num = 2166136261u;
		num = 2166136261u;
		for (int i = 0; i < obj.Length; i++)
		{
			num = (obj[i] ^ num) * 16777619;
		}
		return (int)num;
	}

	public static int HashCodeFNV1A32(this Type type)
	{
		return type.FullName.HashCodeFNV1A32();
	}

	public static long HashCodeFNV164(this Type type)
	{
		return type.FullName.HashCodeFNV1A64();
	}

	public static long HashCodeFNV1A64(this string str)
	{
		return Encoding.UTF8.GetBytes(str).HashCodeFNV1A64();
	}

	public static long HashCodeFNV1A64(this byte[] obj)
	{
		ulong num = 14695981039346656037uL;
		for (int i = 0; i < obj.Length; i++)
		{
			num = (obj[i] ^ num) * 1099511628211L;
		}
		return (long)num;
	}

	public static long HashCodeFNV1A64(this Type type)
	{
		return type.FullName.HashCodeFNV1A64();
	}

	public static int MixFinal(int hash)
	{
		hash ^= hash >> 16;
		hash *= -2048144789;
		hash ^= hash >> 13;
		hash *= -1028477387;
		hash ^= hash >> 16;
		return hash;
	}

	public static int RotateLeft(int value, int offset)
	{
		return (value << offset) | (value >> 32 - offset);
	}

	public static int GetHashCode<T0, T1>(T0 obj0, T1 obj1)
	{
		int hashCode = obj0.GetHashCode();
		int hashCode2 = obj1.GetHashCode();
		return MixFinal((RotateLeft(hashCode, 5) + hashCode) ^ hashCode2);
	}
}
