using System;

namespace UnityEngine.UIElements;

public abstract class Focusable : CallbackEventHandler
{
	private bool m_DelegatesFocus;

	private bool m_ExcludeFromFocusRing;

	internal bool isIMGUIContainer = false;

	public abstract FocusController focusController { get; }

	public bool focusable { get; set; }

	public int tabIndex { get; set; }

	public bool delegatesFocus
	{
		get
		{
			return m_DelegatesFocus;
		}
		set
		{
			if (!((VisualElement)this).isCompositeRoot)
			{
				throw new InvalidOperationException("delegatesFocus should only be set on composite roots.");
			}
			m_DelegatesFocus = value;
		}
	}

	internal bool excludeFromFocusRing
	{
		get
		{
			return m_ExcludeFromFocusRing;
		}
		set
		{
			if (!((VisualElement)this).isCompositeRoot)
			{
				throw new InvalidOperationException("excludeFromFocusRing should only be set on composite roots.");
			}
			m_ExcludeFromFocusRing = value;
		}
	}

	public virtual bool canGrabFocus => focusable;

	protected Focusable()
	{
		focusable = true;
		tabIndex = 0;
	}

	public virtual void Focus()
	{
		if (focusController != null)
		{
			if (canGrabFocus)
			{
				Focusable focusDelegate = GetFocusDelegate();
				focusController.SwitchFocus(focusDelegate);
			}
			else
			{
				focusController.SwitchFocus(null);
			}
		}
	}

	public virtual void Blur()
	{
		if (focusController != null && focusController.IsFocused(this))
		{
			focusController.SwitchFocus(null);
		}
	}

	private Focusable GetFocusDelegate()
	{
		Focusable focusable = this;
		while (focusable != null && focusable.delegatesFocus)
		{
			focusable = GetFirstFocusableChild(focusable as VisualElement);
		}
		return focusable;
	}

	private static Focusable GetFirstFocusableChild(VisualElement ve)
	{
		foreach (VisualElement item in ve.hierarchy.Children())
		{
			if (item.canGrabFocus)
			{
				return item;
			}
			bool flag = item.hierarchy.parent != null && item == item.hierarchy.parent.contentContainer;
			if (!item.isCompositeRoot && !flag)
			{
				Focusable firstFocusableChild = GetFirstFocusableChild(item);
				if (firstFocusableChild != null)
				{
					return firstFocusableChild;
				}
			}
		}
		return null;
	}

	protected override void ExecuteDefaultAction(EventBase evt)
	{
		base.ExecuteDefaultAction(evt);
		if (evt == null || evt.target != evt.leafTarget)
		{
			return;
		}
		if (evt.eventTypeId == EventBase<MouseDownEvent>.TypeId())
		{
			Focus();
			if (canGrabFocus)
			{
				evt.doNotSendToRootIMGUIContainer = true;
			}
		}
		focusController?.SwitchFocusOnEvent(evt);
	}
}
