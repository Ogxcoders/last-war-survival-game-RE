using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Audio;

public class G_AudioText : MonoBehaviour
{
	[SerializeField]
	private Text m_DBText;

	private GraphyManager m_graphyManager;

	private G_AudioMonitor m_audioMonitor;

	private int m_updateRate = 4;

	private float m_deltaTimeOffset;

	private void Awake()
	{
		Init();
	}

	private void Update()
	{
		if (m_audioMonitor.SpectrumDataAvailable)
		{
			if (m_deltaTimeOffset > 1f / (float)m_updateRate)
			{
				m_deltaTimeOffset = 0f;
				m_DBText.text = Mathf.Clamp(m_audioMonitor.MaxDB, -80f, 0f).ToStringNonAlloc();
			}
			else
			{
				m_deltaTimeOffset += Time.deltaTime;
			}
		}
	}

	public void UpdateParameters()
	{
		m_updateRate = m_graphyManager.AudioTextUpdateRate;
	}

	private void Init()
	{
		if (!G_FloatString.Inited || G_FloatString.MinValue > -1000f || G_FloatString.MaxValue < 16384f)
		{
			G_FloatString.Init(-1001f, 16386f);
		}
		m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
		m_audioMonitor = GetComponent<G_AudioMonitor>();
		UpdateParameters();
	}
}
