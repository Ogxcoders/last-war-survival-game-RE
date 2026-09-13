#define UNITY_ASSERTIONS
namespace UnityEngine.UIElements;

public abstract class MouseEventBase<T> : EventBase<T>, IMouseEvent, IMouseEventInternal where T : MouseEventBase<T>, new()
{
	public EventModifiers modifiers { get; protected set; }

	public Vector2 mousePosition { get; protected set; }

	public Vector2 localMousePosition { get; internal set; }

	public Vector2 mouseDelta { get; protected set; }

	public int clickCount { get; protected set; }

	public int button { get; protected set; }

	public int pressedButtons { get; protected set; }

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

	bool IMouseEventInternal.triggeredByOS { get; set; }

	bool IMouseEventInternal.recomputeTopElementUnderMouse { get; set; }

	IPointerEvent IMouseEventInternal.sourcePointerEvent { get; set; }

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
				localMousePosition = ele.WorldToLocal(mousePosition);
			}
			else
			{
				localMousePosition = mousePosition;
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
		modifiers = EventModifiers.None;
		mousePosition = Vector2.zero;
		localMousePosition = Vector2.zero;
		mouseDelta = Vector2.zero;
		clickCount = 0;
		button = 0;
		pressedButtons = 0;
		((IMouseEventInternal)this).triggeredByOS = false;
		((IMouseEventInternal)this).recomputeTopElementUnderMouse = true;
		((IMouseEventInternal)this).sourcePointerEvent = null;
	}

	protected internal override void PreDispatch(IPanel panel)
	{
		base.PreDispatch(panel);
		if (((IMouseEventInternal)this).triggeredByOS)
		{
			PointerDeviceState.SavePointerPosition(PointerId.mousePointerId, mousePosition, panel);
		}
	}

	protected internal override void PostDispatch(IPanel panel)
	{
		if (((IMouseEventInternal)this).sourcePointerEvent is EventBase eventBase)
		{
			Debug.Assert(eventBase.processed);
			(panel as BaseVisualElementPanel)?.CommitElementUnderPointers();
			if (base.isPropagationStopped)
			{
				eventBase.StopPropagation();
			}
			if (base.isImmediatePropagationStopped)
			{
				eventBase.StopImmediatePropagation();
			}
			if (base.isDefaultPrevented)
			{
				eventBase.PreventDefault();
			}
		}
		base.PostDispatch(panel);
	}

	public static T GetPooled(Event systemEvent)
	{
		T val = EventBase<T>.GetPooled();
		val.imguiEvent = systemEvent;
		if (systemEvent != null)
		{
			val.modifiers = systemEvent.modifiers;
			val.mousePosition = systemEvent.mousePosition;
			val.localMousePosition = systemEvent.mousePosition;
			val.mouseDelta = systemEvent.delta;
			val.button = systemEvent.button;
			val.pressedButtons = PointerDeviceState.GetPressedButtons(PointerId.mousePointerId);
			val.clickCount = systemEvent.clickCount;
			((IMouseEventInternal)val).triggeredByOS = true;
			((IMouseEventInternal)val).recomputeTopElementUnderMouse = true;
		}
		return val;
	}

	public static T GetPooled(Vector2 position, int button, int clickCount, Vector2 delta, EventModifiers modifiers = EventModifiers.None)
	{
		return GetPooled(position, button, clickCount, delta, modifiers, fromOS: false);
	}

	internal static T GetPooled(Vector2 position, int button, int clickCount, Vector2 delta, EventModifiers modifiers, bool fromOS)
	{
		T val = EventBase<T>.GetPooled();
		val.modifiers = modifiers;
		val.mousePosition = position;
		val.localMousePosition = position;
		val.mouseDelta = delta;
		val.button = button;
		val.pressedButtons = PointerDeviceState.GetPressedButtons(PointerId.mousePointerId);
		val.clickCount = clickCount;
		((IMouseEventInternal)val).triggeredByOS = fromOS;
		((IMouseEventInternal)val).recomputeTopElementUnderMouse = true;
		return val;
	}

	internal static T GetPooled(IMouseEvent triggerEvent, Vector2 mousePosition, bool recomputeTopElementUnderMouse)
	{
		if (triggerEvent != null)
		{
			return GetPooled(triggerEvent);
		}
		T val = EventBase<T>.GetPooled();
		val.mousePosition = mousePosition;
		val.localMousePosition = mousePosition;
		((IMouseEventInternal)val).recomputeTopElementUnderMouse = recomputeTopElementUnderMouse;
		return val;
	}

	public static T GetPooled(IMouseEvent triggerEvent)
	{
		T val = EventBase<T>.GetPooled(triggerEvent as EventBase);
		if (triggerEvent != null)
		{
			val.modifiers = triggerEvent.modifiers;
			val.mousePosition = triggerEvent.mousePosition;
			val.localMousePosition = triggerEvent.mousePosition;
			val.mouseDelta = triggerEvent.mouseDelta;
			val.button = triggerEvent.button;
			val.pressedButtons = triggerEvent.pressedButtons;
			val.clickCount = triggerEvent.clickCount;
			if (triggerEvent is IMouseEventInternal mouseEventInternal)
			{
				((IMouseEventInternal)val).triggeredByOS = mouseEventInternal.triggeredByOS;
				((IMouseEventInternal)val).recomputeTopElementUnderMouse = false;
			}
		}
		return val;
	}

	protected static T GetPooled(IPointerEvent pointerEvent)
	{
		T val = EventBase<T>.GetPooled();
		val.target = (pointerEvent as EventBase)?.target;
		val.imguiEvent = (pointerEvent as EventBase)?.imguiEvent;
		if ((pointerEvent as EventBase)?.path != null)
		{
			val.path = (pointerEvent as EventBase).path;
		}
		val.modifiers = pointerEvent.modifiers;
		val.mousePosition = pointerEvent.position;
		val.localMousePosition = pointerEvent.position;
		val.mouseDelta = pointerEvent.deltaPosition;
		val.button = ((pointerEvent.button != -1) ? pointerEvent.button : 0);
		val.pressedButtons = pointerEvent.pressedButtons;
		val.clickCount = pointerEvent.clickCount;
		if (pointerEvent is IPointerEventInternal pointerEventInternal)
		{
			((IMouseEventInternal)val).triggeredByOS = pointerEventInternal.triggeredByOS;
			((IMouseEventInternal)val).recomputeTopElementUnderMouse = true;
			((IMouseEventInternal)val).sourcePointerEvent = pointerEvent;
		}
		return val;
	}

	protected MouseEventBase()
	{
		LocalInit();
	}
}
