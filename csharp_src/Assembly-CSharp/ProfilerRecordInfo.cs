using System.Collections.Generic;
using System.Text;
using GameKit.Base;
using UnityEngine;
using UnityEngine.Profiling;

public class ProfilerRecordInfo : SingletonBehaviour<ProfilerRecordInfo>
{
	public class RecorderEntry
	{
		private Recorder m_recorder;

		private float m_accTime;

		private float m_accValue;

		private float m_maxValue;

		private float m_minValue;

		private int m_sampleCount;

		private int m_accSampleBlockCount;

		private float m_updateInterval;

		private float m_lastUpdateTime;

		private string m_name;

		private string m_status;

		public float averageValue { get; private set; }

		public int averageSampleCount { get; private set; }

		public float maxValue => m_maxValue;

		public float minValue => m_minValue;

		public string name => m_name;

		public RecorderEntry(string recorderName, float updateInterval = 1f)
		{
			m_name = recorderName;
			Sampler sampler = Sampler.Get(recorderName);
			if (sampler.isValid)
			{
				m_recorder = sampler.GetRecorder();
				m_updateInterval = updateInterval;
				m_lastUpdateTime = Time.unscaledTime;
				m_minValue = float.MaxValue;
				m_maxValue = float.MinValue;
			}
			else
			{
				Debug.LogError("Failed to get sampler for " + recorderName);
			}
		}

		public void Update()
		{
			if (m_recorder != null && m_recorder.isValid)
			{
				float unscaledTime = Time.unscaledTime;
				if (unscaledTime - m_lastUpdateTime >= m_updateInterval)
				{
					UpdateStatsText();
					averageValue = ((m_sampleCount > 0) ? (m_accValue / (float)m_sampleCount) : 0f);
					averageSampleCount = ((m_accSampleBlockCount > 0) ? (m_accSampleBlockCount / m_sampleCount) : 0);
					m_accValue = 0f;
					m_accSampleBlockCount = 0;
					m_maxValue = float.MinValue;
					m_minValue = float.MaxValue;
					m_sampleCount = 0;
					m_lastUpdateTime = unscaledTime;
				}
				float num = (float)m_recorder.elapsedNanoseconds / 1000000f;
				m_accValue += num;
				m_accSampleBlockCount += m_recorder.sampleBlockCount;
				m_sampleCount++;
				m_maxValue = Mathf.Max(m_maxValue, num);
				m_minValue = Mathf.Min(m_minValue, num);
			}
		}

		public void Reset()
		{
			m_accTime = 0f;
			m_accValue = 0f;
			m_maxValue = float.MinValue;
			m_minValue = float.MaxValue;
			m_sampleCount = 0;
			averageValue = 0f;
			m_lastUpdateTime = Time.unscaledTime;
		}

		public string GetStatsText()
		{
			return m_status;
		}

		public void UpdateStatsText()
		{
			m_status = $"{m_name}{{max:{maxValue:F2} min:{minValue:F2} avg:{averageValue:F2} count:{averageSampleCount} }}";
		}
	}

	private Dictionary<string, RecorderEntry> m_recorders = new Dictionary<string, RecorderEntry>();

	private StringBuilder m_statsBuilder = new StringBuilder(1024);

	private float m_lastStatsUpdateTime;

	private const float STATS_UPDATE_INTERVAL = 1f;

	private string m_cachedStatsText;

	private readonly Rect _dragRect = new Rect(0f, 0f, float.MaxValue, 25f);

	private Rect _windowRect = new Rect(10f, 10f, 60f, 60f);

	public bool EnableDebugWindow { get; set; }

	public void Awake()
	{
		UpdateWindowRect();
	}

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
	}

	public void OnGUI()
	{
		if (EnableDebugWindow && m_recorders.Count != 0)
		{
			_windowRect = GUILayout.Window(GetInstanceID(), _windowRect, DrawWindow, "<b>Profiler Record Info</b>");
		}
	}

	private void UpdateWindowRect()
	{
		_windowRect = new Rect(20f, Screen.height - 40 - m_recorders.Count * 22, Screen.width - 40, 60f);
	}

	private void DrawWindow(int windowId)
	{
		GUI.DragWindow(_dragRect);
		GUILayout.Label(GetStatsText());
	}

	public void AddRecorder(string recorderName, float updateInterval = 1f)
	{
		if (!m_recorders.ContainsKey(recorderName))
		{
			m_recorders[recorderName] = new RecorderEntry(recorderName, updateInterval);
		}
	}

	public void AddDefaultRecorders()
	{
		string[] array = new string[10] { "PlayerLoop", "Update.DirectorUpdate", "BehaviourUpdate", "Director.PrepareFrame", "Director.ProcessFrame", "ParticleSystem.Update", "StdRender.ApplyShader", "SRPBRender.ApplyShader", "MeshSkinning.Skin", "ParticleSystem.UpdateJob" };
		foreach (string recorderName in array)
		{
			AddRecorder(recorderName);
		}
	}

	public void RemoveRecorder(string recorderName)
	{
		if (m_recorders.ContainsKey(recorderName))
		{
			m_recorders.Remove(recorderName);
		}
	}

	public void SetRecorders(List<string> recorderNames)
	{
		m_recorders.Clear();
		foreach (string recorderName in recorderNames)
		{
			AddRecorder(recorderName);
		}
		UpdateWindowRect();
	}

	private void Update()
	{
		if (m_recorders.Count == 0)
		{
			return;
		}
		foreach (RecorderEntry value in m_recorders.Values)
		{
			value.Update();
		}
		float unscaledTime = Time.unscaledTime;
		if (unscaledTime - m_lastStatsUpdateTime >= 1f)
		{
			UpdateStatsText();
			m_lastStatsUpdateTime = unscaledTime;
		}
	}

	private void UpdateStatsText()
	{
		m_statsBuilder.Clear();
		foreach (RecorderEntry value in m_recorders.Values)
		{
			m_statsBuilder.AppendLine(value.GetStatsText());
		}
		m_cachedStatsText = m_statsBuilder.ToString();
	}

	public string GetStatsText()
	{
		return m_cachedStatsText;
	}

	public void ResetAll()
	{
		foreach (RecorderEntry value in m_recorders.Values)
		{
			value.Reset();
		}
		m_cachedStatsText = string.Empty;
		m_lastStatsUpdateTime = Time.unscaledTime;
	}
}
