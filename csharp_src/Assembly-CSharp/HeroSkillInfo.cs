using System;
using Protobuf;

public class HeroSkillInfo : IDisposable
{
	public int skillId;

	public int skillLv;

	public void Dispose()
	{
	}

	public void UpdateHeroSkill(HeroSkillInfoProto proto)
	{
		skillId = proto.SkillId;
		skillLv = proto.SkillLv;
	}
}
