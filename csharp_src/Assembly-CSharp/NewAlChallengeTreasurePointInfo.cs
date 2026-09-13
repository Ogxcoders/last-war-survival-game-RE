using Protobuf;

public class NewAlChallengeTreasurePointInfo : PointInfo
{
	public MonsterChallengeTreasurePointInfo treasurePointInfo;

	public NewAlChallengeTreasurePointInfo()
	{
	}

	public NewAlChallengeTreasurePointInfo(WorldPointInfo pi)
		: base(pi)
	{
		treasurePointInfo = new MonsterChallengeTreasurePointInfo(pi.MonsterChallengeTreasurePointInfo);
		tileSize = 3;
	}

	public override PointInfo Clone()
	{
		NewAlChallengeTreasurePointInfo newAlChallengeTreasurePointInfo = new NewAlChallengeTreasurePointInfo();
		BaseClone(newAlChallengeTreasurePointInfo);
		newAlChallengeTreasurePointInfo.treasurePointInfo = treasurePointInfo;
		return newAlChallengeTreasurePointInfo;
	}
}
