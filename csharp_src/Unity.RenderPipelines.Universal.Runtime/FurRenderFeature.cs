using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FurRenderFeature : ScriptableRendererFeature
{
	[Serializable]
	public class FilterSettings
	{
		public RenderQueueType RenderQueueType;

		public LayerMask LayerMask = 1;

		public string[] PassNames;

		public FilterSettings()
		{
			RenderQueueType = RenderQueueType.Opaque;
			LayerMask = -1;
			PassNames = new string[2] { "FurRendererBase", "FurRendererLayer" };
		}
	}

	[Serializable]
	public class PassSettings
	{
		public string passTag = "FurRenderer";

		[Header("Settings")]
		public bool ShouldRender = true;

		[Tooltip("Set Layer Num")]
		[Range(1f, 200f)]
		public int PassLayerNum = 20;

		[Range(1000f, 5000f)]
		public int QueueMin = 2000;

		[Range(1000f, 5000f)]
		public int QueueMax = 5000;

		public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingSkybox;

		public FilterSettings filterSettings = new FilterSettings();
	}

	public class FurRenderPass : ScriptableRenderPass
	{
		private string m_ProfilerTag;

		private RenderQueueType renderQueueType;

		private PassSettings settings;

		private FurRenderFeature furRenderFeature;

		public List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

		private ShaderTagId shadowCasterSTI = new ShaderTagId("ShadowCaster");

		private FilteringSettings filter;

		public Material overrideMaterial { get; set; }

		public int overrideMaterialPassIndex { get; set; }

		public FurRenderPass(PassSettings setting, FurRenderFeature render, FilterSettings filterSettings)
		{
			m_ProfilerTag = setting.passTag;
			string[] passNames = filterSettings.PassNames;
			settings = setting;
			renderQueueType = filterSettings.RenderQueueType;
			furRenderFeature = render;
			filter = new FilteringSettings(new RenderQueueRange
			{
				lowerBound = setting.QueueMin,
				upperBound = setting.QueueMax
			}, filterSettings.LayerMask);
			if (passNames != null && passNames.Length != 0)
			{
				string[] array = passNames;
				foreach (string name in array)
				{
					m_ShaderTagIdList.Add(new ShaderTagId(name));
				}
			}
		}

		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			_ = renderQueueType;
			_ = 1;
			CommandBuffer commandBuffer = CommandBufferPool.Get(m_ProfilerTag);
			if (m_ShaderTagIdList.Count <= 0)
			{
				return;
			}
			DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList[0], ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
			if (m_ShaderTagIdList.Count > 1)
			{
				DrawingSettings drawingSettings2 = CreateDrawingSettings(m_ShaderTagIdList[1], ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
				float num = 1f / (float)settings.PassLayerNum;
				commandBuffer.Clear();
				commandBuffer.SetGlobalFloat("_FUR_OFFSET", 0f);
				context.ExecuteCommandBuffer(commandBuffer);
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filter);
				for (int i = 1; i < settings.PassLayerNum; i++)
				{
					commandBuffer.Clear();
					commandBuffer.SetGlobalFloat("_FUR_OFFSET", (float)i * num);
					context.ExecuteCommandBuffer(commandBuffer);
					context.DrawRenderers(renderingData.cullResults, ref drawingSettings2, ref filter);
				}
				CommandBufferPool.Release(commandBuffer);
			}
		}

		public override void FrameCleanup(CommandBuffer cmd)
		{
		}
	}

	public static FurRenderFeature instance;

	public PassSettings settings = new PassSettings();

	private FurRenderPass m_ScriptablePass;

	public override void Create()
	{
		instance = this;
		FilterSettings filterSettings = settings.filterSettings;
		m_ScriptablePass = new FurRenderPass(settings, this, filterSettings);
		m_ScriptablePass.renderPassEvent = settings.PassEvent;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(m_ScriptablePass);
	}
}
