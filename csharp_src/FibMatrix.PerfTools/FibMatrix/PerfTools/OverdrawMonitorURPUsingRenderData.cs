using System.Reflection;
using FibMatrix.PerfTools.RenderDataForURP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.PerfTools;

public class OverdrawMonitorURPUsingRenderData : OverdrawMonitor
{
	private RenderTexture _OverdrawBaseCameraTargetTexture;

	private OverdrawRendererConverter _OverdrawRendererConverter;

	protected override void OnEnableInternal()
	{
		base.OnEnableInternal();
		_OverdrawRendererConverter = new OverdrawRendererConverter();
		RenderPipelineManager.beginCameraRendering += RenderPipelineManager_BeforeCameraRendering;
		RenderPipelineManager.endCameraRendering += RenderPipelineManager_EndCameraRendering;
	}

	protected override void OnDisableInternal()
	{
		base.OnDisableInternal();
		RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_BeforeCameraRendering;
		RenderPipelineManager.endCameraRendering -= RenderPipelineManager_EndCameraRendering;
		if (_OverdrawBaseCameraTargetTexture != null)
		{
			_OverdrawBaseCameraTargetTexture.Release();
			Object.DestroyImmediate(_OverdrawBaseCameraTargetTexture);
			_OverdrawBaseCameraTargetTexture = null;
		}
		_OverdrawRendererConverter = null;
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
		CopyCameraFromHelper(target, m_OverdrawCamera);
	}

	public static void CopyCameraFromHelper(Camera target, Camera overdrawCam)
	{
		if (target == null || overdrawCam == target)
		{
			return;
		}
		UniversalAdditionalCameraData universalAdditionalCameraData = overdrawCam.GetUniversalAdditionalCameraData();
		if (universalAdditionalCameraData == null)
		{
			return;
		}
		UniversalAdditionalCameraData universalAdditionalCameraData2 = target.GetUniversalAdditionalCameraData();
		if (universalAdditionalCameraData2 == null)
		{
			return;
		}
		OverdrawMonitorManager instance = OverdrawMonitorManager.Instance;
		if (universalAdditionalCameraData2.renderType != CameraRenderType.Base)
		{
			universalAdditionalCameraData.renderType = CameraRenderType.Base;
		}
		else
		{
			universalAdditionalCameraData.renderType = universalAdditionalCameraData2.renderType;
		}
		if (universalAdditionalCameraData2.renderType != CameraRenderType.Base)
		{
			overdrawCam.enabled = false;
		}
		universalAdditionalCameraData.GetType().GetField("m_ClearDepth", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(universalAdditionalCameraData, universalAdditionalCameraData2.clearDepth);
		universalAdditionalCameraData.renderPostProcessing = false;
		universalAdditionalCameraData.dithering = false;
		universalAdditionalCameraData.requiresColorOption = CameraOverrideOption.Off;
		universalAdditionalCameraData.requiresDepthOption = CameraOverrideOption.Off;
		universalAdditionalCameraData.renderShadows = false;
		if (universalAdditionalCameraData.renderType != CameraRenderType.Base)
		{
			return;
		}
		universalAdditionalCameraData.cameraStack.Clear();
		if (universalAdditionalCameraData2.renderType != CameraRenderType.Base)
		{
			return;
		}
		foreach (Camera item in universalAdditionalCameraData2.cameraStack)
		{
			OverdrawMonitorURPUsingRenderData overdrawMonitorURPUsingRenderData = instance.FindOverdrawMonitor(item) as OverdrawMonitorURPUsingRenderData;
			if (!(overdrawMonitorURPUsingRenderData == null) && !overdrawMonitorURPUsingRenderData.IsBaseCamera())
			{
				universalAdditionalCameraData.cameraStack.Add(overdrawMonitorURPUsingRenderData.m_OverdrawCamera);
			}
		}
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
		if (!(camera != m_OverdrawCamera))
		{
			_OverdrawRendererConverter.Convert();
			CopyPass.SetOutputTarget(base.overdrawTexture);
		}
	}

	private void RenderPipelineManager_EndCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		if (!(camera != m_OverdrawCamera))
		{
			CopyPass.ClearOutputTarget();
			_OverdrawRendererConverter.Restore();
			CalculateOverdrawFromRT();
		}
	}
}
