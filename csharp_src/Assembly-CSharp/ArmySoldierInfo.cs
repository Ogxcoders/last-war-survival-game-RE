using System;
using Protobuf;

public class ArmySoldierInfo : IDisposable
{
	public string uid;

	public string armsId;

	public int type;

	public int total;

	public int lost;

	public int wounded;

	public int injured;

	public int dead;

	public void UpdateSoldier(SoldierProto proto)
	{
		armsId = proto.ArmsId;
		type = proto.Type;
		total = proto.Total;
		lost = proto.Lost;
		wounded = proto.Wounded;
		injured = proto.Injured;
		dead = proto.Dead;
	}

	public void Dispose()
	{
	}

	public bool IsMummy()
	{
		if (type >= 12001)
		{
			return type <= 12011;
		}
		return false;
	}
}
