using Sfs2X.Entities.Data;
using UnityEngine;

public class DynamicHeatSource : HeatSourceBase
{
	public byte state;

	public string abbr;

	public byte otherCfgId;

	public short minX;

	public short minY;

	public short maxX;

	public short maxY;

	public short x;

	public short y;

	public new void ParseData(ISFSObject msg)
	{
		base.ParseData(msg);
		state = (byte)msg.TryGetInt("state");
		abbr = msg.TryGetString("abbr");
		int num = msg.TryGetInt("radius");
		int index = msg.TryGetInt("pointId");
		otherCfgId = (byte)msg.TryGetInt("otherCfgId");
		Vector2Int vector2Int = TileCoord.IndexToTilePos(index, ForceChangeScene.World);
		x = (short)vector2Int.x;
		y = (short)vector2Int.y;
		minX = (short)(vector2Int.x - num);
		minY = (short)(vector2Int.y - num);
		maxX = (short)(vector2Int.x + num);
		maxY = (short)(vector2Int.y + num);
	}

	public bool IsInRange(int x, int y)
	{
		if (minX <= x && x <= maxX && minY <= y)
		{
			return y <= maxY;
		}
		return false;
	}

	public override void Dispose()
	{
		abbr = null;
	}
}
