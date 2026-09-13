using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

[LuaCallCSharp(GenFlag.No)]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class ChatItemFrame : UIBehaviour
{
	[Serializable]
	public class AlignItem
	{
		public RectTransform rectTransform;

		public float offset;
	}

	private static Vector2 AlignToLeftPivot = new Vector2(0f, 1f);

	private static Vector2 AlignToRightPivot = new Vector2(1f, 1f);

	[DoNotGen]
	public bool alignToLeft = true;

	[DoNotGen]
	public AlignItem[] alignItems;

	private Dictionary<RectTransform, int> _additionOffset = new Dictionary<RectTransform, int>();

	[ContextMenu("SwapAlign")]
	public void SwapAlign()
	{
		SwapAlign(!alignToLeft);
	}

	public void SwapAlign(bool toLeft)
	{
		alignToLeft = toLeft;
		int i = 0;
		for (int num = alignItems.Length; i < num; i++)
		{
			AlignItem alignItem = alignItems[i];
			alignItem.rectTransform.pivot = (alignToLeft ? AlignToLeftPivot : AlignToRightPivot);
			RectTransform.Edge edge = ((!alignToLeft) ? RectTransform.Edge.Right : RectTransform.Edge.Left);
			_additionOffset.TryGetValue(alignItem.rectTransform, out var value);
			SetInsetFromParentEdge(alignItem.rectTransform, edge, alignItem.offset + (float)value);
		}
	}

	private void SetInsetFromParentEdge(RectTransform rectTransform, RectTransform.Edge edge, float inset)
	{
		int index = ((edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Bottom) ? 1 : 0);
		bool flag = edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Right;
		float value = (flag ? 1f : 0f);
		Vector2 anchorMin = rectTransform.anchorMin;
		anchorMin[index] = value;
		rectTransform.anchorMin = anchorMin;
		Vector2 anchorMax = rectTransform.anchorMax;
		anchorMax[index] = value;
		rectTransform.anchorMax = anchorMax;
		Vector2 anchoredPosition = rectTransform.anchoredPosition;
		anchoredPosition[index] = (flag ? (0f - inset) : inset);
		rectTransform.anchoredPosition = anchoredPosition;
	}

	public void ClearAdditionOffset()
	{
		_additionOffset.Clear();
	}

	public void AddAdditionOffset(RectTransform rect, int offset)
	{
		_additionOffset[rect] = offset;
	}
}
