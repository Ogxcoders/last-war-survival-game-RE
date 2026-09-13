namespace Sfs2XLw.Entities.Data;

public class SFSRefDataWrapper : SFSDataWrapper
{
	private object _data;

	public override object Data => _data;

	public SFSRefDataWrapper(int type, object data, int keyByteCount)
		: base(type, keyByteCount)
	{
		_data = data;
	}

	public SFSRefDataWrapper(SFSDataType tp, object data, int keyByteCount)
		: base(tp, keyByteCount)
	{
		_data = data;
	}
}
