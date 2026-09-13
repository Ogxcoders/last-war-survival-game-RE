using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

public class ShowFPSText : MonoBehaviour
{
	private float updateInterval = 1f;

	private double lastInterval;

	private int frames;

	private int fps;

	public TextMeshProUGUI txt;

	private int _lastPing;

	private float _luaMemMega;

	private StringBuilder sbFps;

	private StringBuilder sbMemory;

	private StringBuilder sbGpuSkin;

	public static ShowFPSText Instance { get; private set; }

	public string FpsString => sbFps?.ToString() ?? string.Empty;

	public string MemString => sbMemory?.ToString() ?? string.Empty;

	public string GpuSkinString => sbGpuSkin?.ToString() ?? string.Empty;

	private void Start()
	{
		Instance = this;
		if (CommonUtils.IsDebug())
		{
			sbFps = sbFps ?? new StringBuilder(64);
			sbMemory = sbMemory ?? new StringBuilder(64);
			sbGpuSkin = sbGpuSkin ?? new StringBuilder(64);
			lastInterval = Time.realtimeSinceStartup;
			frames = 0;
			fps = 0;
			_lastPing = GameEntry.Network.GetPing();
			_luaMemMega = 0f;
			RefreshText();
		}
	}

	private void Update()
	{
		if (!CommonUtils.IsDebug())
		{
			return;
		}
		frames++;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if ((double)realtimeSinceStartup > lastInterval + (double)updateInterval)
		{
			fps = (int)((double)frames / ((double)realtimeSinceStartup - lastInterval));
			frames = 0;
			lastInterval = realtimeSinceStartup;
			_lastPing = GameEntry.Network.GetPing();
			XLuaManager lua = GameEntry.Lua;
			if (lua != null && lua.Env != null)
			{
				_luaMemMega = 0.001f * lua.CallWithReturn<float, string>("_G.collectgarbage", "count");
			}
			RefreshText();
		}
	}

	private void RefreshText()
	{
		if (sbFps != null)
		{
			long serverTimeWithoutOffset = GameEntry.Timer.GetServerTimeWithoutOffset();
			serverTimeWithoutOffset = ((serverTimeWithoutOffset == long.MaxValue) ? (-1) : serverTimeWithoutOffset);
			long num = 1048576L;
			long num2 = Profiler.GetMonoUsedSizeLong() / num;
			long num3 = Profiler.GetTotalAllocatedMemoryLong() / num;
			ulong num4 = Texture.currentTextureMemory / (ulong)num;
			sbFps.Clear().Append($"F:{fps}/P:{_lastPing}/T:{serverTimeWithoutOffset}");
			sbMemory.Clear().Append($"L:{_luaMemMega:F0}/M:{num2}/N:{num3}/T:{num4}");
			sbGpuSkin.Clear().Append($"AR:{GPUSkinnedMeshRenderer.activeRendererCount}/TR:{GPUSkinnedMeshRenderer.totalRendererCount}");
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private int GetVisibleSkinnedMeshCount()
	{
		SkinnedMeshRenderer[] array = Object.FindObjectsOfType<SkinnedMeshRenderer>();
		int num = 0;
		SkinnedMeshRenderer[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i].isVisible)
			{
				num++;
			}
		}
		return num;
	}

	private int GetVisibleGPUMeshRendererCount()
	{
		GPUSkinnedMeshRenderer[] array = Object.FindObjectsOfType<GPUSkinnedMeshRenderer>();
		int num = 0;
		GPUSkinnedMeshRenderer[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i].meshRenderer.isVisible)
			{
				num++;
			}
		}
		return num;
	}
}
