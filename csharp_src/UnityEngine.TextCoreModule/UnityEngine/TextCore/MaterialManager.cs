using System.Collections.Generic;

namespace UnityEngine.TextCore;

internal static class MaterialManager
{
	private static Dictionary<long, Material> s_FallbackMaterials = new Dictionary<long, Material>();

	public static Material GetFallbackMaterial(Material sourceMaterial, Material targetMaterial)
	{
		int instanceID = sourceMaterial.GetInstanceID();
		Texture texture = targetMaterial.GetTexture(ShaderUtilities.ID_MainTex);
		int instanceID2 = texture.GetInstanceID();
		long key = ((long)instanceID << 32) | (uint)instanceID2;
		if (s_FallbackMaterials.TryGetValue(key, out var value))
		{
			return value;
		}
		if (sourceMaterial.HasProperty(ShaderUtilities.ID_GradientScale) && targetMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
		{
			value = new Material(sourceMaterial);
			value.hideFlags = HideFlags.HideAndDontSave;
			value.SetTexture(ShaderUtilities.ID_MainTex, texture);
			value.SetFloat(ShaderUtilities.ID_GradientScale, targetMaterial.GetFloat(ShaderUtilities.ID_GradientScale));
			value.SetFloat(ShaderUtilities.ID_TextureWidth, targetMaterial.GetFloat(ShaderUtilities.ID_TextureWidth));
			value.SetFloat(ShaderUtilities.ID_TextureHeight, targetMaterial.GetFloat(ShaderUtilities.ID_TextureHeight));
			value.SetFloat(ShaderUtilities.ID_WeightNormal, targetMaterial.GetFloat(ShaderUtilities.ID_WeightNormal));
			value.SetFloat(ShaderUtilities.ID_WeightBold, targetMaterial.GetFloat(ShaderUtilities.ID_WeightBold));
		}
		else
		{
			value = new Material(targetMaterial);
		}
		s_FallbackMaterials.Add(key, value);
		return value;
	}
}
