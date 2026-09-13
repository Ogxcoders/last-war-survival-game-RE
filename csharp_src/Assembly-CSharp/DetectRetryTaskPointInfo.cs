using Protobuf;

public class DetectRetryTaskPointInfo : PointInfo
{
	public new string ownerUid;

	public new long uuid;

	public string eventId;

	public int featureConfigId;

	public DetectRetryTaskPointInfo()
	{
	}

	public DetectRetryTaskPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		Protobuf.SamplePointInfo samplePointInfo = pi.SamplePointInfo;
		ownerUid = samplePointInfo.OwnerUid;
		uuid = samplePointInfo.Uuid;
		eventId = samplePointInfo.EventId;
		DetectRetryTaskInfo detectRetryTaskInfo = DetectRetryTaskInfo.Parser.ParseFrom(pi.ExtraInfo);
		if (detectRetryTaskInfo != null)
		{
			featureConfigId = detectRetryTaskInfo.FeatureConfigId;
		}
	}

	public override PointInfo Clone()
	{
		DetectRetryTaskPointInfo detectRetryTaskPointInfo = new DetectRetryTaskPointInfo();
		BaseClone(detectRetryTaskPointInfo);
		detectRetryTaskPointInfo.ownerUid = ownerUid;
		detectRetryTaskPointInfo.uuid = uuid;
		detectRetryTaskPointInfo.eventId = eventId;
		detectRetryTaskPointInfo.featureConfigId = featureConfigId;
		return detectRetryTaskPointInfo;
	}
}
