using System;

namespace UnityEngine;

internal struct SliderHandler
{
	private readonly Rect position;

	private readonly float currentValue;

	private readonly float size;

	private readonly float start;

	private readonly float end;

	private readonly GUIStyle slider;

	private readonly GUIStyle thumb;

	private readonly GUIStyle thumbExtent;

	private readonly bool horiz;

	private readonly int id;

	public SliderHandler(Rect position, float currentValue, float size, float start, float end, GUIStyle slider, GUIStyle thumb, bool horiz, int id, GUIStyle thumbExtent = null)
	{
		this.position = position;
		this.currentValue = currentValue;
		this.size = size;
		this.start = start;
		this.end = end;
		this.slider = slider;
		this.thumb = thumb;
		this.thumbExtent = thumbExtent;
		this.horiz = horiz;
		this.id = id;
	}

	public float Handle()
	{
		if (slider == null || thumb == null)
		{
			return currentValue;
		}
		return CurrentEventType() switch
		{
			EventType.MouseDown => OnMouseDown(), 
			EventType.MouseDrag => OnMouseDrag(), 
			EventType.MouseUp => OnMouseUp(), 
			EventType.Repaint => OnRepaint(), 
			_ => currentValue, 
		};
	}

	private float OnMouseDown()
	{
		Vector2 mousePosition = CurrentEvent().mousePosition;
		Rect rect = ThumbSelectionRect();
		bool flag = GUIUtility.HitTest(rect, CurrentEvent());
		Rect zero = Rect.zero;
		zero.xMin = Math.Min(position.xMin, rect.xMin);
		zero.xMax = Math.Max(position.xMax, rect.xMax);
		zero.yMin = Math.Min(position.yMin, rect.yMin);
		zero.yMax = Math.Max(position.yMax, rect.yMax);
		if (IsEmptySlider() || (!GUIUtility.HitTest(zero, CurrentEvent()) && !flag))
		{
			return currentValue;
		}
		GUI.scrollTroughSide = 0;
		GUIUtility.hotControl = id;
		CurrentEvent().Use();
		if (flag)
		{
			StartDraggingWithValue(ClampedCurrentValue());
			return currentValue;
		}
		GUI.changed = true;
		if (SupportsPageMovements())
		{
			SliderState().isDragging = false;
			GUI.nextScrollStepTime = SystemClock.now.AddMilliseconds(250.0);
			GUI.scrollTroughSide = CurrentScrollTroughSide();
			return PageMovementValue();
		}
		float num = ValueForCurrentMousePosition();
		StartDraggingWithValue(num);
		return Clamp(num);
	}

	private float OnMouseDrag()
	{
		if (GUIUtility.hotControl != id)
		{
			return currentValue;
		}
		SliderState sliderState = SliderState();
		if (!sliderState.isDragging)
		{
			return currentValue;
		}
		GUI.changed = true;
		CurrentEvent().Use();
		float num = MousePosition() - sliderState.dragStartPos;
		float value = sliderState.dragStartValue + num / ValuesPerPixel();
		return Clamp(value);
	}

	private float OnMouseUp()
	{
		if (GUIUtility.hotControl == id)
		{
			CurrentEvent().Use();
			GUIUtility.hotControl = 0;
		}
		return currentValue;
	}

	private float OnRepaint()
	{
		bool flag = GUIUtility.HitTest(position, CurrentEvent());
		slider.Draw(position, GUIContent.none, id, on: false, flag);
		if (!IsEmptySlider() && currentValue >= Mathf.Min(start, end) && currentValue <= Mathf.Max(start, end))
		{
			if (thumbExtent != null)
			{
				thumbExtent.Draw(ThumbExtRect(), GUIContent.none, id, on: false, flag);
			}
			thumb.Draw(ThumbRect(), GUIContent.none, id, on: false, flag);
		}
		if (GUIUtility.hotControl != id || !flag || IsEmptySlider())
		{
			return currentValue;
		}
		if (GUIUtility.HitTest(ThumbRect(), CurrentEvent()))
		{
			if (GUI.scrollTroughSide != 0)
			{
				GUIUtility.hotControl = 0;
			}
			return currentValue;
		}
		GUI.InternalRepaintEditorWindow();
		if (SystemClock.now < GUI.nextScrollStepTime)
		{
			return currentValue;
		}
		if (CurrentScrollTroughSide() != GUI.scrollTroughSide)
		{
			return currentValue;
		}
		GUI.nextScrollStepTime = SystemClock.now.AddMilliseconds(30.0);
		if (SupportsPageMovements())
		{
			SliderState().isDragging = false;
			GUI.changed = true;
			return PageMovementValue();
		}
		return ClampedCurrentValue();
	}

