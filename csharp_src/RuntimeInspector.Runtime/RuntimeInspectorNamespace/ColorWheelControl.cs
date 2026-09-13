using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class ColorWheelControl : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
{
	public delegate void OnColorChangedDelegate(Color32 color);

	private const float RGB_CONST = 2f / MathF.PI;

	private const float G_CONST = MathF.PI * 2f / 3f;

	private const float B_CONST = 4.1887903f;

	private Color m_color;

	private RectTransform rectTransform;

	[SerializeField]
	private RectTransform SelectorOut;

	[SerializeField]
	private RectTransform SelectorIn;

	[SerializeField]
	private WindowDragHandler colorPickerWindow;

	private float outer;

	private Vector2 inner;

	private Material mat;

	private bool draggingOuter;

	private bool draggingInner;

	private float halfSize;

	private float halfSizeSqr;

	private float outerCirclePaddingSqr;

	private float innerSquareHalfSize;

	private int pointerId = -98765;

	public Color Color
	{
		get
		{
			return m_color;
		}
		private set
		{
			if (m_color != value)
			{
				m_color = value;
				m_color.a = Alpha;
				if (this.OnColorChanged != null)
				{
					this.OnColorChanged(m_color);
				}
			}
		}
	}

	public float Alpha { get; set; }

	public event OnColorChangedDelegate OnColorChanged;

	private void Awake()
	{
		rectTransform = (RectTransform)base.transform;
		Image component = GetComponent<Image>();
		mat = new Material(component.material);
		component.material = mat;
		UpdateProperties();
	}

	private void OnRectTransformDimensionsChange()
	{
		if (!(rectTransform == null))
		{
			UpdateProperties();
			UpdateSelectors();
		}
	}

	private void UpdateProperties()
	{
		halfSize = rectTransform.rect.size.x * 0.5f;
		halfSizeSqr = halfSize * halfSize;
		outerCirclePaddingSqr = halfSizeSqr * 0.75f * 0.75f;
		innerSquareHalfSize = halfSize * 0.5f;
	}

	public void PickColor(Color c)
	{
		Alpha = c.a;
		Color.RGBToHSV(c, out var H, out var S, out var V);
		outer = H * 2f * MathF.PI;
		inner.x = 1f - S;
		inner.y = 1f - V;
		UpdateSelectors();
		Color = c;
		mat.SetColor("_Color", GetCurrentBaseColor());
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint))
		{
			return;
		}
		float sqrMagnitude = localPoint.sqrMagnitude;
		if (sqrMagnitude <= halfSizeSqr && sqrMagnitude >= outerCirclePaddingSqr)
		{
			draggingOuter = true;
		}
		else
		{
			if (!(Mathf.Abs(localPoint.x) <= innerSquareHalfSize) || !(Mathf.Abs(localPoint.y) <= innerSquareHalfSize))
			{
				return;
			}
			draggingInner = true;
		}
		GetSelectedColor(localPoint);
		pointerId = eventData.pointerId;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (pointerId != eventData.pointerId)
		{
			eventData.pointerDrag = colorPickerWindow.gameObject;
			colorPickerWindow.OnBeginDrag(eventData);
		}
		else
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
			GetSelectedColor(localPoint);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (pointerId == eventData.pointerId)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
			GetSelectedColor(localPoint);
			draggingOuter = false;
			draggingInner = false;
			pointerId = -98765;
		}
	}

	private void GetSelectedColor(Vector2 pointerPos)
	{
		if (draggingOuter)
		{
			Vector2 vector = -pointerPos.normalized;
			outer = Mathf.Atan2(0f - vector.x, 0f - vector.y);
			UpdateColor();
		}
		else if (draggingInner)
		{
			Vector2 vector2 = -pointerPos;
			vector2.x = Mathf.Clamp(vector2.x, 0f - innerSquareHalfSize, innerSquareHalfSize) + innerSquareHalfSize;
			vector2.y = Mathf.Clamp(vector2.y, 0f - innerSquareHalfSize, innerSquareHalfSize) + innerSquareHalfSize;
			inner = vector2 / halfSize;
			UpdateColor();
		}
		UpdateSelectors();
	}

	private void UpdateColor()
	{
		Color currentBaseColor = GetCurrentBaseColor();
		mat.SetColor("_Color", currentBaseColor);
		currentBaseColor = Color.Lerp(currentBaseColor, Color.white, inner.x);
		currentBaseColor = Color.Lerp(currentBaseColor, Color.black, inner.y);
		Color = currentBaseColor;
	}

	private Color GetCurrentBaseColor()
	{
		Color white = Color.white;
		white.r = Mathf.Clamp(2f / MathF.PI * Mathf.Asin(Mathf.Cos(outer)) * 1.5f + 0.5f, 0f, 1f);
		white.g = Mathf.Clamp(2f / MathF.PI * Mathf.Asin(Mathf.Cos(MathF.PI * 2f / 3f - outer)) * 1.5f + 0.5f, 0f, 1f);
		white.b = Mathf.Clamp(2f / MathF.PI * Mathf.Asin(Mathf.Cos(4.1887903f - outer)) * 1.5f + 0.5f, 0f, 1f);
		return white;
	}

	private void UpdateSelectors()
	{
		SelectorOut.anchoredPosition = new Vector2(Mathf.Sin(outer) * halfSize * 0.85f, Mathf.Cos(outer) * halfSize * 0.85f);
		SelectorIn.anchoredPosition = new Vector2(innerSquareHalfSize - inner.x * halfSize, innerSquareHalfSize - inner.y * halfSize);
	}
}
