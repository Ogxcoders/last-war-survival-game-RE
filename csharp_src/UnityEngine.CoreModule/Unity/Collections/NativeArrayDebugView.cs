namespace Unity.Collections;

internal sealed class NativeArrayDebugView<T> where T : struct
{
	private NativeArray<T> m_Array;

	public T[] Items => m_Array.ToArray();

	public NativeArrayDebugView(NativeArray<T> array)
	{
		m_Array = array;
	}
}
