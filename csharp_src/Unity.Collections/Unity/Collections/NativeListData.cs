namespace Unity.Collections;

internal struct NativeListData
{
	public unsafe void* buffer;

	public int length;

	public int capacity;
}
