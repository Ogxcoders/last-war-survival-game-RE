using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LS.UnityEngine.UI;

[AddComponentMenu("UI/SpecialButton", 55)]
public class SpecialButton : Selectable
{
	[Serializable]
	public class ButtonClickDownEvent : UnityEvent
	{
	}

	[Serializable]
	public class ButtonClickUpEvent : UnityEvent
	{
	}

	[FormerlySerializedAs("onClickDown")]
	[SerializeField]
	private ButtonClickDownEvent m_OnClickDown = new ButtonClickDownEvent();

	[FormerlySerializedAs("onClickUp")]
	[SerializeField]
	private ButtonClickUpEvent m_OnClickUp = new ButtonClickUpEvent();

	[SerializeField]
	private bool m_DoubleEffect = true;

	private void TriggerAnimation(string triggername)
	{
		if (base.transition == Transition.Animation && !(base.animator == null) && base.animator.isActiveAndEnabled && base.animator.hasBoundPlayables && !string.IsNullOrEmpty(triggername))
		{
			base.animator.ResetTrigger(base.animationTriggers.normalTrigger);
			base.animator.ResetTrigger(base.animationTriggers.pressedTrigger);
			base.animator.ResetTrigger(base.animationTriggers.highlightedTrigger);
			base.animator.ResetTrigger(base.animationTriggers.disabledTrigger);
			base.animator.SetTrigger(triggername);
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		m_OnClickDown.Invoke();
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		m_OnClickUp.Invoke();
	}
}
