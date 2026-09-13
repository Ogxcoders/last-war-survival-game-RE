using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.PerfTools;

public class OverdrawPass : ScriptableRenderPass
{
	private string _ProfilerTag;

	private FilteringSettings _FilteringSettings;

	private List<ShaderTagId> _TagIdList = new List<ShaderTagId>();

	private bool _IsOpaque;

	private Material _Material;

	public OverdrawPass(string profilerTag, RenderQueueRange renderQueueRange, Shader shader, bool isOpaque)
	{
		_ProfilerTag = profilerTag;
		_IsOpaque = isOpaque;
		_TagIdList.Add(new ShaderTagId("UniversalForward"));
		_TagIdList.Add(new ShaderTagId("LightweightForward"));
		_TagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
		_FilteringSettings = new FilteringSettings(renderQueueRange, LayerMask.NameToLayer("Everything"));
		_Material = CoreUtils.CreateEngineMaterial(shader);
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get(_ProfilerTag);
		SortingCriteria sortingCriteria = (_IsOpaque ? renderingData.cameraData.defaultOpaqueSortFlags : SortingCriteria.CommonTransparent);
		DrawingSettings drawingSettings = CreateDrawingSettings(_TagIdList, ref renderingData, sortingCriteria);
		drawingSettings.overrideMaterial = _Material;
		drawingSettings.enableDynamicBatching = renderingData.supportsDynamicBatching;
		context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _FilteringSettings);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}
}
