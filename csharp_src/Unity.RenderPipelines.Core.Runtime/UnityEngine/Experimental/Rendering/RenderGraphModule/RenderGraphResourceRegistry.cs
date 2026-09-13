using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule;

public class RenderGraphResourceRegistry
{
	internal struct TextureResource
	{
		public TextureDesc desc;

		public bool imported;

		public RTHandle rt;

		public int cachedHash;

		public int firstWritePassIndex;

		public int lastReadPassIndex;

		public int shaderProperty;

		public bool wasReleased;

		internal TextureResource(RTHandle rt, int shaderProperty)
		{
			this = default(TextureResource);
			Reset();
			this.rt = rt;
			imported = true;
			this.shaderProperty = shaderProperty;
		}

		internal TextureResource(in TextureDesc desc, int shaderProperty)
		{
			this = default(TextureResource);
			Reset();
			this.desc = desc;
			this.shaderProperty = shaderProperty;
		}

		private void Reset()
		{
			imported = false;
			rt = null;
			cachedHash = -1;
			firstWritePassIndex = int.MaxValue;
			lastReadPassIndex = -1;
			wasReleased = false;
		}
	}

	internal struct RendererListResource
	{
		public RendererListDesc desc;

		public RendererList rendererList;

		internal RendererListResource(in RendererListDesc desc)
		{
			this.desc = desc;
			rendererList = default(RendererList);
		}
	}

	private static readonly ShaderTagId s_EmptyName = new ShaderTagId("");

	private DynamicArray<TextureResource> m_TextureResources = new DynamicArray<TextureResource>();

	private Dictionary<int, Stack<RTHandle>> m_TexturePool = new Dictionary<int, Stack<RTHandle>>();

	private DynamicArray<RendererListResource> m_RendererListResources = new DynamicArray<RendererListResource>();

	private RTHandleSystem m_RTHandleSystem = new RTHandleSystem();

	private RenderGraphDebugParams m_RenderGraphDebug;

	private RenderGraphLogger m_Logger;

	private List<(int, RTHandle)> m_AllocatedTextures = new List<(int, RTHandle)>();

	public RTHandle GetTexture(in RenderGraphResource handle)
	{
		return m_TextureResources[handle.handle].rt;
	}

	public RendererList GetRendererList(in RenderGraphResource handle)
	{
		return m_RendererListResources[handle.handle].rendererList;
	}

	private RenderGraphResourceRegistry()
	{
	}

	internal RenderGraphResourceRegistry(bool supportMSAA, MSAASamples initialSampleCount, RenderGraphDebugParams renderGraphDebug, RenderGraphLogger logger)
	{
		m_RTHandleSystem.Initialize(Screen.width, Screen.height, supportMSAA, initialSampleCount);
		m_RenderGraphDebug = renderGraphDebug;
		m_Logger = logger;
	}

	internal void SetRTHandleReferenceSize(int width, int height, MSAASamples msaaSamples)
	{
		m_RTHandleSystem.SetReferenceSize(width, height, msaaSamples);
	}

	internal RTHandleProperties GetRTHandleProperties()
	{
		return m_RTHandleSystem.rtHandleProperties;
	}

	internal RenderGraphMutableResource ImportTexture(RTHandle rt, int shaderProperty = 0)
	{
		return new RenderGraphMutableResource(m_TextureResources.Add(new TextureResource(rt, shaderProperty)), RenderGraphResourceType.Texture);
	}

	internal RenderGraphMutableResource CreateTexture(in TextureDesc desc, int shaderProperty = 0)
	{
		ValidateTextureDesc(in desc);
		return new RenderGraphMutableResource(m_TextureResources.Add(new TextureResource(in desc, shaderProperty)), RenderGraphResourceType.Texture);
	}

	internal void UpdateTextureFirstWrite(RenderGraphResource tex, int passIndex)
	{
		ref TextureResource textureResource = ref GetTextureResource(tex);
		textureResource.firstWritePassIndex = Math.Min(passIndex, textureResource.firstWritePassIndex);
	}

	internal void UpdateTextureLastRead(RenderGraphResource tex, int passIndex)
	{
		ref TextureResource textureResource = ref GetTextureResource(tex);
		textureResource.lastReadPassIndex = Math.Max(passIndex, textureResource.lastReadPassIndex);
	}

	private ref TextureResource GetTextureResource(RenderGraphResource res)
	{
		return ref m_TextureResources[res.handle];
	}

	internal TextureDesc GetTextureResourceDesc(RenderGraphResource res)
	{
		return m_TextureResources[res.handle].desc;
	}

