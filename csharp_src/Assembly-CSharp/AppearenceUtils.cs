using System.Runtime.CompilerServices;
using RiverGame.Rendering.MaterialPropertyBlockUtilities;
using UnityEngine;

public class AppearenceUtils
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Transform FindAttachmentPoint(Transform charRoot, string boneFullPath)
	{
		Transform transform = charRoot.Find(boneFullPath);
		if (transform == null)
		{
			if (charRoot.TryGetComponent<GPUSkinNodeMapping>(out var component))
			{
				transform = component.FindNodeByOrigPath(boneFullPath);
			}
			if (transform == null)
			{
				Debug.LogError("gpu skin attachment point not found: " + boneFullPath + ", for: " + charRoot.name);
			}
		}
		return transform;
	}

	public static void ApplyRimMPB(Renderer renderer, MaterialPropertyBlock mpb)
	{
		if (renderer.TryGetComponent<GPUSkinnedMeshRenderer>(out var component))
		{
			ApplyRimMPB(component, renderer, mpb);
		}
		else
		{
			ApplyRimMPB(null, renderer, mpb);
		}
	}

	public static void ApplyRimMPB(GPUSkinnedMeshRenderer gsRenderer, Renderer renderer, MaterialPropertyBlock mpb)
	{
		if (gsRenderer == null)
		{
			renderer.SetPropertyBlock(mpb);
			return;
		}
		Material[] sharedMaterials = renderer.sharedMaterials;
		foreach (Material material in sharedMaterials)
		{
			if (!(material == null))
			{
				if (mpb != null)
				{
					material.SetFloat("_USERIM", mpb.GetFloat("_USERIMInstancing"));
					material.SetFloat("_BWC", mpb.GetFloat("_BWCInstancing"));
					material.SetFloat("_RimPow", mpb.GetFloat("_RimPowInstancing"));
					material.SetFloat("_RimRng", mpb.GetFloat("_RimRngInstancing"));
					material.SetColor("_RimCol", mpb.GetColor("_RimColInstancing"));
				}
				else
				{
					material.SetFloat("_USERIM", 0f);
					material.SetFloat("_BWC", 0f);
					material.SetFloat("_RimPow", 0f);
				}
			}
		}
	}

	public static (Material, Material) ProcessAndGetDefaultMaterial(Renderer renderer)
	{
		if (renderer.GetComponent<GPUSkinnedMeshRenderer>() != null)
		{
			return (renderer.sharedMaterial, null);
		}
		Material sharedMaterial = renderer.sharedMaterial;
		Material item = (renderer.sharedMaterial = Object.Instantiate(sharedMaterial));
		return (item, sharedMaterial);
	}

	public static void GrayEffect(Renderer renderer, bool enable)
	{
		if (renderer.TryGetComponent<GPUSkinnedMeshRenderer>(out var component))
		{
			HitWhiteV3(component, renderer, enable, AppearenceStyleRim.Gray);
		}
		else
		{
			HitWhiteV3(null, renderer, enable, AppearenceStyleRim.Gray);
		}
	}

	public static void ModelScaleEffect(Renderer renderer, bool enable)
	{
		if (renderer.TryGetComponent<GPUSkinnedMeshRenderer>(out var component))
		{
			HitWhiteV3(component, renderer, enable, AppearenceStyleRim.ModelScale);
		}
		else
		{
			HitWhiteV3(null, renderer, enable, AppearenceStyleRim.ModelScale);
		}
	}

	public static void HitWhiteV2(Renderer renderer, bool enable, bool red)
	{
		if (renderer.TryGetComponent<GPUSkinnedMeshRenderer>(out var component))
		{
			HitWhiteV3(component, renderer, enable, red);
		}
		else
		{
			HitWhiteV3(null, renderer, enable, red);
		}
	}

	public static void HitWhiteV3(GPUSkinnedMeshRenderer gsRenderer, Renderer renderer, bool enable, bool red)
	{
		if (red)
		{
			HitWhiteV3(gsRenderer, renderer, enable, AppearenceStyleRim.Red);
		}
		else
		{
			HitWhiteV3(gsRenderer, renderer, enable, AppearenceStyleRim.Gold);
		}
	}

	public static void HitWhiteV3(GPUSkinnedMeshRenderer gsRenderer, Renderer renderer, bool enable, MaterialPropertyGroup style)
	{
		if (gsRenderer == null)
		{
			renderer.SetPropertyBlock((enable && style != null) ? style.GetMaterialPropertyBlock(useInstancingName: false) : null);
		}
		else if (!gsRenderer.enableInstancing)
		{
			Material[] materials = renderer.materials;
			int num = materials.Length;
			for (int i = 0; i < num; i++)
			{
				Material material = materials[i];
				if (!(material == null))
				{
					if (enable)
					{
						style.ApplyToMaterial(material);
					}
					else
					{
						material.SetFloat("_USERIM", 0f);
						material.SetFloat("_RimPow", 0f);
						material.SetFloat("_BWC", 0f);
					}
					materials[i] = material;
				}
			}
			renderer.materials = materials;
		}
		else
		{
			MaterialPropertyBlock propertyBlock = ((!enable) ? gsRenderer.materialPropertyBlockController.Remove(style) : gsRenderer.materialPropertyBlockController.Add(style));
			renderer.SetPropertyBlock(propertyBlock);
		}
	}

	public static void ReplaceMaterial(Renderer renderer, bool replace, Material mat)
	{
		if (mat == null)
		{
			return;
		}
		if (replace)
		{
			Material sharedMaterial = renderer.sharedMaterial;
			Material material = mat;
			if (sharedMaterial != mat)
			{
				material = Object.Instantiate(mat);
			}
			if (sharedMaterial.IsKeywordEnabled("_VERTEX_SKINNING_ON"))
			{
				renderer.sharedMaterial = material;
				material.EnableKeyword("_VERTEX_SKINNING_ON");
				material.SetTexture("_AnimTex", sharedMaterial.GetTexture("_AnimTex"));
				material.SetVector("_AnimTex_TexelSize", sharedMaterial.GetVector("_AnimTex_TexelSize"));
				material.enableInstancing = false;
				if (sharedMaterial.HasProperty("_AnimParams"))
				{
					material.SetVector("_AnimParams", sharedMaterial.GetVector("_AnimParams"));
				}
				if (sharedMaterial.HasProperty("_AnimGPUTime"))
				{
					material.SetVector("_AnimGPUTime", sharedMaterial.GetVector("_AnimGPUTime"));
				}
				if (sharedMaterial.HasProperty("_AnimInterruptParams"))
				{
					material.SetVector("_AnimInterruptParams", sharedMaterial.GetVector("_AnimInterruptParams"));
				}
				if (sharedMaterial.HasProperty("_BWC"))
				{
					material.SetFloat("_BWC", sharedMaterial.GetFloat("_BWC"));
				}
			}
			else
			{
				renderer.sharedMaterial = material;
				if (sharedMaterial.HasProperty("_BWC"))
				{
					material.SetFloat("_BWC", sharedMaterial.GetFloat("_BWC"));
				}
			}
			return;
		}
		if (renderer.sharedMaterial.IsKeywordEnabled("_VERTEX_SKINNING_ON"))
		{
			Material sharedMaterial2 = renderer.sharedMaterial;
			mat.EnableKeyword("_VERTEX_SKINNING_ON");
			mat.SetTexture("_AnimTex", sharedMaterial2.GetTexture("_AnimTex"));
			mat.SetVector("_AnimTex_TexelSize", sharedMaterial2.GetVector("_AnimTex_TexelSize"));
			if (sharedMaterial2.HasProperty("_AnimParams"))
			{
				mat.SetVector("_AnimParams", sharedMaterial2.GetVector("_AnimParams"));
			}
			if (sharedMaterial2.HasProperty("_AnimGPUTime"))
			{
				mat.SetVector("_AnimGPUTime", sharedMaterial2.GetVector("_AnimGPUTime"));
			}
			if (sharedMaterial2.HasProperty("_AnimInterruptParams"))
			{
				mat.SetVector("_AnimInterruptParams", sharedMaterial2.GetVector("_AnimInterruptParams"));
			}
			if (sharedMaterial2.HasProperty("_BWC"))
			{
				mat.SetFloat("_BWC", sharedMaterial2.GetFloat("_BWC"));
			}
		}
		else
		{
			Material sharedMaterial3 = renderer.sharedMaterial;
			if (sharedMaterial3.HasProperty("_BWC"))
			{
				mat.SetFloat("_BWC", sharedMaterial3.GetFloat("_BWC"));
			}
		}
		if (renderer.sharedMaterial.IsKeywordEnabled("_CROSSFADE_INTERRUPT_ON"))
		{
			mat.EnableKeyword("_CROSSFADE_INTERRUPT_ON");
		}
		else
		{
			mat.DisableKeyword("_CROSSFADE_INTERRUPT_ON");
		}
		if (renderer.sharedMaterial != mat)
		{
			Object.Destroy(renderer.sharedMaterial);
		}
		renderer.sharedMaterial = mat;
	}
}
