using UnityEngine;

public class AutoMirrorPositionData
{
	public float width;

	public Vector3 position;

	public Vector2 pivot;

	public Quaternion localRotation;

	public string name;

	public AutoMirrorPositionData(GameObject gameObject)
	{
		if (gameObject.transform is RectTransform rectTransform)
		{
			width = rectTransform.rect.width;
			position = rectTransform.position;
			pivot = rectTransform.pivot;
			name = rectTransform.name;
			localRotation = rectTransform.localRotation;
		}
	}
}
