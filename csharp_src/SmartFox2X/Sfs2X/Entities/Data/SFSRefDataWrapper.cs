namespace Sfs2X.Entities.Data;

public class SFSRefDataWrapper : SFSDataWrapper
{
	private object _data;

	public override object Data => _data;

	public SFSRefDataWrapper(int type, object data)
		: base(type)
	{
		_data = data;
	}

	public SFSRefDataWrapper(SFSDataType tp, object data)
		: base(tp)
	{
		_data = data;
	}
}
