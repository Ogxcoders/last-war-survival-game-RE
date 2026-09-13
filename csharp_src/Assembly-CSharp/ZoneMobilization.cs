using Protobuf;
using Sfs2X.Entities.Data;

public class ZoneMobilization
{
	public int aimByPoint;

	public long aimEndTime;

	public long aimBossUuid;

	public ZoneMobilization()
	{
	}

	public ZoneMobilization(ISFSObject msg)
	{
		ParseData(msg);
	}

	public ZoneMobilization(Protobuf.ZoneMobilization msg)
	{
		ParseData(msg);
	}

	public virtual void ParseData(ISFSObject msg)
	{
		aimByPoint = msg.TryGetInt("AimByPoint");
		aimEndTime = msg.TryGetLong("AimEndTime");
		aimBossUuid = msg.TryGetLong("aimBossUuid");
	}

	protected virtual void ParseData(Protobuf.ZoneMobilization msg)
	{
		aimByPoint = msg.AimByPoint;
		aimEndTime = msg.AimEndTime;
		aimBossUuid = msg.AimBossUuid;
	}
}
