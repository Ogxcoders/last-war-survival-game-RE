using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Ram;

public class G_RamText : MonoBehaviour
{
	[SerializeField]
	private Text m_allocatedSystemMemorySizeText;

	[SerializeField]
	private Text m_reservedSystemMemorySizeText;

	[SerializeField]
	private Text m_monoSystemMemorySizeText;

	[SerializeField]
	private Text m_luaSystemMemorySizeText;

	private GraphyManager m_graphyManager;

	private G_RamMonitor m_ramMonitor;

	private float m_updateRate = 4f;

	private float m_deltaTime;

	private readonly string m_memoryStringFormat = "0.0";

	private void Awake()
	{
		Init();
	}

	private void Update()
	{
		m_deltaTime += Time.unscaledDeltaTime;
		if (m_deltaTime > 1f / m_updateRate)
		{
			m_allocatedSystemMemorySizeText.text = m_ramMonitor.AllocatedRam.ToStringNonAlloc(m_memoryStringFormat);
			m_reservedSystemMemorySizeText.text = m_ramMonitor.ReservedRam.ToStringNonAlloc(m_memoryStringFormat);
			m_monoSystemMemorySizeText.text = m_ramMonitor.MonoRam.ToStringNonAlloc(m_memoryStringFormat);
			m_luaSystemMemorySizeText.text = m_ramMonitor.LuaRam.ToStringNonAlloc(m_memoryStringFormat);
			m_deltaTime = 0f;
		}
	}

	public void UpdateParameters()
	{
		m_allocatedSystemMemorySizeText.color = m_graphyManager.AllocatedRamColor;
		m_reservedSystemMemorySizeText.color = m_graphyManager.ReservedRamColor;
		m_monoSystemMemorySizeText.color = m_graphyManager.MonoRamColor;
		m_luaSystemMemorySizeText.color = m_graphyManager.LuaRamColor;
		m_updateRate = m_graphyManager.RamTextUpdateRate;
	}

	private void Init()
	{
		if (!G_FloatString.Inited || G_FloatString.MinValue > -1000f || G_FloatString.MaxValue < 16384f)
		{
			G_FloatString.Init(-1001f, 16386f);
		}
		m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
		m_ramMonitor = GetComponent<G_RamMonitor>();
		UpdateParameters();
	}
}
