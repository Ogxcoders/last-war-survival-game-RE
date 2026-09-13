using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.RenderingHotUpdatable;

public class CustomRenderObjectsPassHotUpdatable : ScriptableRenderPass
{
	private RenderQueueType renderQueueType;

	private FilteringSettings m_FilteringSettings;

	private RenderObjects.CustomCameraSettings m_CameraSettings;

	private CustomRenderObjectsFeatureHotUpdatable.RenderTargetSettings m_RenderTargetSettings;

	private string m_ProfilerTag;

	private ProfilingSampler m_ProfilingSampler;

	private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

	private RenderTargetHandle destination;

	private Dictionary<int, int> m_OverrideGlobalPropertyInts = new Dictionary<int, int>();

	private Dictionary<int, float> m_OverrideGlobalPropertyFloats = new Dictionary<int, float>();

	private Dictionary<int, Color> m_OverrideGlobalPropertyColors = new Dictionary<int, Color>();

	private Dictionary<int, Vector4> m_OverrideGlobalPropertyVectors = new Dictionary<int, Vector4>();

	private Dictionary<int, Texture> m_OverrideGlobalPropertyTextures = new Dictionary<int, Texture>();

	private RenderStateBlock m_RenderStateBlock;

	public Material overrideMaterial { get; set; }

	public int overrideMaterialPassIndex { get; set; }

	public void SetDetphState(bool writeEnabled, CompareFunction function = CompareFunction.Less)
	{
		m_RenderStateBlock.mask |= RenderStateMask.Depth;
		m_RenderStateBlock.depthState = new DepthState(writeEnabled, function);
	}

	public void SetStencilState(int reference, CompareFunction compareFunction, StencilOp passOp, StencilOp failOp, StencilOp zFailOp)
	{
		StencilState defaultValue = StencilState.defaultValue;
		defaultValue.enabled = true;
		defaultValue.SetCompareFunction(compareFunction);
		defaultValue.SetPassOperation(passOp);
		defaultValue.SetFailOperation(failOp);
		defaultValue.SetZFailOperation(zFailOp);
		m_RenderStateBlock.mask |= RenderStateMask.Stencil;
		m_RenderStateBlock.stencilReference = reference;
		m_RenderStateBlock.stencilState = defaultValue;
	}

	public CustomRenderObjectsPassHotUpdatable(CustomRenderObjectsFeatureHotUpdatable.RenderObjectsSettings settings)
		: this(settings.passTag, settings)
	{
	}

	public CustomRenderObjectsPassHotUpdatable(string tag, CustomRenderObjectsFeatureHotUpdatable.RenderObjectsSettings settings)
		: this(tag, (RenderPassEvent)((int)settings.Event + (int)settings.eventOffset), settings.filterSettings.PassNames, settings.filterSettings.RenderQueueType, settings.filterSettings.LayerMask, (uint)settings.filterSettings.RenderingLayerMask, settings.cameraSettings, settings.renderTargetSettings, settings)
	{
	}

