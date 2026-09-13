using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WorldGPUInstancingRenderFeature : ScriptableRendererFeature
{
	private class GPUInstancingPass : ScriptableRenderPass
	{
		private List<IWorldGPUInstancingRenderer> renderers;

		public void AddRenderer(IWorldGPUInstancingRenderer renderer)
		{
			if (renderer != null)
			{
				if (renderers == null)
				{
					renderers = new List<IWorldGPUInstancingRenderer>();
					renderers.Add(renderer);
				}
				else if (!renderers.Contains(renderer))
				{
					renderers.Add(renderer);
				}
				renderers.Sort(SortRenderers);
			}
		}

		public void RemoveRenderer(IWorldGPUInstancingRenderer renderer)
		{
			if (renderer != null)
			{
				renderers?.Remove(renderer);
			}
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (renderers == null || renderers.Count <= 0)
			{
				return;
			}
			int i = 0;
			for (int count = renderers.Count; i < count; i++)
			{
				IWorldGPUInstancingRenderer worldGPUInstancingRenderer = renderers[i];
				if (worldGPUInstancingRenderer != null && worldGPUInstancingRenderer.IsRenderable)
				{
					CommandBuffer commandBuffer = CommandBufferPool.Get(worldGPUInstancingRenderer.Name);
					if (commandBuffer != null)
					{
						worldGPUInstancingRenderer.Draw(commandBuffer);
						context.ExecuteCommandBuffer(commandBuffer);
						commandBuffer.Clear();
						CommandBufferPool.Release(commandBuffer);
					}
				}
			}
		}

		private static int SortRenderers(IWorldGPUInstancingRenderer a, IWorldGPUInstancingRenderer b)
		{
			return a.SortingOrder - b.SortingOrder;
		}

		public string Description()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("renderPassEvent:" + base.renderPassEvent);
			stringBuilder.AppendLine($"renderers:{renderers?.Count}");
			List<IWorldGPUInstancingRenderer> list = renderers;
			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < renderers.Count; i++)
				{
					stringBuilder.AppendLine(string.Format("[{0}]{1}", i, renderers[i]?.Name ?? "NULL"));
				}
			}
			return stringBuilder.ToString();
		}
	}

	private List<GPUInstancingPass> renderPassList;

	private const string TagMainCamera = "MainCamera";

	private Camera mainCamera;

	public override void Create()
	{
		mainCamera = Camera.main;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderPassList == null || renderPassList.Count <= 0)
		{
			return;
		}
		Camera camera = renderingData.cameraData.camera;
		if (camera == null)
		{
			return;
		}
		int i = 0;
		for (int count = renderPassList.Count; i < count; i++)
		{
			if ((object)camera == mainCamera)
			{
				renderer.EnqueuePass(renderPassList[i]);
			}
		}
	}

	public void AddRenderer(IWorldGPUInstancingRenderer renderer)
	{
		if (renderer == null)
		{
			return;
		}
		renderPassList = renderPassList ?? new List<GPUInstancingPass>();
		RenderPassEvent renderPassEvent = renderer.RenderPassEvent;
		GPUInstancingPass gPUInstancingPass = null;
		int i = 0;
		for (int count = renderPassList.Count; i < count; i++)
		{
			if (renderPassList[i].renderPassEvent == renderPassEvent)
			{
				gPUInstancingPass = renderPassList[i];
				break;
			}
		}
		if (gPUInstancingPass == null)
		{
			gPUInstancingPass = new GPUInstancingPass();
			gPUInstancingPass.renderPassEvent = renderPassEvent;
			renderPassList.Add(gPUInstancingPass);
		}
		gPUInstancingPass.AddRenderer(renderer);
	}

	public void RemoveRenderer(IWorldGPUInstancingRenderer renderer)
	{
		if (renderer == null || renderPassList == null)
		{
			return;
		}
		RenderPassEvent renderPassEvent = renderer.RenderPassEvent;
		GPUInstancingPass gPUInstancingPass = null;
		int i = 0;
		for (int count = renderPassList.Count; i < count; i++)
		{
			if (renderPassList[i].renderPassEvent == renderPassEvent)
			{
				gPUInstancingPass = renderPassList[i];
				break;
			}
		}
		gPUInstancingPass?.RemoveRenderer(renderer);
	}

	public void ClearAllPass()
	{
		renderPassList?.Clear();
	}

	public string Description()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine($"开启状态:{base.isActive}");
		stringBuilder.AppendLine("主相机:" + mainCamera?.name);
		stringBuilder.AppendLine($"Pass数量:{renderPassList?.Count}");
		List<GPUInstancingPass> list = renderPassList;
		if (list != null && list.Count > 0)
		{
			foreach (GPUInstancingPass renderPass in renderPassList)
			{
				stringBuilder.AppendLine(renderPass.Description());
			}
		}
		return stringBuilder.ToString();
	}
}
