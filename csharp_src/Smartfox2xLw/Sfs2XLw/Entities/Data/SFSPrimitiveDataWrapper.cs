namespace Sfs2XLw.Entities.Data;

public class SFSPrimitiveDataWrapper<T> : SFSDataWrapper where T : struct
{
	private T _data;

	public override object Data => _data;

	public T Value => _data;

	public SFSPrimitiveDataWrapper(int type, T data, int keyByteCount)
		: base(type, keyByteCount)
	{
		_data = data;
	}

	public SFSPrimitiveDataWrapper(SFSDataType tp, T data, int keyByteCount)
		: base(tp, keyByteCount)
	{
		_data = data;
	}
}
