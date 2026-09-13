using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Spine.Unity.Playables;

[TrackColor(0.9960785f, 0.2509804f, 0.003921569f)]
[TrackClipType(typeof(SpineAnimationStateClip))]
[TrackBindingType(typeof(SkeletonGraphic))]
public class SpineAnimationStateGraphicTrack : TrackAsset
{
	public int trackIndex;

	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		ScriptPlayable<SpineAnimationStateMixerBehaviour> scriptPlayable = ScriptPlayable<SpineAnimationStateMixerBehaviour>.Create(graph, inputCount);
		scriptPlayable.GetBehaviour().trackIndex = trackIndex;
		return scriptPlayable;
	}
}
