using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("UI/HorizontalInfinityScrollView")]
public class HorizontalInfinityScrollView : InfinityScrollViewBase
{
	public override void Init(Action<GameObject, int> onInit, Action<GameObject, int, int> onUpdate, Action<GameObject, int> onDestroy)
	{
		base.Init(onInit, onUpdate, onDestroy);
	}

	public override void MoveItemByIndex(int index, float delay)
	{
		if ((index < 0) | (index > rectDic.Count))
		{
			return;
		}
		index = Math.Min(index, rectDic.Count);
		index = Math.Max(0, index);
		Vector2 anchoredPosition = rectTransform.anchoredPosition;
		Vector2 vector = new Vector2((float)index * (0f - GetItemSizeX()), anchoredPosition.y);
		Vector2 sizeDelta = scrollRect.content.sizeDelta;
		float num = 0f - (sizeDelta.x - maskSize.x);
		float num2 = 0f - (sizeDelta.y - maskSize.y);
		if (scrollRect.horizontal && vector.x < num)
		{
			vector.x = num;
		}
		else if (scrollRect.vertical && vector.y < num2)
		{
			vector.y = num2;
		}
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
			UpdateRender(vector);
		}
	}

	public override void ForceUpdate()
	{
		base.ForceUpdate();
	}

	protected override int CalculationItemCount()
	{
		int num = Mathf.CeilToInt(maskSize.x / GetItemSizeX());
		if (maxCount > 0 && maxCount <= num)
		{
			return maxCount;
		}
		return num + 1;
	}

	public override void Dispose()
	{
		base.Dispose();
	}

	public override void SetItemCount(int itemCount)
	{
		base.SetItemCount(itemCount);
	}

	public override InfinityItem GetInfinityItemByIndex(int index)
	{
		return base.GetInfinityItemByIndex(index);
	}

	public void SetScrollRectHorizontal(bool enable)
	{
		scrollRect.horizontal = enable;
	}

	protected override void UpdateDynmicRects(int itemCount)
	{
		rectDic = new Dictionary<int, InfinityRect>();
		for (int i = 0; i < itemCount; i++)
		{
			InfinityRect value = new InfinityRect((float)i * GetItemSizeX(), 0f, cellSize.x, cellSize.y, i);
			rectDic[i] = value;
		}
	}

	protected override void SetRenderListSize(int itemCount)
	{
		rectTransform.sizeDelta = new Vector2((float)itemCount * GetItemSizeX(), rectTransform.sizeDelta.y);
		maskRect = new Rect(0f, 0f, maskSize.x, maskSize.y);
	}

	protected override Vector2 GetItemAnchoredPos(int index)
	{
		Vector2 zero = Vector2.zero;
		if ((index < 0) | (index > rectDic.Count))
		{
			return zero;
		}
		index = Math.Min(index, rectDic.Count);
		index = Math.Max(0, index);
		Vector2 anchoredPosition = rectTransform.anchoredPosition;
		zero.Set((float)index * (0f - GetItemSizeX()), anchoredPosition.y);
		return zero;
	}

	protected override void UpdateItemTransformPos(GameObject item, int index)
	{
		((RectTransform)item.transform).anchoredPosition3D = new Vector3((float)index * GetItemSizeX(), 0f, 0f);
	}

	protected override void UpdateMaskRect()
	{
		maskRect.x = 0f - rectTransform.anchoredPosition.x;
	}
}
