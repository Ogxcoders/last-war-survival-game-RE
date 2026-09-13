using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class SetShadowDistancePlayableAsset : PlayableAsset
{
	public int shadowDistance = 60;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
	{
		ScriptPlayable<SetShadowDistancePlayableBehaviour> scriptPlayable = ScriptPlayable<SetShadowDistancePlayableBehaviour>.Create(graph);
		scriptPlayable.GetBehaviour().shadowDistance = shadowDistance;
		return scriptPlayable;
	}
}
