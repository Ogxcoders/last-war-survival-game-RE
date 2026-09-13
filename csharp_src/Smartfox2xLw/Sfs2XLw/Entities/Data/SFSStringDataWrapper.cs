namespace Sfs2XLw.Entities.Data;

public class SFSStringDataWrapper : SFSRefDataWrapper
{
	private int valueByteCount;

	public int ValueByteCount => valueByteCount;

	public SFSStringDataWrapper(int type, object data, int keyByteCount, int valueByteCount)
		: base(type, data, keyByteCount)
	{
		this.valueByteCount = valueByteCount;
	}

	public SFSStringDataWrapper(SFSDataType tp, object data, int keyByteCount, int valueByteCount)
		: base(tp, data, keyByteCount)
	{
		this.valueByteCount = valueByteCount;
	}
}
