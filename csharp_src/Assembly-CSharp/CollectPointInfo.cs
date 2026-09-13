using Protobuf;

public class CollectPointInfo : PointInfo
{
	public ResourceType resourceType;

	public int level;

	public int type;

	public int attachId;

	public CollectPointInfo()
	{
	}

	public CollectPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		CollectResourceInfo collectResourceInfo = pi.CollectResourceInfo;
		resourceType = (ResourceType)collectResourceInfo.ResourceType;
		type = collectResourceInfo.Type;
		attachId = collectResourceInfo.AttachId;
		level = collectResourceInfo.Level;
		tileSize = SceneManager.World.GetCollectResourceTile();
	}

	public override PointInfo Clone()
	{
		CollectPointInfo collectPointInfo = new CollectPointInfo();
		BaseClone(collectPointInfo);
		collectPointInfo.resourceType = resourceType;
		collectPointInfo.level = level;
		collectPointInfo.type = type;
		collectPointInfo.attachId = attachId;
		return collectPointInfo;
	}

	public int GetResourceType()
	{
		return (int)resourceType;
	}
}
