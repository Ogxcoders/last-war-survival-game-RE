using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineScript;

[Serializable]
public class LoopControlClip : PlayableAsset, ITimelineClipAsset
{
	[HideInInspector]
	public LoopControlBehaviour template = new LoopControlBehaviour();

	public ClipCaps clipCaps => ClipCaps.All;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<LoopControlBehaviour>.Create(graph, template);
	}
}
