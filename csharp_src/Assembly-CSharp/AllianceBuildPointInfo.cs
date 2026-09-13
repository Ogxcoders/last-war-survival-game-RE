using Protobuf;

public class AllianceBuildPointInfo : PointInfo
{
	public int buildId;

	public int buildState;

	public int offter_range;

	public string alAbbr;

	public string allianceId;

	public int fightState;

	public bool isCreate;

	public int assistanceCount;

	public int maxAssistanceCount;

	public AllianceFurnaceInfo furnaceInfo;

	public ShieldSkillInfo shieldSkillInfo;

	public void UpdateAssistanceCount()
	{
		base.PointManager?.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out assistanceCount, out maxAssistanceCount);
	}

	public AllianceBuildPointInfo()
	{
	}

	public override PointInfo Clone()
	{
		AllianceBuildPointInfo allianceBuildPointInfo = new AllianceBuildPointInfo();
		BaseClone(allianceBuildPointInfo);
		allianceBuildPointInfo.buildId = buildId;
		allianceBuildPointInfo.buildState = buildState;
		allianceBuildPointInfo.offter_range = offter_range;
		allianceBuildPointInfo.alAbbr = alAbbr;
		allianceBuildPointInfo.allianceId = allianceId;
		allianceBuildPointInfo.fightState = fightState;
		allianceBuildPointInfo.isCreate = isCreate;
		allianceBuildPointInfo.assistanceCount = assistanceCount;
		allianceBuildPointInfo.maxAssistanceCount = maxAssistanceCount;
		allianceBuildPointInfo.furnaceInfo = furnaceInfo;
		allianceBuildPointInfo.shieldSkillInfo = shieldSkillInfo;
		return allianceBuildPointInfo;
	}

	public AllianceBuildPointInfo(WorldPointInfo pi, bool isCreate)
		: base(pi)
	{
		tileSize = 3;
		if (isCreate)
		{
			this.isCreate = true;
		}
		AllianceBuildingPointInfo allianceBuildingPointInfo = AllianceBuildingPointInfo.Parser.ParseFrom(extraInfo);
		if (allianceBuildingPointInfo != null)
		{
			fightState = allianceBuildingPointInfo.FightState;
			buildId = allianceBuildingPointInfo.BuildId;
			buildState = allianceBuildingPointInfo.State;
			alAbbr = allianceBuildingPointInfo.AlAbbr;
			allianceId = allianceBuildingPointInfo.AllianceId;
			furnaceInfo = allianceBuildingPointInfo.AllianceFurnaceInfo;
			shieldSkillInfo = allianceBuildingPointInfo.ShieldSkillInfo;
			string tabName = "alliance_res_build";
			string str = GameEntry.ConfigCache.TryGetTemplateData(tabName, buildId, "res_size");
			if (!str.IsNullOrEmpty())
			{
				tileSize = str.ToInt();
			}
			else if (buildId == 200000 || buildId == 201000)
			{
				tileSize = 5;
			}
			else if (buildId == 300000 || buildId == 301000)
			{
				tileSize = 9;
			}
			else if (buildId == 400000 || buildId == 401000)
			{
				tileSize = 9;
			}
		}
		PointInfo pointInfoByUuid = SceneManager.World.GetPointInfoByUuid(pi.Uuid);
		if (pointInfoByUuid != null && pointInfoByUuid is AllianceBuildPointInfo allianceBuildPointInfo)
		{
			offter_range = allianceBuildPointInfo.offter_range;
		}
		UpdateAssistanceCount();
	}

	public bool IsGuardianTower()
	{
		if (shieldSkillInfo != null && buildState != 2 && shieldSkillInfo.SkillId == 10005)
		{
			return shieldSkillInfo.OverTime > GameEntry.Timer.GetServerTime();
		}
		return false;
	}

	public override PlayerType GetPlayerType()
	{
		string text = GameEntry.Data.Player.GetAllianceId();
		if (string.IsNullOrEmpty(text))
		{
			return PlayerType.PlayerOther;
		}
		AllianceBuildingPointInfo allianceBuildingPointInfo = AllianceBuildingPointInfo.Parser.ParseFrom(extraInfo);
		if (allianceBuildingPointInfo != null && allianceBuildingPointInfo.AllianceId == text)
		{
			return PlayerType.PlayerAlliance;
		}
		return PlayerType.PlayerOther;
	}
}
