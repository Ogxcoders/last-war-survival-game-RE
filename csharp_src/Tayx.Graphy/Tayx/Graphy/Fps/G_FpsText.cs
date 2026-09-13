using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Fps;

public class G_FpsText : MonoBehaviour
{
	[SerializeField]
	private Text m_fpsText;

	[SerializeField]
	private Text m_msText;

	[SerializeField]
	private Text m_avgFpsText;

	[SerializeField]
	private Text m_minFpsText;

	[SerializeField]
	private Text m_maxFpsText;

	[SerializeField]
	private Text m_cpuTimeText;

	[SerializeField]
	private Text m_gpuTimeText;

	private GraphyManager m_graphyManager;

	private G_FpsMonitor m_fpsMonitor;

	private int m_updateRate = 4;

	private int m_frameCount;

	private float m_deltaTime;

	private float m_fps;

	private FrameTiming[] frameTimings;

	private const int m_minFps = 0;

	private const int m_maxFps = 10000;

	private const string m_msStringFormat = "0.0";

	private void Awake()
	{
		Init();
	}

	private void Update()
	{
		m_deltaTime += Time.unscaledDeltaTime;
		m_frameCount++;
		FrameTimingManager.CaptureFrameTimings();
		if (m_deltaTime > 1f / (float)m_updateRate)
		{
			m_fps = (float)m_frameCount / m_deltaTime;
			m_fpsText.text = Mathf.RoundToInt(m_fps).ToStringNonAlloc();
			m_msText.text = (m_deltaTime / (float)m_frameCount * 1000f).ToStringNonAlloc("0.0");
			m_minFpsText.text = m_fpsMonitor.MinFPS.ToInt().ToStringNonAlloc();
			SetFpsRelatedTextColor(m_minFpsText, m_fpsMonitor.MinFPS);
			m_maxFpsText.text = m_fpsMonitor.MaxFPS.ToInt().ToStringNonAlloc();
			SetFpsRelatedTextColor(m_maxFpsText, m_fpsMonitor.MaxFPS);
			m_avgFpsText.text = m_fpsMonitor.AverageFPS.ToInt().ToStringNonAlloc();
			SetFpsRelatedTextColor(m_avgFpsText, m_fpsMonitor.AverageFPS);
			FrameTimingManager.GetLatestTimings(1u, frameTimings);
			if (frameTimings.Length != 0)
			{
				m_cpuTimeText.text = ((float)frameTimings[0].cpuFrameTime).ToStringNonAlloc("0.0");
				m_gpuTimeText.text = ((float)frameTimings[0].gpuFrameTime).ToStringNonAlloc("0.0");
			}
			m_deltaTime = 0f;
			m_frameCount = 0;
		}
	}

	public void UpdateParameters()
	{
		m_updateRate = m_graphyManager.FpsTextUpdateRate;
	}

	private void SetFpsRelatedTextColor(Text text, float fps)
	{
		if (fps > (float)m_graphyManager.GoodFPSThreshold)
		{
			text.color = m_graphyManager.GoodFPSColor;
		}
		else if (fps > (float)m_graphyManager.CautionFPSThreshold)
		{
			text.color = m_graphyManager.CautionFPSColor;
		}
		else
		{
			text.color = m_graphyManager.CriticalFPSColor;
		}
	}

	private void Init()
	{
		if (!G_IntString.Inited || G_IntString.MinValue > 0 || G_IntString.MaxValue < 10000)
		{
			G_IntString.Init(0, 10000);
		}
		m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
		m_fpsMonitor = GetComponent<G_FpsMonitor>();
		frameTimings = new FrameTiming[1];
		UpdateParameters();
	}
}
