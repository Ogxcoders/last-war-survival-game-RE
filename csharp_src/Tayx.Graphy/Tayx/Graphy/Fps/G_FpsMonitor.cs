using System.Collections.Generic;
using UnityEngine;

namespace Tayx.Graphy.Fps;

public class G_FpsMonitor : MonoBehaviour
{
	[SerializeField]
	private int m_averageSamples = 200;

	private GraphyManager m_graphyManager;

	private float m_currentFps;

	private float m_avgFps;

	private float m_minFps;

	private float m_maxFps;

	private List<float> m_averageFpsSamples;

	private int m_timeToResetMinMaxFps = 10;

	private float m_timeToResetMinFpsPassed;

	private float m_timeToResetMaxFpsPassed;

	private float unscaledDeltaTime;

	public float CurrentFPS => m_currentFps;

	public float AverageFPS => m_avgFps;

	public float MinFPS => m_minFps;

	public float MaxFPS => m_maxFps;

	private void Awake()
	{
		Init();
	}

	private void Update()
	{
		unscaledDeltaTime = Time.unscaledDeltaTime;
		m_timeToResetMinFpsPassed += unscaledDeltaTime;
		m_timeToResetMaxFpsPassed += unscaledDeltaTime;
		m_currentFps = 1f / unscaledDeltaTime;
		m_avgFps = 0f;
		if (m_averageFpsSamples.Count >= m_averageSamples)
		{
			m_averageFpsSamples.Add(m_currentFps);
			m_averageFpsSamples.RemoveAt(0);
		}
		else
		{
			m_averageFpsSamples.Add(m_currentFps);
		}
		for (int i = 0; i < m_averageFpsSamples.Count; i++)
		{
			m_avgFps += m_averageFpsSamples[i];
		}
		m_avgFps /= m_averageSamples;
		if (m_timeToResetMinMaxFps > 0 && m_timeToResetMinFpsPassed > (float)m_timeToResetMinMaxFps)
		{
			m_minFps = 0f;
			m_timeToResetMinFpsPassed = 0f;
		}
		if (m_timeToResetMinMaxFps > 0 && m_timeToResetMaxFpsPassed > (float)m_timeToResetMinMaxFps)
		{
			m_maxFps = 0f;
			m_timeToResetMaxFpsPassed = 0f;
		}
		if (m_currentFps < m_minFps || m_minFps <= 0f)
		{
			m_minFps = m_currentFps;
			m_timeToResetMinFpsPassed = 0f;
		}
		if (m_currentFps > m_maxFps || m_maxFps <= 0f)
		{
			m_maxFps = m_currentFps;
			m_timeToResetMaxFpsPassed = 0f;
		}
	}

	public void UpdateParameters()
	{
		m_timeToResetMinMaxFps = m_graphyManager.TimeToResetMinMaxFps;
	}

	private void Init()
	{
		m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
		m_averageFpsSamples = new List<float>();
		UpdateParameters();
	}
}
