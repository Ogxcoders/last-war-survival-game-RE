using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BitBenderGames;

public class TouchInputController2 : MonoBehaviour
{
	public delegate void InputDragStartDelegate(Vector3 pos, bool isLongTap, Vector3 offset);

	public delegate void Input1PositionDelegate(Vector3 pos);

	public delegate void DragUpdateDelegate(Vector3 dragPosStart, Vector3 dragPosCurrent, Vector3 correctionOffset);

	public delegate void DragStopDelegate(Vector3 dragStopPos, Vector3 dragFinalMomentum);

	public delegate void PinchStartDelegate(Vector3 pinchCenter, float pinchStartDistance);

	public delegate void PinchUpdateDelegate(Vector3 pinchCenter, float pinchDistance, float pinchStartDistance);

	public delegate void PinchUpdateExtendedDelegate(PinchUpdateData pinchUpdateData);

	public delegate void InputLongTapProgress(float progress);

	public delegate void InputClickDelegate(Vector3 clickPosition, bool isDoubleClick, bool isLongTap);

	[Header("Expert Mode")]
	[SerializeField]
	private bool expertModeEnabled;

	[SerializeField]
	[Tooltip("当手指在一个物品上停留至少这个时间而不动时，这个手势就被认为是一个长敲击.")]
	private float clickDurationThreshold = 0.7f;

	[SerializeField]
	[Tooltip("当连续两次点击之间的时间短于此时间时，可以识别双击手势.")]
	private float doubleclickDurationThreshold = 0.5f;

	[SerializeField]
	[Tooltip("此值控制用户必须执行倾斜手势才能将其识别为垂直线的距离.")]
	private float tiltMoveDotTreshold = 0.7f;

	[SerializeField]
	[Tooltip("阈值，用于检测手指是否水平到足以启动倾斜。使用此值可以防止将垂直手指放置计算为倾斜手势.")]
	private float tiltHorizontalDotThreshold = 0.5f;

	[SerializeField]
	[Tooltip("当用户的手指移动的距离超过这个值时，就会开始拖动。该值被定义为规范化值。拖动屏幕的整个宽度等于1。拖动整个屏幕的高度也等于1")]
	private float dragStartDistanceThresholdRelative = 0.05f;

	[SerializeField]
	[Tooltip("当启用此标志时，当长点击时间成功时，立即调用拖动启动事件.")]
	private bool longTapStartsDrag;

	[SerializeField]
	private bool checkInputOnUI;

	private float lastFingerDownTimeReal;

	private float lastClickTimeReal;

	private bool wasFingerDownLastFrame;

	private Vector3 lastFinger0DownPos;

	private const float dragDurationThreshold = 0.01f;

	private bool isDragging;

	private Vector3 dragStartPos;

	private Vector3 dragStartOffset;

	private List<Vector3> DragFinalMomentumVector;

	private const int momentumSamplesCount = 5;

	private float pinchStartDistance;

	private List<Vector3> pinchStartPositions;

	private List<Vector3> touchPositionLastFrame;

	private Vector3 pinchRotationVectorStart = Vector3.zero;

	private Vector3 pinchVectorLastFrame = Vector3.zero;

	private float totalFingerMovement;

	private bool wasDraggingLastFrame;

	private bool wasPinchingLastFrame;

	private bool isPinching;

	private bool isInputOnLockedArea;

	private float timeSinceDragStart;

	private bool isClickPrevented;

	private bool isFingerDown;

	public bool stopUpdate;

	public bool IsDragging => isDragging;

	public bool IsPinching => isPinching;

	public bool LongTapStartsDrag => longTapStartsDrag;

	public bool IsInputOnLockedArea
	{
		get
		{
			return isInputOnLockedArea;
		}
		set
		{
			isInputOnLockedArea = value;
		}
	}

	public event InputDragStartDelegate OnDragStart;

	public event Input1PositionDelegate OnFingerDown;

	public event Input1PositionDelegate OnFingerUp;

	public event DragUpdateDelegate OnDragUpdate;

	public event DragStopDelegate OnDragStop;

	public event PinchStartDelegate OnPinchStart;

	public event PinchUpdateDelegate OnPinchUpdate;

	public event PinchUpdateExtendedDelegate OnPinchUpdateExtended;

	public event Action OnPinchStop;

	public event InputLongTapProgress OnLongTapProgress;

	public event InputClickDelegate OnInputClick;

