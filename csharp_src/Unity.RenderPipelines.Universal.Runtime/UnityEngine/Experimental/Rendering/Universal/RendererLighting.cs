using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Experimental.Rendering.Universal;

internal static class RendererLighting
{
	private static readonly ShaderTagId k_NormalsRenderingPassName = new ShaderTagId("NormalsRendering");

	private static readonly Color k_NormalClearColor = new Color(0.5f, 0.5f, 1f, 1f);

	private static readonly string k_SpriteLightKeyword = "SPRITE_LIGHT";

	private static readonly string k_UsePointLightCookiesKeyword = "USE_POINT_LIGHT_COOKIES";

	private static readonly string k_LightQualityFastKeyword = "LIGHT_QUALITY_FAST";

	private static readonly string k_UseNormalMap = "USE_NORMAL_MAP";

	private static readonly string k_UseAdditiveBlendingKeyword = "USE_ADDITIVE_BLENDING";

	private const int k_NumberOfLightMaterials = 256;

	private static readonly string[] k_UseBlendStyleKeywords = new string[4] { "USE_SHAPE_LIGHT_TYPE_0", "USE_SHAPE_LIGHT_TYPE_1", "USE_SHAPE_LIGHT_TYPE_2", "USE_SHAPE_LIGHT_TYPE_3" };

	private static readonly string[] k_BlendFactorsPropNames = new string[4] { "_ShapeLightBlendFactors0", "_ShapeLightBlendFactors1", "_ShapeLightBlendFactors2", "_ShapeLightBlendFactors3" };

	private static readonly string[] k_MaskFilterPropNames = new string[4] { "_ShapeLightMaskFilter0", "_ShapeLightMaskFilter1", "_ShapeLightMaskFilter2", "_ShapeLightMaskFilter3" };

	private static readonly string[] k_InvertedFilterPropNames = new string[4] { "_ShapeLightInvertedFilter0", "_ShapeLightInvertedFilter1", "_ShapeLightInvertedFilter2", "_ShapeLightInvertedFilter3" };

	private static Renderer2DData s_Renderer2DData;

	private static RenderingData s_RenderingData;

	private static Light2DBlendStyle[] s_BlendStyles;

	private static RenderTargetHandle[] s_LightRenderTargets;

	private static bool[] s_LightRenderTargetsDirty;

	private static RenderTargetHandle s_ShadowsRenderTarget;

	private static RenderTargetHandle s_NormalsTarget;

	private static Texture s_LightLookupTexture;

	private static Texture s_FalloffLookupTexture;

	private static Material[] s_LightMaterials;

	private static Material[] s_ShadowMaterials;

	private static Material[] s_RemoveSelfShadowMaterials;

	private static GraphicsFormat s_RenderTextureFormatToUse = GraphicsFormat.R8G8B8A8_UNorm;

	private static bool s_HasSetupRenderTextureFormatToUse;

	public static void Setup(RenderingData renderingData, Renderer2DData renderer2DData)
	{
		s_Renderer2DData = renderer2DData;
		s_BlendStyles = renderer2DData.lightBlendStyles;
		s_RenderingData = renderingData;
		if (s_LightRenderTargets == null)
		{
			s_LightRenderTargets = new RenderTargetHandle[s_BlendStyles.Length];
			s_LightRenderTargets[0].Init("_ShapeLightTexture0");
			s_LightRenderTargets[1].Init("_ShapeLightTexture1");
			s_LightRenderTargets[2].Init("_ShapeLightTexture2");
			s_LightRenderTargets[3].Init("_ShapeLightTexture3");
			s_LightRenderTargetsDirty = new bool[s_BlendStyles.Length];
		}
		if (s_NormalsTarget.id == 0)
		{
			s_NormalsTarget.Init("_NormalMap");
		}
		if (s_ShadowsRenderTarget.id == 0)
		{
			s_ShadowsRenderTarget.Init("_ShadowTex");
		}
		if (s_LightMaterials == null)
		{
			s_LightMaterials = new Material[256];
		}
		if (s_ShadowMaterials == null)
		{
			s_ShadowMaterials = new Material[256];
		}
		if (s_RemoveSelfShadowMaterials == null)
		{
			s_RemoveSelfShadowMaterials = new Material[256];
		}
	}

