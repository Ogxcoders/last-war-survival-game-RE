using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering;

public class SRPBatcherProfiler : MonoBehaviour
{
	internal class RecorderEntry
	{
		public string name;

		public string oldName;

		public int callCount;

		public float accTime;

		public Recorder recorder;
	}

	private enum SRPBMarkers
	{
		kStdRenderDraw,
		kStdShadowDraw,
		kSRPBRenderDraw,
		kSRPBShadowDraw,
		kRenderThreadIdle,
		kStdRenderApplyShader,
		kStdShadowApplyShader,
		kSRPBRenderApplyShader,
		kSRPBShadowApplyShader,
		kPrepareBatchRendererGroupNodes
	}

	public bool m_Enable = true;

	private const float kAverageStatDuration = 1f;

	private int m_frameCount;

	private float m_AccDeltaTime;

	private string m_statsLabel;

	private GUIStyle m_style;

	private bool m_oldBatcherEnable;

	private RecorderEntry[] recordersList = new RecorderEntry[10]
	{
		new RecorderEntry
		{
			name = "RenderLoop.Draw"
		},
		new RecorderEntry
		{
			name = "Shadows.Draw"
		},
		new RecorderEntry
		{
			name = "SRPBatcher.Draw",
			oldName = "RenderLoopNewBatcher.Draw"
		},
		new RecorderEntry
		{
			name = "SRPBatcherShadow.Draw",
			oldName = "ShadowLoopNewBatcher.Draw"
		},
		new RecorderEntry
		{
			name = "RenderLoopDevice.Idle"
		},
		new RecorderEntry
		{
			name = "StdRender.ApplyShader"
		},
		new RecorderEntry
		{
			name = "StdShadow.ApplyShader"
		},
		new RecorderEntry
		{
			name = "SRPBRender.ApplyShader"
		},
		new RecorderEntry
		{
			name = "SRPBShadow.ApplyShader"
		},
		new RecorderEntry
		{
			name = "PrepareBatchRendererGroupNodes"
		}
	};

	private void Awake()
	{
		for (int i = 0; i < recordersList.Length; i++)
		{
			Sampler sampler = Sampler.Get(recordersList[i].name);
			if (sampler.isValid)
			{
				recordersList[i].recorder = sampler.GetRecorder();
			}
			else if (recordersList[i].oldName != null)
			{
				sampler = Sampler.Get(recordersList[i].oldName);
				if (sampler.isValid)
				{
					recordersList[i].recorder = sampler.GetRecorder();
				}
			}
		}
		m_style = new GUIStyle();
		m_style.fontSize = 15;
		m_style.normal.textColor = Color.white;
		m_oldBatcherEnable = m_Enable;
		ResetStats();
	}

	private void RazCounters()
	{
		m_AccDeltaTime = 0f;
		m_frameCount = 0;
		for (int i = 0; i < recordersList.Length; i++)
		{
			recordersList[i].accTime = 0f;
			recordersList[i].callCount = 0;
		}
	}

	private void ResetStats()
	{
		m_statsLabel = "Gathering data...";
		RazCounters();
	}

	public void ToggleStats()
	{
		m_Enable = !m_Enable;
		ResetStats();
	}

	public void HideStats()
	{
		m_Enable = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F7))
		{
			GraphicsSettings.useScriptableRenderPipelineBatching = !GraphicsSettings.useScriptableRenderPipelineBatching;
		}
		if (GraphicsSettings.useScriptableRenderPipelineBatching != m_oldBatcherEnable)
		{
			ResetStats();
			m_oldBatcherEnable = GraphicsSettings.useScriptableRenderPipelineBatching;
		}
		if (Input.GetKeyDown(KeyCode.F8))
		{
			ToggleStats();
		}
		if (!m_Enable)
		{
			return;
		}
		bool useScriptableRenderPipelineBatching = GraphicsSettings.useScriptableRenderPipelineBatching;
		m_AccDeltaTime += Time.unscaledDeltaTime;
		m_frameCount++;
		for (int i = 0; i < recordersList.Length; i++)
		{
			if (recordersList[i].recorder != null)
			{
				recordersList[i].accTime += (float)recordersList[i].recorder.elapsedNanoseconds / 1000000f;
				recordersList[i].callCount += recordersList[i].recorder.sampleBlockCount;
			}
		}
		if (m_AccDeltaTime >= 1f)
		{
			float num = 1f / (float)m_frameCount;
			float num2 = recordersList[0].accTime * num;
			float num3 = recordersList[1].accTime * num;
			float num4 = recordersList[2].accTime * num;
			float num5 = recordersList[3].accTime * num;
			float num6 = recordersList[4].accTime * num;
			float num7 = recordersList[9].accTime * num;
			m_statsLabel = $"Accumulated time for RenderLoop.Draw and ShadowLoop.Draw (all threads)\n{num2 + num3 + num4 + num5 + num7:F2}ms CPU Rendering time ( incl {num6:F2}ms RT idle )\n";
			if (useScriptableRenderPipelineBatching)
			{
				m_statsLabel += $"  {num4 + num5:F2}ms SRP Batcher code path\n";
				m_statsLabel += $"    {num4:F2}ms All objects ( {recordersList[7].callCount / m_frameCount} ApplyShader calls )\n";
				m_statsLabel += $"    {num5:F2}ms Shadows ( {recordersList[8].callCount / m_frameCount} ApplyShader calls )\n";
			}
			m_statsLabel += $"  {num2 + num3:F2}ms Standard code path\n";
			m_statsLabel += $"    {num2:F2}ms All objects ( {recordersList[5].callCount / m_frameCount} ApplyShader calls )\n";
			m_statsLabel += $"    {num3:F2}ms Shadows ( {recordersList[6].callCount / m_frameCount} ApplyShader calls )\n";
			m_statsLabel += $"  {num7:F2}ms PIR Prepare Group Nodes ( {recordersList[9].callCount / m_frameCount} calls )\n";
			m_statsLabel += $"Global Main Loop: {m_AccDeltaTime * 1000f * num:F2}ms ({(int)((float)m_frameCount / m_AccDeltaTime)} FPS)\n";
			RazCounters();
		}
	}

	private void OnGUI()
	{
		float num = 50f;
		if (m_Enable)
		{
			bool useScriptableRenderPipelineBatching = GraphicsSettings.useScriptableRenderPipelineBatching;
			GUI.color = new Color(1f, 1f, 1f, 1f);
			float width = 700f;
			float num2 = 256f;
			num += num2 + 50f;
			if (useScriptableRenderPipelineBatching)
			{
				GUILayout.BeginArea(new Rect(32f, 50f, width, num2), "SRP batcher ON (F7)", GUI.skin.window);
			}
			else
			{
				GUILayout.BeginArea(new Rect(32f, 50f, width, num2), "SRP batcher OFF (F7)", GUI.skin.window);
			}
			GUILayout.Label(m_statsLabel, m_style);
			GUILayout.EndArea();
		}
	}
}