	public void Awake()
	{
		lastFingerDownTimeReal = 0f;
		lastClickTimeReal = 0f;
		lastFinger0DownPos = Vector3.zero;
		dragStartPos = Vector3.zero;
		isDragging = false;
		wasFingerDownLastFrame = false;
		DragFinalMomentumVector = new List<Vector3>();
		pinchStartPositions = new List<Vector3>
		{
			Vector3.zero,
			Vector3.zero
		};
		touchPositionLastFrame = new List<Vector3>
		{
			Vector3.zero,
			Vector3.zero
		};
		pinchStartDistance = 1f;
		isPinching = false;
		isClickPrevented = false;
	}

	public void OnEventTriggerPointerDown(GameObject go)
	{
		isInputOnLockedArea = true;
	}

	public void OnEventTriggerPointerDown(BaseEventData baseEventData)
	{
		isInputOnLockedArea = true;
	}

	private void InputDonwOnUI()
	{
		if (isInputOnLockedArea || !TouchWrapper.IsFingerDown)
		{
			return;
		}
		if (Input.touchCount > 0)
		{
			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
			{
				isInputOnLockedArea = true;
			}
		}
		else if (!isFingerDown && EventSystem.current.IsPointerOverGameObject())
		{
			isInputOnLockedArea = true;
		}
	}

	public void Update()
	{
		if (stopUpdate)
		{
			return;
		}
		if (checkInputOnUI)
		{
			InputDonwOnUI();
		}
		if (!TouchWrapper.IsFingerDown)
		{
			isInputOnLockedArea = false;
		}
		bool flag = false;
		if (!isInputOnLockedArea)
		{
			if (!isPinching)
			{
				if (TouchWrapper.TouchCount == 2)
				{
					StartPinch();
					isPinching = true;
				}
			}
			else if (TouchWrapper.TouchCount < 2)
			{
				StopPinch();
				isPinching = false;
			}
			else if (TouchWrapper.TouchCount == 2)
			{
				UpdatePinch();
			}
			if (!isPinching)
			{
				if (!wasPinchingLastFrame)
				{
					if (wasFingerDownLastFrame && TouchWrapper.IsFingerDown && !isDragging)
					{
						float relativeDragDistance = GetRelativeDragDistance(TouchWrapper.Touch0.Position, dragStartPos);
						float num = Time.realtimeSinceStartup - lastFingerDownTimeReal;
						_ = Input.touchCount;
						_ = 0;
						bool flag2 = num > clickDurationThreshold;
						if (this.OnLongTapProgress != null)
						{
							float progress = 0f;
							if (!Mathf.Approximately(clickDurationThreshold, 0f))
							{
								progress = num;
							}
							this.OnLongTapProgress(progress);
						}
						if (TouchWrapper.TouchCount == 1 && ((relativeDragDistance >= dragStartDistanceThresholdRelative && num >= 0.01f) || (longTapStartsDrag && flag2)))
						{
							isDragging = true;
							dragStartOffset = lastFinger0DownPos - dragStartPos;
							DragStart(dragStartPos, flag2, dragStartOffset);
						}
					}
				}
				else if (TouchWrapper.IsFingerDown)
				{
					isDragging = true;
					dragStartPos = TouchWrapper.Touch0.Position;
					DragStart(dragStartPos, isLongTap: false, Vector3.zero);
					flag = true;
				}
				if (isDragging && TouchWrapper.IsFingerDown)
				{
					DragUpdate(TouchWrapper.Touch0.Position);
				}
				if (isDragging && !TouchWrapper.IsFingerDown)
				{
					isDragging = false;
					DragStop(lastFinger0DownPos);
				}
			}
			if (!isPinching && !isDragging && !wasPinchingLastFrame && !wasDraggingLastFrame && !isClickPrevented)
			{
				if (!wasFingerDownLastFrame && TouchWrapper.IsFingerDown)
				{
					lastFingerDownTimeReal = Time.realtimeSinceStartup;
					dragStartPos = TouchWrapper.Touch0.Position;
					FingerDown(TouchWrapper.AverageTouchPos);
				}
				if (wasFingerDownLastFrame && !TouchWrapper.IsFingerDown)
				{
					float num2 = Time.realtimeSinceStartup - lastFingerDownTimeReal;
					if (!wasDraggingLastFrame && !wasPinchingLastFrame)
					{
						bool isDoubleClick = Time.realtimeSinceStartup - lastClickTimeReal < doubleclickDurationThreshold;
						bool isLongTap = num2 > clickDurationThreshold;
						this.OnInputClick?.Invoke(lastFinger0DownPos, isDoubleClick, isLongTap);
						lastClickTimeReal = Time.realtimeSinceStartup;
					}
				}
			}
		}
		if (isDragging && TouchWrapper.IsFingerDown && !flag)
		{
			DragFinalMomentumVector.Add(TouchWrapper.Touch0.Position - lastFinger0DownPos);
			if (DragFinalMomentumVector.Count > 5)
			{
				DragFinalMomentumVector.RemoveAt(0);
			}
		}
		if (!isInputOnLockedArea)
		{
			wasFingerDownLastFrame = TouchWrapper.IsFingerDown;
		}
		if (wasFingerDownLastFrame)
		{
			lastFinger0DownPos = TouchWrapper.Touch0.Position;
		}
		wasDraggingLastFrame = isDragging;
		wasPinchingLastFrame = isPinching;
		if (TouchWrapper.TouchCount == 0)
		{
			isClickPrevented = false;
			if (isFingerDown)
			{
				FingerUp();
			}
		}
	}

