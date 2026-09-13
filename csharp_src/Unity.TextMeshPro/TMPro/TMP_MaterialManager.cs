using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro;

public static class TMP_MaterialManager
{
	private class FallbackMaterial
	{
		public long fallbackID;

		public Material sourceMaterial;

		internal int sourceMaterialCRC;

		public Material fallbackMaterial;

		public int count;
	}

	private class MaskingMaterial
	{
		public Material baseMaterial;

		public Material stencilMaterial;

		public int count;

		public int stencilID;
	}

	private static List<MaskingMaterial> m_materialList;

	private static Dictionary<long, FallbackMaterial> m_fallbackMaterials;

	private static Dictionary<int, long> m_fallbackMaterialLookup;

	private static List<FallbackMaterial> m_fallbackCleanupList;

	private static bool isFallbackListDirty;

	static TMP_MaterialManager()
	{
		m_materialList = new List<MaskingMaterial>();
		m_fallbackMaterials = new Dictionary<long, FallbackMaterial>();
		m_fallbackMaterialLookup = new Dictionary<int, long>();
		m_fallbackCleanupList = new List<FallbackMaterial>();
		Canvas.willRenderCanvases += OnPreRender;
	}

	private static void OnPreRender()
	{
		if (isFallbackListDirty)
		{
			CleanupFallbackMaterials();
			isFallbackListDirty = false;
		}
	}

	public static Material GetStencilMaterial(Material baseMaterial, int stencilID)
	{
		if (!baseMaterial.HasProperty(ShaderUtilities.ID_StencilID))
		{
			Debug.LogWarning("Selected Shader does not support Stencil Masking. Please select the Distance Field or Mobile Distance Field Shader.");
			return baseMaterial;
		}
		int instanceID = baseMaterial.GetInstanceID();
		for (int i = 0; i < m_materialList.Count; i++)
		{
			if (m_materialList[i].baseMaterial.GetInstanceID() == instanceID && m_materialList[i].stencilID == stencilID)
			{
				m_materialList[i].count++;
				return m_materialList[i].stencilMaterial;
			}
		}
		Material material = new Material(baseMaterial);
		material.hideFlags = HideFlags.HideAndDontSave;
		material.shaderKeywords = baseMaterial.shaderKeywords;
		ShaderUtilities.GetShaderPropertyIDs();
		material.SetFloat(ShaderUtilities.ID_StencilID, stencilID);
		material.SetFloat(ShaderUtilities.ID_StencilComp, 4f);
		MaskingMaterial maskingMaterial = new MaskingMaterial();
		maskingMaterial.baseMaterial = baseMaterial;
		maskingMaterial.stencilMaterial = material;
		maskingMaterial.stencilID = stencilID;
		maskingMaterial.count = 1;
		m_materialList.Add(maskingMaterial);
		return material;
	}

	public static void ReleaseStencilMaterial(Material stencilMaterial)
	{
		int instanceID = stencilMaterial.GetInstanceID();
		for (int i = 0; i < m_materialList.Count; i++)
		{
			if (m_materialList[i].stencilMaterial.GetInstanceID() == instanceID)
			{
				if (m_materialList[i].count > 1)
				{
					m_materialList[i].count--;
					break;
				}
				Object.DestroyImmediate(m_materialList[i].stencilMaterial);
				m_materialList.RemoveAt(i);
				stencilMaterial = null;
				break;
			}
		}
	}

