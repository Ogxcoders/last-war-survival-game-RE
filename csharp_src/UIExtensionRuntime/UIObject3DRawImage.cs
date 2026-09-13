using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
[ExecuteInEditMode]
public class UIObject3DRawImage : RawImage, ILayoutElement
{
	float ILayoutElement.flexibleHeight => 1f;

	float ILayoutElement.flexibleWidth => 1f;

	int ILayoutElement.layoutPriority => -1;

	float ILayoutElement.minHeight => 0f;

	float ILayoutElement.minWidth => 0f;

	float ILayoutElement.preferredHeight => 0f;

	float ILayoutElement.preferredWidth => 0f;

	void ILayoutElement.CalculateLayoutInputHorizontal()
	{
	}

	void ILayoutElement.CalculateLayoutInputVertical()
	{
	}
}
