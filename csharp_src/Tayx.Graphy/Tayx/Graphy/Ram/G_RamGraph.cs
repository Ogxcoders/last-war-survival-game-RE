using Tayx.Graphy.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Ram;

public class G_RamGraph : G_Graph
{
	[SerializeField]
	private Image m_imageAllocated;

	[SerializeField]
	private Image m_imageReserved;

	[SerializeField]
	private Image m_imageMono;

	[SerializeField]
	private Image m_imageLua;

	[SerializeField]
	private Shader ShaderFull;

	[SerializeField]
	private Shader ShaderLight;

	private GraphyManager m_graphyManager;

	private G_RamMonitor m_ramMonitor;

	private int m_resolution = 150;

	private G_GraphShader m_shaderGraphAllocated;

	private G_GraphShader m_shaderGraphReserved;

	private G_GraphShader m_shaderGraphMono;

	private G_GraphShader m_shaderGraphLua;

	private float[] m_allocatedArray;

	private float[] m_reservedArray;

	private float[] m_monoArray;

	private float[] m_luaArray;

	private float m_highestMemory;

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
		if (m_shaderGraphAllocated == null || m_shaderGraphReserved == null || m_shaderGraphMono == null || m_shaderGraphLua == null)
		{
			Init();
		}
		switch (m_graphyManager.GraphyMode)
		{
		case GraphyManager.Mode.FULL:
			m_shaderGraphAllocated.ArrayMaxSize = 512;
			m_shaderGraphReserved.ArrayMaxSize = 512;
			m_shaderGraphMono.ArrayMaxSize = 512;
			m_shaderGraphLua.ArrayMaxSize = 512;
			m_shaderGraphAllocated.Image.material = new Material(ShaderFull);
			m_shaderGraphReserved.Image.material = new Material(ShaderFull);
			m_shaderGraphMono.Image.material = new Material(ShaderFull);
			m_shaderGraphLua.Image.material = new Material(ShaderFull);
			break;
		case GraphyManager.Mode.LIGHT:
			m_shaderGraphAllocated.ArrayMaxSize = 128;
			m_shaderGraphReserved.ArrayMaxSize = 128;
			m_shaderGraphMono.ArrayMaxSize = 128;
			m_shaderGraphLua.ArrayMaxSize = 128;
			m_shaderGraphAllocated.Image.material = new Material(ShaderLight);
			m_shaderGraphReserved.Image.material = new Material(ShaderLight);
			m_shaderGraphMono.Image.material = new Material(ShaderLight);
			m_shaderGraphLua.Image.material = new Material(ShaderLight);
			break;
		}
		m_shaderGraphAllocated.InitializeShader();
		m_shaderGraphReserved.InitializeShader();
		m_shaderGraphMono.InitializeShader();
		m_shaderGraphLua.InitializeShader();
		m_resolution = m_graphyManager.RamGraphResolution;
		CreatePoints();
	}

	protected override void UpdateGraph()
	{
		float allocatedRam = m_ramMonitor.AllocatedRam;
		float reservedRam = m_ramMonitor.ReservedRam;
		float monoRam = m_ramMonitor.MonoRam;
		float luaRam = m_ramMonitor.LuaRam;
		m_highestMemory = 0f;
		for (int i = 0; i <= m_resolution - 1; i++)
		{
			if (i >= m_resolution - 1)
			{
				m_allocatedArray[i] = allocatedRam;
				m_reservedArray[i] = reservedRam;
				m_monoArray[i] = monoRam;
				m_luaArray[i] = luaRam;
			}
			else
			{
				m_allocatedArray[i] = m_allocatedArray[i + 1];
				m_reservedArray[i] = m_reservedArray[i + 1];
				m_monoArray[i] = m_monoArray[i + 1];
				m_luaArray[i] = m_luaArray[i + 1];
			}
			if (m_highestMemory < m_allocatedArray[i])
			{
				m_highestMemory = m_allocatedArray[i];
			}
			if (m_highestMemory < m_reservedArray[i])
			{
				m_highestMemory = m_reservedArray[i];
			}
			if (m_highestMemory < m_monoArray[i])
			{
				m_highestMemory = m_monoArray[i];
			}
			if (m_highestMemory < m_luaArray[i])
			{
				m_highestMemory = m_luaArray[i];
			}
		}
		for (int j = 0; j <= m_resolution - 1; j++)
		{
			m_shaderGraphAllocated.Array[j] = m_allocatedArray[j] / m_highestMemory;
			m_shaderGraphReserved.Array[j] = m_reservedArray[j] / m_highestMemory;
			m_shaderGraphMono.Array[j] = m_monoArray[j] / m_highestMemory;
			m_shaderGraphLua.Array[j] = m_luaArray[j] / m_highestMemory;
		}
		m_shaderGraphAllocated.UpdatePoints();
		m_shaderGraphReserved.UpdatePoints();
		m_shaderGraphMono.UpdatePoints();
		m_shaderGraphLua.UpdatePoints();
	}

	protected override void CreatePoints()
	{
		m_shaderGraphAllocated.Array = new float[m_resolution];
		m_shaderGraphReserved.Array = new float[m_resolution];
		m_shaderGraphMono.Array = new float[m_resolution];
		m_shaderGraphLua.Array = new float[m_resolution];
		m_allocatedArray = new float[m_resolution];
		m_reservedArray = new float[m_resolution];
		m_monoArray = new float[m_resolution];
		m_luaArray = new float[m_resolution];
		for (int i = 0; i < m_resolution; i++)
		{
			m_shaderGraphAllocated.Array[i] = 0f;
			m_shaderGraphReserved.Array[i] = 0f;
			m_shaderGraphMono.Array[i] = 0f;
			m_shaderGraphLua.Array[i] = 0f;
		}
		m_shaderGraphAllocated.GoodColor = m_graphyManager.AllocatedRamColor;
		m_shaderGraphAllocated.CautionColor = m_graphyManager.AllocatedRamColor;
		m_shaderGraphAllocated.CriticalColor = m_graphyManager.AllocatedRamColor;
		m_shaderGraphAllocated.UpdateColors();
		m_shaderGraphReserved.GoodColor = m_graphyManager.ReservedRamColor;
		m_shaderGraphReserved.CautionColor = m_graphyManager.ReservedRamColor;
		m_shaderGraphReserved.CriticalColor = m_graphyManager.ReservedRamColor;
		m_shaderGraphReserved.UpdateColors();
		m_shaderGraphMono.GoodColor = m_graphyManager.MonoRamColor;
		m_shaderGraphMono.CautionColor = m_graphyManager.MonoRamColor;
		m_shaderGraphMono.CriticalColor = m_graphyManager.MonoRamColor;
		m_shaderGraphMono.UpdateColors();
		m_shaderGraphLua.GoodColor = m_graphyManager.LuaRamColor;
		m_shaderGraphLua.CautionColor = m_graphyManager.LuaRamColor;
		m_shaderGraphLua.CriticalColor = m_graphyManager.LuaRamColor;
		m_shaderGraphLua.UpdateColors();
		m_shaderGraphAllocated.GoodThreshold = 0f;
		m_shaderGraphAllocated.CautionThreshold = 0f;
		m_shaderGraphAllocated.UpdateThresholds();
		m_shaderGraphReserved.GoodThreshold = 0f;
		m_shaderGraphReserved.CautionThreshold = 0f;
		m_shaderGraphReserved.UpdateThresholds();
		m_shaderGraphMono.GoodThreshold = 0f;
		m_shaderGraphMono.CautionThreshold = 0f;
		m_shaderGraphMono.UpdateThresholds();
		m_shaderGraphLua.GoodThreshold = 0f;
		m_shaderGraphLua.CautionThreshold = 0f;
		m_shaderGraphLua.UpdateThresholds();
		m_shaderGraphAllocated.UpdateArray();
		m_shaderGraphReserved.UpdateArray();
		m_shaderGraphMono.UpdateArray();
		m_shaderGraphLua.UpdateArray();
		m_shaderGraphAllocated.Average = 0f;
		m_shaderGraphReserved.Average = 0f;
		m_shaderGraphMono.Average = 0f;
		m_shaderGraphLua.Average = 0f;
		m_shaderGraphAllocated.UpdateAverage();
		m_shaderGraphReserved.UpdateAverage();
		m_shaderGraphMono.UpdateAverage();
		m_shaderGraphLua.UpdateAverage();
	}

	private void Init()
	{
		m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
		m_ramMonitor = GetComponent<G_RamMonitor>();
		m_shaderGraphAllocated = new G_GraphShader();
		m_shaderGraphReserved = new G_GraphShader();
		m_shaderGraphMono = new G_GraphShader();
		m_shaderGraphLua = new G_GraphShader();
		m_shaderGraphAllocated.Image = m_imageAllocated;
		m_shaderGraphReserved.Image = m_imageReserved;
		m_shaderGraphMono.Image = m_imageMono;
		m_shaderGraphLua.Image = m_imageLua;
		UpdateParameters();
	}
}
