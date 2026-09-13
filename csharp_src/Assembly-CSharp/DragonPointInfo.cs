using Protobuf;

public class DragonPointInfo : PointInfo
{
	public DragonBuildingPointInfo detail;

	public int assistanceCount;

	public int maxAssistanceCount;

	public DragonPointInfo()
	{
	}

	public DragonPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		detail = DragonBuildingPointInfo.Parser.ParseFrom(extraInfo);
		tileSize = SceneManager.World.GetDragonBuildSizeByItemId(detail.BuildId);
		UpdateAssistanceCount();
	}

	public override PointInfo Clone()
	{
		DragonPointInfo dragonPointInfo = new DragonPointInfo();
		BaseClone(dragonPointInfo);
		dragonPointInfo.detail = detail;
		dragonPointInfo.assistanceCount = assistanceCount;
		dragonPointInfo.maxAssistanceCount = maxAssistanceCount;
		return dragonPointInfo;
	}

	public override PlayerType GetPlayerType()
	{
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		if (string.IsNullOrEmpty(allianceId))
		{
			return PlayerType.PlayerOther;
		}
		if (detail != null && !detail.AllianceId.IsNullOrEmpty())
		{
			if (detail.AllianceId == allianceId)
			{
				return PlayerType.PlayerAlliance;
			}
			return PlayerType.PlayerOther;
		}
		return PlayerType.PlayerNone;
	}

	public void UpdateAssistanceCount()
	{
		base.PointManager?.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out assistanceCount, out maxAssistanceCount);
	}
}