	public void RestartDrag()
	{
		if (isDragging)
		{
			DragStop(lastFinger0DownPos);
			if (TouchWrapper.TouchCount > 0)
			{
				dragStartOffset = Vector3.zero;
				dragStartPos = TouchWrapper.Touch0.Position;
				DragStart(dragStartPos, isLongTap: false, Vector3.zero);
			}
		}
	}

	private void StartPinch()
	{
		List<Vector3> list = pinchStartPositions;
		Vector3 value = (touchPositionLastFrame[0] = TouchWrapper.Touches[0].Position);
		list[0] = value;
		List<Vector3> list2 = pinchStartPositions;
		value = (touchPositionLastFrame[1] = TouchWrapper.Touches[1].Position);
		list2[1] = value;
		pinchStartDistance = GetPinchDistance(pinchStartPositions[0], pinchStartPositions[1]);
		this.OnPinchStart?.Invoke((pinchStartPositions[0] + pinchStartPositions[1]) * 0.5f, pinchStartDistance);
		isClickPrevented = true;
		pinchRotationVectorStart = TouchWrapper.Touches[1].Position - TouchWrapper.Touches[0].Position;
		pinchVectorLastFrame = pinchRotationVectorStart;
		totalFingerMovement = 0f;
	}

	private void UpdatePinch()
	{
		float pinchDistance = GetPinchDistance(TouchWrapper.Touches[0].Position, TouchWrapper.Touches[1].Position);
		Vector3 vector = TouchWrapper.Touches[1].Position - TouchWrapper.Touches[0].Position;
		float num = ((!(Vector3.Cross(pinchVectorLastFrame, vector).z < 0f)) ? 1 : (-1));
		float num2 = 0f;
		if (!Mathf.Approximately(Vector3.Distance(pinchVectorLastFrame, vector), 0f))
		{
			num2 = Vector3.Angle(pinchVectorLastFrame, vector) * num;
		}
		float num3 = Mathf.Abs(pinchVectorLastFrame.magnitude - vector.magnitude);
		float pinchAngleDeltaNormalized = 0f;
		if (!Mathf.Approximately(num3, 0f))
		{
			pinchAngleDeltaNormalized = num2 / num3;
		}
		Vector3 pinchCenter = (TouchWrapper.Touches[0].Position + TouchWrapper.Touches[1].Position) * 0.5f;
		float pinchTiltDelta = 0f;
		Vector3 touchPositionRelative = GetTouchPositionRelative(TouchWrapper.Touches[0].Position - touchPositionLastFrame[0]);
		Vector3 touchPositionRelative2 = GetTouchPositionRelative(TouchWrapper.Touches[1].Position - touchPositionLastFrame[1]);
		float f = Vector2.Dot(touchPositionRelative.normalized, Vector2.up);
		float f2 = Vector2.Dot(touchPositionRelative2.normalized, Vector2.up);
		float f3 = Vector3.Dot(vector.normalized, Vector3.right);
		if (Mathf.Sign(f) == Mathf.Sign(f2) && Mathf.Abs(f) > tiltMoveDotTreshold && Mathf.Abs(f2) > tiltMoveDotTreshold && Mathf.Abs(f3) >= tiltHorizontalDotThreshold)
		{
			pinchTiltDelta = 0.5f * (touchPositionRelative.y + touchPositionRelative2.y);
		}
		totalFingerMovement += touchPositionRelative.magnitude + touchPositionRelative2.magnitude;
		this.OnPinchUpdate?.Invoke(pinchCenter, pinchDistance, pinchStartDistance);
		this.OnPinchUpdateExtended?.Invoke(new PinchUpdateData
		{
			pinchCenter = pinchCenter,
			pinchDistance = pinchDistance,
			pinchStartDistance = pinchStartDistance,
			pinchAngleDelta = num2,
			pinchAngleDeltaNormalized = pinchAngleDeltaNormalized,
			pinchTiltDelta = pinchTiltDelta,
			pinchTotalFingerMovement = totalFingerMovement
		});
		pinchVectorLastFrame = vector;
		touchPositionLastFrame[0] = TouchWrapper.Touches[0].Position;
		touchPositionLastFrame[1] = TouchWrapper.Touches[1].Position;
	}

