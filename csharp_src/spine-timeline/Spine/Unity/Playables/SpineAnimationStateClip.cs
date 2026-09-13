using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Spine.Unity.Playables;

[Serializable]
public class SpineAnimationStateClip : PlayableAsset, ITimelineClipAsset
{
	public SpineAnimationStateBehaviour template = new SpineAnimationStateBehaviour();

	[NonSerialized]
	public TimelineClip timelineClip;

	public ClipCaps clipCaps => (ClipCaps)(0x1C | (template.loop ? 1 : 0));

	public override double duration
	{
		get
		{
			if (template.animationReference == null || template.animationReference.Animation == null)
			{
				return 0.0;
			}
			return template.animationReference.Animation.Duration;
		}
	}

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		template.timelineClip = timelineClip;
		ScriptPlayable<SpineAnimationStateBehaviour> scriptPlayable = ScriptPlayable<SpineAnimationStateBehaviour>.Create(graph, template);
		scriptPlayable.GetBehaviour();
		return scriptPlayable;
	}
}
