namespace Sfs2XLw.Entities.Data;

public abstract class SFSDataWrapper
{
	private int type;

	private int keyByteCount;

	public int Type => type;

	public int KeyByteCount => keyByteCount;

	public abstract object Data { get; }

	public SFSDataWrapper(int type, int keyByteCount)
	{
		this.type = type;
		this.keyByteCount = keyByteCount;
	}

	public SFSDataWrapper(SFSDataType tp, int keyByteCount)
	{
		type = (int)tp;
		this.keyByteCount = keyByteCount;
	}
}
