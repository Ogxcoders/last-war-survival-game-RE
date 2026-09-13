using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("UI/GridInfinityScrollView")]
public class GridInfinityScrollView : InfinityScrollViewBase
{
	[SerializeField]
	private int minColumnCount;

	[SerializeField]
	private int maxColumnCount;

	[SerializeField]
	private int columnCount;

	[SerializeField]
	public bool isRTLInArabic;

	public override void Init(Action<GameObject, int> onInit, Action<GameObject, int, int> onUpdate, Action<GameObject, int> onDestroy)
	{
		RefreshMaskSize();
		base.Init(onInit, onUpdate, onDestroy);
	}

	public override void ForceUpdate()
	{
		base.ForceUpdate();
	}

	public override void ForceUpdateCell()
	{
		base.ForceUpdateCell();
	}

	public override void SetItemCount(int itemCount)
	{
		base.SetItemCount(itemCount);
	}

	public override void Dispose()
	{
		base.Dispose();
	}

	public override void MoveItemByIndex(int index, float delay)
	{
		if (rectDic == null || ((index < 0) | (index > rectDic.Count)))
		{
			return;
		}
		Vector2 anchoredPosition = rectTransform.anchoredPosition;
		int num = index / columnCount;
		Vector2 vector = new Vector2(anchoredPosition.x, (float)num * GetItemSizeY());
		if (delay > 0f)
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}
			_coroutine = StartCoroutine(TweenMoveToPos(anchoredPosition, vector, delay));
		}
		else
		{
			rectTransform.anchoredPosition = vector;
			UpdateRender(vector, isForce: true);
		}
	}

	public void LaterItemByIndex(int index, float delay)
	{
		if (!((index < 0) | (index > rectDic.Count)))
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}
			_coroutine = StartCoroutine(TweenUpdateToPoss(index, delay));
		}
	}

	protected override Vector2 GetItemAnchoredPos(int index)
	{
		Vector2 zero = Vector2.zero;
		if ((index < 0) | (index > rectDic.Count))
		{
			return zero;
		}
		_ = rectTransform.anchoredPosition;
		int num = index / columnCount;
		int num2 = index % columnCount;
		zero.Set((float)num2 * GetItemSizeX(), (float)num * (GetItemSizeY() * -1f));
		return zero;
	}

	public override int GetColumnCount()
	{
		return columnCount;
	}

	protected override int CalculationItemCount()
	{
		if (maxCount > 0 && maxCount <= columnCount * Mathf.CeilToInt(maskSize.y / GetItemSizeY()))
		{
			return maxCount;
		}
		return columnCount * (Mathf.CeilToInt(maskSize.y / GetItemSizeY()) + 2);
	}

	protected override void SetRenderListSize(int itemCount)
	{
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (float)Mathf.CeilToInt((float)itemCount * 1f / (float)columnCount) * GetItemSizeY() - spacingSize.y);
		maskRect = new Rect(0f, 0f - maskSize.y, maskSize.x, maskSize.y);
	}

	protected override void UpdateDynmicRects(int itemCount)
	{
		rectDic = new Dictionary<int, InfinityRect>();
		for (int i = 0; i < itemCount; i++)
		{
			int num = i / columnCount;
			InfinityRect value = new InfinityRect((float)(i % columnCount) * GetItemSizeX(), (float)(-num) * GetItemSizeY() - cellSize.y, cellSize.x, cellSize.y, i);
			rectDic[i] = value;
		}
	}

	protected override void UpdateItemTransformPos(GameObject item, int index)
	{
		bool flag = UIRunTimeConfig.IsArabic && MirrorVersionConfig.IsMirrorVersionOpen && isRTLInArabic;
		if (flag && item.transform is RectTransform rectTransform)
		{
			if ((double)Math.Abs(rectTransform.anchorMin.x - rectTransform.anchorMax.x) < 0.01 && (double)rectTransform.anchorMin.x < 0.5)
			{
				rectTransform.anchorMin = new Vector2(1f - rectTransform.anchorMin.x, rectTransform.anchorMin.y);
				rectTransform.anchorMax = new Vector2(1f - rectTransform.anchorMax.x, rectTransform.anchorMax.y);
			}
			if ((double)rectTransform.pivot.x < 0.5)
			{
				rectTransform.pivot = new Vector2(1f - rectTransform.pivot.x, rectTransform.pivot.y);
			}
		}
		int num = index / columnCount;
		int num2 = index % columnCount;
		((RectTransform)item.transform).anchoredPosition3D = new Vector3((float)(((!flag) ? 1 : (-1)) * num2) * GetItemSizeX(), (float)num * (GetItemSizeY() * -1f), 0f);
	}

	protected override void UpdateMaskRect()
	{
		maskRect.y = 0f - maskSize.y - rectTransform.anchoredPosition.y;
	}

	public float GetRenderItemSizeY()
	{
		return GetItemSizeY();
	}

	public void RefreshMaskSize()
	{
		if (minColumnCount != 0 && maxColumnCount != 0)
		{
			rectTransform = base.transform as RectTransform;
			RectTransform component = base.transform.parent.GetComponent<RectTransform>();
			maskSize = new Vector2(component.rect.width, component.rect.height);
			columnCount = Mathf.CeilToInt(maskSize.x / GetItemSizeX());
			columnCount = Math.Min(maxColumnCount, columnCount);
			columnCount = Math.Max(minColumnCount, columnCount);
		}
	}
}
