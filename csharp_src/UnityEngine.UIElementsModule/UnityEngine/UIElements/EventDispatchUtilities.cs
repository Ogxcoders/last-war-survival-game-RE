#define UNITY_ASSERTIONS
namespace UnityEngine.UIElements;

internal static class EventDispatchUtilities
{
	public static void PropagateEvent(EventBase evt)
	{
		Debug.Assert(!evt.dispatch, "Event is being dispatched recursively.");
		evt.dispatch = true;
		if (evt.path == null)
		{
			(evt.target as CallbackEventHandler)?.HandleEventAtTargetPhase(evt);
		}
		else
		{
			if (evt.tricklesDown)
			{
				evt.propagationPhase = PropagationPhase.TrickleDown;
				int num = evt.path.trickleDownPath.Count - 1;
				while (num >= 0 && !evt.isPropagationStopped)
				{
					if (!evt.Skip(evt.path.trickleDownPath[num]))
					{
						evt.currentTarget = evt.path.trickleDownPath[num];
						evt.currentTarget.HandleEvent(evt);
					}
					num--;
				}
			}
			evt.propagationPhase = PropagationPhase.AtTarget;
			foreach (VisualElement targetElement in evt.path.targetElements)
			{
				if (!evt.Skip(targetElement))
				{
					evt.target = targetElement;
					evt.currentTarget = evt.target;
					evt.currentTarget.HandleEvent(evt);
				}
			}
			evt.propagationPhase = PropagationPhase.DefaultActionAtTarget;
			foreach (VisualElement targetElement2 in evt.path.targetElements)
			{
				if (!evt.Skip(targetElement2))
				{
					evt.target = targetElement2;
					evt.currentTarget = evt.target;
					evt.currentTarget.HandleEvent(evt);
				}
			}
			evt.target = evt.leafTarget;
			if (evt.bubbles)
			{
				evt.propagationPhase = PropagationPhase.BubbleUp;
				foreach (VisualElement item in evt.path.bubbleUpPath)
				{
					if (!evt.Skip(item))
					{
						evt.currentTarget = item;
						evt.currentTarget.HandleEvent(evt);
					}
				}
			}
		}
		evt.dispatch = false;
		evt.propagationPhase = PropagationPhase.None;
		evt.currentTarget = null;
	}

	internal static void PropagateToIMGUIContainer(VisualElement root, EventBase evt)
	{
		if (evt.imguiEvent == null || root.elementPanel.contextType == ContextType.Player)
		{
			return;
		}
		if (root.isIMGUIContainer)
		{
			IMGUIContainer iMGUIContainer = root as IMGUIContainer;
			if (evt.Skip(iMGUIContainer))
			{
				return;
			}
			bool flag = (evt.target as Focusable)?.focusable ?? false;
			if (iMGUIContainer.SendEventToIMGUI(evt, !flag))
			{
				evt.StopPropagation();
				evt.PreventDefault();
			}
			if (evt.imguiEvent.rawType == EventType.Used)
			{
				Debug.Assert(evt.isPropagationStopped);
			}
		}
		if (root.imguiContainerDescendantCount <= 0)
		{
			return;
		}
		int childCount = root.hierarchy.childCount;
		for (int i = 0; i < childCount; i++)
		{
			PropagateToIMGUIContainer(root.hierarchy[i], evt);
			if (evt.isPropagationStopped)
			{
				break;
			}
		}
	}

	public static void ExecuteDefaultAction(EventBase evt, IPanel panel)
	{
		if (evt.target == null && panel != null)
		{
			evt.target = panel.visualTree;
		}
		if (evt.target != null)
		{
			evt.dispatch = true;
			evt.currentTarget = evt.target;
			evt.propagationPhase = PropagationPhase.DefaultAction;
			evt.currentTarget.HandleEvent(evt);
			evt.propagationPhase = PropagationPhase.None;
			evt.currentTarget = null;
			evt.dispatch = false;
		}
	}
}
