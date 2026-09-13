using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.PerfTools;

public class ProfilingChartRenderer_SummaryOverdraw : ProfilingChartRenderer
{
	private List<OverdrawMonitor> m_Monitors;

	private GUIStyle m_ChartStyle;

	public ProfilingChartRenderer_SummaryOverdraw(string name, CDChart chart, CDChartAppearance configAppearance, List<OverdrawMonitor> monitors)
		: base(name, chart, configAppearance)
	{
		m_Monitors = monitors;
		m_ChartStyle = new GUIStyle("Label")
		{
			normal = new GUIStyleState()
		};
		m_ChartStyle.alignment = TextAnchor.MiddleCenter;
	}

	public override void UninitializeRenderer()
	{
		m_Monitors = null;
		base.UninitializeRenderer();
	}

	public override void DoUpdateFrame()
	{
	}

	public override void DoRecord()
	{
		if (m_Monitors == null)
		{
			return;
		}
		float num = 0f;
		foreach (OverdrawMonitor monitor in m_Monitors)
		{
			num += monitor.LastFrameOverdraw;
		}
		RecordChartValue(num);
	}

	public override void DoGUI(bool fixedFontSize, bool foldout)
	{
		ProfilingChartRenderer.CalculateSizes(fixedFontSize, foldout, out var _, out var chartWidth, out var chartHeight);
		GUILayout.BeginVertical();
		if (foldout && m_RenderTextureChart != null)
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
		GUILayout.EndVertical();
	}
}
