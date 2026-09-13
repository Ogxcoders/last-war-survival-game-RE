using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI;

public class Button_LongPress : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerClickHandler
{
	private Color32 gray = new Color32(235, 235, 235, byte.MaxValue);

	public float pressDurationTime = 2f;

	private Action longPressAction;

	private Action clickAction;

	private Image[] bgImage;

	private bool _touchGray;

	private bool isDown;

	private float downTime;

	public void SetTouchBgGray(bool gray)
	{
		_touchGray = gray;
	}

	public void SetLongPressAction(Action _action)
	{
		longPressAction = _action;
	}

	public void SetClickAction(Action action)
	{
		clickAction = action;
	}

	private void Start()
	{
		bgImage = GetComponentsInChildren<Image>();
	}

	private void Update()
	{
		if (isDown)
		{
			downTime += Time.deltaTime;
			if (downTime > pressDurationTime)
			{
				isDown = false;
				longPressAction?.Invoke();
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isDown = true;
		downTime = 0f;
		if (bgImage != null && _touchGray)
		{
			for (int i = 0; i < bgImage.Length; i++)
			{
				bgImage[i].color = gray;
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (bgImage != null && _touchGray)
		{
			for (int i = 0; i < bgImage.Length; i++)
			{
				bgImage[i].color = Color.white;
			}
		}
		isDown = false;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		clickAction?.Invoke();
	}
}
