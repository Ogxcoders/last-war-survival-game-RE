using System;
using Sfs2X.Entities.Data;
using UnityEngine;

public class WorldTriggerData : IDisposable
{
	public long uuid;

	public int cfgId;

	public string ownerId;

	public string ownerName;

	public string abbr;

	public long startTime;

	public long endTime;

	public int pointId;

	public int serverId;

	public string allianceId;

	public WorldTriggerConfig config;

	public int minX;

	public int minY;

	public int maxX;

	public int maxY;

	public void ParseData(ISFSObject msg, int _serverId, int _worldId)
	{
		uuid = msg.TryGetLong("uuid");
		cfgId = msg.TryGetInt("cfgId");
		ownerId = msg.TryGetString("ownerId");
		startTime = msg.TryGetLong("startTime");
		endTime = msg.TryGetLong("endTime");
		pointId = msg.TryGetInt("pointId");
		serverId = msg.TryGetInt("serverId");
		allianceId = msg.TryGetString("allianceId");
		ISFSObject obj = msg.TryGetObj("avatar");
		abbr = obj.TryGetString("abbr");
		ownerName = obj.TryGetString("name");
		if (serverId != 0)
		{
			return;
		}
		if (_serverId > 0)
		{
			serverId = _serverId;
			return;
		}
		DCPlayer dCPlayer = GameEntry.Data?.Player;
		if (dCPlayer != null)
		{
			serverId = dCPlayer.GetSourceServerId();
		}
	}

	public void SetConfig(WorldTriggerConfig config)
	{
		this.config = config;
		Vector2Int vector2Int = TileCoord.IndexToTilePos(pointId, ForceChangeScene.World);
		int num = (config.size - 1) / 2;
		minX = vector2Int.x - num;
		minY = vector2Int.y - num;
		maxX = vector2Int.x + num;
		maxY = vector2Int.y + num;
	}

	public void Dispose()
	{
		ownerId = null;
		allianceId = null;
	}

	public bool IsInRange(int x, int y, int _serverId)
	{
		if ((_serverId == serverId || serverId == 0) && minX <= x && x <= maxX && minY <= y)
		{
			return y <= maxY;
		}
		return false;
	}

	public bool IsVisible()
	{
		return true;
	}
}
