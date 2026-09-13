using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UIShaderCommonHelp : MonoBehaviour
{
	public static int m_Color = Shader.PropertyToID("_Color");

	public static int m_Texture_Scale = Shader.PropertyToID("_Texture_Scale");

	public static int m_Alpha_Intensity = Shader.PropertyToID("_Alpha_Intensity");

	public static int m_Noise_Intensity_X = Shader.PropertyToID("_Noise_Intensity_X");

	public static int m_Noise_Intensity_Y = Shader.PropertyToID("_Noise_Intensity_Y");

	public static int m_Dissolve_Intensity = Shader.PropertyToID("_Dissolve_Intensity");

	public static int m_Dissolve_Intensity_U = Shader.PropertyToID("_Dissolve_Intensity_U");

	public static int m_WorldPosition_Intensity = Shader.PropertyToID("_WorldPosition_Intensity");

	public static int m_Texture_ST = Shader.PropertyToID("_MainTex_ST");

	public static int m_Noise_ST = Shader.PropertyToID("_Noise_ST");

	public static int m_Dissolve_ST = Shader.PropertyToID("_Dissolve_ST");

	public static int m_Mask_ST = Shader.PropertyToID("_Mask_ST");

	public static int m_WorldPosition_ST = Shader.PropertyToID("_WorldPosition_ST");

	public Color _Color;

	public float _Texture_Scale;

	public float _Alpha_Intensity;

	public float _Noise_Intensity_X;

	public float _Noise_Intensity_Y;

	public float _Dissolve_Intensity;

	public float _Dissolve_Intensity_U;

	public Vector4 _WorldPosition_Intensity = new Vector4(0f, 0f, 0f, 0f);

	public Vector4 _Texture_ST = new Vector4(1f, 1f, 0f, 0f);

	public Vector4 _Noise_ST = new Vector4(1f, 1f, 0f, 0f);

	public Vector4 _Dissolve_ST = new Vector4(1f, 1f, 0f, 0f);

	public Vector4 _Mask_ST = new Vector4(1f, 1f, 0f, 0f);

	public Vector4 _WorldPosition_ST = new Vector4(1f, 1f, 0f, 0f);

	private Material mat;

	private void OnEnable()
	{
		MaskableGraphic component = GetComponent<MaskableGraphic>();
		Material materialForRendering = component.materialForRendering;
		if (component != null && materialForRendering.shader.name == "APS/V_ui_shader_common")
		{
			mat = materialForRendering;
			_Color = mat.GetColor(m_Color);
			_Texture_Scale = mat.GetFloat(m_Texture_Scale);
			_Alpha_Intensity = mat.GetFloat(m_Alpha_Intensity);
			_Noise_Intensity_X = mat.GetFloat(m_Noise_Intensity_X);
			_Noise_Intensity_Y = mat.GetFloat(m_Noise_Intensity_Y);
			_Dissolve_Intensity = mat.GetFloat(m_Dissolve_Intensity);
			_Dissolve_Intensity_U = mat.GetFloat(m_Dissolve_Intensity_U);
			_WorldPosition_Intensity = mat.GetVector(m_WorldPosition_Intensity);
			_Texture_ST = mat.GetVector(m_Texture_ST);
			_Noise_ST = mat.GetVector(m_Noise_ST);
			_Dissolve_ST = mat.GetVector(m_Dissolve_ST);
			_Mask_ST = mat.GetVector(m_Mask_ST);
			_WorldPosition_ST = mat.GetVector(m_WorldPosition_ST);
		}
	}

	private void Update()
	{
		if (!(mat == null))
		{
			mat.SetColor(m_Color, _Color);
			mat.SetFloat(m_Texture_Scale, _Texture_Scale);
			mat.SetFloat(m_Alpha_Intensity, _Alpha_Intensity);
			mat.SetFloat(m_Noise_Intensity_X, _Noise_Intensity_X);
			mat.SetFloat(m_Noise_Intensity_Y, _Noise_Intensity_Y);
			mat.SetFloat(m_Dissolve_Intensity, _Dissolve_Intensity);
			mat.SetFloat(m_Dissolve_Intensity_U, _Dissolve_Intensity_U);
			mat.SetVector(m_WorldPosition_Intensity, _WorldPosition_Intensity);
			mat.SetVector(m_Texture_ST, _Texture_ST);
			mat.SetVector(m_Noise_ST, _Noise_ST);
			mat.SetVector(m_Dissolve_ST, _Dissolve_ST);
			mat.SetVector(m_Mask_ST, _Mask_ST);
			mat.SetVector(m_WorldPosition_ST, _WorldPosition_ST);
		}
	}
}
