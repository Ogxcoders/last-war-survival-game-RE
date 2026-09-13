using UnityEngine;

namespace Spine.Unity;

[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonRootMotion")]
public class SkeletonRootMotion : SkeletonRootMotionBase
{
	private const int DefaultAnimationTrackFlags = -1;

	public int animationTrackFlags = -1;

	private AnimationState animationState;

	private Canvas canvas;

	protected override float AdditionalScale
	{
		get
		{
			if (!canvas)
			{
				return 1f;
			}
			return canvas.referencePixelsPerUnit;
		}
	}

	public override Vector2 GetRemainingRootMotion(int trackIndex)
	{
		TrackEntry current = animationState.GetCurrent(trackIndex);
		if (current == null)
		{
			return Vector2.zero;
		}
		Animation animation = current.Animation;
		float animationTime = current.AnimationTime;
		float duration = animation.Duration;
		return GetAnimationRootMotion(animationTime, duration, animation);
	}

	public override RootMotionInfo GetRootMotionInfo(int trackIndex)
	{
		TrackEntry current = animationState.GetCurrent(trackIndex);
		if (current == null)
		{
			return default(RootMotionInfo);
		}
		_ = current.Animation;
		float animationTime = current.AnimationTime;
		return GetAnimationRootMotionInfo(current.Animation, animationTime);
	}

	protected override void Reset()
	{
		base.Reset();
		animationTrackFlags = -1;
	}

	protected override void Start()
	{
		base.Start();
		animationState = (skeletonComponent as IAnimationStateComponent)?.AnimationState;
		if (GetComponent<CanvasRenderer>() != null)
		{
			canvas = GetComponentInParent<Canvas>();
		}
	}

	protected override Vector2 CalculateAnimationsMovementDelta()
	{
		Vector2 zero = Vector2.zero;
		int count = animationState.Tracks.Count;
		for (int i = 0; i < count; i++)
		{
			if (animationTrackFlags != -1 && (animationTrackFlags & (1 << i)) == 0)
			{
				continue;
			}
			TrackEntry trackEntry = animationState.GetCurrent(i);
			TrackEntry next = null;
			while (trackEntry != null)
			{
				Animation animation = trackEntry.Animation;
				float animationLast = trackEntry.AnimationLast;
				float animationTime = trackEntry.AnimationTime;
				Vector2 currentDelta = GetAnimationRootMotion(animationLast, animationTime, animation);
				if (currentDelta != Vector2.zero)
				{
					ApplyMixAlphaToDelta(ref currentDelta, next, trackEntry);
					zero += currentDelta;
				}
				next = trackEntry;
				trackEntry = trackEntry.MixingFrom;
			}
		}
		return zero;
	}

	private void ApplyMixAlphaToDelta(ref Vector2 currentDelta, TrackEntry next, TrackEntry track)
	{
		float num;
		if (next != null)
		{
			if (next.MixDuration == 0f)
			{
				num = 1f;
			}
			else
			{
				num = next.MixTime / next.MixDuration;
				if (num > 1f)
				{
					num = 1f;
				}
			}
			float num2 = track.Alpha * next.InterruptAlpha * (1f - num);
			currentDelta *= num2;
			return;
		}
		if (track.MixDuration == 0f)
		{
			num = 1f;
		}
		else
		{
			num = track.Alpha * (track.MixTime / track.MixDuration);
			if (num > 1f)
			{
				num = 1f;
			}
		}
		currentDelta *= num;
	}
}
