namespace Sfs2X.Entities.Data;

public abstract class SFSDataWrapper
{
	private int type;

	public int Type => type;

	public abstract object Data { get; }

	public SFSDataWrapper(int type)
	{
		this.type = type;
	}

	public SFSDataWrapper(SFSDataType tp)
	{
		type = (int)tp;
	}
}
