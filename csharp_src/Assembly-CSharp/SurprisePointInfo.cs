using Protobuf;

public class SurprisePointInfo : PointInfo
{
	public new long uuid;

	public int configId;

	public long showTime;

	public long openTime;

	public SurprisePointInfo()
	{
	}

	public SurprisePointInfo(WorldPointInfo pi)
		: base(pi)
	{
		Protobuf.SurprisePointInfo surprisePoint = pi.SurprisePoint;
		uuid = surprisePoint.Uuid;
		configId = surprisePoint.ConfigId;
		string templateData = GameEntry.ConfigCache.GetTemplateData("map_surprise", configId, "size");
		tileSize = ((!string.IsNullOrEmpty(templateData)) ? int.Parse(templateData) : 0);
		showTime = surprisePoint.ShowTime;
		openTime = surprisePoint.OpenTime;
	}

	public override PointInfo Clone()
	{
		SurprisePointInfo surprisePointInfo = new SurprisePointInfo();
		BaseClone(surprisePointInfo);
		surprisePointInfo.uuid = uuid;
		surprisePointInfo.configId = configId;
		return surprisePointInfo;
	}
}
