using System;
using UnityEngine;

namespace FibMatrix.PerfTools;

public abstract class ProfilingChartRenderer
{
	private string m_Name;

	private CDChart m_Config;

	private CDChartAppearance m_ConfigAppearance;

	private const int k_MaxChartValuesLength = 128;

	private float[] m_ChartValues = new float[128];

	private float[] m_RecordedChartValues = new float[128];

	private int m_RecordedChartValueStartIndex;

	private int m_RecordedChartValueValidLength;

	private const int k_OnePeakValueRecordTimes = 10;

	private const int k_MaxRecordedPeakValueLength = 13;

	private int m_PeakValueRecordTimes;

	private float[] m_RecordedPeakValues = new float[13];

	private int m_RecordedPeakValueCurrentIndex;

	private float m_MaxRecordedPeakValue;

	protected RenderTexture m_RenderTextureChart;

	private Material m_MaterialChart;

	protected static readonly Color s_LowColor = new Color(0f, 1f, 0f, 1f);

	protected static readonly Color s_MiddlingColor = new Color(1f, 1f, 0f, 1f);

	protected static readonly Color s_HighColor = new Color(1f, 0.5f, 0f, 1f);

	protected static readonly Color s_VeryHighColor = new Color(1f, 0f, 0f, 1f);

	protected static readonly string s_RichtextLowColorString = ConvertColor32ToRichtextColor(s_LowColor);

	protected static readonly string s_RichtextMiddlingColorString = ConvertColor32ToRichtextColor(s_MiddlingColor);

	protected static readonly string s_RichtextHighColorString = ConvertColor32ToRichtextColor(s_HighColor);

	protected static readonly string s_RichtextVeryHighColorString = ConvertColor32ToRichtextColor(s_VeryHighColor);

	private const int k_ChartAspact = 4;

	protected float maxRecordedPeakValue => m_MaxRecordedPeakValue;

	public ProfilingChartRenderer(string name, CDChart config, CDChartAppearance configAppearance)
	{
		m_Name = name;
		m_Config = config;
		m_ConfigAppearance = configAppearance;
	}

	public virtual void InitializeRenderer()
	{
		if (m_MaterialChart == null)
		{
			m_MaterialChart = new Material(Shader.Find("Hidden/PerfTools/OverdrawMonitor/ProfilingChart"));
		}
		if (m_RenderTextureChart == null)
		{
			m_RenderTextureChart = new RenderTexture(512, 128, 0);
			m_RenderTextureChart.useMipMap = false;
			m_RenderTextureChart.Create();
		}
		ClearChartValues();
	}

	public virtual void UninitializeRenderer()
	{
		m_MaterialChart = null;
		if (m_RenderTextureChart != null)
		{
			m_RenderTextureChart.Release();
			m_RenderTextureChart = null;
		}
	}

	public bool isSelfName(string name)
	{
		if (!(m_Name == name))
		{
			return false;
		}
		return true;
	}

	public void ChangeTargetWH(int targetWidth, int targetHight)
	{
		if (m_RenderTextureChart != null)
		{
			if (m_RenderTextureChart.width == targetWidth && m_RenderTextureChart.height == targetHight)
			{
				return;
			}
			m_RenderTextureChart.Release();
			m_RenderTextureChart = null;
		}
		if (m_RenderTextureChart == null)
		{
			m_RenderTextureChart = new RenderTexture(targetWidth, targetHight, 0);
			m_RenderTextureChart.useMipMap = false;
			m_RenderTextureChart.Create();
		}
	}

	public void ClearChartValues()
	{
		Array.Clear(m_ChartValues, 0, m_ChartValues.Length);
		Array.Clear(m_RecordedChartValues, 0, m_RecordedChartValues.Length);
		m_RecordedChartValueStartIndex = 0;
		m_RecordedChartValueValidLength = 0;
		m_PeakValueRecordTimes = 0;
		Array.Clear(m_RecordedPeakValues, 0, m_RecordedPeakValues.Length);
		m_RecordedPeakValueCurrentIndex = 0;
		m_MaxRecordedPeakValue = 0f;
	}

