using System;

namespace UnityEngine.UIElements;

public class ContextualMenuManipulator : MouseManipulator
{
	private Action<ContextualMenuPopulateEvent> m_MenuBuilder;

	public ContextualMenuManipulator(Action<ContextualMenuPopulateEvent> menuBuilder)
	{
		m_MenuBuilder = menuBuilder;
		base.activators.Add(new ManipulatorActivationFilter
		{
			button = MouseButton.RightMouse
		});
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
		{
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse,
				modifiers = EventModifiers.Control
			});
		}
	}

	protected override void RegisterCallbacksOnTarget()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
		{
			base.target.RegisterCallback<MouseDownEvent>(OnMouseUpDownEvent);
		}
		else
		{
			base.target.RegisterCallback<MouseUpEvent>(OnMouseUpDownEvent);
		}
		base.target.RegisterCallback<KeyUpEvent>(OnKeyUpEvent);
		base.target.RegisterCallback<ContextualMenuPopulateEvent>(OnContextualMenuEvent);
	}

	protected override void UnregisterCallbacksFromTarget()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
		{
			base.target.UnregisterCallback<MouseDownEvent>(OnMouseUpDownEvent);
		}
		else
		{
			base.target.UnregisterCallback<MouseUpEvent>(OnMouseUpDownEvent);
		}
		base.target.UnregisterCallback<KeyUpEvent>(OnKeyUpEvent);
		base.target.UnregisterCallback<ContextualMenuPopulateEvent>(OnContextualMenuEvent);
	}

	private void OnMouseUpDownEvent(IMouseEvent evt)
	{
		if (CanStartManipulation(evt) && base.target.elementPanel != null && base.target.elementPanel.contextualMenuManager != null)
		{
			EventBase eventBase = evt as EventBase;
			base.target.elementPanel.contextualMenuManager.DisplayMenu(eventBase, base.target);
			eventBase.StopPropagation();
			eventBase.PreventDefault();
		}
	}

	private void OnKeyUpEvent(KeyUpEvent evt)
	{
		if (evt.keyCode == KeyCode.Menu && base.target.elementPanel != null && base.target.elementPanel.contextualMenuManager != null)
		{
			base.target.elementPanel.contextualMenuManager.DisplayMenu(evt, base.target);
			evt.StopPropagation();
			evt.PreventDefault();
		}
	}

	private void OnContextualMenuEvent(ContextualMenuPopulateEvent evt)
	{
		if (m_MenuBuilder != null)
		{
			m_MenuBuilder(evt);
		}
	}
}
