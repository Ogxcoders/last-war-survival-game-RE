using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class SetShadowDistancePlayableBehaviour : PlayableBehaviour
{
	public int shadowDistance = 60;

	public override void OnGraphStart(Playable playable)
	{
	}

	public override void OnGraphStop(Playable playable)
	{
	}

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		(QualitySettings.renderPipeline as UniversalRenderPipelineAsset).shadowDistance = shadowDistance;
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
	}

	public override void PrepareFrame(Playable playable, FrameData info)
	{
	}
}
