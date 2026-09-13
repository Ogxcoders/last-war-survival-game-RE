using Tayx.Graphy.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Fps;

public class G_FpsGraph : G_Graph
{
	[SerializeField]
	private Image m_imageGraph;

	[SerializeField]
	private Shader ShaderFull;

	[SerializeField]
	private Shader ShaderLight;

	private GraphyManager m_graphyManager;

	private G_FpsMonitor m_fpsMonitor;

	private int m_resolution = 150;

	private G_GraphShader m_shaderGraph;

	private int[] m_fpsArray;

	private int m_highestFps;

	private void OnEnable()
	{
		Init();
	}

	private void Update()
	{
		UpdateGraph();
	}

	public void UpdateParameters()
	{
		switch (m_graphyManager.GraphyMode)
		{
		case GraphyManager.Mode.FULL:
			m_shaderGraph.ArrayMaxSize = 512;
			m_shaderGraph.Image.material = new Material(ShaderFull);
			break;
		case GraphyManager.Mode.LIGHT:
			m_shaderGraph.ArrayMaxSize = 128;
			m_shaderGraph.Image.material = new Material(ShaderLight);
			break;
		}
		m_shaderGraph.InitializeShader();
		m_resolution = m_graphyManager.FpsGraphResolution;
		CreatePoints();
	}

	protected override void UpdateGraph()
	{
		int num = (int)(1f / Time.unscaledDeltaTime);
		int num2 = 0;
		for (int i = 0; i <= m_resolution - 1; i++)
		{
			if (i >= m_resolution - 1)
			{
				m_fpsArray[i] = num;
			}
			else
			{
				m_fpsArray[i] = m_fpsArray[i + 1];
			}
			if (num2 < m_fpsArray[i])
			{
				num2 = m_fpsArray[i];
			}
		}
		m_highestFps = ((m_highestFps < 1 || m_highestFps <= num2) ? num2 : (m_highestFps - 1));
		for (int j = 0; j <= m_resolution - 1; j++)
		{
			m_shaderGraph.Array[j] = (float)m_fpsArray[j] / (float)m_highestFps;
		}
		m_shaderGraph.UpdatePoints();
		m_shaderGraph.Average = m_fpsMonitor.AverageFPS / (float)m_highestFps;
		m_shaderGraph.UpdateAverage();
		m_shaderGraph.GoodThreshold = (float)m_graphyManager.GoodFPSThreshold / (float)m_highestFps;
		m_shaderGraph.CautionThreshold = (float)m_graphyManager.CautionFPSThreshold / (float)m_highestFps;
		m_shaderGraph.UpdateThresholds();
	}

	protected override void CreatePoints()
	{
		m_shaderGraph.Array = new float[m_resolution];
		m_fpsArray = new int[m_resolution];
		for (int i = 0; i < m_resolution; i++)
		{
			m_shaderGraph.Array[i] = 0f;
		}
		m_shaderGraph.GoodColor = m_graphyManager.GoodFPSColor;
		m_shaderGraph.CautionColor = m_graphyManager.CautionFPSColor;
		m_shaderGraph.CriticalColor = m_graphyManager.CriticalFPSColor;
		m_shaderGraph.UpdateColors();
		m_shaderGraph.UpdateArray();
	}

	private void Init()
	{
		m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
		m_fpsMonitor = GetComponent<G_FpsMonitor>();
		m_shaderGraph = new G_GraphShader
		{
			Image = m_imageGraph
		};
		UpdateParameters();
	}
}