	public CustomRenderObjectsPassHotUpdatable(string profilerTag, RenderPassEvent renderPassEvent, string[] shaderTags, RenderQueueType renderQueueType, int layerMask, uint renderingLayerMask, RenderObjects.CustomCameraSettings cameraSettings, CustomRenderObjectsFeatureHotUpdatable.RenderTargetSettings renderTargetSettings, CustomRenderObjectsFeatureHotUpdatable.RenderObjectsSettings settings)
	{
		m_ProfilerTag = profilerTag;
		m_ProfilingSampler = new ProfilingSampler(profilerTag);
		base.renderPassEvent = renderPassEvent;
		this.renderQueueType = renderQueueType;
		overrideMaterial = null;
		overrideMaterialPassIndex = 0;
		m_FilteringSettings = new FilteringSettings((renderQueueType == RenderQueueType.Transparent) ? RenderQueueRange.transparent : RenderQueueRange.opaque, layerMask, renderingLayerMask);
		if (shaderTags != null && shaderTags.Length != 0)
		{
			foreach (string name in shaderTags)
			{
				m_ShaderTagIdList.Add(new ShaderTagId(name));
			}
		}
		else
		{
			m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
			m_ShaderTagIdList.Add(new ShaderTagId("LightweightForward"));
			m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
		}
		m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
		m_CameraSettings = cameraSettings;
		m_RenderTargetSettings = renderTargetSettings;
		if (settings.overrideGlobalProperties)
		{
			overrideMaterial = null;
			RecordOverrideGlobalProperties(settings);
		}
		else
		{
			overrideMaterial = settings.overrideMaterial;
			overrideMaterialPassIndex = settings.overrideMaterialPassIndex;
		}
		if (settings.overrideDepthState)
		{
			SetDetphState(settings.enableWrite, settings.depthCompareFunction);
		}
		if (settings.stencilSettings.overrideStencilState)
		{
			SetStencilState(settings.stencilSettings.stencilReference, settings.stencilSettings.stencilCompareFunction, settings.stencilSettings.passOperation, settings.stencilSettings.failOperation, settings.stencilSettings.zFailOperation);
		}
		destination.Init(m_RenderTargetSettings.name);
	}

	protected virtual void RecordOverrideGlobalProperties(CustomRenderObjectsFeatureHotUpdatable.RenderObjectsSettings settings)
	{
		string text = "_OverrideGlobalProperty";
		Material material = settings.overrideMaterial;
		Shader shader = material.shader;
		for (int i = 0; i < shader.GetPropertyCount(); i++)
		{
			shader.GetPropertyNameId(i);
			string propertyName = shader.GetPropertyName(i);
			ShaderPropertyType propertyType = shader.GetPropertyType(i);
			ShaderPropertyFlags propertyFlags = shader.GetPropertyFlags(i);
			int key = Shader.PropertyToID(propertyName + text);
			switch (propertyType)
			{
			case ShaderPropertyType.Float:
			case ShaderPropertyType.Range:
				m_OverrideGlobalPropertyFloats.Add(key, (QualitySettings.activeColorSpace == ColorSpace.Linear && propertyFlags.HasFlag(ShaderPropertyFlags.Gamma)) ? Mathf.GammaToLinearSpace(material.GetFloat(propertyName)) : material.GetFloat(propertyName));
				break;
			case ShaderPropertyType.Color:
				m_OverrideGlobalPropertyColors.Add(key, (QualitySettings.activeColorSpace == ColorSpace.Linear) ? material.GetColor(propertyName).linear : material.GetColor(propertyName));
				break;
			case ShaderPropertyType.Vector:
				m_OverrideGlobalPropertyVectors.Add(key, material.GetVector(propertyName));
				break;
			case ShaderPropertyType.Texture:
				m_OverrideGlobalPropertyTextures.Add(key, material.GetTexture(propertyName));
				break;
			}
		}
	}

