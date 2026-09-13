using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal.Internal;

public class DrawObjectsPass : ScriptableRenderPass
{
	private FilteringSettings m_FilteringSettings;

	private RenderStateBlock m_RenderStateBlock;

	private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

	private string m_ProfilerTag;

	private ProfilingSampler m_ProfilingSampler;

	private bool m_IsOpaque;

	private static readonly int s_DrawObjectPassDataPropID = Shader.PropertyToID("_DrawObjectPassData");

	public DrawObjectsPass(string profilerTag, bool opaque, RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask, StencilState stencilState, int stencilReference)
	{
		m_ProfilerTag = profilerTag;
		m_ProfilingSampler = new ProfilingSampler(profilerTag);
		m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
		m_ShaderTagIdList.Add(new ShaderTagId("LightweightForward"));
		m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
		base.renderPassEvent = evt;
		m_FilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
		m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
		m_IsOpaque = opaque;
		if (stencilState.enabled)
		{
			m_RenderStateBlock.stencilReference = stencilReference;
			m_RenderStateBlock.mask = RenderStateMask.Stencil;
			m_RenderStateBlock.stencilState = stencilState;
		}
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get(m_ProfilerTag);
		using (new ProfilingScope(commandBuffer, m_ProfilingSampler))
		{
			Vector4 value = new Vector4(0f, 0f, 0f, m_IsOpaque ? 1f : 0f);
			commandBuffer.SetGlobalVector(s_DrawObjectPassDataPropID, value);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			SortingCriteria sortingCriteria = (m_IsOpaque ? renderingData.cameraData.defaultOpaqueSortFlags : SortingCriteria.CommonTransparent);
			DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
			FilteringSettings filteringSettings = m_FilteringSettings;
			context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings, ref m_RenderStateBlock);
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}
}
