using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections.Experimental;

internal struct StructLayoutData4
{
	internal struct FieldInfo
	{
		public int Offset;
	}

	public const int ChunkSizeBytes = 32;

	public const int ElementSize = 4;

	public const int ElementsPerChunk = 8;

	private FieldInfo[] m_Fields;

	public bool IsCreated => m_Fields != null;

	public int FieldCount => m_Fields.Length;

	public StructLayoutData4(Type t)
	{
		m_Fields = ComputeFieldInfo(t);
	}

	public int ChunksNeeded(int count)
	{
		return count + 8 - 1 >> 3;
	}

	public int ChunkIndex(int element)
	{
		return element >> 3;
	}

	public int ChunkOffset(int element)
	{
		return element & 7;
	}

	public FieldInfo GetFieldInfo(int attrIndex)
	{
		return m_Fields[attrIndex];
	}

	private static FieldInfo[] ComputeFieldInfo(Type t)
	{
		List<FieldInfo> list = new List<FieldInfo>();
		FindFields(list, t, 0);
		return list.ToArray();
	}

	private static void FindFields(List<FieldInfo> result, Type type, int parentOffset)
	{
		System.Reflection.FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (System.Reflection.FieldInfo fieldInfo in fields)
		{
			int num = parentOffset + UnsafeUtility.GetFieldOffset(fieldInfo);
			if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType.IsPointer)
			{
				int num2 = -1;
				num2 = ((!fieldInfo.FieldType.IsPointer) ? UnsafeUtility.SizeOf(fieldInfo.FieldType) : UnsafeUtility.SizeOf<IntPtr>());
				if ((num2 & (num2 - 1)) != 0)
				{
					throw new ArgumentException($"Field {type}.{fieldInfo} is of size {num2} which is not a power of two");
				}
				if (num2 != 4)
				{
					throw new ArgumentException($"Field {type}.{fieldInfo} is of size {num2}; currently only types of size {4} bytes are allowed");
				}
				result.Add(new FieldInfo
				{
					Offset = num
				});
			}
			else
			{
				FindFields(result, fieldInfo.FieldType, num);
			}
		}
	}
}
