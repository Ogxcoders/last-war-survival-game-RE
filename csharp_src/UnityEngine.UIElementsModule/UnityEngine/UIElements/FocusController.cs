using System.Collections.Generic;

namespace UnityEngine.UIElements;

public class FocusController
{
	private struct FocusedElement
	{
		public VisualElement m_SubTreeRoot;

		public Focusable m_FocusedElement;
	}

	private List<FocusedElement> m_FocusedElements = new List<FocusedElement>();

	private IFocusRing focusRing { get; }

	public Focusable focusedElement => GetRetargetedFocusedElement(null);

	internal int imguiKeyboardControl { get; set; }

	public FocusController(IFocusRing focusRing)
	{
		this.focusRing = focusRing;
		imguiKeyboardControl = 0;
	}

	internal bool IsFocused(Focusable f)
	{
		foreach (FocusedElement focusedElement in m_FocusedElements)
		{
			if (focusedElement.m_FocusedElement == f)
			{
				return true;
			}
		}
		return false;
	}

	internal Focusable GetRetargetedFocusedElement(VisualElement retargetAgainst)
	{
		VisualElement visualElement = retargetAgainst?.hierarchy.parent;
		if (visualElement == null)
		{
			if (m_FocusedElements.Count > 0)
			{
				return m_FocusedElements[m_FocusedElements.Count - 1].m_FocusedElement;
			}
		}
		else
		{
			while (!visualElement.isCompositeRoot && visualElement.hierarchy.parent != null)
			{
				visualElement = visualElement.hierarchy.parent;
			}
			foreach (FocusedElement focusedElement in m_FocusedElements)
			{
				if (focusedElement.m_SubTreeRoot == visualElement)
				{
					return focusedElement.m_FocusedElement;
				}
			}
		}
		return null;
	}

	internal Focusable GetLeafFocusedElement()
	{
		if (m_FocusedElements.Count > 0)
		{
			return m_FocusedElements[0].m_FocusedElement;
		}
		return null;
	}

	internal void DoFocusChange(Focusable f)
	{
		m_FocusedElements.Clear();
		for (VisualElement visualElement = f as VisualElement; visualElement != null; visualElement = visualElement.hierarchy.parent)
		{
			if (visualElement.hierarchy.parent == null || visualElement.isCompositeRoot)
			{
				m_FocusedElements.Add(new FocusedElement
				{
					m_SubTreeRoot = visualElement,
					m_FocusedElement = f
				});
				f = visualElement;
			}
		}
	}

	private void AboutToReleaseFocus(Focusable focusable, Focusable willGiveFocusTo, FocusChangeDirection direction)
	{
		using FocusOutEvent e = FocusEventBase<FocusOutEvent>.GetPooled(focusable, willGiveFocusTo, direction, this);
		focusable.SendEvent(e);
	}

	private void ReleaseFocus(Focusable focusable, Focusable willGiveFocusTo, FocusChangeDirection direction)
	{
		using BlurEvent e = FocusEventBase<BlurEvent>.GetPooled(focusable, willGiveFocusTo, direction, this);
		focusable.SendEvent(e);
	}

	private void AboutToGrabFocus(Focusable focusable, Focusable willTakeFocusFrom, FocusChangeDirection direction)
	{
		using FocusInEvent e = FocusEventBase<FocusInEvent>.GetPooled(focusable, willTakeFocusFrom, direction, this);
		focusable.SendEvent(e);
	}

	private void GrabFocus(Focusable focusable, Focusable willTakeFocusFrom, FocusChangeDirection direction)
	{
		using FocusEvent e = FocusEventBase<FocusEvent>.GetPooled(focusable, willTakeFocusFrom, direction, this);
		focusable.SendEvent(e);
	}

	internal void SwitchFocus(Focusable newFocusedElement)
	{
		SwitchFocus(newFocusedElement, FocusChangeDirection.unspecified);
	}

	private void SwitchFocus(Focusable newFocusedElement, FocusChangeDirection direction)
	{
		if (GetLeafFocusedElement() == newFocusedElement)
		{
			return;
		}
		Focusable leafFocusedElement = GetLeafFocusedElement();
		if (newFocusedElement == null || !newFocusedElement.canGrabFocus)
		{
			if (leafFocusedElement != null)
			{
				AboutToReleaseFocus(leafFocusedElement, null, direction);
				ReleaseFocus(leafFocusedElement, null, direction);
			}
		}
		else if (newFocusedElement != leafFocusedElement)
		{
			VisualElement willGiveFocusTo = (newFocusedElement as VisualElement)?.RetargetElement(leafFocusedElement as VisualElement);
			VisualElement willTakeFocusFrom = (leafFocusedElement as VisualElement)?.RetargetElement(newFocusedElement as VisualElement);
			if (leafFocusedElement != null)
			{
				AboutToReleaseFocus(leafFocusedElement, willGiveFocusTo, direction);
			}
			AboutToGrabFocus(newFocusedElement, willTakeFocusFrom, direction);
			if (leafFocusedElement != null)
			{
				ReleaseFocus(leafFocusedElement, willGiveFocusTo, direction);
			}
			GrabFocus(newFocusedElement, willTakeFocusFrom, direction);
		}
	}

	internal Focusable SwitchFocusOnEvent(EventBase e)
	{
		FocusChangeDirection focusChangeDirection = focusRing.GetFocusChangeDirection(GetLeafFocusedElement(), e);
		if (focusChangeDirection != FocusChangeDirection.none)
		{
			Focusable nextFocusable = focusRing.GetNextFocusable(GetLeafFocusedElement(), focusChangeDirection);
			SwitchFocus(nextFocusable, focusChangeDirection);
			return nextFocusable;
		}
		return GetLeafFocusedElement();
	}

	internal void SyncIMGUIFocus(int imguiKeyboardControlID, Focusable imguiContainerHavingKeyboardControl, bool forceSwitch)
	{
		imguiKeyboardControl = imguiKeyboardControlID;
		if (forceSwitch || imguiKeyboardControl != 0)
		{
			SwitchFocus(imguiContainerHavingKeyboardControl, FocusChangeDirection.unspecified);
		}
		else
		{
			SwitchFocus(null, FocusChangeDirection.unspecified);
		}
	}
}
