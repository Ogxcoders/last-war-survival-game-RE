using Protobuf;

public class DetectAttackCityS0TaskPointInfo : PointInfo
{
	public long Id;

	public new long uuid;

	public string eventId;

	public DetectAttackCityS0TaskPointInfo()
	{
	}

	public DetectAttackCityS0TaskPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		Id = pi.Id;
		uuid = pi.Uuid;
		eventId = GameEntry.Lua.CallWithReturn<string, long>("CSharpCallLuaInterface.GetAttackCityS0DetectInfo", pi.Id);
	}

	public override PointInfo Clone()
	{
		DetectAttackCityS0TaskPointInfo detectAttackCityS0TaskPointInfo = new DetectAttackCityS0TaskPointInfo();
		BaseClone(detectAttackCityS0TaskPointInfo);
		detectAttackCityS0TaskPointInfo.Id = Id;
		detectAttackCityS0TaskPointInfo.uuid = uuid;
		detectAttackCityS0TaskPointInfo.eventId = eventId;
		return detectAttackCityS0TaskPointInfo;
	}
}
