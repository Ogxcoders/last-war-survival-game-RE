using UnityEngine;

public class BattleUtil
{
	public static void ReSetParticleSystems(ParticleSystem[] particles)
	{
		for (int i = 0; i < particles.Length; i++)
		{
			if (particles[i] != null)
			{
				particles[i].Simulate(0f);
				particles[i].Play();
			}
		}
	}
}
