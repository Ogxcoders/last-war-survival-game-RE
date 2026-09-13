using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.PerfTools;

public class OverdrawMonitorURP : OverdrawMonitor
{
	private RenderTexture _OverdrawBaseCameraTargetTexture;

	private OverdrawRendererFeature m_OverdrawRendererFeature;

	protected override void OnEnableInternal()
	{
		base.OnEnableInternal();
		m_OverdrawRendererFeature = new OverdrawRendererFeature();
		m_OverdrawRendererFeature.Create();
		RenderPipelineManager.beginCameraRendering += RenderPipelineManager_BeforeCameraRendering;
		RenderPipelineManager.endCameraRendering += RenderPipelineManager_EndCameraRendering;
	}

	protected override void OnDisableInternal()
	{
		base.OnDisableInternal();
		m_OverdrawRendererFeature = null;
		RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_BeforeCameraRendering;
		RenderPipelineManager.endCameraRendering -= RenderPipelineManager_EndCameraRendering;
		if (_OverdrawBaseCameraTargetTexture != null)
		{
			_OverdrawBaseCameraTargetTexture.Release();
			Object.DestroyImmediate(_OverdrawBaseCameraTargetTexture);
			_OverdrawBaseCameraTargetTexture = null;
		}
	}

	protected override void RecreateTexture()
	{
		base.RecreateTexture();
		if (base.overdrawTexture == null)
		{
			base.overdrawTexture = new RenderTexture(256, 256, 0);
			base.overdrawTexture.name = "Overdraw_" + m_OverdrawCamera?.name;
			base.overdrawTexture.hideFlags = HideFlags.HideAndDontSave;
			base.overdrawTexture.enableRandomWrite = false;
		}
		if (IsBaseCamera() && _OverdrawBaseCameraTargetTexture == null)
		{
			_OverdrawBaseCameraTargetTexture = new RenderTexture(256, 256, 24, RenderTextureFormat.Default);
			_OverdrawBaseCameraTargetTexture.name = "Overdraw(BaseTarget)" + m_OverdrawCamera?.name;
			_OverdrawBaseCameraTargetTexture.hideFlags = HideFlags.HideAndDontSave;
			_OverdrawBaseCameraTargetTexture.enableRandomWrite = false;
		}
	}

	protected override void SetCameraTarget()
	{
		base.SetCameraTarget();
		if (IsBaseCamera())
		{
			if (_OverdrawBaseCameraTargetTexture != null)
			{
				m_OverdrawCamera.targetTexture = _OverdrawBaseCameraTargetTexture;
			}
		}
		else if (base.overdrawTexture != null)
		{
			Rect pixelRect = m_OverdrawCamera.pixelRect;
			pixelRect.Set(0f, 0f, base.overdrawTexture.width, base.overdrawTexture.height);
			m_OverdrawCamera.pixelRect = pixelRect;
		}
		m_OverdrawCamera.aspect = m_TargetCamera.aspect;
	}

	protected override void CopyCameraFrom(Camera target)
	{
		base.CopyCameraFrom(target);
		OverdrawMonitorURPUsingRenderData.CopyCameraFromHelper(target, m_OverdrawCamera);
	}

	private bool IsBaseCamera()
	{
		bool result = false;
		if (m_OverdrawCamera != null)
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = m_OverdrawCamera.GetUniversalAdditionalCameraData();
			if (universalAdditionalCameraData != null && universalAdditionalCameraData.renderType == CameraRenderType.Base)
			{
				result = true;
			}
		}
		return result;
	}

	private void RenderPipelineManager_BeforeCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		if (camera != m_OverdrawCamera)
		{
			return;
		}
		UniversalAdditionalCameraData universalAdditionalCameraData = m_OverdrawCamera.GetUniversalAdditionalCameraData();
		if (universalAdditionalCameraData != null)
		{
			if (universalAdditionalCameraData.renderType == CameraRenderType.Base)
			{
				m_OverdrawRendererFeature.setRenderTarget(_OverdrawBaseCameraTargetTexture);
			}
			m_OverdrawRendererFeature.setOutputTarget(base.overdrawTexture);
			m_OverdrawRendererFeature?.AddRenderPasses(universalAdditionalCameraData.scriptableRenderer);
		}
	}

	private void RenderPipelineManager_EndCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		if (!(camera != m_OverdrawCamera))
		{
			CalculateOverdrawFromRT();
		}
	}
}
