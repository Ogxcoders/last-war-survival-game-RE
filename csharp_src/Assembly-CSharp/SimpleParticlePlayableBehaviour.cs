using UnityEngine;
using UnityEngine.Playables;

public class SimpleParticlePlayableBehaviour : PlayableBehaviour
{
	public GameObject target;

	private ParticleSystem[] _particleSystems;

	public override void OnPlayableCreate(Playable playable)
	{
		Initialize();
	}

	private void Initialize()
	{
		if (_particleSystems == null && !(target == null))
		{
			_particleSystems = target.GetComponentsInChildren<ParticleSystem>();
		}
	}

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		Initialize();
		if (_particleSystems != null)
		{
			target.SetActive(value: true);
			double time = playable.GetTime();
			for (int i = 0; i < _particleSystems.Length; i++)
			{
				ParticleSystem obj = _particleSystems[i];
				obj.Simulate((float)time, withChildren: false, restart: true);
				obj.Play(withChildren: false);
			}
		}
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		Initialize();
		if (_particleSystems != null)
		{
			for (int i = 0; i < _particleSystems.Length; i++)
			{
				_ = (bool)_particleSystems[i];
				_particleSystems[i].Pause(withChildren: false);
			}
			if (target != null)
			{
				target.SetActive(value: false);
			}
		}
	}

	public override void OnGraphStop(Playable playable)
	{
		if (_particleSystems == null)
		{
			return;
		}
		for (int i = 0; i < _particleSystems.Length; i++)
		{
			if (_particleSystems[i] != null)
			{
				_particleSystems[i].Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
	}
}
