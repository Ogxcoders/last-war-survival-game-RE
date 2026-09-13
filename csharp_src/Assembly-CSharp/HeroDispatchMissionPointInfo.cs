using System.Collections.Generic;
using System.Linq;
using Protobuf;

public class HeroDispatchMissionPointInfo : PointInfo
{
	public int cfgId;

	public long completionTime;

	public List<string> stealList;

	public List<long> heroList;

	public int rewarded;

	public long actEndTime;

	public List<string> accList;

	public string allianceId;

	public long expiredTime;

	public HeroDispatchMissionPointInfo()
	{
	}

	public HeroDispatchMissionPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		Protobuf.HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = pi.HeroDispatchMissionPointInfo;
		ownerUid = heroDispatchMissionPointInfo.OwnerUid;
		cfgId = heroDispatchMissionPointInfo.CfgId;
		completionTime = heroDispatchMissionPointInfo.CompletionTime;
		stealList = heroDispatchMissionPointInfo.StealList.ToArray().ToList();
		heroList = heroDispatchMissionPointInfo.HeroList.ToArray().ToList();
		rewarded = heroDispatchMissionPointInfo.Rewarded;
		actEndTime = heroDispatchMissionPointInfo.ActEndTime;
		accList = heroDispatchMissionPointInfo.AccList.ToArray().ToList();
		allianceId = heroDispatchMissionPointInfo.AllianceId;
		tileSize = heroDispatchMissionPointInfo.Size;
		if (GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", cfgId, "is_special").ToInt() == 1)
		{
			expiredTime = completionTime + 259200000;
		}
		else
		{
			expiredTime = completionTime + 86400000;
		}
	}

	public override PointInfo Clone()
	{
		HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = new HeroDispatchMissionPointInfo();
		BaseClone(heroDispatchMissionPointInfo);
		heroDispatchMissionPointInfo.cfgId = cfgId;
		heroDispatchMissionPointInfo.completionTime = completionTime;
		heroDispatchMissionPointInfo.stealList = stealList;
		heroDispatchMissionPointInfo.heroList = heroList;
		heroDispatchMissionPointInfo.rewarded = rewarded;
		heroDispatchMissionPointInfo.actEndTime = actEndTime;
		heroDispatchMissionPointInfo.accList = accList;
		heroDispatchMissionPointInfo.allianceId = allianceId;
		heroDispatchMissionPointInfo.expiredTime = expiredTime;
		return heroDispatchMissionPointInfo;
	}
}
