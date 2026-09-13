using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.CustomPlugins;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening;

public static class ShortcutExtensions
{
	public static TweenerCore<float, float, FloatOptions> DOAspect(this Camera target, float endValue, float duration)
	{
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.aspect, delegate(float x)
		{
			target.aspect = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Color, Color, ColorOptions> DOColor(this Camera target, Color endValue, float duration)
	{
		TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.backgroundColor, delegate(Color x)
		{
			target.backgroundColor = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<float, float, FloatOptions> DOFarClipPlane(this Camera target, float endValue, float duration)
	{
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.farClipPlane, delegate(float x)
		{
			target.farClipPlane = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<float, float, FloatOptions> DOFieldOfView(this Camera target, float endValue, float duration)
	{
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.fieldOfView, delegate(float x)
		{
			target.fieldOfView = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<float, float, FloatOptions> DONearClipPlane(this Camera target, float endValue, float duration)
	{
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.nearClipPlane, delegate(float x)
		{
			target.nearClipPlane = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<float, float, FloatOptions> DOOrthoSize(this Camera target, float endValue, float duration)
	{
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.orthographicSize, delegate(float x)
		{
			target.orthographicSize = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Rect, Rect, RectOptions> DOPixelRect(this Camera target, Rect endValue, float duration)
	{
		TweenerCore<Rect, Rect, RectOptions> tweenerCore = DOTween.To(() => target.pixelRect, delegate(Rect x)
		{
			target.pixelRect = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Rect, Rect, RectOptions> DORect(this Camera target, Rect endValue, float duration)
	{
		TweenerCore<Rect, Rect, RectOptions> tweenerCore = DOTween.To(() => target.rect, delegate(Rect x)
		{
			target.rect = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static Tweener DOShakePosition(this Camera target, float duration, float strength = 3f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => target.transform.localPosition, delegate(Vector3 x)
		{
			target.transform.localPosition = x;
		}, duration, strength, vibrato, randomness, ignoreZAxis: true, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetCameraShakePosition);
	}

	public static Tweener DOShakePosition(this Camera target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => target.transform.localPosition, delegate(Vector3 x)
		{
			target.transform.localPosition = x;
		}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetCameraShakePosition);
	}

	public static Tweener DOShakeRotation(this Camera target, float duration, float strength = 90f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => target.transform.localEulerAngles, delegate(Vector3 x)
		{
			target.transform.localRotation = Quaternion.Euler(x);
		}, duration, strength, vibrato, randomness, ignoreZAxis: false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
	}

	public static Tweener DOShakeRotation(this Camera target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => target.transform.localEulerAngles, delegate(Vector3 x)
		{
			target.transform.localRotation = Quaternion.Euler(x);
		}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
	}

	public static TweenerCore<Color, Color, ColorOptions> DOColor(this Light target, Color endValue, float duration)
	{
		TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
		{
			target.color = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<float, float, FloatOptions> DOIntensity(this Light target, float endValue, float duration)
	{
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.intensity, delegate(float x)
		{
			target.intensity = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<float, float, FloatOptions> DOShadowStrength(this Light target, float endValue, float duration)
	{
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.shadowStrength, delegate(float x)
		{
			target.shadowStrength = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static Tweener DOColor(this LineRenderer target, Color2 startValue, Color2 endValue, float duration)
	{
		return DOTween.To(() => startValue, delegate(Color2 x)
		{
			target.SetColors(x.ca, x.cb);
		}, endValue, duration).SetTarget(target);
	}

	public static TweenerCore<Color, Color, ColorOptions> DOColor(this Material target, Color endValue, float duration)
	{
		TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
		{
			target.color = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Color, Color, ColorOptions> DOColor(this Material target, Color endValue, string property, float duration)
	{
		if (!target.HasProperty(property))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(property);
			}
			return null;
		}
		TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.GetColor(property), delegate(Color x)
		{
			target.SetColor(property, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Color, Color, ColorOptions> DOColor(this Material target, Color endValue, int propertyID, float duration)
	{
		if (!target.HasProperty(propertyID))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(propertyID);
			}
			return null;
		}
		TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.GetColor(propertyID), delegate(Color x)
		{
			target.SetColor(propertyID, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Color, Color, ColorOptions> DOFade(this Material target, float endValue, float duration)
	{
		TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.color, delegate(Color x)
		{
			target.color = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Color, Color, ColorOptions> DOFade(this Material target, float endValue, string property, float duration)
	{
		if (!target.HasProperty(property))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(property);
			}
			return null;
		}
		TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.GetColor(property), delegate(Color x)
		{
			target.SetColor(property, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Color, Color, ColorOptions> DOFade(this Material target, float endValue, int propertyID, float duration)
	{
		if (!target.HasProperty(propertyID))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(propertyID);
			}
			return null;
		}
		TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.GetColor(propertyID), delegate(Color x)
		{
			target.SetColor(propertyID, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<float, float, FloatOptions> DOFloat(this Material target, float endValue, string property, float duration)
	{
		if (!target.HasProperty(property))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(property);
			}
			return null;
		}
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.GetFloat(property), delegate(float x)
		{
			target.SetFloat(property, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<float, float, FloatOptions> DOFloat(this Material target, float endValue, int propertyID, float duration)
	{
		if (!target.HasProperty(propertyID))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(propertyID);
			}
			return null;
		}
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.GetFloat(propertyID), delegate(float x)
		{
			target.SetFloat(propertyID, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector2, Vector2, VectorOptions> DOOffset(this Material target, Vector2 endValue, float duration)
	{
		TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.mainTextureOffset, delegate(Vector2 x)
		{
			target.mainTextureOffset = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector2, Vector2, VectorOptions> DOOffset(this Material target, Vector2 endValue, string property, float duration)
	{
		if (!target.HasProperty(property))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(property);
			}
			return null;
		}
		TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.GetTextureOffset(property), delegate(Vector2 x)
		{
			target.SetTextureOffset(property, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector2, Vector2, VectorOptions> DOTiling(this Material target, Vector2 endValue, float duration)
	{
		TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.mainTextureScale, delegate(Vector2 x)
		{
			target.mainTextureScale = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector2, Vector2, VectorOptions> DOTiling(this Material target, Vector2 endValue, string property, float duration)
	{
		if (!target.HasProperty(property))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(property);
			}
			return null;
		}
		TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.GetTextureScale(property), delegate(Vector2 x)
		{
			target.SetTextureScale(property, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector4, Vector4, VectorOptions> DOVector(this Material target, Vector4 endValue, string property, float duration)
	{
		if (!target.HasProperty(property))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(property);
			}
			return null;
		}
		TweenerCore<Vector4, Vector4, VectorOptions> tweenerCore = DOTween.To(() => target.GetVector(property), delegate(Vector4 x)
		{
			target.SetVector(property, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector4, Vector4, VectorOptions> DOVector(this Material target, Vector4 endValue, int propertyID, float duration)
	{
		if (!target.HasProperty(propertyID))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(propertyID);
			}
			return null;
		}
		TweenerCore<Vector4, Vector4, VectorOptions> tweenerCore = DOTween.To(() => target.GetVector(propertyID), delegate(Vector4 x)
		{
			target.SetVector(propertyID, x);
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static Tweener DOResize(this TrailRenderer target, float toStartWidth, float toEndWidth, float duration)
	{
		return DOTween.To(() => new Vector2(target.startWidth, target.endWidth), delegate(Vector2 x)
		{
			target.startWidth = x.x;
			target.endWidth = x.y;
		}, new Vector2(toStartWidth, toEndWidth), duration).SetTarget(target);
	}

	public static TweenerCore<float, float, FloatOptions> DOTime(this TrailRenderer target, float endValue, float duration)
	{
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.time, delegate(float x)
		{
			target.time = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(this Transform target, Vector3 endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, endValue, duration);
		tweenerCore.SetOptions(snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveX(this Transform target, float endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, new Vector3(endValue, 0f, 0f), duration);
		tweenerCore.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveY(this Transform target, float endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, new Vector3(0f, endValue, 0f), duration);
		tweenerCore.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveZ(this Transform target, float endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, new Vector3(0f, 0f, endValue), duration);
		tweenerCore.SetOptions(AxisConstraint.Z, snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMove(this Transform target, Vector3 endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, endValue, duration);
		tweenerCore.SetOptions(snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveX(this Transform target, float endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, new Vector3(endValue, 0f, 0f), duration);
		tweenerCore.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveY(this Transform target, float endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, new Vector3(0f, endValue, 0f), duration);
		tweenerCore.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveZ(this Transform target, float endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, new Vector3(0f, 0f, endValue), duration);
		tweenerCore.SetOptions(AxisConstraint.Z, snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DORotate(this Transform target, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
	{
		TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => target.rotation, delegate(Quaternion x)
		{
			target.rotation = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		tweenerCore.plugOptions.rotateMode = mode;
		return tweenerCore;
	}

	public static TweenerCore<Quaternion, Quaternion, NoOptions> DORotateQuaternion(this Transform target, Quaternion endValue, float duration)
	{
		TweenerCore<Quaternion, Quaternion, NoOptions> tweenerCore = DOTween.To(PureQuaternionPlugin.Plug(), () => target.rotation, delegate(Quaternion x)
		{
			target.rotation = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DOLocalRotate(this Transform target, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
	{
		TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => target.localRotation, delegate(Quaternion x)
		{
			target.localRotation = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		tweenerCore.plugOptions.rotateMode = mode;
		return tweenerCore;
	}

	public static TweenerCore<Quaternion, Quaternion, NoOptions> DOLocalRotateQuaternion(this Transform target, Quaternion endValue, float duration)
	{
		TweenerCore<Quaternion, Quaternion, NoOptions> tweenerCore = DOTween.To(PureQuaternionPlugin.Plug(), () => target.localRotation, delegate(Quaternion x)
		{
			target.localRotation = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this Transform target, Vector3 endValue, float duration)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localScale, delegate(Vector3 x)
		{
			target.localScale = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this Transform target, float endValue, float duration)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> obj = DOTween.To(endValue: new Vector3(endValue, endValue, endValue), getter: () => target.localScale, setter: delegate(Vector3 x)
		{
			target.localScale = x;
		}, duration: duration);
		obj.SetTarget(target);
		return obj;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleX(this Transform target, float endValue, float duration)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localScale, delegate(Vector3 x)
		{
			target.localScale = x;
		}, new Vector3(endValue, 0f, 0f), duration);
		tweenerCore.SetOptions(AxisConstraint.X).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleY(this Transform target, float endValue, float duration)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localScale, delegate(Vector3 x)
		{
			target.localScale = x;
		}, new Vector3(0f, endValue, 0f), duration);
		tweenerCore.SetOptions(AxisConstraint.Y).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleZ(this Transform target, float endValue, float duration)
	{
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localScale, delegate(Vector3 x)
		{
			target.localScale = x;
		}, new Vector3(0f, 0f, endValue), duration);
		tweenerCore.SetOptions(AxisConstraint.Z).SetTarget(target);
		return tweenerCore;
	}

	public static Tweener DOLookAt(this Transform target, Vector3 towards, float duration, AxisConstraint axisConstraint = AxisConstraint.None, Vector3? up = null)
	{
		TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => target.rotation, delegate(Quaternion x)
		{
			target.rotation = x;
		}, towards, duration).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetLookAt);
		tweenerCore.plugOptions.axisConstraint = axisConstraint;
		tweenerCore.plugOptions.up = ((!up.HasValue) ? Vector3.up : up.Value);
		return tweenerCore;
	}

	public static Tweener DOPunchPosition(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f, bool snapping = false)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOPunchPosition: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Punch(() => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, punch, duration, vibrato, elasticity).SetTarget(target).SetOptions(snapping);
	}

	public static Tweener DOPunchScale(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOPunchScale: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Punch(() => target.localScale, delegate(Vector3 x)
		{
			target.localScale = x;
		}, punch, duration, vibrato, elasticity).SetTarget(target);
	}

	public static Tweener DOPunchRotation(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOPunchRotation: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Punch(() => target.localEulerAngles, delegate(Vector3 x)
		{
			target.localRotation = Quaternion.Euler(x);
		}, punch, duration, vibrato, elasticity).SetTarget(target);
	}

	public static Tweener DOShakePosition(this Transform target, float duration, float strength = 1f, int vibrato = 10, float randomness = 90f, bool snapping = false, bool fadeOut = true)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, duration, strength, vibrato, randomness, ignoreZAxis: false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake)
			.SetOptions(snapping);
	}

	public static Tweener DOShakePosition(this Transform target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool snapping = false, bool fadeOut = true)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake)
			.SetOptions(snapping);
	}

	public static Tweener DOShakeRotation(this Transform target, float duration, float strength = 90f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => target.localEulerAngles, delegate(Vector3 x)
		{
			target.localRotation = Quaternion.Euler(x);
		}, duration, strength, vibrato, randomness, ignoreZAxis: false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
	}

	public static Tweener DOShakeRotation(this Transform target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => target.localEulerAngles, delegate(Vector3 x)
		{
			target.localRotation = Quaternion.Euler(x);
		}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
	}

	public static Tweener DOShakeScale(this Transform target, float duration, float strength = 1f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (duration <= 0f)
		{
			Debug.Log(Debugger.logPriority);
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOShakeScale: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => target.localScale, delegate(Vector3 x)
		{
			target.localScale = x;
		}, duration, strength, vibrato, randomness, ignoreZAxis: false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
	}

	public static Tweener DOShakeScale(this Transform target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOShakeScale: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		return DOTween.Shake(() => target.localScale, delegate(Vector3 x)
		{
			target.localScale = x;
		}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
	}

	public static Sequence DOJump(this Transform target, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
	{
		if (numJumps < 1)
		{
			numJumps = 1;
		}
		float startPosY = 0f;
		float offsetY = -1f;
		bool offsetYSet = false;
		Sequence s = DOTween.Sequence();
		Tween yTween = DOTween.To(() => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, new Vector3(0f, jumpPower, 0f), duration / (float)(numJumps * 2)).SetOptions(AxisConstraint.Y, snapping).SetEase(Ease.OutQuad)
			.SetRelative()
			.SetLoops(numJumps * 2, LoopType.Yoyo)
			.OnStart(delegate
			{
				startPosY = target.position.y;
			});
		s.Append(DOTween.To(() => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, new Vector3(endValue.x, 0f, 0f), duration).SetOptions(AxisConstraint.X, snapping).SetEase(Ease.Linear)).Join(DOTween.To(() => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, new Vector3(0f, 0f, endValue.z), duration).SetOptions(AxisConstraint.Z, snapping).SetEase(Ease.Linear)).Join(yTween)
			.SetTarget(target)
			.SetEase(DOTween.defaultEaseType);
		yTween.OnUpdate(delegate
		{
			if (!offsetYSet)
			{
				offsetYSet = true;
				offsetY = (s.isRelative ? endValue.y : (endValue.y - startPosY));
			}
			Vector3 position = target.position;
			position.y += DOVirtual.EasedValue(0f, offsetY, yTween.ElapsedPercentage(), Ease.OutQuad);
			target.position = position;
		});
		return s;
	}

	public static Sequence DOLocalJump(this Transform target, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
	{
		if (numJumps < 1)
		{
			numJumps = 1;
		}
		float startPosY = target.localPosition.y;
		float offsetY = -1f;
		bool offsetYSet = false;
		Sequence s = DOTween.Sequence();
		s.Append(DOTween.To(() => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, new Vector3(endValue.x, 0f, 0f), duration).SetOptions(AxisConstraint.X, snapping).SetEase(Ease.Linear)).Join(DOTween.To(() => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, new Vector3(0f, 0f, endValue.z), duration).SetOptions(AxisConstraint.Z, snapping).SetEase(Ease.Linear)).Join(DOTween.To(() => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, new Vector3(0f, jumpPower, 0f), duration / (float)(numJumps * 2)).SetOptions(AxisConstraint.Y, snapping).SetEase(Ease.OutQuad)
			.SetRelative()
			.SetLoops(numJumps * 2, LoopType.Yoyo))
			.SetTarget(target)
			.SetEase(DOTween.defaultEaseType)
			.OnUpdate(delegate
			{
				if (!offsetYSet)
				{
					offsetYSet = false;
					offsetY = (s.isRelative ? endValue.y : (endValue.y - startPosY));
				}
				Vector3 localPosition = target.localPosition;
				localPosition.y += DOVirtual.EasedValue(0f, offsetY, s.ElapsedDirectionalPercentage(), Ease.OutQuad);
				target.localPosition = localPosition;
			});
		return s;
	}

	public static TweenerCore<Vector3, Path, PathOptions> DOPath(this Transform target, Vector3[] path, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
	{
		if (resolution < 1)
		{
			resolution = 1;
		}
		TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To(PathPlugin.Get(), () => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, new Path(pathType, path, resolution, gizmoColor), duration).SetTarget(target);
		tweenerCore.plugOptions.mode = pathMode;
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Transform target, Vector3[] path, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
	{
		if (resolution < 1)
		{
			resolution = 1;
		}
		TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To(PathPlugin.Get(), () => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, new Path(pathType, path, resolution, gizmoColor), duration).SetTarget(target);
		tweenerCore.plugOptions.mode = pathMode;
		tweenerCore.plugOptions.useLocalPosition = true;
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Path, PathOptions> DOPath(this Transform target, Path path, float duration, PathMode pathMode = PathMode.Full3D)
	{
		TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To(PathPlugin.Get(), () => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, path, duration).SetTarget(target);
		tweenerCore.plugOptions.mode = pathMode;
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Transform target, Path path, float duration, PathMode pathMode = PathMode.Full3D)
	{
		TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To(PathPlugin.Get(), () => target.localPosition, delegate(Vector3 x)
		{
			target.localPosition = x;
		}, path, duration).SetTarget(target);
		tweenerCore.plugOptions.mode = pathMode;
		tweenerCore.plugOptions.useLocalPosition = true;
		return tweenerCore;
	}

	public static TweenerCore<float, float, FloatOptions> DOTimeScale(this Tween target, float endValue, float duration)
	{
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.timeScale, delegate(float x)
		{
			target.timeScale = x;
		}, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static Tweener DOBlendableColor(this Light target, Color endValue, float duration)
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

	public static Tweener DOBlendableColor(this Material target, Color endValue, float duration)
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

	public static Tweener DOBlendableColor(this Material target, Color endValue, string property, float duration)
	{
		if (!target.HasProperty(property))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(property);
			}
			return null;
		}
		endValue -= target.GetColor(property);
		Color to = new Color(0f, 0f, 0f, 0f);
		return DOTween.To(() => to, delegate(Color x)
		{
			Color color = x - to;
			to = x;
			target.SetColor(property, target.GetColor(property) + color);
		}, endValue, duration).Blendable().SetTarget(target);
	}

	public static Tweener DOBlendableColor(this Material target, Color endValue, int propertyID, float duration)
	{
		if (!target.HasProperty(propertyID))
		{
			if (Debugger.logPriority > 0)
			{
				Debugger.LogMissingMaterialProperty(propertyID);
			}
			return null;
		}
		endValue -= target.GetColor(propertyID);
		Color to = new Color(0f, 0f, 0f, 0f);
		return DOTween.To(() => to, delegate(Color x)
		{
			Color color = x - to;
			to = x;
			target.SetColor(propertyID, target.GetColor(propertyID) + color);
		}, endValue, duration).Blendable().SetTarget(target);
	}

	public static Tweener DOBlendableMoveBy(this Transform target, Vector3 byValue, float duration, bool snapping = false)
	{
		Vector3 to = Vector3.zero;
		return DOTween.To(() => to, delegate(Vector3 x)
		{
			Vector3 vector = x - to;
			to = x;
			target.position += vector;
		}, byValue, duration).Blendable().SetOptions(snapping)
			.SetTarget(target);
	}

	public static Tweener DOBlendableLocalMoveBy(this Transform target, Vector3 byValue, float duration, bool snapping = false)
	{
		Vector3 to = Vector3.zero;
		return DOTween.To(() => to, delegate(Vector3 x)
		{
			Vector3 vector = x - to;
			to = x;
			target.localPosition += vector;
		}, byValue, duration).Blendable().SetOptions(snapping)
			.SetTarget(target);
	}

	public static Tweener DOBlendableRotateBy(this Transform target, Vector3 byValue, float duration, RotateMode mode = RotateMode.Fast)
	{
		Quaternion to = Quaternion.identity;
		TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => to, delegate(Quaternion x)
		{
			Quaternion quaternion = x * Quaternion.Inverse(to);
			to = x;
			Quaternion rotation = target.rotation;
			target.rotation = rotation * Quaternion.Inverse(rotation) * quaternion * rotation;
		}, byValue, duration).Blendable().SetTarget(target);
		tweenerCore.plugOptions.rotateMode = mode;
		return tweenerCore;
	}

	public static Tweener DOBlendableLocalRotateBy(this Transform target, Vector3 byValue, float duration, RotateMode mode = RotateMode.Fast)
	{
		Quaternion to = Quaternion.identity;
		TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => to, delegate(Quaternion x)
		{
			Quaternion quaternion = x * Quaternion.Inverse(to);
			to = x;
			Quaternion localRotation = target.localRotation;
			target.localRotation = localRotation * Quaternion.Inverse(localRotation) * quaternion * localRotation;
		}, byValue, duration).Blendable().SetTarget(target);
		tweenerCore.plugOptions.rotateMode = mode;
		return tweenerCore;
	}

	public static Tweener DOBlendablePunchRotation(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
	{
		if (duration <= 0f)
		{
			if (Debugger.logPriority > 0)
			{
				Debug.LogWarning("DOBlendablePunchRotation: duration can't be 0, returning NULL without creating a tween");
			}
			return null;
		}
		Vector3 to = Vector3.zero;
		return DOTween.Punch(() => to, delegate(Vector3 v)
		{
			Quaternion rotation = Quaternion.Euler(to.x, to.y, to.z);
			Quaternion quaternion = Quaternion.Euler(v.x, v.y, v.z) * Quaternion.Inverse(rotation);
			to = v;
			Quaternion rotation2 = target.rotation;
			target.rotation = rotation2 * Quaternion.Inverse(rotation2) * quaternion * rotation2;
		}, punch, duration, vibrato, elasticity).Blendable().SetTarget(target);
	}

	public static Tweener DOBlendableScaleBy(this Transform target, Vector3 byValue, float duration)
	{
		Vector3 to = Vector3.zero;
		return DOTween.To(() => to, delegate(Vector3 x)
		{
			Vector3 vector = x - to;
			to = x;
			target.localScale += vector;
		}, byValue, duration).Blendable().SetTarget(target);
	}

	public static int DOComplete(this Component target, bool withCallbacks = false)
	{
		return DOTween.Complete(target, withCallbacks);
	}

	public static int DOComplete(this Material target, bool withCallbacks = false)
	{
		return DOTween.Complete(target, withCallbacks);
	}

	public static int DOKill(this Component target, bool complete = false)
	{
		return DOTween.Kill(target, complete);
	}

	public static int DOKill(this Material target, bool complete = false)
	{
		return DOTween.Kill(target, complete);
	}

	public static int DOFlip(this Component target)
	{
		return DOTween.Flip(target);
	}

	public static int DOFlip(this Material target)
	{
		return DOTween.Flip(target);
	}

	public static int DOGoto(this Component target, float to, bool andPlay = false)
	{
		return DOTween.Goto(target, to, andPlay);
	}

	public static int DOGoto(this Material target, float to, bool andPlay = false)
	{
		return DOTween.Goto(target, to, andPlay);
	}

	public static int DOPause(this Component target)
	{
		return DOTween.Pause(target);
	}

	public static int DOPause(this Material target)
	{
		return DOTween.Pause(target);
	}

	public static int DOPlay(this Component target)
	{
		return DOTween.Play(target);
	}

	public static int DOPlay(this Material target)
	{
		return DOTween.Play(target);
	}

	public static int DOPlayBackwards(this Component target)
	{
		return DOTween.PlayBackwards(target);
	}

	public static int DOPlayBackwards(this Material target)
	{
		return DOTween.PlayBackwards(target);
	}

	public static int DOPlayForward(this Component target)
	{
		return DOTween.PlayForward(target);
	}

	public static int DOPlayForward(this Material target)
	{
		return DOTween.PlayForward(target);
	}

	public static int DORestart(this Component target, bool includeDelay = true)
	{
		return DOTween.Restart(target, includeDelay);
	}

	public static int DORestart(this Material target, bool includeDelay = true)
	{
		return DOTween.Restart(target, includeDelay);
	}

	public static int DORewind(this Component target, bool includeDelay = true)
	{
		return DOTween.Rewind(target, includeDelay);
	}

	public static int DORewind(this Material target, bool includeDelay = true)
	{
		return DOTween.Rewind(target, includeDelay);
	}

	public static int DOSmoothRewind(this Component target)
	{
		return DOTween.SmoothRewind(target);
	}

	public static int DOSmoothRewind(this Material target)
	{
		return DOTween.SmoothRewind(target);
	}

	public static int DOTogglePause(this Component target)
	{
		return DOTween.TogglePause(target);
	}

	public static int DOTogglePause(this Material target)
	{
		return DOTween.TogglePause(target);
	}
}
