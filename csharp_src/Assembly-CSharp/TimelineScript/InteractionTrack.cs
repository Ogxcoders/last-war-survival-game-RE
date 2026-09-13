using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineScript;

[TrackColor(0.855f, 0.903f, 0.87f)]
[TrackClipType(typeof(InteractionClip))]
public class InteractionTrack : TrackAsset
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		foreach (TimelineClip clip in GetClips())
		{
			InteractionClip interactionClip = clip.asset as InteractionClip;
			if (interactionClip != null)
			{
				interactionClip.template.Duration = (float)clip.duration;
			}
		}
		return base.CreateTrackMixer(graph, go, inputCount);
	}
}
