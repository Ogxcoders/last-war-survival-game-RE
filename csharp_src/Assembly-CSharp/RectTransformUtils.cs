using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class RectTransformUtils
{
	private class AdjustmentInfo
	{
		public RectTransform rectTransform;

		public CanvasScaler canvasScaler;

		public Canvas canvas;

		public TextAnchor anchorPreset;

		public float originalLeftOffset;

		public float originalRightOffset;
	}

	private static float upper = 1f;

	private static float middle = 0.5f;

	private static float lower = 0f;

	private static float right = 1f;

	private static float center = 0.5f;

	private static float left = 0f;

	private static Dictionary<RectTransform, AdjustmentInfo> listenForAdjustment = new Dictionary<RectTransform, AdjustmentInfo>();

	public static Vector2 ToViewportCoords(this TextAnchor textAnchor)
	{
		return textAnchor switch
		{
			TextAnchor.UpperLeft => new Vector2(left, upper), 
			TextAnchor.UpperCenter => new Vector2(center, upper), 
			TextAnchor.UpperRight => new Vector2(right, upper), 
			TextAnchor.MiddleLeft => new Vector2(left, middle), 
			TextAnchor.MiddleCenter => new Vector2(center, middle), 
			TextAnchor.MiddleRight => new Vector2(right, middle), 
			TextAnchor.LowerLeft => new Vector2(left, lower), 
			TextAnchor.LowerCenter => new Vector2(center, lower), 
			TextAnchor.LowerRight => new Vector2(right, lower), 
			_ => throw new NotImplementedException("Didn't account for " + textAnchor), 
		};
	}

	public static void SetPivot(this RectTransform rectTransform, Vector2 newPivot)
	{
		Vector2 size = rectTransform.rect.size;
		Vector2 vector = rectTransform.pivot - newPivot;
		Vector3 vector2 = new Vector3(vector.x * size.x, vector.y * size.y);
		rectTransform.pivot = newPivot;
		rectTransform.localPosition -= vector2;
	}

	public static void SetPivot(this RectTransform rectTransform, TextAnchor newPivot)
	{
		Vector2 newPivot2 = newPivot.ToViewportCoords();
		rectTransform.SetPivot(newPivot2);
	}

	public static void SetAnchors(this RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax)
	{
		Vector2 size = rectTransform.rect.size;
		rectTransform.anchorMin = anchorMin;
		rectTransform.anchorMax = anchorMax;
		rectTransform.sizeDelta = size;
	}

	public static void AnchorToPoint(this RectTransform rectTransform, Vector2 anchorPoint)
	{
		rectTransform.SetAnchors(anchorPoint, anchorPoint);
	}

	public static void AnchorAndPivotToPoint(this RectTransform rectTransform, Vector2 point)
	{
		rectTransform.AnchorToPoint(point);
		rectTransform.SetPivot(point);
	}

	public static float RightEdgeX(this RectTransform rectTransform, bool inWorldCoordinates = false)
	{
		float num = rectTransform.rect.xMax;
		if (inWorldCoordinates)
		{
			num = rectTransform.TransformPoint(new Vector3(num, 0f, 0f)).x;
		}
		return num;
	}

	public static float LeftEdgeX(this RectTransform rectTransform, bool inWorldCoordinates = false)
	{
		float num = rectTransform.rect.xMin;
		if (inWorldCoordinates)
		{
			num = rectTransform.TransformPoint(new Vector3(num, 0f, 0f)).x;
		}
		return num;
	}

	public static float UpperEdgeY(this RectTransform rectTransform, bool inWorldCoordinates = false)
	{
		float num = rectTransform.rect.yMax;
		if (inWorldCoordinates)
		{
			num = rectTransform.TransformPoint(new Vector3(0f, num, 0f)).y;
		}
		return num;
	}

	public static float LowerEdgeY(this RectTransform rectTransform, bool inWorldCoordinates = false)
	{
		float num = rectTransform.rect.yMin;
		if (inWorldCoordinates)
		{
			num = rectTransform.TransformPoint(new Vector3(0f, num, 0f)).y;
		}
		return num;
	}

	public static Vector2 LowerLeftCorner(this RectTransform rectTransform, bool inWorldCoordinates = false)
	{
		Vector2 vector = new Vector2(rectTransform.LeftEdgeX(), rectTransform.LowerEdgeY());
		if (inWorldCoordinates)
		{
			return rectTransform.TransformPoint(vector);
		}
		return vector;
	}

	public static Vector2 LowerRightCorner(this RectTransform rectTransform, bool inWorldCoordinates = false)
	{
		Vector2 vector = new Vector2(rectTransform.RightEdgeX(), rectTransform.LowerEdgeY());
		if (inWorldCoordinates)
		{
			return rectTransform.TransformPoint(vector);
		}
		return vector;
	}

	public static Vector2 UpperRightCorner(this RectTransform rectTransform, bool inWorldCoordinates = false)
	{
		Vector2 vector = new Vector2(rectTransform.RightEdgeX(), rectTransform.UpperEdgeY());
		if (inWorldCoordinates)
		{
			return rectTransform.TransformPoint(vector);
		}
		return vector;
	}

	public static Vector2 UpperLeftCorner(this RectTransform rectTransform, bool inWorldCoordinates = false)
	{
		Vector2 vector = new Vector2(rectTransform.LeftEdgeX(), rectTransform.UpperEdgeY());
		if (inWorldCoordinates)
		{
			return rectTransform.TransformPoint(vector);
		}
		return vector;
	}

	public static Vector2 Center(this RectTransform rectTransform, bool inWorldCoordinates = false)
	{
		Vector2 vector = rectTransform.rect.center;
		if (inWorldCoordinates)
		{
			vector = rectTransform.TransformPoint(vector);
		}
		return vector;
	}

	public static float Width(this RectTransform rectTransform)
	{
		return rectTransform.rect.size.x;
	}

	public static float Height(this RectTransform rectTransform)
	{
		return rectTransform.rect.size.y;
	}

	public static Vector2 GetPositionOnRect(this RectTransform rectTransform, Vector2 anchorPos, bool inWorldCoordinates = false)
	{
		Vector2 result = rectTransform.LowerLeftCorner(inWorldCoordinates);
		result.x += rectTransform.Width() * anchorPos.x;
		result.y += rectTransform.Height() * anchorPos.y;
		return result;
	}

	public static void PositionRelativeToParent(this RectTransform rectTransform, Vector2 anchorPos)
	{
		RectTransform component = rectTransform.parent.GetComponent<RectTransform>();
		if (component == null)
		{
			throw new NullReferenceException($"{rectTransform.name} needs to have a parent to be positioned relative to it.");
		}
		Vector2 positionOnRect = component.GetPositionOnRect(anchorPos, inWorldCoordinates: true);
		positionOnRect.x -= rectTransform.Width() * (anchorPos.x - rectTransform.pivot.x);
		positionOnRect.y -= rectTransform.Height() * (anchorPos.y - rectTransform.pivot.y);
		rectTransform.position = positionOnRect;
	}

	public static void PositionRelativeToParent(this RectTransform rectTransform, TextAnchor preset)
	{
		Vector2 anchorPos = preset.ToViewportCoords();
		rectTransform.PositionRelativeToParent(anchorPos);
	}

	public static void ApplyAnchorPreset(this RectTransform rectTransform, TextAnchor presetToApply, bool alsoSetPivot = false, bool alsoSetPosition = false)
	{
		Vector2 vector = presetToApply.ToViewportCoords();
		rectTransform.SetAnchors(vector, vector);
		if (alsoSetPivot)
		{
			rectTransform.SetPivot(vector);
		}
		if (alsoSetPosition)
		{
			rectTransform.anchoredPosition = Vector2.zero;
		}
	}

	public static void ApplyAnchorPresetRecursively(this RectTransform rectTransform, TextAnchor presetToApply, bool alsoSetPivot = false, bool alsoSetPosition = false)
	{
		rectTransform.ApplyAnchorPreset(presetToApply, alsoSetPivot, alsoSetPosition);
		foreach (RectTransform item in rectTransform)
		{
			item.ApplyAnchorPresetRecursively(presetToApply, alsoSetPivot, alsoSetPosition);
		}
	}

	public static void ApplyAutoScaling(this RectTransform rectTransform, float aspectRatio)
	{
		AspectRatioFitter aspectRatioFitter = rectTransform.GetComponent<AspectRatioFitter>();
		if (aspectRatioFitter == null)
		{
			aspectRatioFitter = rectTransform.gameObject.AddComponent<AspectRatioFitter>();
		}
		aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
		aspectRatioFitter.aspectRatio = aspectRatio;
	}

	public static void AdjustWidthForCanvasScaler(this RectTransform rectTransform, TextAnchor anchorPreset = TextAnchor.MiddleCenter)
	{
		if (!listenForAdjustment.ContainsKey(rectTransform))
		{
			listenForAdjustment.Add(rectTransform, new AdjustmentInfo
			{
				rectTransform = rectTransform,
				canvasScaler = rectTransform.GetComponentInParent<CanvasScaler>(),
				canvas = rectTransform.GetComponentInParent<Canvas>(),
				anchorPreset = anchorPreset,
				originalLeftOffset = rectTransform.offsetMin.x,
				originalRightOffset = rectTransform.offsetMax.x
			});
			Adjustment(listenForAdjustment[rectTransform]);
		}
	}

	public static void ResetAdjustWidth(this RectTransform rectTransform)
	{
		if (listenForAdjustment.ContainsKey(rectTransform))
		{
			AdjustmentInfo adjustmentInfo = listenForAdjustment[rectTransform];
			rectTransform.offsetMin = new Vector2(adjustmentInfo.originalLeftOffset, rectTransform.offsetMin.y);
			rectTransform.offsetMax = new Vector2(adjustmentInfo.originalRightOffset, rectTransform.offsetMax.y);
			listenForAdjustment.Remove(rectTransform);
		}
	}

	private static void Adjustment(AdjustmentInfo info)
	{
		if (info.rectTransform == null || info.canvasScaler == null || info.canvas == null)
		{
			listenForAdjustment.Remove(info.rectTransform);
			return;
		}
		float num = (float)Screen.width / info.canvas.scaleFactor;
		float x = info.canvasScaler.referenceResolution.x;
		if (num > x)
		{
			float num2 = num - x;
			float num3 = num2 / 2f;
			if (info.anchorPreset == TextAnchor.UpperCenter || TextAnchor.MiddleCenter == info.anchorPreset || TextAnchor.LowerCenter == info.anchorPreset)
			{
				info.rectTransform.offsetMin = new Vector2(info.originalLeftOffset + num3, info.rectTransform.offsetMin.y);
				info.rectTransform.offsetMax = new Vector2(info.originalRightOffset - num3, info.rectTransform.offsetMax.y);
			}
			else if (info.anchorPreset == TextAnchor.UpperLeft || TextAnchor.MiddleLeft == info.anchorPreset || TextAnchor.LowerLeft == info.anchorPreset)
			{
				info.rectTransform.offsetMax = new Vector2(info.originalRightOffset - num2, info.rectTransform.offsetMax.y);
			}
			else if (TextAnchor.UpperRight == info.anchorPreset || TextAnchor.MiddleRight == info.anchorPreset || TextAnchor.LowerRight == info.anchorPreset)
			{
				info.rectTransform.offsetMin = new Vector2(info.originalLeftOffset + num2, info.rectTransform.offsetMin.y);
			}
		}
		else
		{
			info.rectTransform.offsetMin = new Vector2(info.originalLeftOffset, info.rectTransform.offsetMin.y);
			info.rectTransform.offsetMax = new Vector2(info.originalRightOffset, info.rectTransform.offsetMax.y);
		}
	}

	public static void _UpdateAdjustmentByResizeEvent()
	{
		foreach (KeyValuePair<RectTransform, AdjustmentInfo> item in listenForAdjustment)
		{
			Adjustment(item.Value);
		}
	}
}
