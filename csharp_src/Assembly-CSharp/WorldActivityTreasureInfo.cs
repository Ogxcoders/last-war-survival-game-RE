using Protobuf;

public class WorldActivityTreasureInfo : PointInfo
{
	public WorldActivityTreasureInfo actEasterEggPointInfo;

	public int cfgId;

	public new long uuid;

	public string ownerId;

	public int aid;

	public WorldActivityTreasureInfo()
	{
	}

	public WorldActivityTreasureInfo(WorldPointInfo pi)
		: base(pi)
	{
		ActivityTreasurePointInfo activityTreasurePoint = pi.ActivityTreasurePoint;
		ownerUid = activityTreasurePoint.OwnerId;
		cfgId = activityTreasurePoint.CfgId;
		uuid = activityTreasurePoint.Uuid;
		int num = 0;
		string templateData = GameEntry.ConfigCache.GetTemplateData("activity_world_treasure", cfgId, "size");
		if (!templateData.IsNullOrEmpty())
		{
			num = templateData.ToInt();
		}
		tileSize = num;
	}

	public override PointInfo Clone()
	{
		WorldActivityTreasureInfo worldActivityTreasureInfo = new WorldActivityTreasureInfo();
		BaseClone(worldActivityTreasureInfo);
		worldActivityTreasureInfo.actEasterEggPointInfo = actEasterEggPointInfo;
		return worldActivityTreasureInfo;
	}
}