	public static void CreateNormalMapRenderTexture(CommandBuffer cmd)
	{
		if (!s_HasSetupRenderTextureFormatToUse)
		{
			if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, FormatUsage.Blend))
			{
				s_RenderTextureFormatToUse = GraphicsFormat.B10G11R11_UFloatPack32;
			}
			else if (SystemInfo.IsFormatSupported(GraphicsFormat.R16G16B16A16_SFloat, FormatUsage.Blend))
			{
				s_RenderTextureFormatToUse = GraphicsFormat.R16G16B16A16_SFloat;
			}
			s_HasSetupRenderTextureFormatToUse = true;
		}
		RenderTextureDescriptor desc = new RenderTextureDescriptor(s_RenderingData.cameraData.cameraTargetDescriptor.width, s_RenderingData.cameraData.cameraTargetDescriptor.height);
		desc.graphicsFormat = s_RenderTextureFormatToUse;
		desc.useMipMap = false;
		desc.autoGenerateMips = false;
		desc.depthBufferBits = 0;
		desc.msaaSamples = s_RenderingData.cameraData.cameraTargetDescriptor.msaaSamples;
		desc.dimension = TextureDimension.Tex2D;
		cmd.GetTemporaryRT(s_NormalsTarget.id, desc, FilterMode.Bilinear);
	}

	public static void CreateBlendStyleRenderTexture(CommandBuffer cmd, int blendStyleIndex)
	{
		if (!s_HasSetupRenderTextureFormatToUse)
		{
			if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, FormatUsage.Blend))
			{
				s_RenderTextureFormatToUse = GraphicsFormat.B10G11R11_UFloatPack32;
			}
			else if (SystemInfo.IsFormatSupported(GraphicsFormat.R16G16B16A16_SFloat, FormatUsage.Blend))
			{
				s_RenderTextureFormatToUse = GraphicsFormat.R16G16B16A16_SFloat;
			}
			s_HasSetupRenderTextureFormatToUse = true;
		}
		float num = Mathf.Clamp(s_BlendStyles[blendStyleIndex].renderTextureScale, 0.01f, 1f);
		int width = (int)((float)s_RenderingData.cameraData.cameraTargetDescriptor.width * num);
		int height = (int)((float)s_RenderingData.cameraData.cameraTargetDescriptor.height * num);
		RenderTextureDescriptor desc = new RenderTextureDescriptor(width, height);
		desc.graphicsFormat = s_RenderTextureFormatToUse;
		desc.useMipMap = false;
		desc.autoGenerateMips = false;
		desc.depthBufferBits = 0;
		desc.msaaSamples = 1;
		desc.dimension = TextureDimension.Tex2D;
		cmd.GetTemporaryRT(s_LightRenderTargets[blendStyleIndex].id, desc, FilterMode.Bilinear);
		s_LightRenderTargetsDirty[blendStyleIndex] = true;
	}

	public static void EnableBlendStyle(CommandBuffer cmd, int blendStyleIndex, bool enabled)
	{
		string keyword = k_UseBlendStyleKeywords[blendStyleIndex];
		if (enabled)
		{
			cmd.EnableShaderKeyword(keyword);
		}
		else
		{
			cmd.DisableShaderKeyword(keyword);
		}
	}

	public static void CreateShadowRenderTexture(CommandBuffer cmd, int blendStyleIndex)
	{
		float num = Mathf.Clamp(s_BlendStyles[blendStyleIndex].renderTextureScale, 0.01f, 1f);
		int width = (int)((float)s_RenderingData.cameraData.cameraTargetDescriptor.width * num);
		int height = (int)((float)s_RenderingData.cameraData.cameraTargetDescriptor.height * num);
		RenderTextureDescriptor desc = new RenderTextureDescriptor(width, height);
		desc.useMipMap = false;
		desc.autoGenerateMips = false;
		desc.depthBufferBits = 24;
		desc.graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;
		desc.msaaSamples = 1;
		desc.dimension = TextureDimension.Tex2D;
		cmd.GetTemporaryRT(s_ShadowsRenderTarget.id, desc, FilterMode.Bilinear);
	}

	public static void ReleaseShadowRenderTexture(CommandBuffer cmd)
	{
		cmd.ReleaseTemporaryRT(s_ShadowsRenderTarget.id);
	}

	public static void ReleaseRenderTextures(CommandBuffer cmd)
	{
		for (int i = 0; i < s_BlendStyles.Length; i++)
		{
			cmd.ReleaseTemporaryRT(s_LightRenderTargets[i].id);
		}
		cmd.ReleaseTemporaryRT(s_NormalsTarget.id);
		cmd.ReleaseTemporaryRT(s_ShadowsRenderTarget.id);
	}

	private static void RenderShadows(CommandBuffer cmdBuffer, int layerToRender, Light2D light, float shadowIntensity, RenderTargetIdentifier renderTexture, RenderTargetIdentifier depthTexture)
	{
		cmdBuffer.SetGlobalFloat("_ShadowIntensity", 1f - light.shadowIntensity);
		cmdBuffer.SetGlobalFloat("_ShadowVolumeIntensity", 1f - light.shadowVolumeIntensity);
		if (!(shadowIntensity > 0f))
		{
			return;
		}
		CreateShadowRenderTexture(cmdBuffer, light.blendStyleIndex);
		cmdBuffer.SetRenderTarget(s_ShadowsRenderTarget.Identifier());
		cmdBuffer.ClearRenderTarget(clearDepth: true, clearColor: true, Color.black);
		float value = 1.42f * light.GetBoundingSphere().radius;
		cmdBuffer.SetGlobalVector("_LightPos", light.transform.position);
		cmdBuffer.SetGlobalFloat("_ShadowRadius", value);
		Material shadowMaterial = GetShadowMaterial(1);
		Material removeSelfShadowMaterial = GetRemoveSelfShadowMaterial(1);
		List<ShadowCasterGroup2D> shadowCasterGroups = ShadowCasterGroup2DManager.shadowCasterGroups;
		if (shadowCasterGroups != null && shadowCasterGroups.Count > 0)
		{
			int b = -1;
			int num = 0;
			for (int i = 0; i < shadowCasterGroups.Count; i++)
			{
				ShadowCasterGroup2D shadowCasterGroup2D = shadowCasterGroups[i];
				List<ShadowCaster2D> shadowCasters = shadowCasterGroup2D.GetShadowCasters();
				int shadowGroup = shadowCasterGroup2D.GetShadowGroup();
				if (LightUtility.CheckForChange(shadowGroup, ref b) || shadowGroup == 0)
				{
					num++;
					shadowMaterial = GetShadowMaterial(num);
					removeSelfShadowMaterial = GetRemoveSelfShadowMaterial(num);
				}
				if (shadowCasters == null)
				{
					continue;
				}
				for (int j = 0; j < shadowCasters.Count; j++)
				{
					ShadowCaster2D shadowCaster2D = shadowCasters[j];
					if (shadowCaster2D != null && shadowMaterial != null && shadowCaster2D.IsShadowedLayer(layerToRender) && shadowCaster2D.castsShadows)
					{
						cmdBuffer.DrawMesh(shadowCaster2D.mesh, shadowCaster2D.transform.localToWorldMatrix, shadowMaterial);
					}
				}
				for (int k = 0; k < shadowCasters.Count; k++)
				{
					ShadowCaster2D shadowCaster2D2 = shadowCasters[k];
					if (!(shadowCaster2D2 != null) || !(shadowMaterial != null) || !shadowCaster2D2.IsShadowedLayer(layerToRender))
					{
						continue;
					}
					if (shadowCaster2D2.useRendererSilhouette)
					{
						Renderer component = shadowCaster2D2.GetComponent<Renderer>();
						if (component != null)
						{
							if (!shadowCaster2D2.selfShadows)
							{
								cmdBuffer.DrawRenderer(component, removeSelfShadowMaterial);
							}
							else
							{
								cmdBuffer.DrawRenderer(component, shadowMaterial, 0, 1);
							}
						}
					}
					else if (!shadowCaster2D2.selfShadows)
					{
						Matrix4x4 localToWorldMatrix = shadowCaster2D2.transform.localToWorldMatrix;
						cmdBuffer.DrawMesh(shadowCaster2D2.mesh, localToWorldMatrix, removeSelfShadowMaterial);
					}
				}
			}
		}
		ReleaseShadowRenderTexture(cmdBuffer);
		cmdBuffer.SetRenderTarget(renderTexture, depthTexture);
	}

	private static bool RenderLightSet(Camera camera, int blendStyleIndex, CommandBuffer cmdBuffer, int layerToRender, RenderTargetIdentifier renderTexture, List<Light2D> lights)
	{
		bool result = false;
		foreach (Light2D light in lights)
		{
			if (!(light != null) || light.lightType == Light2D.LightType.Global || light.blendStyleIndex != blendStyleIndex || !light.IsLitLayer(layerToRender) || !light.IsLightVisible(camera))
			{
				continue;
			}
			Material lightMaterial = GetLightMaterial(light, isVolume: false);
			if (!(lightMaterial != null))
			{
				continue;
			}
			Mesh mesh = light.GetMesh();
			if (mesh != null)
			{
				RenderShadows(cmdBuffer, layerToRender, light, light.shadowIntensity, renderTexture, renderTexture);
				result = true;
				if (light.lightType == Light2D.LightType.Sprite && light.lightCookieSprite != null && light.lightCookieSprite.texture != null)
				{
					cmdBuffer.SetGlobalTexture("_CookieTex", light.lightCookieSprite.texture);
				}
				cmdBuffer.SetGlobalFloat("_FalloffIntensity", light.falloffIntensity);
				cmdBuffer.SetGlobalFloat("_FalloffDistance", light.shapeLightFalloffSize);
				cmdBuffer.SetGlobalVector("_FalloffOffset", light.shapeLightFalloffOffset);
				cmdBuffer.SetGlobalColor("_LightColor", light.intensity * light.color);
				cmdBuffer.SetGlobalFloat("_VolumeOpacity", light.volumeOpacity);
				if (light.useNormalMap || light.lightType == Light2D.LightType.Point)
				{
					SetPointLightShaderGlobals(cmdBuffer, light);
				}
				if (light.lightType == Light2D.LightType.Parametric || light.lightType == Light2D.LightType.Freeform || light.lightType == Light2D.LightType.Sprite)
				{
					cmdBuffer.DrawMesh(mesh, light.transform.localToWorldMatrix, lightMaterial);
				}
				else if (light.lightType == Light2D.LightType.Point)
				{
					Matrix4x4 matrix = Matrix4x4.TRS(s: new Vector3(light.pointLightOuterRadius, light.pointLightOuterRadius, light.pointLightOuterRadius), pos: light.transform.position, q: Quaternion.identity);
					cmdBuffer.DrawMesh(mesh, matrix, lightMaterial);
				}
			}
		}
		return result;
	}

	private static void RenderLightVolumeSet(Camera camera, int blendStyleIndex, CommandBuffer cmdBuffer, int layerToRender, RenderTargetIdentifier renderTexture, RenderTargetIdentifier depthTexture, List<Light2D> lights)
	{
		if (lights.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < lights.Count; i++)
		{
			Light2D light2D = lights[i];
			int topMostLitLayer = light2D.GetTopMostLitLayer();
			if (layerToRender != topMostLitLayer || !(light2D != null) || light2D.lightType == Light2D.LightType.Global || !(light2D.volumeOpacity > 0f) || light2D.blendStyleIndex != blendStyleIndex || !light2D.IsLitLayer(layerToRender) || !light2D.IsLightVisible(camera))
			{
				continue;
			}
			Material lightMaterial = GetLightMaterial(light2D, isVolume: true);
			if (!(lightMaterial != null))
			{
				continue;
			}
			Mesh mesh = light2D.GetMesh();
			if (mesh != null)
			{
				RenderShadows(cmdBuffer, layerToRender, light2D, light2D.shadowVolumeIntensity, renderTexture, depthTexture);
				if (light2D.lightType == Light2D.LightType.Sprite && light2D.lightCookieSprite != null && light2D.lightCookieSprite.texture != null)
				{
					cmdBuffer.SetGlobalTexture("_CookieTex", light2D.lightCookieSprite.texture);
				}
				cmdBuffer.SetGlobalFloat("_FalloffIntensity", light2D.falloffIntensity);
				cmdBuffer.SetGlobalFloat("_FalloffDistance", light2D.shapeLightFalloffSize);
				cmdBuffer.SetGlobalVector("_FalloffOffset", light2D.shapeLightFalloffOffset);
				cmdBuffer.SetGlobalColor("_LightColor", light2D.intensity * light2D.color);
				cmdBuffer.SetGlobalFloat("_VolumeOpacity", light2D.volumeOpacity);
				if (light2D.useNormalMap || light2D.lightType == Light2D.LightType.Point)
				{
					SetPointLightShaderGlobals(cmdBuffer, light2D);
				}
				if (light2D.lightType == Light2D.LightType.Parametric || light2D.lightType == Light2D.LightType.Freeform || light2D.lightType == Light2D.LightType.Sprite)
				{
					cmdBuffer.DrawMesh(mesh, light2D.transform.localToWorldMatrix, lightMaterial);
				}
				else if (light2D.lightType == Light2D.LightType.Point)
				{
					Matrix4x4 matrix = Matrix4x4.TRS(s: new Vector3(light2D.pointLightOuterRadius, light2D.pointLightOuterRadius, light2D.pointLightOuterRadius), pos: light2D.transform.position, q: Quaternion.identity);
					cmdBuffer.DrawMesh(mesh, matrix, lightMaterial);
				}
			}
		}
	}

	public static void SetShapeLightShaderGlobals(CommandBuffer cmdBuffer)
	{
		for (int i = 0; i < s_BlendStyles.Length && i < k_BlendFactorsPropNames.Length; i++)
		{
			cmdBuffer.SetGlobalVector(k_BlendFactorsPropNames[i], s_BlendStyles[i].blendFactors);
			cmdBuffer.SetGlobalVector(k_MaskFilterPropNames[i], s_BlendStyles[i].maskTextureChannelFilter.mask);
			cmdBuffer.SetGlobalVector(k_InvertedFilterPropNames[i], s_BlendStyles[i].maskTextureChannelFilter.inverted);
		}
		cmdBuffer.SetGlobalTexture("_FalloffLookup", GetFalloffLookupTexture());
	}

	private static Texture GetLightLookupTexture()
	{
		if (s_LightLookupTexture == null)
		{
			s_LightLookupTexture = Light2DLookupTexture.CreatePointLightLookupTexture();
		}
		return s_LightLookupTexture;
	}

	private static Texture GetFalloffLookupTexture()
	{
		if (s_FalloffLookupTexture == null)
		{
			s_FalloffLookupTexture = Light2DLookupTexture.CreateFalloffLookupTexture();
		}
		return s_FalloffLookupTexture;
	}

	public static float GetNormalizedInnerRadius(Light2D light)
	{
		return light.pointLightInnerRadius / light.pointLightOuterRadius;
	}

	public static float GetNormalizedAngle(float angle)
	{
		return angle / 360f;
	}

	public static void GetScaledLightInvMatrix(Light2D light, out Matrix4x4 retMatrix, bool includeRotation)
	{
		float pointLightOuterRadius = light.pointLightOuterRadius;
		Vector3 one = Vector3.one;
		Matrix4x4 m = Matrix4x4.TRS(s: new Vector3(one.x * pointLightOuterRadius, one.y * pointLightOuterRadius, one.z * pointLightOuterRadius), q: (!includeRotation) ? Quaternion.identity : light.transform.rotation, pos: light.transform.position);
		retMatrix = Matrix4x4.Inverse(m);
	}

	public static void SetPointLightShaderGlobals(CommandBuffer cmdBuffer, Light2D light)
	{
		GetScaledLightInvMatrix(light, out var retMatrix, includeRotation: true);
		GetScaledLightInvMatrix(light, out var retMatrix2, includeRotation: false);
		float normalizedInnerRadius = GetNormalizedInnerRadius(light);
		float normalizedAngle = GetNormalizedAngle(light.pointLightInnerAngle);
		float normalizedAngle2 = GetNormalizedAngle(light.pointLightOuterAngle);
		float value = 1f / (1f - normalizedInnerRadius);
		cmdBuffer.SetGlobalVector("_LightPosition", light.transform.position);
		cmdBuffer.SetGlobalMatrix("_LightInvMatrix", retMatrix);
		cmdBuffer.SetGlobalMatrix("_LightNoRotInvMatrix", retMatrix2);
		cmdBuffer.SetGlobalFloat("_InnerRadiusMult", value);
		cmdBuffer.SetGlobalFloat("_OuterAngle", normalizedAngle2);
		cmdBuffer.SetGlobalFloat("_InnerAngleMult", 1f / (normalizedAngle2 - normalizedAngle));
		cmdBuffer.SetGlobalTexture("_LightLookup", GetLightLookupTexture());
		cmdBuffer.SetGlobalTexture("_FalloffLookup", GetFalloffLookupTexture());
		cmdBuffer.SetGlobalFloat("_FalloffIntensity", light.falloffIntensity);
		cmdBuffer.SetGlobalFloat("_IsFullSpotlight", (normalizedAngle == 1f) ? 1f : 0f);
		cmdBuffer.SetGlobalFloat("_LightZDistance", light.pointLightDistance);
		if (light.lightCookieSprite != null && light.lightCookieSprite.texture != null)
		{
			cmdBuffer.SetGlobalTexture("_PointLightCookieTex", light.lightCookieSprite.texture);
		}
	}

	public static void ClearDirtyLighting(CommandBuffer cmdBuffer, uint blendStylesUsed)
	{
		for (int i = 0; i < s_BlendStyles.Length; i++)
		{
			if ((blendStylesUsed & (uint)(1 << i)) != 0 && s_LightRenderTargetsDirty[i])
			{
				cmdBuffer.SetRenderTarget(s_LightRenderTargets[i].Identifier());
				cmdBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.black);
				s_LightRenderTargetsDirty[i] = false;
			}
		}
	}

	public static void RenderNormals(ScriptableRenderContext renderContext, CullingResults cullResults, DrawingSettings drawSettings, FilteringSettings filterSettings, RenderTargetIdentifier depthTarget)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get("Clear Normals");
		commandBuffer.SetRenderTarget(s_NormalsTarget.Identifier(), depthTarget);
		commandBuffer.ClearRenderTarget(clearDepth: true, clearColor: true, k_NormalClearColor);
		renderContext.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
		drawSettings.SetShaderPassName(0, k_NormalsRenderingPassName);
		renderContext.DrawRenderers(cullResults, ref drawSettings, ref filterSettings);
	}

	public static void RenderLights(Camera camera, CommandBuffer cmdBuffer, int layerToRender, uint blendStylesUsed)
	{
		for (int i = 0; i < s_BlendStyles.Length; i++)
		{
			if ((blendStylesUsed & (uint)(1 << i)) != 0)
			{
				string name = s_BlendStyles[i].name;
				cmdBuffer.BeginSample(name);
				cmdBuffer.SetRenderTarget(s_LightRenderTargets[i].Identifier());
				bool flag = false;
				if (!Light2DManager.GetGlobalColor(layerToRender, i, out var color))
				{
					color = Color.black;
				}
				else
				{
					flag = true;
				}
				if (s_LightRenderTargetsDirty[i] || flag)
				{
					cmdBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, color);
				}
				flag |= RenderLightSet(camera, i, cmdBuffer, layerToRender, s_LightRenderTargets[i].Identifier(), Light2D.GetLightsByBlendStyle(i));
				s_LightRenderTargetsDirty[i] = flag;
				cmdBuffer.EndSample(name);
			}
		}
	}

	public static void RenderLightVolumes(Camera camera, CommandBuffer cmdBuffer, int layerToRender, RenderTargetIdentifier renderTarget, RenderTargetIdentifier depthTarget, uint blendStylesUsed)
	{
		for (int i = 0; i < s_BlendStyles.Length; i++)
		{
			if ((blendStylesUsed & (uint)(1 << i)) != 0)
			{
				string name = s_BlendStyles[i].name;
				cmdBuffer.BeginSample(name);
				RenderLightVolumeSet(camera, i, cmdBuffer, layerToRender, renderTarget, depthTarget, Light2D.GetLightsByBlendStyle(i));
				cmdBuffer.EndSample(name);
			}
		}
	}

	private static void SetBlendModes(Material material, BlendMode src, BlendMode dst)
	{
		material.SetFloat("_SrcBlend", (float)src);
		material.SetFloat("_DstBlend", (float)dst);
	}

	private static uint GetLightMaterialIndex(Light2D light, bool isVolume)
	{
		int num = 0;
		uint num2 = (isVolume ? ((uint)(1 << num)) : 0u);
		num++;
		uint num3 = (light.IsShapeLight() ? ((uint)(1 << num)) : 0u);
		num++;
		uint num4 = ((!light.alphaBlendOnOverlap) ? ((uint)(1 << num)) : 0u);
		num++;
		uint num5 = ((light.lightType == Light2D.LightType.Sprite) ? ((uint)(1 << num)) : 0u);
		num++;
		uint num6 = ((!light.IsShapeLight() && light.lightCookieSprite != null && light.lightCookieSprite.texture != null) ? ((uint)(1 << num)) : 0u);
		num++;
		int num7 = ((!light.IsShapeLight() && light.pointLightQuality == Light2D.PointLightQuality.Fast) ? (1 << num) : 0);
		num++;
		uint num8 = (light.useNormalMap ? ((uint)(1 << num)) : 0u);
		return (uint)num7 | num6 | num5 | num4 | num3 | num2 | num8;
	}

	private static Material CreateLightMaterial(Light2D light, bool isVolume)
	{
		bool flag = light.IsShapeLight();
		Material material;
		if (isVolume)
		{
			material = CoreUtils.CreateEngineMaterial(flag ? s_Renderer2DData.shapeLightVolumeShader : s_Renderer2DData.pointLightVolumeShader);
		}
		else
		{
			material = CoreUtils.CreateEngineMaterial(flag ? s_Renderer2DData.shapeLightShader : s_Renderer2DData.pointLightShader);
			if (!light.alphaBlendOnOverlap)
			{
				SetBlendModes(material, BlendMode.One, BlendMode.One);
				material.EnableKeyword(k_UseAdditiveBlendingKeyword);
			}
			else
			{
				SetBlendModes(material, BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha);
			}
		}
		if (light.lightType == Light2D.LightType.Sprite)
		{
			material.EnableKeyword(k_SpriteLightKeyword);
		}
		if (!flag && light.lightCookieSprite != null && light.lightCookieSprite.texture != null)
		{
			material.EnableKeyword(k_UsePointLightCookiesKeyword);
		}
		if (!flag && light.pointLightQuality == Light2D.PointLightQuality.Fast)
		{
			material.EnableKeyword(k_LightQualityFastKeyword);
		}
		if (light.useNormalMap)
		{
			material.EnableKeyword(k_UseNormalMap);
		}
		return material;
	}

	private static Material GetLightMaterial(Light2D light, bool isVolume)
	{
		uint lightMaterialIndex = GetLightMaterialIndex(light, isVolume);
		if (s_LightMaterials[lightMaterialIndex] == null)
		{
			s_LightMaterials[lightMaterialIndex] = CreateLightMaterial(light, isVolume);
		}
		return s_LightMaterials[lightMaterialIndex];
	}

	private static Material GetShadowMaterial(int index)
	{
		int num = index % 255;
		if (s_ShadowMaterials[num] == null)
		{
			s_ShadowMaterials[num] = CoreUtils.CreateEngineMaterial(s_Renderer2DData.shadowGroupShader);
			s_ShadowMaterials[num].SetFloat("_ShadowStencilGroup", index);
		}
		return s_ShadowMaterials[num];
	}

	private static Material GetRemoveSelfShadowMaterial(int index)
	{
		int num = index % 255;
		if (s_RemoveSelfShadowMaterials[num] == null)
		{
			s_RemoveSelfShadowMaterials[num] = CoreUtils.CreateEngineMaterial(s_Renderer2DData.removeSelfShadowShader);
			s_RemoveSelfShadowMaterials[num].SetFloat("_ShadowStencilGroup", index);
		}
		return s_RemoveSelfShadowMaterials[num];
	}
}
