using UnityEngine;

public class WorldTileData
{
	public float Ecost;

	public float Gcost;

	public int PathID;

	public Vector2Int Pos;

	public WorldTileData Parent;

	public bool HasClosed;

	public float Fcost => Gcost + Ecost;

	public void reset()
	{
		Ecost = 0f;
		Gcost = 0f;
		PathID = 0;
		Parent = null;
		HasClosed = false;
	}
}
