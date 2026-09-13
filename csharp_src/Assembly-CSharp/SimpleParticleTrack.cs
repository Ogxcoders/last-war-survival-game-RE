using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.5f, 0.3f, 0.8f)]
[TrackClipType(typeof(SimpleParticleClip))]
public class SimpleParticleTrack : TrackAsset
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<EmptyMixerPlayableBehaviour>.Create(graph, inputCount);
	}
}
