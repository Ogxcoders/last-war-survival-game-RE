using UnityEngine;

public interface ITouchObject
{
	float Priority { get; }

	Vector2Int TilePos { get; }

	WorldPreviewType PreviewType { get; }

	PointInfo GetPointInfo();
}
