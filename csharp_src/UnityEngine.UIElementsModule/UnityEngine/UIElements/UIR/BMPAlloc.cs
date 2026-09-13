namespace UnityEngine.UIElements.UIR;

internal struct BMPAlloc
{
	public static readonly BMPAlloc Invalid = new BMPAlloc
	{
		page = -1
	};

	public int page;

	public ushort pageLine;

	public byte bitIndex;

	public byte owned;

	public bool Equals(BMPAlloc other)
	{
		return page == other.page && pageLine == other.pageLine && bitIndex == other.bitIndex;
	}

	public bool IsValid()
	{
		return page >= 0;
	}
}