	private float GetPinchDistance(Vector3 pos0, Vector3 pos1)
	{
		float num = Mathf.Abs(pos0.x - pos1.x) / (float)Screen.width;
		float num2 = Mathf.Abs(pos0.y - pos1.y) / (float)Screen.height;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	private void StopPinch()
	{
		dragStartOffset = Vector3.zero;
		this.OnPinchStop?.Invoke();
	}

	private void DragStart(Vector3 pos, bool isLongTap, Vector3 offset)
	{
		this.OnDragStart?.Invoke(pos, isLongTap, offset);
		isClickPrevented = true;
		timeSinceDragStart = 0f;
		DragFinalMomentumVector.Clear();
	}

	private void DragUpdate(Vector3 pos)
	{
		if (this.OnDragUpdate != null)
		{
			timeSinceDragStart += Time.deltaTime;
			Vector3 correctionOffset = Vector3.Lerp(Vector3.zero, dragStartOffset, Mathf.Clamp01(timeSinceDragStart * 10f));
			this.OnDragUpdate(dragStartPos, pos, correctionOffset);
		}
	}

	private void DragStop(Vector3 pos)
	{
		if (this.OnDragStop != null)
		{
			Vector3 zero = Vector3.zero;
			if (DragFinalMomentumVector.Count > 0)
			{
				for (int i = 0; i < DragFinalMomentumVector.Count; i++)
				{
					zero += DragFinalMomentumVector[i];
				}
				zero /= (float)DragFinalMomentumVector.Count;
			}
			this.OnDragStop(pos, zero);
		}
		DragFinalMomentumVector.Clear();
	}

	private void FingerDown(Vector3 pos)
	{
		isFingerDown = true;
		this.OnFingerDown?.Invoke(pos);
	}

	private void FingerUp()
	{
		isFingerDown = false;
		this.OnFingerUp?.Invoke(lastFinger0DownPos);
	}

	private Vector3 GetTouchPositionRelative(Vector3 touchPosScreen)
	{
		return new Vector3(touchPosScreen.x / (float)Screen.width, touchPosScreen.y / (float)Screen.height, touchPosScreen.z);
	}

	private float GetRelativeDragDistance(Vector3 pos0, Vector3 pos1)
	{
		Vector2 vector = pos0 - pos1;
		return new Vector2(vector.x / (float)Screen.width, vector.y / (float)Screen.height).magnitude;
	}

	public void ClearAllEvent()
	{
		this.OnDragStart = null;
		this.OnFingerDown = null;
		this.OnFingerUp = null;
		this.OnDragUpdate = null;
		this.OnDragStop = null;
		this.OnPinchStart = null;
		this.OnPinchUpdate = null;
		this.OnPinchUpdateExtended = null;
		this.OnLongTapProgress = null;
		this.OnInputClick = null;
	}

	public void ClearEventOnInputClick()
	{
		this.OnInputClick = null;
	}

	public void SetOnInputClick(InputClickDelegate delegateFunc)
	{
		this.OnInputClick = delegateFunc;
	}

	public void SetOnDragStart(InputDragStartDelegate delegateFunc)
	{
		this.OnDragStart = delegateFunc;
	}

	public void SetOnDragUpdate(DragUpdateDelegate delegateFunc)
	{
		this.OnDragUpdate = delegateFunc;
	}

	public void SetOnDragStop(DragStopDelegate delegateFunc)
	{
		this.OnDragStop = delegateFunc;
	}

	public void SetOnPinchStart(PinchStartDelegate delegateFunc)
	{
		this.OnPinchStart = delegateFunc;
	}

	public void SetOnPinchUpdate(PinchUpdateDelegate delegateFunc)
	{
		this.OnPinchUpdate = delegateFunc;
	}

	public void SetOnPinchStop(Action actFunc)
	{
		this.OnPinchStop = actFunc;
	}
}
