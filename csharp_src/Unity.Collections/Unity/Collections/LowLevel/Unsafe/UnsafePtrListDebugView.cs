using System;

namespace Unity.Collections.LowLevel.Unsafe;

internal sealed class UnsafePtrListDebugView
{
	private UnsafePtrList m_UnsafePtrList;

	public unsafe IntPtr[] Items
	{
		get
		{
			IntPtr[] array = new IntPtr[m_UnsafePtrList.m_size];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (IntPtr)m_UnsafePtrList.m_pointer[i];
			}
			return array;
		}
	}

	public UnsafePtrListDebugView(UnsafePtrList UnsafePtrList)
	{
		m_UnsafePtrList = UnsafePtrList;
	}
}
