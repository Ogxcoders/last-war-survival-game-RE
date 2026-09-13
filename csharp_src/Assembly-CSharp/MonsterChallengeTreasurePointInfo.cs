using Protobuf;
using Sfs2X.Entities.Data;

public class MonsterChallengeTreasurePointInfo
{
	public long startTime;

	public string allianceId;

	public string allianceName;

	public string allianceAbbr;

	public long allianceDamage;

	public int configId;

	public long endTime;

	public MonsterChallengeTreasurePointInfo()
	{
	}

	public MonsterChallengeTreasurePointInfo(ISFSObject msg)
	{
		ParseData(msg);
	}

	public MonsterChallengeTreasurePointInfo(Protobuf.MonsterChallengeTreasurePointInfo msg)
	{
		ParseData(msg);
	}

	public virtual void ParseData(ISFSObject msg)
	{
		startTime = msg.TryGetLong("startTime");
		allianceId = msg.TryGetString("allianceId");
		allianceName = msg.TryGetString("allianceName");
		allianceAbbr = msg.TryGetString("allianceAbbr");
		allianceDamage = msg.TryGetLong("allianceDamage");
		configId = msg.TryGetInt("configId");
		endTime = msg.TryGetLong("endTime");
	}

	protected virtual void ParseData(Protobuf.MonsterChallengeTreasurePointInfo msg)
	{
		allianceId = msg.AllianceId;
		startTime = msg.StartTime;
		allianceName = msg.AllianceName;
		allianceAbbr = msg.AllianceAbbr;
		allianceDamage = msg.AllianceDamage;
		configId = msg.ConfigId;
		endTime = msg.EndTime;
	}
}
