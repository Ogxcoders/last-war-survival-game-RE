public class ConstHeatSource : HeatSourceBase
{
	public void CreateByClient(int cfgId, int type, float temperature)
	{
		base.cfgId = (short)cfgId;
		base.type = (byte)type;
		base.temperature = temperature;
	}
}
