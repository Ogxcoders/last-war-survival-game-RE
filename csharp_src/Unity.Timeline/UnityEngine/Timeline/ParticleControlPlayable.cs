using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline;

public class ParticleControlPlayable : PlayableBehaviour
{
	private const float kUnsetTime = float.MaxValue;

	private float m_LastPlayableTime = float.MaxValue;

	private float m_LastParticleTime = float.MaxValue;

	private uint m_RandomSeed = 1u;

	public ParticleSystem particleSystem { get; private set; }

	public static ScriptPlayable<ParticleControlPlayable> Create(PlayableGraph graph, ParticleSystem component, uint randomSeed)
	{
		if (component == null)
		{
			return ScriptPlayable<ParticleControlPlayable>.Null;
		}
		ScriptPlayable<ParticleControlPlayable> result = ScriptPlayable<ParticleControlPlayable>.Create(graph);
		result.GetBehaviour().Initialize(component, randomSeed);
		return result;
	}

	public void Initialize(ParticleSystem ps, uint randomSeed)
	{
		m_RandomSeed = Math.Max(1u, randomSeed);
		particleSystem = ps;
		SetRandomSeed(particleSystem, m_RandomSeed);
	}

	private static void SetRandomSeed(ParticleSystem particleSystem, uint randomSeed)
	{
		if (!(particleSystem == null))
		{
			particleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
			if (particleSystem.useAutoRandomSeed)
			{
				particleSystem.useAutoRandomSeed = false;
				particleSystem.randomSeed = randomSeed;
			}
			for (int i = 0; i < particleSystem.subEmitters.subEmittersCount; i++)
			{
				SetRandomSeed(particleSystem.subEmitters.GetSubEmitterSystem(i), ++randomSeed);
			}
		}
	}

	public override void PrepareFrame(Playable playable, FrameData data)
	{
		if (particleSystem == null || !particleSystem.gameObject.activeInHierarchy)
		{
			m_LastPlayableTime = float.MaxValue;
			return;
		}
		float num = (float)playable.GetTime();
		float time = particleSystem.time;
		if (m_LastPlayableTime > num || !Mathf.Approximately(time, m_LastParticleTime))
		{
			Simulate(num, restart: true);
		}
		else if (m_LastPlayableTime < num)
		{
			Simulate(num - m_LastPlayableTime, restart: false);
		}
		m_LastPlayableTime = num;
		m_LastParticleTime = particleSystem.time;
	}

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		m_LastPlayableTime = float.MaxValue;
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		m_LastPlayableTime = float.MaxValue;
	}

	private void Simulate(float time, bool restart)
	{
		float maximumDeltaTime = Time.maximumDeltaTime;
		if (restart)
		{
			particleSystem.Simulate(0f, withChildren: false, restart: true, fixedTimeStep: false);
		}
		while (time > maximumDeltaTime)
		{
			particleSystem.Simulate(maximumDeltaTime, withChildren: false, restart: false, fixedTimeStep: false);
			time -= maximumDeltaTime;
		}
		if (time > 0f)
		{
			particleSystem.Simulate(time, withChildren: false, restart: false, fixedTimeStep: false);
		}
	}
}
