using Protobuf;

public class WorldOutpostPoint : PointInfo
{
	public readonly int CityId;

	public int buildState;

	public long protectTime;

	public long battleStartTime;

	public int ownerServerId;

	public int tmpOwnerServerId;

	public bool connectSwitch;

	public string ownerAllianceId;

	public string alAbbr;

	public string allianceId;

	public bool isCreate;

	public int assistanceCount;

	public int maxAssistanceCount;

	public OutpostInfo cityInfo;

	public void UpdateAssistanceCount()
	{
		base.PointManager?.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out assistanceCount, out maxAssistanceCount);
	}

	public WorldOutpostPoint(int theCityId)
	{
		CityId = theCityId;
	}

	public override PointInfo Clone()
	{
		WorldOutpostPoint worldOutpostPoint = new WorldOutpostPoint(CityId);
		BaseClone(worldOutpostPoint);
		worldOutpostPoint.buildState = buildState;
		worldOutpostPoint.protectTime = protectTime;
		worldOutpostPoint.battleStartTime = battleStartTime;
		worldOutpostPoint.ownerServerId = ownerServerId;
		worldOutpostPoint.ownerAllianceId = ownerAllianceId;
		worldOutpostPoint.tmpOwnerServerId = tmpOwnerServerId;
		worldOutpostPoint.connectSwitch = connectSwitch;
		worldOutpostPoint.alAbbr = alAbbr;
		worldOutpostPoint.allianceId = allianceId;
		worldOutpostPoint.isCreate = isCreate;
		worldOutpostPoint.assistanceCount = assistanceCount;
		worldOutpostPoint.maxAssistanceCount = maxAssistanceCount;
		worldOutpostPoint.cityInfo = cityInfo;
		return worldOutpostPoint;
	}

	public WorldOutpostPoint(WorldPointInfo pi, bool isCreate)
		: base(pi)
	{
		tileSize = 7;
		if (isCreate)
		{
			this.isCreate = true;
		}
		cityInfo = OutpostInfo.Parser.ParseFrom(extraInfo);
		CityId = cityInfo.CityId;
		buildState = cityInfo.State;
		protectTime = cityInfo.ProtectTime;
		battleStartTime = cityInfo.BattleStartTime;
		ownerServerId = cityInfo.OwnerServerId;
		ownerAllianceId = cityInfo.OwnerAllianceId;
		tmpOwnerServerId = cityInfo.TmpOwnerServerId;
		UpdateAssistanceCount();
	}

	public override PlayerType GetPlayerType()
	{
		int sourceServerId = GameEntry.Data.Player.GetSourceServerId();
		OutpostInfo outpostInfo = cityInfo;
		if (outpostInfo != null && outpostInfo.OwnerServerId == sourceServerId)
		{
			return PlayerType.PlayerSelf;
		}
		return PlayerType.PlayerSeasonEnemy;
	}
}