	public void RecordChartValue(float v)
	{
		if (m_RecordedChartValueValidLength >= m_RecordedChartValues.Length)
		{
			m_RecordedChartValues[m_RecordedChartValueStartIndex] = v;
			m_RecordedChartValueStartIndex++;
			if (m_RecordedChartValueStartIndex >= m_RecordedChartValues.Length)
			{
				m_RecordedChartValueStartIndex = 0;
			}
		}
		else
		{
			m_RecordedChartValues[m_RecordedChartValueValidLength] = v;
			m_RecordedChartValueValidLength++;
		}
		float num = m_RecordedPeakValues[m_RecordedPeakValueCurrentIndex];
		if (v > num)
		{
			m_RecordedPeakValues[m_RecordedPeakValueCurrentIndex] = v;
			if (v > m_MaxRecordedPeakValue)
			{
				m_MaxRecordedPeakValue = v;
			}
		}
		m_PeakValueRecordTimes++;
		if (m_PeakValueRecordTimes < 10)
		{
			return;
		}
		m_PeakValueRecordTimes = 0;
		m_MaxRecordedPeakValue = 0f;
		for (int i = 0; i < 13; i++)
		{
			float num2 = m_RecordedPeakValues[i];
			if (num2 > m_MaxRecordedPeakValue)
			{
				m_MaxRecordedPeakValue = num2;
			}
		}
		m_RecordedPeakValueCurrentIndex++;
		if (m_RecordedPeakValueCurrentIndex >= 13)
		{
			m_RecordedPeakValueCurrentIndex = 0;
		}
		m_RecordedPeakValues[m_RecordedPeakValueCurrentIndex] = 0f;
	}

	private void FillChartValues()
	{
		if (m_RecordedChartValueValidLength >= m_RecordedChartValues.Length)
		{
			if (m_RecordedChartValueValidLength >= m_ChartValues.Length)
			{
				if (m_RecordedChartValueStartIndex >= m_ChartValues.Length)
				{
					int num = m_ChartValues.Length;
					int sourceIndex = m_RecordedChartValueStartIndex - num;
					int destinationIndex = 0;
					Array.Copy(m_RecordedChartValues, sourceIndex, m_ChartValues, destinationIndex, num);
					return;
				}
				int num2 = 0;
				int recordedChartValueStartIndex = m_RecordedChartValueStartIndex;
				num2 = recordedChartValueStartIndex;
				int sourceIndex2 = 0;
				int destinationIndex2 = m_ChartValues.Length - recordedChartValueStartIndex;
				Array.Copy(m_RecordedChartValues, sourceIndex2, m_ChartValues, destinationIndex2, recordedChartValueStartIndex);
				int num3 = m_ChartValues.Length - num2;
				int sourceIndex3 = m_RecordedChartValueValidLength - num3;
				int destinationIndex3 = 0;
				Array.Copy(m_RecordedChartValues, sourceIndex3, m_ChartValues, destinationIndex3, num3);
			}
			else
			{
				int num4 = 0;
				int recordedChartValueStartIndex2 = m_RecordedChartValueStartIndex;
				num4 = recordedChartValueStartIndex2;
				int sourceIndex4 = 0;
				int destinationIndex4 = m_ChartValues.Length - recordedChartValueStartIndex2;
				Array.Copy(m_RecordedChartValues, sourceIndex4, m_ChartValues, destinationIndex4, recordedChartValueStartIndex2);
				int num5 = m_RecordedChartValueValidLength - m_RecordedChartValueStartIndex;
				int recordedChartValueStartIndex3 = m_RecordedChartValueStartIndex;
				int destinationIndex5 = m_ChartValues.Length - num4 - num5;
				Array.Copy(m_RecordedChartValues, recordedChartValueStartIndex3, m_ChartValues, destinationIndex5, num5);
			}
		}
		else if (m_RecordedChartValueValidLength >= m_ChartValues.Length)
		{
			int num6 = m_ChartValues.Length;
			int sourceIndex5 = m_RecordedChartValueValidLength - num6;
			int destinationIndex6 = 0;
			Array.Copy(m_RecordedChartValues, sourceIndex5, m_ChartValues, destinationIndex6, num6);
		}
		else
		{
			int recordedChartValueValidLength = m_RecordedChartValueValidLength;
			int sourceIndex6 = 0;
			int destinationIndex7 = m_ChartValues.Length - recordedChartValueValidLength;
			Array.Copy(m_RecordedChartValues, sourceIndex6, m_ChartValues, destinationIndex7, recordedChartValueValidLength);
		}
	}

