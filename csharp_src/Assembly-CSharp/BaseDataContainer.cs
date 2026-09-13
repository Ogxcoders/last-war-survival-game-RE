using Sfs2X.Entities.Data;

public abstract class BaseDataContainer
{
	public void Update(ISFSObject obj)
	{
		CSUpdate(obj);
	}

	public void Init(ISFSObject obj)
	{
		CSInit(obj);
	}

	public virtual void CSUpdate(ISFSObject obj)
	{
	}

	public virtual void CSInit(ISFSObject obj)
	{
	}

	public virtual void Release()
	{
	}
}
