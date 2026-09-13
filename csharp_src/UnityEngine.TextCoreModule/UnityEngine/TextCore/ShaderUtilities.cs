using System.Linq;

namespace UnityEngine.TextCore;

internal static class ShaderUtilities
{
	public static int ID_MainTex;

	public static int ID_FaceTex;

	public static int ID_FaceColor;

	public static int ID_FaceDilate;

	public static int ID_Shininess;

	public static int ID_UnderlayColor;

	public static int ID_UnderlayOffsetX;

	public static int ID_UnderlayOffsetY;

	public static int ID_UnderlayDilate;

	public static int ID_UnderlaySoftness;

	public static int ID_WeightNormal;

	public static int ID_WeightBold;

	public static int ID_OutlineTex;

	public static int ID_OutlineWidth;

	public static int ID_OutlineSoftness;

	public static int ID_OutlineColor;

	public static int ID_GradientScale;

	public static int ID_ScaleX;

	public static int ID_ScaleY;

	public static int ID_PerspectiveFilter;

	public static int ID_TextureWidth;

	public static int ID_TextureHeight;

	public static int ID_BevelAmount;

	public static int ID_GlowColor;

	public static int ID_GlowOffset;

	public static int ID_GlowPower;

	public static int ID_GlowOuter;

	public static int ID_LightAngle;

	public static int ID_EnvMap;

	public static int ID_EnvMatrix;

	public static int ID_EnvMatrixRotation;

	public static int ID_MaskCoord;

	public static int ID_ClipRect;

	public static int ID_MaskSoftnessX;

	public static int ID_MaskSoftnessY;

	public static int ID_VertexOffsetX;

	public static int ID_VertexOffsetY;

	public static int ID_UseClipRect;

	public static int ID_StencilID;

	public static int ID_StencilOp;

	public static int ID_StencilComp;

	public static int ID_StencilReadMask;

	public static int ID_StencilWriteMask;

	public static int ID_ShaderFlags;

	public static int ID_ScaleRatio_A;

	public static int ID_ScaleRatio_B;

	public static int ID_ScaleRatio_C;

	public static string Keyword_Bevel;

	public static string Keyword_Glow;

	public static string Keyword_Underlay;

	public static string Keyword_Ratios;

	public static string Keyword_MASK_SOFT;

	public static string Keyword_MASK_HARD;

	public static string Keyword_MASK_TEX;

	public static string Keyword_Outline;

	public static string ShaderTag_ZTestMode;

	public static string ShaderTag_CullMode;

	private static float m_clamp;

	public static bool isInitialized;

	private static Shader k_ShaderRef_MobileSDF;

	private static Shader k_ShaderRef_MobileBitmap;

	internal static Shader ShaderRef_MobileSDF
	{
		get
		{
			if (k_ShaderRef_MobileSDF == null)
			{
				k_ShaderRef_MobileSDF = Shader.Find("Hidden/TextCore/Distance Field SSD");
			}
			return k_ShaderRef_MobileSDF;
		}
	}

	internal static Shader ShaderRef_MobileBitmap
	{
		get
		{
			if (k_ShaderRef_MobileBitmap == null)
			{
				k_ShaderRef_MobileBitmap = Shader.Find("Hidden/Internal-GUITextureClipText");
			}
			return k_ShaderRef_MobileBitmap;
		}
	}

	static ShaderUtilities()
	{
		Keyword_Bevel = "BEVEL_ON";
		Keyword_Glow = "GLOW_ON";
		Keyword_Underlay = "UNDERLAY_ON";
		Keyword_Ratios = "RATIOS_OFF";
		Keyword_MASK_SOFT = "MASK_SOFT";
		Keyword_MASK_HARD = "MASK_HARD";
		Keyword_MASK_TEX = "MASK_TEX";
		Keyword_Outline = "OUTLINE_ON";
		ShaderTag_ZTestMode = "unity_GUIZTestMode";
		ShaderTag_CullMode = "_CullMode";
		m_clamp = 1f;
		GetShaderPropertyIDs();
	}

