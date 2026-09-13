using UnityEngine;

public static class TileCoord
{
	public const int WORLD_TILE_COUNT_MAX = 1000;

	public const float TileSize = 2f;

	public const float TileDiagonalSize = 2.8284f;

	public static readonly Vector2Int WorldTileCount = new Vector2Int(1000, 1000);

	public static readonly Vector2Int CityTileCount = new Vector2Int(100, 100);

	public static Vector3 TileToWorld(Vector2Int tilePos, int serverId)
	{
		if (serverId > 0 && SceneManager.World != null && SceneManager.World.WorldSize > 1000)
		{
			return new Vector3(((float)tilePos.x + 0.5f) * 2f, 0f, ((float)tilePos.y + 0.5f) * 2f) + SeasonDataManager.Instance.GetWorldBasePos(serverId);
		}
		return new Vector3(((float)tilePos.x + 0.5f) * 2f, 0f, ((float)tilePos.y + 0.5f) * 2f);
	}

	public static Vector3 TileToWorld(int tilePosX, int tilePosY, int serverId)
	{
		if (serverId > 0 && SceneManager.World != null && SceneManager.World.WorldSize > 1000)
		{
			return new Vector3(((float)tilePosX + 0.5f) * 2f, 0f, ((float)tilePosY + 0.5f) * 2f) + SeasonDataManager.Instance.GetWorldBasePos(serverId);
		}
		return new Vector3(((float)tilePosX + 0.5f) * 2f, 0f, ((float)tilePosY + 0.5f) * 2f);
	}

	public static Vector3 WorldToClosestGridWorld(Vector3 worldPos)
	{
		return new Vector3(((float)(int)(worldPos.x / 2f) + 0.5f) * 2f, 0f, ((float)(int)(worldPos.z / 2f) + 0.5f) * 2f);
	}

	public static Vector2Int WorldToTile(Vector3 worldPos)
	{
		if (SceneManager.World != null && SceneManager.World.WorldSize > 1000)
		{
			return new Vector2Int((int)(worldPos.x / 2f) % 1000, (int)(worldPos.z / 2f) % 1000);
		}
		return new Vector2Int((int)(worldPos.x / 2f), (int)(worldPos.z / 2f));
	}

	public static Vector2Int WorldToTile(Vector3 worldPos, int mapSize)
	{
		return new Vector2Int((int)(worldPos.x / 2f) % mapSize, (int)(worldPos.z / 2f) % mapSize);
	}

	public static Vector3 SnapToTileCenter(Vector3 worldPos)
	{
		Vector2Int vector2Int = new Vector2Int((int)(worldPos.x / 2f), (int)(worldPos.z / 2f));
		return new Vector3(((float)vector2Int.x + 0.5f) * 2f, 0f, ((float)vector2Int.y + 0.5f) * 2f);
	}

	public static Vector3 TileFloatToWorld(Vector2 tilePos, int serverId)
	{
		if (serverId > 0 && SceneManager.World != null && SceneManager.World.WorldSize > 1000)
		{
			return new Vector3(tilePos.x * 2f, 0f, tilePos.y * 2f) + SeasonDataManager.Instance.GetWorldBasePos(serverId);
		}
		return new Vector3(tilePos.x * 2f, 0f, tilePos.y * 2f);
	}

	public static Vector3 TileFloatToWorld(float x, float y, int serverId)
	{
		if (serverId > 0 && SceneManager.World != null && SceneManager.World.WorldSize > 1000)
		{
			return new Vector3(x * 2f, 0f, y * 2f) + SeasonDataManager.Instance.GetWorldBasePos(serverId);
		}
		return new Vector3(x * 2f, 0f, y * 2f);
	}

	public static Vector2 WorldToTileFloat(Vector3 worldPos)
	{
		if (SceneManager.World != null && SceneManager.World.WorldSize > 1000)
		{
			return new Vector2(worldPos.x / 2f % 1000f, worldPos.z / 2f % 1000f);
		}
		return new Vector2(worldPos.x / 2f, worldPos.z / 2f);
	}

	public static Vector2Int IndexToTilePos(int index, Vector2Int tileCount)
	{
		if (index < 1 || index > tileCount.x * tileCount.y)
		{
			return Vector2Int.zero;
		}
		index--;
		return new Vector2Int(index % tileCount.x, index / tileCount.y);
	}

	public static Vector2Int IndexToTilePos(int index, ForceChangeScene sceneType)
	{
		return IndexToTilePos(index, (sceneType == ForceChangeScene.City) ? CityTileCount : WorldTileCount);
	}

	public static int TilePosToIndex(Vector2Int tilePos, Vector2Int tileCount)
	{
		if (tilePos.x < 0 || tilePos.y < 0 || tilePos.x > tileCount.x - 1 || tilePos.y > tileCount.y - 1)
		{
			return 0;
		}
		return tilePos.x + tilePos.y * tileCount.x + 1;
	}

	public static int TilePosToIndex(Vector2Int tilePos, ForceChangeScene sceneType)
	{
		return TilePosToIndex(tilePos, (sceneType == ForceChangeScene.City) ? CityTileCount : WorldTileCount);
	}

	public static int TileXYToIndex(int x, int y, ForceChangeScene sceneType)
	{
		return x + y * ((sceneType == ForceChangeScene.City) ? 100 : 1000) + 1;
	}

	public static Vector3 TileIndexToWorld(int index, Vector2Int tileCount, int serverId)
	{
		return TileToWorld(IndexToTilePos(index, tileCount), serverId);
	}

	public static Vector3 TileIndexToWorld(int index, ForceChangeScene type, int serverId)
	{
		return TileIndexToWorld(index, (type == ForceChangeScene.City) ? CityTileCount : WorldTileCount, serverId);
	}

	public static int WorldToTileIndex(Vector3 pos, Vector2Int tileCount)
	{
		return TilePosToIndex(WorldToTile(pos), tileCount);
	}

	public static int WorldToTileIndex(Vector3 pos, ForceChangeScene type)
	{
		return WorldToTileIndex(pos, (type == ForceChangeScene.City) ? CityTileCount : WorldTileCount);
	}

	public static float TileDistance(Vector2Int a, Vector2Int b)
	{
		return Vector2Int.Distance(a, b);
	}

	public static Vector2Int ClampTilePos(ref Vector2Int tilePos, int tileCount = 1000)
	{
		if (tilePos.x < 0)
		{
			tilePos.x = 0;
		}
		if (tilePos.x > tileCount - 1)
		{
			tilePos.x = tileCount - 1;
		}
		if (tilePos.y < 0)
		{
			tilePos.y = 0;
		}
		if (tilePos.y > tileCount - 1)
		{
			tilePos.y = tileCount - 1;
		}
		return tilePos;
	}

	public static Vector2 ClampTilePos(ref Vector2 tilePos, int tileCount = 1000)
	{
		if (tilePos.x < 0f)
		{
			tilePos.x = 0f;
		}
		if (tilePos.x > (float)(tileCount - 1))
		{
			tilePos.x = tileCount - 1;
		}
		if (tilePos.y < 0f)
		{
			tilePos.y = 0f;
		}
		if (tilePos.y > (float)(tileCount - 1))
		{
			tilePos.y = tileCount - 1;
		}
		return tilePos;
	}
}
