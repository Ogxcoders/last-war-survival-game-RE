using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SimpleParticleClip : PlayableAsset, ITimelineClipAsset
{
	public ExposedReference<GameObject> targetGameObject;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<SimpleParticlePlayableBehaviour> scriptPlayable = ScriptPlayable<SimpleParticlePlayableBehaviour>.Create(graph);
		scriptPlayable.GetBehaviour().target = targetGameObject.Resolve(graph.GetResolver());
		return scriptPlayable;
	}
}
