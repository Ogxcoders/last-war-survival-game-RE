using System;
using System.Collections.Generic;
using Protobuf;

public class HeroInfo : IDisposable
{
	public int heroId;

	public long heroUuid;

	public int heroLevel;

	public int heroQuality;

	public int index;

	public int rankLv;

	public int stage;

	public int weaponLevel;

	public List<HeroSkillInfo> skillInfos = new List<HeroSkillInfo>();

	public DominatorInfo dominatorInfo;

	private static ThreadSafeObjectPool<HeroSkillInfo> _skillInfoPool = new ThreadSafeObjectPool<HeroSkillInfo>(() => new HeroSkillInfo());

	private static ThreadSafeObjectPool<DominatorInfo> _dominatorInfoPool = new ThreadSafeObjectPool<DominatorInfo>(() => new DominatorInfo());

	public static void ReleasePools(int remainCount)
	{
		_skillInfoPool.Release(remainCount);
		_dominatorInfoPool.Release(remainCount);
	}

	public void Dispose()
	{
		skillInfos.Clear();
		if (dominatorInfo != null)
		{
			_dominatorInfoPool.Recycle(dominatorInfo);
		}
	}

	public void UpdateHeroInfo(HeroInfoProto proto)
	{
		heroId = proto.HeroId;
		heroLevel = proto.HeroLevel;
		heroQuality = proto.HeroQuality;
		index = proto.Index;
		rankLv = proto.RankLv;
		stage = proto.Stage;
		skillInfos.Clear();
		foreach (HeroSkillInfoProto skillInfo in proto.SkillInfos)
		{
			HeroSkillInfo heroSkillInfo = new HeroSkillInfo();
			heroSkillInfo.UpdateHeroSkill(skillInfo);
			skillInfos.Add(heroSkillInfo);
		}
		weaponLevel = proto.WeaponLevel;
		if (proto.Dominator != null)
		{
			dominatorInfo = _dominatorInfoPool.Allocate();
			dominatorInfo.UpdateDominator(proto.Dominator);
		}
		else
		{
			dominatorInfo = null;
		}
	}

	public bool GetIsAllSKillReachMax()
	{
		int num = 0;
		int num2 = 0;
		foreach (HeroSkillInfo skillInfo in skillInfos)
		{
			if (skillInfo.skillLv > 0)
			{
				num++;
			}
			num2 += skillInfo.skillLv;
		}
		if (num2 >= num * 5 && num == skillInfos.Count)
		{
			return true;
		}
		return false;
	}
}
