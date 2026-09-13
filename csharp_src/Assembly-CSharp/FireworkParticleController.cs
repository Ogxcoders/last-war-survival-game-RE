using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FireworkParticleController : MonoBehaviour
{
	[Tooltip("总时间")]
	private float totalDuration = 30f;

	[Tooltip("单次时间")]
	private float singleDuration = 10f;

	[Range(0f, 1f)]
	private float initialProgress;

	private ParticleSystem[] particleSystems;

	private float totalElapsedTime;

	private bool isReady;

	private void Reset()
	{
		CollectParticleSystems();
	}

	private void Awake()
	{
		if (particleSystems == null || particleSystems.Length == 0)
		{
			CollectParticleSystems();
		}
	}

	private void OnEnable()
	{
		isReady = true;
	}

	private void Update()
	{
		if (isReady && !(totalDuration <= 0f) && !(totalElapsedTime > totalDuration))
		{
			float previousTime = totalElapsedTime;
			totalElapsedTime += Time.deltaTime;
			HandleLoopProgress(previousTime, totalElapsedTime);
		}
	}

	public void Configure(float duration, float cycleTime, float normalizedProgress)
	{
		totalDuration = Mathf.Max(duration, Mathf.Epsilon);
		singleDuration = Mathf.Max(cycleTime, Mathf.Epsilon);
		initialProgress = Mathf.Clamp01(normalizedProgress);
		ApplyProgress(initialProgress);
	}

	private void ApplyProgress(float normalizedProgress)
	{
		totalElapsedTime = normalizedProgress * totalDuration;
		AlignParticleSystems(normalizedProgress);
		PlayAll();
	}

	private void AlignParticleSystems(float normalizedProgress)
	{
		if (particleSystems == null)
		{
			return;
		}
		float t = normalizedProgress * totalDuration;
		ParticleSystem[] array = particleSystems;
		foreach (ParticleSystem particleSystem in array)
		{
			if (!(particleSystem == null))
			{
				float t2 = Mathf.Repeat(t, singleDuration);
				particleSystem.Simulate(t2, withChildren: true, restart: true, fixedTimeStep: true);
			}
		}
	}

	private void PlayAll()
	{
		for (int i = 0; i < particleSystems.Length; i++)
		{
			ParticleSystem particleSystem = particleSystems[i];
			if (particleSystem == null)
			{
				break;
			}
			particleSystem.Play(withChildren: true);
		}
	}

	private void CollectParticleSystems()
	{
		List<ParticleSystem> list = new List<ParticleSystem>();
		TraverseAndCollect(base.transform, list);
		particleSystems = list.ToArray();
	}

	private void HandleLoopProgress(float previousTime, float currentTime)
	{
		if (particleSystems == null)
		{
			return;
		}
		ParticleSystem[] array = particleSystems;
		foreach (ParticleSystem particleSystem in array)
		{
			if (particleSystem == null)
			{
				continue;
			}
			int num = Mathf.FloorToInt(previousTime / singleDuration);
			if (Mathf.FloorToInt(currentTime / singleDuration) != num)
			{
				particleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
				if (currentTime < totalDuration)
				{
					particleSystem.Play(withChildren: true);
				}
			}
		}
	}

	private void TraverseAndCollect(Transform node, List<ParticleSystem> collector)
	{
		if (node == null)
		{
			return;
		}
		ParticleSystem component = node.GetComponent<ParticleSystem>();
		if (component != null)
		{
			collector.Add(component);
			return;
		}
		int childCount = node.childCount;
		for (int i = 0; i < childCount; i++)
		{
			TraverseAndCollect(node.GetChild(i), collector);
		}
	}
}
