using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps;

[RequiredByNativeCode]
public abstract class TileBase : ScriptableObject
{
	[RequiredByNativeCode]
	public virtual void RefreshTile(Vector3Int position, ITilemap tilemap)
	{
		tilemap.RefreshTile(position);
	}

	[RequiredByNativeCode]
	public virtual void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
	{
	}

	private TileData GetTileDataNoRef(Vector3Int position, ITilemap tilemap)
	{
		TileData tileData = default(TileData);
		GetTileData(position, tilemap, ref tileData);
		return tileData;
	}

	[RequiredByNativeCode]
	public virtual bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
	{
		return false;
	}

	private TileAnimationData GetTileAnimationDataNoRef(Vector3Int position, ITilemap tilemap)
	{
		TileAnimationData tileAnimationData = default(TileAnimationData);
		GetTileAnimationData(position, tilemap, ref tileAnimationData);
		return tileAnimationData;
	}

	[RequiredByNativeCode]
	public virtual bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
	{
		return false;
	}
}
