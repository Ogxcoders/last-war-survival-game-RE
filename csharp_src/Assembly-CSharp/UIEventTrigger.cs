using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : EventTrigger
{
	private bool mIsDown;

	private float mTimePressStarted;

	private PointerEventData mPointDownEventData;

	public float durationThreshold = 0.3f;

	private bool mLongPressTriggered;

	private bool mHasDragged;

	public Action<PointerEventData> onBeginDrag;

	public Action<PointerEventData> onDrag;

	public Action<PointerEventData> onDrop;

	public Action<PointerEventData> onEndDrag;

	public Action<PointerEventData> onPointerClick;

	public Action<PointerEventData> onPointerDown;

	public Action<PointerEventData> onLongPress;

	public Action<PointerEventData> onPointerEnter;

	public Action<PointerEventData> onPointerExit;

	public Action<PointerEventData> onPointerUp;

	public override void OnBeginDrag(PointerEventData data)
	{
		onBeginDrag?.Invoke(data);
	}

	public override void OnDrag(PointerEventData data)
	{
		onDrag?.Invoke(data);
		mHasDragged = true;
	}

	public override void OnDrop(PointerEventData data)
	{
		onDrop?.Invoke(data);
	}

	public override void OnEndDrag(PointerEventData data)
	{
		onEndDrag?.Invoke(data);
	}

	public override void OnPointerClick(PointerEventData data)
	{
		onPointerClick?.Invoke(data);
	}

	public override void OnPointerDown(PointerEventData data)
	{
		onPointerDown?.Invoke(data);
		mIsDown = true;
		mTimePressStarted = Time.time;
		mPointDownEventData = data;
		mLongPressTriggered = false;
		mHasDragged = false;
	}

	public override void OnUpdateSelected(BaseEventData data)
	{
		if (mIsDown && !mLongPressTriggered && !mHasDragged && Time.time - mTimePressStarted >= durationThreshold)
		{
			mLongPressTriggered = true;
			onLongPress?.Invoke(mPointDownEventData);
		}
	}

	public override void OnPointerEnter(PointerEventData data)
	{
		onPointerEnter?.Invoke(data);
	}

	public override void OnPointerExit(PointerEventData data)
	{
		onPointerExit?.Invoke(data);
	}

	public override void OnPointerUp(PointerEventData data)
	{
		onPointerUp?.Invoke(data);
		mIsDown = false;
	}
}
