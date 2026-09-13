using Protobuf;

public class ExplorePointInfo : PointInfo
{
	public new string ownerUid;

	public new long uuid;

	public string eventId;

	public ExplorePointInfo()
	{
	}

	public ExplorePointInfo(WorldPointInfo pi)
		: base(pi)
	{
		Protobuf.ExplorePointInfo explorePointInfo = pi.ExplorePointInfo;
		ownerUid = explorePointInfo.OwnerUid;
		uuid = explorePointInfo.Uuid;
		eventId = explorePointInfo.EventId;
		if (explorePointInfo.ThermalConductor != null)
		{
			thermalConductor = new ThermalConductor(explorePointInfo.ThermalConductor);
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
