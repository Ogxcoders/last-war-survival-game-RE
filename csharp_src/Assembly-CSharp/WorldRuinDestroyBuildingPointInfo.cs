using Protobuf;

public class WorldRuinDestroyBuildingPointInfo : PointInfo
{
	public new int serverId;

	public string allianceId;

	public string alAbbr;

	public string userName;

	public long destroyEndTime;

	public WorldRuinDestroyBuildingPointInfo()
	{
	}

	public WorldRuinDestroyBuildingPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		RuinDestroyBuildingPointInfo ruinDestroyBuildingPointInfo = RuinDestroyBuildingPointInfo.Parser.ParseFrom(pi.ExtraInfo);
		if (ruinDestroyBuildingPointInfo != null)
		{
			ownerUid = ruinDestroyBuildingPointInfo.Uid;
			serverId = ruinDestroyBuildingPointInfo.PlayerServerId;
			allianceId = ruinDestroyBuildingPointInfo.AllianceId;
			alAbbr = ruinDestroyBuildingPointInfo.AlAbbr;
			userName = ruinDestroyBuildingPointInfo.UserName;
			destroyEndTime = ruinDestroyBuildingPointInfo.ExpireTime;
			PlayerType playerType = GetPlayerType();
			if (playerType == PlayerType.PlayerSelf || playerType == PlayerType.PlayerAllianceLeader || playerType == PlayerType.PlayerAlliance)
			{
				tileSize = 0;
			}
			else
			{
				tileSize = 3;
			}
		}
	}

	public override PointInfo Clone()
	{
		WorldRuinDestroyBuildingPointInfo worldRuinDestroyBuildingPointInfo = new WorldRuinDestroyBuildingPointInfo();
		BaseClone(worldRuinDestroyBuildingPointInfo);
		worldRuinDestroyBuildingPointInfo.ownerUid = ownerUid;
		worldRuinDestroyBuildingPointInfo.serverId = serverId;
		worldRuinDestroyBuildingPointInfo.allianceId = allianceId;
		worldRuinDestroyBuildingPointInfo.alAbbr = alAbbr;
		worldRuinDestroyBuildingPointInfo.userName = userName;
		worldRuinDestroyBuildingPointInfo.destroyEndTime = destroyEndTime;
		return worldRuinDestroyBuildingPointInfo;
	}

	public sealed override PlayerType GetPlayerType()
	{
		if (GameEntry.Data.Player.GetUid() == ownerUid)
		{
			return PlayerType.PlayerSelf;
		}
		string text = GameEntry.Data.Player.GetAllianceId();
		if (string.IsNullOrEmpty(text))
		{
			return PlayerType.PlayerOther;
		}
		if (text == allianceId)
		{
			string allianceLeaderID = GameEntry.Data.Player.GetAllianceLeaderID();
			if (ownerUid != allianceLeaderID)
			{
				return PlayerType.PlayerAlliance;
			}
			return PlayerType.PlayerAllianceLeader;
		}
		return PlayerType.PlayerOther;
	}
}
