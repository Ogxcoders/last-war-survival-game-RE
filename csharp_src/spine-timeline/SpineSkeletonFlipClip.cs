using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SpineSkeletonFlipClip : PlayableAsset, ITimelineClipAsset
{
	public SpineSkeletonFlipBehaviour template = new SpineSkeletonFlipBehaviour();

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<SpineSkeletonFlipBehaviour>.Create(graph, template);
	}
}
