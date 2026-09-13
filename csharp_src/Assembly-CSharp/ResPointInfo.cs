using System.Text;
using Protobuf;

public class ResPointInfo : PointInfo
{
	public enum Type
	{
		Normal,
		Radar
	}

	public int id;

	public int state;

	public long gatherMarchUuid;

	public string gatherUid;

	public string gatherAllianceId;

	public Protobuf.SpecialType specialType;

	public ResPointInfo()
	{
	}

	public ResPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		ResourceInfo resourceInfo = pi.ResourceInfo;
		id = resourceInfo.ResourceId;
		state = resourceInfo.State;
		gatherMarchUuid = resourceInfo.GatherUuid;
		gatherUid = resourceInfo.GatherUid;
		gatherAllianceId = resourceInfo.GatherAllianceId;
		specialType = resourceInfo.SpecialType;
		ownerUid = "";
		if (resourceInfo.SamplePointInfo != null)
		{
			ownerUid = resourceInfo.SamplePointInfo.OwnerUid;
		}
	}

	public override PointInfo Clone()
	{
		ResPointInfo resPointInfo = new ResPointInfo();
		BaseClone(resPointInfo);
		resPointInfo.id = id;
		resPointInfo.state = state;
		resPointInfo.gatherMarchUuid = gatherMarchUuid;
		return resPointInfo;
	}

	public int GetResPointType()
	{
		int result = -1;
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_gather_resource", id, "special");
		if (!templateData.IsNullOrEmpty())
		{
			result = templateData.ToInt();
		}
		return result;
	}

	public int GetResType()
	{
		int result = -1;
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_gather_resource", id, "resource_type");
		if (!templateData.IsNullOrEmpty())
		{
			result = templateData.ToInt();
		}
		return result;
	}

	public int GetResLevel()
	{
		if (int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_gather_resource", id, "level"), out var result))
		{
			return result;
		}
		return 0;
	}

	public override bool CheckClickIsValid(int lod)
	{
		if (lod == 3)
		{
			return true;
		}
		return false;
	}

	public override void OnDescription(StringBuilder sb)
	{
		sb.AppendLine("--ResDetail--");
		sb.AppendLine($"id:{id}");
		sb.AppendLine($"state:{state}");
		sb.AppendLine($"gatherMarchUuid:{gatherMarchUuid}");
		sb.AppendLine("gatherUid:" + gatherUid);
		sb.AppendLine("gatherAllianceId:" + gatherAllianceId);
		sb.AppendLine($"GetResPointType:{GetResPointType()}");
		sb.AppendLine($"GetResType:{GetResType()}");
	}
}
