namespace Sfs2X.Entities.Data;

public class SFSPrimitiveDataWrapper<T> : SFSDataWrapper where T : struct
{
	private T _data;

	public override object Data => _data;

	public T Value => _data;

	public SFSPrimitiveDataWrapper(int type, T data)
		: base(type)
	{
		_data = data;
	}

	public SFSPrimitiveDataWrapper(SFSDataType tp, T data)
		: base(tp)
	{
		_data = data;
	}
}
