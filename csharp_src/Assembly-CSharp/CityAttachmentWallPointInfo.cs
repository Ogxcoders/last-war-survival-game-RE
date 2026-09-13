using Protobuf;

public class CityAttachmentWallPointInfo : CityAttachmentPointInfo
{
	public CityAttachmentWallPointInfo()
	{
		tileSize = 3;
	}

	public CityAttachmentWallPointInfo(WorldPointInfo pi)
		: base(pi)
	{
	}

	public override PointInfo Clone()
	{
		CityAttachmentWallPointInfo cityAttachmentWallPointInfo = new CityAttachmentWallPointInfo();
		BaseClone(cityAttachmentWallPointInfo);
		cityAttachmentWallPointInfo.buildId = buildId;
		cityAttachmentWallPointInfo.tileSize = tileSize;
		cityAttachmentWallPointInfo.tileSizeX = tileSizeX;
		cityAttachmentWallPointInfo.tileSizeY = tileSizeY;
		return cityAttachmentWallPointInfo;
	}
}
