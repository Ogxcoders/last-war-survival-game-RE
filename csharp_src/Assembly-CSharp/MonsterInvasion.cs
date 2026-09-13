using Protobuf;
using Sfs2X.Entities.Data;

public class MonsterInvasion
{
	public int aimByPoint;

	public long aimEndTime;

	public MonsterInvasion()
	{
	}

	public MonsterInvasion(ISFSObject msg)
	{
		ParseData(msg);
	}

	public MonsterInvasion(Protobuf.MonsterInvasion msg)
	{
		ParseData(msg);
	}

	public virtual void ParseData(ISFSObject msg)
	{
		aimByPoint = msg.TryGetInt("AimByPoint");
		aimEndTime = msg.TryGetLong("AimEndTime");
	}

	protected virtual void ParseData(Protobuf.MonsterInvasion msg)
	{
		aimByPoint = msg.AimByPoint;
		aimEndTime = msg.AimEndTime;
	}
}
