using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule;

public ref struct RenderGraphContext
{
	public ScriptableRenderContext renderContext;

	public CommandBuffer cmd;

	public RenderGraphObjectPool renderGraphPool;

	public RenderGraphResourceRegistry resources;
}
