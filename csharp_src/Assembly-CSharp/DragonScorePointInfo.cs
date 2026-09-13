using Protobuf;

public class DragonScorePointInfo : PointInfo
{
	public DragonScoreInfo detail;

	public DragonScorePointInfo()
	{
	}

	public DragonScorePointInfo(WorldPointInfo pi)
		: base(pi)
	{
		detail = DragonScoreInfo.Parser.ParseFrom(extraInfo);
	}

	public override PointInfo Clone()
	{
		DragonScorePointInfo dragonScorePointInfo = new DragonScorePointInfo();
		BaseClone(dragonScorePointInfo);
		dragonScorePointInfo.detail = detail;
		return dragonScorePointInfo;
	}
}
