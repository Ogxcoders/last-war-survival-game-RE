namespace Unity.Collections;

internal sealed class NativeListDebugView<T> where T : struct
{
	private NativeList<T> m_Array;

	public T[] Items => m_Array.ToArray();

	public NativeListDebugView(NativeList<T> array)
	{
		m_Array = array;
	}
}
