using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SceneCameraCheckStrategy : NeedHighFPSCheckStrategyBase
{
	private SceneManager.SceneID[] m_IDs;

	public static WeakReference<UniversalAdditionalCameraData> data { get; } = new WeakReference<UniversalAdditionalCameraData>(null);

	public SceneCameraCheckStrategy(params SceneManager.SceneID[] ids)
	{
		if (!data.TryGetTarget(out var _))
		{
			Camera orFindMainCamera = CameraUtilities.GetOrFindMainCamera();
			if (orFindMainCamera != null)
			{
				data.SetTarget(orFindMainCamera.GetComponent<UniversalAdditionalCameraData>());
			}
		}
		m_IDs = ids;
	}

	public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
	{
		pass = false;
		if (m_IDs == null || !data.TryGetTarget(out var target) || target.disableRender)
		{
			return;
		}
		for (int i = 0; i < m_IDs.Length; i++)
		{
			if (SceneManager.CurrSceneID == (int)m_IDs[i])
			{
				pass = true;
				break;
			}
		}
	}
}
