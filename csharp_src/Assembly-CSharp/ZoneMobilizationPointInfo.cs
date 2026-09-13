using Protobuf;
using Sfs2X.Entities.Data;

public class ZoneMobilizationPointInfo
{
	public int bossId;

	public int stage;

	public bool donateMax;

	public ZoneMobilizationPointInfo()
	{
	}

	public ZoneMobilizationPointInfo(ISFSObject msg)
	{
		ParseData(msg);
	}

	public ZoneMobilizationPointInfo(Protobuf.ZoneMobilizationPointInfo msg)
	{
		ParseData(msg);
	}

	public virtual void ParseData(ISFSObject msg)
	{
		bossId = msg.TryGetInt("bossId");
		stage = msg.TryGetInt("stage");
		donateMax = msg.TryGetBool("donateMax");
	}

	protected virtual void ParseData(Protobuf.ZoneMobilizationPointInfo msg)
	{
		bossId = msg.BossId;
		stage = msg.Stage;
		donateMax = msg.DonateMax;
	}
}