	public static Material GetBaseMaterial(Material stencilMaterial)
	{
		int num = m_materialList.FindIndex((MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
		if (num == -1)
		{
			return null;
		}
		return m_materialList[num].baseMaterial;
	}

	public static Material SetStencil(Material material, int stencilID)
	{
		material.SetFloat(ShaderUtilities.ID_StencilID, stencilID);
		if (stencilID == 0)
		{
			material.SetFloat(ShaderUtilities.ID_StencilComp, 8f);
		}
		else
		{
			material.SetFloat(ShaderUtilities.ID_StencilComp, 4f);
		}
		return material;
	}

	public static void AddMaskingMaterial(Material baseMaterial, Material stencilMaterial, int stencilID)
	{
		int num = m_materialList.FindIndex((MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
		if (num == -1)
		{
			MaskingMaterial maskingMaterial = new MaskingMaterial();
			maskingMaterial.baseMaterial = baseMaterial;
			maskingMaterial.stencilMaterial = stencilMaterial;
			maskingMaterial.stencilID = stencilID;
			maskingMaterial.count = 1;
			m_materialList.Add(maskingMaterial);
		}
		else
		{
			stencilMaterial = m_materialList[num].stencilMaterial;
			m_materialList[num].count++;
		}
	}

	public static void RemoveStencilMaterial(Material stencilMaterial)
	{
		int num = m_materialList.FindIndex((MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
		if (num != -1)
		{
			m_materialList.RemoveAt(num);
		}
	}

	public static void ReleaseBaseMaterial(Material baseMaterial)
	{
		int num = m_materialList.FindIndex((MaskingMaterial item) => item.baseMaterial == baseMaterial);
		if (num == -1)
		{
			Debug.Log("No Masking Material exists for " + baseMaterial.name);
		}
		else if (m_materialList[num].count > 1)
		{
			m_materialList[num].count--;
			Debug.Log("Removed (1) reference to " + m_materialList[num].stencilMaterial.name + ". There are " + m_materialList[num].count + " references left.");
		}
		else
		{
			Debug.Log("Removed last reference to " + m_materialList[num].stencilMaterial.name + " with ID " + m_materialList[num].stencilMaterial.GetInstanceID());
			Object.DestroyImmediate(m_materialList[num].stencilMaterial);
			m_materialList.RemoveAt(num);
		}
	}

	public static void ClearMaterials()
	{
		if (m_materialList.Count == 0)
		{
			Debug.Log("Material List has already been cleared.");
			return;
		}
		for (int i = 0; i < m_materialList.Count; i++)
		{
			Object.DestroyImmediate(m_materialList[i].stencilMaterial);
		}
		m_materialList.Clear();
	}

	public static int GetStencilID(GameObject obj)
	{
		int num = 0;
		Transform transform = obj.transform;
		Transform transform2 = FindRootSortOverrideCanvas(transform);
		if (transform == transform2)
		{
			return num;
		}
		Transform parent = transform.parent;
		List<Mask> list = TMP_ListPool<Mask>.Get();
		while (parent != null)
		{
			parent.GetComponents(list);
			for (int i = 0; i < list.Count; i++)
			{
				Mask mask = list[i];
				if (mask != null && mask.MaskEnabled() && mask.graphic.IsActive())
				{
					num++;
					break;
				}
			}
			if (parent == transform2)
			{
				break;
			}
			parent = parent.parent;
		}
		TMP_ListPool<Mask>.Release(list);
		return Mathf.Min((1 << num) - 1, 255);
	}

	public static Material GetMaterialForRendering(MaskableGraphic graphic, Material baseMaterial)
	{
		if (baseMaterial == null)
		{
			return null;
		}
		List<IMaterialModifier> list = TMP_ListPool<IMaterialModifier>.Get();
		graphic.GetComponents(list);
		Material material = baseMaterial;
		for (int i = 0; i < list.Count; i++)
		{
			material = list[i].GetModifiedMaterial(material);
		}
		TMP_ListPool<IMaterialModifier>.Release(list);
		return material;
	}

	private static Transform FindRootSortOverrideCanvas(Transform start)
	{
		List<Canvas> list = TMP_ListPool<Canvas>.Get();
		start.GetComponentsInParent(includeInactive: false, list);
		Canvas canvas = null;
		for (int i = 0; i < list.Count; i++)
		{
			canvas = list[i];
			if (canvas.overrideSorting)
			{
				break;
			}
		}
		TMP_ListPool<Canvas>.Release(list);
		if (!(canvas != null))
		{
			return null;
		}
		return canvas.transform;
	}

	internal static Material GetFallbackMaterial(TMP_FontAsset fontAsset, Material sourceMaterial, int atlasIndex)
	{
		int instanceID = sourceMaterial.GetInstanceID();
		Texture texture = fontAsset.atlasTextures[atlasIndex];
		int instanceID2 = texture.GetInstanceID();
		long num = ((long)instanceID << 32) | (uint)instanceID2;
		if (m_fallbackMaterials.TryGetValue(num, out var value))
		{
			int num2 = sourceMaterial.ComputeCRC();
			if (num2 == value.sourceMaterialCRC)
			{
				return value.fallbackMaterial;
			}
			CopyMaterialPresetProperties(sourceMaterial, value.fallbackMaterial);
			value.sourceMaterialCRC = num2;
			return value.fallbackMaterial;
		}
		Material material = new Material(sourceMaterial);
		material.SetTexture(ShaderUtilities.ID_MainTex, texture);
		material.hideFlags = HideFlags.HideAndDontSave;
		value = new FallbackMaterial();
		value.fallbackID = num;
		value.sourceMaterial = fontAsset.material;
		value.sourceMaterialCRC = sourceMaterial.ComputeCRC();
		value.fallbackMaterial = material;
		value.count = 0;
		m_fallbackMaterials.Add(num, value);
		m_fallbackMaterialLookup.Add(material.GetInstanceID(), num);
		return material;
	}

	public static Material GetFallbackMaterial(Material sourceMaterial, Material targetMaterial)
	{
		int instanceID = sourceMaterial.GetInstanceID();
		Texture texture = targetMaterial.GetTexture(ShaderUtilities.ID_MainTex);
		int instanceID2 = texture.GetInstanceID();
		long num = ((long)instanceID << 32) | (uint)instanceID2;
		if (m_fallbackMaterials.TryGetValue(num, out var value))
		{
			int num2 = sourceMaterial.ComputeCRC();
			if (num2 == value.sourceMaterialCRC)
			{
				return value.fallbackMaterial;
			}
			CopyMaterialPresetProperties(sourceMaterial, value.fallbackMaterial);
			value.sourceMaterialCRC = num2;
			return value.fallbackMaterial;
		}
		Material material;
		if (sourceMaterial.HasProperty(ShaderUtilities.ID_GradientScale) && targetMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
		{
			material = new Material(sourceMaterial);
			material.hideFlags = HideFlags.HideAndDontSave;
			material.SetTexture(ShaderUtilities.ID_MainTex, texture);
			material.SetFloat(ShaderUtilities.ID_GradientScale, targetMaterial.GetFloat(ShaderUtilities.ID_GradientScale));
			material.SetFloat(ShaderUtilities.ID_TextureWidth, targetMaterial.GetFloat(ShaderUtilities.ID_TextureWidth));
			material.SetFloat(ShaderUtilities.ID_TextureHeight, targetMaterial.GetFloat(ShaderUtilities.ID_TextureHeight));
			material.SetFloat(ShaderUtilities.ID_WeightNormal, targetMaterial.GetFloat(ShaderUtilities.ID_WeightNormal));
			material.SetFloat(ShaderUtilities.ID_WeightBold, targetMaterial.GetFloat(ShaderUtilities.ID_WeightBold));
		}
		else
		{
			material = new Material(targetMaterial);
		}
		value = new FallbackMaterial();
		value.fallbackID = num;
		value.sourceMaterial = sourceMaterial;
		value.sourceMaterialCRC = sourceMaterial.ComputeCRC();
		value.fallbackMaterial = material;
		value.count = 0;
		m_fallbackMaterials.Add(num, value);
		m_fallbackMaterialLookup.Add(material.GetInstanceID(), num);
		return material;
	}

	public static void AddFallbackMaterialReference(Material targetMaterial)
	{
		if (!(targetMaterial == null))
		{
			int instanceID = targetMaterial.GetInstanceID();
			if (m_fallbackMaterialLookup.TryGetValue(instanceID, out var value) && m_fallbackMaterials.TryGetValue(value, out var value2))
			{
				value2.count++;
			}
		}
	}

	public static void RemoveFallbackMaterialReference(Material targetMaterial)
	{
		if (targetMaterial == null)
		{
			return;
		}
		int instanceID = targetMaterial.GetInstanceID();
		if (m_fallbackMaterialLookup.TryGetValue(instanceID, out var value) && m_fallbackMaterials.TryGetValue(value, out var value2))
		{
			value2.count--;
			if (value2.count < 1)
			{
				m_fallbackCleanupList.Add(value2);
			}
		}
	}

	public static void CleanupFallbackMaterials()
	{
		if (m_fallbackCleanupList.Count == 0)
		{
			return;
		}
		for (int i = 0; i < m_fallbackCleanupList.Count; i++)
		{
			FallbackMaterial fallbackMaterial = m_fallbackCleanupList[i];
			if (fallbackMaterial.count < 1)
			{
				Material fallbackMaterial2 = fallbackMaterial.fallbackMaterial;
				m_fallbackMaterials.Remove(fallbackMaterial.fallbackID);
				m_fallbackMaterialLookup.Remove(fallbackMaterial2.GetInstanceID());
				Object.DestroyImmediate(fallbackMaterial2);
				fallbackMaterial2 = null;
			}
		}
		m_fallbackCleanupList.Clear();
	}

	public static void ReleaseFallbackMaterial(Material fallbackMaterial)
	{
		if (fallbackMaterial == null)
		{
			return;
		}
		int instanceID = fallbackMaterial.GetInstanceID();
		if (m_fallbackMaterialLookup.TryGetValue(instanceID, out var value) && m_fallbackMaterials.TryGetValue(value, out var value2))
		{
			value2.count--;
			if (value2.count < 1)
			{
				m_fallbackCleanupList.Add(value2);
			}
		}
		isFallbackListDirty = true;
	}

	public static void CopyMaterialPresetProperties(Material source, Material destination)
	{
		if (source.HasProperty(ShaderUtilities.ID_GradientScale) && destination.HasProperty(ShaderUtilities.ID_GradientScale))
		{
			Texture texture = destination.GetTexture(ShaderUtilities.ID_MainTex);
			float value = destination.GetFloat(ShaderUtilities.ID_GradientScale);
			float value2 = destination.GetFloat(ShaderUtilities.ID_TextureWidth);
			float value3 = destination.GetFloat(ShaderUtilities.ID_TextureHeight);
			float value4 = destination.GetFloat(ShaderUtilities.ID_WeightNormal);
			float value5 = destination.GetFloat(ShaderUtilities.ID_WeightBold);
			destination.CopyPropertiesFromMaterial(source);
			destination.shaderKeywords = source.shaderKeywords;
			destination.SetTexture(ShaderUtilities.ID_MainTex, texture);
			destination.SetFloat(ShaderUtilities.ID_GradientScale, value);
			destination.SetFloat(ShaderUtilities.ID_TextureWidth, value2);
			destination.SetFloat(ShaderUtilities.ID_TextureHeight, value3);
			destination.SetFloat(ShaderUtilities.ID_WeightNormal, value4);
			destination.SetFloat(ShaderUtilities.ID_WeightBold, value5);
		}
	}
}
