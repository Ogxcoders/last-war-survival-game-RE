using DG.Tweening;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace LS.UnityEngine.UI;

[AddComponentMenu("UI/NewButton", 31)]
public class NewButton : Button
{
	[Tooltip("需使用“SpriteGray”材质球")]
	[SerializeField]
	private Material m_GrayMaterial;

	[SerializeField]
	private bool m_UseGrayMaterial;

	[SerializeField]
	private bool m_DoubleEffect = true;

	private Sequence _btnSeq;

	private void SetGrayMaterial(SelectionState state)
	{
		if (base.targetGraphic == null)
		{
			Log.Warning("button target graphic is none !");
		}
		else if (m_GrayMaterial == null)
		{
			Log.Warning("button gray material is none !");
		}
		else if (state == SelectionState.Disabled)
		{
			if (base.interactable)
			{
				base.targetGraphic.material = null;
			}
			else
			{
				base.targetGraphic.material = m_GrayMaterial;
			}
		}
		else if (m_UseGrayMaterial && "SpriteGray" == base.targetGraphic.material.name)
		{
			base.targetGraphic.material = null;
		}
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		if (m_UseGrayMaterial)
		{
			SetGrayMaterial(state);
		}
		Color color;
		Sprite newSprite;
		string triggername;
		switch (state)
		{
		case SelectionState.Normal:
			color = base.colors.normalColor;
			newSprite = null;
			triggername = base.animationTriggers.normalTrigger;
			break;
		case SelectionState.Highlighted:
			color = base.colors.highlightedColor;
			newSprite = base.spriteState.highlightedSprite;
			triggername = base.animationTriggers.highlightedTrigger;
			break;
		case SelectionState.Pressed:
			color = base.colors.pressedColor;
			newSprite = base.spriteState.pressedSprite;
			triggername = base.animationTriggers.pressedTrigger;
			break;
		case SelectionState.Disabled:
			color = base.colors.disabledColor;
			newSprite = base.spriteState.disabledSprite;
			triggername = base.animationTriggers.disabledTrigger;
			break;
		case SelectionState.Selected:
			color = base.colors.selectedColor;
			newSprite = base.spriteState.selectedSprite;
			triggername = base.animationTriggers.selectedTrigger;
			break;
		default:
			color = Color.black;
			newSprite = null;
			triggername = string.Empty;
			break;
		}
		if (base.gameObject.activeInHierarchy)
		{
			switch (base.transition)
			{
			case Transition.ColorTint:
				StartColorTween(color * base.colors.colorMultiplier, instant);
				break;
			case Transition.SpriteSwap:
				DoSpriteSwap(newSprite);
				break;
			case Transition.Animation:
				TriggerAnimation(triggername);
				break;
			}
		}
	}

	private void StartColorTween(Color targetColor, bool instant)
	{
		if (!(base.targetGraphic == null))
		{
			base.targetGraphic.CrossFadeColor(targetColor, (!instant) ? base.colors.fadeDuration : 0f, ignoreTimeScale: true, useAlpha: true);
		}
	}

	private void DoSpriteSwap(Sprite newSprite)
	{
		if (!(base.image == null))
		{
			base.image.overrideSprite = newSprite;
		}
	}

	private void TriggerAnimation(string triggername)
	{
		if (base.transition == Transition.Animation && !string.IsNullOrEmpty(triggername))
		{
			_btnSeq?.Kill();
			if (triggername == "Pressed")
			{
				_btnSeq = DOTween.Sequence();
				_btnSeq.Append(base.gameObject.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.1f));
				_btnSeq.OnKill(OnAnimationKill);
			}
			else
			{
				_btnSeq = DOTween.Sequence();
				_btnSeq.Append(base.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
				_btnSeq.OnKill(OnAnimationKill);
			}
		}
	}

	private void OnAnimationKill()
	{
		_btnSeq = null;
	}

	protected override void OnDestroy()
	{
		_btnSeq?.Kill();
		_btnSeq = null;
		base.OnDestroy();
	}
}
