using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineScript;

[Serializable]
public class InteractionClip : PlayableAsset, ITimelineClipAsset
{
	public InteractionBehaviour template = new InteractionBehaviour();

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<InteractionBehaviour>.Create(graph, template);
	}
}
