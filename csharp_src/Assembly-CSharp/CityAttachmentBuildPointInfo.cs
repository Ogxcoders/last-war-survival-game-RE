using Protobuf;

public class CityAttachmentBuildPointInfo : CityAttachmentPointInfo
{
	public CityAttachment mBuildData;

	public CityAttachmentBuildPointInfo()
	{
		tileSize = 3;
	}

	public CityAttachmentBuildPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		mBuildData = pi.CityAttachment;
	}

	public override PointInfo Clone()
	{
		CityAttachmentBuildPointInfo cityAttachmentBuildPointInfo = new CityAttachmentBuildPointInfo();
		BaseClone(cityAttachmentBuildPointInfo);
		cityAttachmentBuildPointInfo.buildId = buildId;
		cityAttachmentBuildPointInfo.tileSize = tileSize;
		cityAttachmentBuildPointInfo.tileSizeX = tileSizeX;
		cityAttachmentBuildPointInfo.tileSizeY = tileSizeY;
		cityAttachmentBuildPointInfo.mBuildData = mBuildData;
		return cityAttachmentBuildPointInfo;
	}
}
