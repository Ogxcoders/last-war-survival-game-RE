#define UNITY_ASSERTIONS
namespace UnityEngine.UIElements;

public abstract class PointerEventBase<T> : EventBase<T>, IPointerEvent, IPointerEventInternal where T : PointerEventBase<T>, new()
{
	public int pointerId { get; protected set; }

	public string pointerType { get; protected set; }

	public bool isPrimary { get; protected set; }

	public int button { get; protected set; }

	public int pressedButtons { get; protected set; }

	public Vector3 position { get; protected set; }

	public Vector3 localPosition { get; protected set; }

	public Vector3 deltaPosition { get; protected set; }

	public float deltaTime { get; protected set; }

	public int clickCount { get; protected set; }

	public float pressure { get; protected set; }

	public float tangentialPressure { get; protected set; }

	public float altitudeAngle { get; protected set; }

	public float azimuthAngle { get; protected set; }

	public float twist { get; protected set; }

	public Vector2 radius { get; protected set; }

	public Vector2 radiusVariance { get; protected set; }

	public EventModifiers modifiers { get; protected set; }

	public bool shiftKey => (modifiers & EventModifiers.Shift) != 0;

	public bool ctrlKey => (modifiers & EventModifiers.Control) != 0;

	public bool commandKey => (modifiers & EventModifiers.Command) != 0;

	public bool altKey => (modifiers & EventModifiers.Alt) != 0;

	public bool actionKey
	{
		get
		{
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			{
				return commandKey;
			}
			return ctrlKey;
		}
	}

	bool IPointerEventInternal.triggeredByOS { get; set; }

	bool IPointerEventInternal.recomputeTopElementUnderPointer { get; set; }

