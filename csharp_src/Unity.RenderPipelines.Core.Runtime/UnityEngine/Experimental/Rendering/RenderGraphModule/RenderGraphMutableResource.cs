using System.Diagnostics;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule;

[DebuggerDisplay("{type} ({handle})")]
public struct RenderGraphMutableResource
{
	internal int handle { get; private set; }

	internal RenderGraphResourceType type { get; private set; }

	internal int version { get; private set; }

	internal RenderGraphMutableResource(int handle, RenderGraphResourceType type)
	{
		this.handle = handle;
		this.type = type;
		version = 0;
	}

	internal RenderGraphMutableResource(RenderGraphMutableResource other)
	{
		handle = other.handle;
		type = other.type;
		version = other.version + 1;
	}

	public static implicit operator RenderGraphResource(RenderGraphMutableResource handle)
	{
		return new RenderGraphResource(handle);
	}

	internal bool IsValid()
	{
		return type != RenderGraphResourceType.Invalid;
	}
}
