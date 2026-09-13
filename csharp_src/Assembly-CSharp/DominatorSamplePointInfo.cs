using Protobuf;

public class DominatorSamplePointInfo : SamplePointInfo
{
	public DominatorSamplePointInfo(WorldPointInfo pi)
		: base(pi)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("detect_event", eventId.ToInt(), "para3");
		int result = -1;
		if (int.TryParse(templateData, out result))
		{
			tileSize = result;
		}
	}
}
