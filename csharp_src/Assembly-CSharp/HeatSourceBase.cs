using System;
using Sfs2X.Entities.Data;

public class HeatSourceBase : IDisposable
{
	public long uuid;

	public float temperature;

	public short cfgId;

	public byte type;

	public void ParseData(ISFSObject msg)
	{
		uuid = msg.TryGetLong("uuid");
		temperature = msg.TryGetFloat("temperature");
		cfgId = (short)msg.TryGetInt("cfgId");
		type = (byte)msg.TryGetInt("type");
	}

	public virtual void Dispose()
	{
	}

	public override string ToString()
	{
		return $"uuid={uuid};type={type};cfgId={cfgId};temperature={temperature}";
	}
}
