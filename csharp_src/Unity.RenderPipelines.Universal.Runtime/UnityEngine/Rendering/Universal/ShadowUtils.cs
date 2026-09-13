using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public static class ShadowUtils
{
	private static readonly RenderTextureFormat m_ShadowmapFormat;

	private static readonly bool m_ForceShadowPointSampling;

	static ShadowUtils()
	{
		m_ShadowmapFormat = ((!RenderingUtils.SupportsRenderTextureFormat(RenderTextureFormat.Shadowmap) || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2) ? RenderTextureFormat.Depth : RenderTextureFormat.Shadowmap);
		m_ForceShadowPointSampling = SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal && GraphicsSettings.HasShaderDefine(Graphics.activeTier, BuiltinShaderDefine.UNITY_METAL_SHADOWS_USE_POINT_FILTERING);
	}

	public static bool ExtractDirectionalLightMatrix(ref CullingResults cullResults, ref ShadowData shadowData, int shadowLightIndex, int cascadeIndex, int shadowmapWidth, int shadowmapHeight, int shadowResolution, float shadowNearPlane, out Vector4 cascadeSplitDistance, out ShadowSliceData shadowSliceData, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix)
	{
		ShadowSplitData shadowSplitData;
		bool result = cullResults.ComputeDirectionalShadowMatricesAndCullingPrimitives(shadowLightIndex, cascadeIndex, shadowData.mainLightShadowCascadesCount, shadowData.mainLightShadowCascadesSplit, shadowResolution, shadowNearPlane, out viewMatrix, out projMatrix, out shadowSplitData);
		cascadeSplitDistance = shadowSplitData.cullingSphere;
		shadowSliceData.offsetX = cascadeIndex % 2 * shadowResolution;
		shadowSliceData.offsetY = cascadeIndex / 2 * shadowResolution;
		shadowSliceData.resolution = shadowResolution;
		shadowSliceData.viewMatrix = viewMatrix;
		shadowSliceData.projectionMatrix = projMatrix;
		shadowSliceData.shadowTransform = GetShadowTransform(projMatrix, viewMatrix);
		if (shadowData.mainLightShadowCascadesCount > 1)
		{
			ApplySliceTransform(ref shadowSliceData, shadowmapWidth, shadowmapHeight);
		}
		return result;
	}

	public static bool ExtractSpotLightMatrix(ref CullingResults cullResults, ref ShadowData shadowData, int shadowLightIndex, out Matrix4x4 shadowMatrix, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix)
	{
		ShadowSplitData shadowSplitData;
		bool result = cullResults.ComputeSpotShadowMatricesAndCullingPrimitives(shadowLightIndex, out viewMatrix, out projMatrix, out shadowSplitData);
		shadowMatrix = GetShadowTransform(projMatrix, viewMatrix);
		return result;
	}

	public static void RenderShadowSlice(CommandBuffer cmd, ref ScriptableRenderContext context, ref ShadowSliceData shadowSliceData, ref ShadowDrawingSettings settings, Matrix4x4 proj, Matrix4x4 view)
	{
		cmd.SetViewport(new Rect(shadowSliceData.offsetX, shadowSliceData.offsetY, shadowSliceData.resolution, shadowSliceData.resolution));
		cmd.EnableScissorRect(new Rect(shadowSliceData.offsetX + 4, shadowSliceData.offsetY + 4, shadowSliceData.resolution - 8, shadowSliceData.resolution - 8));
		cmd.SetViewProjectionMatrices(view, proj);
		context.ExecuteCommandBuffer(cmd);
		cmd.Clear();
		context.DrawShadows(ref settings);
		cmd.DisableScissorRect();
		context.ExecuteCommandBuffer(cmd);
		cmd.Clear();
	}

	public static void RenderShadowSlice(CommandBuffer cmd, ref ScriptableRenderContext context, ref ShadowSliceData shadowSliceData, ref ShadowDrawingSettings settings)
	{
		RenderShadowSlice(cmd, ref context, ref shadowSliceData, ref settings, shadowSliceData.projectionMatrix, shadowSliceData.viewMatrix);
	}

	public static int GetMaxTileResolutionInAtlas(int atlasWidth, int atlasHeight, int tileCount)
	{
		int num = Mathf.Min(atlasWidth, atlasHeight);
		for (int num2 = atlasWidth / num * atlasHeight / num; num2 < tileCount; num2 = atlasWidth / num * atlasHeight / num)
		{
			num >>= 1;
		}
		return num;
	}

	public static void ApplySliceTransform(ref ShadowSliceData shadowSliceData, int atlasWidth, int atlasHeight)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		float num = 1f / (float)atlasWidth;
		float num2 = 1f / (float)atlasHeight;
		identity.m00 = (float)shadowSliceData.resolution * num;
		identity.m11 = (float)shadowSliceData.resolution * num2;
		identity.m03 = (float)shadowSliceData.offsetX * num;
		identity.m13 = (float)shadowSliceData.offsetY * num2;
		shadowSliceData.shadowTransform = identity * shadowSliceData.shadowTransform;
	}

	public static Vector4 GetShadowBias(ref VisibleLight shadowLight, int shadowLightIndex, ref ShadowData shadowData, Matrix4x4 lightProjectionMatrix, float shadowResolution)
	{
		if (shadowLightIndex < 0 || shadowLightIndex >= shadowData.bias.Count)
		{
			Debug.LogWarning($"{shadowLightIndex} is not a valid light index.");
			return Vector4.zero;
		}
		float num;
		if (shadowLight.lightType == LightType.Directional)
		{
			num = 2f / lightProjectionMatrix.m00;
		}
		else if (shadowLight.lightType == LightType.Spot)
		{
			num = Mathf.Tan(shadowLight.spotAngle * 0.5f * (MathF.PI / 180f)) * shadowLight.range;
		}
		else
		{
			Debug.LogWarning("Only spot and directional shadow casters are supported in universal pipeline");
			num = 0f;
		}
		float num2 = num / shadowResolution;
		float num3 = (0f - shadowData.bias[shadowLightIndex].x) * num2;
		float num4 = (0f - shadowData.bias[shadowLightIndex].y) * num2;
		if (shadowData.supportsSoftShadows)
		{
			num3 *= 2.5f;
			num4 *= 2.5f;
		}
		return new Vector4(num3, num4, 0f, 0f);
	}

	public static void SetupShadowCasterConstantBuffer(CommandBuffer cmd, ref VisibleLight shadowLight, Vector4 shadowBias)
	{
		Vector3 vector = -shadowLight.localToWorldMatrix.GetColumn(2);
		cmd.SetGlobalVector("_ShadowBias", shadowBias);
		cmd.SetGlobalVector("_LightDirection", new Vector4(vector.x, vector.y, vector.z, 0f));
	}

	public static RenderTexture GetTemporaryShadowTexture(int width, int height, int bits)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, bits, m_ShadowmapFormat);
		temporary.filterMode = ((!m_ForceShadowPointSampling) ? FilterMode.Bilinear : FilterMode.Point);
		temporary.wrapMode = TextureWrapMode.Clamp;
		return temporary;
	}

	private static Matrix4x4 GetShadowTransform(Matrix4x4 proj, Matrix4x4 view)
	{
		if (SystemInfo.usesReversedZBuffer)
		{
			proj.m20 = 0f - proj.m20;
			proj.m21 = 0f - proj.m21;
			proj.m22 = 0f - proj.m22;
			proj.m23 = 0f - proj.m23;
		}
		Matrix4x4 matrix4x = proj * view;
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = 0.5f;
		identity.m11 = 0.5f;
		identity.m22 = 0.5f;
		identity.m03 = 0.5f;
		identity.m23 = 0.5f;
		identity.m13 = 0.5f;
		return identity * matrix4x;
	}
}
