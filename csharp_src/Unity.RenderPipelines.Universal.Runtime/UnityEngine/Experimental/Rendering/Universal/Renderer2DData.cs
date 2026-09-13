using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace UnityEngine.Experimental.Rendering.Universal;

[Serializable]
[ReloadGroup]
[ExcludeFromPreset]
[MovedFrom("UnityEngine.Experimental.Rendering.LWRP")]
public class Renderer2DData : ScriptableRendererData
{
	public enum Renderer2DDefaultMaterialType
	{
		Lit,
		Unlit,
		Custom
	}

	[SerializeField]
	private TransparencySortMode m_TransparencySortMode;

	[SerializeField]
	private Vector3 m_TransparencySortAxis = Vector3.up;

	[SerializeField]
	private float m_HDREmulationScale = 1f;

	[SerializeField]
	[FormerlySerializedAs("m_LightOperations")]
	private Light2DBlendStyle[] m_LightBlendStyles;

	[SerializeField]
	private bool m_UseDepthStencilBuffer = true;

	[SerializeField]
	[Reload("Shaders/2D/Light2D-Shape.shader", ReloadAttribute.Package.Root)]
	private Shader m_ShapeLightShader;

	[SerializeField]
	[Reload("Shaders/2D/Light2D-Shape-Volumetric.shader", ReloadAttribute.Package.Root)]
	private Shader m_ShapeLightVolumeShader;

	[SerializeField]
	[Reload("Shaders/2D/Light2D-Point.shader", ReloadAttribute.Package.Root)]
	private Shader m_PointLightShader;

	[SerializeField]
	[Reload("Shaders/2D/Light2D-Point-Volumetric.shader", ReloadAttribute.Package.Root)]
	private Shader m_PointLightVolumeShader;

	[SerializeField]
	[Reload("Shaders/Utils/Blit.shader", ReloadAttribute.Package.Root)]
	private Shader m_BlitShader;

	[SerializeField]
	[Reload("Shaders/2D/ShadowGroup2D.shader", ReloadAttribute.Package.Root)]
	private Shader m_ShadowGroupShader;

	[SerializeField]
	[Reload("Shaders/2D/Shadow2DRemoveSelf.shader", ReloadAttribute.Package.Root)]
	private Shader m_RemoveSelfShadowShader;

	[SerializeField]
	[Reload("Runtime/Data/PostProcessData.asset", ReloadAttribute.Package.Root)]
	private PostProcessData m_PostProcessData;

	public float hdrEmulationScale => m_HDREmulationScale;

	public Light2DBlendStyle[] lightBlendStyles => m_LightBlendStyles;

	internal bool useDepthStencilBuffer => m_UseDepthStencilBuffer;

	internal Shader shapeLightShader => m_ShapeLightShader;

	internal Shader shapeLightVolumeShader => m_ShapeLightVolumeShader;

	internal Shader pointLightShader => m_PointLightShader;

	internal Shader pointLightVolumeShader => m_PointLightVolumeShader;

	internal Shader blitShader => m_BlitShader;

	internal Shader shadowGroupShader => m_ShadowGroupShader;

	internal Shader removeSelfShadowShader => m_RemoveSelfShadowShader;

	internal PostProcessData postProcessData => m_PostProcessData;

	internal TransparencySortMode transparencySortMode => m_TransparencySortMode;

	internal Vector3 transparencySortAxis => m_TransparencySortAxis;

	protected override ScriptableRenderer Create()
	{
		return new Renderer2D(this);
	}
}
