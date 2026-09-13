namespace UnityEngine.UIElements;

public class ContextualMenuPopulateEvent : MouseEventBase<ContextualMenuPopulateEvent>
{
	private ContextualMenuManager m_ContextualMenuManager;

	public DropdownMenu menu { get; private set; }

	public EventBase triggerEvent { get; private set; }

	public static ContextualMenuPopulateEvent GetPooled(EventBase triggerEvent, DropdownMenu menu, IEventHandler target, ContextualMenuManager menuManager)
	{
		ContextualMenuPopulateEvent contextualMenuPopulateEvent = EventBase<ContextualMenuPopulateEvent>.GetPooled(triggerEvent);
		if (triggerEvent != null)
		{
			triggerEvent.Acquire();
			contextualMenuPopulateEvent.triggerEvent = triggerEvent;
			if (triggerEvent is IMouseEvent mouseEvent)
			{
				contextualMenuPopulateEvent.modifiers = mouseEvent.modifiers;
				contextualMenuPopulateEvent.mousePosition = mouseEvent.mousePosition;
				contextualMenuPopulateEvent.localMousePosition = mouseEvent.mousePosition;
				contextualMenuPopulateEvent.mouseDelta = mouseEvent.mouseDelta;
				contextualMenuPopulateEvent.button = mouseEvent.button;
				contextualMenuPopulateEvent.clickCount = mouseEvent.clickCount;
			}
			if (triggerEvent is IMouseEventInternal mouseEventInternal)
			{
				((IMouseEventInternal)contextualMenuPopulateEvent).triggeredByOS = mouseEventInternal.triggeredByOS;
			}
		}
		contextualMenuPopulateEvent.target = target;
		contextualMenuPopulateEvent.menu = menu;
		contextualMenuPopulateEvent.m_ContextualMenuManager = menuManager;
		return contextualMenuPopulateEvent;
	}

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		menu = null;
		m_ContextualMenuManager = null;
		if (triggerEvent != null)
		{
			triggerEvent.Dispose();
			triggerEvent = null;
		}
	}

	public ContextualMenuPopulateEvent()
	{
		LocalInit();
	}

	protected internal override void PostDispatch(IPanel panel)
	{
		if (!base.isDefaultPrevented && m_ContextualMenuManager != null)
		{
			menu.PrepareForDisplay(triggerEvent);
			m_ContextualMenuManager.DoDisplayMenu(menu, triggerEvent);
		}
		base.PostDispatch(panel);
	}
}
