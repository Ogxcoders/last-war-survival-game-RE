using System;
using System.Text;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public static class GameHashUtil
{
	public static int GameHashFNV1A32(this string str)
	{
		return Encoding.UTF8.GetBytes(str).GameHashFNV1A32();
	}

	public static int GameHashFNV1A32(this byte[] obj)
	{
		uint num = 2166136261u;
		num = 2166136261u;
		for (int i = 0; i < obj.Length; i++)
		{
			num = (obj[i] ^ num) * 16777619;
		}
		return (int)num;
	}

	public static int GameHashFNV1A32(this Type type)
	{
		return type.FullName.GameHashFNV1A32();
	}

	public static long GameHashCodeFNV164(this Type type)
	{
		return type.FullName.GameHashCodeFNV1A64();
	}

	public static long GameHashCodeFNV1A64(this string str)
	{
		return Encoding.UTF8.GetBytes(str).GameHashCodeFNV1A64();
	}

	public static long GameHashCodeFNV1A64(this byte[] obj)
	{
		ulong num = 14695981039346656037uL;
		for (int i = 0; i < obj.Length; i++)
		{
			num = (obj[i] ^ num) * 1099511628211L;
		}
		return (long)num;
	}

	public static long GameHashCodeFNV1A64(this Type type)
	{
		return type.FullName.GameHashCodeFNV1A64();
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

	public static int MergeHashCode(int a, int b)
	{
		return MixFinal((RotateLeft(a, 5) + a) ^ b);
	}

	public static long MergeHashCode(long a, long b)
	{
		return MixFinal((RotateLeft(a, 5) + a) ^ b);
	}

	public static long MixFinal(long hash)
	{
		hash ^= hash >> 33;
		hash *= -49064778989728563L;
		hash ^= hash >> 33;
		hash *= -4265267296055464877L;
		hash ^= hash >> 33;
		return hash;
	}

	public static long RotateLeft(long value, int offset)
	{
		return (value << offset) | (value >> 64 - offset);
	}

	public static long GetComponentsHashCode(EcsWorld world, int entity)
	{
		Type[] list = null;
		world.GetComponentTypes(entity, ref list);
		long num = 0L;
		for (int i = 0; i < list.Length; i++)
		{
			if (!(list[i] == null))
			{
				num = ((num != 0L) ? MergeHashCode(num, list[i].GameHashCodeFNV1A64()) : list[i].GameHashCodeFNV1A64());
			}
		}
		return num;
	}
}
