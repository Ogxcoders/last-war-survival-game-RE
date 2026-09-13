using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
	[SerializeField]
	private float length;

	[SerializeField]
	private ParticleSystem particleSystem;

	private float progress = -1f;

	public float Progress
	{
		get
		{
			return progress;
		}
		set
		{
			float a = Mathf.Clamp01(value);
			if (!Mathf.Approximately(a, progress))
			{
				progress = a;
				float t = progress * length;
				if (particleSystem != null)
				{
					particleSystem.Simulate(t, withChildren: true, restart: true, fixedTimeStep: true);
				}
			}
		}
	}

	private void Awake()
	{
		if (particleSystem != null)
		{
			particleSystem.Clear();
			particleSystem.playOnAwake = false;
		}
	}
}
