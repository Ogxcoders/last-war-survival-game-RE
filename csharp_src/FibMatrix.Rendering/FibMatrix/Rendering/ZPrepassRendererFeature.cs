using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

[ExcludeFromPreset]
public class ZPrepassRendererFeature : ScriptableRendererFeature
{
	public const string ZPrepassSettingEditorTag = "ZPrepassSettingEditor";

	public const string ZPrepassSettingTag = "ZPrepassSetting";

	public const string RenderObjectsSettingTag = "RenderObjectsSetting";

	[Header("ZPrepassSettingEditor")]
	public CustomRenderObjectsFeature.RenderObjectsSettings zPrepassSettingEditor = new CustomRenderObjectsFeature.RenderObjectsSettings();

	[Header("ZPrepassSetting")]
	public CustomRenderObjectsFeature.RenderObjectsSettings zPrepassSetting = new CustomRenderObjectsFeature.RenderObjectsSettings();

	[Header("RenderObjectsSetting")]
	public CustomRenderObjectsFeature.RenderObjectsSettings renderObjectsSetting = new CustomRenderObjectsFeature.RenderObjectsSettings();

	private CustomRenderObjectsPass m_ZPreassPassEditor;

	private CustomRenderObjectsPass m_ZPreassPass;

	private CustomRenderObjectsPass m_RenderObjectsPass;

	public override void Create()
	{
		zPrepassSettingEditor.cameraType = CameraType.SceneView | CameraType.Preview;
		zPrepassSettingEditor.Event = RenderPassEvent.BeforeRenderingPrepasses;
		zPrepassSettingEditor.eventOffset = 1u;
		zPrepassSettingEditor.filterSettings.PassNames = new string[1] { "DepthOnly" };
		zPrepassSetting.filterSettings.PassNames = new string[1] { "DepthOnly" };
		renderObjectsSetting.overrideDepthState = true;
		renderObjectsSetting.enableWrite = false;
		renderObjectsSetting.depthCompareFunction = CompareFunction.Equal;
		m_ZPreassPassEditor = new CustomRenderObjectsPass("ZPrepassSettingEditor", zPrepassSettingEditor);
		m_ZPreassPass = new CustomRenderObjectsPass("ZPrepassSetting", zPrepassSetting);
		m_RenderObjectsPass = new CustomRenderObjectsPass("RenderObjectsSetting", renderObjectsSetting);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (zPrepassSettingEditor.cameraType.HasFlag(renderingData.cameraData.cameraType))
		{
			renderer.EnqueuePass(m_ZPreassPassEditor);
		}
		if (zPrepassSetting.cameraType.HasFlag(renderingData.cameraData.cameraType))
		{
			renderer.EnqueuePass(m_ZPreassPass);
		}
		if (renderObjectsSetting.cameraType.HasFlag(renderingData.cameraData.cameraType))
		{
			renderer.EnqueuePass(m_RenderObjectsPass);
		}
	}
}
