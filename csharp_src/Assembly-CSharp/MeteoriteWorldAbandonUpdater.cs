using UnityEngine;

public class MeteoriteWorldAbandonUpdater : MonoBehaviour
{
	private enum ParticlePlayState
	{
		None,
		Playing,
		Finished
	}

	public BezierMovement bezierMovement;

	public ParticleSystem postEffectParticle;

	private float flyTime;

	private float postEffectTime;

	private float totalTime;

	private float elapsedTime;

	private ParticlePlayState playState;

	public void Init(Vector3 fromPosition, Vector3 toPosition, float flyTime, float height, float postEffectTime, float elapsedTime)
	{
		this.flyTime = flyTime;
		this.postEffectTime = postEffectTime;
		this.elapsedTime = elapsedTime;
		totalTime = flyTime + postEffectTime;
		bezierMovement.Init(fromPosition, toPosition, flyTime, height);
		postEffectParticle.gameObject.TryActive(active: false);
		postEffectParticle.Stop();
		playState = ParticlePlayState.None;
	}

	public void DeltaUpdate(float deltaTime)
	{
		elapsedTime += deltaTime;
		float progress = Mathf.Clamp01(elapsedTime / flyTime);
		bezierMovement.ManualUpdate(progress);
		if (elapsedTime > flyTime && playState == ParticlePlayState.None)
		{
			postEffectParticle.gameObject.TryActive(active: true);
			postEffectParticle.Simulate(elapsedTime - flyTime, withChildren: true, restart: true, fixedTimeStep: true);
			postEffectParticle.Play();
			playState = ParticlePlayState.Playing;
		}
	}
}
