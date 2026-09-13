using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class ColorPickerAlphaSlider : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
{
	public delegate void OnValueChangedDelegate(float alpha);

	private RectTransform rectTransform;

	[SerializeField]
	private Image alphaImage;

	[SerializeField]
	private RectTransform selector;

	private float m_value;

	public OnValueChangedDelegate OnValueChanged;

	public float Value
	{
		get
		{
			return m_value;
		}
		set
		{
			m_value = value;
			selector.anchorMin = new Vector2(m_value, 0.5f);
			selector.anchorMax = new Vector2(m_value, 0.5f);
		}
	}

	public Color Color
	{
		get
		{
			return alphaImage.color;
		}
		set
		{
			value.a = 1f;
			alphaImage.color = value;
		}
	}

	private void Awake()
	{
		rectTransform = (RectTransform)base.transform;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnDrag(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
		Value = Mathf.Clamp01(localPoint.x / rectTransform.sizeDelta.x);
		if (OnValueChanged != null)
		{
			OnValueChanged(m_value);
		}
	}
}
