public class WorldTileInfo
{
	public int pointIndex;

	public int serverId;

	private PointInfo pointInfo;

	private int pointType;

	private WorldDesertInfo desertInfo;

	public void AddPointInfo(PointInfo p)
	{
		if (p != null)
		{
			pointInfo = p;
			pointIndex = pointInfo.pointIndex;
			serverId = pointInfo.serverId;
			pointType = pointInfo.PointType;
		}
	}

	public void RemovePointInfo()
	{
		pointInfo = null;
		pointType = 0;
	}

	public WorldPointType GetWorldPointType()
	{
		if (pointInfo != null)
		{
			return pointInfo.pointType;
		}
		return WorldPointType.Other;
	}

	public PointInfo GetPointInfo()
	{
		return pointInfo;
	}

	public int GetPointType()
	{
		return pointType;
	}

	public int GetPointSize()
	{
		if (pointInfo != null)
		{
			return pointInfo.tileSize;
		}
		return 0;
	}

	public WorldDesertInfo GetWorldDesertInfo()
	{
		return desertInfo;
	}

	public void AddDesertInfo(WorldDesertInfo d)
	{
		if (d != null)
		{
			desertInfo = d;
			pointIndex = desertInfo.pointIndex;
			serverId = desertInfo.serverId;
		}
	}

	public void RemoveDesertInfo()
	{
		desertInfo = null;
	}

	public bool GetIsDataEmpty()
	{
		if (pointInfo == null)
		{
			return desertInfo == null;
		}
		return false;
	}
}
