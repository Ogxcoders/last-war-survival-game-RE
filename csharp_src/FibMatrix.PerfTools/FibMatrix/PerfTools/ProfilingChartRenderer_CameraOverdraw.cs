using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FibMatrix.PerfTools;

public class ProfilingChartRenderer_CameraOverdraw : ProfilingChartRenderer
{
	private CDOverdrawChart m_ODConfig;

	private OverdrawMonitor m_Monitor;

	private string m_CameraName;

	private string m_CameraNameSimplified;

	private int m_veryHighFrameCount;

	private StringBuilder m_TextBuilder;

	private Color m_ColorForOptimization = Color.white;

	private GUIStyle m_TextStyle;

	private GUIStyle m_ChartStyle;

	public static CDOverdrawChart FetchConfig(CDOverdrawChart configDefault, List<CDOverdrawChart> configCustoms, string cameraName)
	{
		CDOverdrawChart cDOverdrawChart = null;
		if (configCustoms != null)
		{
			foreach (CDOverdrawChart configCustom in configCustoms)
			{
				if (!(configCustom.cameraName != cameraName))
				{
					cDOverdrawChart = configCustom;
				}
			}
		}
		if (cDOverdrawChart == null)
		{
			cDOverdrawChart = configDefault;
		}
		return cDOverdrawChart;
	}

	public ProfilingChartRenderer_CameraOverdraw(string name, CDOverdrawChart config, CDChartAppearance configAppearance, OverdrawMonitor monitor)
		: base(name, config.chart, configAppearance)
	{
		m_Monitor = monitor;
		m_CameraName = ((m_Monitor != null) ? m_Monitor.GetTargetCameraName() : "Unknow");
		m_ODConfig = config;
		int num = m_CameraName.ToLower().IndexOf("camera");
		m_CameraNameSimplified = ((num > 0) ? m_CameraName.Substring(0, num) : m_CameraName);
		m_CameraNameSimplified = m_CameraNameSimplified.Trim();
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

	public bool HasSameMontor(OverdrawMonitor monitor)
	{
		if (m_Monitor != null)
		{
			return monitor == m_Monitor;
		}
		return false;
	}

	public bool IsMonitorValid()
	{
		return m_Monitor != null;
	}

	public override void UninitializeRenderer()
	{
		m_Monitor = null;
		base.UninitializeRenderer();
	}

	public override void DoUpdateFrame()
	{
		float num = ((m_Monitor != null) ? m_Monitor.LastFrameOverdraw : 0f);
		float veryHighValue = m_ODConfig.chart.veryHighValue;
		if (num >= veryHighValue)
		{
			m_veryHighFrameCount++;
		}
	}

	public override void DoRecord()
	{
		if (!(m_Monitor == null))
		{
			float lastFrameOverdraw = m_Monitor.LastFrameOverdraw;
			RecordChartValue(lastFrameOverdraw);
		}
	}

	public override void DoGUI(bool fixedFontSize, bool foldout)
	{
		ProfilingChartRenderer.CalculateSizes(fixedFontSize, foldout, out var fontSize, out var chartWidth, out var chartHeight);
		m_TextStyle.fontSize = fontSize;
		GUILayoutOption gUILayoutOption = GUILayout.Height(m_TextStyle.lineHeight + (float)m_TextStyle.margin.bottom);
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		if (!foldout)
		{
			string text = GetCameraText();
			m_TextStyle.normal.textColor = Color.white;
			GUILayout.Label(text, m_TextStyle, gUILayoutOption);
			GetVeryHighFrameCountTextParams(ref text, ref m_ColorForOptimization);
			m_TextStyle.normal.textColor = m_ColorForOptimization;
			GUILayout.Label(text, m_TextStyle, gUILayoutOption);
		}
		GUILayout.EndHorizontal();
		if (!foldout && m_RenderTextureChart != null)
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
		}
		else
		{
			GUILayout.BeginHorizontal();
			string text2 = "";
			GetCurTextParams(ref text2, ref m_ColorForOptimization);
			m_TextStyle.normal.textColor = m_ColorForOptimization;
			GUILayout.Label(text2, m_TextStyle, gUILayoutOption);
			string text3 = "";
			GetCurPeakValueTextParams(ref text3, ref m_ColorForOptimization);
			m_TextStyle.normal.textColor = m_ColorForOptimization;
			GUILayout.Label(text3, m_TextStyle, gUILayoutOption);
			string text4 = "";
			GetHistoryPeakValueTextParams(ref text4, ref m_ColorForOptimization);
			m_TextStyle.normal.textColor = m_ColorForOptimization;
			GUILayout.Label(text4, m_TextStyle, gUILayoutOption);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	private string GetCameraText()
	{
		if (m_TextBuilder == null)
		{
			m_TextBuilder = new StringBuilder(512);
		}
		m_TextBuilder.Clear();
		string cameraName = m_CameraName;
		m_TextBuilder.AppendLine(cameraName ?? "");
		return m_TextBuilder.ToString();
	}

	private void GetVeryHighFrameCountTextParams(ref string text, ref Color textColor)
	{
		m_TextBuilder.Clear();
		int veryHighFrameCount = m_veryHighFrameCount;
		m_TextBuilder.AppendLine($"极高消耗帧数：{veryHighFrameCount}");
		text = m_TextBuilder.ToString();
		if (veryHighFrameCount >= m_ODConfig.veryHighFrameCountThresholdOverdraw)
		{
			textColor = ProfilingChartRenderer.s_VeryHighColor;
		}
		else
		{
			textColor = ProfilingChartRenderer.s_LowColor;
		}
		textColor.a = 1f;
	}

	private string GetRichtextString()
	{
		if (m_TextBuilder == null)
		{
			m_TextBuilder = new StringBuilder(512);
		}
		m_TextBuilder.Clear();
		string cameraNameSimplified = m_CameraNameSimplified;
		float num = ((m_Monitor != null) ? m_Monitor.LastFrameOverdraw : 0f);
		float num2 = base.maxRecordedPeakValue;
		float num3 = ((m_Monitor != null) ? m_Monitor.MaxOverdraw : 0f);
		string levelRichtextColorString = GetLevelRichtextColorString(num);
		string levelRichtextColorString2 = GetLevelRichtextColorString(num2);
		string levelRichtextColorString3 = GetLevelRichtextColorString(num3);
		m_TextBuilder.AppendLine(cameraNameSimplified + ":" + $"<color={levelRichtextColorString}>{num:F1}</color>" + $"/<color={levelRichtextColorString2}>{num2:F1}</color>" + $"/<color={levelRichtextColorString3}>{num3:F1}</color>");
		return m_TextBuilder.ToString();
	}

	private void GetCurTextParams(ref string text, ref Color textColor)
	{
		if (m_TextBuilder == null)
		{
			m_TextBuilder = new StringBuilder(512);
		}
		m_TextBuilder.Clear();
		float num = ((m_Monitor != null) ? m_Monitor.LastFrameOverdraw : 0f);
		m_TextBuilder.AppendLine($"当前：{num:F1}");
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
		float num = base.maxRecordedPeakValue;
		m_TextBuilder.AppendLine($"峰值：{num:F1}");
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
		float num = ((m_Monitor != null) ? m_Monitor.MaxOverdraw : 0f);
		m_TextBuilder.AppendLine($"历史峰值：{num:F1}");
		text = m_TextBuilder.ToString();
		GetLevelColor(num, ref textColor);
	}
}
