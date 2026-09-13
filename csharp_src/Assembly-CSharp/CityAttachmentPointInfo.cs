using System;
using Protobuf;

public class CityAttachmentPointInfo : PointInfo
{
	public int tileSizeX = 3;

	public int tileSizeY = 3;

	public int buildId;

	public string allianceId;

	public CityAttachmentPointInfo()
	{
		tileSize = 3;
	}

	public CityAttachmentPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		allianceId = pi.CityAttachment.AllianceId;
		buildId = pi.CityAttachment.BuildId;
		string tabName = "season_builders_alliance_list";
		string str = GameEntry.ConfigCache.TryGetTemplateData(tabName, buildId, "size_x");
		tileSizeX = (str.IsNullOrEmpty() ? 3 : str.ToInt());
		string str2 = GameEntry.ConfigCache.TryGetTemplateData(tabName, buildId, "size_y");
		tileSizeY = (str2.IsNullOrEmpty() ? 3 : str2.ToInt());
		tileSize = Math.Max(tileSizeX, tileSizeY);
	}

	public override PointInfo Clone()
	{
		CityAttachmentPointInfo cityAttachmentPointInfo = new CityAttachmentPointInfo();
		BaseClone(cityAttachmentPointInfo);
		cityAttachmentPointInfo.buildId = buildId;
		cityAttachmentPointInfo.tileSize = tileSize;
		cityAttachmentPointInfo.tileSizeX = tileSizeX;
		cityAttachmentPointInfo.tileSizeY = tileSizeY;
		return cityAttachmentPointInfo;
	}
}
