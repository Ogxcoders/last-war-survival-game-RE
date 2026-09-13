using UnityEngine;

[ExecuteInEditMode]
public class PositionMarker : MonoBehaviour
{
	[SerializeField]
	private Vector2Int tilePos;

	private void Awake()
	{
		tilePos = TileCoord.WorldToTile(base.transform.position);
	}

	private void Update()
	{
	}

	public void SetToTileCenter()
	{
		Vector3 position = base.transform.position;
		Vector3 vector = TileCoord.SnapToTileCenter(position);
		base.transform.position = new Vector3(vector.x, position.y, vector.z);
		tilePos = TileCoord.WorldToTile(base.transform.position);
	}

	public Vector2Int GetTilePos()
	{
		return TileCoord.WorldToTile(base.transform.position);
	}
}