	public abstract void DoUpdateFrame();

	public abstract void DoRecord();

	public void DoRender()
	{
		if (!(m_RenderTextureChart == null) && !(m_MaterialChart == null))
		{
			FillChartValues();
			m_MaterialChart.SetFloat("_BackroungTransparent", m_ConfigAppearance.backroungTransparent);
			m_MaterialChart.SetFloat("_ChartTransparent", m_ConfigAppearance.chartTransparent);
			m_MaterialChart.SetColor("_LowColor", s_LowColor);
			m_MaterialChart.SetColor("_MiddlingColor", s_MiddlingColor);
			m_MaterialChart.SetColor("_HighColor", s_HighColor);
			m_MaterialChart.SetColor("_VeryHighColor", s_VeryHighColor);
			m_MaterialChart.SetFloat("_MiddlingValue", m_Config.middlingValue);
			m_MaterialChart.SetFloat("_HighValue", m_Config.highValue);
			m_MaterialChart.SetFloat("_VeryHighValue", m_Config.veryHighValue);
			m_MaterialChart.SetFloat("_FullValue", m_MaxRecordedPeakValue);
			m_MaterialChart.SetFloatArray("_ChartValues", m_ChartValues);
			Graphics.Blit(null, m_RenderTextureChart, m_MaterialChart);
		}
	}

	public abstract void DoGUI(bool fixedFontSize, bool foldout);

	protected static string ConvertColor32ToRichtextColor(Color c)
	{
		Color32 color = c;
		return $"#{color.r:x2}{color.g:x2}{color.b:x2}{color.a:x2}";
	}

	protected void GetLevelColor(float v, ref Color c)
	{
		if (v >= m_Config.veryHighValue)
		{
			c = s_VeryHighColor;
		}
		else if (v >= m_Config.highValue)
		{
			c = s_HighColor;
		}
		else if (v >= m_Config.middlingValue)
		{
			c = s_MiddlingColor;
		}
		else
		{
			c = s_LowColor;
		}
		c.a = 1f;
	}

	protected string GetLevelRichtextColorString(float v)
	{
		if (v >= m_Config.veryHighValue)
		{
			return s_RichtextVeryHighColorString;
		}
		if (v >= m_Config.highValue)
		{
			return s_RichtextHighColorString;
		}
		if (v >= m_Config.middlingValue)
		{
			return s_RichtextMiddlingColorString;
		}
		return s_RichtextLowColorString;
	}

	public static void CalculateSizes(bool fixedFontSize, bool foldout, out int fontSize, out float chartWidth, out float chartHeight)
	{
		if (fixedFontSize)
		{
			fontSize = 12;
			chartWidth = -1f;
			chartHeight = 50f;
			return;
		}
		int num = Mathf.Min(Screen.width, Screen.height);
		fontSize = (int)((float)num * PerfToolsRuntimeCfg.Instance.overdrawMonitorConfig.fontSize);
		chartWidth = fontSize * 16;
		chartHeight = chartWidth / 4f;
		if (foldout)
		{
			chartWidth /= 2f;
			chartHeight /= 2f;
		}
	}
}
