using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering;

[RequiredByNativeCode]
public class OnDemandRendering
{
	private static int m_RenderFrameInterval = 1;

	public static bool willCurrentFrameRender => Time.frameCount % renderFrameInterval == 0;

	public static int renderFrameInterval
	{
		get
		{
			return m_RenderFrameInterval;
		}
		set
		{
			m_RenderFrameInterval = Math.Max(1, value);
		}
	}

	public static int effectiveRenderFrameRate
	{
		get
		{
			if (QualitySettings.vSyncCount > 0)
			{
				return Screen.currentResolution.refreshRate / QualitySettings.vSyncCount / renderFrameInterval;
			}
			if (Application.targetFrameRate <= 0)
			{
				return 30 / renderFrameInterval;
			}
			return Application.targetFrameRate / renderFrameInterval;
		}
	}

	[RequiredByNativeCode]
	internal static void GetRenderFrameInterval(out int frameInterval)
	{
		frameInterval = renderFrameInterval;
	}
}
