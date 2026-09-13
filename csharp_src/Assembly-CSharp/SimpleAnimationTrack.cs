using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
[TrackColor(0.5f, 0.8f, 0.3f)]
[TrackClipType(typeof(SimpleAnimationClip))]
public class SimpleAnimationTrack : TrackAsset
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<EmptyMixerPlayableBehaviour>.Create(graph, inputCount);
	}
}
