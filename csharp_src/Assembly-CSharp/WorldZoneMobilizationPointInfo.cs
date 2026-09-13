using Protobuf;

public class WorldZoneMobilizationPointInfo : PointInfo
{
	public ZoneMobilizationPointInfo zoneMobilizationPointInfo;

	public WorldZoneMobilizationPointInfo()
	{
	}

	public WorldZoneMobilizationPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		zoneMobilizationPointInfo = new ZoneMobilizationPointInfo(pi.ZoneMobilizationPointInfo);
		int.TryParse(GameEntry.ConfigCache.GetTemplateData("zone_mobilization_boss", zoneMobilizationPointInfo.bossId, "building_size"), out tileSize);
	}

	public override PointInfo Clone()
	{
		WorldZoneMobilizationPointInfo worldZoneMobilizationPointInfo = new WorldZoneMobilizationPointInfo();
		BaseClone(worldZoneMobilizationPointInfo);
		worldZoneMobilizationPointInfo.zoneMobilizationPointInfo = zoneMobilizationPointInfo;
		return worldZoneMobilizationPointInfo;
	}

	public string GetModelPath()
	{
		return GameEntry.ConfigCache.GetTemplateData("zone_mobilization_boss", zoneMobilizationPointInfo.bossId, "building_model");
	}

	public string GetName()
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("zone_mobilization_boss", zoneMobilizationPointInfo.bossId, "building_name");
		return GameEntry.Localization.GetString(templateData);
	}

	public string GetIconPath()
	{
		return GameEntry.ConfigCache.GetTemplateData("zone_mobilization_boss", zoneMobilizationPointInfo.bossId, "lod_icon");
	}
}
