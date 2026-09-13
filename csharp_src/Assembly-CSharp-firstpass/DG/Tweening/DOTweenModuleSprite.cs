using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening;

public static class DOTweenModuleSprite
{
	public static TweenerCore<Color, Color, ColorOptions> DOColor(this SpriteRenderer target, Color endValue, float duration)
	{
		TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
		{
			target.color = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Color, Color, ColorOptions> DOFade(this SpriteRenderer target, float endValue, float duration)
	{
		TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.color, delegate(Color x)
		{
			target.color = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static Sequence DOGradientColor(this SpriteRenderer target, Gradient gradient, float duration)
	{
		Sequence sequence = DOTween.Sequence();
		GradientColorKey[] colorKeys = gradient.colorKeys;
		int num = colorKeys.Length;
		for (int i = 0; i < num; i++)
		{
			GradientColorKey gradientColorKey = colorKeys[i];
			if (i == 0 && gradientColorKey.time <= 0f)
			{
				target.color = gradientColorKey.color;
				continue;
			}
			float duration2 = ((i == num - 1) ? (duration - sequence.Duration(includeLoops: false)) : (duration * ((i == 0) ? gradientColorKey.time : (gradientColorKey.time - colorKeys[i - 1].time))));
			sequence.Append(target.DOColor(gradientColorKey.color, duration2).SetEase(Ease.Linear));
		}
		sequence.SetTarget(target);
		return sequence;
	}

	public static Tweener DOBlendableColor(this SpriteRenderer target, Color endValue, float duration)
	{
		endValue -= target.color;
		Color to = new Color(0f, 0f, 0f, 0f);
		return DOTween.To(() => to, delegate(Color x)
		{
			Color color = x - to;
			to = x;
			target.color += color;
		}, endValue, duration).Blendable().SetTarget(target);
	}
}
