using UnityEngine;

namespace FibMatrix.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainFoliageFxField : MonoBehaviour
{
	public Texture fieldTex;

	public string rendererTexName = "_BaseMap";

	[Tooltip("覆盖区域是本模型包围盒为中心的正方形，区域外数值取决于贴图wrapMode")]
	public float fieldSize = 20f;

	private Renderer m_RendererForPainting;

	[Range(-1f, 1.5f)]
	public float brightnessOffset;

	[Range(0f, 2f)]
	public float brightnessTexValueScale = 1f;

	public bool enableGrassGradient;

	public Texture grassGradientTex;

	private const float PlaneSize = 10f;

	private void OnEnable()
	{
		if (fieldTex == null)
		{
			fieldTex = Texture2D.grayTexture;
		}
		if (Application.isEditor)
		{
			m_RendererForPainting = GetComponent<MeshRenderer>();
		}
		if (grassGradientTex != null && grassGradientTex.wrapMode != TextureWrapMode.Clamp)
		{
			grassGradientTex.wrapMode = TextureWrapMode.Clamp;
		}
		SetShaderParam();
	}

	private void OnDisable()
	{
		Shader.SetGlobalFloat("_GlobalPlaneFieldValueOffset", 0f);
		Shader.SetGlobalFloat("_GlobalPlaneFieldValueScale", 0f);
		Shader.SetGlobalTexture("_GlobalGrassGradientTex", Texture2D.whiteTexture);
	}

	private void SetShaderParam()
	{
		Shader.SetGlobalTexture("_GlobalTerrainFoliageFxFieldTex", fieldTex);
		Matrix4x4 value = Matrix4x4.Rotate(Quaternion.Euler(0f, -180f, 0f)) * base.transform.worldToLocalMatrix;
		Shader.SetGlobalFloat("_GlobalPlaneFieldSize", fieldSize);
		Shader.SetGlobalMatrix("_GlobalPlaneFieldWorldToLocalMatrix", value);
		Shader.SetGlobalFloat("_GlobalPlaneFieldValueOffset", brightnessOffset);
		Shader.SetGlobalFloat("_GlobalPlaneFieldValueScale", brightnessTexValueScale);
		Shader.SetGlobalTexture("_GlobalGrassGradientTex", (grassGradientTex != null && enableGrassGradient) ? grassGradientTex : Texture2D.whiteTexture);
	}

	private void Update()
	{
		if (Application.isEditor)
		{
			base.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
			fieldSize = 10f;
			Texture texture = m_RendererForPainting.sharedMaterial.GetTexture(rendererTexName);
			if (texture != null)
			{
				fieldTex = texture;
			}
		}
		SetShaderParam();
	}
}
