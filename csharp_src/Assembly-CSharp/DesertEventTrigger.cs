using System;
using UnityEngine;

public class DesertEventTrigger : ITouchObject
{
	public Action onPointerClick;

	[HideInInspector]
	public string previewIconPath;

	[HideInInspector]
	public string previewName;

	[HideInInspector]
	public WorldPreviewType previewType;

	public float Priority { get; set; }

	public Vector2Int TilePos { get; set; }

	public WorldPreviewType PreviewType => previewType;

	public bool OnClick()
	{
		onPointerClick?.Invoke();
		return onPointerClick != null;
	}

	public int GetPreviewType()
	{
		return (int)previewType;
	}

	public void OnDestroy()
	{
		onPointerClick = null;
	}

	public PointInfo GetPointInfo()
	{
		if (SceneManager.World != null)
		{
			return SceneManager.World.GetPointInfo(SceneManager.World.TilePosToIndex(TilePos));
		}
		return null;
	}
}
