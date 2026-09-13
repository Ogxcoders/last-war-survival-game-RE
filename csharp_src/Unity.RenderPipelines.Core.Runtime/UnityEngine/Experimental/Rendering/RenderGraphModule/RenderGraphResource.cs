using System.Diagnostics;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule;

[DebuggerDisplay("{type} ({handle})")]
public struct RenderGraphResource
{
	internal int handle { get; private set; }

	internal RenderGraphResourceType type { get; private set; }

	internal RenderGraphResource(RenderGraphMutableResource mutableResource)
	{
		handle = mutableResource.handle;
		type = mutableResource.type;
	}

	internal RenderGraphResource(int handle, RenderGraphResourceType type)
	{
		this.handle = handle;
		this.type = type;
	}

	public bool IsValid()
	{
		return type != RenderGraphResourceType.Invalid;
	}
}
