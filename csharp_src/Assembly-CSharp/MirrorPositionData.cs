using System;
using UnityEngine;

[Serializable]
public class MirrorPositionData
{
	public Vector2 anchoredPosition;

	public Vector2 anchorMin;

	public Vector2 anchorMax;

	public Vector2 pivot;

	public Quaternion localRotation;

	public Vector3 localScale;

	public MirrorPositionData(GameObject gameObject)
	{
		if (gameObject.transform is RectTransform rectTransform)
		{
			anchoredPosition = rectTransform.anchoredPosition;
			anchorMin = rectTransform.anchorMin;
			anchorMax = rectTransform.anchorMax;
			pivot = rectTransform.pivot;
			localRotation = rectTransform.localRotation;
			localScale = rectTransform.localScale;
		}
	}
}
