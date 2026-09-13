using System;
using System.Collections.Generic;

namespace UnityEngine.UI;

public static class LayoutUtility
{
	public static float GetMinSize(RectTransform rect, int axis)
	{
		if (axis == 0)
		{
			return GetMinWidth(rect);
		}
		return GetMinHeight(rect);
	}

	public static float GetPreferredSize(RectTransform rect, int axis)
	{
		if (axis == 0)
		{
			return GetPreferredWidth(rect);
		}
		return GetPreferredHeight(rect);
	}

	public static float GetFlexibleSize(RectTransform rect, int axis)
	{
		if (axis == 0)
		{
			return GetFlexibleWidth(rect);
		}
		return GetFlexibleHeight(rect);
	}

	public static float GetMinWidth(RectTransform rect)
	{
		return GetLayoutProperty(rect, (ILayoutElement e) => e.minWidth, 0f);
	}

	public static float GetPreferredWidth(RectTransform rect)
	{
		return Mathf.Max(GetLayoutProperty(rect, (ILayoutElement e) => e.minWidth, 0f), GetLayoutProperty(rect, (ILayoutElement e) => e.preferredWidth, 0f));
	}

	public static float GetFlexibleWidth(RectTransform rect)
	{
		return GetLayoutProperty(rect, (ILayoutElement e) => e.flexibleWidth, 0f);
	}

	public static float GetMinHeight(RectTransform rect)
	{
		return GetLayoutProperty(rect, (ILayoutElement e) => e.minHeight, 0f);
	}

	public static float GetPreferredHeight(RectTransform rect)
	{
		return Mathf.Max(GetLayoutProperty(rect, (ILayoutElement e) => e.minHeight, 0f), GetLayoutProperty(rect, (ILayoutElement e) => e.preferredHeight, 0f));
	}

	public static float GetFlexibleHeight(RectTransform rect)
	{
		return GetLayoutProperty(rect, (ILayoutElement e) => e.flexibleHeight, 0f);
	}

	public static float GetLayoutProperty(RectTransform rect, Func<ILayoutElement, float> property, float defaultValue)
	{
		ILayoutElement source;
		return GetLayoutProperty(rect, property, defaultValue, out source);
	}

	public static float GetLayoutProperty(RectTransform rect, Func<ILayoutElement, float> property, float defaultValue, out ILayoutElement source)
	{
		source = null;
		if (rect == null)
		{
			return 0f;
		}
		float num = defaultValue;
		int num2 = int.MinValue;
		List<Component> list = ListPool<Component>.Get();
		rect.GetComponents(typeof(ILayoutElement), list);
		for (int i = 0; i < list.Count; i++)
		{
			ILayoutElement layoutElement = list[i] as ILayoutElement;
			if (layoutElement is Behaviour && !((Behaviour)layoutElement).isActiveAndEnabled)
			{
				continue;
			}
			int layoutPriority = layoutElement.layoutPriority;
			if (layoutPriority < num2)
			{
				continue;
			}
			float num3 = property(layoutElement);
			if (!(num3 < 0f))
			{
				if (layoutPriority > num2)
				{
					num = num3;
					num2 = layoutPriority;
					source = layoutElement;
				}
				else if (num3 > num)
				{
					num = num3;
					source = layoutElement;
				}
			}
		}
		ListPool<Component>.Release(list);
		return num;
	}
}
