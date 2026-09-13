using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening;

public static class DOTweenModulePhysics2D
{
	public static TweenerCore<Vector2, Vector2, VectorOptions> DOMove(this Rigidbody2D target, Vector2 endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.position, target.MovePosition, endValue, duration);
		tweenerCore.SetOptions(snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector2, Vector2, VectorOptions> DOMoveX(this Rigidbody2D target, float endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.position, target.MovePosition, new Vector2(endValue, 0f), duration);
		tweenerCore.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<Vector2, Vector2, VectorOptions> DOMoveY(this Rigidbody2D target, float endValue, float duration, bool snapping = false)
	{
		TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.position, target.MovePosition, new Vector2(0f, endValue), duration);
		tweenerCore.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
		return tweenerCore;
	}

	public static TweenerCore<float, float, FloatOptions> DORotate(this Rigidbody2D target, float endValue, float duration)
	{
		TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.rotation, target.MoveRotation, endValue, duration);
		tweenerCore.SetTarget(target);
		return tweenerCore;
	}

	public static Sequence DOJump(this Rigidbody2D target, Vector2 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
	{
		if (numJumps < 1)
		{
			numJumps = 1;
		}
		float startPosY = 0f;
		float offsetY = -1f;
		bool offsetYSet = false;
		Sequence s = DOTween.Sequence();
		Tween yTween = DOTween.To(() => target.position, delegate(Vector2 x)
		{
			target.position = x;
		}, new Vector2(0f, jumpPower), duration / (float)(numJumps * 2)).SetOptions(AxisConstraint.Y, snapping).SetEase(Ease.OutQuad)
			.SetRelative()
			.SetLoops(numJumps * 2, LoopType.Yoyo)
			.OnStart(delegate
			{
				startPosY = target.position.y;
			});
		s.Append(DOTween.To(() => target.position, delegate(Vector2 x)
		{
			target.position = x;
		}, new Vector2(endValue.x, 0f), duration).SetOptions(AxisConstraint.X, snapping).SetEase(Ease.Linear)).Join(yTween).SetTarget(target)
			.SetEase(DOTween.defaultEaseType);
		yTween.OnUpdate(delegate
		{
			if (!offsetYSet)
			{
				offsetYSet = true;
				offsetY = (s.isRelative ? endValue.y : (endValue.y - startPosY));
			}
			Vector3 vector = target.position;
			vector.y += DOVirtual.EasedValue(0f, offsetY, yTween.ElapsedPercentage(), Ease.OutQuad);
			target.MovePosition(vector);
		});
		return s;
	}

	public static TweenerCore<Vector3, Path, PathOptions> DOPath(this Rigidbody2D target, Vector2[] path, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
	{
		if (resolution < 1)
		{
			resolution = 1;
		}
		int num = path.Length;
		Vector3[] array = new Vector3[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = path[i];
		}
		TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To(PathPlugin.Get(), () => target.position, delegate(Vector3 x)
		{
			target.MovePosition(x);
		}, new Path(pathType, array, resolution, gizmoColor), duration).SetTarget(target).SetUpdate(UpdateType.Fixed);
		tweenerCore.plugOptions.isRigidbody = true;
		tweenerCore.plugOptions.mode = pathMode;
		return tweenerCore;
	}

	public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Rigidbody2D target, Vector2[] path, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
	{
		if (resolution < 1)
		{
			resolution = 1;
		}
		int num = path.Length;
		Vector3[] array = new Vector3[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = path[i];
		}
		Transform trans = target.transform;
		TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To(PathPlugin.Get(), () => trans.localPosition, delegate(Vector3 x)
		{
			target.MovePosition((trans.parent == null) ? x : trans.parent.TransformPoint(x));
		}, new Path(pathType, array, resolution, gizmoColor), duration).SetTarget(target).SetUpdate(UpdateType.Fixed);
		tweenerCore.plugOptions.isRigidbody = true;
		tweenerCore.plugOptions.mode = pathMode;
		tweenerCore.plugOptions.useLocalPosition = true;
		return tweenerCore;
	}
}
