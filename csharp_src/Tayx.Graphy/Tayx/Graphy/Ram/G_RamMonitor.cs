using UnityEngine;
using UnityEngine.Profiling;

namespace Tayx.Graphy.Ram;

public class G_RamMonitor : MonoBehaviour
{
	private GraphyManager m_graphyManager;

	private float m_allocatedRam;

	private float m_reservedRam;

	private float m_monoRam;

	private float m_luaRam;

	public float AllocatedRam => m_allocatedRam;

	public float ReservedRam => m_reservedRam;

	public float MonoRam => m_monoRam;

	public float LuaRam => m_luaRam;

	private void Awake()
	{
		m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
	}

	private void Update()
	{
		m_allocatedRam = (float)Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
		m_reservedRam = (float)Profiler.GetTotalReservedMemoryLong() / 1048576f;
		m_monoRam = (float)Profiler.GetMonoUsedSizeLong() / 1048576f;
		m_luaRam = (float)m_graphyManager.GetLuaMemory() / 1048576f;
	}
}
