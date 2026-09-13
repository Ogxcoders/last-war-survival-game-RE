namespace Unity.Collections;

internal struct NativeQueueBlockHeader
{
	public unsafe byte* nextBlock;

	public int itemsInBlock;
}