	public static void GetShaderPropertyIDs()
	{
		if (!isInitialized)
		{
			isInitialized = true;
			ID_MainTex = Shader.PropertyToID("_MainTex");
			ID_FaceTex = Shader.PropertyToID("_FaceTex");
			ID_FaceColor = Shader.PropertyToID("_FaceColor");
			ID_FaceDilate = Shader.PropertyToID("_FaceDilate");
			ID_Shininess = Shader.PropertyToID("_FaceShininess");
			ID_UnderlayColor = Shader.PropertyToID("_UnderlayColor");
			ID_UnderlayOffsetX = Shader.PropertyToID("_UnderlayOffsetX");
			ID_UnderlayOffsetY = Shader.PropertyToID("_UnderlayOffsetY");
			ID_UnderlayDilate = Shader.PropertyToID("_UnderlayDilate");
			ID_UnderlaySoftness = Shader.PropertyToID("_UnderlaySoftness");
			ID_WeightNormal = Shader.PropertyToID("_WeightNormal");
			ID_WeightBold = Shader.PropertyToID("_WeightBold");
			ID_OutlineTex = Shader.PropertyToID("_OutlineTex");
			ID_OutlineWidth = Shader.PropertyToID("_OutlineWidth");
			ID_OutlineSoftness = Shader.PropertyToID("_OutlineSoftness");
			ID_OutlineColor = Shader.PropertyToID("_OutlineColor");
			ID_GradientScale = Shader.PropertyToID("_GradientScale");
			ID_ScaleX = Shader.PropertyToID("_ScaleX");
			ID_ScaleY = Shader.PropertyToID("_ScaleY");
			ID_PerspectiveFilter = Shader.PropertyToID("_PerspectiveFilter");
			ID_TextureWidth = Shader.PropertyToID("_TextureWidth");
			ID_TextureHeight = Shader.PropertyToID("_TextureHeight");
			ID_BevelAmount = Shader.PropertyToID("_Bevel");
			ID_LightAngle = Shader.PropertyToID("_LightAngle");
			ID_EnvMap = Shader.PropertyToID("_Cube");
			ID_EnvMatrix = Shader.PropertyToID("_EnvMatrix");
			ID_EnvMatrixRotation = Shader.PropertyToID("_EnvMatrixRotation");
			ID_GlowColor = Shader.PropertyToID("_GlowColor");
			ID_GlowOffset = Shader.PropertyToID("_GlowOffset");
			ID_GlowPower = Shader.PropertyToID("_GlowPower");
			ID_GlowOuter = Shader.PropertyToID("_GlowOuter");
			ID_MaskCoord = Shader.PropertyToID("_MaskCoord");
			ID_ClipRect = Shader.PropertyToID("_ClipRect");
			ID_UseClipRect = Shader.PropertyToID("_UseClipRect");
			ID_MaskSoftnessX = Shader.PropertyToID("_MaskSoftnessX");
			ID_MaskSoftnessY = Shader.PropertyToID("_MaskSoftnessY");
			ID_VertexOffsetX = Shader.PropertyToID("_VertexOffsetX");
			ID_VertexOffsetY = Shader.PropertyToID("_VertexOffsetY");
			ID_StencilID = Shader.PropertyToID("_Stencil");
			ID_StencilOp = Shader.PropertyToID("_StencilOp");
			ID_StencilComp = Shader.PropertyToID("_StencilComp");
			ID_StencilReadMask = Shader.PropertyToID("_StencilReadMask");
			ID_StencilWriteMask = Shader.PropertyToID("_StencilWriteMask");
			ID_ShaderFlags = Shader.PropertyToID("_ShaderFlags");
			ID_ScaleRatio_A = Shader.PropertyToID("_ScaleRatioA");
			ID_ScaleRatio_B = Shader.PropertyToID("_ScaleRatioB");
			ID_ScaleRatio_C = Shader.PropertyToID("_ScaleRatioC");
		}
	}

	public static void UpdateShaderRatios(Material mat)
	{
		bool flag = !mat.shaderKeywords.Contains(Keyword_Ratios);
		float num = mat.GetFloat(ID_GradientScale);
		float num2 = mat.GetFloat(ID_FaceDilate);
		float num3 = mat.GetFloat(ID_OutlineWidth);
		float num4 = mat.GetFloat(ID_OutlineSoftness);
		float num5 = Mathf.Max(mat.GetFloat(ID_WeightNormal), mat.GetFloat(ID_WeightBold)) / 4f;
		float num6 = Mathf.Max(1f, num5 + num2 + num3 + num4);
		float value = (flag ? ((num - m_clamp) / (num * num6)) : 1f);
		mat.SetFloat(ID_ScaleRatio_A, value);
		if (mat.HasProperty(ID_GlowOffset))
		{
			float num7 = mat.GetFloat(ID_GlowOffset);
			float num8 = mat.GetFloat(ID_GlowOuter);
			float num9 = (num5 + num2) * (num - m_clamp);
			num6 = Mathf.Max(1f, num7 + num8);
			float value2 = (flag ? (Mathf.Max(0f, num - m_clamp - num9) / (num * num6)) : 1f);
			mat.SetFloat(ID_ScaleRatio_B, value2);
		}
		if (mat.HasProperty(ID_UnderlayOffsetX))
		{
			float f = mat.GetFloat(ID_UnderlayOffsetX);
			float f2 = mat.GetFloat(ID_UnderlayOffsetY);
			float num10 = mat.GetFloat(ID_UnderlayDilate);
			float num11 = mat.GetFloat(ID_UnderlaySoftness);
			float num12 = (num5 + num2) * (num - m_clamp);
			num6 = Mathf.Max(1f, Mathf.Max(Mathf.Abs(f), Mathf.Abs(f2)) + num10 + num11);
			float value3 = (flag ? (Mathf.Max(0f, num - m_clamp - num12) / (num * num6)) : 1f);
			mat.SetFloat(ID_ScaleRatio_C, value3);
		}
	}

