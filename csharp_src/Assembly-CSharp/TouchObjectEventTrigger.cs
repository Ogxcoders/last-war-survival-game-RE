using System;
using UnityEngine;

public class TouchObjectEventTrigger : MonoBehaviour, ITouchObjectPointerDownHandler, ITouchObject, ITouchObjectPointerUpHandler, ITouchObjectClickHandler, ITouchObjectDoubleClickHandler, ITouchObjectBeginDragHandler, ITouchObjectEndDragHandler, ITouchObjectDragHandler, ITouchObjectBeginLongTabHandler, ITouchObjectEndLongTabHandler, ITouchObjectPointerEnterHandler, ITouchObjectPointerExitHandler
{
	[SerializeField]
	private float priority;

	public Action<Vector3> onBeginDrag;

	public Action<Vector3, Vector3> onDrag;

	public Action<Vector3> onEndDrag;

	public Action onBeginLongTab;

	public Action onEndLongTab;

	public Action onPointerDown;

	public Action onPointerClick;

	public Action onPointerUp;

	public Action onPointerDoubleClick;

	public Action onPointerEnter;

	public Action onPointerExit;

	[HideInInspector]
	public string previewIconPath;

	[HideInInspector]
	public string previewName;

	[HideInInspector]
	public WorldPreviewType previewType;

	public float Priority => priority;

	public Vector2Int TilePos => SceneManager.World.WorldToTile(base.transform.position);

	public WorldPreviewType PreviewType => previewType;

	public bool OnClick()
	{
		onPointerClick?.Invoke();
		return onPointerClick != null;
	}

	public PointInfo GetPointInfo()
	{
		if (SceneManager.World != null)
		{
			return SceneManager.World.GetPointInfo(SceneManager.World.WorldToTileIndex(base.transform.position));
		}
		return null;
	}

	public bool OnDoubleClick()
	{
		onPointerDoubleClick?.Invoke();
		return onPointerDoubleClick != null;
	}

	public bool OnBeginDrag(Vector3 dragStartPos)
	{
		onBeginDrag?.Invoke(dragStartPos);
		return onBeginDrag != null;
	}

	public bool OnEndDrag(Vector3 dragStopPos)
	{
		onEndDrag?.Invoke(dragStopPos);
		return onEndDrag != null;
	}

	public bool OnDrag(Vector3 dragStartPos, Vector3 dragCurrPos)
	{
		onDrag?.Invoke(dragStartPos, dragCurrPos);
		return onDrag != null;
	}

	public bool OnBeginLongTap()
	{
		onBeginLongTab?.Invoke();
		return onBeginLongTab != null;
	}

	public bool OnEndLongTap()
	{
		onEndLongTab?.Invoke();
		return onEndLongTab != null;
	}

	public bool OnPointerEnter()
	{
		onPointerEnter?.Invoke();
		return onPointerEnter != null;
	}

	public bool OnPointerExit()
	{
		onPointerExit?.Invoke();
		return onPointerExit != null;
	}

	public bool OnPointerDown()
	{
		onPointerDown?.Invoke();
		return onPointerDown != null;
	}

	public bool OnPointerUp()
	{
		onPointerUp?.Invoke();
		return onPointerUp != null;
	}

	public int GetPreviewType()
	{
		return (int)previewType;
	}

	public void OnDestroy()
	{
		onBeginDrag = null;
		onDrag = null;
		onEndDrag = null;
		onBeginLongTab = null;
		onEndLongTab = null;
		onPointerDown = null;
		onPointerClick = null;
		onPointerUp = null;
		onPointerDoubleClick = null;
		onPointerEnter = null;
		onPointerExit = null;
	}
}
