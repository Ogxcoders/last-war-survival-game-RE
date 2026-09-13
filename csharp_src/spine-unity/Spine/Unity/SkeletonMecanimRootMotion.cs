using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity;

[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonMecanimRootMotion")]
public class SkeletonMecanimRootMotion : SkeletonRootMotionBase
{
	private const int DefaultMecanimLayerFlags = -1;

	public int mecanimLayerFlags = -1;

	protected Vector2 movementDelta;

	private SkeletonMecanim skeletonMecanim;

	public SkeletonMecanim SkeletonMecanim
	{
		get
		{
			if (!skeletonMecanim)
			{
				return skeletonMecanim = GetComponent<SkeletonMecanim>();
			}
			return skeletonMecanim;
		}
	}

	public override Vector2 GetRemainingRootMotion(int layerIndex)
	{
		KeyValuePair<Animation, float> activeAnimationAndTime = skeletonMecanim.Translator.GetActiveAnimationAndTime(layerIndex);
		Animation key = activeAnimationAndTime.Key;
		float value = activeAnimationAndTime.Value;
		if (key == null)
		{
			return Vector2.zero;
		}
		float startTime = value;
		float duration = key.Duration;
		return GetAnimationRootMotion(startTime, duration, key);
	}

	public override RootMotionInfo GetRootMotionInfo(int layerIndex)
	{
		KeyValuePair<Animation, float> activeAnimationAndTime = skeletonMecanim.Translator.GetActiveAnimationAndTime(layerIndex);
		Animation key = activeAnimationAndTime.Key;
		float value = activeAnimationAndTime.Value;
		if (key == null)
		{
			return default(RootMotionInfo);
		}
		return GetAnimationRootMotionInfo(key, value);
	}

	protected override void Reset()
	{
		base.Reset();
		mecanimLayerFlags = -1;
	}

	protected override void Start()
	{
		base.Start();
		skeletonMecanim = GetComponent<SkeletonMecanim>();
		if ((bool)skeletonMecanim)
		{
			skeletonMecanim.Translator.OnClipApplied -= OnClipApplied;
			skeletonMecanim.Translator.OnClipApplied += OnClipApplied;
		}
	}

	private void OnClipApplied(Animation animation, int layerIndex, float weight, float time, float lastTime, bool playsBackward)
	{
		if ((mecanimLayerFlags & (1 << layerIndex)) != 0 && weight != 0f)
		{
			if (!playsBackward)
			{
				movementDelta += weight * GetAnimationRootMotion(lastTime, time, animation);
			}
			else
			{
				movementDelta -= weight * GetAnimationRootMotion(time, lastTime, animation);
			}
		}
	}

	protected override Vector2 CalculateAnimationsMovementDelta()
	{
		Vector2 result = movementDelta;
		movementDelta = Vector2.zero;
		return result;
	}
}
