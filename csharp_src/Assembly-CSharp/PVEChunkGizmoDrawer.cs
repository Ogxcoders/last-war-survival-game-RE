using UnityEngine;

public class PVEChunkGizmoDrawer : MonoBehaviour
{
	private float chunkSize = 20f;

	private Color _gizmoColor = Color.cyan;

	private bool _showCoordinates = true;

	private float _labelHeight = 2f;

	private Color _cameraChunkColor = Color.red;

	private Vector2Int _currentCameraChunk = Vector2Int.zero;

	private Vector3 _cameraPosition = Vector3.zero;

	private Color _visibleChunkColor = Color.yellow;

	private int _visibleChunkRange = 1;

	private bool _enableChunkUnload;

	public void SetChunkSize(float chunkSize)
	{
		this.chunkSize = chunkSize;
	}

	public void SetCameraPosition(Vector2Int cameraTilePos, Vector2Int chunkCoord)
	{
		_cameraPosition = TileCoord.TileToWorld(cameraTilePos, 0);
		_currentCameraChunk = chunkCoord;
	}

	public void SetCameraVisibleRange(int visibleChunkRange)
	{
		_visibleChunkRange = visibleChunkRange;
	}

	public void SetChunkUnloadEnable(bool enableChunkUnload)
	{
		_enableChunkUnload = enableChunkUnload;
	}

	private void OnDrawGizmos()
	{
		Vector3 position = base.transform.position;
		int num = _currentCameraChunk.x - _visibleChunkRange;
		int num2 = _currentCameraChunk.x + _visibleChunkRange;
		int num3 = (_enableChunkUnload ? _currentCameraChunk.y : (_currentCameraChunk.y - _visibleChunkRange));
		int num4 = _currentCameraChunk.y + _visibleChunkRange;
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Vector3 vector = new Vector3(position.x + ((float)i + 0.5f) * chunkSize, position.y, position.z + ((float)j + 0.5f) * chunkSize);
				Gizmos.color = new Color(_visibleChunkColor.r, _visibleChunkColor.g, _visibleChunkColor.b, 0.2f);
				Gizmos.DrawCube(vector, new Vector3(chunkSize, 0.1f, chunkSize));
				Gizmos.color = _visibleChunkColor;
				Gizmos.DrawWireCube(vector, new Vector3(chunkSize, 0.1f, chunkSize));
				if (IsCameraChunk(i, j))
				{
					Gizmos.color = _cameraChunkColor;
					Gizmos.DrawWireCube(vector, new Vector3(chunkSize, 0.2f, chunkSize));
					Gizmos.DrawLine(_cameraPosition, vector);
				}
			}
		}
		_ = _showCoordinates;
		Gizmos.color = _cameraChunkColor;
		Gizmos.DrawSphere(_cameraPosition, 0.5f);
	}

	private bool IsCameraChunk(int x, int z)
	{
		if (x == _currentCameraChunk.x)
		{
			return z == _currentCameraChunk.y;
		}
		return false;
	}

	private bool IsVisibleChunk(int x, int z)
	{
		int num = Mathf.Abs(x - _currentCameraChunk.x);
		int num2 = Mathf.Abs(z - _currentCameraChunk.y);
		if (num <= _visibleChunkRange)
		{
			return num2 <= _visibleChunkRange;
		}
		return false;
	}
}
