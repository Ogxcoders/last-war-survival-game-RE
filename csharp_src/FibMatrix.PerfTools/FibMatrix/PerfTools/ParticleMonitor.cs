using UnityEngine;

namespace FibMatrix.PerfTools;

[ExecuteAlways]
public class ParticleMonitor : MonoBehaviour
{
	private CDParticleMonitor m_Config;

	private int m_ParticleCount;

	private int m_ParticlePeakValue;

	private int m_ParticleSystemCountPlaying;

	private int m_ParticleSystemCountNotPlaying;

	private int m_ParticleSystemCountPrefab;

	private float m_IntervalTimeAcc;

	public int particleCount => m_ParticleCount;

	public int particlePeakValue => m_ParticlePeakValue;

	public int particleSystemCountPlaying => m_ParticleSystemCountPlaying;

	public int particleSystemCountNotPlaying => m_ParticleSystemCountNotPlaying;

	public int particleSystemCountPrefab => m_ParticleSystemCountPrefab;

	public static ParticleMonitor GetMonitor(Transform parent, CDParticleMonitor config)
	{
		string n = "ParticleMonitor";
		GameObject gameObject;
		if (parent == null)
		{
			gameObject = GameObject.Find(n);
			if (gameObject == null)
			{
				gameObject = new GameObject(n, typeof(ParticleMonitor));
			}
		}
		else
		{
			Transform transform = parent.Find(n);
			if (transform == null)
			{
				gameObject = new GameObject(n, typeof(ParticleMonitor));
				gameObject.transform.SetParent(parent, worldPositionStays: true);
			}
			else
			{
				gameObject = transform.gameObject;
			}
		}
		ParticleMonitor particleMonitor = gameObject.GetComponent<ParticleMonitor>();
		if (particleMonitor == null)
		{
			particleMonitor = gameObject.AddComponent<ParticleMonitor>();
		}
		particleMonitor.m_Config = config;
		return particleMonitor;
	}

	public void Awake()
	{
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		if (!Application.isPlaying)
		{
			base.gameObject.hideFlags = HideFlags.DontSave;
		}
	}

	private void LateUpdate()
	{
		DoCount();
	}

	public void Dispose()
	{
		if (this != null)
		{
			Object.DestroyImmediate(base.gameObject);
		}
	}

	private void DoCount()
	{
		m_IntervalTimeAcc += Time.deltaTime;
		float num = ((m_Config == null) ? 0.5f : m_Config.sampleInterval);
		if (!(m_IntervalTimeAcc > num))
		{
			return;
		}
		m_IntervalTimeAcc -= num;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		ParticleSystem[] array = Resources.FindObjectsOfTypeAll<ParticleSystem>();
		if (array != null)
		{
			ParticleSystem[] array2 = array;
			foreach (ParticleSystem particleSystem in array2)
			{
				if (false)
				{
					num5++;
				}
				else if (particleSystem.gameObject.activeInHierarchy)
				{
					if (!particleSystem.isStopped)
					{
						num2 += particleSystem.particleCount;
						num3++;
					}
					else
					{
						num4++;
					}
				}
				else
				{
					num4++;
				}
			}
		}
		m_ParticleCount = num2;
		if (m_ParticleCount > m_ParticlePeakValue)
		{
			m_ParticlePeakValue = m_ParticleCount;
		}
		m_ParticleSystemCountPlaying = num3;
		m_ParticleSystemCountNotPlaying = num4;
		m_ParticleSystemCountPrefab = num5;
	}
}
