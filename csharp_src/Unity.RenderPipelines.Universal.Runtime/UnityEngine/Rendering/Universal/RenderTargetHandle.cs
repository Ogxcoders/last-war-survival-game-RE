using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public struct RenderTargetHandle
{
	public static readonly RenderTargetHandle CameraTarget = new RenderTargetHandle
	{
		id = -1
	};

	public int id { get; set; }

	public void Init(string shaderProperty)
	{
		id = Shader.PropertyToID(shaderProperty);
	}

	public RenderTargetIdentifier Identifier()
	{
		if (id == -1)
		{
			return BuiltinRenderTextureType.CameraTarget;
		}
		return new RenderTargetIdentifier(id);
	}

	public bool Equals(RenderTargetHandle other)
	{
		return id == other.id;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj is RenderTargetHandle)
		{
			return Equals((RenderTargetHandle)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return id;
	}

	public static bool operator ==(RenderTargetHandle c1, RenderTargetHandle c2)
	{
		return c1.Equals(c2);
	}

	public static bool operator !=(RenderTargetHandle c1, RenderTargetHandle c2)
	{
		return !c1.Equals(c2);
	}
}