	private EventType CurrentEventType()
	{
		return CurrentEvent().GetTypeForControl(id);
	}

	private int CurrentScrollTroughSide()
	{
		float num = (horiz ? CurrentEvent().mousePosition.x : CurrentEvent().mousePosition.y);
		float num2 = (horiz ? ThumbRect().x : ThumbRect().y);
		return (num > num2) ? 1 : (-1);
	}

	private bool IsEmptySlider()
	{
		return start == end;
	}

	private bool SupportsPageMovements()
	{
		return size != 0f && GUI.usePageScrollbars;
	}

	private float PageMovementValue()
	{
		float num = currentValue;
		int num2 = ((!(start > end)) ? 1 : (-1));
		num = ((!(MousePosition() > PageUpMovementBound())) ? (num - size * (float)num2 * 0.9f) : (num + size * (float)num2 * 0.9f));
		return Clamp(num);
	}

	private float PageUpMovementBound()
	{
		if (horiz)
		{
			return ThumbRect().xMax - position.x;
		}
		return ThumbRect().yMax - position.y;
	}

	private Event CurrentEvent()
	{
		return Event.current;
	}

	private float ValueForCurrentMousePosition()
	{
		if (horiz)
		{
			return (MousePosition() - ThumbRect().width * 0.5f) / ValuesPerPixel() + start - size * 0.5f;
		}
		return (MousePosition() - ThumbRect().height * 0.5f) / ValuesPerPixel() + start - size * 0.5f;
	}

	private float Clamp(float value)
	{
		return Mathf.Clamp(value, MinValue(), MaxValue());
	}

	private Rect ThumbSelectionRect()
	{
		return ThumbRect();
	}

	private void StartDraggingWithValue(float dragStartValue)
	{
		SliderState sliderState = SliderState();
		sliderState.dragStartPos = MousePosition();
		sliderState.dragStartValue = dragStartValue;
		sliderState.isDragging = true;
	}

	private SliderState SliderState()
	{
		return (SliderState)GUIUtility.GetStateObject(typeof(SliderState), id);
	}

	private Rect ThumbExtRect()
	{
		Rect result = new Rect(0f, 0f, thumbExtent.fixedWidth, thumbExtent.fixedHeight);
		result.center = ThumbRect().center;
		return result;
	}

	private Rect ThumbRect()
	{
		return horiz ? HorizontalThumbRect() : VerticalThumbRect();
	}

	private Rect VerticalThumbRect()
	{
		Rect rect = thumb.margin.Remove(slider.padding.Remove(position));
		float width = ((thumb.fixedWidth != 0f) ? thumb.fixedWidth : rect.width);
		float num = ThumbSize();
		float num2 = ValuesPerPixel();
		if (start < end)
		{
			return new Rect(rect.x, (ClampedCurrentValue() - start) * num2 + rect.y, width, size * num2 + num);
		}
		return new Rect(rect.x, (ClampedCurrentValue() + size - start) * num2 + rect.y, width, size * (0f - num2) + num);
	}

	private Rect HorizontalThumbRect()
	{
		Rect rect = thumb.margin.Remove(slider.padding.Remove(position));
		float height = ((thumb.fixedHeight != 0f) ? thumb.fixedHeight : rect.height);
		float num = ThumbSize();
		float num2 = ValuesPerPixel();
		if (start < end)
		{
			return new Rect((ClampedCurrentValue() - start) * num2 + rect.x, rect.y, size * num2 + num, height);
		}
		return new Rect((ClampedCurrentValue() + size - start) * num2 + rect.x, rect.y, size * (0f - num2) + num, height);
	}

	private float ClampedCurrentValue()
	{
		return Clamp(currentValue);
	}

	private float MousePosition()
	{
		if (horiz)
		{
			return CurrentEvent().mousePosition.x - position.x;
		}
		return CurrentEvent().mousePosition.y - position.y;
	}

	private float ValuesPerPixel()
	{
		if (horiz)
		{
			return (position.width - (float)slider.padding.horizontal - ThumbSize()) / (end - start);
		}
		return (position.height - (float)slider.padding.vertical - ThumbSize()) / (end - start);
	}

	private float ThumbSize()
	{
		if (horiz)
		{
			return (thumb.fixedWidth != 0f) ? thumb.fixedWidth : ((float)thumb.padding.horizontal);
		}
		return (thumb.fixedHeight != 0f) ? thumb.fixedHeight : ((float)thumb.padding.vertical);
	}

	private float MaxValue()
	{
		return Mathf.Max(start, end) - size;
	}

	private float MinValue()
	{
		return Mathf.Min(start, end);
	}
}