	internal RenderGraphResource CreateRendererList(in RendererListDesc desc)
	{
		ValidateRendererListDesc(in desc);
		return new RenderGraphResource(m_RendererListResources.Add(new RendererListResource(in desc)), RenderGraphResourceType.RendererList);
	}

	internal void CreateAndClearTexturesForPass(RenderGraphContext rgContext, int passIndex, List<RenderGraphMutableResource> textures)
	{
		foreach (RenderGraphMutableResource texture in textures)
		{
			ref TextureResource textureResource = ref GetTextureResource(texture);
			if (textureResource.imported || textureResource.firstWritePassIndex != passIndex)
			{
				continue;
			}
			CreateTextureForPass(ref textureResource);
			if (textureResource.desc.clearBuffer || m_RenderGraphDebug.clearRenderTargetsAtCreation)
			{
				bool flag = m_RenderGraphDebug.clearRenderTargetsAtCreation && !textureResource.desc.clearBuffer;
				using (new ProfilingScope(rgContext.cmd, ProfilingSampler.Get(RenderGraphProfileId.RenderGraphClear)))
				{
					ClearFlag clearFlag = ((textureResource.desc.depthBufferBits == DepthBits.None) ? ClearFlag.Color : ClearFlag.Depth);
					Color clearColor = (flag ? Color.magenta : textureResource.desc.clearColor);
					CoreUtils.SetRenderTarget(rgContext.cmd, textureResource.rt, clearFlag, clearColor);
				}
			}
			LogTextureCreation(textureResource.rt, textureResource.desc.clearBuffer || m_RenderGraphDebug.clearRenderTargetsAtCreation);
		}
	}

	private void CreateTextureForPass(ref TextureResource resource)
	{
		TextureDesc desc = resource.desc;
		int hashCode = desc.GetHashCode();
		if (resource.rt != null)
		{
			throw new InvalidOperationException($"Trying to create an already created texture ({resource.desc.name}). Texture was probably declared for writing more than once.");
		}
		resource.rt = null;
		if (!TryGetRenderTarget(hashCode, out resource.rt))
		{
			switch (desc.sizeMode)
			{
			case TextureSizeMode.Explicit:
				resource.rt = m_RTHandleSystem.Alloc(desc.width, desc.height, desc.slices, desc.depthBufferBits, desc.colorFormat, desc.filterMode, desc.wrapMode, desc.dimension, desc.enableRandomWrite, desc.useMipMap, desc.autoGenerateMips, desc.isShadowMap, desc.anisoLevel, desc.mipMapBias, desc.msaaSamples, desc.bindTextureMS, desc.useDynamicScale, desc.memoryless, desc.name);
				break;
			case TextureSizeMode.Scale:
				resource.rt = m_RTHandleSystem.Alloc(desc.scale, desc.slices, desc.depthBufferBits, desc.colorFormat, desc.filterMode, desc.wrapMode, desc.dimension, desc.enableRandomWrite, desc.useMipMap, desc.autoGenerateMips, desc.isShadowMap, desc.anisoLevel, desc.mipMapBias, desc.enableMSAA, desc.bindTextureMS, desc.useDynamicScale, desc.memoryless, desc.name);
				break;
			case TextureSizeMode.Functor:
				resource.rt = m_RTHandleSystem.Alloc(desc.func, desc.slices, desc.depthBufferBits, desc.colorFormat, desc.filterMode, desc.wrapMode, desc.dimension, desc.enableRandomWrite, desc.useMipMap, desc.autoGenerateMips, desc.isShadowMap, desc.anisoLevel, desc.mipMapBias, desc.enableMSAA, desc.bindTextureMS, desc.useDynamicScale, desc.memoryless, desc.name);
				break;
			}
		}
		resource.cachedHash = hashCode;
	}

	private void SetGlobalTextures(RenderGraphContext rgContext, List<RenderGraphResource> textures, bool bindDummyTexture)
	{
		foreach (RenderGraphResource texture in textures)
		{
			TextureResource textureResource = GetTextureResource(texture);
			if (textureResource.shaderProperty != 0)
			{
				if (textureResource.rt == null)
				{
					throw new InvalidOperationException($"Trying to set Global Texture parameter for \"{textureResource.desc.name}\" which was never created.\nCheck that at least one write operation happens before reading it.");
				}
				rgContext.cmd.SetGlobalTexture(textureResource.shaderProperty, bindDummyTexture ? TextureXR.GetMagentaTexture() : textureResource.rt);
			}
		}
	}

	internal void PreRenderPassSetGlobalTextures(RenderGraphContext rgContext, List<RenderGraphResource> textures)
	{
		SetGlobalTextures(rgContext, textures, bindDummyTexture: false);
	}

