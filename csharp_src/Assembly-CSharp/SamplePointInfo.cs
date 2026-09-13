using Protobuf;

public class SamplePointInfo : PointInfo
{
	public new string ownerUid;

	public new long uuid;

	public string eventId;

	public SamplePointInfo()
	{
	}

	public SamplePointInfo(WorldPointInfo pi)
		: base(pi)
	{
		Protobuf.SamplePointInfo samplePointInfo = pi.SamplePointInfo;
		ownerUid = samplePointInfo.OwnerUid;
		uuid = samplePointInfo.Uuid;
		eventId = samplePointInfo.EventId;
		if (pointType == WorldPointType.DETECT_DIG_GAME)
		{
			tileSize = 3;
		}
	}

	public override PointInfo Clone()
	{
		ExplorePointInfo explorePointInfo = new ExplorePointInfo();
		BaseClone(explorePointInfo);
		explorePointInfo.ownerUid = ownerUid;
		explorePointInfo.uuid = uuid;
		explorePointInfo.eventId = eventId;
		return explorePointInfo;
	}
}
