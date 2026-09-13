using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class SimpleAnimationClip : PlayableAsset
{
	[SerializeField]
	public ExposedReference<SimpleAnimation> animation;

	[SerializeField]
	public string state = "";

	[SerializeField]
	public float speed = 1f;

	[SerializeField]
	public AnimationClip clip;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<SimpleAnimationPlayableBehaviour> scriptPlayable = ScriptPlayable<SimpleAnimationPlayableBehaviour>.Create(graph);
		scriptPlayable.GetBehaviour().Initialize(animation.Resolve(graph.GetResolver()), state, speed);
		return scriptPlayable;
	}
}
