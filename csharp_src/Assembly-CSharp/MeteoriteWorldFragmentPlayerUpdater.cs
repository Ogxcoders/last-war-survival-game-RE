using System;
using UnityEngine;

[Serializable]
public abstract class MeteoriteWorldFragmentPlayerUpdater
{
	private enum ParticlePlayState
	{
		None,
		Playing,
		Finished
	}

	protected MeteoriteWorldFragmentPlayer player;

	protected Vector3 startPosition;

	protected Vector3 targetPosition;

	protected Vector3 direction;

	protected float dropTimeSec;

	protected float exploreTimeSec;

	protected float afterDropTimeSec;

	protected float crackTimeSec;

	protected float elapsedTime;

	protected float rotateSpeed;

	private bool canRotate;

	public float dropRetainSec;

	private ParticlePlayState hitEffectState;

	private ParticlePlayState crackEffectState;

	public void Init(MeteoriteWorldFragmentPlayer player, MeteoriteWorldFragmentPlayer.DisplaySetting settings, float elapsedTimeSec)
	{
		this.player = player;
		Quaternion quaternion = Quaternion.Euler(0f, 0f, settings.degree);
		targetPosition = Vector3.zero;
		startPosition = targetPosition + quaternion * Vector3.up * settings.height;
		player.transform.localPosition = settings.target;
		direction = targetPosition - startPosition;
		direction.Normalize();
		player.nodeDrop.forward = direction;
		if (player.nodeScaler != null)
		{
			player.nodeScaler.localScale = settings.scale;
		}
		dropTimeSec = settings.dropTimeSec;
		exploreTimeSec = settings.exploreTimeSec;
		crackTimeSec = settings.crackTimeSec;
		afterDropTimeSec = Mathf.Max(exploreTimeSec, crackTimeSec);
		rotateSpeed = settings.rotateSpeed;
		canRotate = player.nodeRotator != null;
		if (exploreTimeSec > 0f && player.particleHitEffect != null)
		{
			player.particleHitEffect.Stop(withChildren: true);
		}
		if (crackTimeSec > 0f && player.particleCrackEffect != null)
		{
			player.particleCrackEffect.Stop(withChildren: true);
		}
		player.nodeDrop.gameObject.TryActive(active: false);
		player.nodeExplore.gameObject.TryActive(active: false);
		player.nodeCrack.gameObject.TryActive(active: false);
		hitEffectState = ParticlePlayState.None;
		crackEffectState = ParticlePlayState.None;
		if (player.trailRenderer != null)
		{
			player.trailRenderer.Clear();
		}
		elapsedTime = elapsedTimeSec;
		DeltaUpdate(0f);
		OnInit(settings, elapsedTimeSec);
	}

	protected virtual void OnInit(MeteoriteWorldFragmentPlayer.DisplaySetting settings, float elapsedTimeSec)
	{
	}

	public void DeltaUpdate(float deltaTime)
	{
		float num = 0f;
		elapsedTime += deltaTime;
		if (elapsedTime <= dropTimeSec)
		{
			num = elapsedTime / dropTimeSec;
			if (player.nodeDrop != null)
			{
				player.nodeDrop.localPosition = Vector3.Lerp(startPosition, targetPosition, num);
			}
			if (canRotate)
			{
				player.nodeRotator.Rotate(player.rotateAxis, deltaTime * rotateSpeed, Space.Self);
			}
		}
		if (elapsedTime <= dropTimeSec + dropRetainSec)
		{
			num = elapsedTime / (dropTimeSec + dropRetainSec);
			player.nodeDrop.gameObject.TryActive(active: true);
			OnUpdateDrop(deltaTime, elapsedTime, num);
		}
		else
		{
			player.nodeDrop.gameObject.TryActive(active: false);
		}
		if (exploreTimeSec > 0f && player.particleHitEffect != null)
		{
			if (elapsedTime > dropTimeSec && elapsedTime <= dropTimeSec + exploreTimeSec)
			{
				if (hitEffectState == ParticlePlayState.None)
				{
					hitEffectState = ParticlePlayState.Playing;
					player.nodeExplore.gameObject.TryActive(active: true);
					player.particleHitEffect.Simulate(elapsedTime - dropTimeSec, withChildren: true, restart: true, fixedTimeStep: true);
					player.particleHitEffect.Play();
				}
				OnUpdateExplore(deltaTime, elapsedTime, num);
			}
			else if (elapsedTime > dropTimeSec + exploreTimeSec && hitEffectState == ParticlePlayState.Playing)
			{
				hitEffectState = ParticlePlayState.Playing;
				player.nodeExplore.gameObject.TryActive(active: false);
			}
		}
		if (crackTimeSec > 0f && player.particleCrackEffect != null)
		{
			if (elapsedTime > dropTimeSec && elapsedTime <= dropTimeSec + crackTimeSec)
			{
				if (crackEffectState == ParticlePlayState.None)
				{
					crackEffectState = ParticlePlayState.Playing;
					player.nodeCrack.gameObject.TryActive(active: true);
					player.particleCrackEffect.Simulate(elapsedTime - dropTimeSec, withChildren: true, restart: true, fixedTimeStep: true);
					player.particleCrackEffect.Play();
				}
				OnUpdateCrack(deltaTime, elapsedTime, num);
			}
			else if (elapsedTime > dropTimeSec + crackTimeSec && crackEffectState == ParticlePlayState.Playing)
			{
				crackEffectState = ParticlePlayState.Finished;
				player.nodeCrack.gameObject.TryActive(active: false);
			}
		}
		if (elapsedTime > dropTimeSec + afterDropTimeSec)
		{
			player.nodeDrop.gameObject.TryActive(active: false);
			player.nodeExplore.gameObject.TryActive(active: false);
			player.nodeCrack.gameObject.TryActive(active: false);
		}
	}

	protected virtual void OnUpdateDrop(float deltaTime, float elapsedTime, float progress)
	{
	}

	protected virtual void OnUpdateExplore(float deltaTime, float elapsedTime, float progress)
	{
	}

	protected virtual void OnUpdateCrack(float deltaTime, float elapsedTime, float progress)
	{
	}
}
