using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SuperScrollView;

public class DragChangSizeScript : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	private bool mIsDraging;

	public Camera mCamera;

	public float mBorderSize = 10f;

	public Texture2D mCursorTexture;

	public Vector2 mCursorHotSpot = new Vector2(16f, 16f);

	private RectTransform mCachedRectTransform;

	public Action mOnDragEndAction;

	public RectTransform CachedRectTransform
	{
		get
		{
			if (mCachedRectTransform == null)
			{
				mCachedRectTransform = base.gameObject.GetComponent<RectTransform>();
			}
			return mCachedRectTransform;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetCursor(mCursorTexture, mCursorHotSpot, CursorMode.Auto);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SetCursor(null, mCursorHotSpot, CursorMode.Auto);
	}

	private void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
	{
		if (Input.mousePresent)
		{
			Cursor.SetCursor(texture, hotspot, cursorMode);
		}
	}

	private void LateUpdate()
	{
		if (mCursorTexture == null)
		{
			return;
		}
		if (mIsDraging)
		{
			SetCursor(mCursorTexture, mCursorHotSpot, CursorMode.Auto);
			return;
		}
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(CachedRectTransform, Input.mousePosition, mCamera, out var localPoint))
		{
			SetCursor(null, mCursorHotSpot, CursorMode.Auto);
			return;
		}
		float num = CachedRectTransform.rect.width - localPoint.x;
		if (num < 0f)
		{
			SetCursor(null, mCursorHotSpot, CursorMode.Auto);
		}
		else if (num <= mBorderSize)
		{
			SetCursor(mCursorTexture, mCursorHotSpot, CursorMode.Auto);
		}
		else
		{
			SetCursor(null, mCursorHotSpot, CursorMode.Auto);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		mIsDraging = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		mIsDraging = false;
		if (mOnDragEndAction != null)
		{
			mOnDragEndAction();
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(CachedRectTransform, eventData.position, mCamera, out var localPoint);
		if (!(localPoint.x <= 0f))
		{
			CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, localPoint.x);
		}
	}
}
