using System.Text;
using Protobuf;

public class AllyCityPointInfo : PointInfo
{
	public readonly int CityId;

	public int CityLevel = -1;

	public int CityField = -1;

	public int assistanceCount;

	public int maxAssistanceCount;

	public CityStrongholdPointInfo StrongholdInfo;

	public AllianceCityPointInfo CityInfo;

	public CityTradePointInfo WorldTradeInfo;

	public GoldTreePointInfo GoldTreePointInfo;

	public void UpdateAssistanceCount()
	{
		base.PointManager?.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out assistanceCount, out maxAssistanceCount);
	}

	public AllyCityPointInfo(int theCityId)
	{
		CityId = theCityId;
	}

	public override PointInfo Clone()
	{
		AllyCityPointInfo allyCityPointInfo = new AllyCityPointInfo(CityId);
		BaseClone(allyCityPointInfo);
		allyCityPointInfo.CityLevel = CityLevel;
		allyCityPointInfo.CityField = CityField;
		allyCityPointInfo.assistanceCount = assistanceCount;
		allyCityPointInfo.maxAssistanceCount = maxAssistanceCount;
		allyCityPointInfo.StrongholdInfo = StrongholdInfo;
		allyCityPointInfo.CityInfo = CityInfo;
		allyCityPointInfo.WorldTradeInfo = WorldTradeInfo;
		allyCityPointInfo.GoldTreePointInfo = GoldTreePointInfo;
		return allyCityPointInfo;
	}

	public AllyCityPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		WorldTradeInfo = null;
		CityInfo = null;
		StrongholdInfo = null;
		if (pointType == WorldPointType.WORLD_CITY_STRONGHOLD)
		{
			StrongholdInfo = CityStrongholdPointInfo.Parser.ParseFrom(pi.ExtraInfo);
			if (StrongholdInfo != null)
			{
				CityId = StrongholdInfo.StrongholdId;
				if (StrongholdInfo.ThermalConductor != null)
				{
					thermalConductor = new ThermalConductor(StrongholdInfo.ThermalConductor);
				}
			}
		}
		else if (pointType == WorldPointType.WORLD_CITY_TRADE)
		{
			WorldTradeInfo = CityTradePointInfo.Parser.ParseFrom(pi.ExtraInfo);
			if (WorldTradeInfo != null)
			{
				CityId = WorldTradeInfo.TradeId;
			}
		}
		else if (pointType == WorldPointType.GOLD_TREE)
		{
			GoldTreePointInfo = GoldTreePointInfo.Parser.ParseFrom(pi.ExtraInfo);
			if (GoldTreePointInfo != null)
			{
				CityId = GoldTreePointInfo.TreeId;
			}
		}
		else
		{
			CityInfo = AllianceCityPointInfo.Parser.ParseFrom(pi.ExtraInfo);
			if (CityInfo != null)
			{
				CityId = CityInfo.CityId;
				if (CityInfo.ThermalConductor != null)
				{
					thermalConductor = new ThermalConductor(CityInfo.ThermalConductor);
				}
			}
		}
		tileSize = SceneManager.World.GetAllianceCitySizeByItemId(CityId);
		PointInfo pointInfoByUuid = SceneManager.World.GetPointInfoByUuid(pi.Uuid);
		if (pointInfoByUuid != null && pointInfoByUuid is AllyCityPointInfo allyCityPointInfo)
		{
			CityLevel = allyCityPointInfo.CityLevel;
			CityField = allyCityPointInfo.CityField;
		}
		UpdateAssistanceCount();
	}

	public string GetAllianceId()
	{
		if (CityInfo != null)
		{
			return CityInfo.AllianceId;
		}
		if (StrongholdInfo != null)
		{
			return StrongholdInfo.AllianceId;
		}
		return null;
	}

	public bool HasOwner()
	{
		return !string.IsNullOrEmpty(GetAllianceId());
	}

	public override void OnDescription(StringBuilder sb)
	{
		sb.AppendLine($"assistanceCount = {assistanceCount}");
		sb.AppendLine($"maxAssistanceCount = {maxAssistanceCount}");
	}
}