	public static bool IsMaskingEnabled(Material material)
	{
		if (material == null || !material.HasProperty(ID_ClipRect))
		{
			return false;
		}
		if (material.shaderKeywords.Contains(Keyword_MASK_SOFT) || material.shaderKeywords.Contains(Keyword_MASK_HARD) || material.shaderKeywords.Contains(Keyword_MASK_TEX))
		{
			return true;
		}
		return false;
	}

	public static float GetPadding(Material material, bool enableExtraPadding, bool isBold)
	{
		if (!isInitialized)
		{
			GetShaderPropertyIDs();
		}
		if (material == null)
		{
			return 0f;
		}
		int num = (enableExtraPadding ? 4 : 0);
		if (!material.HasProperty(ID_GradientScale))
		{
			return num;
		}
		Vector4 zero = Vector4.zero;
		Vector4 zero2 = Vector4.zero;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		UpdateShaderRatios(material);
		string[] shaderKeywords = material.shaderKeywords;
		if (material.HasProperty(ID_ScaleRatio_A))
		{
			num5 = material.GetFloat(ID_ScaleRatio_A);
		}
		if (material.HasProperty(ID_FaceDilate))
		{
			num2 = material.GetFloat(ID_FaceDilate) * num5;
		}
		if (material.HasProperty(ID_OutlineSoftness))
		{
			num3 = material.GetFloat(ID_OutlineSoftness) * num5;
		}
		if (material.HasProperty(ID_OutlineWidth))
		{
			num4 = material.GetFloat(ID_OutlineWidth) * num5;
		}
		float a = num4 + num3 + num2;
		if (material.HasProperty(ID_GlowOffset) && shaderKeywords.Contains(Keyword_Glow))
		{
			if (material.HasProperty(ID_ScaleRatio_B))
			{
				num6 = material.GetFloat(ID_ScaleRatio_B);
			}
			num8 = material.GetFloat(ID_GlowOffset) * num6;
			num9 = material.GetFloat(ID_GlowOuter) * num6;
		}
		a = Mathf.Max(a, num2 + num8 + num9);
		if (material.HasProperty(ID_UnderlaySoftness) && shaderKeywords.Contains(Keyword_Underlay))
		{
			if (material.HasProperty(ID_ScaleRatio_C))
			{
				num7 = material.GetFloat(ID_ScaleRatio_C);
			}
			float num10 = material.GetFloat(ID_UnderlayOffsetX) * num7;
			float num11 = material.GetFloat(ID_UnderlayOffsetY) * num7;
			float num12 = material.GetFloat(ID_UnderlayDilate) * num7;
			float num13 = material.GetFloat(ID_UnderlaySoftness) * num7;
			zero.x = Mathf.Max(zero.x, num2 + num12 + num13 - num10);
			zero.y = Mathf.Max(zero.y, num2 + num12 + num13 - num11);
			zero.z = Mathf.Max(zero.z, num2 + num12 + num13 + num10);
			zero.w = Mathf.Max(zero.w, num2 + num12 + num13 + num11);
		}
		zero.x = Mathf.Max(zero.x, a);
		zero.y = Mathf.Max(zero.y, a);
		zero.z = Mathf.Max(zero.z, a);
		zero.w = Mathf.Max(zero.w, a);
		zero.x += num;
		zero.y += num;
		zero.z += num;
		zero.w += num;
		zero.x = Mathf.Min(zero.x, 1f);
		zero.y = Mathf.Min(zero.y, 1f);
		zero.z = Mathf.Min(zero.z, 1f);
		zero.w = Mathf.Min(zero.w, 1f);
		zero2.x = ((zero2.x < zero.x) ? zero.x : zero2.x);
		zero2.y = ((zero2.y < zero.y) ? zero.y : zero2.y);
		zero2.z = ((zero2.z < zero.z) ? zero.z : zero2.z);
		zero2.w = ((zero2.w < zero.w) ? zero.w : zero2.w);
		float num14 = material.GetFloat(ID_GradientScale);
		zero *= num14;
		a = Mathf.Max(zero.x, zero.y);
		a = Mathf.Max(zero.z, a);
		a = Mathf.Max(zero.w, a);
		return a + 0.5f;
	}
}
