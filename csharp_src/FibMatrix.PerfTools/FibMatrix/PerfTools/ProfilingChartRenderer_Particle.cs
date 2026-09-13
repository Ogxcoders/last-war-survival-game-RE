using System.Text;
using UnityEngine;

namespace FibMatrix.PerfTools;

public class ProfilingChartRenderer_Particle : ProfilingChartRenderer
{
	private ParticleMonitor m_Monitor;

	private Color m_ColorForOptimization = Color.white;

	private GUIStyle m_TextStyle;

	private GUIStyle m_ChartStyle;

	private StringBuilder m_TextBuilder;

	public ProfilingChartRenderer_Particle(string name, CDChart chart, CDChartAppearance configAppearance, ParticleMonitor monitor)
		: base(name, chart, configAppearance)
	{
		m_Monitor = monitor;
		m_TextStyle = new GUIStyle("Label")
		{
			normal = new GUIStyleState()
		};
		m_ChartStyle = new GUIStyle("Label")
		{
			normal = new GUIStyleState()
		};
		m_TextStyle.fontStyle = FontStyle.Bold;
		m_ChartStyle.alignment = TextAnchor.MiddleCenter;
	}

	public override void UninitializeRenderer()
	{
		m_Monitor = null;
		base.UninitializeRenderer();
	}

	public override void DoUpdateFrame()
	{
	}

	public override void DoRecord()
	{
		if (!(m_Monitor == null))
		{
			float v = m_Monitor.particleCount;
			RecordChartValue(v);
		}
	}

	public override void DoGUI(bool fixedFontSize, bool foldout)
	{
		ProfilingChartRenderer.CalculateSizes(fixedFontSize, foldout, out var fontSize, out var chartWidth, out var chartHeight);
		m_TextStyle.fontSize = fontSize;
		GUILayoutOption gUILayoutOption = GUILayout.Height(m_TextStyle.lineHeight + (float)m_TextStyle.margin.bottom);
		GUILayout.BeginVertical();
		if (!foldout)
		{
			GUILayout.BeginHorizontal();
			m_TextStyle.normal.textColor = Color.white;
			GUILayout.Label("粒子", m_TextStyle, gUILayoutOption);
			string particleSystemNotInPlayingCountString = GetParticleSystemNotInPlayingCountString(foldout: false);
			m_TextStyle.normal.textColor = Color.white;
			GUILayout.Label(particleSystemNotInPlayingCountString, m_TextStyle, gUILayoutOption);
			GUILayout.EndHorizontal();
		}
		if (m_RenderTextureChart != null)
		{
			if (chartWidth > 0f)
			{
				GUILayout.Label(m_RenderTextureChart, m_ChartStyle, GUILayout.Width(chartWidth), GUILayout.Height(chartHeight));
			}
			else
			{
				GUILayout.Label(m_RenderTextureChart, m_ChartStyle);
			}
		}
		if (foldout)
		{
			string richtextString = GetRichtextString();
			m_TextStyle.normal.textColor = Color.white;
			GUILayout.Label(richtextString, m_TextStyle, gUILayoutOption);
			string particleSystemNotInPlayingCountString2 = GetParticleSystemNotInPlayingCountString(foldout: true);
			m_TextStyle.normal.textColor = Color.white;
			GUILayout.Label(particleSystemNotInPlayingCountString2, m_TextStyle, gUILayoutOption);
		}
		else
		{
			GUILayout.BeginHorizontal();
			string text = "";
			GetCurTextParams(ref text, ref m_ColorForOptimization);
			m_TextStyle.normal.textColor = m_ColorForOptimization;
			GUILayout.Label(text, m_TextStyle, gUILayoutOption);
			string text2 = "";
			GetCurPeakValueTextParams(ref text2, ref m_ColorForOptimization);
			m_TextStyle.normal.textColor = m_ColorForOptimization;
			GUILayout.Label(text2, m_TextStyle, gUILayoutOption);
			string text3 = "";
			GetHistoryPeakValueTextParams(ref text3, ref m_ColorForOptimization);
			m_TextStyle.normal.textColor = m_ColorForOptimization;
			GUILayout.Label(text3, m_TextStyle, gUILayoutOption);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	private string GetRichtextString()
	{
		if (m_TextBuilder == null)
		{
			m_TextBuilder = new StringBuilder(512);
		}
		m_TextBuilder.Clear();
		int num = ((m_Monitor != null) ? m_Monitor.particleCount : 0);
		int num2 = (int)base.maxRecordedPeakValue;
		int num3 = ((m_Monitor != null) ? m_Monitor.particlePeakValue : 0);
		string levelRichtextColorString = GetLevelRichtextColorString(num);
		string levelRichtextColorString2 = GetLevelRichtextColorString(num2);
		string levelRichtextColorString3 = GetLevelRichtextColorString(num3);
		m_TextBuilder.AppendLine("粒子:" + $"<color={levelRichtextColorString}>{num}</color>" + $"/<color={levelRichtextColorString2}>{num2}</color>" + $"/<color={levelRichtextColorString3}>{num3}</color>");
		return m_TextBuilder.ToString();
	}

	private void GetCurTextParams(ref string text, ref Color textColor)
	{
		if (m_TextBuilder == null)
		{
			m_TextBuilder = new StringBuilder(512);
		}
		m_TextBuilder.Clear();
		int num = ((m_Monitor != null) ? m_Monitor.particleCount : 0);
		m_TextBuilder.AppendLine($"当前：{num}");
		text = m_TextBuilder.ToString();
		GetLevelColor(num, ref textColor);
	}

	private void GetCurPeakValueTextParams(ref string text, ref Color textColor)
	{
		if (m_TextBuilder == null)
		{
			m_TextBuilder = new StringBuilder(512);
		}
		m_TextBuilder.Clear();
		int num = (int)base.maxRecordedPeakValue;
		m_TextBuilder.AppendLine($"峰值：{num}");
		text = m_TextBuilder.ToString();
		GetLevelColor(num, ref textColor);
	}

	private void GetHistoryPeakValueTextParams(ref string text, ref Color textColor)
	{
		if (m_TextBuilder == null)
		{
			m_TextBuilder = new StringBuilder(512);
		}
		m_TextBuilder.Clear();
		int num = ((m_Monitor != null) ? m_Monitor.particlePeakValue : 0);
		m_TextBuilder.AppendLine($"历史峰值：{num}");
		text = m_TextBuilder.ToString();
		GetLevelColor(num, ref textColor);
	}

	private string GetParticleSystemNotInPlayingCountString(bool foldout)
	{
		if (m_TextBuilder == null)
		{
			m_TextBuilder = new StringBuilder(512);
		}
		m_TextBuilder.Clear();
		int num = ((m_Monitor != null) ? m_Monitor.particleSystemCountPlaying : 0);
		int num2 = ((m_Monitor != null) ? m_Monitor.particleSystemCountNotPlaying : 0);
		int num3 = ((m_Monitor != null) ? m_Monitor.particleSystemCountPrefab : 0);
		if (foldout)
		{
			m_TextBuilder.AppendLine($"PS:{num}/{num2}/{num3}");
		}
		else
		{
			m_TextBuilder.AppendLine($"播:{num} 停:{num2} Prefab:{num3}");
		}
		return m_TextBuilder.ToString();
	}
}
