using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[Serializable]
[ReloadGroup]
[ExcludeFromPreset]
[MovedFrom("UnityEngine.Rendering.LWRP")]
public class ForwardRendererData : ScriptableRendererData
{
	[Serializable]
	[ReloadGroup]
	public sealed class ShaderResources
	{
		[Reload("Shaders/Utils/Blit.shader", ReloadAttribute.Package.Root)]
		public Shader blitPS;

		[Reload("Shaders/Utils/CopyDepth.shader", ReloadAttribute.Package.Root)]
		public Shader copyDepthPS;

		[Reload("Shaders/Utils/ScreenSpaceShadows.shader", ReloadAttribute.Package.Root)]
		public Shader screenSpaceShadowPS;

		[Reload("Shaders/Utils/Sampling.shader", ReloadAttribute.Package.Root)]
		public Shader samplingPS;

		[Reload("Shaders/Utils/FallbackError.shader", ReloadAttribute.Package.Root)]
		public Shader fallbackErrorPS;
	}

	[Reload("Runtime/Data/PostProcessData.asset", ReloadAttribute.Package.Root)]
	public PostProcessData postProcessData;

	public ShaderResources shaders;

	[SerializeField]
	private LayerMask m_OpaqueLayerMask = -1;

	[SerializeField]
	private LayerMask m_TransparentLayerMask = -1;

	[SerializeField]
	private StencilStateData m_DefaultStencilState = new StencilStateData();

	[SerializeField]
	private bool m_ShadowTransparentReceive = true;

	public LayerMask opaqueLayerMask
	{
		get
		{
			return m_OpaqueLayerMask;
		}
		set
		{
			SetDirty();
			m_OpaqueLayerMask = value;
		}
	}

	public LayerMask transparentLayerMask
	{
		get
		{
			return m_TransparentLayerMask;
		}
		set
		{
			SetDirty();
			m_TransparentLayerMask = value;
		}
	}

	public StencilStateData defaultStencilState
	{
		get
		{
			return m_DefaultStencilState;
		}
		set
		{
			SetDirty();
			m_DefaultStencilState = value;
		}
	}

	public bool shadowTransparentReceive
	{
		get
		{
			return m_ShadowTransparentReceive;
		}
		set
		{
			SetDirty();
			m_ShadowTransparentReceive = value;
		}
	}

	protected override ScriptableRenderer Create()
	{
		return new ForwardRenderer(this);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_ = shaders;
	}
}
