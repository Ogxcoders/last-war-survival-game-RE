using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class XYLayoutGroup : XYLayoutElement
{
	private readonly Vector2 ITEM_ANCHOR = new Vector2(0.5f, 0.5f);

	private bool isDirty;

	[Header("布局设置")]
	[Tooltip("布局方向")]
	public XYLayoutDirection direction;

	public TextAnchor childAlignment;

	public float spacing;

	protected Vector2 size = Vector2.zero;

	private List<XYLayoutElement> childElements = new List<XYLayoutElement>();

	private Vector2 _lastCalculatedSize;

	public override XYLayoutElementType ElementType => XYLayoutElementType.Group;

	public override Vector2 Anchor => childAlignment switch
	{
		TextAnchor.UpperLeft => new Vector2(0f, 0f), 
		TextAnchor.UpperCenter => new Vector2(0.5f, 0f), 
		TextAnchor.UpperRight => new Vector2(1f, 0f), 
		TextAnchor.MiddleLeft => new Vector2(0f, 0.5f), 
		TextAnchor.MiddleCenter => new Vector2(0.5f, 0.5f), 
		TextAnchor.MiddleRight => new Vector2(1f, 0.5f), 
		TextAnchor.LowerLeft => new Vector2(0f, 1f), 
		TextAnchor.LowerCenter => new Vector2(0.5f, 1f), 
		TextAnchor.LowerRight => new Vector2(1f, 1f), 
		_ => new Vector2(0.5f, 0.5f), 
	};

	public override Vector2 Size => size;

	public bool IsDirty => isDirty;

	public void RegisterElement(XYLayoutElement element)
	{
		if (!childElements.Contains(element))
		{
			childElements.Add(element);
			RefreshLayout();
		}
	}

	public void UnregisterElement(XYLayoutElement element)
	{
		if (childElements.Contains(element))
		{
			childElements.Remove(element);
			RefreshLayout();
		}
	}

	public void RefreshLayout()
	{
		if (parentGroup == null)
		{
			isDirty = true;
		}
		else
		{
			parentGroup.RefreshLayout();
		}
	}

	private void Update()
	{
		if (!(parentGroup != null) && IsDirty)
		{
			CalculateChildrenRect();
			UpdateChildrenLocalPosition(Vector2.zero);
			isDirty = false;
		}
	}

	private void OnTransformChildrenChanged()
	{
		TryUpdateParentLayout();
	}

	private void CalculateChildrenRect()
	{
		localRect.Set(0f, 0f, 0f, 0f);
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		int childCount = base.transform.childCount;
		if (childCount == 0)
		{
			return;
		}
		Vector2 center = Vector2.zero;
		XYLayoutElement xYLayoutElement = null;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (!child.gameObject.activeInHierarchy)
			{
				continue;
			}
			XYLayoutElement component = child.GetComponent<XYLayoutElement>();
			if (component == null)
			{
				continue;
			}
			if (direction == XYLayoutDirection.Horizontal)
			{
				if (xYLayoutElement == null)
				{
					ApplyCenterOffset(ref center, 0f);
				}
				else
				{
					ApplyCenterOffset(ref center, spacing);
				}
			}
			else if (xYLayoutElement == null)
			{
				ApplyCenterOffset(ref center, 0f);
			}
			else
			{
				ApplyCenterOffset(ref center, spacing);
			}
			switch (component.ElementType)
			{
			case XYLayoutElementType.Group:
			{
				XYLayoutGroup xYLayoutGroup = component as XYLayoutGroup;
				xYLayoutGroup?.CalculateChildrenRect();
				CalculateChildElement(xYLayoutGroup, ref center);
				break;
			}
			case XYLayoutElementType.Item:
			{
				XYLayoutItem item = component as XYLayoutItem;
				CalculateChildElement(item, ref center);
				break;
			}
			}
			xYLayoutElement = component;
			localRect = MergeRect(localRect, component.LocalRect);
		}
		size = localRect.size;
	}

	public override void PresetCenter(Vector2 centerXY)
	{
		localRect.center = centerXY;
	}

	private void ApplyCenterOffset(ref Vector2 center, float offset)
	{
		switch (direction)
		{
		case XYLayoutDirection.Vertical:
			center.y -= offset;
			break;
		case XYLayoutDirection.Horizontal:
			center.x += offset;
			break;
		}
	}

	private void CalculateChildElement(XYLayoutElement item, ref Vector2 center)
	{
		switch (direction)
		{
		case XYLayoutDirection.Vertical:
			CalculateVerticalElement(item, ref center);
			break;
		case XYLayoutDirection.Horizontal:
			CalculateHorizontalElement(item, ref center);
			break;
		}
	}

	private void CalculateVerticalElement(XYLayoutElement item, ref Vector2 parentCenter)
	{
		Vector2 vector = item.Size;
		Vector2 zero = Vector2.zero;
		Vector2 iTEM_ANCHOR = ITEM_ANCHOR;
		switch (childAlignment)
		{
		case TextAnchor.UpperLeft:
		case TextAnchor.MiddleLeft:
		case TextAnchor.LowerLeft:
			zero.Set(vector.x * iTEM_ANCHOR.x, (0f - vector.y) * iTEM_ANCHOR.y);
			break;
		case TextAnchor.UpperCenter:
		case TextAnchor.MiddleCenter:
		case TextAnchor.LowerCenter:
			zero.Set(0f, (0f - vector.y) * iTEM_ANCHOR.y);
			break;
		case TextAnchor.UpperRight:
		case TextAnchor.MiddleRight:
		case TextAnchor.LowerRight:
			zero.Set((0f - vector.x) * (1f - iTEM_ANCHOR.x), (0f - vector.y) * iTEM_ANCHOR.y);
			break;
		}
		Vector2 centerXY = parentCenter + zero;
		item.PresetCenter(centerXY);
		zero.y -= (1f - iTEM_ANCHOR.y) * vector.y;
		parentCenter.y += zero.y;
	}

	private void CalculateHorizontalElement(XYLayoutElement item, ref Vector2 parentCenter)
	{
		Vector2 vector = item.Size;
		Vector2 zero = Vector2.zero;
		Vector2 iTEM_ANCHOR = ITEM_ANCHOR;
		switch (childAlignment)
		{
		case TextAnchor.UpperLeft:
		case TextAnchor.UpperCenter:
		case TextAnchor.UpperRight:
			zero.Set(vector.x * iTEM_ANCHOR.x, (0f - vector.y) * iTEM_ANCHOR.y);
			break;
		case TextAnchor.MiddleLeft:
		case TextAnchor.MiddleCenter:
		case TextAnchor.MiddleRight:
			zero.Set(vector.x * iTEM_ANCHOR.x, 0f);
			break;
		case TextAnchor.LowerLeft:
		case TextAnchor.LowerCenter:
		case TextAnchor.LowerRight:
			zero.Set(vector.x * iTEM_ANCHOR.x, vector.y * iTEM_ANCHOR.y);
			break;
		}
		Vector2 centerXY = parentCenter + zero;
		item.PresetCenter(centerXY);
		zero.x += (1f - iTEM_ANCHOR.x) * vector.x;
		parentCenter.x += zero.x;
	}

	private void UpdateChildrenLocalPosition(Vector2 offset)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		int childCount = base.transform.childCount;
		if (childCount == 0)
		{
			return;
		}
		Vector2 zero = Vector2.zero;
		if (direction == XYLayoutDirection.Horizontal)
		{
			switch (childAlignment)
			{
			case TextAnchor.UpperLeft:
			case TextAnchor.MiddleLeft:
			case TextAnchor.LowerLeft:
				zero.Set(0f, 0f);
				break;
			case TextAnchor.UpperCenter:
			case TextAnchor.MiddleCenter:
			case TextAnchor.LowerCenter:
				zero.Set((0f - size.x) * 0.5f, 0f);
				break;
			case TextAnchor.UpperRight:
			case TextAnchor.MiddleRight:
			case TextAnchor.LowerRight:
				zero.Set(0f - size.x, 0f);
				break;
			}
		}
		else
		{
			switch (childAlignment)
			{
			case TextAnchor.UpperLeft:
			case TextAnchor.UpperCenter:
			case TextAnchor.UpperRight:
				zero.Set(0f, 0f);
				break;
			case TextAnchor.MiddleLeft:
			case TextAnchor.MiddleCenter:
			case TextAnchor.MiddleRight:
				zero.Set(0f, size.y * 0.5f);
				break;
			case TextAnchor.LowerLeft:
			case TextAnchor.LowerCenter:
			case TextAnchor.LowerRight:
				zero.Set(0f, size.y);
				break;
			}
		}
		zero += offset;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (!child.gameObject.activeInHierarchy)
			{
				continue;
			}
			XYLayoutElement component = child.GetComponent<XYLayoutElement>();
			if (!(component == null))
			{
				switch (component.ElementType)
				{
				case XYLayoutElementType.Group:
					(component as XYLayoutGroup)?.RefreshLocalPosition(zero);
					break;
				case XYLayoutElementType.Item:
					(component as XYLayoutItem)?.RefreshLocalPosition(zero);
					break;
				}
			}
		}
		if (parentGroup == null)
		{
			localRect.center += zero;
		}
	}

	public override void RefreshLocalPosition(Vector2 parentStartPosition)
	{
		base.transform.localPosition = parentStartPosition + localRect.center;
		localRect.center = Vector2.zero;
		Vector2 vector = size * 0.5f;
		Vector2 vector2 = Anchor * size;
		UpdateChildrenLocalPosition(new Vector2(0f - vector.x + vector2.x, vector.y - vector2.y));
	}
}
