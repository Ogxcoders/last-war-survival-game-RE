using Google.Protobuf.Collections;
using Protobuf;

public class CommonMonsterSkillInfo
{
	public RepeatedField<Protobuf.CommonMonsterSkillDetailInfo> detailInfo;

	public CommonMonsterSkillInfo()
	{
	}

	public CommonMonsterSkillInfo(Protobuf.CommonMonsterSkillInfo msg)
	{
		ParseData(msg);
	}

	protected virtual void ParseData(Protobuf.CommonMonsterSkillInfo msg)
	{
		detailInfo = msg.DetailInfo;
	}
}
