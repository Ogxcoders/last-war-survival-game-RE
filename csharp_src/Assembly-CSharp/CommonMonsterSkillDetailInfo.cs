using Protobuf;
using Sfs2X.Entities.Data;

public class CommonMonsterSkillDetailInfo
{
	public int aimByPoint;

	public long aimEndTime;

	public long aimBossUuid;

	public int skillId;

	public CommonMonsterSkillDetailInfo()
	{
	}

	public CommonMonsterSkillDetailInfo(Protobuf.CommonMonsterSkillDetailInfo msg)
	{
		ParseData(msg);
	}

	public virtual void ParseData(ISFSObject msg)
	{
		aimByPoint = msg.TryGetInt("AimByPoint");
		aimEndTime = msg.TryGetLong("AimEndTime");
		aimBossUuid = msg.TryGetLong("aimBossUuid");
		skillId = msg.TryGetInt("skillId");
	}

	protected virtual void ParseData(Protobuf.CommonMonsterSkillDetailInfo msg)
	{
		aimByPoint = msg.AimByPoint;
		aimEndTime = msg.AimEndTime;
		aimBossUuid = msg.AimBossUuid;
		skillId = msg.SkillId;
	}
}
