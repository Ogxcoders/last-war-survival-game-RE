using Protobuf;

public class WorldOutpostTowerPoint : PointInfo
{
	public readonly int CityId;

	public int buildState;

	public long protectTime;

	public long battleStartTime;

	public long lastTowerAttackTime;

	public int tmpOwnerServerId;

	public string alAbbr;

	public string allianceId;

	public bool isCreate;

	public int assistanceCount;

	public int maxAssistanceCount;

	public OutpostTowerInfo towerInfo;

	public void UpdateAssistanceCount()
	{
		base.PointManager?.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out assistanceCount, out maxAssistanceCount);
	}

	public WorldOutpostTowerPoint(int theCityId)
	{
		CityId = theCityId;
	}

	public override PointInfo Clone()
	{
		WorldOutpostTowerPoint worldOutpostTowerPoint = new WorldOutpostTowerPoint(CityId);
		BaseClone(worldOutpostTowerPoint);
		worldOutpostTowerPoint.buildState = buildState;
		worldOutpostTowerPoint.protectTime = protectTime;
		worldOutpostTowerPoint.battleStartTime = battleStartTime;
		worldOutpostTowerPoint.lastTowerAttackTime = lastTowerAttackTime;
		worldOutpostTowerPoint.tmpOwnerServerId = tmpOwnerServerId;
		worldOutpostTowerPoint.alAbbr = alAbbr;
		worldOutpostTowerPoint.allianceId = allianceId;
		worldOutpostTowerPoint.isCreate = isCreate;
		worldOutpostTowerPoint.assistanceCount = assistanceCount;
		worldOutpostTowerPoint.maxAssistanceCount = maxAssistanceCount;
		worldOutpostTowerPoint.towerInfo = towerInfo;
		return worldOutpostTowerPoint;
	}

	public WorldOutpostTowerPoint(WorldPointInfo pi, bool isCreate)
		: base(pi)
	{
		tileSize = 3;
		if (isCreate)
		{
			this.isCreate = true;
		}
		towerInfo = OutpostTowerInfo.Parser.ParseFrom(extraInfo);
		CityId = towerInfo.CityId;
		buildState = towerInfo.State;
		protectTime = towerInfo.ProtectTime;
		battleStartTime = towerInfo.BattleStartTime;
		lastTowerAttackTime = towerInfo.LastTowerAttackTime;
		tmpOwnerServerId = towerInfo.TmpOwnerServerId;
		UpdateAssistanceCount();
	}

	public override PlayerType GetPlayerType()
	{
		int sourceServerId = GameEntry.Data.Player.GetSourceServerId();
		OutpostTowerInfo outpostTowerInfo = towerInfo;
		if (outpostTowerInfo != null && outpostTowerInfo.TmpOwnerServerId == sourceServerId)
		{
			return PlayerType.PlayerSelf;
		}
		return PlayerType.PlayerSeasonEnemy;
	}
}
