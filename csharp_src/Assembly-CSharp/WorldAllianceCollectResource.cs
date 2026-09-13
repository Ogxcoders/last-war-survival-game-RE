using Protobuf;

public class WorldAllianceCollectResource : PointInfo
{
	private WorldAllianceCollectResPointInfo _pbInfo;

	public bool isCreate;

	public int configId => _pbInfo.BuildId;

	public new long uuid => _pbInfo.Uuid;

	public string allianceId => _pbInfo.AllianceId;

	public string allianceAbbr => _pbInfo.AllianceAbbr;

	public int state => _pbInfo.State;

	public WorldAllianceCollectResource()
	{
	}

	public WorldAllianceCollectResource(WorldPointInfo pi, bool isCreate = false)
		: base(pi)
	{
		_pbInfo = pi.AllianceCollectResInfo.Clone();
		tileSize = 3;
		this.isCreate = isCreate;
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