	public override IEventHandler currentTarget
	{
		get
		{
			return base.currentTarget;
		}
		internal set
		{
			base.currentTarget = value;
			if (currentTarget is VisualElement ele)
			{
				localPosition = ele.WorldToLocal(position);
			}
			else
			{
				localPosition = position;
			}
		}
	}

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Bubbles | EventPropagation.TricklesDown | EventPropagation.Cancellable;
		base.propagateToIMGUI = false;
		pointerId = 0;
		pointerType = PointerType.unknown;
		isPrimary = false;
		button = -1;
		pressedButtons = 0;
		position = Vector3.zero;
		localPosition = Vector3.zero;
		deltaPosition = Vector3.zero;
		deltaTime = 0f;
		clickCount = 0;
		pressure = 0f;
		tangentialPressure = 0f;
		altitudeAngle = 0f;
		azimuthAngle = 0f;
		twist = 0f;
		radius = Vector2.zero;
		radiusVariance = Vector2.zero;
		modifiers = EventModifiers.None;
		((IPointerEventInternal)this).triggeredByOS = false;
		((IPointerEventInternal)this).recomputeTopElementUnderPointer = false;
	}

	private static bool IsMouse(Event systemEvent)
	{
		EventType rawType = systemEvent.rawType;
		return rawType == EventType.MouseMove || rawType == EventType.MouseDown || rawType == EventType.MouseUp || rawType == EventType.MouseDrag || rawType == EventType.ContextClick || rawType == EventType.MouseEnterWindow || rawType == EventType.MouseLeaveWindow;
	}

	public static T GetPooled(Event systemEvent)
	{
		T val = EventBase<T>.GetPooled();
		if (!IsMouse(systemEvent) && systemEvent.rawType != EventType.DragUpdated)
		{
			Debug.Assert(condition: false, string.Concat("Unexpected event type: ", systemEvent.rawType, " (", systemEvent.type, ")"));
		}
		switch (systemEvent.pointerType)
		{
		default:
			val.pointerType = PointerType.mouse;
			val.pointerId = PointerId.mousePointerId;
			break;
		case UnityEngine.PointerType.Touch:
			val.pointerType = PointerType.touch;
			val.pointerId = PointerId.touchPointerIdBase;
			break;
		case UnityEngine.PointerType.Pen:
			val.pointerType = PointerType.pen;
			val.pointerId = PointerId.penPointerIdBase;
			break;
		}
		val.isPrimary = true;
		val.altitudeAngle = 0f;
		val.azimuthAngle = 0f;
		val.twist = 0f;
		val.radius = Vector2.zero;
		val.radiusVariance = Vector2.zero;
		val.imguiEvent = systemEvent;
		if (systemEvent.rawType == EventType.MouseDown)
		{
			PointerDeviceState.PressButton(PointerId.mousePointerId, systemEvent.button);
			val.button = systemEvent.button;
		}
		else if (systemEvent.rawType == EventType.MouseUp)
		{
			PointerDeviceState.ReleaseButton(PointerId.mousePointerId, systemEvent.button);
			val.button = systemEvent.button;
		}
		else if (systemEvent.rawType == EventType.MouseMove)
		{
			val.button = -1;
		}
		val.pressedButtons = PointerDeviceState.GetPressedButtons(val.pointerId);
		val.position = systemEvent.mousePosition;
		val.localPosition = systemEvent.mousePosition;
		val.deltaPosition = systemEvent.delta;
		val.clickCount = systemEvent.clickCount;
		val.modifiers = systemEvent.modifiers;
		UnityEngine.PointerType pointerType = systemEvent.pointerType;
		if ((uint)(pointerType - 1) > 1u)
		{
			val.pressure = ((val.pressedButtons == 0) ? 0f : 0.5f);
		}
		else
		{
			val.pressure = systemEvent.pressure;
		}
		val.tangentialPressure = 0f;
		((IPointerEventInternal)val).triggeredByOS = true;
		return val;
	}

	public static T GetPooled(Touch touch, EventModifiers modifiers = EventModifiers.None)
	{
		T val = EventBase<T>.GetPooled();
		val.pointerId = touch.fingerId + PointerId.touchPointerIdBase;
		val.pointerType = PointerType.touch;
		bool flag = false;
		for (int i = PointerId.touchPointerIdBase; i < PointerId.touchPointerIdBase + PointerId.touchPointerCount; i++)
		{
			if (i != val.pointerId && PointerDeviceState.GetPressedButtons(i) != 0)
			{
				flag = true;
				break;
			}
		}
		val.isPrimary = !flag;
		if (touch.phase == TouchPhase.Began)
		{
			PointerDeviceState.PressButton(val.pointerId, 0);
			val.button = 0;
		}
		else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
		{
			PointerDeviceState.ReleaseButton(val.pointerId, 0);
			val.button = 0;
		}
		else
		{
			val.button = -1;
		}
		val.pressedButtons = PointerDeviceState.GetPressedButtons(val.pointerId);
		val.position = touch.position;
		val.localPosition = touch.position;
		val.deltaPosition = touch.deltaPosition;
		val.deltaTime = touch.deltaTime;
		val.clickCount = touch.tapCount;
		val.pressure = ((Mathf.Abs(touch.maximumPossiblePressure) > Mathf.Epsilon) ? (touch.pressure / touch.maximumPossiblePressure) : 1f);
		val.tangentialPressure = 0f;
		val.altitudeAngle = touch.altitudeAngle;
		val.azimuthAngle = touch.azimuthAngle;
		val.twist = 0f;
		val.radius = new Vector2(touch.radius, touch.radius);
		val.radiusVariance = new Vector2(touch.radiusVariance, touch.radiusVariance);
		val.modifiers = modifiers;
		((IPointerEventInternal)val).triggeredByOS = true;
		return val;
	}

	internal static T GetPooled(IPointerEvent triggerEvent, Vector2 position, int pointerId)
	{
		if (triggerEvent != null)
		{
			return GetPooled(triggerEvent);
		}
		T val = EventBase<T>.GetPooled();
		val.position = position;
		val.localPosition = position;
		val.pointerId = pointerId;
		val.pointerType = PointerType.GetPointerType(pointerId);
		return val;
	}

	public static T GetPooled(IPointerEvent triggerEvent)
	{
		T val = EventBase<T>.GetPooled();
		if (triggerEvent != null)
		{
			val.pointerId = triggerEvent.pointerId;
			val.pointerType = triggerEvent.pointerType;
			val.isPrimary = triggerEvent.isPrimary;
			val.button = triggerEvent.button;
			val.pressedButtons = triggerEvent.pressedButtons;
			val.position = triggerEvent.position;
			val.localPosition = triggerEvent.localPosition;
			val.deltaPosition = triggerEvent.deltaPosition;
			val.deltaTime = triggerEvent.deltaTime;
			val.clickCount = triggerEvent.clickCount;
			val.pressure = triggerEvent.pressure;
			val.tangentialPressure = triggerEvent.tangentialPressure;
			val.altitudeAngle = triggerEvent.altitudeAngle;
			val.azimuthAngle = triggerEvent.azimuthAngle;
			val.twist = triggerEvent.twist;
			val.radius = triggerEvent.radius;
			val.radiusVariance = triggerEvent.radiusVariance;
			val.modifiers = triggerEvent.modifiers;
			if (triggerEvent is IPointerEventInternal pointerEventInternal)
			{
				((IPointerEventInternal)val).triggeredByOS = pointerEventInternal.triggeredByOS;
			}
		}
		return val;
	}

	protected internal override void PreDispatch(IPanel panel)
	{
		base.PreDispatch(panel);
		if (((IPointerEventInternal)this).triggeredByOS)
		{
			PointerDeviceState.SavePointerPosition(pointerId, position, panel);
		}
	}

	protected internal override void PostDispatch(IPanel panel)
	{
		for (int i = 0; i < PointerId.maxPointers; i++)
		{
			panel.ProcessPointerCapture(i);
		}
		if (!panel.ShouldSendCompatibilityMouseEvents(this) && ((IPointerEventInternal)this).triggeredByOS)
		{
			(panel as BaseVisualElementPanel)?.CommitElementUnderPointers();
		}
		base.PostDispatch(panel);
	}

	protected PointerEventBase()
	{
		LocalInit();
	}
}