	internal void PostRenderPassUnbindGlobalTextures(RenderGraphContext rgContext, List<RenderGraphResource> textures)
	{
		SetGlobalTextures(rgContext, textures, bindDummyTexture: true);
	}

	internal void ReleaseTexturesForPass(RenderGraphContext rgContext, int passIndex, List<RenderGraphResource> readTextures, List<RenderGraphMutableResource> writtenTextures)
	{
		foreach (RenderGraphResource readTexture in readTextures)
		{
			RenderGraphResource handle = readTexture;
			ref TextureResource textureResource = ref GetTextureResource(handle);
			if (textureResource.imported || textureResource.lastReadPassIndex != passIndex)
			{
				continue;
			}
			if (m_RenderGraphDebug.clearRenderTargetsAtRelease)
			{
				using (new ProfilingScope(rgContext.cmd, ProfilingSampler.Get(RenderGraphProfileId.RenderGraphClearDebug)))
				{
					ClearFlag clearFlag = ((textureResource.desc.depthBufferBits == DepthBits.None) ? ClearFlag.Color : ClearFlag.Depth);
					CoreUtils.SetRenderTarget(rgContext.cmd, GetTexture(in handle), clearFlag, Color.magenta);
				}
			}
			ReleaseTextureForPass(handle);
		}
		foreach (RenderGraphMutableResource writtenTexture in writtenTextures)
		{
			ref TextureResource textureResource2 = ref GetTextureResource(writtenTexture);
			if (!textureResource2.imported && textureResource2.lastReadPassIndex <= passIndex)
			{
				ReleaseTextureForPass(writtenTexture);
			}
		}
	}

	private void ReleaseTextureForPass(RenderGraphResource res)
	{
		ref TextureResource reference = ref m_TextureResources[res.handle];
		if (reference.rt != null)
		{
			LogTextureRelease(reference.rt);
			ReleaseTextureResource(reference.cachedHash, reference.rt);
			reference.cachedHash = -1;
			reference.rt = null;
			reference.wasReleased = true;
		}
	}

	private void ReleaseTextureResource(int hash, RTHandle rt)
	{
		if (!m_TexturePool.TryGetValue(hash, out var value))
		{
			value = new Stack<RTHandle>();
			m_TexturePool.Add(hash, value);
		}
		value.Push(rt);
	}

	private void ValidateTextureDesc(in TextureDesc desc)
	{
	}

	private void ValidateRendererListDesc(in RendererListDesc desc)
	{
	}

	private bool TryGetRenderTarget(int hashCode, out RTHandle rt)
	{
		if (m_TexturePool.TryGetValue(hashCode, out var value) && value.Count > 0)
		{
			rt = value.Pop();
			return true;
		}
		rt = null;
		return false;
	}

	internal void CreateRendererLists(List<RenderGraphResource> rendererLists)
	{
		foreach (RenderGraphResource rendererList2 in rendererLists)
		{
			ref RendererListResource reference = ref m_RendererListResources[rendererList2.handle];
			RendererList rendererList = RendererList.Create(in reference.desc);
			reference.rendererList = rendererList;
		}
	}

	internal void Clear()
	{
		LogResources();
		m_TextureResources.Clear();
		m_RendererListResources.Clear();
	}

	internal void Cleanup()
	{
		foreach (KeyValuePair<int, Stack<RTHandle>> item in m_TexturePool)
		{
			foreach (RTHandle item2 in item.Value)
			{
				m_RTHandleSystem.Release(item2);
			}
		}
	}

	private void LogTextureCreation(RTHandle rt, bool cleared)
	{
		if (m_RenderGraphDebug.logFrameInformation)
		{
			m_Logger.LogLine("Created Texture: {0} (Cleared: {1})", rt.rt.name, cleared);
		}
	}

	private void LogTextureRelease(RTHandle rt)
	{
		if (m_RenderGraphDebug.logFrameInformation)
		{
			m_Logger.LogLine("Released Texture: {0}", rt.rt.name);
		}
	}

	private void LogResources()
	{
		if (!m_RenderGraphDebug.logResources)
		{
			return;
		}
		m_Logger.LogLine("==== Allocated Resources ====\n");
		List<string> list = new List<string>();
		foreach (KeyValuePair<int, Stack<RTHandle>> item in m_TexturePool)
		{
			foreach (RTHandle item2 in item.Value)
			{
				list.Add(item2.rt.name);
			}
		}
		list.Sort();
		int num = 0;
		foreach (string item3 in list)
		{
			m_Logger.LogLine("[{0}] {1}", num++, item3);
		}
	}
}
