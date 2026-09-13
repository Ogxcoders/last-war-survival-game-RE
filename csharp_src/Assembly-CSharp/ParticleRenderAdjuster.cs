using System.Collections.Generic;
using BitBenderGames;
using UnityEngine;

public class ParticleRenderAdjuster : MonoBehaviourWrapped
{
	public int thresholdXZ = 1;

	public int thresholdY;

	public ParticleSystem[] particles;

	private Vector3 _lastPos;

	public void LateUpdate()
	{
		if (particles.Length == 0)
		{
			return;
		}
		Vector3 position = base.Transform.position;
		bool flag = false;
		if (thresholdXZ > 0)
		{
			flag = Mathf.Abs(_lastPos.x - position.x) > (float)thresholdXZ || Mathf.Abs(_lastPos.z - position.z) > (float)thresholdXZ;
		}
		if (thresholdY > 0)
		{
			flag = flag || Mathf.Abs(_lastPos.y - position.y) > (float)thresholdY;
		}
		if (flag)
		{
			int num = particles.Length;
			for (int i = 0; i < num; i++)
			{
				if (particles[i] != null)
				{
					particles[i].Clear();
				}
			}
		}
		_lastPos = position;
	}

	private void Reset()
	{
		CollectParticles();
	}

	[ContextMenu("自动收集子物体中包含Trails的ParticleSystem")]
	private void CollectParticles()
	{
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		List<ParticleSystem> list = new List<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].trails.enabled)
			{
				list.Add(componentsInChildren[i]);
			}
		}
		particles = list.ToArray();
	}
}
