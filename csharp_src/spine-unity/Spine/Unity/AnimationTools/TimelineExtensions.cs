using UnityEngine;

namespace Spine.Unity.AnimationTools;

public static class TimelineExtensions
{
	public static Vector2 Evaluate(this TranslateTimeline timeline, float time, SkeletonData skeletonData = null)
	{
		if (time < timeline.Frames[0])
		{
			return Vector2.zero;
		}
		timeline.GetCurveValue(out var x, out var y, time);
		if (skeletonData == null)
		{
			return new Vector2(x, y);
		}
		BoneData boneData = skeletonData.Bones.Items[timeline.BoneIndex];
		return new Vector2(boneData.X + x, boneData.Y + y);
	}

	public static TranslateTimeline FindTranslateTimelineForBone(this Animation a, int boneIndex)
	{
		foreach (Timeline timeline in a.Timelines)
		{
			if (!timeline.GetType().IsSubclassOf(typeof(TranslateTimeline)) && timeline is TranslateTimeline translateTimeline && translateTimeline.BoneIndex == boneIndex)
			{
				return translateTimeline;
			}
		}
		return null;
	}
}
