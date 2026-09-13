using Sfs2X.Entities.Data;

public class CityHeatSource : HeatSourceBase
{
	public int otherCfgId;

	public int pointId;

	public new void ParseData(ISFSObject msg)
	{
		base.ParseData(msg);
		pointId = msg.TryGetInt("pointId");
		otherCfgId = msg.TryGetInt("otherCfgId");
	}

	public int GetCityId()
	{
		return otherCfgId;
	}
}
