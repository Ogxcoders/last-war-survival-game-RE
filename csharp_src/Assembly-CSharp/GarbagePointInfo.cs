using Protobuf;

public class GarbagePointInfo : PointInfo
{
	public new string ownerUid;

	public new long uuid;

	public string eventId;

	public long endTime;

	public GarbagePointInfo()
	{
	}

	public GarbagePointInfo(WorldPointInfo pi)
		: base(pi)
	{
		Protobuf.GarbagePointInfo garbagePointInfo = pi.GarbagePointInfo;
		ownerUid = garbagePointInfo.OwnerUid;
		uuid = garbagePointInfo.Uuid;
		eventId = garbagePointInfo.EventId;
		endTime = garbagePointInfo.EndTime;
	}

	public override PointInfo Clone()
	{
		GarbagePointInfo garbagePointInfo = new GarbagePointInfo();
		BaseClone(garbagePointInfo);
		garbagePointInfo.ownerUid = ownerUid;
		garbagePointInfo.uuid = uuid;
		garbagePointInfo.eventId = eventId;
		garbagePointInfo.endTime = endTime;
		return garbagePointInfo;
	}
}