	protected virtual void ApplyOverrideGlobalProperties(CommandBuffer cmd)
	{
		foreach (KeyValuePair<int, int> overrideGlobalPropertyInt in m_OverrideGlobalPropertyInts)
		{
			cmd.SetGlobalInt(overrideGlobalPropertyInt.Key, overrideGlobalPropertyInt.Value);
		}
		foreach (KeyValuePair<int, float> overrideGlobalPropertyFloat in m_OverrideGlobalPropertyFloats)
		{
			cmd.SetGlobalFloat(overrideGlobalPropertyFloat.Key, overrideGlobalPropertyFloat.Value);
		}
		foreach (KeyValuePair<int, Color> overrideGlobalPropertyColor in m_OverrideGlobalPropertyColors)
		{
			cmd.SetGlobalColor(overrideGlobalPropertyColor.Key, overrideGlobalPropertyColor.Value);
		}
		foreach (KeyValuePair<int, Vector4> overrideGlobalPropertyVector in m_OverrideGlobalPropertyVectors)
		{
			cmd.SetGlobalVector(overrideGlobalPropertyVector.Key, overrideGlobalPropertyVector.Value);
		}
		foreach (KeyValuePair<int, Texture> overrideGlobalPropertyTexture in m_OverrideGlobalPropertyTextures)
		{
			cmd.SetGlobalTexture(overrideGlobalPropertyTexture.Key, overrideGlobalPropertyTexture.Value);
		}
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		if (cmd == null)
		{
			throw new ArgumentNullException("cmd");
		}
		bool flag = m_RenderTargetSettings.scale * (float)cameraTextureDescriptor.width > 1f && m_RenderTargetSettings.scale * (float)cameraTextureDescriptor.height > 1f;
		if (m_RenderTargetSettings != null && m_RenderTargetSettings.overrideTarget && !string.IsNullOrEmpty(m_RenderTargetSettings.name) && (flag || !(m_RenderTargetSettings.size.SqrMagnitude() < 1f)))
		{
			RenderTextureDescriptor desc = cameraTextureDescriptor;
			desc.colorFormat = m_RenderTargetSettings.format;
			desc.depthBufferBits = m_RenderTargetSettings.depth;
			desc.msaaSamples = ((m_RenderTargetSettings.format == RenderTextureFormat.Depth) ? 1 : desc.msaaSamples);
			desc.width = Math.Max(2, Convert.ToInt32(flag ? ((float)cameraTextureDescriptor.width * m_RenderTargetSettings.scale) : m_RenderTargetSettings.size.x));
			desc.height = Math.Max(2, Convert.ToInt32(flag ? ((float)cameraTextureDescriptor.height * m_RenderTargetSettings.scale) : m_RenderTargetSettings.size.y));
			cmd.GetTemporaryRT(destination.id, desc, m_RenderTargetSettings.filterMode);
			ConfigureTarget(destination.Identifier());
			ConfigureClear(m_RenderTargetSettings.clearFlag, m_RenderTargetSettings.clearColor);
		}
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		SortingCriteria sortingCriteria = ((renderQueueType == RenderQueueType.Transparent) ? SortingCriteria.CommonTransparent : renderingData.cameraData.defaultOpaqueSortFlags);
		DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
		drawingSettings.overrideMaterial = overrideMaterial;
		drawingSettings.overrideMaterialPassIndex = overrideMaterialPassIndex;
		ref CameraData cameraData = ref renderingData.cameraData;
		Camera camera = cameraData.camera;
		float aspect = renderingData.cameraData.camera.aspect;
		CommandBuffer commandBuffer = CommandBufferPool.Get(m_ProfilerTag);
		using (new ProfilingScope(commandBuffer, m_ProfilingSampler))
		{
			if (m_CameraSettings.overrideCamera && cameraData.isStereoEnabled)
			{
				Debug.LogWarning("RenderObjects pass is configured to override camera matrices. While rendering in stereo camera matrices cannot be overriden.");
			}
			if (m_CameraSettings.overrideCamera && !cameraData.isStereoEnabled)
			{
				Matrix4x4 proj = Matrix4x4.Perspective(m_CameraSettings.cameraFieldOfView, aspect, camera.nearClipPlane, camera.farClipPlane);
				proj = GL.GetGPUProjectionMatrix(proj, cameraData.IsCameraProjectionMatrixFlipped());
				Matrix4x4 viewMatrix = cameraData.GetViewMatrix();
				Vector4 column = viewMatrix.GetColumn(3);
				viewMatrix.SetColumn(3, column + m_CameraSettings.offset);
				RenderingUtils.SetViewAndProjectionMatrices(commandBuffer, viewMatrix, proj, setInverseMatrices: false);
			}
			ApplyOverrideGlobalProperties(commandBuffer);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings, ref m_RenderStateBlock);
			if (m_CameraSettings.overrideCamera && m_CameraSettings.restoreCamera && !cameraData.isStereoEnabled)
			{
				RenderingUtils.SetViewAndProjectionMatrices(commandBuffer, cameraData.GetViewMatrix(), cameraData.GetGPUProjectionMatrix(), setInverseMatrices: false);
			}
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		if (cmd == null)
		{
			throw new ArgumentNullException("cmd");
		}
		cmd.ReleaseTemporaryRT(destination.id);
	}
}
